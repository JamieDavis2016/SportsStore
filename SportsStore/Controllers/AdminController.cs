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
    }
}