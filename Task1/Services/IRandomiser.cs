using Task1.Configs;

namespace Task1.Services
{
    public interface IRandomiser
    {
        /// <summary>
        /// Returns random date (without random time) after 'minDate' and before 'minDate'+'timespanInDays'
        /// </summary>
        public DateTime GenerateRandomDate(DateTime minDate, int timespanInDays);

        /// <summary>
        /// Returns random string of required length with upppercase and lowercase latin symbols
        /// </summary>
        public string GenerateRandomLatinString(int length);

        /// <summary>
        /// Returns random string of required length with upppercase and lowercase russian symbols.
        /// Letter ё not included due to its place in UTF16
        /// </summary>
        public string GenerateRandomRussianString(int length);

        /// <summary>
        /// Returns random integer larger than 'minValue' and lower than 'minValue'+'diapazoneOfValues'
        /// </summary>
        public int GenerateRandomInt(int minValue, int diapazoneOfValues);

        /// <summary>
        /// Returns random double larger than 'minValue' and lower than 'maxValue'
        /// with required number of digits after dot.
        /// </summary>
        public double GenerateRandomDouble(int minValue, int maxValue, int digitsAfterDot);

        /// <summary>
        /// Returns required amount of lines (format: RandomDate||RandomLatinString||RandomRussianString||RandomInt||RandomDouble||\n)
        /// Elements of each line are randomly generated according to 'configs'
        /// </summary>
        public string GenerateRandomLines(int numberOfLines, LineRandomiserConfigs configs);
    }
}
