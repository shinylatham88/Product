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
    public class Product_detailController : ApiController
    {
        DemoEntities demo = new DemoEntities();

        [HttpPost]
        [Route("api/Product_detail/AddProductDetail")]
        public async Task<IHttpActionResult> AddProductDetail(Product_detail product_detail)
        {
            demo.Product_detail.Add(product_detail);
            var productobject = demo.SaveChanges();
            return Ok(productobject);
        }

        [HttpGet]
        [Route("api/Product_detail/ProductDetailList")]
        public List<Product_detail>ProductDetailList()
        {
            List<Product_detail> productlistdetail = new List<Product_detail>();
            var listofproductdetail = demo.Product_detail.ToList();

            foreach (var list in listofproductdetail)
            {
                Product_detail modal = new Product_detail();
                modal.id = list.id;
                modal.product_id = list.product_id;              
                modal.product_name = demo.Products.Where(x => x.id == list.product_id).Select(x => x.product_name).FirstOrDefault();
                modal.vendor = list.vendor;
                modal.purchase_date = list.purchase_date;
                modal.purchase_price = list.purchase_price;
                modal.total_count = list.total_count;
                productlistdetail.Add(modal);
            }
            return(productlistdetail);
        }

        [HttpGet]
        [Route("api/Product_detail/EditProductDetailById/{id}")]
        public async Task<IHttpActionResult> EditProductDetailById(int id)
        {
            Product_detail modal = new Product_detail();
            var data = demo.Product_detail.Where(x => x.id == id).FirstOrDefault();
            modal.id = data.id;
            modal.vendor = data.vendor;
            modal.product_name = demo.Products.Where(x => x.id == data.product_id).Select(x => x.product_name).FirstOrDefault();
            modal.purchase_date = data.purchase_date;
            modal.purchase_price = data.purchase_price; 
            modal.total_count = data.total_count;
            modal.product_id = data.product_id;
            return Ok(modal);
        }

        [HttpPost]
        [Route("api/Product_detail/EditProductDetail/{id}")]
        public async Task<IHttpActionResult> EditProductDetail(int id, Product_detail product)
        {
            var _product = demo.Product_detail.Find(id);
            _product.id = product.id;
            _product.product_id = product.product_id;
            _product.vendor = product.vendor;
            _product.purchase_date = product.purchase_date;
            _product.purchase_price = product.purchase_price;
            _product.total_count = product.total_count;
            demo.Entry(_product).State = System.Data.Entity.EntityState.Modified;
            demo.SaveChanges();
            return Ok();
        }

       
    }
}
