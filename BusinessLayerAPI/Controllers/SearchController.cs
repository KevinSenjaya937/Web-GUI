using CustomerDatabaseAPI.Models;
using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BusinessLayerAPI.Controllers
{
    public class SearchController : ApiController
    {
        private static string URL = "http://localhost:50915/";
        private static RestClient client = new RestClient(URL);
        // GET: api/Search
        public List<Customer> Get()
        {
            RestRequest request = new RestRequest("api/customers/", Method.Get);
            RestResponse response = client.Execute(request);

            List<Customer> customers = JsonConvert.DeserializeObject<List<Customer>>(response.Content);
            return customers;
        }

        // GET: api/Search/5
        public IHttpActionResult Get(int id)
        {
            RestRequest request = new RestRequest("api/customers/{id}", Method.Get);
            request.AddUrlSegment("id", id);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                Customer customer = JsonConvert.DeserializeObject<Customer>(response.Content);
                customer.ProfileBase64 = Convert.ToBase64String(customer.ProfilePicture);
                return Ok(customer);
            }
            else
            {
                return BadRequest();
            }
            
            
        }

        // POST: api/Search
        public IHttpActionResult Post([FromBody]Customer customer)
        {
            RestRequest request = new RestRequest("api/customers", Method.Post);
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

        // PUT: api/Search/5
        public IHttpActionResult Put(int id, [FromBody]Customer customer)
        {
            RestRequest request = new RestRequest("api/customers/{id}", Method.Put);
            request.AddUrlSegment("id", id);
            request.AddJsonBody(JsonConvert.SerializeObject(customer));
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok("Successfully Added");
            }
            else
            {
                return BadRequest();
            }
        }

        // DELETE: api/Search/5
        public IHttpActionResult Delete(int id)
        {
            RestRequest request = new RestRequest("api/customers/{id}", Method.Delete);
            request.AddUrlSegment("id", id);
            RestResponse response = client.Execute(request);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return Ok("Successfully Added");
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
