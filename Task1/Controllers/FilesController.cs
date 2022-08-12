using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Task1.Configs;
using Task1.Data;
using Task1.Services;

namespace Task1.Controllers
{
    [Route("/api/files")]
    public class FilesController : Controller
    {
        private IFileService _fileService;
        private LineRandomiserConfigs _configs;

        public FilesController(IFileService fileService, IOptions<LineRandomiserConfigs> options)
        {
            _fileService = fileService;
            _configs = options.Value;
        }

        /// <summary>
        /// Returns all fileNames fitting ^\d+[.]txt$ regex 
        /// (or required amount of them according to pagination)
        /// </summary>
        [HttpGet]
        public IActionResult GetFiles(int page=0, int filesOnPage=1000)
        {
            var files = _fileService.GetFiles((page-1)*filesOnPage, filesOnPage);
            return Json(files.ToArray());
        }

        /// <summary>
        /// Works in a new thread.
        /// Creates files required amount of files with required amount of random Lines in each.
        /// Returns RazorPage with ProgressBar
        /// </summary>
        [HttpGet("create")]
        public IActionResult CreateFilesAsync(int numberOfFiles, int linesInFile)
        {
            _fileService.CreateFilesAsync(numberOfFiles, linesInFile, _configs);
            return View("progressPage",numberOfFiles);
        }

        /// <summary>
        /// Returns amount of files created in CreateFilesAsync method
        /// or combined in CombineFilesAsync method
        /// </summary>
        [HttpGet("processed")]
        public IActionResult GetProcessedAmount()
        {
            int amount = _fileService.GetProcessedAmount();
            return Json(amount);
        }

        /// <summary>
        /// Returns RazorPage with all text from file in textArea
        /// </summary>
        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            return View("File",_fileService.ReadFile(fileName));
        }

        /// <summary>
        /// Combines all files with name fitting ^\d+[.]txt$ regex into new file 'combinedFileName',
        /// removing all lines that contain 'substringForDeletion'.
        /// Returns RazorPage with ProgressBar
        /// </summary>
        [HttpGet("combine")]
        public IActionResult CombineFilesAsync(string combinedFileName, string substringForDeletion)
        {
            _fileService.CombineFilesWithDeletionAsync(combinedFileName, substringForDeletion);
            return View("combinePage", _fileService.GetFilesAmount());
        }

        /// <summary>
        /// Returns amount of lines removed in CombineFilesAsync method
        /// </summary>
        [HttpGet("removed")]
        public IActionResult GetRemovedLinesAmount()
        {
            int amount = _fileService.GetDeletedLinesAmount();
            return Json(amount);
        }

        
    }
}
