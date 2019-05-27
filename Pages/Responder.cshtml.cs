using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace nettest.Pages
{
    public class ResponderResult
    {
        public NetInfo NetInfo;
        public HttpInfo HttpInfo;

        public ResponderResult()
        {
            NetInfo = new NetInfo();
            HttpInfo = new HttpInfo();
        }

        public void Update(HttpRequest request)
        {
            NetInfo.Update();
            HttpInfo.Update(request);
        }
    }

    public class ResponderModel : PageModel
    {
        public JsonResult OnGet()
        {
            ResponderResult responderResult = new ResponderResult();
            responderResult.Update(Request);
            JsonSerializerSettings serializerSettings = new Newtonsoft.Json.JsonSerializerSettings();
            serializerSettings.MaxDepth = 5;
            JsonResult jsonResult = new JsonResult(responderResult, serializerSettings);
            return jsonResult;
        }
    }
}