﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shopping.Data;
using Shopping.Data.Entities;
using Shopping.Helpers;
using Shopping.Models;
using Vereyon.Web;
using static Shopping.Helpers.ModalHelper;

namespace Shopping.Controllers
{

    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlobHelper _blobHelper;
        private readonly IFlashMessage _flashMessage;

        public ProductsController(DataContext context, ICombosHelper combosHelper, IBlobHelper blobHelper, IFlashMessage flashMessage)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blobHelper = blobHelper;
            _flashMessage = flashMessage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync());
        }


        [NoDirectAccess]
        public async Task<IActionResult> Create()
        {
            CreateProductViewModel model = new()
            {
                Categories = await _combosHelper.GetComboCategoriesAsync(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                }

                Product product = new()
                {
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price,
                    Stock = model.Stock,
                    ProductCategories = new List<ProductCategory>()
            {
                new() {
                    Category = await _context.Categories.FindAsync(model.CategoryId)
                }
            }
                };

                if (imageId != Guid.Empty)
                {
                    product.ProductImages = new List<ProductImage>()
                    {
                            new() { ImageId = imageId }
                    };
                }

                try
                {
                    _ = _context.Add(product);
                    _ = await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Registro creado.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "_ViewAllProducts", _context.Products
                        .Include(p => p.ProductImages)
                        .Include(p => p.ProductCategories)
                        .ThenInclude(pc => pc.Category).ToList())
                    });

                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        _flashMessage.Danger(string.Empty, "Ya existe un producto con el mismo nombre.");
                    }
                    else
                    {
                        _flashMessage.Danger(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(string.Empty, exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Create", model) });
        }
        [NoDirectAccess]
        public async Task<IActionResult> Edit(int id)
        {


            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            EditProductViewModel model = new()
            {
                Description = product.Description,
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Stock = product.Stock,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CreateProductViewModel model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            try
            {
                Product product = await _context.Products.FindAsync(model.Id);
                product.Description = model.Description;
                product.Name = model.Name;
                product.Price = model.Price;
                product.Stock = model.Stock;
                _ = _context.Update(product);
                _ = await _context.SaveChangesAsync();
                _flashMessage.Confirmation("Registro actualizado.");
                return Json(new
                {
                    isValid = true,
                    html = ModalHelper.RenderRazorViewToString(this, "_ViewAllProducts", _context.Products
                    .Include(p => p.ProductImages)
                    .Include(p => p.ProductCategories)
                    .ThenInclude(pc => pc.Category).ToList())
                });

            }
            catch (DbUpdateException dbUpdateException)
            {
                if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                {
                    _flashMessage.Danger(string.Empty, "Ya existe un producto con el mismo nombre.");
                }
                else
                {
                    _flashMessage.Danger(string.Empty, dbUpdateException.InnerException.Message);
                }
            }
            catch (Exception exception)
            {
                _flashMessage.Danger(string.Empty, exception.Message);
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "Edit", model) });
        }
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            return product == null ? NotFound() : View(product);
        }
        [NoDirectAccess]
        public async Task<IActionResult> AddImage(int id)
        {

            Product product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            AddProductImageViewModel model = new()
            {
                ProductId = product.Id,
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddImage(AddProductImageViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = await _blobHelper.UploadBlobAsync(model.ImageFile, "products");
                Product product = await _context.Products.FindAsync(model.ProductId);
                ProductImage productImage = new()
                {
                    Product = product,
                    ImageId = imageId,
                };

                try
                {
                    _ = _context.Add(productImage);
                    _ = await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Imagen Agregada.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _context.Products
                            .Include(p => p.ProductImages)
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .FirstOrDefaultAsync(p => p.Id == model.ProductId))
                    });

                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(string.Empty, exception.Message);
                }
            }

            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddImage", model) });
        }
        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductImage productImage = await _context.ProductImages
                .Include(pi => pi.Product)
                .FirstOrDefaultAsync(pi => pi.Id == id);
            if (productImage == null)
            {
                return NotFound();
            }

            await _blobHelper.DeleteBlobAsync(productImage.ImageId, "products");
            _ = _context.ProductImages.Remove(productImage);
            _ = await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { productImage.Product.Id });
        }

        [NoDirectAccess]
        public async Task<IActionResult> AddCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            List<Category> categories = product.ProductCategories
                .Where(pc => pc.Category != null)
                .Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name,
            }).ToList();

            AddCategoryProductViewModel model = new()
            {
                ProductId = product.Id,
                Categories = await _combosHelper.GetComboCategoriesAsync(categories),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(AddCategoryProductViewModel model)
        {
            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .FirstOrDefaultAsync(p => p.Id == model.ProductId);
            if (ModelState.IsValid)
            {

                ProductCategory productCategory = new()
                {
                    Category = await _context.Categories.FindAsync(model.CategoryId),
                    Product = product,
                };

                try
                {
                    _ = _context.Add(productCategory);
                    _ = await _context.SaveChangesAsync();
                    _flashMessage.Confirmation("Categoria agregada.");
                    return Json(new
                    {
                        isValid = true,
                        html = ModalHelper.RenderRazorViewToString(this, "Details", _context.Products
                            .Include(p => p.ProductImages)
                            .Include(p => p.ProductCategories)
                            .ThenInclude(pc => pc.Category)
                            .FirstOrDefaultAsync(p => p.Id == model.ProductId))
                    });

                }
                catch (Exception exception)
                {
                    _flashMessage.Danger(string.Empty, exception.Message);
                }
            }



            List<Category> categories = product.ProductCategories.Select(pc => new Category
            {
                Id = pc.Category.Id,
                Name = pc.Category.Name,
            }).ToList();

            model.Categories = await _combosHelper.GetComboCategoriesAsync(categories);
            return Json(new { isValid = false, html = ModalHelper.RenderRazorViewToString(this, "AddCategory", model) });
        }

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ProductCategory productCategory = await _context.ProductCategories
                .Include(pc => pc.Product)
                .FirstOrDefaultAsync(pc => pc.Id == id);
            if (productCategory == null)
            {
                return NotFound();
            }

            _ = _context.ProductCategories.Remove(productCategory);
            _ = await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Details), new { productCategory.Product.Id });
        }


        [NoDirectAccess]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _context.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.ProductImages)
                .FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            foreach (ProductImage productImage in product.ProductImages)
            {
                await _blobHelper.DeleteBlobAsync(productImage.ImageId, "products");
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _flashMessage.Info("Registro borrado.");
            return RedirectToAction(nameof(Index));
        }


    }
}