using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace FileCopyProtectionServer
{
    class rsa
    {
        public static RSACryptoServiceProvider RsaService = new RSACryptoServiceProvider();
        public static RSAParameters privateKey;

        public static string RSAPrivateKeyGen()
        {
            // 개인키 생성(복호화용)
            privateKey = RSA.Create().ExportParameters(true);
            RsaService.ImportParameters(privateKey);
            string privateKeyText = RsaService.ToXmlString(true);
            return privateKeyText;
        }

        public static string RSAPublicKeyGen()
        {
            // 공개키 생성(암호화용)
            RSAParameters publicKey = new RSAParameters();
            publicKey.Modulus = privateKey.Modulus;
            publicKey.Exponent = privateKey.Exponent;
            RsaService.ImportParameters(publicKey);
            string publicKeyText = RsaService.ToXmlString(false);
            return publicKeyText;
        }
        public string RSAEncrypt(string getValue, string pubKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(pubKey);

            //암호화할 문자열을 UFT8인코딩
            byte[] inbuf = (new UTF8Encoding()).GetBytes(getValue);

            //암호화
            byte[] encbuf = rsa.Encrypt(inbuf, false);

            //암호화된 문자열 Base64인코딩
            return System.Convert.ToBase64String(encbuf);
        }
        public static string RSADecrypt(string getValue, string priKey)
        {
            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.FromXmlString(priKey);

            //sValue문자열을 바이트배열로 변환
            byte[] srcbuf = System.Convert.FromBase64String(getValue);

            //바이트배열 복호화
            byte[] decbuf = rsa.Decrypt(srcbuf, false);

            //복호화 바이트배열을 문자열로 변환
            string sDec = (new UTF8Encoding()).GetString(decbuf, 0, decbuf.Length);
            return sDec;
        }

    }
}
