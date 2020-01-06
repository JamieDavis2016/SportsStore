using Microsoft.AspNetCore.Mvc;
using SportsStore.Models;
using System.Linq;

namespace SportsStore.Controllers
{
    public class OrderController : Controller
    {
        private IOrderRepository OrderRepository;
        private Cart Cart;
        public OrderController(IOrderRepository orderRepository, Cart cartService)
        {
            OrderRepository = orderRepository;
            Cart = cartService;
        }

        public ViewResult Checkout() => View(new Order());

        [HttpPost]
        public IActionResult Checkout(Order order)
        {
            if (Cart.Lines.Count() < 0)
            {
                ModelState.AddModelError("", "Sorry, your cart is empty");
            }

            if (ModelState.IsValid)
            {
                order.Lines = Cart.Lines.ToArray();
                OrderRepository.SaveOrder(order);
                return RedirectToAction(nameof(Completed));
            }
            return View(order);
        }

        public ViewResult Completed()
        {
            Cart.Clear();
            return View();
        }

        public ViewResult List() => View(OrderRepository.Orders.Where(x => !x.Shipped));

        [HttpPost]
        public IActionResult MarkShipped(int OrderId)
        {
            var order = OrderRepository.Orders.FirstOrDefault(x => x.OrderID == OrderId);
            if (order != null)
            {
                order.Shipped = true;
                OrderRepository.SaveOrder(order);
            }

            return RedirectToAction(nameof(List));
        }
    }
}