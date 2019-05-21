using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace nettest.Pages
{
    public class HealthModel : PageModel
    {
        public static bool Healthy = true;

        public IActionResult OnGet()
        {
            return new StatusCodeResult((int)(Healthy ? HttpStatusCode.OK : HttpStatusCode.Gone));
        }
        public IActionResult OnPost(bool healthy)
        {
            Healthy = healthy;
            return RedirectToPage("/Index");
        }
    }
}