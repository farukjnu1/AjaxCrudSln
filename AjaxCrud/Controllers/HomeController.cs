using AjaxCrud.Models;
using AjaxCrud.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AjaxCrud.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetStudents()
        {
            var db = new Database1Entities();
            var listStudent = db.Students.ToList();
            return Json(listStudent, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetStudentById(int id)
        {
            var db = new Database1Entities();
            var oStudent = db.Students.Where(w=>w.StudentID == id).FirstOrDefault();
            return Json(oStudent, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertStudent(VmStudent model)//HttpPostedFileBase file)
        {
            object resdata = null;
            //HttpPostedFileBase file = Request.Files[0];
            string picture = "";
            if (model.imgFile != null && model.imgFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.imgFile.FileName);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);
                model.imgFile.SaveAs(fileLocation);

                picture = "/uploads/" + model.imgFile.FileName;
            }
            var db = new Database1Entities();
            var oStudent = new Student();
            oStudent.Address = model.Address;
            oStudent.DoB = model.DoB;
            oStudent.Picture = picture;
            oStudent.StudentName = model.StudentName;
            db.Students.Add(oStudent);
            db.SaveChanges();
            resdata = new { message = "image inserted successfully." };
            return Json(resdata);
        }

        [HttpPost]
        public ActionResult UpdateStudent(VmStudent model)//HttpPostedFileBase file)
        {
            object resdata = null;
            //HttpPostedFileBase file = Request.Files[0];
            string picture = "";
            if (model.imgFile != null && model.imgFile.ContentLength > 0)
            {
                var fileName = Path.GetFileName(model.imgFile.FileName);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);
                model.imgFile.SaveAs(fileLocation);

                picture = "/uploads/" + model.imgFile.FileName;
            }
            var db = new Database1Entities();
            var oStudent = db.Students.Where(w=>w.StudentID == model.StudentID).FirstOrDefault();
            if (oStudent != null)
            {
                oStudent.Address = model.Address;
                oStudent.DoB = model.DoB;
                if (!string.IsNullOrEmpty(picture))
                {
                    var fileName = Path.GetFileName(oStudent.Picture);
                    string fileLocation = Path.Combine(Server.MapPath("~/uploads"), fileName);
                    if (System.IO.File.Exists(fileLocation))
                    {  
                        System.IO.File.Delete(fileLocation);
                    }
                }
                oStudent.Picture = picture == ""  ? oStudent.Picture : picture;
                oStudent.StudentName = model.StudentName;
                db.SaveChanges();

                resdata = new { message = "image updated successfully." };
            }
            
            return Json(resdata);
        }

        [HttpPost]
        public ActionResult DeleteStudent(int id)
        {
            object resdata = null;
            var db = new Database1Entities();
            var oStudent = db.Students.Where(w => w.StudentID == id).FirstOrDefault();
            if (oStudent != null)
            {
                db.Students.Remove(oStudent);
                db.SaveChanges();

                var fileName = Path.GetFileName(oStudent.Picture);
                string fileLocation = Path.Combine(
                    Server.MapPath("~/uploads"), fileName);

                // Check if file exists with its full path    
                if (System.IO.File.Exists(fileLocation))
                {
                    // If file found, delete it    
                    System.IO.File.Delete(fileLocation);
                }

                resdata = new { message = "image deleted successfully." };
            }
            
            return Json(resdata);
        }

    }
}