using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.Controllers;
using Web.Framework.HTML;

namespace kNOwaste.Admin.Controllers
{
    public class AccountController : UserController
    {
        public AccountController() : base()
        {
            Model.ControllerName = "account";
            Model.Controller = this;
        }

        public override void Initialize()
        {
            ViewBag.Header = CreateMenu();
            ViewBag.Account = CreateAccountMenu();
            ViewBag.Title = "kNOw.Waste Content Management System";
        }

        protected virtual string CreateMenu()
        {
            HTMLList menu = new HTMLList();
            menu.Add("home", "Home", "../");            

            return menu.GenerateHTML();
        }

        protected virtual string CreateAccountMenu()
        {
            HTMLList menu = new HTMLList();
            menu.ID = "account-menu";

            if (Username != null)
            {
                menu.Add("user", Username, "../account/logout");
            }
            else
                menu.Add("user", "Login", "../account/login");

            return menu.GenerateHTML();
        }
    }
}