

using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Helpers;

namespace Shopping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;

        public SeedDb(DataContext context, IUserHelper userHelper)
        {
            _context = context;
            _userHelper = userHelper;
        }

        public async Task SeedAsync()
        {
            _ = await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("0001", "Paris", "Bravo", "paris@yopmail.com", "56581111", "Calle Siempre Viva", UserType.Admin);
            await CheckUserAsync("0002", "Graciela", "Garcia", "graciela@yopmail.com", "56581112", "Calle Siempre Viva", UserType.User);

        }

        private async Task<User> CheckUserAsync(

            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            UserType userType)


        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    City = _context.Cities.FirstOrDefault(),
                    UserType = userType,
                };

                await _userHelper.AddUserAsync(user, "123456");
                await _userHelper.AddUserToRoleAsync(user, userType.ToString());
                string token = await _userHelper.GenerateEmailConfirmationTokenAsync(user);
                await _userHelper.ConfirmEmailAsync(user, token);

            }
            return user;
        }

        private async Task CheckRolesAsync()
        {
            await _userHelper.CheckRoleAsync(UserType.Admin.ToString());
            await _userHelper.CheckRoleAsync(UserType.User.ToString());

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
                        new() {
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
                        new() {
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

                                                new() {
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

                                                                        new() {
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

                _ = _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    States = new List<State>()
                    {
                        new()
                        {
                            Name="Florida",
                            Cities = new List<City>()
                            {
                                new() { Name="Orlando"},
                                new() { Name="Miami"},
                                new() { Name="Tampa"},
                                new() { Name="Fort Lauderdale"},
                                new() { Name="Key West"},

                            }
                        },
                        new()
                        {
                            Name="Texas",
                            Cities= new List<City>()
                            {
                                new() { Name="Houston"},
                                new() { Name="San Antonio"},
                                new() { Name="Dallas"},
                                new() { Name="Austin"},
                                new() { Name="El Paso"},
                            }

                    },
                        }
                });
            }

            _ = await _context.SaveChangesAsync();
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
