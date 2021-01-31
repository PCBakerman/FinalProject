using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FinalProject.Controllers
{
    public class UserInventoryController : Controller
    {
        // GET: UserInventoryController
        public ActionResult Index()
        {
            return View();
        }

        // GET: UserInventoryController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: UserInventoryController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: UserInventoryController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserInventoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: UserInventoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: UserInventoryController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: UserInventoryController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
