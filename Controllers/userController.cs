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
using System.Net.Mail;
using System.Net;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace cFunding.Controllers
{
    public class userController : Controller
    {
        private readonly AppliContext _context;

        public userController(AppliContext context)
        {
            _context = context;
        }

        // GET: user
        public async Task<IActionResult> Index()
        {
            return View(await _context.users.ToListAsync());
        }

        // GET: user/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            // get the user corresponding to the requested id
            var user = await _context.users.FirstOrDefaultAsync(m => m.id == id);
            // retrieve all Session variables needed for the navigation rendering
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.pageOwner = id;
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.listUser = HttpContext.Session.GetString("users");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");

            // identify if the connected owns the profile / project page or not 
            var connectedUser = Convert.ToInt32(HttpContext.Session.GetInt32("userid"));
            if (id != connectedUser){
                ViewBag.anotherUser = "true";
            }
            else{
                ViewBag.anotherUser = "false";
            }
            
            if (id == null || user == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            // list all projects published by the connected user
            List<category> categories = _context.categories.ToList();
            List<user> users = _context.users.ToList();

            var listPublishedProjects = _context.projects.Where(p => p.fkuser.id == id);
            HttpContext.Session.SetString("publishedProjects", JsonConvert.SerializeObject(listPublishedProjects));
            ViewBag.listPublishedProjects = HttpContext.Session.GetString("publishedProjects");;
            return View(user);
        }

        // GET: user/Create
        public IActionResult Create()
        {
            ViewBag.listUser = HttpContext.Session.GetString("users");
            return View();
        }
        
        // POST: user/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,userfname,userlname,username,useremail,userpassword,userphoto,userdob,useraddress,usercountry,companyname,companylogo,companydescription,nbshareordinary,sharevalueordinary,descordinary,additionalordinary,nbsharepreferencial,sharevaluepreferencial,descpreferencial,additionalpreferencial,nbsharenonvoting,sharevaluenonvoting,descnonvoting,additionalnonvoting,nbshareredeemable,sharevalueredeemable,descredeemable,additionalredeemable,userassets")] user user, IFormFile fileuserphoto, IFormFile filecompanylogo)
        {
            if (ModelState.IsValid)
            {
                // user photo upload
               if (fileuserphoto == null || fileuserphoto.Length == 0)  
                    return Content("file not selected");  
            
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileuserphoto.FileName);  
            
                using (var stream = new FileStream(path, FileMode.Create))  
                {  
                    await fileuserphoto.CopyToAsync(stream);  
                }  

                user.userphoto = "images/" + fileuserphoto.FileName; 


                // company logo photo upload
               if (filecompanylogo == null || filecompanylogo.Length == 0)  
                    return Content("file not selected");  
            
                var path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", filecompanylogo.FileName);  
            
                using (var stream1 = new FileStream(path1, FileMode.Create))  
                {  
                    await filecompanylogo.CopyToAsync(stream1);  
                }  

                user.companylogo = "images/" + filecompanylogo.FileName; 
                
                //usetype=false for a normal user
                user.isadmin=false; 

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // user Registration Home Page
        // the useremail and userpassword are Bind to a user object before creating the record 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> registerHome([Bind("useremail,userpassword")] user user)
        {
            if (ModelState.IsValid)
            {
                user.userfname = null; 
                user.userlname = null; 
                user.username = null; 
                user.userphoto = "images/avatar.jpg"; 
                user.userdob = DateTimeOffset.Now; 
                user.useraddress = null;
                user.usercountry = null;
                user.companyname = null; 
                user.companylogo = "images/noImageAvailable.jpg"; 
                user.companydescription = null; 
                user.nbshareordinary = 0;
                user.sharevalueordinary = 0;
                user.descordinary = null;
                user.additionalordinary = null;
                user.nbsharepreferencial = 0;
                user.sharevaluepreferencial = 0;
                user.descpreferencial = null;
                user.additionalpreferencial = null;
                user.nbsharenonvoting = 0;
                user.sharevaluenonvoting = 0;
                user.descnonvoting = null;
                user.additionalnonvoting = null;
                user.nbshareredeemable = 0;
                user.sharevalueredeemable = 0;
                user.descredeemable = null;
                user.additionalredeemable = null;
                user.userassets = 0;
                user.isadmin=false; 
                user.validatedUser=false;
                
                
                _context.Add(user);
                await _context.SaveChangesAsync();

                // configuration of email 
                var emailSender = "cFundingZrsi@gmail.com";
                var password = "cFundingZrsi2019";

                //get the domain of the application and reach the corresponding view 
                var activateUrl = HttpContext.Session.GetString("urlHome")+"user/ActivateEmail/"+user.id;

                // create an smtp client bearing all parameters needed to send the email
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); 
                client.EnableSsl = true; 
                client.Timeout = 100000; 
                client.DeliveryMethod = SmtpDeliveryMethod.Network; 
                client.UseDefaultCredentials = false;  
                client.Credentials = new NetworkCredential(emailSender, password);
                 
                HttpContext.Session.SetInt32("day", DateTime.Now.Day);
                HttpContext.Session.SetInt32("month", DateTime.Now.Month);
                HttpContext.Session.SetInt32("year", DateTime.Now.Year);

                // setting the email content
                var subject = "Cfunding Account Validation"; 
                var body = "Thank you for registering!<br>Please activate your account by clicking the link below:<br><a href='"+activateUrl+"'><img src='http://149.202.210.119:8080/images/btn-activate.png' width='200' height='50'></a><br>Regards,<br>Cfunding Team.<br>This Link will be expired after the "+DateTime.Now.AddDays(1).ToString("MM/dd/yyyy");
                MailMessage mailMessage = new MailMessage (emailSender, user.useremail, subject, body); 
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8; 

                // sending the email
                client.Send(mailMessage); 
                
                return RedirectToAction("Index", "Home");
            }
            return View("~/Views/Home/index.cshtml");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.listUser = HttpContext.Session.GetString("users");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");
            
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var user = await _context.users.FindAsync(id);
            if (user == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }
            return View(user);
        }

        // POST: user/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string userfname, string userlname, string username, string useremail, string userpassword, DateTimeOffset userdob, string useraddress, string usercountry, string companyname, string companydescription, int nbshareordinary, double sharevalueordinary, string descordinary, string additionalordinary, int nbsharepreferencial, double sharevaluepreferencial, string descpreferencial, string additionalpreferencial, int nbsharenonvoting, double sharevaluenonvoting, string descnonvoting, string additionalnonvoting, int nbshareredeemable, double sharevalueredeemable, string descredeemable, string additionalredeemable, double userassets, IFormFile fileUserphotoEdit, IFormFile fileCompanyLogoEdit, string editpart)
        {
            var user = await _context.users.FirstOrDefaultAsync(m => m.id == id);
            if (ModelState.IsValid)
            {
                try
                {
                    if (user == null)
                    {
                        return RedirectToAction("customErrorPage", "user");
                    }
                    else
                    {
                        // if the user profile is to be modified 
                        if (editpart == "edituser"){
                            // upload new user photo
                            if (fileUserphotoEdit == null || fileUserphotoEdit.Length == 0){
                                user.userphoto = user.userphoto;
                            }
                            else {
                                var newUserPhoto = "images/"+fileUserphotoEdit.FileName;
                                
                                if (newUserPhoto == user.userphoto) {
                                    user.userphoto = user.userphoto;
                                } 
                                else {
                                    //save the new photo in the server
                                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileUserphotoEdit.FileName);  

                                    using (var stream = new FileStream(path, FileMode.Create))  
                                    {  
                                        await fileUserphotoEdit.CopyToAsync(stream);  
                                    }  
                                    user.userphoto = "images/" + fileUserphotoEdit.FileName; 
                                } 
                            }
                            // set record's value to the new ones. If no parameter is given keep the old ones 
                            if(userfname == null){user.userfname = user.userfname; }else{user.userfname = userfname;}
                            if(userlname == null){user.userlname = user.userlname; }else{user.userlname = userlname;}
                            if(username == null){user.username = user.username; }else{user.username = username;}
                            if(useremail == null){user.useremail = user.useremail; }else{user.useremail = useremail;}
                            if(userpassword == null){user.userpassword = user.userpassword; }else{user.userpassword = userpassword;}
                            if(userdob == null){user.userdob = user.userdob; }else{user.userdob = userdob;}
                            if(useraddress == null){user.useraddress = user.useraddress; }else{user.useraddress = useraddress;}
                            if(usercountry == null){user.usercountry = user.usercountry; }else{user.usercountry = usercountry;}
                            
                            user.companyname = user.companyname;
                            user.companylogo = user.companylogo;
                            user.companydescription = user.companydescription;
                            user.nbshareordinary = user.nbshareordinary;
                            user.sharevalueordinary = user.sharevalueordinary;
                            user.descordinary = user.descordinary;
                            user.additionalordinary = user.additionalordinary;
                            user.nbsharepreferencial = user.nbsharepreferencial;
                            user.sharevaluepreferencial = user.sharevaluepreferencial;
                            user.descpreferencial = user.descpreferencial;
                            user.additionalpreferencial = user.additionalpreferencial;
                            user.nbsharenonvoting = user.nbsharenonvoting;
                            user.sharevaluenonvoting = user.sharevaluenonvoting;
                            user.descnonvoting = user.descnonvoting;
                            user.additionalnonvoting = user.additionalnonvoting;
                            user.nbshareredeemable = user.nbshareredeemable;
                            user.sharevalueredeemable = user.sharevalueredeemable;
                            user.descredeemable = user.descredeemable;
                            user.additionalredeemable = user.additionalredeemable;
                            user.userassets = user.userassets;
                        }
                        // end edit user
                        // if the company profile is to be updated
                        else if (editpart == "editcompany") {
                            // upload new company logo
                            if (fileCompanyLogoEdit == null || fileCompanyLogoEdit.Length == 0){
                                user.companylogo = user.companylogo;
                            }
                            else 
                            {
                                var newcLogo = "images/"+fileCompanyLogoEdit.FileName;
                                if (newcLogo == user.companylogo) {
                                    user.companylogo = user.companylogo;
                                } 
                                else {
                                    //save the new company logo in the server
                                    var path1 = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/", fileCompanyLogoEdit.FileName);  

                                    using (var stream1 = new FileStream(path1, FileMode.Create))  
                                    {  
                                        await fileCompanyLogoEdit.CopyToAsync(stream1);  
                                    }  
                                    user.companylogo = "images/" + fileCompanyLogoEdit.FileName; 
                                } 
                            }
                            user.userphoto = user.userphoto;
                            user.userfname = user.userfname;
                            user.userlname = user.userlname;
                            user.username = user.username;
                            user.useremail = user.useremail;
                            user.userpassword = user.userpassword;
                            user.userdob = user.userdob;
                            user.useraddress = user.useraddress;
                            user.usercountry = user.usercountry;

                            if(companyname == null){user.companyname = user.companyname; }else{user.companyname = companyname;}
                            if(companydescription == null){user.companydescription = user.companydescription; }else{user.companydescription = companydescription; }
                            if(nbshareordinary == 0){user.nbshareordinary = user.nbshareordinary; }else{user.nbshareordinary = nbshareordinary; }
                            if(sharevalueordinary == 0){user.sharevalueordinary = user.sharevalueordinary; }else{user.sharevalueordinary = sharevalueordinary; }
                            if(descordinary == null){user.descordinary = user.descordinary; }else{user.descordinary = descordinary; }
                            if(additionalordinary == null){user.additionalordinary = user.additionalordinary; }else{user.additionalordinary = additionalordinary; }
                            if(nbsharepreferencial == 0){user.nbsharepreferencial = user.nbsharepreferencial; }else{user.nbsharepreferencial = nbsharepreferencial; }
                            if(sharevaluepreferencial == 0){user.sharevaluepreferencial = user.sharevaluepreferencial; }else{user.sharevaluepreferencial = sharevaluepreferencial; }
                            if(descpreferencial == null){user.descpreferencial = user.descpreferencial; }else{user.descpreferencial = descpreferencial; }
                            if(additionalpreferencial == null){user.additionalpreferencial = user.additionalpreferencial; }else{user.additionalpreferencial = additionalpreferencial; }
                            if(nbsharenonvoting == 0){user.nbsharenonvoting = user.nbsharenonvoting; }else{user.nbsharenonvoting = nbsharenonvoting; }
                            if(sharevaluenonvoting == 0){user.sharevaluenonvoting = user.sharevaluenonvoting; }else{user.sharevaluenonvoting = sharevaluenonvoting; }
                            if(descnonvoting == null){user.descnonvoting = user.descnonvoting; }else{user.descnonvoting = descnonvoting; }
                            if(additionalnonvoting == null){user.additionalnonvoting = user.additionalnonvoting; }else{user.additionalnonvoting = additionalnonvoting; }
                            if(nbshareredeemable ==0 ){user.nbshareredeemable = user.nbshareredeemable; }else{user.nbshareredeemable = nbshareredeemable; }
                            if(sharevalueredeemable ==0 ){user.sharevalueredeemable = user.sharevalueredeemable; }else{user.sharevalueredeemable = sharevalueredeemable; }
                            if(descredeemable == null){user.descredeemable = user.descredeemable; }else{user.descredeemable = descredeemable; }
                            if(additionalredeemable == null){user.additionalredeemable = user.additionalredeemable; }else{user.additionalredeemable = additionalredeemable; }
                            if(userassets == 0){user.userassets = user.userassets; }else{user.userassets = userassets;}
                        }
                        // end edit company

                        user.isadmin=user.isadmin;
                        user.validatedUser=user.validatedUser;
                
                    }
                    // update the user and save changes
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!userExists(user.id))
                    {
                        return RedirectToAction("customErrorPage", "user");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "user", new { id = user.id} );
            }
            return RedirectToAction("Details", "user",  new { id = user.id} );
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var user = await _context.users.FirstOrDefaultAsync(m => m.id == id);
            if (user == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            //return RedirectToAction("adminDashboard", "user");
            return View(user);
        }

        // POST: user/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _context.users.FindAsync(id);
            _context.users.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "user");
            
        }       

        private bool userExists(int id)
        {
            return _context.users.Any(e => e.id == id);
        }

        public IActionResult ActivateEmail(int id)
        {
            
            int day = HttpContext.Session.GetInt32("day").Value;
            int month = HttpContext.Session.GetInt32("month").Value;
            int year = HttpContext.Session.GetInt32("year").Value;

            
            DateTime expirydate = new DateTime(year, month, day); 
             
            DateTime endDate = expirydate.AddDays(1);
            
            int value = DateTime.Compare(endDate, DateTime.Now); 
            // checking 
            if (value > 0) 
                return View();
            else
                return RedirectToAction("customErrorPage", "user"); 
        

        }
        public IActionResult customErrorPage(){
            return View(); 
        }
        public async Task<IActionResult> adminDashboard()
        {
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            ViewBag.listUser = HttpContext.Session.GetString("users");

            return View(await _context.users.ToListAsync());
        }

        public async Task<IActionResult> ValidateUser(int? id)
        {
            var user = await _context.users.FindAsync(id);
            
            user.validatedUser = true; 

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "user");
        }

        public async Task<IActionResult> BlockUser(int? id)
        {
            var user = await _context.users.FindAsync(id);
            
            user.validatedUser = false; 

            _context.Update(user);
            await _context.SaveChangesAsync();

            return RedirectToAction("adminDashboard", "user");
        }

        public IActionResult askQuestions()
        {
            return RedirectToAction("Index", "Home");
        }

    }
}
