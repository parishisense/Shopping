using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Models;

namespace Shopping.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CountriesController : Controller
    {
        private readonly DataContext _context;

        public CountriesController(DataContext context)
        {
            _context = context;
        }

        // GET: Countries
        //Index

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return _context.Countries != null ?
                        View(await _context.Countries
                        .Include(c => c.States)
                        .ToListAsync()) :
                        Problem("Entity set 'DataContext.Countries'  is null.");
        }

        // GET: Countries/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            Country? country = await _context.Countries
                .Include(c => c.States)
                .ThenInclude(c => c.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            return country == null ? NotFound() : View(country);
        }

        // GET: Countries/Create
        [HttpGet]
        public IActionResult Create()
        {
            Country country = new()
            {
                States = new List<State>()
            };
            return View(country);
        }



        // POST: Countries/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Country country)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Add(country);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
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
            return View(country);
        }



        // GET: Countries/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            Country? country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(c => c.Id == id);

            return country == null ? NotFound() : View(country);
        }

        // POST: Countries/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Country country)
        {
            if (id != country.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _ = _context.Update(country);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un país con el mismo nombre.");
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
            return View(country);
        }

        // GET: Countries/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            Country? country = await _context.Countries
                .Include(c => c.States)
                .FirstOrDefaultAsync(m => m.Id == id);
            return country == null ? NotFound() : View(country);
        }

        // POST: Countries/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Countries == null)
            {
                return Problem("Entity set 'DataContext.Countries'  is null.");
            }
            Country? country = await _context.Countries.FindAsync(id);
            if (country != null)
            {
                _ = _context.Countries.Remove(country);
            }

            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        //Controlador para los ESTADOS, ESTADOS

        // GET: States/Create
        [HttpGet]
        public async Task<IActionResult> AddState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            Country country = await _context.Countries.FindAsync(id);
            if (country == null)
            {
                return NotFound();
            }
            StateViewModel model = new()
            {
                CountryId = country.Id,
            };
            return View(model);
        }

        // POST: States/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddState(StateViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Cities = new List<City>(),
                        Country = await _context.Countries.FindAsync(model.CountryId),
                        Name = model.Name,
                    };

                    _ = _context.Add(state);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.CountryId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un Departamento o Estado con el mismo nombre.");
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
            return View(model);
        }



        // GET: States/Edit/5
        public async Task<IActionResult> EditState(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            State? state = await _context.States
                .Include(s => s.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (state == null)
            {
                return NotFound();
            }
            StateViewModel model = new()
            {
                CountryId = state.Country.Id,
                Id = state.Id,
                Name = state.Name,
            };
            return View(model);



        }

        // POST: States/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditState(int id, StateViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    State state = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _ = _context.Update(state);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Details), new { Id = model.CountryId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un Departamento / Estado con el mismo nombre en este País.");
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
            return View(model);
        }

        // GET: States/Details/5
        [HttpGet]
        public async Task<IActionResult> DetailsState(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            State? state = await _context.States
                .Include(s => s.Country)
                .Include(s => s.Cities)
                .FirstOrDefaultAsync(m => m.Id == id);
            return state == null ? NotFound() : View(state);
        }

        // GET: CITY/Create
        [HttpGet]
        public async Task<IActionResult> AddCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            State state = await _context.States.FindAsync(id);
            if (state == null)
            {
                return NotFound();
            }
            CityViewModel model = new()
            {
                StateId = state.Id,
            };
            return View(model);
        }

        // POST: CITY/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCity(CityViewModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        State = await _context.States.FindAsync(model.StateId),
                        Name = model.Name,
                    };

                    _ = _context.Add(city);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsState), new { Id = model.StateId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Ciudad con el mismo nombre en este departamento o estado.");
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
            return View(model);
        }

        // GET: CITY/Edit/5
        public async Task<IActionResult> EditCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City? city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(c => c.Id == id);
            if (city == null)
            {
                return NotFound();
            }
            CityViewModel model = new()
            {
                StateId = city.State.Id,
                Id = city.Id,
                Name = city.Name,
            };
            return View(model);



        }

        // POST: States/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCity(int id, CityViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    City city = new()
                    {
                        Id = model.Id,
                        Name = model.Name,
                    };
                    _ = _context.Update(city);
                    _ = await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(DetailsState), new { Id = model.StateId });
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe una Ciudad con el mismo nombre en este Estado.");
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
            return View(model);
        }


        // GET: CITY/Details/5
        [HttpGet]
        public async Task<IActionResult> DetailsCity(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            City? city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(c => c.Id == id);


            return city == null ? NotFound() : View(city);
        }

        // GET: STATES/Delete/5
        public async Task<IActionResult> DeleteState(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            State? state = await _context.States
                .Include(c => c.Country)
                .FirstOrDefaultAsync(s => s.Id == id);
            return state == null ? NotFound() : View(state);
        }

        // POST: STATES/Delete/5
        [HttpPost, ActionName("DeleteState")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStateConfirmed(int id)
        {

            State? state = await _context.States
    .Include(c => c.Country)
    .FirstOrDefaultAsync(s => s.Id == id);

            _ = _context.States.Remove(state);

            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = state.Country.Id });
        }

        // GET: CITY/Delete/5
        public async Task<IActionResult> DeleteCity(int? id)
        {
            if (id == null || _context.Countries == null)
            {
                return NotFound();
            }

            City? city = await _context.Cities
                .Include(c => c.State)
                .FirstOrDefaultAsync(s => s.Id == id);
            return city == null ? NotFound() : View(city);
        }

        // POST: CITY/Delete/5
        [HttpPost, ActionName("DeleteCity")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCityConfirmed(int id)
        {

            City? city = await _context.Cities
    .Include(s => s.State)
    .FirstOrDefaultAsync(c => c.Id == id);

            _ = _context.Cities.Remove(city);

            _ = await _context.SaveChangesAsync();
            return RedirectToAction(nameof(DetailsState), new { id = city.State.Id });
        }
    }


}
