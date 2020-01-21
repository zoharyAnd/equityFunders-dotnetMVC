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
    public class transactionController : Controller
    {
        private readonly AppliContext _context;

        public transactionController(AppliContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.transactions.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var transaction = await _context.transactions
                .FirstOrDefaultAsync(m => m.id == id);
            if (transaction == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(transaction);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("id,fkproject,fkuser,amount,transactiondate,accountemail")] transaction transaction)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(transaction);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            var transaction = await _context.transactions
                .FirstOrDefaultAsync(m => m.id == id);
            if (transaction == null)
            {
                return RedirectToAction("customErrorPage", "user");
            }

            return View(transaction);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.transactions.FindAsync(id);
            _context.transactions.Remove(transaction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool transactionExists(int id)
        {
            return _context.transactions.Any(e => e.id == id);
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

            List<user> users = _context.users.ToList();
            List<project> projects = _context.projects.ToList();
            return View(await _context.transactions.ToListAsync());
        }
    }
}