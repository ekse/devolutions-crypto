use super::Argon2Parameters;

use super::DcHeader;
use super::DevoCryptoError;
use super::Result;

use std::convert::TryFrom;

use rand::rngs::OsRng;
use x25519_dalek::{PublicKey, StaticSecret};
use zeroize::Zeroize;

const PRIVATE: u16 = 1;
const PUBLIC: u16 = 2;

#[derive(Clone)]
pub enum DcKeyV1 {
    Private(StaticSecret),
    Public(PublicKey),
}

impl From<DcKeyV1> for Vec<u8> {
    fn from(key: DcKeyV1) -> Vec<u8> {
        match key {
            DcKeyV1::Private(x) => x.to_bytes().to_vec(),
            DcKeyV1::Public(x) => x.as_bytes().to_vec(),
        }
    }
}

impl DcKeyV1 {
    pub fn try_from_header(data: &[u8], header: &DcHeader) -> Result<DcKeyV1> {
        if data.len() != 32 {
            return Err(DevoCryptoError::InvalidLength);
        };

        let mut key_bytes = [0u8; 32];

        key_bytes.copy_from_slice(&data[0..32]);

        let key = match header.data_subtype {
            PRIVATE => DcKeyV1::Private(StaticSecret::from(key_bytes)),
            PUBLIC => DcKeyV1::Public(PublicKey::from(key_bytes)),
            _ => return Err(DevoCryptoError::UnknownSubtype),
        };
        Ok(key)
    }

    pub fn generate_keypair(
        private_header: &mut DcHeader,
        public_header: &mut DcHeader,
    ) -> Result<(DcKeyV1, DcKeyV1)> {
        private_header.data_subtype = PRIVATE;
        public_header.data_subtype = PUBLIC;

        let private = StaticSecret::new(&mut OsRng);
        let public = PublicKey::from(&private);

        Ok((DcKeyV1::Private(private), DcKeyV1::Public(public)))
    }

    pub fn mix_key_exchange(&self, public: &DcKeyV1) -> Result<Vec<u8>> {
        match (self, public) {
            (DcKeyV1::Private(private), DcKeyV1::Public(public)) => {
                Ok(private.diffie_hellman(&public).as_bytes().to_vec())
            }
            _ => Err(DevoCryptoError::InvalidDataType),
        }
    }

    pub fn derive_keypair(
        password: &[u8],
        parameters: &Argon2Parameters,
        private_header: &mut DcHeader,
        public_header: &mut DcHeader,
    ) -> Result<(DcKeyV1, DcKeyV1)> {
        if parameters.length != 32 {
            return Err(DevoCryptoError::InvalidLength);
        }

        private_header.data_subtype = PRIVATE;
        public_header.data_subtype = PUBLIC;

        let mut derived_pass = parameters.compute(password)?;

        let mut key_bytes = [0u8; 32];
        key_bytes.copy_from_slice(&derived_pass[0..32]);

        derived_pass.zeroize();

        let private = StaticSecret::from(key_bytes);
        let public = PublicKey::from(&private);

        Ok((DcKeyV1::Private(private), DcKeyV1::Public(public)))
    }
}

impl TryFrom<&DcKeyV1> for x25519_dalek::PublicKey {
    type Error = DevoCryptoError;

    fn try_from(data: &DcKeyV1) -> Result<Self> {
        match data {
            DcKeyV1::Public(x) => Ok(*x),
            _ => Err(DevoCryptoError::InvalidDataType),
        }
    }
}

impl TryFrom<&DcKeyV1> for x25519_dalek::StaticSecret {
    type Error = DevoCryptoError;

    fn try_from(data: &DcKeyV1) -> Result<Self> {
        match data {
            DcKeyV1::Private(x) => Ok(x.clone()),
            _ => Err(DevoCryptoError::InvalidDataType),
        }
    }
}
