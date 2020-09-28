using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using APIConsume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace APIConsume.Controllers
{
    public class HomeController : Controller
    {
        private string _Token;

        public async Task<IActionResult> Index()
        {

            List<User> userList = new List<User>();
            bool isSuccess = false;
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:5000/api/v1/identity/user"))
                {
                    if (isSuccess = response.IsSuccessStatusCode)
                    {
                        string apiResponse = await response.Content.ReadAsStringAsync();
                        userList = JsonConvert.DeserializeObject<List<User>>(apiResponse);
                    }
                }
            }

            if (isSuccess)
                return View(userList);
            else
                return View("ErrorView");
        }

        // Get User
        public ViewResult GetUser() => View();

        [HttpPost]
        public async Task<IActionResult> GetUser(int id)
        {
            User reservation = new User();
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://localhost:5000/api/v1/identity/" + id))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    reservation = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(reservation);
        }


        // Register User
        public ViewResult AddUser() => View();

        [HttpPost]
        public async Task<IActionResult> AddUser(User reservation)
        {
            User receivedUser = new User();
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:5000/api/v1/identity/Register", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    receivedUser = JsonConvert.DeserializeObject<User>(apiResponse);
                }
            }
            return View(receivedUser);
        }



        [HttpPost]
        public async Task<string> Authenticate(User reservation)
        {
            using (var httpClient = new HttpClient())
            {
                StringContent content = new StringContent(JsonConvert.SerializeObject(reservation), Encoding.UTF8, "application/json");

                using (var response = await httpClient.PostAsync("http://localhost:5000/api/v1/identity/Authenticate", content))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    _Token = JsonConvert.DeserializeObject<string>(apiResponse);
                }
            }

            return _Token;
        }
    }
}
