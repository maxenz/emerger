using Emerger.Core.Utilities;
using Emerger.DomainModel;
using Emerger.Services;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt.Filters;
using System.Linq;

namespace Emerger.Controllers
{
	public class FiltersController : ApiController
	{
		#region Properties

		public FiltersService _FiltersService { get; set; }

		#endregion

		#region Constructors

		public FiltersController()
		{
			_FiltersService = new FiltersService();
		}

		#endregion

		#region Actions

		[HttpGet]
		[JwtAuthentication]
		[Route("api/filters/companies")]
		public HttpResponseMessage GetCompanies()
		{
			try
			{
				int user = Convert.ToInt32(Request.Headers.GetValues("User").FirstOrDefault());
				List<Filter> companies = _FiltersService.GetCompanies(user);

				return Request.CreateResponse(
					HttpStatusCode.OK,
					new
					{
						Companies = companies
					});


			}
			catch (Exception ex)
			{
				Logger.Error("Error al consultar las empresas", ex);
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}
		}

		[HttpGet]
		[JwtAuthentication]
		[Route("api/filters/periods")]
		public HttpResponseMessage GetPeriods()
		{
			try
			{
				List<Filter> periods = _FiltersService.GetPeriods();

				return Request.CreateResponse(
					HttpStatusCode.OK,
					new
					{
						Periods = periods
					});


			}
			catch (Exception ex)
			{
				Logger.Error("Error al consultar los períodos", ex);
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}
		}

		[HttpGet]
		[JwtAuthentication]
		[Route("api/filters/states")]
		public HttpResponseMessage GetStates()
		{
			try
			{
				List<Filter> states = _FiltersService.GetStates();

				return Request.CreateResponse(
					HttpStatusCode.OK,
					new
					{
						States = states
					});


			}
			catch (Exception ex)
			{
				Logger.Error("Error al consultar los estados", ex);
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}
		}



		#endregion
	}
}