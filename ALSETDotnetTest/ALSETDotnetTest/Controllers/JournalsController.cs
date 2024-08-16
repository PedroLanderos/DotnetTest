using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ALSETDotnetTest.Models;

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
            var mainDBContext = _context.Journals.Include(j => j.Researcher);
            return View(await mainDBContext.ToListAsync());
        }

        // GET: Journals/Details/5
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
        public IActionResult Create()
        {
            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
            return View();
        }

        // POST: Journals/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
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
                Console.WriteLine("El modelo no es válido. Errores:");
                foreach (var error in ModelState)
                {
                    Console.WriteLine($"Clave: {error.Key}, Errores: {string.Join(", ", error.Value.Errors.Select(e => e.ErrorMessage))}");
                }
            }
            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", journal.ResearcherId);
            return View(journal);
        }


        //new create version
        [HttpPost]
        public async Task<IActionResult> CreateNew(int ResearcherId, string Description, IFormFile UploadedFile)
        {
            if (UploadedFile == null || Path.GetExtension(UploadedFile.FileName).ToLower() != ".pdf")
            {
                ModelState.AddModelError("File", "Only PDF files are allowed.");
                ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
                return View();
            }

            if (ModelState.IsValid)
            {
                // Generar el nombre y la ruta del archivo
                var fileName = Path.GetFileName(UploadedFile.FileName);
                var researcherFolder = Path.Combine("Files", ResearcherId.ToString());
                var filePath = Path.Combine(researcherFolder, fileName);

                // Crear la carpeta si no existe
                if (!Directory.Exists(researcherFolder))
                {
                    Directory.CreateDirectory(researcherFolder);
                }

                // Guardar el archivo en el servidor
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await UploadedFile.CopyToAsync(stream);
                }

                // Crear el registro del Journal en la base de datos
                var journal = new Journal
                {
                    ResearcherId = ResearcherId,
                    FileName = fileName,
                    FilePath = filePath,
                    Description = Description,
                    UploadDate = DateTime.Now
                };

                _context.Add(journal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["ResearcherId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
            return View();
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
