using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using UCOnline.Models;
using Web.Framework.Controllers;

namespace UCOnline.Controllers
{
    public class ProjectTaskIssuesController : BaseController
    {
        public ProjectTaskIssuesController() : base(new ModelProjectTaskIssues())
        {
        }
    }
}