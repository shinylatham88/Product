using Demo_Products_MVC.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace Demo_Products_MVC.Controllers
{
    public class Product_DetailController : Controller
    {       
        DemoEntities demo = new DemoEntities();       
        public ActionResult AddProductDetail()
        {
            Product_detail model = new Product_detail();
            ViewBag.product = new SelectList(GetProductName(), "id", "product_name");
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddProductDetail(Product_detail model)
        {
            Product_detail product_detail = new Product_detail();
            product_detail.vendor = model.vendor;
            product_detail.purchase_date = model.purchase_date;
            product_detail.purchase_price = model.purchase_price;
            ViewBag.product = new SelectList(GetProductName(), "id", "product_name");
            string product = Request.Form["product"].ToString();
            product_detail.product_id = Convert.ToInt32(product);
            product_detail.total_count = model.total_count;
            var httpClient = new HttpClient();
            string LoginWebServiceUrl = "https://localhost:44396/api/Product_detail/AddProductDetail";
            string jsonData = JsonConvert.SerializeObject(product_detail);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(LoginWebServiceUrl, content);
            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductDetailList", "Product_Detail");
            }
            else
            {
                return View();
            }
        }
        public async Task<ActionResult> ProductDetailList()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.GetAsync("https://localhost:44396/api/Product_detail/ProductDetailList/").Result;
            List<Product_detail> list = new List<Product_detail>();
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<Product_detail[]>(response.Content.ReadAsStringAsync().Result).ToList();
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult EditProductDetail(int id)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.GetAsync("https://localhost:44396/api/Product_detail/EditProductDetailById/" + id).Result;
            Product_detail model = new Product_detail();
            if (response.IsSuccessStatusCode)
            {
                model = JsonConvert.DeserializeObject<Product_detail>(response.Content.ReadAsStringAsync().Result);
                ViewBag.product = new SelectList(GetProductName(), "id", "product_name",model.product_id);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditProductDetail(Product_detail model)
        {
            int id = model.id;
            Product_detail product_detail = new Product_detail();
            product_detail.id = model.id;
            product_detail.vendor = model.vendor;
            product_detail.purchase_date = model.purchase_date;
            product_detail.purchase_price = model.purchase_price;
            ViewBag.product = new SelectList(GetProductName(), "id", "product_name");
            string product = Request.Form["product"].ToString();
            product_detail.product_id = Convert.ToInt32(product);
            product_detail.total_count = model.total_count;           
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(product_detail);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:44396/api/Product_detail/EditProductDetail/" + id, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("ProductDetailList", "Product_Detail");
            }
            else
            {
                return View();
            }
        }

        public List<Product> GetProductName()
        {
            List<Product> list = new List<Product>();
            var obj = (from Product in demo.Products
                       select new
                       {
                           Product.id,
                           Product.product_name,
                       }).ToList();
            if (obj != null && obj.Count() > 0)
            {
                foreach (var data in obj)
                {
                    Product model = new Product();
                    model.id = data.id;
                    model.product_name = data.product_name;
                    list.Add(model);
                }
            }
            return (list);
        }
    }
}