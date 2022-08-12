using Task1.Configs;
using Task1.Data;

namespace Task1.Services
{
    public interface IFileService
    {
        /// <summary>
        /// Creates required number of files with required number of lines in each.
        /// Lines contain random information according to configs
        /// </summary>
        public int CreateFiles(int numberOfFiles, int linesInFile, LineRandomiserConfigs configs);

        /// <summary>
        /// Works in a new thread.
        /// Creates required number of files with required number of lines in each.
        /// Lines contain random information according to configs
        /// </summary>
        public void CreateFilesAsync(int numberOfFiles, int linesInFile, LineRandomiserConfigs configs);

        /// <summary>
        /// Returns amount of files created in CreateFilesAsync or combined in CombineFilesWithDeletionAsync
        /// </summary>
        public int GetProcessedAmount();

        /// <summary>
        /// Returns total amount of files with name fitting ^\d+[.]txt$ regexp
        /// (files processed with this program)
        /// </summary>
        public int GetFilesAmount();

        /// <summary>
        /// Returns filenames fitting ^\d+[.]txt$ regexp
        /// (files processed with this program).
        /// Skips first 'from' filenames, returns 'amount' or less filenames
        /// </summary>
        public IEnumerable<string> GetFiles(int from, int amount);

        /// <summary>
        /// Returns all text from file
        /// </summary>
        public string ReadFile(string fileName);

        /// <summary>
        /// Combines all files with name fitting ^\d[.]txt$ regexp into new one.
        /// Removes lines that contain 'substringForDeletion'.
        /// Returns number of files that were combined.
        /// </summary>
        public int CombineFilesWithDeletion(string newFileName, string substringForDeletion);

        /// <summary>
        /// Works in a new thread.
        /// Combines all files with name fitting ^\d[.]txt$ regexp into new one.
        /// Removes lines that contain 'substringForDeletion'.
        /// </summary>
        public void CombineFilesWithDeletionAsync(string newFileName, string substringForDeletion);

        /// <summary>
        /// Returns number of lines that were removed from all files in CombineFilesWithDeletion method
        /// </summary>
        public int GetDeletedLinesAmount();
    }
}
