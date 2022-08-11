using System.Text.RegularExpressions;
using Task1.Services;
using static Tests.MockData;

namespace Tests
{
    [TestClass]
    public class TestLineRandomiser
    {
        private const int NUMBER_OF_TEST_ITERATIONS = 500;

        IRandomiser _randomiser;
        IRandomiser Randomiser
        {
            get
            {
                if (_randomiser == null)
                {
                    _randomiser = new LineRandomiser();
                }
                return _randomiser;
            }
        }

        [TestMethod]
        public void TestDateGeneration()
        {
            int timespanInDays = Configs.TimespanInYears * 365;
            for (int i = 0; i < NUMBER_OF_TEST_ITERATIONS; i++)
            {
                DateTime randomDate = Randomiser.GenerateRandomDate(DateTime.Now.AddDays(-timespanInDays), timespanInDays - 1);
                Assert.IsTrue(randomDate <= DateTime.Now, $"{randomDate} is greater than max value: {DateTime.Now}");
                Assert.IsTrue(randomDate >= DateTime.Now.AddDays(-timespanInDays-1), $"{randomDate} is lower than min value {DateTime.Now.AddDays(-timespanInDays)}");
            }
        }

        [TestMethod]
        public void TestLatinStringGeneration()
        {
            Regex latinLineRegex = new Regex("^[A-Za-z]{10}$",RegexOptions.Compiled);
            for (int i = 0; i < NUMBER_OF_TEST_ITERATIONS; i++)
            {
                string randomLatinLine = Randomiser.GenerateRandomLatinString(Configs.LengthOfLatinString);
                Assert.IsTrue(latinLineRegex.Matches(randomLatinLine).Count == 1, $"{randomLatinLine} doesn't match required regexp: {latinLineRegex}");
            }
        }

        [TestMethod]
        public void TestRussianStringGeneration()
        {
            Regex russianLineRegex = new Regex("^[А-Яа-я]{11}$", RegexOptions.Compiled);
            for (int i = 0; i < NUMBER_OF_TEST_ITERATIONS; i++)
            {
                string randomRussianLine = Randomiser.GenerateRandomRussianString(Configs.LengthOfRussianString);
                Assert.IsTrue(russianLineRegex.Matches(randomRussianLine).Count == 1, $"{randomRussianLine} doesn't match required regexp: {russianLineRegex}");
            }
        }

        [TestMethod]
        public void TestIntegerGeneration()
        {
            for(int i=0; i < NUMBER_OF_TEST_ITERATIONS; i++)
            {
                int randomInteger = Randomiser.GenerateRandomInt(Configs.MinimalIntegerValue, Configs.MaximalIntegerValue);
                Assert.IsTrue(randomInteger >= Configs.MinimalIntegerValue, $"{randomInteger} is lower than min value {Configs.MinimalIntegerValue}");
                Assert.IsTrue(randomInteger <= Configs.MaximalIntegerValue, $"{randomInteger} is greater than max value {Configs.MaximalIntegerValue}");
            }
        }

        [TestMethod]
        public void TestDoubleGeneration()
        {
            for(int i=0; i < NUMBER_OF_TEST_ITERATIONS; i++)
            {
                double randomDouble = Randomiser.GenerateRandomDouble(Configs.MinimalDoubleValue, Configs.MaximalDoubleValue, Configs.DigitsAfterDot);
                Assert.IsTrue(randomDouble >= Configs.MinimalDoubleValue, $"{randomDouble} is lower than min value {Configs.MinimalDoubleValue}");
                Assert.IsTrue(randomDouble <= Configs.MaximalDoubleValue, $"{randomDouble} is greater than max value {Configs.MaximalDoubleValue}");
                Assert.IsTrue(Math.Round(randomDouble * Math.Pow(10, Configs.DigitsAfterDot+3)) == Math.Round(randomDouble * Math.Pow(10, Configs.DigitsAfterDot))*1000,
                    $"{randomDouble} has more digits after dot than expected {Configs.DigitsAfterDot}");
            }
        }

        [TestMethod]
        public void TestLinesGeneration()
        {
            Regex tenLinesRegex = new Regex("^(([0-3]\\d[.][0-1]\\d[.]20[1-2][1-2])[|]{2}([A-Za-z]{10})[|]{2}([А-Яа-я]{11})[|]{2}(\\d{2})[|]{2}(\\d{1,2},\\d{0,5})[|]{2}\\n){10}$");
            for (int i = 0; i < NUMBER_OF_TEST_ITERATIONS; i++)
            {
                string randomLines = Randomiser.GenerateRandomLines(10, Configs);
                Assert.IsTrue(tenLinesRegex.IsMatch(randomLines), $"{randomLines} doesn't match regex {tenLinesRegex}");
            }
        }
    }
}