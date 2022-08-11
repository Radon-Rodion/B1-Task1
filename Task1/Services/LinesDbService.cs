using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Task1.Data;
using Task1.Models;
using static System.Convert;

namespace Task1.Services
{
    public class LinesDbService
    {
        object locker = new object();
        private int _processedAmount = 0;
        public async Task<IEnumerable<Line>> GetAllAsync(ApplicationDbContext context)
        {
            return await context.Lines.ToListAsync();
        }

        public Line GetLineFromString(string stringifiedLine)
        {
            try
            {
                var stringifiedLineFields = stringifiedLine.Split("||"); //Line example: 03.03.2015||ZAwRbpGUiK||мДМЮаНкуКД||14152932||7,87742021||
                var dateParts = stringifiedLineFields[0].Split("."); //Date example: 30.07.2019

                var line = new Line()
                {
                    Date = new DateTime(ToInt32(dateParts[2]), ToInt32(dateParts[1]), ToInt32(dateParts[0])),
                    LatinString = stringifiedLineFields[1],
                    RussianString = stringifiedLineFields[2],
                    IntegerNumber = ToInt32(stringifiedLineFields[3]),
                    DoubleNumber = ToDouble(stringifiedLineFields[4], new CultureInfo("de-DE"))
                };
                return line;
            } catch (Exception ex){
                throw new InvalidCastException(
                    $"{stringifiedLine} doesn't match line format: 'day'.'month'.'year'||'latin string'||'russian string'||'integer'||'double'||");
            }
        }

        public void WriteStringifiedLineWithoutSaving(Line line, ApplicationDbContext context)
        {
            context.Add(line);
        }

        public async Task SaveContextAsync(ApplicationDbContext context)
        {
            await context.SaveChangesAsync();
        }

        public (int integerSum, double medianDouble) PerformProcedure(ApplicationDbContext context)
        {
            throw new NotImplementedException();
        }

        public void SaveEachLineFromFile(string fileName, ApplicationDbContext context)
        {
            Interlocked.Exchange(ref _processedAmount, 0);
            foreach (var fileLine in File.ReadAllLines(fileName))
            {
                context.Add(GetLineFromString(fileLine));
                Interlocked.Increment(ref _processedAmount);
            }
            context.SaveChanges();
        }

        public int GetProcessedAmount()
        {
            return _processedAmount;
        }
    }
}
