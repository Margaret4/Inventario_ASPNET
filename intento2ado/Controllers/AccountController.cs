using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using intento2ado.Models;
using System.Data.SqlClient;

namespace intento2ado.Controllers
{

    public class AccountController : Controller
    {

        SqlConnection con = new SqlConnection();
        SqlCommand com = new SqlCommand();
        SqlDataReader dr;
        // GET: Account
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        void connectionString()
        {
            con.ConnectionString = "data source=DESKTOP-56NO6T0; database=MINIMARKET; integrated security=SSPI;";
            //&quot; data source = ; initial catalog = MINIMARKET; integrated security = True; MultipleActiveResultSets = True; App = EntityFramework & quot

        }
        [HttpPost]
        public ActionResult Verify(Account acc)
        {
            connectionString();
            con.Open();
            com.Connection = con;

            com.CommandText = "select * from vend where dni='" + acc.dni + "' and pwd='" + acc.pwd + "'";
            
            dr = com.ExecuteReader();
            if (dr.Read())
            {
                con.Close();
                HttpCookie kt1 = new HttpCookie("vend", acc.dni);
                kt1.Expires = DateTime.Now.AddHours(2);
                Response.Cookies.Add(kt1);
               
                return RedirectToAction("Index", "Home");
                
            }
            else
            {
                con.Close();
                //return RedirectToAction("About", "Home");
                //return RedirectToAction("Error.cshtml","Account");
                return View("Error");
            }

        }

        public ActionResult Error()
        {
            return View();
        }

    }
}