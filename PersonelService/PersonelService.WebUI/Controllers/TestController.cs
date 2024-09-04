using Microsoft.AspNetCore.Mvc;
using PersonelService.WebUI.Utils;

namespace PersonelService.WebUI.Controllers
{
    public class TestController : Controller
    {
        private readonly MailHelper _mailHelper;

        public TestController(MailHelper mailHelper)
        {
            _mailHelper = mailHelper;
        }

        public async Task<IActionResult> SendEmail()
        {
            await _mailHelper.SendEmailToCustomersAsync(); 

            return Content("E-posta gönderildi."); 
        }
    }
}
