using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.Models;
using Task1.Services;

namespace Tests
{
    [TestClass]
    public class TestLinesDbService
    {
        private LinesDbService _service;
        public LinesDbService Service
        {
            get
            {
                if(_service == null)
                {
                    _service = new LinesDbService();
                }
                return _service;
            }
        }

        private string ConvertLineToString(Line line)
        {
            return $"{line.Date.ToString("dd.MM.yyyy")}||{line.LatinString}||{line.RussianString}||{line.IntegerNumber}||{line.DoubleNumber.ToString(new CultureInfo("de-DE"))}||\n";
        }

        private bool CompareLines(Line line1, Line line2)
        {
            return line1.Date.Year == line2.Date.Year && line1.Date.Month == line2.Date.Month && line1.Date.Day == line2.Date.Day
                && line1.LatinString == line2.LatinString && line1.RussianString == line2.RussianString
                && line1.IntegerNumber == line2.IntegerNumber && line1.DoubleNumber == line2.DoubleNumber;
        }

        [TestMethod]
        public void TestGettingLineFromString()
        {
            Line line1 = new Line()
            {
                Date = DateTime.Now,
                LatinString = "SomeLatinString",
                RussianString = "КакаяТоСтрока",
                IntegerNumber = 10873,
                DoubleNumber = 3.89213401
            };
            Line convertedLineFromString = Service.GetLineFromString(ConvertLineToString(line1));
            Assert.IsTrue(CompareLines(line1, convertedLineFromString),
                $"Invalid line conversion; Expected: {ConvertLineToString(line1)}, got: {ConvertLineToString(convertedLineFromString)}");

            Line line2 = new Line()
            {
                Id = 1,
                Date = new DateTime(2000, 1, 29),
                LatinString = "AaBbCcDdEe",
                RussianString = "АаБбВвГгЕе",
                IntegerNumber = 0x00FF_FFFF,
                DoubleNumber = 1.23456789
            };
            convertedLineFromString = Service.GetLineFromString(ConvertLineToString(line2));
            Assert.IsTrue(CompareLines(line2, convertedLineFromString),
                $"Invalid line conversion; Expected: {ConvertLineToString(line2)}, got: {ConvertLineToString(convertedLineFromString)}");

            Line line3 = new Line()
            {
                Date = new DateTime(1, 1, 1),
                LatinString = "",
                RussianString = "",
                IntegerNumber = 0,
                DoubleNumber = 0
            };
            convertedLineFromString = Service.GetLineFromString(ConvertLineToString(line3));
            Assert.IsTrue(CompareLines(line3, convertedLineFromString),
                $"Invalid line conversion; Expected: {ConvertLineToString(line3)}, got: {ConvertLineToString(convertedLineFromString)}");
        }

        [TestMethod]
        public void TestNegativeGettingLineFromString()
        {
            Assert.ThrowsException<InvalidCastException>(() =>
            {
                Service.GetLineFromString("");
            });

            Assert.ThrowsException<InvalidCastException>(() =>
            {
                Service.GetLineFromString("01.02||adfgtrew||апролдить||12345678||9.01234567||");
            });

            Assert.ThrowsException<InvalidCastException>(() =>
            {
                Service.GetLineFromString("..||adfgtrew||апролдить||12345678||9.01234567||");
            });
        }
    }
}
