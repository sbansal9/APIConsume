using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using APIConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIConsume.Controllers
{
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {
            List<User> userList = new List<User>();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:5000/api/v1/identity/user"))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    userList = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                }
            }
            return View(userList);
        }
    }
}
