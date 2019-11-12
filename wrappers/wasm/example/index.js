// Import library
import * as devolutionscrypto from "../pkg/devolutionscrypto";

// Generate a random key for the lifetime of the script
var key = devolutionscrypto.generate_key();

// Declare a bunch of global variables to access the DOM
var enc = new TextEncoder();
var dec = new TextDecoder();

var publicBob = null;
var privateBob = null;

var publicAlice = null;
var privateAlice = null;

var encryptionInput = document.getElementById("encryptionInput");
var encryptionOutput = document.getElementById("encryptionOutput");

var hashPasswordPasswordInput = document.getElementById("hashPasswordPasswordInput");
var hashPasswordIterationsInput = document.getElementById("hashPasswordIterationsInput");
var hashPasswordOutput = document.getElementById("hashPasswordOutput");

var verifyPasswordPasswordInput = document.getElementById("verifyPasswordPasswordInput");
var verifyPasswordHashInput = document.getElementById("verifyPasswordHashInput");
var verifyPasswordOutput = document.getElementById("verifyPasswordOutput");

var outputalice = document.getElementById("alice");
var outputbob = document.getElementById("bob");

var outputalicemix = document.getElementById("alicemix");
var outputbobmix = document.getElementById("bobmix");

var base64input = document.getElementById("base64input");
var base64output = document.getElementById("base64output");

var generateKeyLengthInput = document.getElementById("generateKeyLengthInput");
var generateKeyOutput = document.getElementById("generateKeyOutput");

var deriveKeyPasswordInput = document.getElementById("deriveKeyPasswordInput");
var deriveKeySaltInput = document.getElementById("deriveKeySaltInput");
var deriveKeyIterationsInput = document.getElementById("deriveKeyIterationsInput");
var deriveKeyLengthInput = document.getElementById("deriveKeyLengthInput");
var deriveKeyOutput = document.getElementById("deriveKeyOutput");

var btnEncrypt = document.getElementById("btnEncrypt");
var btnDecrypt = document.getElementById("btnDecrypt");

var btnHashPassword = document.getElementById("btnHashPassword");
var btnVerifyPassword = document.getElementById("btnVerifyPassword");

var btnBob = document.getElementById("btnBob");
var btnAlice = document.getElementById("btnAlice");

var btnBobMix = document.getElementById("btnBobMix");
var btnAliceMix = document.getElementById("btnAliceMix");

var btnBase64Encode = document.getElementById("btnBase64Encode");
var btnBase64Decode = document.getElementById("btnBase64Decode");

var btnGenerateKey = document.getElementById("btnGenerateKey");

var btnDeriveKey = document.getElementById("btnDeriveKey");

// Add handlers to buttons

// Encryption
btnEncrypt.addEventListener("click", () => 
{
    let result = devolutionscrypto.encrypt(enc.encode(encryptionInput.value),  key);
    encryptionOutput.value = devolutionscrypto.base64encode(result);
});

btnDecrypt.addEventListener("click", () =>
{
    let buffer = devolutionscrypto.base64decode(encryptionInput.value);

    let result = devolutionscrypto.decrypt(buffer, key);
    encryptionOutput.value = dec.decode(result);

});

//Password Hasing
// Hash
btnHashPassword.addEventListener("click", () => {
    let password = enc.encode(hashPasswordPasswordInput.value);

    let iterations = parseInt(hashPasswordIterationsInput.value);
    if(!iterations) {
        iterations = 10000;
    }

    hashPasswordOutput.value = devolutionscrypto.base64encode(devolutionscrypto.hash_password(password, iterations));
});

//Verify
btnVerifyPassword.addEventListener("click", () => {
    let password = enc.encode(verifyPasswordPasswordInput.value);
    let hash = devolutionscrypto.base64decode(verifyPasswordHashInput.value);

    verifyPasswordOutput.value = devolutionscrypto.verify_password(password, hash);
});

// Keypair Generation
btnBob.addEventListener("click", () => {
    let result = devolutionscrypto.generate_key_exchange();

    publicBob = result.public();
    privateBob = result.private();

    outputbob.value = devolutionscrypto.base64encode(publicBob);

});

btnAlice.addEventListener("click", () => {
    let result = devolutionscrypto.generate_key_exchange();

    publicAlice = result.public();
    privateAlice = result.private();

    outputalice.value = devolutionscrypto.base64encode(publicAlice);
});

// Key Exchange
btnBobMix.addEventListener("click", () => {
    let result = devolutionscrypto.mix_key_exchange(privateBob, publicAlice);
    outputbobmix.value = devolutionscrypto.base64encode(result);
});

btnAliceMix.addEventListener("click", () => {
    let result = devolutionscrypto.mix_key_exchange(privateAlice, publicBob);
    outputalicemix.value = devolutionscrypto.base64encode(result);
});

// Utils
// Base64
btnBase64Encode.addEventListener("click", () => {
    base64output.value = devolutionscrypto.base64encode(enc.encode(base64input.value));
});

btnBase64Decode.addEventListener("click", () => {
    base64output.value = dec.decode(devolutionscrypto.base64decode(base64input.value));
});

// Generate Key
btnGenerateKey.addEventListener("click", () => {
    let length = parseInt(generateKeyLengthInput.value);
    if(!length) {
        length = 32;
    }

    generateKeyOutput.value = devolutionscrypto.base64encode(devolutionscrypto.generate_key(length));
});

// Derive Key
btnDeriveKey.addEventListener("click", () => {
    let password = enc.encode(deriveKeyPasswordInput.value);

    let iterations = parseInt(deriveKeyIterationsInput.value);
    if(!iterations) {
        iterations = 10000;
    }

    let salt = deriveKeySaltInput.value;
    if(!salt) {
        salt = new Uint8Array(0);
    }
    else {
        salt = devolutionscrypto.base64decode(salt);
    }

    let length = parseInt(deriveKeyLengthInput.value);
    if(!length) {
        length = 32;
    }

    deriveKeyOutput.value = devolutionscrypto.base64encode(devolutionscrypto.derive_key(password, salt, iterations, length));
});
