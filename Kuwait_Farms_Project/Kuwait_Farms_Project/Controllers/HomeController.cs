using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kuwait_Farms_Project.Models;

namespace Kuwait_Farms_Project.Controllers
{
    public class HomeController : Controller
    {
        FarmsContext Db;
        int total;
        public HomeController()
        {
            Db = new FarmsContext();
            total = 0;
        }
        
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
        #region Contact Us
        [HttpGet]
        public ActionResult Contactus()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Contactus( ContactUs contact)
        {
            if (ModelState.IsValid)
            {
                Db.GetContactUs.Add(contact);
                Db.SaveChanges();
                contact.ContacterrorMsg = "Done";
                return RedirectToAction("Contactus");

            }
            else
            {
                contact.ContacterrorMsg = "  The message has not been received";


                return View("Contactus", contact);
            }
        }
        #endregion
        [HttpGet]
        public ActionResult Products()
        {
            if (Session["id"] == null)
            {
                return View("Login");
            }
            else
            {
                var products = Db.Products.ToList();
                return View("Products", products);
            }
        }

      [HttpPost]
             public ActionResult cart( int n)
        {
            var product = Db.Products.Where(ww => ww.Id == n).FirstOrDefault();
            var cart = new Cart();
            cart.Price = product.Price;
            cart.Product_id = n;
            cart.User_Id = int.Parse(Session["id"].ToString());
            Db.Carts.Add(cart);
            Db.SaveChanges();
            var prod = Db.Products.Where(ww => ww.Id == n).FirstOrDefault();
            prod.Quntity--;
            Db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("Products");
        }
        public ActionResult MyCart()
        {
            if (Session["id"] == null)
            {
                return View("Login");
            }
            else
            {
                int total = 0;
                int userid = int.Parse(Session["id"].ToString());
                var carts = Db.Carts.Where(ww => ww.User_Id == userid).ToList();
                List<Product> productss = new List<Product>();
                foreach (var item in carts)
                {
                    productss.Add(Db.Products.Where(ww => ww.Id == item.Product_id).FirstOrDefault());
                }
                for (int i = 0; i < productss.Count; i++)
                {
                    total += productss[i].Price;
                    Session["total"] = total;
                }


                return View(productss);
            }
        }
        [HttpPost]
        public ActionResult delete(int n)
        {
            var cart = Db.Carts.Where(ww => ww.Product_id == n).FirstOrDefault();

            Db.Carts.Remove(cart);
            Db.SaveChanges();
            var prod = Db.Products.Where(ww => ww.Id == n).FirstOrDefault();
            prod.Quntity++;
            Db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            cart = Db.Carts.Where(ww => ww.Product_id == n).FirstOrDefault();
            if (cart == null)
            {
                total = 0;
                Session["total"] = 0;


            }
            return RedirectToAction("MyCart");
        }
        #region Log Out
        public ActionResult Logout()
        {
            Session["id"] = null;
            Session["log"] = null;
            /////
            return RedirectToAction("Index");
        }
        #endregion
        #region Login
        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login( User user)
        {
            var userDetails = Db.Users.Where(w => w.Email == user.Email && w.Password == user.Password).FirstOrDefault();
            if (userDetails == null)
            {
                Session["log"] = "Log In";

                Session["id"] = null;
                user.LoginErrorMsg = "Invalid Email or Password";
                return View("Login", user);
            }
            string useroradnin = userDetails.Type;
            Session["type"] = useroradnin;
            Session["log"] = "Log out";

            Session["id"] = userDetails.Id;
            if (useroradnin == "admin")
            {
                Session["type"] = "admin";
                return RedirectToAction("Productaadmin");
            }
            else
            {
                Session["type"] = "user";
                return RedirectToAction("Products");
            }
        }
#endregion
        #region Sugn Up
        [HttpGet]
        public ActionResult Signup()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Signup(User user)
        {
            user.Type = "user";
            if (ModelState.IsValid)
            {
                if (Db.Users.Any(w => w.Email == user.Email))
                {
                    user.LoginErrorMsg = "This Email Already Exists.";
                    return View("Signup", user);

                }
                else
                {
                    Db.Users.Add(user);
                    Db.SaveChanges();
                    Session["log"] = "Log out";

                    var val = Db.Users.Where(ww => ww.Email == user.Email);
                    int id = val.Select(ww => ww.Id).First();

                    Session["id"] = id;
                    return RedirectToAction("Products");
                    Session["type"] = "user";

                }
            }
            else
            {
                return View("Signup");
            }
            return View();
        }
#endregion
        public ActionResult Checkout()
        {
            int userid = int.Parse(Session["id"].ToString());
            var carts = Db.Carts.Where(ww => ww.User_Id == userid).ToList();
            var saless = new Sales();
            var pro = new Product();
            foreach (var item in carts)
            {
                saless.User_Id = item.User_Id;
                saless.Product_id = item.Product_id;
                saless.Date = DateTime.Now;

                pro = Db.Products.Where(ww => ww.Id == saless.Product_id).FirstOrDefault();
                saless.Price = pro.Price;
                Db.GetSales.Add(saless);
                Db.SaveChanges();

            }
            return View();
        }
        [HttpPost]
        public ActionResult Checkout2()
        {
            int userid = int.Parse(Session["id"].ToString());
            var carts = Db.Carts.Where(ww => ww.User_Id == userid).ToList();
            foreach (var item in carts)
            {
                Db.Carts.Remove(item);
                Db.SaveChanges();
            }
            total = 0;
            Session["total"] = 0;
            return RedirectToAction("Products");
     
        }

        public ActionResult Productaadmin()
        {
            if (Session["id"] == null)
            {
                return View("Login");
            }
            else
            {
                var products = Db.Products.ToList();
                return View("Productaadmin", products);
            }
        
        }
        public ActionResult add(int n)
        {
           
           
            var prod = Db.Products.Where(ww => ww.Id == n).FirstOrDefault();
            prod.Quntity++;
            Db.Entry(prod).State = System.Data.Entity.EntityState.Modified;
            Db.SaveChanges();
            return RedirectToAction("Productaadmin");
        }

        public ActionResult profile()
        {
            if (Session["id"] == null)
            {
                return View("Login");
            }
            else
            { 
                int id = int.Parse(Session["id"].ToString());
            var user = Db.Users.Where(ww => ww.Id == id).FirstOrDefault();



            return View(user);
        }
        }
    }
}