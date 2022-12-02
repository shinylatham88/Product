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
    public class ProductController : Controller
    {
        DemoEntities demo = new DemoEntities();      
        public ActionResult AddProduct()
        {
            Product model = new Product();          
            ViewBag.category = new SelectList(GetCategoryName(), "id", "category_name");           
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult>AddProduct(Product model)
        {
            Product product = new Product();
            product.product_code = model.product_code;
            product.product_name = model.product_name;
            product.selling_price = model.selling_price;
            ViewBag.category = new SelectList(GetCategoryName(), "id", "category_name");
            string category = Request.Form["category"].ToString();
            product.category_id = Convert.ToInt32(category);
            var httpClient = new HttpClient();
            string LoginWebServiceUrl = "https://localhost:44396/api/Product/AddProduct";
            string jsonData = JsonConvert.SerializeObject(product);
            StringContent content = new StringContent(jsonData, Encoding.UTF8, "application/json");
            HttpResponseMessage response = await httpClient.PostAsync(LoginWebServiceUrl, content);
            if (response.IsSuccessStatusCode)
            {              
                return RedirectToAction("ProductList", "Product");
            }
            else
            {               
                return View();
            }
        }
        public async Task<ActionResult> ProductList()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.GetAsync("https://localhost:44396/api/Product/ProductList/").Result;
            List<Product> list = new List<Product>();
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<Product[]>(response.Content.ReadAsStringAsync().Result).ToList();
            }
            return View(list);
        }

        [HttpGet]
        public ActionResult EditProduct(int id)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.GetAsync("https://localhost:44396/api/Product/EditProductById/" + id).Result;
            Product model = new Product();
            if (response.IsSuccessStatusCode)
            {
                model = JsonConvert.DeserializeObject<Product>(response.Content.ReadAsStringAsync().Result);
                ViewBag.category = new SelectList(GetCategoryName(), "id", "category_name",model.category_id);
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> EditProduct(Product model)
        {
            int id = model.id;
            Product product = new Product();
            product.id = model.id;
            product.product_code = model.product_code;
            product.product_name = model.product_name;
            product.selling_price = model.selling_price;           
            string category = Request.Form["category"].ToString();
            product.category_id = Convert.ToInt32(category);
            ViewBag.category = new SelectList(GetCategoryName(), "id", "category_name");
            var httpClient = new HttpClient();
            var json = JsonConvert.SerializeObject(product);
            HttpContent content = new StringContent(json);
            content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var client = new HttpClient();
            var response = await client.PostAsync("https://localhost:44396/api/Product/EditProduct/" + id, content);

            if (response.IsSuccessStatusCode)
            {
                return RedirectToAction("", "");
            }
            else
            {
                return View();
            }
        }
        public async Task<ActionResult> ProductDetailListByProductId(int id)
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = httpClient.GetAsync("https://localhost:44396/api/Product/GetProductDetailsByProductId/"+id).Result;
            List<Product_detail> list = new List<Product_detail>();
            if (response.IsSuccessStatusCode)
            {
                list = JsonConvert.DeserializeObject<Product_detail[]>(response.Content.ReadAsStringAsync().Result).ToList();
            }
            return View(list);
        }


        public List<Category> GetCategoryName()
        {
            List<Category> list = new List<Category>();
            var obj = (from Category in demo.Categories
                       select new
                       {
                           Category.id,
                           Category.category_name
                       }).ToList();
            if (obj != null && obj.Count() > 0)
            {
                foreach (var data in obj)
                {
                    Category model = new Category();
                    model.id = data.id;
                    model.category_name = data.category_name;
                    list.Add(model);
                }
            }
            return (list);
        }
    }
}