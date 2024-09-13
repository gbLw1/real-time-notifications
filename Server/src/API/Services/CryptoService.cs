using System.Text;
using System.Security.Cryptography;
using Microsoft.Extensions.Options;

using RTN.API.Shared;

namespace RTN.API.Services;

public class CryptoService {
    private readonly byte[] _key;
    private readonly byte[] _iv;

    public CryptoService(IOptions<AppSettings> appSettings) {
        var encryptionSettings = appSettings.Value.EncryptionSettings;

        // Verifica se as chaves foram fornecidas corretamente
        if (encryptionSettings == null
            || string.IsNullOrWhiteSpace(encryptionSettings.Key)
            || string.IsNullOrWhiteSpace(encryptionSettings.IV)) {
            throw new ArgumentException("Encryption Key and IV must be provided. Check your appsettings.json.");
        }

        // Garantir que a chave e o IV tenham 16 bytes
        if (encryptionSettings.Key.Length != 16 || encryptionSettings.IV.Length != 16) {
            throw new ArgumentException("The key and IV must be 16 bytes long.");
        }

        _key = Encoding.UTF8.GetBytes(encryptionSettings.Key);
        _iv = Encoding.UTF8.GetBytes(encryptionSettings.IV);
    }

    public string Encrypt(string plainText) {
        using (Aes aesAlg = Aes.Create()) {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

            using (var msEncrypt = new MemoryStream()) {
                using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write)) {
                    using (var swEncrypt = new StreamWriter(csEncrypt)) {
                        swEncrypt.Write(plainText);
                    }

                    return Convert.ToBase64String(msEncrypt.ToArray());
                }
            }
        }
    }

    public string Decrypt(string cipherText) {
        using (Aes aesAlg = Aes.Create()) {
            aesAlg.Key = _key;
            aesAlg.IV = _iv;

            ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

            using (var msDecrypt = new MemoryStream(Convert.FromBase64String(cipherText))) {
                using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                    using (var srDecrypt = new StreamReader(csDecrypt)) {
                        return srDecrypt.ReadToEnd();
                    }
                }
            }
        }
    }
}
