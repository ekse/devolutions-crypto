#[macro_use]
extern crate cfg_if;

cfg_if! {
    if #[cfg(target_arch = "wasm32")] {
        extern crate base64;
        extern crate wasm_bindgen_test;

        use wasm_bindgen_test::*;

        use devolutionscrypto::wasm;

        #[wasm_bindgen_test]
        fn test_encrypt_decrypt() {
            let data = "test".as_bytes();
            let key = base64::decode("dpxbute8LZ4tqpw1pVWyBvMzOtm+OJQPcIsU52+FFZU=").unwrap();

            let ciphertext = wasm::encrypt(data, &key, None).unwrap();
            let plaintext = wasm::decrypt(&ciphertext, &key).unwrap();

            assert_eq!(plaintext, data);
        }

        #[wasm_bindgen_test]
        fn test_hash_password() {
            let password = "ThisIsAGoodPassword123".as_bytes();

            let hash = wasm::hash_password(password, Some(123)).unwrap();
            assert!(wasm::verify_password(password, &hash).unwrap());

            let bad_password = "thisisabadpassword1234".as_bytes();
            assert!(!wasm::verify_password(bad_password, &hash).unwrap());
        }

        #[wasm_bindgen_test]
        fn test_key_exchange() {
            let bob_keypair = wasm::generate_key_exchange().unwrap();
            let alice_keypair = wasm::generate_key_exchange().unwrap();

            let bob_key = wasm::mix_key_exchange(&bob_keypair.private(), &alice_keypair.public()).unwrap();
            let alice_key = wasm::mix_key_exchange(&alice_keypair.private(), &bob_keypair.public()).unwrap();

            assert_ne!(bob_key.len(), 0);
            assert_eq!(bob_key, alice_key);
        }

        #[wasm_bindgen_test]
        fn test_generate_key() {
            let key = wasm::generate_key(Some(10)).unwrap();

            assert_eq!(key.len(), 10);
            assert_ne!(&key, &[0, 0, 0, 0, 0, 0, 0, 0, 0, 0]);
        }

        #[wasm_bindgen_test]
        fn test_base64() {
            let plain = "ThIs1saTesT";
            let encoded = wasm::base64encode(plain.as_bytes());
            assert_eq!(&encoded, "VGhJczFzYVRlc1Q=");

            let decoded = wasm::base64decode(&encoded).unwrap();
            assert_eq!(decoded.as_slice(), plain.as_bytes());
        }

        #[wasm_bindgen_test]
        fn test_derive_key() {
            let password = "ThisIsAGoodPassword123".as_bytes();
            let salt = base64::decode("u4tv/i1228VOqoZWITseoQ==").unwrap();
            let key = wasm::derive_key(password, &salt, 123, 32);

            assert_eq!(key, base64::decode("RfIYPWWXRSm/SWjVXvQq1Z3n/mzxGeu/y396bAuYWTI=").unwrap());
        }
    }
}
