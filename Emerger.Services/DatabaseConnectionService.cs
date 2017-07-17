using ShamanRabbitService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emerger.Services
{
	public class DatabaseConnectionService
	{
		public bool Connect()
		{
			return DatabaseConnection.GetInstance().Connect();
		}

	}
}
