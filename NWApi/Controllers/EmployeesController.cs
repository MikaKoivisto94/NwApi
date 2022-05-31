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
    public class EmployeesController : ControllerBase
    {
        //private readonly NorthwindContext db = new NorthwindContext();

        // Dependency Injection tyyli
        private readonly NorthwindContext db;

        public EmployeesController(NorthwindContext dbparam)
        {
            db = dbparam;
        }

        //Get all employees
        [HttpGet]
        public ActionResult GetAllEmp()
        {
            var employees = db.Employees.ToList();
            return Ok(employees);
        }

        //Get 1 employee by id
        [HttpGet]
        [Route("{id}")]
        public ActionResult GetOneEmp(int id)
        {
            try
            {
                var employee = db.Employees.Find(id);
                if (employee == null)
                {
                    return NotFound("Työntekijää id:llä " + id + " ei löytynyt.");
                }

                return Ok(employee);
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        //Delete employee by id
        [HttpDelete]
        [Route("{id}")]
        public ActionResult RemoveEmp(int id)
        {
            var employee = db.Employees.Find(id);
            if (employee == null)
            {
                return NotFound("Työntekijää id:llä " + id + " ei löytynyt.");
            }
            else
            {
                try
                {
                    db.Employees.Remove(employee);
                    return Ok("Poistettiin työntekijä " + employee.FirstName + " " + employee.LastName);
                }
                catch (Exception e)
                {
                    return BadRequest("Työntekijän poistaminen ei onnistunut. Lisätietoja: " + e);
                }
            }
        }

        //Create new employee
        [HttpPost]
        public ActionResult CreateNewEmp([FromBody] Employee employee)
        {
            try
            {
                db.Employees.Add(employee);
                db.SaveChanges();
                return Ok("Lisättiin työntekijä " + employee.FirstName + " " + employee.LastName);
            }
            catch (Exception e)
            {
                return BadRequest("Työntekijän lisääminen ei onnistunut. Lisätietoa" + e);
            }
        }
        //Edit employee
        [HttpPut]
        [Route("{id}")]
        public ActionResult PutEdit(int id, [FromBody] Employee employee)
        {
            try
            {
                Employee employees = db.Employees.Find(id); //Employee voi olla myös var
                if (employees != null)
                {
                    employees.FirstName = employee.FirstName;
                    employees.LastName = employee.LastName;
                    employees.Title = employee.Title;
                    employees.TitleOfCourtesy = employee.TitleOfCourtesy;
                    employees.BirthDate = employee.BirthDate;
                    employees.HireDate = employee.HireDate;
                    employees.Address = employee.Address;
                    employees.City = employee.City;
                    employees.Region = employee.Region;
                    employees.PostalCode = employee.PostalCode;
                    employees.Country = employee.Country;
                    employees.HomePhone = employee.HomePhone;
                    employees.Extension = employee.Extension;
                    employees.Photo = employee.Photo;
                    employees.Notes = employee.Notes;
                    employees.ReportsTo = employee.ReportsTo;
                    employees.PhotoPath = employee.PhotoPath;

                    db.SaveChanges();
                    return Ok("Muokattu työntekijää " + employee.FirstName + " " + employee.LastName);
                }
                else
                {
                    return NotFound("Päivitettävää työntekijää ei löytynyt.");
                }
            }
            catch (Exception e)
            {
                return BadRequest("Jokin meni pieleen työntekijää päivitettäessä. Alla lisätietoa" + e);
            }
        }

        //Get employees by city parameter localhost:xxxxxx/api/employees/city/Seattle
        [HttpGet]
        [Route("city/{kaupunki}")]
        public ActionResult GetSomeEmp(string kaupunki)
        {
            var empl = db.Employees.Where(c => c.City.ToLower().Contains(kaupunki.ToLower()));
            return Ok(empl);
        }

        //Get employee by first name
        [HttpGet]
        [Route("name/{etunimi}")] 
        public ActionResult GetEmp(string etunimi)
        {
            var empl = db.Employees.Where(f => f.FirstName.ToLower().Contains(etunimi.ToLower()));
            return Ok(empl);
        }
    }
}
