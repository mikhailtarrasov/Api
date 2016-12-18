using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using VkDatabaseDll;
using VkDatabaseDll.Domain;
using VkDatabaseDll.Domain.Entity;
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
            //EFDatabaseClient dbClient=  new EFDatabaseClient();
            //Group dbGroup = dbClient.GetGroupByScreenName("csu_iit");
            //var db = new DatabaseContext();
            //User dbGroup = db.Groups.Where(x => x.ScreenName == "csu_iit").FirstOrDefault();
            //Group dbGroup = db.Groups.Find(19);

            var efDbClient = new EFDatabaseClient();
            var dbGroup = efDbClient.GetGroupByScreenName("chelyabinskfw");
            ViewBag.GroupMembers = dbGroup.MembersList;
            //ViewBag.MembersCount = dbGroup.MembersList.Count;


            return View();
        }
        //public ActionResult Index()               
        //{
        //    return View();
        //}

    }
}
