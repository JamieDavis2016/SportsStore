using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SportsStore.Infrastructure;
using SportsStore.Models;
using SportsStore.Models.ViewModels;

namespace SportsStore.Controllers
{
    public class CartController : Controller
    {
        private IProductRepository ProductRepository;
        private Cart cart;

        public CartController(IProductRepository productRepo, Cart cartService)
        {
            ProductRepository = productRepo;
            cart = cartService;
        }

        public ViewResult Index(string returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                returnUrl = returnUrl
            });
        }

        public RedirectToActionResult AddToCart(int productId, string returnUrl)
        {
            var product = ProductRepository.Products
                .FirstOrDefault(x => x.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToActionResult RemoveFromCart(int productId, string returnUrl)
        {
            var product = ProductRepository.Products
                .FirstOrDefault(x => x.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }
    }
}