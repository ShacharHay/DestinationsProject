using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ApplicationsProject.Models;
using ApplicationsProject.ViewModels;

namespace ApplicationsProject.Controllers
{
    public class ClientsController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Clients
        public ActionResult Index()
        {
            return View(db.Clients.ToList());
        }

        // GET: Clients/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // GET: Clients/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clients/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ClientId,Gender,ClientName,FirstName,LastName,Password,IsAdmin")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Clients.Add(client);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(client);
        }

        // GET: Clients/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ClientId,Gender,ClientName,FirstName,LastName,Password,IsAdmin")] Client client)
        {
            if (ModelState.IsValid)
            {
                db.Entry(client).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(client);
        }

        // GET: Clients/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Client client = db.Clients.Find(id);
            if (client == null)
            {
                return HttpNotFound();
            }
            return View(client);
        }

        // POST: Clients/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Client client = db.Clients.Find(id);
            db.Clients.Remove(client);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login([Bind(Include = "ClientName,Password")] Client client)
        {
            var pass = client.Password;
            var logonName = client.ClientName;

            var requestedClient = db.Clients.SingleOrDefault(u => u.ClientName.Equals(logonName) && u.Password.Equals(pass));

            if (requestedClient == null)
            {
                return RedirectToAction("FailedLogin", "Clients");
            }

            Session.Add("Client", requestedClient);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Session.Clear();

            return RedirectToAction("Index", "Home");
        }

        public ActionResult FailedLogin()
        {
            return View();
        }

        public ActionResult DestinationsLogin()
        {
            return View();
        }

        public ActionResult Statistics()
        {
            // join select for users and their destinations
            var query =
                from client in db.Clients
                join destination in db.Destinations on client.ClientId equals destination.ClientId
                select new UserDestinations
                {
                    UserName = client.ClientName,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    Title = destination.Title,
                    Id = client.ClientId
                };

            return View(query.ToList());
        }

        [HttpGet]
        public JsonResult GetGroupByGender()
        {
            var data = db.Clients.GroupBy(x => x.Gender, client => client, (gender, clients) => new
            {
                Name = gender.ToString(),
                Count = clients.Count()
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult Search(string username, string firstname, string lastname)
        {
            if (!AuthorizationAdmin.AdminAuthorized(Session)) return RedirectToAction("Index", "Home");

            var requestedClients = new List<Client>();

            foreach (var client in db.Clients)
            {
                if (!string.IsNullOrEmpty(username) && client.ClientName.Contains(username))
                {
                    requestedClients.Add(client);
                }
                else if (!string.IsNullOrEmpty(firstname) && client.FirstName.Contains(firstname))
                {
                    requestedClients.Add(client);
                }
                else if (!string.IsNullOrEmpty(lastname) && client.LastName.Contains(lastname))
                {
                    requestedClients.Add(client);
                }
            }

            return View(requestedClients.OrderByDescending(x => x.ClientName));
        }
    }
}
