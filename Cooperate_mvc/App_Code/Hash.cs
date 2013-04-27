using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace HashLib
{
    /// <summary>
    /// Typ hashowania.
    /// </summary>
    public enum HashType : int
    {
        MD5,
        SHA1,
        SHA256,
        SHA512
    }

    /// <summary>
    /// Klasa udostępniająca metody hashowania.
    /// </summary>
    public class Hash
    {
        private Hash() { }

        /// <summary>
        /// Hashuje zadany tekst.
        /// </summary>
        /// <param name="text">Tekst do hashowania.</param>
        /// <param name="hashType">Typ hashowania.</param>
        /// <returns>Hash podanego tekstu.</returns>
        public static string GetHash(string text, HashType hashType)
        {
            string hashString;
            switch (hashType)
            {
                case HashType.MD5: 
                    hashString = GetMD5(text); 
                    break;
                case HashType.SHA1: 
                    hashString = GetSHA1(text); 
                    break;
                case HashType.SHA256:
                    hashString = GetSHA256(text);
                    break;
                case HashType.SHA512:
                    hashString = GetSHA512(text);
                    break;
                default: 
                    hashString = "Invalid Hash Type"; 
                    break;
            }
            return hashString;
        }
        
        /// <summary>
        /// Sprawdza, czy tekst jest zgodny z podanym hashem.
        /// </summary>
        /// <param name="original">Tekst</param>
        /// <param name="hashString">Hash, z którym porównujemy tekst</param>
        /// <param name="hashType">Typ hashowania.</param>
        /// <returns></returns>
        public static bool CheckHash(string original, string hashString, HashType hashType)
        {
            string originalHash = GetHash(original, hashType);
            return (originalHash == hashString);
        }

        #region Funkcje hashujące

        private static string GetMD5(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            MD5 hashString = new MD5CryptoServiceProvider();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA1(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);

            SHA1Managed hashString = new SHA1Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA256(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);
            
            SHA256Managed hashString = new SHA256Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        private static string GetSHA512(string text)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            byte[] hashValue;
            byte[] message = UE.GetBytes(text);
            
            SHA512Managed hashString = new SHA512Managed();
            string hex = "";

            hashValue = hashString.ComputeHash(message);
            foreach (byte x in hashValue)
            {
                hex += String.Format("{0:x2}", x);
            }
            return hex;
        }

        #endregion
    }
}