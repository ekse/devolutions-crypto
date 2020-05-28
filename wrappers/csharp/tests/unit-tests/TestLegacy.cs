#pragma warning disable SA1600 // Elements should be documented

namespace Devolutions.Crypto.Tests
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    using Devolutions.Cryptography;

#if XAMARIN_MAC_FULL || XAMARIN_MAC_MODERN || XAMARIN_IOS || XAMARIN_ANDROID
    using NUnit.Framework;
    using TestClassAttribute = NUnit.Framework.TestFixtureAttribute;
    using TestMethodAttribute = NUnit.Framework.TestCaseAttribute;
#else
    using Microsoft.VisualStudio.TestTools.UnitTesting;
#endif

    [TestClass]
    public class TestLegacy
    {
        [TestMethod]
        public void TestLegacyDecryptor()
        {
            TestDecryptor legacy = new TestDecryptor(Utils.DecodeFromBase64("TErOq+UuM6AjF8SAUVmOWg=="));

            string base64Data = "TuOx/rB+iwGrxpdbhaRyc3kphNIb4feEWLzHZZF0+21OE7hnTcLf1JiuUxLoRR1+";
            byte[] data = Utils.DecodeFromBase64(base64Data);
            byte[] key = Utils.DecodeFromBase64("5toYYi+R4MH/ZV1W0dCQ2C8xRYtgwFrmIR2qfEQRP6k=");

            Assert.IsTrue(CompareArrays(Encoding.UTF8.GetBytes("A test Ciphertext!"), Managed.Decrypt(data, key, legacy)));
            Assert.IsTrue(CompareArrays(Encoding.UTF8.GetBytes("A test Ciphertext!"), Managed.DecryptWithKey(data, key, legacy)));
            Assert.IsTrue(CompareArrays(Encoding.UTF8.GetBytes("A test Ciphertext!"), Managed.DecryptWithKey(base64Data, key, legacy)));

            Assert.AreEqual("A test Ciphertext!", Managed.DecryptWithKeyAsUtf8String(data, key, legacy));
            Assert.AreEqual("A test Ciphertext!", Managed.DecryptWithKeyAsUtf8String(base64Data, key, legacy));
        }

        [TestMethod]
        public void TestDecryptionWithLegacyDecryptor()
        {
            TestDecryptor legacy = new TestDecryptor(Utils.DecodeFromBase64("TErOq+UuM6AjF8SAUVmOWg=="));

            string base64Data = "DQwCAAEAAgDiTIrZApcji3I3pDfBADJ6sMa+iXfpdxRwIf7RHot0XNqOCLv5BlMi5RzezdHl+5NYBwvm//SDomwk";
            byte[] data = Utils.DecodeFromBase64(base64Data);
            byte[] key = Utils.DecodeFromBase64("XCF4aJBny9LHFmUBt8zha5O2oOVttykWKrmUl4ujlVg=");

            Assert.IsTrue(CompareArrays(Encoding.UTF8.GetBytes("A test Ciphertext!"), Managed.Decrypt(data, key, legacy)));
            Assert.IsTrue(CompareArrays(Encoding.UTF8.GetBytes("A test Ciphertext!"), Managed.DecryptWithKey(data, key, legacy)));
            Assert.IsTrue(CompareArrays(Encoding.UTF8.GetBytes("A test Ciphertext!"), Managed.DecryptWithKey(base64Data, key, legacy)));

            Assert.AreEqual("A test Ciphertext!", Managed.DecryptWithKeyAsUtf8String(data, key, legacy));
            Assert.AreEqual("A test Ciphertext!", Managed.DecryptWithKeyAsUtf8String(base64Data, key, legacy));
        }

        private static bool CompareArrays(byte[] a1, byte[] a2)
        {
            if (a1 == null || a2 == null || a1.Length != a2.Length)
            {
                return false;
            }

            for (int i = 0; i < a1.Length; i++)
            {
                if (a1[i] != a2[i])
                {
                    return false;
                }
            }

            return true;
        }

        private class TestDecryptor : ILegacyDecryptor
        {
            public TestDecryptor(byte[] salt)
            {
                this.Salt = salt;
            }

            // Not used
            public byte[] Salt { get; private set; }

            public byte[] Decrypt(byte[] data, byte[] key)
            {
                byte[] iv = new byte[16];
                byte[] ciphertext = new byte[data.Length - 16];

                Array.Copy(data, 0, iv, 0, iv.Length);
                Array.Copy(data, iv.Length, ciphertext, 0, ciphertext.Length);

#pragma warning disable SCS0011 // CBC mode is weak
                using (AesManaged aes = new AesManaged())
                {
                    aes.Mode = CipherMode.CBC;
                    aes.Padding = PaddingMode.PKCS7;

                    aes.Key = key;
                    aes.IV = iv;

                    ICryptoTransform encryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    return encryptor.TransformFinalBlock(ciphertext, 0, ciphertext.Length);
                }
#pragma warning restore SCS0011 // CBC mode is weak
            }
        }
    }
}
#pragma warning restore SA1600 // Elements should be documented