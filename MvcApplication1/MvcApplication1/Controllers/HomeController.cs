using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VkClientApp;
using VkDatabaseDll;

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
            ViewBag.GroupMembers = new EFDatabaseClient().GetGroupByScreenName("programm_exam").MembersList;
            
            return View();
        }
        public ViewResult Posts(int id)   /*Создаем ВьюРезалт, вызывая метод Вью*/
        {
            ViewBag.Posts = new EFDatabaseClient().GetSortedNewsById(id);

            return View();
        }
    }
}
