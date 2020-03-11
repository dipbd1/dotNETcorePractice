using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Demo.Models;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace Demo.Controllers
{
    public class HomeController : Controller
    {
        public IConfiguration Configuration { get; }
        private readonly ILogger<HomeController> _logger;
//        public HomeController(ILogger<HomeController> logger)
//        {
//            _logger = logger;
//        }
        public HomeController(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        public IActionResult Index()
        {
            List<DemoModel> demoList = new List<DemoModel>();
            string connectionString = Configuration.GetConnectionString("con");
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                string sql = "SELECT * from [Demo].[dbo].[Users]";
                SqlCommand command = new SqlCommand(sql, connection);
                using (SqlDataReader dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        DemoModel demo = new DemoModel();
                        demo.id = Convert.ToInt32(dataReader["id"]);
                        demo.Name = Convert.ToString(dataReader["Name"]);
                        demoList.Add(demo);
                    }
                }
                connection.Close();
                ViewBag.id = demoList[0].id;
                ViewBag.Name = demoList[0].Name;
            }
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
    }
}
