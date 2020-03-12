mod key_v1;

use std::convert::TryFrom;

use super::Argon2Parameters;

use super::DataType;
use super::DcHeader;
use super::DevoCryptoError;
use super::Result;

use self::key_v1::DcKeyV1;

const V1: u16 = 1;

#[derive(Clone)]
pub enum DcKey {
    V1(DcKeyV1),
}

impl DcKey {
    pub fn try_from_header(data: &[u8], header: &DcHeader) -> Result<DcKey> {
        match header.version {
            V1 => Ok(DcKey::V1(DcKeyV1::try_from_header(data, header)?)),
            _ => Err(DevoCryptoError::UnknownVersion),
        }
    }

    pub fn generate_keypair(
        private_header: &mut DcHeader,
        public_header: &mut DcHeader,
    ) -> Result<(DcKey, DcKey)> {
        private_header.data_type = DataType::Key;
        public_header.data_type = DataType::Key;
        private_header.version = V1;
        public_header.version = V1;

        let (private_key, public_key) = DcKeyV1::generate_keypair(private_header, public_header)?;

        Ok((DcKey::V1(private_key), DcKey::V1(public_key)))
    }

    pub fn derive_keypair(
        password: &[u8],
        parameters: &Argon2Parameters,
        private_header: &mut DcHeader,
        public_header: &mut DcHeader,
    ) -> Result<(DcKey, DcKey)> {
        private_header.data_type = DataType::Key;
        public_header.data_type = DataType::Key;
        private_header.version = V1;
        public_header.version = V1;

        let (private_key, public_key) =
            DcKeyV1::derive_keypair(password, parameters, private_header, public_header)?;

        Ok((DcKey::V1(private_key), DcKey::V1(public_key)))
    }

    pub fn mix_key_exchange(&self, public: &DcKey) -> Result<Vec<u8>> {
        match (self, public) {
            (DcKey::V1(private), DcKey::V1(public)) => private.mix_key_exchange(public),
        }
    }
}

impl TryFrom<&DcKey> for x25519_dalek::PublicKey {
    type Error = DevoCryptoError;

    fn try_from(data: &DcKey) -> Result<Self> {
        match data {
            DcKey::V1(x) => Self::try_from(x),
            //_ => Err(DevoCryptoError::InvalidDataType),
        }
    }
}

impl TryFrom<&DcKey> for x25519_dalek::StaticSecret {
    type Error = DevoCryptoError;

    fn try_from(data: &DcKey) -> Result<Self> {
        match data {
            DcKey::V1(x) => Self::try_from(x),
            //_ => Err(DevoCryptoError::InvalidDataType),
        }
    }
}

impl From<DcKey> for Vec<u8> {
    fn from(key: DcKey) -> Vec<u8> {
        match key {
            DcKey::V1(x) => x.into(),
        }
    }
}
