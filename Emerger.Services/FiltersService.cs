using Emerger.DomainModel;
using System;
using System.Collections.Generic;

namespace Emerger.Services
{
	public class FiltersService
	{
		#region Public Methods

		public List<Filter> GetCompanies()
		{
			return new List<Filter>()
			{
				new Filter(1,"Telefónica"),
				new Filter(2,"Telecom")
			};
		}

		public List<Filter> GetPeriods()
		{
			return new List<Filter>()
			{
				new PeriodFilter(1,"07/10", DateTime.Now, DateTime.Now.AddDays(10)),
				new PeriodFilter(2,"08/10", DateTime.Now.AddDays(30), DateTime.Now.AddDays(40)),
			};
		}

		public List<Filter> GetStates()
		{
			return new List<Filter>()
			{
				new Filter(1,"Finalizado"),
				new Filter(2,"Pendiente")
			};
		}

		#endregion
	}
}
