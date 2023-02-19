using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MOK_MainInterface.Cypher
{
    public class BruteForceFrequencyTable
    {
        public Dictionary<string, int> BuildBruteForceFrequencyTable(string input, int keyLength)
        {
            Dictionary<string, int> frequencyTable = new Dictionary<string, int>();

            // Ініціалізуємо частотну таблицю
            for (int i = 0; i < Math.Pow(32, keyLength); i++)
            {
                string key = ConvertToKey(i, keyLength);
                frequencyTable[key] = 0;
            }

            // Підраховуємо кількість входжень кожного ключа у вхідному тексті
            for (int i = 0; i < input.Length - keyLength + 1; i++)
            {
                string substring = input.Substring(i, keyLength);
                string key = ConvertToKey(substring);
                frequencyTable[key]++;
            }

            return frequencyTable;
        }

        private static string ConvertToKey(string substring)
        {
            StringBuilder keyBuilder = new StringBuilder();
            foreach (char c in substring)
            {
                keyBuilder.Append(ConvertToCharIndex(c));
            }
            return keyBuilder.ToString();
        }

        private static string ConvertToKey(int keyIndex, int keyLength)
        {
            StringBuilder keyBuilder = new StringBuilder();
            for (int i = 0; i < keyLength; i++)
            {
                int charIndex = (keyIndex / (int)Math.Pow(32, i)) % 32;
                keyBuilder.Append(ConvertToChar(charIndex));
            }
            return keyBuilder.ToString();
        }

        private static int ConvertToCharIndex(char c)
        {
            return c - 'а';
        }

        private static char ConvertToChar(int charIndex)
        {
            return (char)(charIndex + 'а');
        }

    }
}
