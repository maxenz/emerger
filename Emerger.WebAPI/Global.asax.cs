using Emerger.Core.Utilities;
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
			try
			{
				DatabaseConnectionService db = new DatabaseConnectionService();
				db.Connect();
				Logger.Info("La base de datos se inicializó correctamente");
			}
			catch (Exception ex)
			{
				Logger.Error("No se pudo inicializar la base de datos", ex);
			}

		}
	}
}
