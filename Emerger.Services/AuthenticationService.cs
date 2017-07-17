using Emerger.Core.Utilities;
using ShamanExpressDLL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emerger.Services
{
	public class AuthenticationService
	{
		#region Public Methods

		public bool Login(string username, string password)
		{
			var conUsers = new conUsuarios();
			bool change = false;
			Logger.Info(string.Format("Se intentará loguear el usuario {0}", username));
			long result = conUsers.Autenticar(username, password, ref change);
			Logger.Info(string.Format("El resultado del intento de login fue: {0}", result.ToString()));

			return result > 0;
		}

		#endregion
	}
}
