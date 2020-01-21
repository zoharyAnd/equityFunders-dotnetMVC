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
    public class answerController : Controller
    {
        private readonly AppliContext _context;

        public answerController(AppliContext context)
        {
            _context = context;
        }

        public String Index()
        {
            return typeof(Controller).Assembly.GetName().Version.ToString ();
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var answer = await _context.answers.FirstOrDefaultAsync(m => m.id == id);
            if (answer == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(answer);
        }

        public IActionResult Create()
        {
            return View();
        }
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fkquestion,fkuser,answermessage, answerdate")] answer answer, int fkquestion, string answermessage)
        {
            if (ModelState.IsValid)
            {
                var userid = HttpContext.Session.GetInt32("userid");
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);
                
                if (currentUser == null) {
                    return RedirectToAction("customErrorPage", "user");
                }
                else {
                    var currentQuestion = await _context.questions.FirstOrDefaultAsync(m => m.id == fkquestion);
                    
                    if (currentQuestion == null) {
                        return RedirectToAction("customErrorPage", "user");
                    }
                    else {
                        answer.fkquestion = currentQuestion;
                        answer.fkuser = currentUser;
                        

                        _context.Add(answer);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                
            }
            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> validateAnswer(int? id)
        {
            var answer = await _context.answers.FindAsync(id);
            
            answer.validatedAnswer = true; 

            _context.Update(answer);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "question");
        }

        public async Task<IActionResult> blockAnswer(int? id)
        {
            var answer = await _context.answers.FindAsync(id);
            
            answer.validatedAnswer = false; 

            _context.Update(answer);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "question");
        }

        

    }
}