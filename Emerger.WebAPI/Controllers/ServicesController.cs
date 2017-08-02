using Emerger.Core.Utilities;
using Emerger.DomainModel;
using Emerger.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Jwt.Filters;

namespace Emerger.Controllers
{
    public class ServicesController : ApiController
    {
		#region Properties

		public ServicesService _ServicesService { get; set; }

		#endregion

		#region Constructors

		public ServicesController()
		{
			_ServicesService = new ServicesService();
		}

		#endregion

		#region Actions

		[HttpGet]
		[JwtAuthentication]
		public HttpResponseMessage Get(long companyId = 0, long stateId = 0, long periodId = 0, DateTime? dayInPeriod = null)
		{
			try
			{
				List<Service> services = _ServicesService.GetServices(companyId, periodId,stateId);

				return Request.CreateResponse(
					HttpStatusCode.OK,
					new
					{
						Services = services
					});


			}
			catch (Exception ex)
			{
				Logger.Error("Error al consultar los servicios", ex);
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}
		}

		#endregion

	}
}