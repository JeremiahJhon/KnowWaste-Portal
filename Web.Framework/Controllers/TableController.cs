using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.ActionFilters;
using Web.Framework.Models;

namespace Web.Framework.Controllers
{
    [ActionFilterTable]
    public class TableController : BaseController
    {
        public TableController() : base(new ModelTable())
        {
        }
    }
}