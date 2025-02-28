using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;


namespace Shopping.Controllers
{

    [Authorize(Roles = "Admin")]
    public class CategoriesController : Controller
    {
        private readonly DataContext _context;

        public CategoriesController(DataContext context)
        {
            this._context = context;
        }


        //Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }


        // GET: Categories/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Category category)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Add(category);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una categoria con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(category);
        }
        // GET: Categories/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            Category? category = await _context.Categories.FindAsync(id);
            return category == null ? NotFound() : View(category);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Update(category);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una categoria con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }
            return View(category);
        }
        // GET: Categories/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            Category? category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            return category == null ? NotFound() : View(category);
        }

        // GET: Categories/Delete/5
        //
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            Category? category = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            return category == null ? NotFound() : View(category);
        }

        // POST: Categories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'DataContext.Countries'  is null.");
            }
            Category? category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _ = _context.Categories.Remove(category);
            }

            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }

}

