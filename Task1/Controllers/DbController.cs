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

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var linesInDb = await _dbService.GetAllAsync(_context);
            return View("Db", linesInDb);
        }

        [HttpGet("processed")]
        public IActionResult GetCreatedAmount()
        {
            int amount = _dbService.GetProcessedAmount();
            return Json(amount);
        }

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

        [HttpGet("procedure")]
        public IActionResult PerformProcedure()
        {
            var result = _dbService.PerformProcedure(_context);
            return Json(result);
        }
    }
}
