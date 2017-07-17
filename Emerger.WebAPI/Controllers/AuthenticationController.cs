using Emerger.Core.Utilities;
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
					Logger.Info(string.Format("El usuario {0} fue logueado correctamente", username));
					return Request.CreateResponse(
						HttpStatusCode.OK,
						new
						{
							IsLogged = true,
							Token = JwtManager.GenerateToken(username),
							Profile = new
							{
								Name = "Javier Nigrelli",
								Email = "jnigrelli@paramedic.com.ar"
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
				Logger.Error("No se pudo autenticar el usuario", ex);
				return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, ex);
			}
		}

		#endregion
	}
}