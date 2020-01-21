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
    public class questionController : Controller
    {
        private readonly AppliContext _context;

        public questionController(AppliContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.questions.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var question = await _context.questions.FirstOrDefaultAsync(m => m.id == id);
            if (question == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(question);
        }

        public IActionResult Create()
        {
            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fkuser,questionmessage, questiondate")] question question, string questionmessage)
        {
            if (ModelState.IsValid)
            {
                var userid = HttpContext.Session.GetInt32("userid");
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);
                
                if (currentUser == null) {
                    return RedirectToAction("customErrorPage", "user");
                }
                else {
                    question.fkuser = currentUser;
                    question.questiondate = DateTimeOffset.Now;

                    _context.Add(question);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index", "Home");
                }
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> adminDashboard()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");
            ViewBag.listProject = HttpContext.Session.GetString("projects");
            List<user> users = _context.users.ToList();
            List<question> questions = _context.questions.ToList();
            // for answers section
            List<answer> answers = _context.answers.ToList();
            HttpContext.Session.SetString("answers", JsonConvert.SerializeObject(answers));
            ViewBag.listAnswer = HttpContext.Session.GetString("answers");

            return View(await _context.questions.ToListAsync());
        }

        public async Task<IActionResult> validateQuestion(int? id)
        {
            var question = await _context.questions.FindAsync(id);
            
            question.validatedQuestion = true; 

            _context.Update(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "question");
        }

        public async Task<IActionResult> blockQuestion(int? id)
        {
            var question = await _context.questions.FindAsync(id);
            
            question.validatedQuestion = false; 

            _context.Update(question);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "question");
        }

        

    }
}