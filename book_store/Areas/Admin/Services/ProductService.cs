using book_store.Areas.Admin.Services.IServices;
using book_store.DataAccess.Repository.IRepository;
using book_store.Models;
using book_store.Models.ViewModels;
using book_store.Utility;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Collections;

namespace book_store.Areas.Admin.Services
{
    public class ProductService : IProductService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _unitOfWork.Product.GetAll(includeProperties:"Category");
        }

        public ServiceResult CreateProduct(ProductVM productVM, List<IFormFile> files)
        {
            try
            {
                _unitOfWork.Product.Add(productVM.Product);
                _unitOfWork.Save();

                // save product image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (var file in files) { 
                        // Save each file in folder
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string path = Path.Combine(wwwRootPath, productPath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (var fileSteam = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileSteam);
                        }

                        // add product image
                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }
                        productVM.Product.ProductImages.Add(productImage);
                    }

                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }

                return ServiceResult.Ok("Created product successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to create product: {ex.Message}");
            }
        }

        public ServiceResult<Product> GetProductById(int? productId, string? includeProperties = null, bool tracked = false)
        {
            if (productId < 0) return ServiceResult<Product>.Fail("id must be positive number");

            try
            {
                var product = _unitOfWork.Product.Get(p => p.Id == productId, includeProperties, tracked);
                if (product == null) return ServiceResult<Product>.Fail("id not found");

                return ServiceResult<Product>.Ok(product, "Success to get product");
            }
            catch (Exception ex)
            {
                return ServiceResult<Product>.Fail($"Fail to retrive product: {ex.Message}");
            }
        }

        public ServiceResult UpdateProduct(ProductVM productVM, List<IFormFile> files)
        {
            try
            {
                _unitOfWork.Product.Update(productVM.Product);
                _unitOfWork.Save();

                // save product image
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (files != null)
                {
                    foreach (var file in files)
                    {
                        // Save each file in folder
                        string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                        string productPath = @"images\products\product-" + productVM.Product.Id;
                        string path = Path.Combine(wwwRootPath, productPath);
                        if (!Directory.Exists(path))
                        {
                            Directory.CreateDirectory(path);
                        }

                        using (var fileSteam = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileSteam);
                        }

                        // add product image
                        ProductImage productImage = new()
                        {
                            ImageUrl = @"\" + productPath + @"\" + fileName,
                            ProductId = productVM.Product.Id,
                        };

                        if (productVM.Product.ProductImages == null)
                        {
                            productVM.Product.ProductImages = new List<ProductImage>();
                        }
                        productVM.Product.ProductImages.Add(productImage);
                    }

                    _unitOfWork.Product.Update(productVM.Product);
                    _unitOfWork.Save();
                }

                return ServiceResult.Ok("Update product successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to update product: {ex.Message}");
            }
        }

        public ServiceResult DeleteProduct(int? id)
        {
            try
            {
                var product = _unitOfWork.Product.Get(p => p.Id == id, tracked: true);
                if (product == null) return ServiceResult.Fail($"Product Id {id} not found.");

                // delete image file
                string productPath = @"images\products\product-" + id;
                string path = Path.Combine(_webHostEnvironment.WebRootPath, productPath); // eg. ProjectName\wwwroot\images\product
                if (Directory.Exists(path))
                {
                    string[] filePaths = Directory.GetFiles(path);
                    foreach (string filePath in filePaths)
                    {
                        System.IO.File.Delete(filePath);
                    }
                    Directory.Delete(path);
                }

                _unitOfWork.Product.Remove(product);
                _unitOfWork.Save();
                return ServiceResult.Ok("Delete product successfully.");
            }
            catch (Exception ex)
            {
                return ServiceResult.Fail($"Fail to delete product: {ex.Message}");
            }
        }
    }
}
