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
    public class DestinationsController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        // GET: Destinations
        public ActionResult Index()
        {
            var destinations = db.Destinations.Include(d => d.Client).Include(d => d.Continent);

            ViewBag.recommendedDestination = GetRecommendedDestination(destinations);

            return View(destinations.ToList());
        }

        private Destination GetRecommendedDestination(IQueryable<Destination> allRecipes)
        {
            var currentUser = (Client)Session["Client"];

            if (currentUser == null) return null;

            var currentUserDestinations = db.Clients.Where(x => x.ClientId == currentUser.ClientId).Include(x => x.Destinations).SingleOrDefault()?.Destinations;

            if (currentUserDestinations == null || !currentUserDestinations.Any()) return null;

            // Find the food category in which the current user wrote most of his recipes, and then get the
            // recipe with the biggest number of comments in this category and display it to the current user
            // as his recommended recipe
            Continent currUserTopCategory = currentUserDestinations
                .GroupBy(x => x.Continent)
                .OrderByDescending(x => x.Key.Destinations.Count(destination => destination.ClientId == currentUser.ClientId))
                .FirstOrDefault()?.Key;

            return allRecipes
                .Where(x => x.Continent.ContinentId == currUserTopCategory.ContinentId)
                .OrderByDescending(x => x.Comments.Count)
                .FirstOrDefault(); ;
        }

        public ActionResult RecommendedDestinationDetails()
        {
            var destinations = db.Destinations.Include(p => p.Client).Include(p => p.Continent);
            var recommendedDestination = GetRecommendedDestination(destinations);

            if (recommendedDestination == null)
            {
                return HttpNotFound();
            }

            return View("Details", recommendedDestination);
        }

        // GET: Destinations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destination destination = db.Destinations.Find(id);
            if (destination == null)
            {
                return HttpNotFound();
            }
            return View(destination);
        }

        // GET: Destinations/Create
        public ActionResult Create()
        {
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName");
            ViewBag.ContinentId = new SelectList(db.Continents, "ContinentId", "Name");
            return View();
        }

        // POST: Destinations/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "DestinationId,ClientId,ContinentId,Title,Content,CreationDate")] Destination destination)
        {
            if (ModelState.IsValid)
            {
                db.Destinations.Add(destination);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", destination.ClientId);
            ViewBag.ContinentId = new SelectList(db.Continents, "ContinentId", "Name", destination.ContinentId);
            return View(destination);
        }

        // GET: Destinations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destination destination = db.Destinations.Find(id);
            if (destination == null)
            {
                return HttpNotFound();
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", destination.ClientId);
            ViewBag.ContinentId = new SelectList(db.Continents, "ContinentId", "Name", destination.ContinentId);
            return View(destination);
        }

        // POST: Destinations/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "DestinationId,ClientId,ContinentId,Title,Content,CreationDate")] Destination destination)
        {
            if (ModelState.IsValid)
            {
                db.Entry(destination).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ClientId = new SelectList(db.Clients, "ClientId", "ClientName", destination.ClientId);
            ViewBag.ContinentId = new SelectList(db.Continents, "ContinentId", "Name", destination.ContinentId);
            return View(destination);
        }

        // GET: Destinations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Destination destination = db.Destinations.Find(id);
            if (destination == null)
            {
                return HttpNotFound();
            }
            return View(destination);
        }

        // POST: Destinations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Destination destination = db.Destinations.Find(id);
            db.Destinations.Remove(destination);
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

      

        public ActionResult Statistics()
        {
            var query =
                from destination in db.Destinations
                join client in db.Clients on destination.ClientId equals client.ClientId
                select new DestinationComment
                {
                    Title = destination.Title,
                    NumberOfComment = destination.Comments.Count,
                    AuthorFullName = client.FirstName + " " + client.LastName
                };

            return View(query.ToList());
        }

        public ActionResult DetailsByTitle(string title)
        {
            if (title == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var destination = db.Destinations.FirstOrDefault(x => x.Title == title);

            if (destination == null)
            {
                return HttpNotFound();
            }

            return View("Details", destination);
        }

        [HttpGet]
        public ActionResult Search(string content, string title, DateTime? date)
        {
            var queryDestinations = new List<Destination>();

            foreach (var destination in db.Destinations)
            {
                if (!string.IsNullOrEmpty(content) && destination.Content.ToLower().Contains(content.ToLower()))
                {
                    queryDestinations.Add(destination);
                }
                else if (!string.IsNullOrEmpty(title) && destination.Title.ToLower().Contains(title.ToLower()))
                {
                    queryDestinations.Add(destination);
                }
                else if (date != null)
                {
                    var formattedDateDestination = destination.CreationDate.ToString("MM/dd/yyyy");
                    var formattedDate = date.Value.ToString("MM/dd/yyyy");

                    if (formattedDateDestination.Equals(formattedDate))
                    {
                        queryDestinations.Add(destination);
                    }
                }
            }

            return View(queryDestinations.OrderByDescending(x => x.CreationDate));
        }

        public ActionResult PostComment(int clientId, int destinationId, string content)
        {
            //if (!AuthorizationMiddleware.Authorized(Session)) return RedirectToAction("Index", "Home");

            var comment = new Comment
            {
                Content = content,
                ClientId = clientId,
                DestinationId = destinationId,
                CreationDate = DateTime.Now
            };

            db.Comments.Add(comment);
            db.SaveChanges();

            return RedirectToAction("Index");
        }
        public JsonResult StatsJson()
        {
            var query =
                from destination in db.Destinations
                join client in db.Clients on destination.ClientId equals client.ClientId
                select new DestinationComment
                {
                    Title = destination.Title,
                    NumberOfComment = destination.Comments.Count,
                    AuthorFullName = client.FirstName + " " + client.LastName
                };
            var data = Json(query.ToList(), JsonRequestBehavior.AllowGet);

            return data;
        }
    }
}
