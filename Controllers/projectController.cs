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
using System.IO;
using System.Web;
using cfunding.Services;
using Newtonsoft.Json;
using PayPal.Api;
using Microsoft.AspNetCore.Http.Extensions;

namespace cFunding.Controllers
{
    public class projectController : Controller
    {
        private readonly AppliContext _context;

        public projectController(AppliContext context)
        {
            _context = context;
        }

        // GET: project
        public IActionResult Index()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");

            List<category> categories = _context.categories.ToList();
            HttpContext.Session.SetString("categories", JsonConvert.SerializeObject(categories));
            ViewBag.listCategory = HttpContext.Session.GetString("categories");

            List<user> users = _context.users.ToList();
            HttpContext.Session.SetString("users", JsonConvert.SerializeObject(users));
            
            List<project> projects = _context.projects.ToList();
            HttpContext.Session.SetString("projects", JsonConvert.SerializeObject(projects));
            ViewBag.listProject = HttpContext.Session.GetString("projects");

            //for project cards favorites 
            var userFavs = HttpContext.Session.GetString("userFavs");
            ViewBag.listFavs = userFavs;
            
            /* filter : take only validated projects */
            var listValidPj = _context.projects.Where(p => p.validatedProject == true);
            return View(listValidPj);
        }

        // GET: project/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // retrieve the corresponding project
            var project = await _context.projects.FirstOrDefaultAsync(m => m.id == id);
            
            // retrieve all sessions variables needed for the navigation rendering
            var userSession = Convert.ToInt32(HttpContext.Session.GetInt32("userid")); 
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");
            List<user> users = _context.users.ToList();
            List<category> categories = _context.categories.ToList();
            List<project> projects = _context.projects.ToList();
            
            // set a Json Object representing a list of projects for display in the view
            ViewBag.listProject =JsonConvert.SerializeObject(projects);
            
            //safe encode project description
            var encodedDescription = HttpUtility.HtmlEncode(project.projectdescription);
            //ViewBag.pdesc = encodedDescription;
            
            ViewBag.pdesc = project.projectdescription; 
            // identify if the connected user is the owner of the project or not for edit button display
            if (userSession != project.fkuser.id){
                ViewBag.anotherUser = "true";
            }
            else{
                ViewBag.anotherUser = "false";
            }

            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            
            if (project == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            //  retrieve project owner company sharevalues for the donation panel
            foreach (var itemUser in users)
            {
                if (project.fkuser.id == itemUser.id){
                    ViewBag.shareValueOrdinary = itemUser.sharevalueordinary;
                    ViewBag.shareValuePreferencial = itemUser.sharevaluepreferencial;
                    ViewBag.shareValueNonvoting = itemUser.sharevaluenonvoting;
                    ViewBag.shareValueRedeemable = itemUser.sharevalueredeemable;
                    ViewBag.descordinary = itemUser.descordinary;
                    ViewBag.descpreferencial = itemUser.descpreferencial;
                    ViewBag.descnonvoting = itemUser.descnonvoting;
                    ViewBag.descredeemable = itemUser.descredeemable;
                    ViewBag.creatorName = itemUser.username;
                    ViewBag.creatorPhoto = itemUser.userphoto;
                    ViewBag.creatorCountry = itemUser.usercountry; 
                    ViewBag.companyName = itemUser.companyname;
                }
            }

            // retrieve transaction list to determine the number of backers 
            List<transaction> transactions = _context.transactions.ToList();
            int count=0;
            foreach (var itemTransaction in transactions)
            {
                if (itemTransaction.fkproject.id == id){
                    count++;
                }
            }
            ViewBag.nbInvestors = count;
            HttpContext.Session.SetString("projectName", project.projectname);

            return View(project);
        }


        // GET: project/Create
        public IActionResult Create()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");
            ViewBag.listUser = HttpContext.Session.GetString("users");
            ViewBag.listCategory = HttpContext.Session.GetString("categories");

            //find the owner of the project and retrieve his/her number of shares
            List<user> users = _context.users.ToList();
            foreach (var item in users)
            {
                if (item.id == HttpContext.Session.GetInt32("userid")){
                    ViewBag.nbshareordinary = item.nbshareordinary;
                    ViewBag.nbsharepreferencial = item.nbsharepreferencial;
                    ViewBag.nbsharenonvoting = item.nbsharenonvoting;
                    ViewBag.nbshareredeemable = item.nbshareredeemable;
                }
            }

            return View();
        }

        // POST: project/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,projectname,projectdescription,projectamounttoraise,projectcreationdate,projectclosingdate,projectimage1,projectimage2,projectimage3,projectimage4,fkuserid,fkcategoryid,nbshareordinary,nbsharepreferencial,nbsharenonvoting,nbshareredeemable")] project project, IFormFile fileOne, IFormFile fileTwo, IFormFile fileThree, IFormFile fileFour, int fkuser, int fkcategory, int nbshareordinary, int nbsharepreferencial, int nbsharenonvoting, int nbshareredeemable)
        {
            if (ModelState.IsValid)
            {
                // photo upload 1 
                if (fileOne == null || fileOne.Length == 0)
                {
                    project.projectimage1 = "images/noImageAvailable.jpg";
                }else{
                    // if a photo was uploaded save it into the server
                    var path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileOne.FileName);  
                    using (var stream1 = new FileStream(path1, FileMode.Create))  
                    {  
                        await fileOne.CopyToAsync(stream1);  
                    }  
                    project.projectimage1 = "images/" + fileOne.FileName;
                }  

                // photo upload 2
               if (fileTwo == null || fileTwo.Length == 0)
                {
                    project.projectimage2 = "images/noImageAvailable.jpg";
                }else{
                    var path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileTwo.FileName);  
                    using (var stream2 = new FileStream(path2, FileMode.Create))  
                    {  
                        await fileTwo.CopyToAsync(stream2);  
                    }  
                    project.projectimage2 = "images/" + fileTwo.FileName;
                }
            
                // photo upload 3
               if (fileThree == null || fileThree.Length == 0)
               {
                   project.projectimage3 = "images/noImageAvailable.jpg";
               }else{
                    var path3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileThree.FileName);  
                    using (var stream3 = new FileStream(path3, FileMode.Create))  
                    {  
                        await fileThree.CopyToAsync(stream3);  
                    }  
                    project.projectimage3 = "images/" + fileThree.FileName;
               }  
                
                // photo upload 4
               if (fileFour == null || fileFour.Length == 0)
               {
                   project.projectimage4 = "images/noImageAvailable.jpg";
               }else{
                   var path4 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileFour.FileName);  
                using (var stream4 = new FileStream(path4, FileMode.Create))  
                {  
                    await fileFour.CopyToAsync(stream4);  
                }  
                project.projectimage4 = "images/" + fileFour.FileName;

               }
                // select the user who owns the project
                project.fkuser = (user)_context.users.First(user => user.id == fkuser);
                // select the project category
                project.fkcategory = (category)_context.categories.First(category => category.id == fkcategory);

                // add the project record and save changes
                _context.Add(project);
                await _context.SaveChangesAsync();
                
                //update the owner number of shares and save changes
                user currentUser =  _context.users.First(user => user.id == fkuser);
                currentUser.nbshareordinary = currentUser.nbshareordinary - nbshareordinary;
                currentUser.nbsharepreferencial = currentUser.nbsharepreferencial - nbsharepreferencial;
                currentUser.nbsharenonvoting = currentUser.nbsharenonvoting- nbsharenonvoting;
                currentUser.nbshareredeemable = currentUser.nbshareredeemable - nbshareredeemable;
                
                _context.users.Update(currentUser);
                await _context.SaveChangesAsync();

                return RedirectToAction("Details", new { id = project.id });
            }
            else{
                var errors = ModelState.Select(x => x.Value.Errors).Where(y =>y.Count>0).ToList();

            }
            //return View(project);
            return RedirectToAction("Index", "Home");
            
        }

        // GET: project/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");
            ViewBag.listUser = HttpContext.Session.GetString("users");
            ViewBag.listCategory = HttpContext.Session.GetString("categories");
            List<user> users = _context.users.ToList();
            List<category> categories = _context.categories.ToList();

            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var project = await _context.projects.FindAsync(id);
            if (project == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }
            ViewBag.categoryId = project.fkcategory.id;
            ViewBag.projectamountraised = project.projectamountraised;
            return View(project);
        }

        // POST: project/Edit/5
       [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("id,projectname,projectdescription,projectamounttoraise,projectamountraised,projectcreationdate,projectclosingdate,projectimage1,projectimage2,projectimage3,projectimage4,fkuser,fkcategory,nbshareordinary,nbsharepreferencial,nbsharenonvoting,nbshareredeemable")] project project, IFormFile fileOne, IFormFile fileTwo, IFormFile fileThree, IFormFile fileFour, int fkuser, int fkcategory)
        {
            if (id != project.id)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            try
            {
                // photo upload 1 
                if (fileOne == null || fileOne.Length == 0)
                {
                    project.projectimage1 = project.projectimage1;
                }
                else
                {

                    var path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileOne.FileName);
                    using (var stream1 = new FileStream(path1, FileMode.Create))
                    {
                        await fileOne.CopyToAsync(stream1);
                    }
                    project.projectimage1 = "images/" + fileOne.FileName;
                }

                // photo upload 2
                if (fileTwo == null || fileTwo.Length == 0)
                {
                    project.projectimage2 = project.projectimage2;
                }
                else
                {
                    var path2 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileTwo.FileName);
                    using (var stream2 = new FileStream(path2, FileMode.Create))
                    {
                        await fileTwo.CopyToAsync(stream2);
                    }
                    project.projectimage2 = "images/" + fileTwo.FileName;
                }

                // photo upload 3
                if (fileThree == null || fileThree.Length == 0)
                {
                    project.projectimage3 = project.projectimage3;
                }
                else
                {
                    var path3 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileThree.FileName);
                    using (var stream3 = new FileStream(path3, FileMode.Create))
                    {
                        await fileThree.CopyToAsync(stream3);
                    }
                    project.projectimage3 = "images/" + fileThree.FileName;
                }

                // photo upload 4
                if (fileFour == null || fileFour.Length == 0)
                {
                    project.projectimage4 = project.projectimage4;
                }
                else
                {
                    var path4 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileFour.FileName);
                    using (var stream4 = new FileStream(path4, FileMode.Create))
                    {
                        await fileFour.CopyToAsync(stream4);
                    }
                    project.projectimage4 = "images/" + fileFour.FileName;

                }

                project.fkuser = (user)_context.users.First(user => user.id == fkuser);
                project.fkcategory = (category)_context.categories.First(category => category.id == fkcategory);

                project.projectamountraised = project.projectamountraised;
                project.validatedProject = false;
                _context.Update(project);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!projectExists(project.id))
                {
                    return RedirectToAction("customErrorPage", "user");
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", new { id = project.id });
            
        }

        // GET: project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var project = await _context.projects
                .FirstOrDefaultAsync(m => m.id == id);
            if (project == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(project);
        }

        // POST: project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.projects.FindAsync(id);
            _context.projects.Remove(project);
            await _context.SaveChangesAsync();
            
            //return RedirectToAction(nameof(Index));
            return RedirectToAction("adminDashboard", "project");
        }

        private bool projectExists(int id)
        {
            return _context.projects.Any(e => e.id == id);
        }

         //getting the apiContext
        public async Task<IActionResult> ValidateProject(int? id)
        {
            var project = await _context.projects.FindAsync(id);

            project.validatedProject = true;

            _context.Update(project);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "project");
        }

        public async Task<IActionResult> BlockProject(int? id)
        {
            var project = await _context.projects.FindAsync(id);

            project.validatedProject = false;

            _context.Update(project);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "project");
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

            return View(await _context.projects.ToListAsync());
        }

        public IActionResult fundedProjects(int? id)
        {
            ViewBag.urlHome = HttpContext.Session.GetString("urlHome");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");

            List<category> categories = _context.categories.ToList();
            List<user> users = _context.users.ToList();
            List<String> listEmail = new List<string>();
            foreach (var item in users)
            {
                listEmail.Add(item.useremail);
            }
            HttpContext.Session.SetString("emails", JsonConvert.SerializeObject(listEmail));
            ViewBag.listEmail = HttpContext.Session.GetString("emails");

            List<project> projects = _context.projects.ToList();

            /* filter : take projects created by this user*/
            var listFunded = _context.projects.Where(p => p.projectamounttoraise == p.projectamountraised);
            return View(listFunded);
        }

        
        public IActionResult MyProjects(int? id)
        {
            ViewBag.urlHome = HttpContext.Session.GetString("urlHome");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");

            List<category> categories = _context.categories.ToList();
            List<user> users = _context.users.ToList();
            List<String> listEmail = new List<string>();
            foreach (var item in users)
            {
                listEmail.Add(item.useremail);
            }
            HttpContext.Session.SetString("emails", JsonConvert.SerializeObject(listEmail));
            ViewBag.listEmail = HttpContext.Session.GetString("emails");

            List<project> projects = _context.projects.ToList();

            /* filter : take projects created by this user*/
            var listValidPj = _context.projects.Where(p => p.fkuser.id == id);
            return View(listValidPj);
        }

        PayPalPaymentService paypalService = new PayPalPaymentService();

        [HttpPost]
        public IActionResult CreatePayment(string tempTotalPrice, int id, string subOrdinary, string subPreferencial, string subNonvoting, string subRedeemable, string paymentEmail)
        {
            paypalService.projectId = id;
            paypalService.donationAmount = tempTotalPrice;
            paypalService.pname = HttpContext.Session.GetString("projectName");
            
            HttpContext.Session.SetInt32("pjid", id);
            HttpContext.Session.SetString("donation", ""+tempTotalPrice);
            HttpContext.Session.SetString("paymentEmail", ""+paymentEmail);

            int intOrdinary = Convert.ToInt32(subOrdinary);
            int intPreferencial = Convert.ToInt32(subPreferencial);
            int intNonvoting = Convert.ToInt32(subNonvoting);
            int intRedeemable = Convert.ToInt32(subRedeemable);
            HttpContext.Session.SetInt32("subOrdinary", intOrdinary);
            HttpContext.Session.SetInt32("subPreferencial", intPreferencial);
            HttpContext.Session.SetInt32("subNonvoting", intNonvoting);
            HttpContext.Session.SetInt32("subRedeemable", intRedeemable);

            string baseUrl = Request.Scheme + "://" + Request.Host + "/Project/";
            var payment = paypalService.CreatePayment(baseUrl, "sale");

            return Redirect(payment.GetApprovalUrl());
        }
        public IActionResult PaymentCancelled()
        {
            return View();
        }
       
        public async Task<IActionResult> PaymentSuccessfull(string paymentId, string token, string PayerID)
        {
             List<user> users = _context.users.ToList();
              List<category> categories = _context.categories.ToList();
            // Execute Payment
            var payment = PayPalPaymentService.ExecutePayment(paymentId, PayerID);
            var pjid = HttpContext.Session.GetInt32("pjid");
            var currentProject = await _context.projects.FirstOrDefaultAsync(m => m.id == pjid);
            int userSub = currentProject.fkuser.id;
            HttpContext.Session.SetInt32("userSub", userSub);

            var donation = HttpContext.Session.GetString("donation");
            currentProject.projectamountraised = currentProject.projectamountraised + Double.Parse(donation);
            int subOrdinary = Convert.ToInt32(HttpContext.Session.GetInt32("subOrdinary"));
            int subPreferencial = Convert.ToInt32(HttpContext.Session.GetInt32("subPreferencial"));
            int subNonvoting = Convert.ToInt32(HttpContext.Session.GetInt32("subNonvoting"));
            int subRedeemable = Convert.ToInt32(HttpContext.Session.GetInt32("subRedeemable"));

            currentProject.nbshareordinary = currentProject.nbshareordinary - subOrdinary;
            currentProject.nbsharepreferencial = currentProject.nbsharepreferencial - subPreferencial;
            currentProject.nbsharenonvoting = currentProject.nbsharenonvoting - subNonvoting;
            currentProject.nbshareredeemable = currentProject.nbshareredeemable - subRedeemable;

            _context.Update(currentProject);
            await _context.SaveChangesAsync();
            
            //updateUserShares();
            return RedirectToAction("createNewTransaction", "project");
        }

        public IActionResult AuthorizeSuccessful()
        {
            return View();
        }

        public async Task<IActionResult> updateUserShares(){
            int userSub = Convert.ToInt32(HttpContext.Session.GetInt32("userSub"));
            int subOrdinary = Convert.ToInt32(HttpContext.Session.GetInt32("subOrdinary"));
            int subPreferencial = Convert.ToInt32(HttpContext.Session.GetInt32("subPreferencial"));
            int subNonvoting = Convert.ToInt32(HttpContext.Session.GetInt32("subNonvoting"));
            int subRedeemable = Convert.ToInt32(HttpContext.Session.GetInt32("subRedeemable"));

            var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userSub);
            currentUser.nbshareordinary = currentUser.nbshareordinary - subOrdinary;
            currentUser.nbsharepreferencial = currentUser.nbsharepreferencial - subPreferencial;
            currentUser.nbsharenonvoting = currentUser.nbsharenonvoting - subNonvoting;
            currentUser.nbshareredeemable = currentUser.nbshareredeemable - subRedeemable;

            _context.Update(currentUser);
            await _context.SaveChangesAsync();
            return Redirect("PaymentSuccessfull");
        }

        public async Task<IActionResult> createNewTransaction(){
            int fkproject = Convert.ToInt32(HttpContext.Session.GetInt32("pjid"));
            int fkuser = Convert.ToInt32(HttpContext.Session.GetInt32("userid"));
            Double amount = Double.Parse(HttpContext.Session.GetString("donation"));
            string accountEmail = HttpContext.Session.GetString("paymentEmail");
            DateTimeOffset transactiondate = DateTimeOffset.Now; 

            cFunding.Models.transaction currentTransaction = new cFunding.Models.transaction();
            var fkprojectM = await _context.projects.FirstOrDefaultAsync(m => m.id == fkproject);
            currentTransaction.fkproject = fkprojectM;
            var fkuserM = await _context.users.FirstOrDefaultAsync(p => p.id == fkuser);
            currentTransaction.fkuser = fkuserM;
            currentTransaction.amount = amount;
            currentTransaction.accountemail = accountEmail;
            currentTransaction.transactiondate = transactiondate;

            _context.Add(currentTransaction);
            await _context.SaveChangesAsync();
            
            return View("PaymentSuccessfull");
        }

    }
}
