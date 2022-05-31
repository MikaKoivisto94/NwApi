using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NWApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace NWApi.Controllers
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        //private readonly NorthwindContext db = new NorthwindContext();

        // Dependency Injection tyyli
        private readonly NorthwindContext db;

        public CustomersController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        //Get all customers
        [HttpGet]
        public ActionResult GetAll()
        {
            var customers = db.Customers.ToList();
            return Ok(customers);
        }

        //Get 1 customer by id
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOneCustomer(string id)
        {
            try
            {
                var asiakas = db.Customers.Find(id);
                if (asiakas == null)
                {
                    return NotFound("Asiakasta id:llä " + id + " ei löytynyt.");
                }
                
                return Ok(asiakas);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Delete customer by id
        [HttpDelete]
        [Route("{id}")]
        public ActionResult Remove(string id)
        {
            var customer = db.Customers.Find(id);
            if (customer == null)
            {
                return NotFound("Asiakasta id:llä " + id + " ei löytynyt.");
            }
            else
            {
                try
                {
                    db.Customers.Remove(customer);
                    return Ok("Poistettiin asiakas " + customer.CompanyName);
                }
                catch (Exception e)
                {
                    return BadRequest("Poistaminen ei onnistunut. Syynä saattaa olla että asiakkaalla on tilauksia?");
                }
            }
        }

        //Create new customer
        [HttpPost]
        public ActionResult PostCreateNew([FromBody] Customer asiakas)
        {
            try
            {
                db.Customers.Add(asiakas);
                db.SaveChanges();
                //return Created("...api/customers", asiakas); <-- yksi tapa tämäkin
                return Ok($"Luotiin {asiakas.CompanyName}");
            }
            catch (Exception e)
            {
                return BadRequest("Asiakkaan lisääminen ei onnistunut. Lisätietoa" + e);
            }
        }
        //Edit customer
        [HttpPut]
        [Route("{id}")]
        public ActionResult PutEdit(string id, [FromBody] Customer asiakas)
        {
            if (asiakas == null)
            {
                return BadRequest("Asiakas puuttuu pyynnön bodysta.");
            }

            try
            {
                Customer customer = db.Customers.Find(id); //Customer voi olla myös var

                if (customer != null)
                {
                    customer.CompanyName = asiakas.CompanyName;
                    customer.ContactName = asiakas.ContactName;
                    customer.ContactTitle = asiakas.ContactTitle;
                    customer.Country = asiakas.Country;
                    customer.Address = asiakas.Address;
                    customer.City = asiakas.City;
                    customer.PostalCode = asiakas.PostalCode;
                    customer.Phone = asiakas.Phone;
                    customer.Fax = asiakas.Fax;

                    db.SaveChanges();
                    return Ok("Muokattu asiakasta " + customer.CompanyName);
                }
                else
                {
                    return NotFound("Päivitettävää asiakasta ei löytynyt.");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Jokin meni pieleen asiakasta päivitettäessä. Alla lisätietoa" + e);
            }
        }

        //Get customers by country parameter localhost:xxxxxx/api/customers/country/finland
        [HttpGet]
        [Route("country/{maa}/city/{city}")]
        public ActionResult GetByCountryAndCity(string maa, string city)
        {
            /*var cust = (from c in db.Customers
                         where c.Country.ToLower() == maa.ToLower()
                         select c).ToList();
            */

            //Sama kuin yllä, mutta lambda-tyylillä:
            //var cust = db.Customers.Where(c => c.Country.ToLower() == maa.ToLower());
            
            //Tässä riittää että tiedetään maan nimen osa:
            var cust = db.Customers.Where(c => c.Country.ToLower().Contains(maa.ToLower()) &&
            c.City.ToLower().Contains(city.ToLower()));

            return Ok(cust);
        }
    }
}
