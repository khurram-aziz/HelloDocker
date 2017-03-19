using Microsoft.AspNetCore.Mvc;

namespace RedisMvc.Controllers
{
    public class HomeController : Controller
    {
        //we can use HomeController(IDistributedCache)
        //but it will not have Redis rich data type apis; like INCR
        public IActionResult Index()
        {
            var db = Startup.RedisConnection.GetDatabase();
            //await db.StringIncrementAsync("hitcounter");
            db.StringIncrement("hitcounter");
            
            var counter = db.StringGet("hitcounter");
            var visitors = db.SetLength("visitor");

            this.ViewData["counter"] = counter;
            this.ViewData["visitors"] = visitors;
            
            return View();
        }

        public IActionResult About()
        {
            ViewData["Message"] = "Your application description page.";

            return View();
        }

        public IActionResult Contact()
        {
            ViewData["Message"] = "Your contact page.";

            return View();
        }

        public IActionResult Error()
        {
            return View();
        }
    }
}
