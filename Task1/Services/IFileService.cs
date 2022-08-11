using Task1.Configs;
using Task1.Data;

namespace Task1.Services
{
    public interface IFileService
    {
        public int CreateFiles(int numberOfFiles, int linesInFile, LineRandomiserConfigs configs);
        public void CreateFilesAsync(int numberOfFiles, int linesInFile, LineRandomiserConfigs configs);
        public int GetProcessedAmount();
        public int GetFilesAmount();
        public IEnumerable<string> GetFiles(int from, int amount);
        public string ReadFile(string fileName);
        public int CombineFilesWithDeletion(string newFileName, string substringForDeletion);
        public void CombineFilesWithDeletionAsync(string newFileName, string substringForDeletion);
        public int GetDeletedLinesAmount();
    }
}
