using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Ionic.Zlib;

namespace SodikmLauncher;

internal class GameService
{
	public static GameService Instance = new GameService();

	private PlaceMetadata Metadata = new PlaceMetadata();

	private List<Tuple<long, string>> Players = new List<Tuple<long, string>>();

	private List<Tuple<long, long>> PlayerBadges = new List<Tuple<long, long>>();

	private Dictionary<long, byte[]> Blobs = new Dictionary<long, byte[]>();

	private static List<string> BlacklistedExtension = new List<string> { ".gz", ".bz2" };

	private static long MaxBlobSize = 280000L;

	public GameService(string? metadataPath = null, Dictionary<long, byte[]>? blobData = null)
	{
		if (metadataPath != null)
		{
			ParseMetadata(GetMetadataPath(metadataPath));
		}
		if (blobData != null)
		{
			Blobs = blobData;
		}
	}

	private void ParseMetadata(string metadataPath)
	{
		if (metadataPath == null || !File.Exists(metadataPath))
		{
			return;
		}
		try
		{
			Metadata = JsonSerializer.Deserialize<PlaceMetadata>(File.ReadAllText(metadataPath)) ?? throw new Exception("JsonSerializer.Deserialize failed to do its job :rolling eyes:");
		}
		catch (Exception)
		{
		}
	}

	public static string GetMetadataPath(string path)
	{
		string extension = Path.GetExtension(path);
		string text;
		if (!BlacklistedExtension.Contains(extension))
		{
			text = path;
		}
		else
		{
			int length = extension.Length;
			text = path.Substring(0, path.Length - length);
		}
		return text + ".meta.txt";
	}

	public string AwardBadge(long badgeId, long userId)
	{
		lock (PlayerBadges)
		{
			BadgeData badgeData = Metadata.badges.ToList().Find((BadgeData x) => x.id == badgeId);
			if (badgeData == null || HasBadge(badgeId, userId))
			{
				return "0";
			}
			PlayerBadges.Add(new Tuple<long, long>(badgeId, userId));
			return $"{GetPlayerName(userId)} won {Metadata.creator}'s \"{badgeData.name}\" award!";
		}
	}

	public bool HasBadge(long badgeId, long userId)
	{
		return PlayerBadges.Any((Tuple<long, long> x) => x.Item1 == badgeId && x.Item2 == userId);
	}

	public void SaveBlob(long userId, byte[] blob)
	{
		byte[] array = GZipStream.CompressBuffer(blob);
		if (array.LongLength < MaxBlobSize)
		{
			Blobs[userId] = array;
		}
	}

	public byte[] LoadBlob(long userId)
	{
		if (!Blobs.ContainsKey(userId))
		{
			return Encoding.UTF8.GetBytes("<Table></Table>");
		}
		return GZipStream.UncompressBuffer(Blobs[userId]);
	}

	public void WipeBlobs()
	{
		Blobs.Clear();
	}

	public Dictionary<long, byte[]> GetBlobData()
	{
		return Blobs;
	}

	public byte[] ExportSOBL()
	{
		using MemoryStream memoryStream = new MemoryStream();
		memoryStream.Write(BitConverter.GetBytes(1018047273));
		memoryStream.Write(BitConverter.GetBytes((short)1));
		memoryStream.Write(BitConverter.GetBytes(Blobs.Count));
		foreach (KeyValuePair<long, byte[]> blob in Blobs)
		{
			memoryStream.Write(BitConverter.GetBytes(blob.Key));
			memoryStream.Write(BitConverter.GetBytes(blob.Value.Length));
			memoryStream.Write(blob.Value);
		}
		return memoryStream.ToArray();
	}

	public bool ImportSOBL(byte[] sobl)
	{
		using MemoryStream memoryStream = new MemoryStream(sobl);
		byte[] array = new byte[4];
		memoryStream.Read(array);
		if (BitConverter.ToInt32(array) != 1018047273)
		{
			return false;
		}
		byte[] array2 = new byte[2];
		memoryStream.Read(array2);
		if (BitConverter.ToInt16(array2) != 1)
		{
			return false;
		}
		byte[] array3 = new byte[4];
		memoryStream.Read(array3);
		int num = BitConverter.ToInt32(array3);
		for (int i = 1; i <= num; i++)
		{
			byte[] array4 = new byte[8];
			memoryStream.Read(array4);
			long key = BitConverter.ToInt64(array4);
			byte[] array5 = new byte[4];
			memoryStream.Read(array5);
			byte[] array6 = new byte[BitConverter.ToInt32(array5)];
			memoryStream.Read(array6);
			Blobs[key] = array6;
		}
		return true;
	}

	public void RegisterPlayer(long userId, string name)
	{
		Players.Add(new Tuple<long, string>(userId, name));
	}

	public void UnregisterPlayer(long userId)
	{
		Tuple<long, string> tuple = Players.Find((Tuple<long, string> x) => x.Item1 == userId);
		if (tuple != null)
		{
			Players.Remove(tuple);
		}
	}

	private string GetPlayerName(long userId)
	{
		Tuple<long, string> tuple = Players.Find((Tuple<long, string> x) => x.Item1 == userId);
		if (tuple != null)
		{
			return tuple.Item2;
		}
		return "MISSINGNO";
	}

	public void Dispose()
	{
	}
}
