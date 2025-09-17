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
    public class StudentInfoController : Controller
    {
        // GET: StudentInfo
        public ActionResult Index()
        {
            var db = new Database1Entities();
            var listStudent = db.Students.ToList();
            return View(listStudent);
        }

        [HttpGet]
        public ActionResult InsertStudent()
        {
            return View();
        }

        [HttpPost]
        public ActionResult InsertStudent(VmStudent model)
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

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult UpdateStudent(int id)
        {
            var db = new Database1Entities();
            var oStudent = db.Students.Where(w => w.StudentID == id).FirstOrDefault();
            var oVmStudent = new VmStudent();
            oVmStudent.Address = oStudent.Address;
            oVmStudent.DoB = oStudent.DoB;
            oVmStudent.Picture = oStudent.Picture;
            oVmStudent.StudentID = oStudent.StudentID;
            oVmStudent.StudentName = oStudent.StudentName;

            return View(oVmStudent);
        }

        [HttpPost]
        public ActionResult UpdateStudent(VmStudent model)
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
            var oStudent = db.Students.Where(w => w.StudentID == model.StudentID).FirstOrDefault();
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
                oStudent.Picture = picture == "" ? oStudent.Picture : picture;
                oStudent.StudentName = model.StudentName;
                db.SaveChanges();

                resdata = new { message = "image updated successfully." };
            }

            return RedirectToAction("Index");

        }


        [HttpGet]
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

            return RedirectToAction("Index");
        }

    }
}