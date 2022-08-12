using Microsoft.AspNetCore.Mvc;
using Task1.Data;
using Task1.Services;

namespace Task1.Controllers
{
    [Route("/api/data")]
    public class DbController : Controller
    {
        private LinesDbService _dbService;
        private IFileService _fileService;
        private readonly ApplicationDbContext _context;

        public DbController(LinesDbService dbService, ApplicationDbContext context)
        {
            _dbService = dbService;
            _context = context;
        }

        /// <summary>
        /// Gets Lines from database and returns RazorPage with them
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var linesInDb = await _dbService.GetAllAsync(_context);
            return View("Db", linesInDb);
        }

        /// <summary>
        /// Returns amount of lines added into database from file in ImportFileToDB method
        /// </summary>
        [HttpGet("processed")]
        public IActionResult GetAddedLinesAmount()
        {
            int amount = _dbService.GetProcessedAmount();
            return Json(amount);
        }

        /// <summary>
        /// Imports Lines from file into database in a new thread. 
        /// Returns RazorPage with progressBar or redirects back if no DBContext available
        /// </summary>
        [HttpGet("import")]
        public IActionResult ImportFileToDB(string importedFileName)
        {
            if (_context != null)
            {
                Task.Run(() => _dbService.SaveEachLineFromFile(importedFileName, _context));
                return View("progressPage", System.IO.File.ReadAllLines(importedFileName).Count());
            }
            else return Redirect("/");
        }
    }
}
