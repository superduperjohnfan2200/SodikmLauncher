using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SodikmLauncher;

internal class Crypto
{
	private static string PemPrivateKey = File.ReadAllText("./data/web/PrivateKey.pem");

	public static string ScriptSign(string script, long assetId = 0L)
	{
		string format = (ClientSettings.Instance.NewSignatureFormat ? "--rbxsig%{0}%" : "%{0}%");
		script = ((assetId == 0L) ? ("\r\n" + script) : (ClientSettings.Instance.NewSignatureFormat ? $"\r\n--rbxassetid%{assetId}%\r\n{script}" : $"%{assetId}%\r\n{script}"));
		using RSA rSA = RSA.Create();
		rSA.ImportFromPem(PemPrivateKey);
		byte[] inArray = rSA.SignData(Encoding.Default.GetBytes(script), HashAlgorithmName.SHA1, RSASignaturePadding.Pkcs1);
		return string.Format(format, Convert.ToBase64String(inArray)) + script;
	}
}
