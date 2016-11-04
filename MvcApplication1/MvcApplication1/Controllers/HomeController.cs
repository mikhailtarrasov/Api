using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        // ViewResult             - поручаем MVC обработать представление и вернуть HTML
        // RedirectResult         - заставляем браузер перенаправиться на другой сайт
        // HttpUnauthorizedResult - заставляем пользователя залогиниться
        // Эти объекты - результаты действия и происходят из класса ActionResult

        public ViewResult Index()   /*Создаем ВьюРезалт, вызывая метод Вью*/
        {
            int hour = DateTime.Now.Hour;
            ViewBag.Greeting = hour < 12 ? "Good Morning" : "Good Afternoon";

            return View();
        }
        //public ActionResult Index()               
        //{
        //    return View();
        //}

    }
}
