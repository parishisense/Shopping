

using Shopping.Data.Entities;

namespace Shopping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;

        public SeedDb(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            _ = await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
        }

        private async Task CheckCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _ = _context.Countries.Add(new Country
                {
                    Name = "México",
                    States = new List<State>()
                    {
                        new State {
                            Name= "Michoacán",
                            Cities= new List<City>()
                            {
                                new() {Name= "Uruapan" },
                                new() {Name= "Zamora" },
                                new() {Name= "Patzcuaro" },
                                new() {Name= "Morelia" },
                                new() {Name= "Janitzio" },
                                new() {Name= "Zurumutaro" },
                            }
                        },
                        new State {
                            Name= "Oaxaca",
                            Cities= new List<City>()
                            {
                                new() {Name= "Oaxaca de Juarez" },
                                new() {Name= "Puerto Escondido" },
                                new() {Name= "Salina Cruz" },
                                new() {Name= "Santa Maria Huatulco" },
                                new() {Name= "Juchitan" },
                                new() {Name= "Huamuchil" },
                            }
                        },

                                                new State {
                            Name= "Veracruz",
                            Cities= new List<City>()
                            {
                                new() {Name= "Veracruz" },
                                new() {Name= "Tajin" },
                                new() {Name= "Boca del Rio" },
                                new() {Name= "Coatzacoalcos" },
                                new() {Name= "Catemaco" },
                                new() {Name= "Xalapa" },
                            }
                        },

                                                                        new State {
                            Name= "Yucatán",
                            Cities= new List<City>()
                            {
                                new() {Name= "Mérida" },
                                new() {Name= "Valladolid" },
                                new() {Name= "Celestún" },
                                new() {Name= "Puerto progreso" },
                                new() {Name= "Juchitan" },
                                new() {Name= "Las Coloradas" },
                            }
                        },

                    }
                });

                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new State()
                        {
                            Name="Florida",
                            Cities = new List<City>()
                            {
                                new City() { Name="Orlando"},
                                new City() { Name="Miami"},
                                new City() { Name="Tampa"},
                                new City() { Name="Fort Lauderdale"},
                                new City() { Name="Key West"},

                            }
                        },
                        new State ()
                        {
                            Name="Texas",
                            Cities= new List<City>()
                            {
                                new City() { Name="Houston"},
                                new City() { Name="San Antonio"},
                                new City() { Name="Dallas"},
                                new City() { Name="Austin"},
                                new City() { Name="El Paso"},
                            }

                    },
                        }
                });
            }

            await _context.SaveChangesAsync();
        }



        private async Task CheckCategoriesAsync()
        {
            if (!_context.Categories.Any())
            {
                _ = _context.Categories.Add(new Entities.Category { Name = "Tecnologia" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Ropa" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Calzado" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Belleza" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Nutricion" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Deportes" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Apple" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Mascotas" });

                _ = await _context.SaveChangesAsync();
            }
        }
    }
}
