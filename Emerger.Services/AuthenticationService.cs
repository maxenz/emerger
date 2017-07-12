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
			return conUsers.Autenticar(username, password, ref change) > 0;
		}

		#endregion
	}
}
