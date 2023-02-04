using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OttBlog23.Data;
using OttBlog23.Models;
using OttBlog23.Services.Interfaces;
using OttBlog23.ViewModels;
using System.Diagnostics;
using X.PagedList;

namespace OttBlog23.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IBlogEmailSender _emailSender;
        private readonly ApplicationDbContext _context;
        private readonly IImageService _imageService;
        private readonly UserManager<BlogUser> _userManager;
        public HomeController(ILogger<HomeController> logger, IBlogEmailSender emailSender, ApplicationDbContext context, IImageService imageService, UserManager<BlogUser> userManager)
        {
            _userManager = userManager;
            _imageService = imageService;
            _context = context;
            _emailSender = emailSender;
            _logger = logger;
        }

        public async Task<IActionResult> Index(int? page)
        {
            var pageNumber = page ?? 1;
            var pageSize = 3;

            var blogs = _context.Blogs
                .Include(b => b.BlogUser)
                .OrderByDescending(b => b.Created)
                .ToPagedListAsync(pageNumber, pageSize);

            ViewData["HeaderImage"] = "/img/home-bg.jpg";
            ViewData["MainText"] = "Lawson's Blog";
            ViewData["SubText"] = "A practical example of programming in ASP.NET MVC";

            return View(await blogs);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Contact()
        {
            ViewBag.items = new[] { "Bold", "Italic", "Underline",
            "Formats", "Alignments", "-", "OrderedList", "UnorderedList", "Image",
             "CreateLink" };
            object tools1 = new
            {
                tooltipText = "Rotate Left",
                template = "<button class='e-tbar-btn e-btn' id='roatateLeft'><span class='e-btn-icon e-icons e-rotate-left'></span>"
            };
            object tools2 = new
            {
                tooltipText = "Rotate Right",
                template = "<button class='e-tbar-btn e-btn' id='roatateRight'><span class='e-btn-icon e-icons e-rotate-right'></span>"
            };
            ViewBag.image = new[] {
            "Replace", "Align", "Caption", "Remove", "InsertLink", "OpenImageLink", "-",
             "EditImageLink", "RemoveImageLink", "Display", "AltText", "Dimension",tools1
             , tools2
             };

            ViewData["HeaderImage"] = "/img/contact-bg.jpg";
            ViewData["MainText"] = "Get in touch!";
            ViewData["SubText"] = $"This form works, but does not deliver to my primary inbox. \r\nHowever, to truly get my attention, use the links in the footer.";
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ContactAsync(ContactMe model)
        {
            if (ModelState.IsValid)
            {
                model.Message = $"{model.Message} <hr/> Phone: {model.Phone}";
                await _emailSender.SendContactEmailAsync(email: model.Email,
                                                         name: model.Name,
                                                         subject: model.Subject,
                                                         message: model.Message);
                return RedirectToAction("Index");


            }
            return View(model);
        }
        public partial class RichTextEditorController : Controller
        {
            public ActionResult DefaultFunctionalities()
            {
                return View();
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
