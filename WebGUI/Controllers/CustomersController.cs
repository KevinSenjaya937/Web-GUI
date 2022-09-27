using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebGUI.Models;
using Newtonsoft.Json;

namespace WebGUI.Controllers
{
    public class CustomersController : Controller
    {
        private static string URL = "http://localhost:53590/";
        private static RestClient client = new RestClient(URL);
        public IActionResult Index()
        {
            ViewBag.Title = "Customers";
            return View();
        }

        [HttpGet]
        public IActionResult Search(int id)
        {
            RestRequest request = new RestRequest("api/search/{id}");
            request.AddUrlSegment("id", id);
            RestResponse response = client.Execute(request);

            return Ok(response.Content);
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Customer customer)
        {
            RestRequest request = new RestRequest("api/search", Method.Post);
            request.AddJsonBody(JsonConvert.SerializeObject(customer));
            RestResponse response = client.Execute(request);

            Customer returnCustomer = JsonConvert.DeserializeObject<Customer>(response.Content);

            if (returnCustomer != null)
            {
                return Ok(returnCustomer);
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
