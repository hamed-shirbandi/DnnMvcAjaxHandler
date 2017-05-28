using System;
using System.Linq;
using System.Web.Mvc;
using Dnn.Modules.DnnMvcAjaxHandlerExample.Components;
using DotNetNuke.Web.Mvc.Framework.Controllers;
using DotNetNuke.Web.Mvc.Framework.ActionFilters;
using DotNetNuke.Entities.Users;
using DotNetNuke.Framework.JavaScriptLibraries;
using DnnMvcAjaxHandlerExample.Service.Users;
using DnnMvcAjaxHandlerExample.Enums;
using System.Web.Mvc.Html;
using DnnMvcAjaxHandlerExample.Service.Users.Dto;

namespace Dnn.Modules.DnnMvcAjaxHandlerExample.Controllers
{
    public class HomeController : DnnController
    {


        private readonly IUserService _userService;
        private int pageSize;
        private int recordsPerPage;
        private int TotalItemCount;



        public HomeController()
        {
            _userService = new UserService();
            pageSize = 0;
            recordsPerPage = 5;
            TotalItemCount = 0;
        }




        /// <summary>
        /// 
        /// </summary>
        public ActionResult Index()
        {
            #region Pagging

            var page = 1;


            #endregion

            AddFakeUsers();

            var users = _userService.Search(page: page, recordsPerPage: recordsPerPage, term: "", sortBy: SortBy.AddDate, sortOrder: SortOrder.Desc, pageSize: out pageSize, TotalItemCount: out TotalItemCount);

            #region ViewBags

            ViewBag.SortOrder = EnumHelper.GetSelectList(typeof(SortOrder));
            ViewBag.SortBy = EnumHelper.GetSelectList(typeof(SortBy));
            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;


            #endregion

            return View(users);
        }






        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult Search(int page = 1, string term = "", SortBy sortBy = SortBy.AddDate, SortOrder sortOrder = SortOrder.Desc)
        {
            System.Threading.Thread.Sleep(700);

            AddFakeUsers();

            var users = _userService.Search(page: page, recordsPerPage: recordsPerPage, term: term, sortBy: sortBy, sortOrder: sortOrder, pageSize: out pageSize, TotalItemCount: out TotalItemCount);

            #region ViewBags


            ViewBag.PageSize = pageSize;
            ViewBag.CurrentPage = page;
            ViewBag.TotalItemCount = TotalItemCount;


            #endregion

            return PartialView("_UsersList", users);
        }






        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public JsonResult Delete(int id)
        {

            //delte user from db
          
            return Json(new { result=true}, JsonRequestBehavior.AllowGet);

        }





        /// <summary>
        /// 
        /// </summary>
        [HttpGet]
        public JsonResult AddUserToRole(int userId, int roleId)
        {
            return Json(new { result = true, message= "Ajax Click me clicked.  userid=" + userId + " roleId=" + roleId }, JsonRequestBehavior.AllowGet);
        }




        /// <summary>
        /// 
        /// </summary>
        private void AddFakeUsers()
        {
            for (int i = 0; i < 50; i++)
            {
                _userService.Create(new UserInput
                {
                    Id=i,
                    Name = "Name " + i,
                    Family = "Family " + i,
                    AddDate = DateTime.Now.AddDays(-i),
                });

            }
        }




    }
}
