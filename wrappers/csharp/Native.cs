namespace Devolutions.Cryptography
{
    using System;
    using System.Runtime.InteropServices;

#if !DEBUG
    using System.Reflection;
#endif

    public static partial class Native
    {
#if RDM
        private const string LibName64 = "x64/DevolutionsCrypto";
        private const string LibName86 = "x86/DevolutionsCrypto";
#endif

#if !ANDROID && !IOS && !MAC_MODERN && !RDM
        private const string LibName64 = "DevolutionsCrypto-x64";

        private const string LibName86 = "DevolutionsCrypto-x86";
#endif

#if !DEBUG
        static Native()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            string managedVersion = assembly.GetName().Version.ToString();
            string nativeVersion = Utils.Version() + ".0";

            if (managedVersion != nativeVersion)
            {
                throw new DevolutionsCryptoException(ManagedError.IncompatibleVersion, "Non-matching versions - Managed: " + managedVersion + " Native: " + nativeVersion);
            }
    }
#endif
        [Obsolete("This method has been deprecated. Use Managed.Decrypt instead.")]
        public static byte[] Decrypt(byte[] data, byte[] key)
        {
            return Managed.Decrypt(data, key);
        }

        [Obsolete("This method has been deprecated. Use Managed.DerivePassword instead.")]
        public static byte[] DerivePassword(string password, string salt, uint iterations = 10000)
        {
            return Managed.DerivePassword(password, salt, iterations);
        }

        [Obsolete("This method has been deprecated. Use Managed.DeriveKey instead.")]
        public static byte[] DeriveKey(byte[] key, byte[] salt, uint iterations = 10000, uint length = 32)
        {
            return Managed.DeriveKey(key, salt, iterations, length);
        }

        [Obsolete("This method has been deprecated. Use Managed.Encrypt instead.")]
        public static byte[] Encrypt(byte[] data, byte[] key, uint version = 0)
        {
            return Managed.Encrypt(data, key, (CipherVersion)version);
        }

        [Obsolete("This method has been deprecated. Use Managed.GenerateKey instead.")]
        public static byte[] GenerateKey(uint keySize)
        {
            return Managed.GenerateKey(keySize);
        }

        [Obsolete("This method has been deprecated. Use Managed.GenerateKeyPair instead.")]
        public static KeyPair GenerateKeyPair()
        {
            return Managed.GenerateKeyPair();
        }

        [Obsolete("This method has been deprecated. Use Managed.HashPassword instead.")]
        public static byte[] HashPassword(byte[] password, uint iterations = 10000)
        {
            return Managed.HashPassword(password, iterations);
        }

        [Obsolete("This method has been deprecated. Use Managed.HashPassword instead.")]
        public static bool VerifyPassword(byte[] password, byte[] hash)
        {
            return Managed.VerifyPassword(password, hash);
        }

#if !ANDROID && !IOS && !MAC_MODERN
        internal static long DecryptNative(byte[] data, UIntPtr dataLength, byte[] key, UIntPtr keyLength, byte[] result, UIntPtr resultLength)
        {
            if (Environment.Is64BitProcess)
            {
                return DecryptNative64(data, dataLength, key, keyLength, result, resultLength);
            }

            return DecryptNative86(data, dataLength, key, keyLength, result, resultLength);
        }

        internal static long DecryptAsymmetricNative(byte[] data, UIntPtr dataLength, byte[] privateKey, UIntPtr privateKeyLength, byte[] result, UIntPtr resultLength)
        {
            if (Environment.Is64BitProcess)
            {
                return DecryptAsymmetricNative64(data, dataLength, privateKey, privateKeyLength, result, resultLength);
            }

            return DecryptAsymmetricNative86(data, dataLength, privateKey, privateKeyLength, result, resultLength);
        }

        internal static long DeriveKeyNative(byte[] key, UIntPtr keyLength, byte[] salt, UIntPtr saltLength, UIntPtr iterations, byte[] result, UIntPtr resultLength)
        {
            if (Environment.Is64BitProcess)
            {
                return DeriveKeyNative64(key, keyLength, salt, saltLength, iterations, result, resultLength);
            }

            return DeriveKeyNative86(key, keyLength, salt, saltLength, iterations, result, resultLength);
        }

        internal static long DeriveKeyPairNative(
            byte[] password,
            UIntPtr passwordLength,
            byte[] parameters,
            UIntPtr parametersLength,
            byte[] privateKey,
            UIntPtr privateKeyLength,
            byte[] publicKey,
            UIntPtr publicKeyLength)
        {
            if (Environment.Is64BitProcess)
            {
                return DeriveKeyPairNative64(password, passwordLength, parameters, parametersLength, privateKey, privateKeyLength, publicKey, publicKeyLength);
            }

            return DeriveKeyPairNative86(password, passwordLength, parameters, parametersLength, privateKey, privateKeyLength, publicKey, publicKeyLength);
        }

        internal static long DeriveKeyPairSizeNative()
        {
            if (Environment.Is64BitProcess)
            {
                return DeriveKeyPairSizeNative64();
            }

            return DeriveKeyPairSizeNative86();
        }

        internal static long GetDefaultArgon2ParametersNative(byte[] argon2Parameters, UIntPtr argon2ParametersLength)
        {
            if (Environment.Is64BitProcess)
            {
                return GetDefaultArgon2ParametersNative64(argon2Parameters, argon2ParametersLength);
            }

            return GetDefaultArgon2ParametersNative86(argon2Parameters, argon2ParametersLength);
        }

        internal static long GetDefaultArgon2ParametersSizeNative()
        {
            if (Environment.Is64BitProcess)
            {
                return GetDefaultArgon2ParametersSizeNative64();
            }

            return GetDefaultArgon2ParametersSizeNative86();
        }

        internal static long EncryptNative(byte[] data, UIntPtr dataLength, byte[] key, UIntPtr keyLength, byte[] result, UIntPtr resultLength, ushort version)
        {
            if (Environment.Is64BitProcess)
            {
                return EncryptNative64(data, dataLength, key, keyLength, result, resultLength, version);
            }

            return EncryptNative86(data, dataLength, key, keyLength, result, resultLength, version);
        }

        internal static long EncryptAsymmetricNative(
            byte[] data,
            UIntPtr dataLength,
            byte[] publicKey,
            UIntPtr publicKeyLength,
            byte[] result,
            UIntPtr resultLength,
            ushort version)
        {
            if (Environment.Is64BitProcess)
            {
                return EncryptAsymmetricNative64(data, dataLength, publicKey, publicKeyLength, result, resultLength, version);
            }

            return EncryptAsymmetricNative86(data, dataLength, publicKey, publicKeyLength, result, resultLength, version);
        }

        internal static long EncryptAsymmetricSizeNative(UIntPtr dataLength, ushort version)
        {
            if (Environment.Is64BitProcess)
            {
                return EncryptAsymmetricSizeNative64(dataLength, version);
            }

            return EncryptAsymmetricSizeNative86(dataLength, version);
        }

        internal static long EncryptSizeNative(UIntPtr dataLength, ushort version)
        {
            if (Environment.Is64BitProcess)
            {
                return EncryptSizeNative64(dataLength, version);
            }

            return EncryptSizeNative86(dataLength, version);
        }

        internal static long GenerateKeyPairNative(byte[] privateKey, UIntPtr privateKeySize, byte[] publicKey, UIntPtr publicKeySize)
        {
            if (Environment.Is64BitProcess)
            {
                return GenerateKeyPairNative64(privateKey, privateKeySize, publicKey, publicKeySize);
            }

            return GenerateKeyPairNative86(privateKey, privateKeySize, publicKey, publicKeySize);
        }

        internal static long GenerateKeyPairSizeNative()
        {
            if (Environment.Is64BitProcess)
            {
                return GenerateKeyPairSizeNative64();
            }

            return GenerateKeyPairSizeNative86();
        }

        internal static long GenerateKeyNative(byte[] key, UIntPtr keyLength)
        {
            if (Environment.Is64BitProcess)
            {
                return GenerateKeyNative64(key, keyLength);
            }

            return GenerateKeyNative86(key, keyLength);
        }

        internal static long HashPasswordLengthNative()
        {
            if (Environment.Is64BitProcess)
            {
                return HashPasswordLengthNative64();
            }

            return HashPasswordLengthNative86();
        }

        internal static long HashPasswordNative(byte[] password, UIntPtr passwordLength, uint iterations, byte[] result, UIntPtr resultLength)
        {
            if (Environment.Is64BitProcess)
            {
                return HashPasswordNative64(password, passwordLength, iterations, result, resultLength);
            }

            return HashPasswordNative86(password, passwordLength, iterations, result, resultLength);
        }

        internal static uint KeySizeNative()
        {
            if (Environment.Is64BitProcess)
            {
                return KeySizeNative64();
            }

            return KeySizeNative86();
        }

        internal static long MixKeyExchangeNative(byte[] privateKey, UIntPtr privateKeySize, byte[] publicKey, UIntPtr publicKeySize, byte[] shared, UIntPtr sharedSize)
        {
            if (Environment.Is64BitProcess)
            {
                return MixKeyExchangeNative64(privateKey, privateKeySize, publicKey, publicKeySize, shared, sharedSize);
            }

            return MixKeyExchangeNative86(privateKey, privateKeySize, publicKey, publicKeySize, shared, sharedSize);
        }

        internal static long MixKeyExchangeSizeNative()
        {
            if (Environment.Is64BitProcess)
            {
                return MixKeyExchangeSizeNative64();
            }

            return MixKeyExchangeSizeNative86();
        }

        internal static long VerifyPasswordNative(byte[] password, UIntPtr passwordLength, byte[] hash, UIntPtr hashLength)
        {
            if (Environment.Is64BitProcess)
            {
                return VerifyPasswordNative64(password, passwordLength, hash, hashLength);
            }

            return VerifyPasswordNative86(password, passwordLength, hash, hashLength);
        }

        internal static long DecodeNative(string input, UIntPtr input_length, byte[] output, UIntPtr output_length)
        {
            if (Environment.Is64BitProcess)
            {
                return Decode64(input, input_length, output, output_length);
            }

            return Decode86(input, input_length, output, output_length);
        }

        internal static long EncodeNative(byte[] input, UIntPtr input_length, byte[] output, UIntPtr output_length)
        {
            if (Environment.Is64BitProcess)
            {
                return Encode64(input, input_length, output, output_length);
            }

            return Encode86(input, input_length, output, output_length);
        }

        internal static long VersionNative(byte[] output, UIntPtr output_length)
        {
            if (Environment.Is64BitProcess)
            {
                return Version64(output, output_length);
            }

            return Version86(output, output_length);
        }

        internal static long VersionSizeNative()
        {
            if (Environment.Is64BitProcess)
            {
                return VersionSize64();
            }

            return VersionSize86();
        }

        [DllImport(LibName86, EntryPoint = "Decode", CallingConvention = CallingConvention.Cdecl)]
        private static extern long Decode86(string input, UIntPtr input_length, byte[] output, UIntPtr output_length);

        [DllImport(LibName64, EntryPoint = "Decode", CallingConvention = CallingConvention.Cdecl)]
        private static extern long Decode64(string input, UIntPtr input_length, byte[] output, UIntPtr output_length);

        [DllImport(LibName64, EntryPoint = "Decrypt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DecryptNative64(byte[] data, UIntPtr dataLength, byte[] key, UIntPtr keyLength, byte[] result, UIntPtr resultLength);

        [DllImport(LibName86, EntryPoint = "Decrypt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DecryptNative86(byte[] data, UIntPtr dataLength, byte[] key, UIntPtr keyLength, byte[] result, UIntPtr resultLength);

        [DllImport(LibName64, EntryPoint = "DecryptAsymmetric", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DecryptAsymmetricNative64(byte[] data, UIntPtr dataLength, byte[] privateKey, UIntPtr privateKeyLength, byte[] result, UIntPtr resultLength);

        [DllImport(LibName86, EntryPoint = "DecryptAsymmetric", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DecryptAsymmetricNative86(byte[] data, UIntPtr dataLength, byte[] privateKey, UIntPtr privateKeyLength, byte[] result, UIntPtr resultLength);

        [DllImport(LibName86, EntryPoint = "DeriveKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DeriveKeyNative86(byte[] key, UIntPtr keyLength, byte[] salt, UIntPtr saltLength, UIntPtr iterations, byte[] result, UIntPtr resultLength);

        [DllImport(LibName64, EntryPoint = "DeriveKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DeriveKeyNative64(byte[] key, UIntPtr keyLength, byte[] salt, UIntPtr saltLength, UIntPtr iterations, byte[] result, UIntPtr resultLength);

        [DllImport(LibName86, EntryPoint = "DeriveKeyPair", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DeriveKeyPairNative86(
            byte[] password,
            UIntPtr passwordLength,
            byte[] parameters,
            UIntPtr parametersLength,
            byte[] privateKey,
            UIntPtr privateKeyLength,
            byte[] publicKey,
            UIntPtr publicKeyLength);

        [DllImport(LibName64, EntryPoint = "DeriveKeyPair", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DeriveKeyPairNative64(
            byte[] password,
            UIntPtr passwordLength,
            byte[] parameters,
            UIntPtr parametersLength,
            byte[] privateKey,
            UIntPtr privateKeyLength,
            byte[] publicKey,
            UIntPtr publicKeyLength);

        [DllImport(LibName86, EntryPoint = "DeriveKeyPairSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DeriveKeyPairSizeNative86();

        [DllImport(LibName64, EntryPoint = "DeriveKeyPairSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long DeriveKeyPairSizeNative64();

        [DllImport(LibName86, EntryPoint = "Encode", CallingConvention = CallingConvention.Cdecl)]
        private static extern long Encode86(byte[] input, UIntPtr input_length, byte[] output, UIntPtr output_length);

        [DllImport(LibName64, EntryPoint = "Encode", CallingConvention = CallingConvention.Cdecl)]
        private static extern long Encode64(byte[] input, UIntPtr input_length, byte[] output, UIntPtr output_length);

        [DllImport(LibName86, EntryPoint = "Encrypt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptNative86(byte[] data, UIntPtr dataLength, byte[] key, UIntPtr keyLength, byte[] result, UIntPtr resultLength, ushort version);

        [DllImport(LibName64, EntryPoint = "Encrypt", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptNative64(byte[] data, UIntPtr dataLength, byte[] key, UIntPtr keyLength, byte[] result, UIntPtr resultLength, ushort version);

        [DllImport(LibName86, EntryPoint = "EncryptAsymmetric", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptAsymmetricNative86(
            byte[] data,
            UIntPtr dataLength,
            byte[] publicKey,
            UIntPtr publicKeyLength,
            byte[] result,
            UIntPtr resultLength,
            ushort version);

        [DllImport(LibName64, EntryPoint = "EncryptAsymmetric", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptAsymmetricNative64(
            byte[] data,
            UIntPtr dataLength,
            byte[] publicKey,
            UIntPtr publicKeyLength,
            byte[] result,
            UIntPtr resultLength,
            ushort version);

        [DllImport(LibName86, EntryPoint = "EncryptAsymmetricSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptAsymmetricSizeNative86(UIntPtr dataLength, ushort version);

        [DllImport(LibName64, EntryPoint = "EncryptAsymmetricSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptAsymmetricSizeNative64(UIntPtr dataLength, ushort version);

        [DllImport(LibName86, EntryPoint = "EncryptSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptSizeNative86(UIntPtr dataLength, ushort version);

        [DllImport(LibName64, EntryPoint = "EncryptSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long EncryptSizeNative64(UIntPtr dataLength, ushort version);

        [DllImport(LibName86, EntryPoint = "GetDefaultArgon2Parameters", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetDefaultArgon2ParametersNative86(byte[] argon2Parameters, UIntPtr argon2ParametersLength);

        [DllImport(LibName64, EntryPoint = "GetDefaultArgon2Parameters", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetDefaultArgon2ParametersNative64(byte[] argon2Parameters, UIntPtr argon2ParametersLength);

        [DllImport(LibName86, EntryPoint = "GetDefaultArgon2ParametersSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetDefaultArgon2ParametersSizeNative86();

        [DllImport(LibName64, EntryPoint = "GetDefaultArgon2ParametersSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GetDefaultArgon2ParametersSizeNative64();

        [DllImport(LibName86, EntryPoint = "GenerateKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GenerateKeyNative86(byte[] key, UIntPtr keyLength);

        [DllImport(LibName64, EntryPoint = "GenerateKey", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GenerateKeyNative64(byte[] key, UIntPtr keyLength);

        [DllImport(LibName86, EntryPoint = "GenerateKeyPair", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GenerateKeyPairNative86(byte[] privateKey, UIntPtr privateKeySize, byte[] publicKey, UIntPtr publicKeySize);

        [DllImport(LibName64, EntryPoint = "GenerateKeyPair", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GenerateKeyPairNative64(byte[] privateKey, UIntPtr privateKeySize, byte[] publicKey, UIntPtr publicKeySize);

        [DllImport(LibName86, EntryPoint = "GenerateKeyPairSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GenerateKeyPairSizeNative86();

        [DllImport(LibName64, EntryPoint = "GenerateKeyPairSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long GenerateKeyPairSizeNative64();

        [DllImport(LibName86, EntryPoint = "HashPassword", CallingConvention = CallingConvention.Cdecl)]
        private static extern long HashPasswordNative86(byte[] password, UIntPtr passwordLength, uint iterations, byte[] result, UIntPtr resultLength);

        [DllImport(LibName64, EntryPoint = "HashPassword", CallingConvention = CallingConvention.Cdecl)]
        private static extern long HashPasswordNative64(byte[] password, UIntPtr passwordLength, uint iterations, byte[] result, UIntPtr resultLength);

        [DllImport(LibName86, EntryPoint = "HashPasswordLength", CallingConvention = CallingConvention.Cdecl)]
        private static extern long HashPasswordLengthNative86();

        [DllImport(LibName64, EntryPoint = "HashPasswordLength", CallingConvention = CallingConvention.Cdecl)]
        private static extern long HashPasswordLengthNative64();

        [DllImport(LibName86, EntryPoint = "KeySize", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint KeySizeNative86();

        [DllImport(LibName64, EntryPoint = "KeySize", CallingConvention = CallingConvention.Cdecl)]
        private static extern uint KeySizeNative64();

        [DllImport(LibName86, EntryPoint = "MixKeyExchange", CallingConvention = CallingConvention.Cdecl)]
        private static extern long MixKeyExchangeNative86(byte[] privateKey, UIntPtr privateKeySize, byte[] publicKey, UIntPtr publicKeySize, byte[] shared, UIntPtr sharedSize);

        [DllImport(LibName64, EntryPoint = "MixKeyExchange", CallingConvention = CallingConvention.Cdecl)]
        private static extern long MixKeyExchangeNative64(byte[] privateKey, UIntPtr privateKeySize, byte[] publicKey, UIntPtr publicKeySize, byte[] shared, UIntPtr sharedSize);

        [DllImport(LibName86, EntryPoint = "MixKeyExchangeSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long MixKeyExchangeSizeNative86();

        [DllImport(LibName64, EntryPoint = "MixKeyExchangeSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long MixKeyExchangeSizeNative64();

        [DllImport(LibName86, EntryPoint = "Version", CallingConvention = CallingConvention.Cdecl)]
        private static extern long Version86(byte[] output, UIntPtr output_length);

        [DllImport(LibName64, EntryPoint = "Version", CallingConvention = CallingConvention.Cdecl)]
        private static extern long Version64(byte[] output, UIntPtr output_length);

        [DllImport(LibName86, EntryPoint = "VersionSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long VersionSize86();

        [DllImport(LibName64, EntryPoint = "VersionSize", CallingConvention = CallingConvention.Cdecl)]
        private static extern long VersionSize64();

        [DllImport(LibName86, EntryPoint = "VerifyPassword", CallingConvention = CallingConvention.Cdecl)]
        private static extern long VerifyPasswordNative86(byte[] password, UIntPtr passwordLength, byte[] hash, UIntPtr hashLength);

        [DllImport(LibName64, EntryPoint = "VerifyPassword", CallingConvention = CallingConvention.Cdecl)]
        private static extern long VerifyPasswordNative64(byte[] password, UIntPtr passwordLength, byte[] hash, UIntPtr hashLength);
#endif
    }
}