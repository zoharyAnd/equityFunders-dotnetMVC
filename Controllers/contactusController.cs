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
using System.Net.Mail;
using System.Text;
using System.Net;

namespace cFunding.Controllers
{
    public class contactusController : Controller
    {
        private readonly AppliContext _context;

        public contactusController(AppliContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.contactsus.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var contactus = await _context.contactsus
                .FirstOrDefaultAsync(m => m.id == id);
            if (contactus == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(contactus);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fkuser,sendername,senderemail,mailsubject,mailmessage")] contactus contactus, string sendername, string senderemail, string mailsubject, string mailmessage)
        {
            if (ModelState.IsValid)
            {
                var userid = HttpContext.Session.GetInt32("userid");
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);

                if (currentUser == null)
                {
                    contactus.fkuser = null;
                }
                else {
                    contactus.fkuser = currentUser;
                    contactus.sendername = sendername;
                    contactus.senderemail = senderemail;
                    contactus.mailsubject = mailsubject;
                    contactus.mailmessage = mailmessage;

                    _context.Add(contactus);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
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
            ViewBag.listUser = HttpContext.Session.GetString("users");

            return View(await _context.contactsus.ToListAsync());
        }


        [HttpPost]
        public IActionResult replyMessage(string useremail, string emailSubjectReply, string replyMessage){

            var emailPlatform = "cFundingZrsi@gmail.com";
            var password = "cFundingZrsi2019";

            // create an smtp client bearing all parameters needed to send the email
                SmtpClient client = new SmtpClient("smtp.gmail.com", 587); 
                client.EnableSsl = true; 
                client.Timeout = 100000; 
                client.DeliveryMethod = SmtpDeliveryMethod.Network; 
                client.UseDefaultCredentials = false;  
                client.Credentials = new NetworkCredential(emailPlatform, password);
                 
                // setting the email content
                var subject = emailSubjectReply; 
                var body = replyMessage;
                MailMessage mailMessage = new MailMessage (emailPlatform, useremail, subject, body); 
                mailMessage.IsBodyHtml = true;
                mailMessage.BodyEncoding = UTF8Encoding.UTF8; 

                // sending the email
                client.Send(mailMessage); 
                
            return RedirectToAction("adminDashboard", "contactus"); 
        }

    }
}