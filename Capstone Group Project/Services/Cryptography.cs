using System;
using System.Security.Cryptography;
using System.Text;

namespace Capstone_Group_Project.Services
{
    static class AsymmetricEncryption
    {
        // We use this class to ENCRYPT a plaintext with a PUBLIC key, and to DECRYPT the resulting ciphertext with a SEPARATE PRIVATE key.
        // Mainly, we use this class to:
        // 1. Encrypt/decrypt a conversation's private SYMMETRIC key (from the class below) with a conversation participant's
        // account private ASYMMETRIC key (from this class).

        // Methods like ExportRSAPublicKey(), ExportRSAPrivateKey(), etc. don't work since the are not part of Xamarin Forms' latest version,
        // which we are currently on. So, we must use the RSA class for the latest version of Xamarin.Android/Xamarin.iOS that is supported:
        // https://docs.microsoft.com/en-us/dotnet/api/system.security.cryptography.rsa?view=xamarinandroid-7.1
        // After trial and error, I've found that exporting public and private keys to XML Strings is the the most convenient route we should take.


        public static String CreateNewPublicAndPrivateKeyAndReturnAsXmlString()
        {
            // We can adjust the output size of the ciphertext via changing the key size from the default 128 bytes, if needed:
            RSA rsa = RSA.Create();
            String publicAndPrivateKeyXmlString = rsa.ToXmlString(true);
            return publicAndPrivateKeyXmlString;
        }


        public static String ExtractPublicKeyAndReturnAsXmlString(String publicAndPrivateKeyXmlString)
        {
            RSA rsa = RSA.Create();
            rsa.FromXmlString(publicAndPrivateKeyXmlString);
            String publicKeyXmlString = rsa.ToXmlString(false);
            return publicKeyXmlString;
        }


        public static String EncryptPlaintextStringToCiphertextBase64String(String plaintextString, String publicKeyXmlString)
        {
            // We most likely will need to change the encoding method to Unicode to support emojis in the future:
            byte[] plaintextByteArray = Encoding.ASCII.GetBytes(plaintextString);
            RSA rsa = RSA.Create();
            rsa.FromXmlString(publicKeyXmlString);
            // EncryptValue(plaintextByteArray) does not seem to work properly due to padding issues,
            // whereas Encrypt(plaintextByteArray) with the Pkcs1 padding does the trick:
            byte[] ciphertextByteArray = rsa.Encrypt(plaintextByteArray, RSAEncryptionPadding.Pkcs1);
            // For the best security practices, we immediately zero out all of the data in the RSA instance, and then destroy it:
            rsa.Clear();
            rsa.Dispose();
            String ciphertextBase64String = Convert.ToBase64String(ciphertextByteArray);
            return ciphertextBase64String;
        }


        public static String DecryptCiphertextBase64StringToPlaintextString(String ciphertextBase64String, String privateKeyXmlString)
        {
            byte[] ciphertextByteArray = Convert.FromBase64String(ciphertextBase64String);
            RSA rsa = RSA.Create();
            rsa.FromXmlString(privateKeyXmlString);
            // DecryptValue(ciphertextByteArray) does not seem to work properly due to padding issues,
            // whereas Decrypt(ciphertextByteArray) with the Pkcs1 padding does the trick:
            byte[] plaintextByteArray = rsa.Decrypt(ciphertextByteArray, RSAEncryptionPadding.Pkcs1);
            // For the best security practices, we immediately zero out all of the data in the RSA instance, and then destroy it:
            rsa.Clear();
            rsa.Dispose();
            String plaintextString = Encoding.ASCII.GetString(plaintextByteArray);
            return plaintextString;
        }


        private static void AsymmetricEncryptionAndDecryptionTest(String justThePublicKey, String publicAndPrivateKey)
        {
            // Encryption test:
            String ciphertext = AsymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String("THIS is AN encryption TEST!", justThePublicKey);
            // Decryption test:
            String plaintext = AsymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(ciphertext, publicAndPrivateKey);
            // Place a breakpoint on the below line to inspect the values of the above two variables:
            return;
        }
    }


    static class SymmetricEncryption
    {
        // We use this class to ENCRYPT a plaintext with a PRIVATE key, and to DECRYPT the resulting ciphertext with the SAME PRIVATE key.
        // Mainly, we use this class to:
        // 1. Encrypt/decrypt a user account's private ASYMMETRIC key (from the class above) with the user's plaintext password.
        // 2. Encrypt/decrypt all messages in a given conversation with that conversation's private SYMMETRIC key (from this class).

        // This IV will eventually be hard coded in so that it's the exact same across all instances of this mobile application:
        private static byte[] IV = Aes.Create().IV;
        // Technically it is more secure to store the IV along with the symmetric private key itself, but at least for Version 1.0 we will omit this step.


        public static String EncryptPlaintextStringToCiphertextBase64String(String plaintextString, String symmetricPrivateKeyString)
        {
            Aes aes = Aes.Create();
            // AES uses a 128 bit encryption key by default,
            // so it's convenient to just convert the symmetricPrivateKeyString to its unique 128 bit hash code first:
            aes.Key = Hashing.ConvertPrivateKeyStringIntoMd5HashCodeByteArray(symmetricPrivateKeyString);
            aes.IV = IV;
            ICryptoTransform encryptor = aes.CreateEncryptor();
            // For the best security practices, we immediately zero out all of the data in the AES instance, and then destroy it:
            aes.Clear();
            aes.Dispose();
            // We most likely will need to change the encoding method to Unicode to support emojis in the future:
            byte[] plaintextByteArray = Encoding.ASCII.GetBytes(plaintextString);
            byte[] ciphertextByteArray = encryptor.TransformFinalBlock(plaintextByteArray, 0, plaintextByteArray.Length);
            // Like with the AES instance above, for the best security practices we immediately destroy the encryptor instance:
            encryptor.Dispose();
            String ciphertextBase64String = Convert.ToBase64String(ciphertextByteArray);
            return ciphertextBase64String;
        }


        public static String DecryptCiphertextBase64StringToPlaintextString(String ciphertextBase64String, String symmetricPrivateKeyString)
        {
            Aes aes = Aes.Create();
            // AES uses a 128 bit encryption key by default, so it's convenient to just convert the symmetricPrivateKeyString to a 128 bit hash first:
            aes.Key = Hashing.ConvertPrivateKeyStringIntoMd5HashCodeByteArray(symmetricPrivateKeyString);
            aes.IV = IV;
            ICryptoTransform decryptor = aes.CreateDecryptor();
            // For the best security practices, we immediately zero out all of the data in the AES instance, and then destroy it:
            aes.Clear();
            aes.Dispose();
            // We most likely will need to change the encoding method to Unicode to support emojis in the future:
            byte[] ciphertextByteArray = Convert.FromBase64String(ciphertextBase64String);
            byte[] plaintextByteArray = decryptor.TransformFinalBlock(ciphertextByteArray, 0, ciphertextByteArray.Length);
            // Like with the AES instance above, for the best security practices we immediately destroy the decryptor instance:
            decryptor.Dispose();
            String plaintextString = Encoding.ASCII.GetString(plaintextByteArray);
            return plaintextString;
        }


        public static String GenerateNewRandomAesKeyAndReturnAsBase64String()
        {
            // AES uses a 128 bit encryption key by default, so we will return a 16 byte, or 16 character String as the new key:
            Aes aes = Aes.Create();
            String newAesKeyAsBase64String = Convert.ToBase64String(aes.Key);
            // For the best security practices, we immediately zero out all of the data in the AES instance, and then destroy it:
            aes.Clear();
            aes.Dispose();
            return newAesKeyAsBase64String;
        }


        private static void SymmetricEncryptionAndDecryptionTest(String enteredPassword)
        {
            // Encryption test:
            String ciphertext = SymmetricEncryption.EncryptPlaintextStringToCiphertextBase64String("THIS is AN encryption TEST!", enteredPassword);
            // Decryption test:
            String plaintext = SymmetricEncryption.DecryptCiphertextBase64StringToPlaintextString(ciphertext, enteredPassword);
            // Place a breakpoint on the below line to inspect the values of the above two variables:
            return;
        }
    }


    public static class Hashing
    {
        public static String ConvertPasswordStringIntoSha256HashCodeBase64String(String passwordString)
        {
            byte[] passwordByteArray = Encoding.ASCII.GetBytes(passwordString);
            SHA256 sha256 = SHA256.Create();
            byte[] passwordHashByteArray = sha256.ComputeHash(passwordByteArray);
            // For the best security practices, we immediately zero out all of the data in the SHA256 instance, and then destroy it:
            sha256.Clear();
            sha256.Dispose();
            String passwordHashBase64String = Convert.ToBase64String(passwordHashByteArray);
            return passwordHashBase64String;
        }


        public static byte[] ConvertPrivateKeyStringIntoMd5HashCodeByteArray(String symmetricPrivateKeyString)
        {
            byte[] symmetricPrivateKeyByteArray = Encoding.ASCII.GetBytes(symmetricPrivateKeyString);
            MD5 md5 = MD5.Create();
            byte[] symmetricPrivateKeyHashByteArray = md5.ComputeHash(symmetricPrivateKeyByteArray);
            // For the best security practices, we immediately zero out all of the data in the MD5 instance, and then destroy it:
            md5.Clear();
            md5.Dispose();
            return symmetricPrivateKeyHashByteArray;
        }
    }
}
