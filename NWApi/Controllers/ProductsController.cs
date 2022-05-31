using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWApi.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

namespace NWApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        //private readonly NorthwindContext db = new NorthwindContext();

        // Dependency Injection tyyli
        private readonly NorthwindContext db;

        public ProductsController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        
        //Get all products
        [HttpGet]
        public ActionResult GetAllProducts()
        {
            var prod = db.Products.ToList();
            return Ok(prod);
        }
        

        /*[HttpGet]
        public ActionResult GetSpecialData(string productName)
        {
            var prod = (from p in db.Products
                        where p.ProductName.ToLower().Contains(productName.ToLower())
                        select p).FirstOrDefault();

            var cat = (from c in db.Categories 
                       where c.CategoryId == prod.CategoryId 
                       select c).FirstOrDefault();

            var sup = (from s in db.Suppliers 
                       where s.SupplierId == prod.SupplierId 
                       select s).FirstOrDefault();

            List<ProductData> pdata = new List<ProductData>()
            {
                new ProductData
                {
                    Id = prod.ProductId,
                    ProductName = prod.ProductName,
                    SupplierName = sup.CompanyName,
                    CategoryName = cat.CategoryName,
                }
            };

            return Ok(pdata);
        } */
        
        //Get 1 product by id
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOneProduct(int id)
        {
            try
            {
                var prod = db.Products.Find(id);
                if (prod == null)
                {
                    return NotFound("Tuotetta id:llä " + id + " ei löytynyt.");
                }

                return Ok(prod);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Get products by unitprice parameter localhost:xxxxxx/api/customers/product/unit
        [HttpGet]
        [Route("product/{unit}")]
        public ActionResult ProductBy(int unit)
        {
            var prod = (from u in db.Products
                        where u.UnitPrice == unit
                        select u).ToList();

            return Ok(prod);
        }

        //Create new product
        [HttpPost]
        public ActionResult AddNewProduct([FromBody] Product prod)
        {
            try
            {
                db.Products.Add(prod);
                db.SaveChanges();
                return Ok("Lisättiin tuote " + prod.ProductName);
            }
            catch (Exception e)
            {
                return BadRequest("Tuotteen lisäämisessä tapahtui virhe. Lisätietoa" + e);
            }
        }

        //Edit product
        [HttpPut]
        [Route("{id}")]
        public ActionResult EditProduct(int id, [FromBody] Product prod)
        {
            try
            {
                Product tuote = db.Products.Find(id);
                if (tuote != null)
                {
                    tuote.ProductName = prod.ProductName;
                    tuote.SupplierId = prod.SupplierId;
                    tuote.CategoryId = prod.CategoryId;
                    tuote.QuantityPerUnit = prod.QuantityPerUnit;
                    tuote.UnitPrice = prod.UnitPrice;
                    tuote.UnitsInStock = prod.UnitsInStock;
                    tuote.UnitsOnOrder = prod.UnitsOnOrder;
                    tuote.ReorderLevel = prod.ReorderLevel;
                    tuote.Discontinued = prod.Discontinued;
                    tuote.ImageLink = prod.ImageLink;

                    db.SaveChanges();
                    return Ok("Muokattu tuotetta " + tuote.ProductName);
                }
                else
                {
                    return NotFound("Päivitettävää tuotetta ei löytynyt.");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Jokin meni pieleen tuotetta päivitettäessä. Alla lisätietoa" + e);
            }
        }

        //Delete product by id
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Remove(int id)
        {
            var prod = db.Products.Find(id);
            if (prod == null)
            {
                return NotFound("Tuotetta id:llä " + id + " ei löytynyt.");
            }
            else
            {
                try
                {
                    db.Products.Remove(prod);
                    return Ok("Poistettiin tuote " + prod.ProductName);
                }
                catch (Exception e)
                {
                    return BadRequest("Tuotteen poistaminen ei onnistunut. Lisätietoa: " + e);
                }
            }
        }

        // Get by category for example: https://localhost:44327/api/Products/cname/seafood
        [HttpGet]
        [Route("cname/{cname}")]
        public ActionResult GetByCategoryName(string cname)
        {

            var products = (from p in db.Products
                            join c in db.Categories on p.CategoryId equals c.CategoryId
                            where c.CategoryName == cname
                            select p).ToList();

            return Ok(products);
        }

        // Get by min and max price
        [HttpGet]
        [Route("min-price/{min}/max-price/{max}")]
        public ActionResult GetByPrice(int min, int max)
        {
            var p = db.Products.Where(p => p.UnitPrice >= min && p.UnitPrice <= max);
            return Ok(p);
        }

        //Get by category id
        [HttpGet]
        [Route("catid-{cid}")]
        public ActionResult GetByCatId(int cid)
        {
            var p = db.Products.Where(p => p.CategoryId == cid);
            return Ok(p);
        }
    }
}
