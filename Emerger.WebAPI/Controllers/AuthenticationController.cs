using Emerger.Services;
using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Jwt;
using WebApi.Jwt.Filters;

namespace Emerger.WebAPI.Controllers
{
	public class AuthenticationController : ApiController
	{
		#region Properties

		public AuthenticationService _AuthenticationService { get; set; }

		#endregion

		#region Constructors

		public AuthenticationController()
		{
			_AuthenticationService = new AuthenticationService();
		}

		#endregion

		#region Actions

		[AllowAnonymous]
		[HttpPost]
		[Route("api/authentication/login/{username}/{password}")]
		public HttpResponseMessage Login(string username, string password)
		{
			try
			{
				bool isLogged = _AuthenticationService.Login(username, password);
				if (isLogged)
				{
					return Request.CreateResponse(
						HttpStatusCode.OK,
						new
						{
							IsLogged = true,
							Token = JwtManager.GenerateToken(username),
							Profile = new
							{
								Name = "Maximiliano Poggio",
								Email = "maximilianopoggio@gmail.com"
							}
						});
				}
				else
				{
					return Request.CreateResponse(
						HttpStatusCode.Unauthorized,
						new
						{
							IsLogged = false,
							ErrorMessage = "Los datos ingresados no son válidos"
						});
				}

			}
			catch (Exception ex)
			{
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}
		}

		[HttpPost]
		[Route("api/authentication/logout/username")]
		[JwtAuthentication]
		public HttpResponseMessage Logout(string username)
		{
			return Request.CreateResponse(
						   HttpStatusCode.OK,
						   new
						   {
							   LoggedOut = true
						   });
		}
		#endregion
	}
}