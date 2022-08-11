using Task1.Models;
using System.IO;
using System.Text.RegularExpressions;
using System.Text;
using Task1.Configs;
using Task1.Data;

namespace Task1.Services
{
    public class LinesFileService : IFileService
    {
        private IRandomiser _randomiser;
        private int _processedAmount;
        private object locker = new object();
        public LinesFileService(IRandomiser randomiser)
        {
            _randomiser = randomiser;
        }

        public int CreateFiles(int numberOfFiles, int linesInFile, LineRandomiserConfigs configs)
        {
            Interlocked.Exchange(ref _processedAmount, 0);
            Parallel.For(0, numberOfFiles, (index) =>
            {
                File.WriteAllText($"{index}.txt", _randomiser.GenerateRandomLines(linesInFile, configs));
                Interlocked.Increment(ref _processedAmount);
            });
            return numberOfFiles;
        }

        public void  CreateFilesAsync(int numberOfFiles, int linesInFile, LineRandomiserConfigs configs)
        {
            (new Task<int>(() =>
            {
                return CreateFiles(numberOfFiles, linesInFile, configs);
            })).Start();
        }

        public int GetProcessedAmount()
        {
            return _processedAmount;
        }

        private IEnumerable<string> GetAllFiles()
        {
            var nameRegularExpression = new Regex("^[.][\\\\]\\d+[.]txt$");
            return Directory.GetFiles(".").Where((name) => nameRegularExpression.IsMatch(name));
        }

        public int GetFilesAmount()
        {
            return GetAllFiles().Count();
        }

        public IEnumerable<string> GetFiles(int from, int amount)
        {
            var fileNames = GetAllFiles().Skip(from).Take(amount);
            return fileNames;
        }

        public string ReadFile(string fileName)
        {
            if (File.Exists(fileName))
            {
                return File.ReadAllText(fileName);
            }
            return null;
        }

        private int _deletedLines;
        public int CombineFilesWithDeletion(string newFileName, string substringForDeletion)
        {
            StringBuilder builder = new StringBuilder();
            Interlocked.Exchange(ref _processedAmount, 0);
            Interlocked.Exchange(ref _deletedLines, 0);
            var filesToCombine = GetAllFiles();

            Parallel.ForEach(filesToCombine, (fileName) =>
            {
                StringBuilder localBuilder = new StringBuilder();
                foreach (var line in File.ReadLines(fileName))
                {
                    if (!line.Contains(substringForDeletion))
                    {
                        localBuilder.AppendLine(line);
                    }
                    else
                    {
                        Interlocked.Increment(ref _deletedLines);
                    }
                }
                File.WriteAllText(fileName, localBuilder.ToString());
                lock (locker)
                {
                    builder.Append(localBuilder.ToString());
                }
                Interlocked.Increment(ref _processedAmount);
            });
            File.WriteAllText(newFileName, builder.ToString());

            return filesToCombine.Count();
        }

        public void CombineFilesWithDeletionAsync(string newFileName, string substringForDeletion)
        {
            (new Task<int>(() =>
            {
                return CombineFilesWithDeletion(newFileName, substringForDeletion);
            })).Start();
        }

        public int GetDeletedLinesAmount()
        {
            return _deletedLines;
        }
    }
}
