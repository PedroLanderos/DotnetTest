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
    public class SubscriptionsController : Controller
    {
        private readonly MainDBContext _context;

        public SubscriptionsController(MainDBContext context)
        {
            _context = context;
        }

        // GET: Subscriptions
        public async Task<IActionResult> Index()
        {
            var mainDBContext = _context.Subscriptions.Include(s => s.SubscribedTo).Include(s => s.Subscriber);
            return View(await mainDBContext.ToListAsync());
        }

        // GET: Subscriptions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.SubscribedTo)
                .Include(s => s.Subscriber)
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        // GET: Subscriptions/Create
        public IActionResult Create()
        {
            ViewData["SubscribedToId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
            ViewData["SubscriberId"] = new SelectList(_context.Researchers, "ResearcherId", "Name");
            return View();
        }

        // POST: Subscriptions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] Subscription subscription)
        {
            if (ModelState.IsValid)
            {
                _context.Add(subscription);
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
            ViewData["SubscribedToId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", subscription.SubscribedToId);
            ViewData["SubscriberId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", subscription.SubscriberId);
            return View(subscription);
        }


        // GET: Subscriptions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription == null)
            {
                return NotFound();
            }
            ViewData["SubscribedToId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", subscription.SubscribedToId);
            ViewData["SubscriberId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", subscription.SubscriberId);
            return View(subscription);
        }

        // POST: Subscriptions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPut]
        public async Task<IActionResult> Edit(int id, [Bind("SubscriberId,SubscribedToId")] Subscription subscription)
        {
            if (id != subscription.SubscriptionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(subscription);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SubscriptionExists(subscription.SubscriptionId))
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
            ViewData["SubscribedToId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", subscription.SubscribedToId);
            ViewData["SubscriberId"] = new SelectList(_context.Researchers, "ResearcherId", "Name", subscription.SubscriberId);
            return View(subscription);
        }

        // GET: Subscriptions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var subscription = await _context.Subscriptions
                .Include(s => s.SubscribedTo)
                .Include(s => s.Subscriber)
                .FirstOrDefaultAsync(m => m.SubscriptionId == id);
            if (subscription == null)
            {
                return NotFound();
            }

            return View(subscription);
        }

        [HttpGet("/Subscriptions/IsSubscribed/{subscriberId}/{subscribedToId}")]
        public async Task<IActionResult> IsSubscribed(int subscriberId, int subscribedToId)
        {
            var subscriptionExists = await _context.Subscriptions
                .AnyAsync(s => s.SubscriberId == subscriberId && s.SubscribedToId == subscribedToId);

            if (subscriptionExists)
            {
                return Ok();  // El usuario está suscrito, código de estado 200
            }

            return BadRequest();  // El usuario no está suscrito, código de estado 400
        }


        // POST: Subscriptions/Delete/5
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var subscription = await _context.Subscriptions.FindAsync(id);
            if (subscription != null)
            {
                _context.Subscriptions.Remove(subscription);
            }
            else
                return BadRequest();

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SubscriptionExists(int id)
        {
            return _context.Subscriptions.Any(e => e.SubscriptionId == id);
        }
    }
}
