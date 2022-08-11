using Task1.Configs;

namespace Tests
{
    internal class MockData
    {
        private static LineRandomiserConfigs _configs = new LineRandomiserConfigs()
        {
            TimespanInYears = 1,
            LengthOfLatinString = 10,
            LengthOfRussianString = 11,
            MinimalIntegerValue = 10,
            MaximalIntegerValue = 90,
            MinimalDoubleValue = 2,
            MaximalDoubleValue = 10,
            DigitsAfterDot = 5
        };
        public static LineRandomiserConfigs Configs
        {
            get { return _configs; }
        }
    }
}
