using Emerger.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Emerger.WebAPI.Controllers
{
	public class AuthenticationController : ApiController
	{
		public AuthenticationService _AuthenticationService { get; set; }

		public AuthenticationController()
		{
			_AuthenticationService = new AuthenticationService();
		}

		[Route("api/authentication/{username}/{password}")]
		public HttpResponseMessage Get(string username, string password)
		{
			DatabaseConnectionService db = new DatabaseConnectionService();
			db.Connect();
			try
			{
				bool isLogged = _AuthenticationService.Login(username, password);
				if (isLogged)
				{
					return Request.CreateResponse(
						HttpStatusCode.OK,
						isLogged);
				}
				else
				{
					return Request.CreateErrorResponse(
						HttpStatusCode.Unauthorized, "Los datos ingresados no son válidos");
				}

			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}

		}
	}
}