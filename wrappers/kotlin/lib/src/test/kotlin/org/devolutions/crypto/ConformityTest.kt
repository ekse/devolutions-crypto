/*
 * This source file was generated by the Gradle 'init' task
 */
package org.devolutions.crypto

import kotlin.test.Test
import kotlin.test.assertContentEquals
import kotlin.test.assertFalse

class ConformityTest {
    @Test
    fun deriveKeyPbkdf2Test() {
        val derivedKey = deriveKeyPbkdf2("testpassword".toByteArray(), null)
        val derivedKeyWithIterations = deriveKeyPbkdf2("testPa\$\$".toByteArray(), null, iterations = 100u)
        val derivedKeyWithSalt = deriveKeyPbkdf2(
            "testPa\$\$".toByteArray(),
            base64Decode("tdTt5wgeqQYLvkiXKkFirqy2hMbzadBtL+jekVeNCRA="),
            iterations = 100u
        )

        val expected = base64Decode("ImfGCyv6PwMYaJShGxR4MfVrjuUrsI0CSarJgOApwf8=")
        val expectedWithIterations = base64Decode("ev/GiJLvOgIkkWrnIrHSi2fdZE5qJBIrW+DLeMLIXK4=")
        val expectedWithSalt = base64Decode("ZaYRZeQiIPJ+Jl511AgHZjv4/HbCFq4eUP9yNa3gowI=")

        assertContentEquals(expected, derivedKey)
        assertContentEquals(expectedWithIterations, derivedKeyWithIterations)
        assertContentEquals(expectedWithSalt, derivedKeyWithSalt)
    }

    @Test
    fun deriveKeyArgon2Test() {
        val password = "password".toByteArray()
        val parameters =
            Argon2Parameters.newFromBytes(base64Decode("AQAAACAAAAABAAAAIAAAAAEAAAACEwAAAAAQAAAAimFBkm3f8+f+YfLRnF5OoQ=="))
        val result = deriveKeyArgon2(password, parameters)

        val expected = base64Decode("AcEN6Cb1Om6tomZScAM725qiXMzaxaHlj3iMiT/Ukq0=")

        assertContentEquals(expected, result)
    }

    @Test
    fun symmetricDecryptV1Test() {
        val key = base64Decode("ozJVEme4+5e/4NG3C+Rl26GQbGWAqGc0QPX8/1xvaFM=")
        val ciphertext =
            base64Decode("DQwCAAAAAQCK1twEut+TeJfFbTWCRgHjyS6bOPOZUEQAeBtSFFRl2jHggM/34n68zIZWGbsZHkufVzU6mTN5N2Dx9bTplrycv5eNVevT4P9FdVHJ751D+A==")

        val result = decrypt(ciphertext, key)

        val expected = "test Ciph3rtext~".toByteArray()

        assertContentEquals(expected, result)
    }

    @Test
    fun symmetricDecryptWithAadV1Test() {
        val key = base64Decode("ozJVEme4+5e/4NG3C+Rl26GQbGWAqGc0QPX8/1xvaFM=")
        val ciphertext =
            base64Decode("DQwCAAEAAQCeKfbTqYjfVCEPEiAJjiypBstPmZz0AnpliZKoR+WXTKdj2f/4ops0++dDBVZ+XdyE1KfqxViWVc9djy/HSCcPR4nDehtNI69heGCIFudXfQ==")
        val aad = "this is some public data".toByteArray()

        val result = decryptWithAad(ciphertext, key, aad)

        val expected = "test Ciph3rtext~".toByteArray()

        assertContentEquals(expected, result)
    }

    @Test
    fun symmetricDecryptV2Test() {
        val key = base64Decode("ozJVEme4+5e/4NG3C+Rl26GQbGWAqGc0QPX8/1xvaFM=")
        val ciphertext =
            base64Decode("DQwCAAAAAgAA0iPpI4IEzcrWAQiy6tqDqLbRYduGvlMC32mVH7tpIN2CXDUu5QHF91I7pMrmjt/61pm5CeR/IcU=")

        val result = decrypt(ciphertext, key)

        val expected = "test Ciph3rtext~2".toByteArray()

        assertContentEquals(expected, result)
    }

    @Test
    fun symmetricDecryptWithAadV2Test() {
        val key = base64Decode("ozJVEme4+5e/4NG3C+Rl26GQbGWAqGc0QPX8/1xvaFM=")
        val ciphertext =
            base64Decode("DQwCAAEAAgA9bh989dao0Pvaz1NpJTI5m7M4br2qVjZtFwXXoXZOlkCjtqU/uif4pbNCcpEodzeP4YG1QvfKVQ==")
        val aad = "this is some public data".toByteArray()

        val result = decryptWithAad(ciphertext, key, aad)

        val expected = "test Ciph3rtext~".toByteArray()

        assertContentEquals(expected, result)
    }

    @Test
    fun asymmetricDecryptWithAadV2Test() {
        val privateKey = base64Decode("DQwBAAEAAQC9qf9UY1ovL/48ALGHL9SLVpVozbdjYsw0EPerUl3zYA==")
        val ciphertext =
            base64Decode("DQwCAAIAAgB1u62xYeyppWf83QdWwbwGUt5QuiAFZr+hIiFEvMRbXiNCE3RMBNbmgQkLr/vME0BeQa+uUTXZARvJcyNXHyAE4tSdw6o/psU/kw/Z/FbsPw==")
        val aad = "this is some public data".toByteArray()

        val result = decryptAsymmetricWithAad(ciphertext, privateKey, aad)

        val expected = "testdata".toByteArray()

        assertContentEquals(expected, result)
    }

    @Test
    fun passwordHashingV1Test() {
        val hash1 =
            base64Decode("DQwDAAAAAQAQJwAAXCzLFoyeZhFSDYBAPiIWhCk04aoP/lalOoCl7D+skIY/i+3WT7dn6L8WvnfEq6flCd7i+IcKb3GEK4rCpzhDlw==")
        val hash2 =
            base64Decode("DQwDAAAAAQAKAAAAmH1BBckBJYDD0xfiwkAk1xwKgw8a57YQT0Igm+Faa9LFamTeEJgqn/qHc2R/8XEyK2iLPkVy+IErdGLLtLKJ2g==")

        assert(verifyPassword("password1".toByteArray(), hash1))
        assert(verifyPassword("password1".toByteArray(), hash2))
    }

    @Test
    fun signatureV1Test() {
        val publicKey = base64Decode("DQwFAAIAAQDeEvwlEigK5AXoTorhmlKP6+mbiUU2rYrVQ25JQ5xang==")
        val signature =
            base64Decode("DQwGAAAAAQD82uRk4sFC8vEni6pDNw/vOdN1IEDg9cAVfprWJZ/JBls9Gi61cUt5u6uBJtseNGZFT7qKLvp4NUZrAOL8FH0K")

        assert(verifySignature("this is a test".toByteArray(), publicKey, signature))
        assertFalse(verifySignature("this is wrong".toByteArray(), publicKey, signature))
    }
}