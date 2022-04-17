using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using WebAppCoreFirst.Data;
using WebAppCoreFirst.Models;

namespace WebAppCoreFirst.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbCon _db;

        public HomeController(AppDbCon db)
        {
            _db = db;
        }


        public ActionResult Index()
        {
            var res = _db.Employees.ToList();

            List<Employee> obj = new List<Employee>();
            foreach (var item in res)
            {
                obj.Add(new Employee
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    Phone = item.Phone,
                    Address=item.Address
                });
            }
            return View(obj);
        }

        [HttpGet]
        public ActionResult AddEmp()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AddEmp(Employee obj)    //this is add method
        {
            
            Employee tbl = new Employee();
            tbl.Id = obj.Id;
            tbl.Name = obj.Name;
            tbl.Email = obj.Email;
            tbl.Phone = obj.Phone;
            tbl.Address = obj.Address;


            if (obj.Id == 0)
            {
                _db.Employees.Add(tbl);
                _db.SaveChanges();
                
            }
            else
            {
                _db.Entry(tbl).State = EntityState.Modified;
                _db.SaveChanges();
            }

            return RedirectToAction("Index", "Home");

        }

        public ActionResult EditEmp(int id)
        {
            
            Employee Emod = new Employee();

            var res = _db.Employees.Where(a =>a.Id == id).First(); 

            Emod.Id = res.Id;
            Emod.Name = res.Name;
            Emod.Email = res.Email;
            Emod.Phone = res.Phone;
            Emod.Address = res.Address;

            return View("AddEmp", Emod);
        }

        public ActionResult Delete(int id)
        {
            var deleteitem = _db.Employees.Where(a => a.Id == id).First();
            _db.Employees.Remove(deleteitem);
            _db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        public IActionResult Edit(int id)
        {
            Employee edobj = new Employee();
            var edititem = _db.Employees.Where(a => a.Id == id).First();
            edobj.Id = edititem.Id;
            edobj.Name = edititem.Name;
            edobj.Email = edititem.Email;
            edobj.Phone = edititem.Phone;
            edobj.Address = edititem.Address;


            return View("AddEmp", edititem);
        }


        


        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
