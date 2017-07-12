using Emerger.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace Emerger
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
			//DatabaseConnectionService db = new DatabaseConnectionService();
			//db.Connect();
        }
    }
}
