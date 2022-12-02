using Demo_Products_Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Demo_Products_Api.Controllers
{
    public class ProductController : ApiController
    {
        DemoEntities demo = new DemoEntities();

        [HttpPost]
        [Route("api/Product/AddProduct")]
        public async Task<IHttpActionResult>AddProduct(Product product)
        {
            demo.Products.Add(product);
            var productobject = demo.SaveChanges();
            return Ok(productobject);
        }

        [HttpGet]
        [Route("api/Product/ProductList")]
        public List<Product> ProductList()
        {            
            List<Product> productlist = new List<Product>();          
            var listofproduct = (from p in demo.Products
                              join c in demo.Categories on p.category_id equals c.id                                                          
                              select new
                              {
                                  id = p.id,
                                  category_id = p.category_id,
                                  product_code = p.product_code,
                                  product_name = p.product_name,
                                  selling_price = p.selling_price,
                                  category_name = c.category_name,
                              }).ToList();
            foreach (var list in listofproduct)
            {
                Product model = new Product();
                model.id = list.id;
                model.product_code = list.product_code;
                model.product_name = list.product_name;
                model.category_name = list.category_name;
                model.category_id = list.category_id;
                model.selling_price = list.selling_price;
                productlist.Add(model);
            }
            return (productlist);
        }

        [HttpGet]
        [Route("api/Product/EditProductById/{id}")]
        public async Task<IHttpActionResult>EditProductById(int id)
        {
            Product model = new Product();
            var data = demo.Products.Where(x => x.id == id).FirstOrDefault();
            model.id = data.id;
            model.product_code = data.product_code;
            model.product_name = data.product_name;
            model.category_name = demo.Categories.Where(x => x.id == data.category_id).Select(x => x.category_name).FirstOrDefault();
            model.category_id = data.category_id;
            model.selling_price = data.selling_price;
            return Ok(model);
        }

        [HttpPost]
        [Route("api/Product/EditProduct/{id}")]
        public async Task<IHttpActionResult> EditProduct(int id, Product product)
        {
            var _product = demo.Products.Find(id);
            _product.id = product.id;
            _product.product_code = product.product_code;
            _product.product_name = product.product_name;
            _product.category_id = product.category_id;
            _product.selling_price = product.selling_price;
            demo.Entry(_product).State = System.Data.Entity.EntityState.Modified;
            demo.SaveChanges();
            return Ok();
        }

        [HttpGet]
        [Route("api/Product/GetProductDetailsByProductId/{productid}")]
        public List<Product_detail> GetProductDetailsByProductId(int productid)
        {

            List<Product_detail> item = new List<Product_detail>();
            var listofitem = (from pd in demo.Product_detail
                              where pd.product_id == productid
                              select new
                              {
                                  id = pd.id,
                                  product_id = pd.product_id,
                                  vendor = pd.vendor,
                                  purchase_date = pd.purchase_date,
                                  purchase_price = pd.purchase_price,
                                  total_count = pd.total_count,
                              }).ToList();
            foreach (var data in listofitem)
            {
                Product_detail modal = new Product_detail();
                modal.id = data.id;
                modal.product_id = data.id;
                modal.vendor = data.vendor;
                modal.purchase_date = data.purchase_date;
                modal.purchase_price = data.purchase_price;
                modal.total_count = data.total_count;
                item.Add(modal);
            }
            return (item);
        }
    }
}
