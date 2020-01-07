using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;

namespace SportsStore.Controllers
{
    public class AdminController : Controller
    {
        private IProductRepository ProductRepository;

        public AdminController(IProductRepository productRepository)
        {
            ProductRepository = productRepository;
        }

        public ViewResult Index() => View(ProductRepository.Products);

        public ViewResult Edit(int productId) => View(ProductRepository.Products.FirstOrDefault(x => x.ProductID == productId));

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if(ModelState.IsValid)
            {
                ProductRepository.SaveProduct(product);
                TempData["message"] = $"{product.Name} has been saved";
                return RedirectToAction("Index");
            } else
            {
                return View(product);
            }
        }

    }
}