using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace nettest.Pages
{
    public class ResponderModel : PageModel
    {
        NetInfo netInfo;

        public JsonResult OnGet()
        {
            netInfo = new NetInfo();
            netInfo.Update();

            JsonResult jsonResult = new JsonResult(netInfo);
            return jsonResult;
        }
    }
}