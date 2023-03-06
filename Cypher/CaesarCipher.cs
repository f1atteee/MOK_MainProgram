using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOK_MainInterface.Cypher
{
    public class CaesarCipher
    {
        const string ukrletters = "АБВГДЕЄЖЗИІЇЙКЛМНОПРСТУФХЦЧШЩЬЮЯ";
        string engletters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string UkrCode(string text, int k)
        {
            var fullUkrletters = ukrletters + ukrletters.ToLower();
            var letterQty = fullUkrletters.Length;
            var retVal = "";
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var index = fullUkrletters.IndexOf(c);
                if (index < 0)
                {
                    retVal += c.ToString(); // якщо немає залишаємо
                }
                else
                {
                    var codeIndex = (letterQty + index + k) % letterQty;
                    retVal += fullUkrletters[codeIndex];
                }
            }

            return retVal;
        }


        private string EnglishCode(string text, int k)
        {
            var fullEngletters = engletters + engletters.ToLower();
            var letterQty = fullEngletters.Length;
            var retVal = "";
            for (int i = 0; i < text.Length; i++)
            {
                var c = text[i];
                var index = fullEngletters.IndexOf(c);
                if (index < 0)
                {
                    retVal += c.ToString(); // якщо немає залишаємо
                }
                else
                {
                    var codeIndex = (letterQty + index + k) % letterQty;
                    retVal += fullEngletters[codeIndex];
                }
            }

            return retVal;
        }


        public string UkrEncrypt(string plainMessage, int key)
            => UkrCode(plainMessage, key);

        public string UkrDecrypt(string encryptedMessage, int key)
            => UkrCode(encryptedMessage, -key);

        public string EngEncrypt(string plainMessage, int key)
            => EnglishCode(plainMessage, key);

        public string EngDecrypt(string encryptedMessage, int key)
            => EnglishCode(encryptedMessage, -key);
    }
}
