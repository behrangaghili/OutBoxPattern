using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DispatcherService.Models;
using DispatcherService.Persistence;
using DispatcherService.Services;

namespace DispatcherService.Controllers
{
    public class OrderController : Controller
    {
 
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrders();
            return View(orders);
        }
 
        // GET: OrderModels/Details/5
         public async Task<IActionResult> Details(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound(); 
            }
            return View(order);
        }

        // GET: OrderModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: OrderModels/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(OrderModel order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.CreateOrder(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: OrderModels/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound(); 
            }
            return View(order);
        }

        // POST: OrderModels/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, OrderModel order)
        {
            if (ModelState.IsValid)
            {
                await _orderService.EditOrder(order);
                return RedirectToAction(nameof(Index));
            }
            return View(order);
        }

        // GET: OrderModels/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            var order = await _orderService.GetOrderById(id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: OrderModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _orderService.DeleteOrder(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
