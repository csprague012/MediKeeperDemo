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
using System.Web.Http.Cors;

namespace MediKeeperDemo.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        [HttpGet]
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
                if (res.message.Contains("no such table: ITEMS"))
                {
                    Connection.CreateTable();
                }
            }
            return new JsonResult(res);
        }
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
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
        [HttpGet]
        [Route("Controllers/HomeController/GetMaxPrice")]
        public JsonResult GetMaxPrice()
        {
            ItemResponse itemResponse = new ItemResponse();
            try
            {
                itemResponse.items = Connection.GetMaxPrice();
                itemResponse.message = "Success";
            }
            catch (Exception e)
            {
                itemResponse.message = e.Message;
            }
            return new JsonResult(itemResponse);
        }
        [HttpGet]
        [Route("Controllers/HomeController/GetMaxPriceByName")]
        public JsonResult GetMaxPriceByName(Item item)
        {
            ItemResponse itemResponse = new ItemResponse();
            try
            {
                itemResponse.items = Connection.GetMaxPrice(item);
                itemResponse.message = "Success";
            }
            catch (Exception e)
            {
                itemResponse.message = e.Message;
            }
            return new JsonResult(itemResponse);
        }
        [HttpPost]
        [Route("Controllers/HomeController/UploadCSV")]
        public JsonResult UploadCSV(UploadRequest upload)
        {
            ItemResponse res = new ItemResponse();
            List<Item> toUpload = new List<Item>();
            string[] dataRows = upload.data.Split("\n");
            for(int i =1; i<dataRows.Length; i++) {
                string[] values = dataRows[i].Split(",");
                for(int j =1; j<values.Length; j+=2) {
                    Item newItem = new Item {
                        name = values[j],
                        cost = Convert.ToDecimal(values[j+1])
                    };
                    toUpload.Add(newItem);
                    break;
                }                
            }
            res.items = toUpload;
            try
            {
                foreach(var item in toUpload) {
                    Connection.SaveItem(item);
                }
                res.message = "Success";
            }
            catch (Exception e)
            {
                res.message = e.Message;
                if (res.message.Contains("no such table: ITEMS"))
                {
                    Connection.CreateTable();
                }
            }
            return new JsonResult(res);
        }
    }
}
