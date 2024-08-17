using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALSETDotnetTest.Models;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using ImageMagick;
using System.IO;
using Microsoft.JSInterop.Implementation;
using Microsoft.AspNetCore.Mvc.Razor.Compilation;

namespace ALSETDotnetTest.Controllers
{
    public class JournalsController : Controller
    {
        private readonly MainDBContext _context;

        public JournalsController(MainDBContext context)
        {
            _context = context;
        }

        // GET: Journals
        public async Task<IActionResult> Index()
        {
            var journals = await _context.Journals.ToListAsync();
            return Ok(journals); //change to get the response into json so the react view could interpretate the data (html was original set)
        }

        // GET: Journals/Details/5
        //endpoint created by vs to show a view from a specific value
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journals
                .Include(j => j.Researcher)
                .FirstOrDefaultAsync(m => m.JournalId == id);

            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        // GET: Journals/Create
        //endpoint created by vs
        public IActionResult Create()
        {
            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
            return View();
        }


        // POST: Journals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost] //post endpoint to get a view created by vs
        public async Task<IActionResult> Create([Bind("ResearcherId,FileName,FilePath,Description")] Journal journal)
        {
            if (ModelState.IsValid)
            {
                _context.Add(journal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                // Log si el modelo no es válido
                Console.WriteLine("Error:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Clave: {error.Key}, Error: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", journal.ResearcherId);
            return View(journal);
        }


        //new create version (also a post request)
        [HttpPost]
        public async Task<IActionResult> CreateNew(int ResearcherId, string Description, IFormFile UploadedFile)
        {
            //checks the pdf file 
            if (UploadedFile == null || Path.GetExtension(UploadedFile.FileName).ToLower() != ".pdf")
            {
                ModelState.AddModelError("File", "Only PDF files are allowed.");
                ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
                return View(); // Devuelve la vista con el mensaje de error
            }

            if (ModelState.IsValid)
            {
                //creates the name using the next structure: ((Files/id/name);
                var fileName = Path.GetFileName(UploadedFile.FileName);
                var researcherFolder = Path.Combine("Files", ResearcherId.ToString());
                var filePath = Path.Combine(researcherFolder, fileName);

                //if directory is empty then create it 
                if (!Directory.Exists(researcherFolder))
                {
                    Directory.CreateDirectory(researcherFolder);
                }

                //save the file path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }

                //creates the jounal obecjt (it's the base model) and the inserts into the database
                var journal = new Journal
                {
                    ResearcherId = ResearcherId,
                    FileName = fileName,
                    FilePath = filePath,
                    Description = Description,
                    UploadDate = DateTime.Now
                };

                _context.Add(journal);

                //everytime that a reseracher uploads a journal the the TotalJournalsUploaded increments by 1
                var researcher = await _context.Researchers.FindAsync(ResearcherId);
                if (researcher != null)
                {
                    researcher.TotalJournalsUploaded++;
                    _context.Update(researcher);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
            return View();
        }


        [HttpGet("GetFilePath/{id}")]
        public IActionResult GetFilePath(int id)
        {
            var journal = _context.Journals.Find(id);
            if (journal == null)
            {
                return NotFound();
            }

            return Ok(journal.FilePath); // Devuelve la ruta del archivo
        }


        // GET: Journals/ByResearcher/5
        [HttpGet("/Journals/GetJournalsByResearcher/{researcherId}")]
        public async Task<IActionResult> GetJournalsByResearcher(int researcherId)
        {
            var journals = await _context.Journals
                                         .Where(j => j.ResearcherId == researcherId)
                                         .ToListAsync();

            if (journals == null || journals.Count == 0)
            {
                return NotFound();
            }

            return Ok(journals);
        }


        //view the pdf
        // GET: Journals/View/5
        [HttpGet]
        public IActionResult ViewPDF(int id)
        {
            var journal = _context.Journals.Find(id);
            if (journal == null)
            {
                return NotFound();
            }

            var filePath = journal.FilePath;
            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            var contentType = "application/pdf";
            var fileName = journal.FileName;

            return File(fileStream, contentType, fileName);
        }





        // GET: Journals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journals.FindAsync(id);
            if (journal == null)
            {
                return NotFound();
            }
            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", journal.ResearcherId);
            return View(journal);
        }

        // POST: Journals/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [Bind("FileName,FilePath,Description")] Journal updateJournal)
        {
            var journal = await _context.Journals.FindAsync(id);

            if (journal == null)
            {
                return NotFound();
            }

            journal.FileName = updateJournal.FileName;
            journal.FilePath = updateJournal.FilePath;
            journal.Description = updateJournal.Description;    

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(journal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JournalExists(journal.JournalId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", journal.ResearcherId);
            return View(journal);
        }

        // GET: Journals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var journal = await _context.Journals
                .Include(j => j.Researcher)
                .FirstOrDefaultAsync(m => m.JournalId == id);
            if (journal == null)
            {
                return NotFound();
            }

            return View(journal);
        }

        // Delete: Journals/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var journal = await _context.Journals.FindAsync(id);
            if (journal != null)
            {
                _context.Journals.Remove(journal);
            }
            else
                return BadRequest();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JournalExists(int id)
        {
            return _context.Journals.Any(e => e.JournalId == id);
        }
    }
}
