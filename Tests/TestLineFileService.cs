using System.Text.RegularExpressions;
using Task1.Configs;
using Task1.Services;
using static Tests.MockData;
using System.IO;
using System.Text;
using Task1.Data;

namespace Tests
{
    [TestClass]
    public class TestLineFileService
    {
        private LinesFileService _service;
        public LinesFileService Service
        {
            get
            {
                if (_service == null)
                {
                    var randomiser = new LineRandomiser();
                    _service = new LinesFileService(randomiser);
                }
                return _service;
            }
        }

        private IEnumerable<string> GetSearchedFilesNames()
        {
            var searchedFilenameRegex = new Regex("^[.][\\\\]\\d+[.]txt$");
            return Directory.GetFiles(".").Where((name) => searchedFilenameRegex.IsMatch(name));
        }
        private void RemoveCreatedFiles()
        {
            foreach (var fileToRemove in GetSearchedFilesNames())
            {
                File.Delete(fileToRemove);
            }
        }

        [TestMethod]
        public void TestFilesCreation()
        {
            int filesToCreate = 10;
            int linesInFile = 1000;

            RemoveCreatedFiles();
            Assert.IsTrue(GetSearchedFilesNames().Count() == 0, $"Expected all files to be deleted, but found {GetSearchedFilesNames().Count()} files!");

            Service.CreateFiles(filesToCreate, linesInFile, Configs);
            Assert.IsTrue(GetSearchedFilesNames().Count() == 10, $"Expected {filesToCreate} files to be created, but found {GetSearchedFilesNames().Count()}!");

            foreach(var file in GetSearchedFilesNames())
            {
                string fileContent = File.ReadAllText(file);
                int lines = fileContent.Count((character) => character == '\n');
                Assert.IsTrue(lines == linesInFile, $"Expected {linesInFile} lines in {file}, but found {lines}!");
            }
        }

        [TestMethod]
        public void TestGettingFiles()
        {
            RemoveCreatedFiles();
            Service.CreateFiles(10, 1000, Configs);

            int expectedFilesAmount = GetSearchedFilesNames().Count();
            int gotFilesAmount = Service.GetFilesAmount();
            Assert.IsTrue(gotFilesAmount == expectedFilesAmount, $"Expected: {expectedFilesAmount}, got: {gotFilesAmount} files amount!");

            var expectedFiles = GetSearchedFilesNames();
            var gotFiles = Service.GetFiles(0, gotFilesAmount);

            Assert.IsTrue(gotFiles.Count() == expectedFiles.Count(), $"Expected: {expectedFiles.Count()} files, got: {gotFiles.Count()} files!");
            foreach(var expectedFile in expectedFiles)
            {
                Assert.IsTrue(gotFiles.Contains(expectedFile), $"Got files don't contain expected file: {expectedFile}");
            }

            gotFiles = Service.GetFiles(0, 1);
            Assert.IsTrue(gotFiles.Count() == 1, $"Expected to get 1 file, but got ${gotFiles.Count()}!");

            RemoveCreatedFiles();
            gotFilesAmount = Service.GetFilesAmount();
            Assert.IsTrue(gotFilesAmount == 0, $"All files were deleted, but service returned {gotFilesAmount} files amount!");
        }

        [TestMethod]
        public void TestReadingFile()
        {
            RemoveCreatedFiles();
            int filesToCreate = 10;
            Service.CreateFiles(filesToCreate, 3, Configs);
            
            for(int i = 0; i < filesToCreate; i++)
            {
                var expectedLines = File.ReadAllText($"./{i}.txt");
                var gotLines = Service.ReadFile($"./{i}.txt");

                Assert.AreEqual(expectedLines, gotLines, "Invalid reading file!");
            }

            var linesFromAbsentFile = Service.ReadFile($"./-999.txt");
            Assert.IsNull(linesFromAbsentFile, $"Expected null, but got: {linesFromAbsentFile}");
        }

        [TestMethod]
        public void TestCombiningFiles()
        {
            RemoveCreatedFiles();
            int filesToCreate = 5;
            int linesInFile = 1000;
            string combinedFileName = "./Combined.txt";
            string substringForDeletion = "0.0";

            Service.CreateFiles(filesToCreate, linesInFile, Configs);
            Service.CombineFilesWithDeletion(combinedFileName, substringForDeletion);
            int totalLines = 0;
            for(int i = 0; i < filesToCreate; i++)
            {
                var fileLines = File.ReadLines($"./{i}.txt");
                int fileLinesAmount = fileLines.Count();
                Assert.IsTrue(fileLinesAmount > 0, $"Expected some lines to remain in file {i}.txt after deletion!");
                Assert.IsTrue(fileLinesAmount < linesInFile, $"Expected some lines to be removed from file {i}.txt!");

                foreach(var line in fileLines)
                {
                    if (line.Contains(substringForDeletion))
                    {
                        Assert.Fail($"Line: {line} in file {i}.txt contains {substringForDeletion} that had to be deleted!");
                    }
                }

                totalLines += fileLinesAmount;
            }
            int linesInCombinedFile = File.ReadLines(combinedFileName).Count();
            Assert.AreEqual(totalLines, linesInCombinedFile, $"Expected {totalLines} to be in combined file, but found {linesInCombinedFile}");
            Assert.AreEqual(linesInFile*filesToCreate - totalLines, Service.GetDeletedLinesAmount(), "Incorrect information about number of deleted lines!");

            File.Delete(combinedFileName);
        }
    }
}
