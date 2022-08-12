using Microsoft.EntityFrameworkCore;
using System.Globalization;
using Task1.Data;
using Task1.Models;
using static System.Convert;

namespace Task1.Services
{
    public class LinesDbService
    {
        private int _processedAmount = 0;

        /// <summary>
        /// Returns all Lines from database
        /// </summary>
        public async Task<IEnumerable<Line>> GetAllAsync(ApplicationDbContext context)
        {
            return await context.Lines.ToListAsync();
        }

        /// <summary>
        /// Converts string representation of Line (format: RandomDate||RandomLatinString||RandomRussianString||RandomInt||RandomDouble||\n)
        /// into Line object
        /// </summary>
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

        /// <summary>
        /// Gets Lines from file 'fileName' and saves them in database
        /// </summary>
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

        /// <summary>
        /// Returns amount of lines added into database in SaveEachLineFromFile method
        /// </summary>
        public int GetProcessedAmount()
        {
            return _processedAmount;
        }
    }
}
