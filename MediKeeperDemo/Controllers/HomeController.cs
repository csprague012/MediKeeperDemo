using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediKeeperDemo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Connections;
using Sql;

namespace MediKeeperDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
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
        [HttpPost]
        [Route("Controllers/HomeController/Init")]
        public JsonResult Init() {
            ItemResponse res = new ItemResponse();            
            try
            {
                res.items.AddRange(Connection.LoadItems());
                res.message = "Success";
            }
            catch (Exception e) {
                res.message = e.Message;
                if (string.Compare(res.message, "SQL logic error↵no such table: ITEMS", StringComparison.OrdinalIgnoreCase) > 0)
                {
                    Connection.CreateTable();
                }
            }
            return new JsonResult(res);
        }
        [HttpPost]
        [Route("Controllers/HomeController/Update")]
        public JsonResult Update(Item item)
        {
            string message = "";
            try
            {
                Connection.UpdateItem(item);
                message = "Success";
            }
            catch (Exception e) {
                message = e.Message;                
            }
            return new JsonResult(message);
        }
        [HttpPost]
        [Route("Controllers/HomeController/Add")]
        public JsonResult Add(Item item)
        {
            string message = "";
            try
            {
                Connection.SaveItem(item);
                message = "Success";
            }
            catch (Exception e)
            {
                message = e.Message;
            }
            return new JsonResult(message);
        }
        [HttpPost]
        [Route("Controllers/HomeController/Delete")]
        public JsonResult Delete(Item item)
        {
            string message = "";
            try
            {
                Connection.DeleteItem(item);
                message = "Success";
            }
            catch (Exception e) {
                message = e.Message;
            }
            return new JsonResult(message);
        }        
    }
}
