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
    public class ResearchersController : Controller
    {
        private readonly MainDBContext _context;

        public ResearchersController(MainDBContext context)
        {
            _context = context;
        }

        // GET: Researchers
        public async Task<IActionResult> Index()
        {
            var researchers = await _context.Researchers.ToListAsync();
            return Ok(researchers);  // Devuelve los datos en formato JSON
        }

        // GET: Researchers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researcher = await _context.Researchers
                .FirstOrDefaultAsync(m => m.ResearcherId == id);
            if (researcher == null)
            {
                return NotFound();
            }

            return Ok(researcher);
        }

        // GET: Researchers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Researchers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        // [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([FromBody] Researcher researcher)
        {
            //get the current date
            researcher.JoinedDate = DateTime.Now;
            //init the amount of journals
            researcher.TotalJournalsUploaded = 0;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Add(researcher);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al guardar en la base de datos: {ex.Message}");
                    // Puedes retornar una vista de error o manejarlo según tu necesidad
                    ModelState.AddModelError(string.Empty, "Error al guardar los datos en la base de datos.");
                }
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

            // Si hay errores, retornar la misma vista con los datos del investigador
            return View(researcher);
        }

        // GET: Researchers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researcher = await _context.Researchers.FindAsync(id);
            if (researcher == null)
            {
                return NotFound();
            }
            return View(researcher);
        }

        // PUT: Researchers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [Bind("Name")] Researcher updatedResearcher)
        {

            var researcher = await _context.Researchers.FindAsync(id);
            if (researcher == null)
            {
                return NotFound();
            }

            // Actualizar el nombre del investigador con el valor proporcionado
            researcher.Name = updatedResearcher.Name;

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(researcher);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResearcherExists(researcher.ResearcherId))
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
            return View(researcher);
        }

        // GET: Researchers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var researcher = await _context.Researchers
                .FirstOrDefaultAsync(m => m.ResearcherId == id);
            if (researcher == null)
            {
                return NotFound();
            }

            return View(researcher);
        }

        // POST: Researchers/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var researcher = await _context.Researchers.FindAsync(id);
            if (researcher != null)
            {
                _context.Researchers.Remove(researcher);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResearcherExists(int id)
        {
            return _context.Researchers.Any(e => e.ResearcherId == id);
        }
    }
}