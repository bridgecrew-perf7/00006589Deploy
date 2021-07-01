using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication.Models;

namespace WebApplication.Controllers
{
    public class ReviewController : Controller
    {
        // GET: Review
        public async Task<ActionResult> Index()
        {
            //Hosted web API REST Service base url
            string Baseurl = "https://localhost:44316/";
            List<ProductReview> ProdReviewInfo = new List<ProductReview>();
            using (var client = new HttpClient())
            {
                //Passing service base url
                client.BaseAddress = new Uri(Baseurl);
                client.DefaultRequestHeaders.Clear();
                //Define request data format
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                //Sending request to find web api REST service resource GetAllEmployees            
                HttpResponseMessage Res = await client.GetAsync("api/ProductReview");
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the Product list
                    ProdReviewInfo = JsonConvert.DeserializeObject<List<ProductReview>>(PrResponse);
                }
                //returning the Product list to view
                return View(ProdReviewInfo);
            }
        }

        // GET: Review/Details/5
        public async Task<ActionResult> Details(int id)
        {
            string Baseurl = "https://localhost:44316/";
            ProductReview review = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/ProductReview/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the ProductReview list
                    review = JsonConvert.DeserializeObject<ProductReview>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(review);
        }

        // GET: Review/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Review/Create
        [HttpPost]
        public async Task<ActionResult> Create(ProductReview review)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44316/");

                    var response = await client.PostAsJsonAsync("api/ProductReview", review);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
            }
            return View(review);
        }

        // GET: Review/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ProductReview review = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:44316/");

                var result = await client.GetAsync("api/ProductReview/" + id);

                if (result.IsSuccessStatusCode)
                {
                    review = await result.Content.ReadAsAsync<ProductReview>();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error try after some time.");
                }
            }
            if (review == null)
            {
                return HttpNotFound();
            }
            return View(review);
        }

        // POST: Review/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, [Bind(Include = "Username,Review,ProductId")] ProductReview review)
        {
            if (ModelState.IsValid)
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri("https://localhost:44316/");
                    var response = await client.PutAsJsonAsync("api/ProductReview/"+id, review);
                    if (response.IsSuccessStatusCode)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, "Server error try after some time.");
                    }
                }
                return RedirectToAction("Index");
            }
            return View(review);
        }

        // GET: Review/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            string Baseurl = "https://localhost:44316/";
            ProductReview review = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(Baseurl);
                HttpResponseMessage Res = await client.GetAsync("api/ProductReview/" + id);
                //Checking the response is successful or not which is sent using HttpClient
                if (Res.IsSuccessStatusCode)
                {
                    //Storing the response details recieved from web api
                    var PrResponse = Res.Content.ReadAsStringAsync().Result;
                    //Deserializing the response recieved from web api and storing into the ProductReview list
                    review = JsonConvert.DeserializeObject<ProductReview>(PrResponse);
                }
                else
                    ModelState.AddModelError(string.Empty, "Server Error. Please contact administrator.");
            }
            return View(review);
        }

        // POST: Review/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string Baseurl = "https://localhost:44316/";
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(Baseurl);
                    //HTTP POST
                    var deleteTask = client.DeleteAsync("api/ProductReview/" + id);
                    deleteTask.Wait();
                    var result = deleteTask.Result;
                    Console.WriteLine("Deleted!");
                    return RedirectToAction("Index");
                }
            }
            catch
            {
                return View();
            }
        }
    }
}
