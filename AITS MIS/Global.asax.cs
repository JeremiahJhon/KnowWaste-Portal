using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using UCOnline.Models;
using Web.Framework.Models;
using Web.Framework.Server;

namespace UCOnline
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            
            if (UCOnline.Properties.Settings.Default.DbType == "MySQL")
            {
                MySQLServer.Driver = UCOnline.Properties.Settings.Default.Driver1;

                MySQLServer.Name = UCOnline.Properties.Settings.Default.Server;
                MySQLServer.UserName = UCOnline.Properties.Settings.Default.Username;

                MySQLServer.Password = UCOnline.Properties.Settings.Default.Password;
                MySQLServer.Database = UCOnline.Properties.Settings.Default.Database;
                MySQLServer.Initialize();

                ServerBase server = new ServerBase("");
                server.Initialize();
            }
            else if (UCOnline.Properties.Settings.Default.DbType == "MSSQL")
            {
                MSSQLServer.Name = UCOnline.Properties.Settings.Default.MS_Server;
                MSSQLServer.UserName = UCOnline.Properties.Settings.Default.MS_User;

                MSSQLServer.Password = UCOnline.Properties.Settings.Default.MS_Password;
                MSSQLServer.Database = UCOnline.Properties.Settings.Default.MS_Database;
                MSSQLServer.Initialize();

                ServerBase server = new ServerBase("");
                server.Initialize();
            }

            //Project
            ModelBase.Register(new ModelProject());
            ModelBase.Register(new ModelProjectEmployee());

            //Project Task
            ModelBase.Register(new ModelProjectTask());
            ModelBase.Register(new ModelProjectTaskEmployee());
            ModelBase.Register(new ModelProjectTaskIssues());

            //News
            ModelBase.Register(new ModelNews());

            //Events
            ModelBase.Register(new ModelEvents());

            //Seminars
            ModelBase.Register(new ModelSeminars());

            //Forums
            ModelBase.Register(new ModelForums());

            //Conferences
            ModelBase.Register(new ModelConferences());

            //Reports
            ModelBase.Register(new ModelReports());

            //Blogs
            ModelBase.Register(new ModelBlogs());
            ModelBase.Register(new ModelBlogsComment());
            ModelBase.Register(new ModelBlogsContact());

            //ExpertRosters
            ModelBase.Register(new ModelExperts());

            //Region and Countries
            ModelBase.Register(new ModelSubRegion());
            ModelBase.Register(new ModelRegion());
            ModelBase.Register(new ModelCountry());

            //Sectors
            ModelBase.Register(new ModelSector());

            //FAQs
            ModelBase.Register(new ModelFaqs());

            //Law/Polocy
            ModelBase.Register(new ModelLawPolicy());

            //Case Studies
            ModelCaseStudy.Register(new ModelCaseStudy());

            //Data and Trends
            ModelCountryWasteStreams.Register(new ModelCountryWasteStreams());
            ModelCountryPolicy.Register(new ModelCountryPolicy());
            ModelCountryPopulation.Register(new ModelCountryPopulation());
            ModelCountryReferences.Register(new ModelCountryReferences());

            //WasteCategory
            ModelWasteCategory.Register(new ModelWasteCategory());

            //Contact
            ModelContact.Register(new ModelContact());

            //E-learning
            ModelCourses.Register(new ModelCourses());

            //Encryption Keys
            byte[] key = { 123, 217, 19, 11, 24, 26, 76, 45, 114, 184, 27, 162, 37, 45, 213, 209, 241, 24, 175, 144, 173, 53, 196, 29, 24, 26, 17, 218, 131, 236, 53, 209 };
            byte[] vector = { 146, 64, 191, 111, 23, 3, 111, 119, 121, 245, 221, 213, 79, 32, 114, 156 };

            ModelBase.EncryptionKey = key;
            ModelBase.EncryptionVector = vector;

            //Path
            ModelCommon.DocumentPath = Server.MapPath("~/Documents");
            ModelCommon.ImagePath = Server.MapPath("~/Content/Images");
        }
    }
}
