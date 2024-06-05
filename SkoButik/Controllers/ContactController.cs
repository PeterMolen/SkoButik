using Microsoft.AspNetCore.Mvc;
using SkoButik.Models;
using static SkoButik.Models.Contact;

namespace SkoButik.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Contact()
        {
            var random = new Random();
            var phone = $"555-01{random.Next(0, 10)}-{random.Next(1000, 10000)}";
            var email = $"contact{random.Next(1, 100)}@G5.com";

            var model = new Contact
            {
                Phone = phone,
                Email = email
            };

            return View(model);
        }
    }
}
