﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyPersonalBlog.Data;
using MyPersonalBlog.Models;
using MyPersonalBlog.Services;
using MyPersonalBlog.ViewModels;
using System.Diagnostics;
using X.PagedList;

namespace MyPersonalBlog.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly IBlogEmailSender _emailSender;
    private readonly ApplicationDbContext _context;

    public HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender, ApplicationDbContext context)
    {
        _logger = logger;
        _emailSender = emailSender;
        _context = context;
    }

    public async Task<IActionResult> Index(int? page)
    {
        var pageNumber = page ?? 1;
        var pageSize = 5;

        var blogs = _context.Blogs
            .Include(b => b.Author)
            .OrderByDescending(b => b.Created)
            .ToPagedListAsync(pageNumber, pageSize);

        return View(await blogs);
            
    }

    public IActionResult About()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

   [HttpPost]
   [ValidateAntiForgeryToken]
   public async Task<IActionResult> Contact(ContactMe model)
    {
        
        await _emailSender.SendContactEmailAsync(model.Email!, model.Name!, model.Subject!, model.Message!);
        return RedirectToAction("Index");
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
