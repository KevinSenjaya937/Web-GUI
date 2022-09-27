using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebGUI.Models;
using Newtonsoft.Json;

namespace WebGUI.Controllers
{
    public class HomeController : Controller
    {
        private static string URL = "http://localhost:53590/";
        private static RestClient client = new RestClient(URL);
        public IActionResult Index()
        {
            ViewBag.Title = "Home";

            List<Customer> customerList = new List<Customer>();

            RestRequest request = new RestRequest("api/search/", Method.Get);
            RestResponse response = client.Execute(request);

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(response.Content);

            return View(customers);
        }
    }
}
