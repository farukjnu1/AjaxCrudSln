using AjaxCrud.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AjaxCrud.ViewModels
{
    public class VmStudent:Student
    {
        public HttpPostedFileBase imgFile { get; set; }
    }
}