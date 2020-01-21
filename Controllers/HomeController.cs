using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using System.Collections;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using cFunding.Models;
using cFunding.Data;
using Newtonsoft.Json;
using PayPal.Api;


namespace cFunding.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppliContext _context;
        public HomeController(AppliContext context)
        {
            _context = context;
        }

        public IActionResult Index(string param=null)
        {
            var url = HttpContext.Request.GetEncodedUrl();
            HttpContext.Session.SetString("urlHome", url);
            ViewBag.urlHome = HttpContext.Session.GetString("urlHome");

            var urlBase = Request.Scheme + "://" + Request.Host ;
            ViewBag.urlBase = urlBase;

            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");

            List<category> categories = _context.categories.ToList();
            HttpContext.Session.SetString("categories", JsonConvert.SerializeObject(categories));
            
            List<user> users = _context.users.ToList();
            HttpContext.Session.SetString("users", JsonConvert.SerializeObject(users));
            
            //for email validation in register modal
            List<String> listEmail = new List<string>();
            foreach (var item in users)
            {
                listEmail.Add(item.useremail);
            }
            HttpContext.Session.SetString("emails", JsonConvert.SerializeObject(listEmail));
            ViewBag.listEmail = HttpContext.Session.GetString("emails");

            // for ask questions section
            List<question> questions = _context.questions.ToList();
            HttpContext.Session.SetString("questions", JsonConvert.SerializeObject(questions));
            ViewBag.listQuestion = HttpContext.Session.GetString("questions");
            // for answers section
            List<answer> answers = _context.answers.ToList();
            HttpContext.Session.SetString("answers", JsonConvert.SerializeObject(answers));
            ViewBag.listAnswer = HttpContext.Session.GetString("answers");

            // for project card section
            List<project> projects = _context.projects.ToList();
            HttpContext.Session.SetString("projects", JsonConvert.SerializeObject(projects));

            //for project cards favorites 
            var userFavs = HttpContext.Session.GetString("userFavs");
            ViewBag.listFavs = userFavs;

            /* filter : take only validated projects */
            var listValidPj = _context.projects.Where(p => p.validatedProject == true);
            return View(listValidPj);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        
        [HttpPost]
        [ValidateAntiForgeryToken]  
        public async Task<IActionResult> login(String loginEmail, String loginPassword)
        {
            var email = loginEmail; 
            var pwd = loginPassword;  
            
            var currentUser =  await _context.users.FirstOrDefaultAsync(m => m.useremail == email && m.userpassword == pwd);
            
            if (currentUser == null)
            {
                // login credentials not found 
                HttpContext.Session.SetString("loginFailed", "true");
            }
            else
            {
                if(currentUser.username == null){
                    HttpContext.Session.SetString("username", "");    
                }
                else{
                    HttpContext.Session.SetString("username", currentUser.username);
                }
                    HttpContext.Session.SetString("loginFailed", "false");
                    HttpContext.Session.SetInt32("userid", currentUser.id);
                    HttpContext.Session.SetString("userphoto", currentUser.userphoto); 
                    HttpContext.Session.SetString("isadmin", currentUser.isadmin.ToString()); 
                    HttpContext.Session.SetString("useremail", currentUser.useremail); 
                    
                    HttpContext.Session.SetString("validatedUser", currentUser.validatedUser.ToString());
                    
                    List<user> users = _context.users.ToList();
                    List<project> projects = _context.projects.ToList();
                    List<favorite> favorites = _context.favorites.ToList();
                    List<String> listFavorites = new List<string>();

                    foreach (var itemF in favorites)
                    {
                        if(itemF.fkuser.id == Convert.ToInt32(HttpContext.Session.GetInt32("userid"))){
                            listFavorites.Add(itemF.fkproject.id.ToString());
                        }
                    }
                    HttpContext.Session.SetString("userFavs", JsonConvert.SerializeObject(listFavorites));
                    HttpContext.Session.GetString("userFavs");

                    if(currentUser.isadmin == true){
                        return RedirectToAction("adminDashboard", "Home");
                    }
                
            }
            
            return RedirectToAction("Index"); 
        }

        public ActionResult logout(){
            HttpContext.Session.SetString("userid", "");
            HttpContext.Session.SetString("username", "");
            HttpContext.Session.SetString("isadmin", "");
            HttpContext.Session.SetString("useremail", "");
            HttpContext.Session.SetString("userphoto", "images/avatar.jpg");
            HttpContext.Session.SetString("loginFailed", "");
            HttpContext.Session.SetString("validatedUser", "");
            
            return RedirectToAction("Index", "Home"); 
        }
        
        public IActionResult adminDashboard()
        {
            var url = HttpContext.Request.GetEncodedUrl();
            HttpContext.Session.SetString("urlHome", url);
            ViewBag.urlHome = HttpContext.Session.GetString("urlHome");

            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            
            int countValidatedUsers = 0; int countUsers =0; int countValidatedProjects = 0; int countProjects = 0;

            List<user> users = _context.users.ToList();
            foreach (var item in users)
            {
                if (item.validatedUser == true) {countValidatedUsers++;}
                countUsers++;
            }

            List<project> projects = _context.projects.ToList();
            foreach (var item in projects)
            {
                if (item.validatedProject == true) {countValidatedProjects++;}
                countProjects++;
            }

            ViewBag.usersRatio = countValidatedUsers +" /"+ countUsers ;
            ViewBag.projectsRatio = countValidatedProjects +"/"+ countProjects;


            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createAnswer(int fkquestion, int fkuser, string answermessage)
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
                    cFunding.Models.answer answer = new cFunding.Models.answer();
                    answer.fkquestion = currentQuestion;
                    answer.fkuser = currentUser;
                    answer.answermessage = answermessage;
                    answer.answerdate = DateTimeOffset.Now;
                    answer.validatedAnswer = false;

                    _context.Add(answer);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createContactus(int fkuser, string sendername, string senderemail, string mailsubject, string mailmessage)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else {
                cFunding.Models.contactus contactus = new cFunding.Models.contactus();
                contactus.fkuser = currentUser;
                contactus.sendername = sendername;
                contactus.senderemail = senderemail;
                contactus.mailsubject = mailsubject;
                contactus.mailmessage = mailmessage;

                _context.Add(contactus);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> createFavorite(int fkproject)
        {
            var userid = HttpContext.Session.GetInt32("userid");
            var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);

            if (currentUser == null)
            {
                return RedirectToAction("Index", "Home");
            }
            else {
                var currentProject = await _context.projects.FirstOrDefaultAsync(p => p.id == fkproject);

                cFunding.Models.favorite favorite = new cFunding.Models.favorite();
                favorite.fkuser = currentUser;
                favorite.fkproject = currentProject;
                
                _context.Add(favorite);
                await _context.SaveChangesAsync();
                
            }
            return RedirectToAction("Index", "Home");
        }

    }
}
