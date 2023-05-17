using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Framework.Models;

namespace Web.Framework.Controllers
{
    public class CalendarController : BaseController
    {
        public CalendarController() : base(new ModelCalendar("calendar", "Calendar"))
        {
        }

        public CalendarController(ModelCalendar model) : base(model)
        {
        }
    }
}