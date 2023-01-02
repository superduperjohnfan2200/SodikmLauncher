using System.IO;
using Ionic.BZip2;

namespace SodikmLauncher;

internal class BZip2
{
	public static byte[] Decompress(byte[] bytes)
	{
		using MemoryStream input = new MemoryStream(bytes);
		using BZip2InputStream bZip2InputStream = new BZip2InputStream(input);
		using MemoryStream memoryStream = new MemoryStream();
		bZip2InputStream.CopyTo(memoryStream);
		return memoryStream.ToArray();
	}
}
