using Task1.Configs;

namespace Task1.Services
{
    public interface IRandomiser
    {
        public DateTime GenerateRandomDate(DateTime minDate, int timespanInDays);
        public string GenerateRandomLatinString(int length);
        public string GenerateRandomRussianString(int length);
        public int GenerateRandomInt(int minValue, int diapazoneOfValues);
        public double GenerateRandomDouble(int minValue, int maxValue, int digitsAfterDot);
        public string GenerateRandomLines(int numberOfLines, LineRandomiserConfigs configs);
    }
}
