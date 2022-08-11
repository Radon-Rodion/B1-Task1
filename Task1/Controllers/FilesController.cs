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
        [HttpGet]
        public IActionResult GetFiles(int page=0, int filesOnPage=1000)
        {
            var files = _fileService.GetFiles((page-1)*filesOnPage, filesOnPage);
            return Json(files.ToArray());
        }

        [HttpGet("create")]
        public IActionResult CreateFilesAsync(int numberOfFiles, int linesInFile)
        {
            _fileService.CreateFilesAsync(numberOfFiles, linesInFile, _configs);
            return View("progressPage",numberOfFiles);
        }

        [HttpGet("processed")]
        public IActionResult GetCreatedAmount()
        {
            int amount = _fileService.GetProcessedAmount();
            return Json(amount);
        }

        [HttpGet("{fileName}")]
        public IActionResult GetFile(string fileName)
        {
            return View("File",_fileService.ReadFile(fileName));
        }

        [HttpGet("combine")]
        public IActionResult CombineFilesAsync(string combinedFileName, string substringForDeletion)
        {
            _fileService.CombineFilesWithDeletionAsync(combinedFileName, substringForDeletion);
            return View("combinePage", _fileService.GetFilesAmount());
        }

        [HttpGet("removed")]
        public IActionResult GetRemovedLinesAmount()
        {
            int amount = _fileService.GetDeletedLinesAmount();
            return Json(amount);
        }

        
    }
}
