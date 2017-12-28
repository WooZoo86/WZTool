using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ZToolLib.Algorithm.Cryptor
{
    public class RijndaelHelper
    {
        private enum TransformType
        {
            ENCRYPT,
            DECRYPT
        }

        public string Phrase
        {
            set
            {
                this._Phrase = value;
                this.GenerateKey(this._Phrase);
            }
        }

        public RijndaelHelper(string SecretPhrase)
        {
            this.Phrase = SecretPhrase;
        }

        public string Encrypt(string EncryptValue)
        {
            string result;
            try
            {
                if (EncryptValue.Length > 0)
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(EncryptValue);
                    result = Convert.ToBase64String(this.Transform(bytes, RijndaelHelper.TransformType.ENCRYPT));
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public string Decrypt(string DecryptValue)
        {
            string result;
            try
            {
                if (DecryptValue.Length > 0)
                {
                    byte[] input = Convert.FromBase64String(DecryptValue);
                    result = Encoding.UTF8.GetString(this.Transform(input, RijndaelHelper.TransformType.DECRYPT));
                }
                else
                {
                    result = "";
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return result;
        }

        public void Encrypt(string InputFile, string OutputFile)
        {
            try
            {
                if (InputFile != null && InputFile.Length > 0)
                {
                    this._inputFile = InputFile;
                }
                if (OutputFile != null && OutputFile.Length > 0)
                {
                    this._outputFile = OutputFile;
                }

                this.Transform(null, RijndaelHelper.TransformType.ENCRYPT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Decrypt(string InputFile, string OutputFile)
        {
            try
            {
                if (InputFile != null && InputFile.Length > 0)
                {
                    this._inputFile = InputFile;
                }

                if (OutputFile != null && OutputFile.Length > 0)
                {
                    this._outputFile = OutputFile;
                }

                this.Transform(null, RijndaelHelper.TransformType.DECRYPT);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void SetPhrase(byte[] iv, byte[] key)
        {
            this._Key = new byte[24];
            this._IV = new byte[16];

            iv.CopyTo(this._IV, 0);
            key.CopyTo(this._Key, 0);
        }

        private void GenerateKey(string SecretPhrase)
        {
            this._Key = new byte[24];
            this._IV = new byte[16];

            byte[] bytes = Encoding.ASCII.GetBytes(SecretPhrase);
            SHA384Managed sha384Managed = new SHA384Managed();
            sha384Managed.ComputeHash(bytes);
            byte[] hash = sha384Managed.Hash;

            for (int i = 0; i < 24; i++)
            {
                this._Key[i] = hash[i];
            }

            for (int i = 24; i < 40; i++)
            {
                this._IV[i - 24] = hash[i];
            }
        }

        private byte[] Transform(byte[] input, RijndaelHelper.TransformType transformType)
        {
            CryptoStream cryptoStream = null;
            RijndaelManaged rijndaelManaged = null;
            ICryptoTransform cryptoTransform = null;
            FileStream fileStream = null;
            FileStream fileStream2 = null;
            MemoryStream memoryStream = null;
            byte[] result;

            try
            {
                rijndaelManaged = new RijndaelManaged();
                rijndaelManaged.Key = this._Key;
                rijndaelManaged.IV = this._IV;

                if (transformType == RijndaelHelper.TransformType.ENCRYPT)
                {
                    cryptoTransform = rijndaelManaged.CreateEncryptor();
                }
                else
                {
                    cryptoTransform = rijndaelManaged.CreateDecryptor();
                }

                if (input != null && input.Length > 0)
                {
                    memoryStream = new MemoryStream();
                    cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write);
                    cryptoStream.Write(input, 0, input.Length);
                    cryptoStream.FlushFinalBlock();
                    result = memoryStream.ToArray();
                }
                else
                {
                    if (this._inputFile.Length > 0 && this._outputFile.Length > 0)
                    {
                        fileStream = new FileStream(this._inputFile, FileMode.Open, FileAccess.Read);
                        fileStream2 = new FileStream(this._outputFile, FileMode.OpenOrCreate, FileAccess.Write);
                        cryptoStream = new CryptoStream(fileStream2, cryptoTransform, CryptoStreamMode.Write);

                        int num = 4096;
                        byte[] buffer = new byte[num];
                        int num2;
                        do
                        {
                            num2 = fileStream.Read(buffer, 0, num);
                            cryptoStream.Write(buffer, 0, num2);
                        }
                        while (num2 != 0);

                        cryptoStream.FlushFinalBlock();
                    }

                    result = null;
                }
            }
            catch (CryptographicException)
            {
                throw new CryptographicException("Password is invalid. Please verify once again.");
            }
            finally
            {
                if (rijndaelManaged != null)
                {
                    rijndaelManaged.Clear();
                }
                if (cryptoTransform != null)
                {
                    cryptoTransform.Dispose();
                }
                if (cryptoStream != null)
                {
                    cryptoStream.Close();
                }
                if (memoryStream != null)
                {
                    memoryStream.Close();
                }
                if (fileStream2 != null)
                {
                    fileStream2.Close();
                }
                if (fileStream != null)
                {
                    fileStream.Close();
                }
            }
            return result;
        }

        private string _Phrase = "";

        private string _inputFile = "";

        private string _outputFile = "";

        private byte[] _IV;

        private byte[] _Key;
    }
}
