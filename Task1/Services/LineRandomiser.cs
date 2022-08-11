using System.Globalization;
using System.Text;
using Task1.Configs;

namespace Task1.Services
{
    public class LineRandomiser : IRandomiser
    {
        private const int LATIN_LETTERS_IN_ALPHABET = 26;
        private const int RUSSIAN_LETTERS_IN_ALPHABET = 32; //ё not included
        private Random _random;
        private Random Rand 
        {
            get
            {
                if (_random == null)
                    _random = new Random();
                return _random;
            }
        }

        public DateTime GenerateRandomDate(DateTime minDate, int timespanInDays)
        {
            int daysAfterMinDate = Rand.Next() % timespanInDays;
            return minDate.AddDays(daysAfterMinDate);
        }

        public double GenerateRandomDouble(int minValue, int maxValue, int digitsAfterDot)
        {
            long randomLong = Rand.NextInt64();
            return (randomLong % ((maxValue - minValue) * Math.Pow(10, digitsAfterDot))) * Math.Pow(10, -digitsAfterDot) + minValue;
        }

        public int GenerateRandomInt(int minValue, int maxValue)
        {
            return Rand.Next() % (maxValue - minValue) + minValue;
        }

        private string GenerateRandomStringFromLettersOfBothRegisters(char upperRegisterLetter, char lowerRegisterLetter, int lettersInAlphabet, int length)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < length; i++)
            {
                int randomInt = Rand.Next();
                if ((randomInt & 0x4000_0000) == 0)
                {
                    builder.Append((char)(upperRegisterLetter + randomInt % lettersInAlphabet));
                }
                else
                {
                    builder.Append((char)(lowerRegisterLetter + randomInt % lettersInAlphabet));
                }
            }
            return builder.ToString();
        }

        public string GenerateRandomLatinString(int length)
        {
            return GenerateRandomStringFromLettersOfBothRegisters('A', 'a', LATIN_LETTERS_IN_ALPHABET, length);
        }

        public string GenerateRandomRussianString(int length)
        {
            return GenerateRandomStringFromLettersOfBothRegisters('А', 'а', RUSSIAN_LETTERS_IN_ALPHABET, length);
        }


        public string GenerateRandomLines(int numberOfLines, LineRandomiserConfigs configs)
        {
            StringBuilder builder = new StringBuilder();
            DateTime dateFiveYearsAgo = DateTime.UtcNow.AddYears(-configs.TimespanInYears);

            for(int i = 0; i < numberOfLines; i++)
            {
                DateTime randomDate = GenerateRandomDate(dateFiveYearsAgo, configs.TimespanInYears * 365);
                string randomLatinString = GenerateRandomLatinString(configs.LengthOfLatinString);
                string randomRussianString = GenerateRandomRussianString(configs.LengthOfRussianString);
                int randomIntegerNumber = GenerateRandomInt(configs.MinimalIntegerValue, configs.MaximalIntegerValue);
                double randomDoubleNumber = GenerateRandomDouble(configs.MinimalDoubleValue, configs.MaximalDoubleValue, configs.DigitsAfterDot);

                builder.Append($"{randomDate.ToString("dd.MM.yyyy")}||{randomLatinString}||{randomRussianString}||{randomIntegerNumber}||{randomDoubleNumber.ToString($"F{configs.DigitsAfterDot}", new CultureInfo("de-DE"))}||\n");
            }

            return builder.ToString();
        }
    }
}
