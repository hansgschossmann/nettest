using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Net.NetworkInformation;

namespace nettest.Pages
{
    public class IndexModel : PageModel
    {
        public NetInfo NetInfo;

        [Url]
        [BindProperty]
        public string UrlToCheck { get; set; }


        public void OnGet()
        {
            UrlToCheck = "http://localhost:8080";
            NetInfo = new NetInfo();
            NetInfo.Update();
        }

        public RedirectToPageResult OnPost(string UrlToCheck)
        {
            return RedirectToPage("/UrlCheck", new { url = UrlToCheck });
        }
    }
}
