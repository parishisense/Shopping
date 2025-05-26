

using Microsoft.EntityFrameworkCore;
using Shopping.Data.Entities;
using Shopping.Enums;
using Shopping.Helpers;

namespace Shopping.Data
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUserHelper _userHelper;
        private readonly IBlobHelper _blobHelper;

        public SeedDb(DataContext context, IUserHelper userHelper, IBlobHelper blobHelper)
        {
            _context = context;
            _userHelper = userHelper;
            _blobHelper = blobHelper;
        }

        public async Task SeedAsync()
        {
            _ = await _context.Database.EnsureCreatedAsync();
            await CheckCategoriesAsync();
            await CheckCountriesAsync();
            await CheckRolesAsync();
            await CheckProductsAsync();
            await CheckUserAsync("0001", "Paris", "Bravo", "paris@yopmail.com", "56581111", "Calle Siempre Viva", "YOPE2.jpg", UserType.Admin);
            await CheckUserAsync("0002", "Graciela", "Garcia", "graciela@yopmail.com", "56581112", "Calle Siempre Viva", "Chelita1.jpg", UserType.User);
            await CheckUserAsync("0003", "Juliet", "Torres", "juliet@yopmail.com", "322 311 8880", "Calle Luna Calle Sol", "Yuliett.jpeg", UserType.Admin);
            await CheckUserAsync("0004", "Chelita", "Garcia", "chelita@yopmail.com", "322 311 66620", "Calle Luna Calle Sol", "Chela.jpg", UserType.User);
            await CheckUserAsync("0005", "Ariel", "Acevedo", "Ariel@yopmail.com", "322 311 3330", "Calle Luna Calle Sol", "Ariel.jpg", UserType.User);
            await CheckUserAsync("0006", "Miguel", "Vichy", "miguel@yopmail.com", "322 311 1110", "Calle Luna Calle Sol", "miguel.jpg", UserType.User);

        }

        private async Task CheckProductsAsync()
        {
            if (!_context.Products.Any())
            {
                await AddProductAsync("Alimento Proplan", 270M, 12F, new List<string>() { "Mascotas", "Nutricion" }, new List<string>() { "AliemntoPP2jpeg.jpeg" });
                await AddProductAsync("Alimento Royal Canin", 250M, 13F, new List<string>() { "Mascotas", "Nutricion" }, new List<string>() { "AlimentoPP 1.jpeg" });
                await AddProductAsync("Changan CS55", 550000M, 7F, new List<string>() { "Tecnología", "Autos" }, new List<string>() { "Changan1.jpeg", "Changan2.jpeg" });
                await AddProductAsync("Telefono Huawei Mate XE", 45000M, 12F, new List<string>() { "Tecnología", "Huawei" }, new List<string>() { "Mate XE.jpeg", "Mate XE2.jpeg" });
                await AddProductAsync("Mini Cooper 2025", 975000M, 6F, new List<string>() { "Tecnología", "Autos" }, new List<string>() { "MiniCooper.jpeg", "MiniCooper2.jpeg" });
                await AddProductAsync("Tinte de Cabello", 120M, 200F, new List<string>() { "Belleza" }, new List<string>() { "Tinte.jpeg", "Tinte2.jpeg" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task AddProductAsync(string name, decimal price, float stock, List<string> categories, List<string> images)
        {
            Product prodcut = new()
            {
                Description = name,
                Name = name,
                Price = price,
                Stock = stock,
                ProductCategories = new List<ProductCategory>(),
                ProductImages = new List<ProductImage>()
            };

            foreach (string? category in categories)
            {
                prodcut.ProductCategories.Add(new ProductCategory { Category = await _context.Categories.FirstOrDefaultAsync(c => c.Name == category) });
            }


            foreach (string? image in images)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\products\\{image}", "products");
                prodcut.ProductImages.Add(new ProductImage { ImageId = imageId });
            }

            _context.Products.Add(prodcut);
        }

        private async Task<User> CheckUserAsync(

            string document,
            string firstName,
            string lastName,
            string email,
            string phone,
            string address,
            string image,
            UserType userType)


        {
            User user = await _userHelper.GetUserAsync(email);
            if (user == null)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync($"{Environment.CurrentDirectory}\\wwwroot\\images\\users\\{image}", "users");
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
                    ImageId = imageId
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
                        new() {
                            Name= "Distrito Federal",
                            Cities= new List<City>()
                            {
                                new() {Name= "Magdalena Contreras" },
                                new() {Name= "Cuajimalpa" },
                                new() {Name= "Tlahuac" },
                                new() {Name= "Iztapalacra" },
                                new() {Name= "Xochimilco" },
                                new() {Name= "Miguel Hidalgo" },
                            }
                        },
                        new() {
                            Name= "Nuevo Leon",
                            Cities= new List<City>()
                            {
                                new() {Name= "Monterrey" },
                                new() {Name= "Salinas Victoria" },
                                new() {Name= "San Nicolas de los Garza" },
                                new() {Name= "Apodaca" },
                                new() {Name= "Guadalpue" },
                                new() {Name= "Linares" },
                            }
                        },
                        new() {
                            Name= "Jalisco",
                            Cities= new List<City>()
                            {
                                new() {Name= "Guadalajara" },
                                new() {Name= "Puerto Vallarta" },
                                new() {Name= "Tequila" },
                                new() {Name= "Arandas" },
                                new() {Name= "Zapotlán" },
                                new() {Name= "Gomez Farías" },
                            }
                        },
                    }
                });

                _ = _context.Countries.Add(new Country
                {
                    Name = "Alemania",
                    States = new List<State>()
                    {
                        new() {
                            Name= "Berlin",
                            Cities= new List<City>()
                            {
                                new() {Name= "Mitte" },
                                new() {Name= "Pankow" },
                                new() {Name= "Spandau" },
                                new() {Name= "Cichtenberg" },
                                new() {Name= "Reinickendorf" },
                                new() {Name= "Tempelhof" },
                            }
                        },
                        new() {
                            Name= "Hamburgo",
                            Cities= new List<City>()
                            {
                                new() {Name= "Ottensen" },
                                new() {Name= "Blankenese" },
                                new() {Name= "Eimsbüttel" },
                                new() {Name= "Hamburgesa" },
                                new() {Name= "Eppendorf" },
                                new() {Name= "Rotherbaum" },
                            }
                        },

                                                new() {
                            Name= "Hesse",
                            Cities= new List<City>()
                            {
                                new() {Name= "Fráncfort del Meno" },
                                new() {Name= "Wiesbaden" },
                                new() {Name= "Darmstadt" },
                                new() {Name= "Kassel " },
                                new() {Name= "Offenbach" },
                                new() {Name= "Wetzlar" },
                            }
                        },

                        new() {
                            Name= "Brandenburg",
                            Cities= new List<City>()
                            {
                                new() {Name= "Altlandsberg" },
                                new() {Name= "Perleberg" },
                                new() {Name= "Wiesenburg" },
                                new() {Name= "Buckow" },
                                new() {Name= "Ziesar" },
                                new() {Name= "Uebigau" },
                            }
                        },
                    }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    States = new List<State>()
            {
                new State()
                {
                    Name = "Antioquia",
                    Cities = new List<City>() {
                        new City() { Name = "Medellín" },
                        new City() { Name = "Itagüí" },
                        new City() { Name = "Envigado" },
                        new City() { Name = "Bello" },
                        new City() { Name = "Sabaneta" },
                        new City() { Name = "La Ceja" },
                        new City() { Name = "La Union" },
                        new City() { Name = "La Estrella" },
                        new City() { Name = "Copacabana" },
                    }
                },
                new State()
                {
                    Name = "Bogotá",
                    Cities = new List<City>() {
                        new City() { Name = "Usaquen" },
                        new City() { Name = "Champinero" },
                        new City() { Name = "Santa fe" },
                        new City() { Name = "Usme" },
                        new City() { Name = "Bosa" },
                    }
                },
                new State()
                {
                    Name = "Valle",
                    Cities = new List<City>() {
                        new City() { Name = "Calí" },
                        new City() { Name = "Jumbo" },
                        new City() { Name = "Jamundí" },
                        new City() { Name = "Chipichape" },
                        new City() { Name = "Buenaventura" },
                        new City() { Name = "Cartago" },
                        new City() { Name = "Buga" },
                        new City() { Name = "Palmira" },
                    }
                },
                new State()
                {
                    Name = "Santander",
                    Cities = new List<City>() {
                        new City() { Name = "Bucaramanga" },
                        new City() { Name = "Málaga" },
                        new City() { Name = "Barrancabermeja" },
                        new City() { Name = "Rionegro" },
                        new City() { Name = "Barichara" },
                        new City() { Name = "Zapatoca" },
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
                _ = _context.Categories.Add(new Entities.Category { Name = "Gamer" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Calzado" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Belleza" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Nutricion" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Deportes" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Apple" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Mascotas" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Autos" });
                _ = _context.Categories.Add(new Entities.Category { Name = "Huawei" });

                _ = await _context.SaveChangesAsync();
            }
        }
    }
}
