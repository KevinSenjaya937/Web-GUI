using Microsoft.AspNetCore.Mvc;
using RestSharp;
using WebGUI.Models;
using Newtonsoft.Json;
using System.Net;

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

        public IActionResult Generate()
        {
            RestRequest request = new RestRequest("api/data/generate");
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content);
            }
            else
            {
                return BadRequest(response.Content);
            }
        }

        [HttpGet]
        public IActionResult Search(int id)
        {
            RestRequest request = new RestRequest("api/search/{id}");
            request.AddUrlSegment("id", id);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok(response.Content);
            }
            else
            {
                return BadRequest(response.Content);
            }
        }

        [HttpPost]
        public IActionResult Insert([FromBody] Customer customer)
        {
            Converter(customer);
            RestRequest request = new RestRequest("api/search", Method.Post);
            request.AddJsonBody(JsonConvert.SerializeObject(customer));
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok("Successfully Added");
            }
            else
            {
                return BadRequest(response.Content);
            }
        }

        [HttpPut]
        public IActionResult Update([FromBody] Customer customer)
        {
            Converter(customer);
            RestRequest request = new RestRequest("api/search/{id}", Method.Put);
            request.AddUrlSegment("id", customer.AccountNumber);
            request.AddJsonBody(JsonConvert.SerializeObject(customer));
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok("Successfully Updated");
            }
            else
            {
                return BadRequest();
            }

        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            RestRequest request = new RestRequest("api/search/{id}", Method.Delete);
            request.AddUrlSegment("id", id);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok("Successfully Deleted");
            }
            else
            {
                return BadRequest();
            }

        }

        private void Converter(Customer customer)
        {
            Console.WriteLine(customer.ProfileBase64);

            customer.ProfilePicture = Convert.FromBase64String(customer.ProfileBase64.Split("data:image/jpeg;base64,")[1]);
        }
    }
}
