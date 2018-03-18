using System;
using System.Data;
using PagedList;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OwinAuth.DAL;
using OwinAuth.Models;
using System.IO;
using System.Threading.Tasks;
using OwinAuth.Utils;
using OwinAuth.AuthAttribute;
using OwinAuth.ViewModels;

namespace OwinAuth.Controllers
{
    public class AnnoucementsController : Controller
    {
        private SchoolContext db = new SchoolContext();
        [Authorize(Roles = "Admin, Member")]
        public ActionResult Index(string sortOrder, int? page)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.CreatedDateSortParm = string.IsNullOrEmpty(sortOrder) ? "CreatedDate_desc" : "";
            ViewBag.TitleSortParm = sortOrder == "Title_asc" ? "Title_desc" : "Title_asc";
            ViewBag.PublishedDateSortParm = sortOrder == "StartDate_asc" ? "StartDate_desc" : "StartDate_asc";
            ViewBag.ExpiredDateSortParm = sortOrder == "EndDate_asc" ? "EndDate_desc" : "EndDate_asc";
            ViewBag.OrderSortParm = sortOrder == "Order_asc" ? "Order_desc" : "Order_asc";

            var annoucements = from s in db.Annoucements
                               select new AnnouncementViewModel {
                                   Id = s.Id,
                                   Title = s.Title,
                                   Details = s.Details,
                                   StartDate = s.StartDate,
                                   EndDate = s.EndDate,
                                   Priority = s.Priority,
                                   IsPopup = s.IsPopup,
                                   IsPublished = s.IsPublished,
                                   CreatedDateTime = s.CreatedDateTime
                               };
            switch (sortOrder)
            {
                case "CreatedDate_asc":
                    annoucements = annoucements.OrderBy(s => s.CreatedDateTime);
                    break;
                case "Title_desc":
                    annoucements = annoucements.OrderByDescending(s => s.Title);
                    break;
                case "Title_asc":
                    annoucements = annoucements.OrderBy(s => s.Title);
                    break;
                case "StartDate_desc":
                    annoucements = annoucements.OrderByDescending(s => s.StartDate);
                    break;
                case "StartDate_asc":
                    annoucements = annoucements.OrderBy(s => s.StartDate);
                    break;
                case "EndDate_desc":
                    annoucements = annoucements.OrderByDescending(s => s.EndDate);
                    break;
                case "EndDate_asc":
                    annoucements = annoucements.OrderBy(s => s.EndDate);
                    break;
                case "Order_asc":
                    annoucements = annoucements.OrderBy(s => s.Priority);
                    break;
                case "Order_desc":
                    annoucements = annoucements.OrderByDescending(s => s.Priority);
                    break;
                default:
                    annoucements = annoucements.OrderByDescending(s => s.CreatedDateTime);
                    break;
            }
            int pageSize = 10;
            int pageNumber = (page ?? 1);
            return View(annoucements.ToPagedList(pageNumber, pageSize));
        }
        public async Task<ActionResult> RenderImage(int id)
        {
            Annoucement item = await db.Annoucements.FindAsync(id);

            byte[] photoBack = item.Image;

            return File(photoBack, "image/png");
        }

        [Authorize(Roles = "Member")]
        public ActionResult Details(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Annoucement annoucement = db.Annoucements.Find(id);
            if (annoucement == null)
            {
                return HttpNotFound();
            }
            AnnouncementViewModel viewmodel = new AnnouncementViewModel();
            viewmodel.Id = annoucement.Id;
            viewmodel.Title = annoucement.Title;
            viewmodel.Details = annoucement.Details;
            viewmodel.StartDate = annoucement.StartDate;
            viewmodel.EndDate = annoucement.EndDate;
            viewmodel.CreatedDateTime = annoucement.CreatedDateTime;
            viewmodel.IsPopup = annoucement.IsPopup;
            viewmodel.Image = annoucement.Image;
            viewmodel.IsPublished = annoucement.IsPublished;
            viewmodel.Priority = annoucement.Priority;
            return View(viewmodel);
        }

        [Authorize]
        public ActionResult Create()
        {
            AnnouncementViewModel item = new AnnouncementViewModel();
            item.StartDate = DateTime.Now;
            item.EndDate = DateTime.Now;
            return View(item);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(AnnouncementViewModel viewmodel)
        {
            HttpPostedFileBase file = viewmodel.ImageStore;
            dynamic showMessageString = string.Empty;
            if (ModelState.IsValid && file != null)
            {
                if (file != null && file.FileName != null && file.FileName != "")
                {
                    var fileExtension = new string[] { "jpeg", "jpg", "png", "gif", "bmp", ".jpeg", ".jpg", ".png", ".gif", ".bmp" };
                    FileInfo fi = new FileInfo(file.FileName);
                    if (fileExtension.Contains(fi.Extension.ToLower()))
                    {
                        var announcementEntity = new Annoucement();
                        announcementEntity.Title = viewmodel.Title;
                        announcementEntity.Details = viewmodel.Details;
                        announcementEntity.StartDate = viewmodel.StartDate;
                        announcementEntity.EndDate = viewmodel.EndDate;
                        announcementEntity.IsPopup = viewmodel.IsPopup;
                        announcementEntity.Priority = viewmodel.Priority;
                        announcementEntity.Image = FileHelper.ConvertToBytes(file);
                        announcementEntity.CreatedDateTime = DateTime.Now;
                        announcementEntity.Id = Guid.NewGuid();
                        
                        try
                        {
                            db.Annoucements.Add(announcementEntity);
                            db.SaveChanges();
                            showMessageString = new
                            {
                                param1 = 200,
                                param2 = "Insert data successfully !!!"
                            };
                        }
                        catch (Exception)
                        {
                            showMessageString = new
                            {
                                param1 = 404,
                                param2 = "Exception occured while insert database"
                            };
                        }
                    }
                }
            }
            else
            {
                showMessageString = new
                {
                    param1 = 404,
                    param2 = "Input data is not valid!"
                };
            }
            return Json(showMessageString, JsonRequestBehavior.AllowGet);
        }

        [Authorize]
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Annoucement annoucement = db.Annoucements.Find(id);
            if (annoucement == null)
            {
                return HttpNotFound();
            }
            AnnouncementViewModel viewmodel = new AnnouncementViewModel();
            viewmodel.Id = annoucement.Id;
            viewmodel.Title = annoucement.Title;
            viewmodel.Details = annoucement.Details;
            viewmodel.StartDate = annoucement.StartDate;
            viewmodel.EndDate = annoucement.EndDate;
            viewmodel.CreatedDateTime = annoucement.CreatedDateTime;
            viewmodel.IsPopup = annoucement.IsPopup;
            viewmodel.IsPublished = annoucement.IsPublished;
            viewmodel.Image = annoucement.Image;
            viewmodel.Priority = annoucement.Priority;
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit([Bind(Include = "Id,Title,Details,Image,StartDate,EndDate,IsPopup,Priority")] AnnouncementViewModel annoucement)
        {
            if (ModelState.IsValid)
            {
                var annoucementForEdit = db.Annoucements.Where(x => x.Id == annoucement.Id).Single();
                annoucementForEdit.Title = annoucement.Title;
                annoucementForEdit.Details = annoucement.Details;
                annoucementForEdit.StartDate = annoucement.StartDate;
                annoucementForEdit.EndDate = annoucement.EndDate;
                annoucementForEdit.IsPopup = annoucement.IsPopup;
                annoucementForEdit.Priority = annoucement.Priority;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(annoucement);
        }

        [Authorize]
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Annoucement annoucement = db.Annoucements.Find(id);
            if (annoucement == null)
            {
                return HttpNotFound();
            }
            AnnouncementViewModel viewmodel = new AnnouncementViewModel();
            viewmodel.Id = annoucement.Id;
            viewmodel.Title = annoucement.Title;
            viewmodel.Details = annoucement.Details;
            viewmodel.StartDate = annoucement.StartDate;
            viewmodel.EndDate = annoucement.EndDate;
            viewmodel.CreatedDateTime = annoucement.CreatedDateTime;
            viewmodel.IsPopup = annoucement.IsPopup;
            viewmodel.Image = annoucement.Image;
            viewmodel.IsPublished = annoucement.IsPublished;
            viewmodel.Priority = annoucement.Priority;
            return View(viewmodel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Annoucement annoucement = db.Annoucements.Find(id);
            db.Annoucements.Remove(annoucement);
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
    }
}
