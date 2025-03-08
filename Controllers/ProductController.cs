using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using CRUD.Models;

namespace CRUD.Controllers
{
    public class ProductController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Product
        public ActionResult Index()
        {
            var products = db.Products
                             .Select(p => new
                             {
                                 p.ProductId,
                                 p.ProductName,
                                 p.CategoryId,
                                 CategoryName = p.Category.CategoryName
                             })
                             .ToList()
                             .Select(p => new Product
                             {
                                 ProductId = p.ProductId,
                                 ProductName = p.ProductName,
                                 CategoryId = p.CategoryId,
                                 Category = new Category { CategoryName = p.CategoryName }
                             })
                             .ToList();

            return View(products);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            var categories = db.Categories.ToList();
            return View(categories);
        }

        // POST: Product/Create
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var categories = db.Categories.ToList();
            return View(categories);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            var categories = db.Categories.ToList();
            ViewBag.Categories = categories;
            return View(product);
        }

        // POST: Product/Edit/5
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.Categories = db.Categories.ToList();
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        public ActionResult ConfirmDelete(int id)
        {
            var product = db.Products.Find(id);
            if (product == null) return HttpNotFound();

            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
