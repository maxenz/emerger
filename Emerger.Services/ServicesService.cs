using Emerger.DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emerger.Services
{
	public class ServicesService
	{
		public List<Service> GetServices()
		{
			List<Service> lst = new List<Service>();
			Service sv = new Service()
			{
				Amount = 212,
				Client = "Paramedic",
				Concept = "Incidente",
				Date = DateTime.Now,
				Destiny = "CBA",
				Kilometers = 21,
				Number = "RW5",
				Origin = "SFE",
				Patient = "GONZALEZ, RODRIGO",
				State = ServiceStateType.NotAcceptedComplaint
			};

			lst.Add(sv);

			return lst;
		}
	}
}
