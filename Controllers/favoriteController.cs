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
using Microsoft.AspNetCore.Http.Extensions;

namespace cFunding.Controllers
{
    public class favoriteController : Controller
    {
        private readonly AppliContext _context;

        public favoriteController(AppliContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var url = HttpContext.Request.GetEncodedUrl();
            HttpContext.Session.SetString("urlHome", url);
            ViewBag.urlHome = HttpContext.Session.GetString("urlHome");

            var userid = HttpContext.Session.GetInt32("userid");
            ViewBag.userid = HttpContext.Session.GetInt32("userid");
            ViewBag.username = HttpContext.Session.GetString("username");
            ViewBag.useremail = HttpContext.Session.GetString("useremail");
            ViewBag.userphoto = HttpContext.Session.GetString("userphoto");
            ViewBag.loginFailed = HttpContext.Session.GetString("loginFailed");
            ViewBag.isadmin = HttpContext.Session.GetString("isadmin");
            ViewBag.validatedUser = HttpContext.Session.GetString("validatedUser");
            ViewBag.listUser = HttpContext.Session.GetString("listUser");
            List<user> users = _context.users.ToList();
            List<project> projects = _context.projects.ToList();

            List<String> listEmail = new List<string>();
            foreach (var item in users)
            {
                listEmail.Add(item.useremail);
            }
            HttpContext.Session.SetString("emails", JsonConvert.SerializeObject(listEmail));
            ViewBag.listEmail = HttpContext.Session.GetString("emails");

            
            var listFavorites = _context.favorites.Where(f => f.fkuser.id == userid);
            return View(listFavorites);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var favorite = await _context.favorites.FirstOrDefaultAsync(m => m.id == id);
            if (favorite == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(favorite);
        }

        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fkuser,fkproject")] favorite favorite, int fkuser, int fkproject)
        {
            if (ModelState.IsValid)
            {
                var userid = HttpContext.Session.GetInt32("userid");
                var currentUser = await _context.users.FirstOrDefaultAsync(m => m.id == userid);
                
                if (currentUser == null) {
                    return RedirectToAction("customErrorPage", "user");
                }
                else {
                    var currentProject = await _context.projects.FirstOrDefaultAsync(m => m.id == fkproject);
                    
                    if (currentProject == null) {
                        return RedirectToAction("customErrorPage", "user");
                    }
                    else {
                        favorite.fkproject = currentProject;
                        favorite.fkuser = currentUser;
                        

                        _context.Add(favorite);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
                
            }
            return View(favorite);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var favorite = await _context.favorites.FirstOrDefaultAsync(m => m.id == id);
            if (favorite == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            _context.favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "favorite");
        }

        // POST: favorite/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var favorite = await _context.favorites.FindAsync(id);
            _context.favorites.Remove(favorite);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "favorite");
        }



    }
}