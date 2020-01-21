using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using cFunding.Data;
using cFunding.Models;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace cFunding.Controllers
{
    public class TinyMCEController : Controller
    {
        private readonly AppliContext _context;

        public TinyMCEController(AppliContext context)
        {
            _context = context;
        }

        public ActionResult Index()
        {
            return View();
        }

        // An action that will accept your Html Content
        [HttpPost]
        public ActionResult Index(tinyMCE model)
        {
            return View();
        }
    }
}