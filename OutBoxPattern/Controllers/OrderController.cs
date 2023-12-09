using Microsoft.AspNetCore.Mvc;
using OutBoxPattern.Models;
using OutBoxPattern.Services;
using System.Threading.Tasks;

namespace OutBoxPattern.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
 
    public class OrderController : Controller
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Order order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrder(order);
                return RedirectToAction(nameof(Index));
            }

            return View(order);
        }

        // Implement other actions as needed
    }
}
