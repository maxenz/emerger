using Emerger.DomainModel;
using ShamanExpressDLL;
using System;
using System.Collections.Generic;
using System.Data;

namespace Emerger.Services
{
	public class FiltersService
	{
		#region Public Methods

		public List<Filter> GetCompanies(int userId)
		{
			conUsuarios userData = new conUsuarios();
			DataTable data = userData.GetPrestadoresByUsuario(userId);
			List<Filter> filters = new List<Filter>();

			foreach(DataRow row in data.Rows)
			{
				filters.Add(new Filter(row));
			}

			return filters;
		}

		public List<Filter> GetPeriods()
		{
			conPeriodosLiquidaciones liquidaciones = new conPeriodosLiquidaciones();
			DataTable data = liquidaciones.GetAll(5);
			List<Filter> filters = new List<Filter>();

			foreach (DataRow row in data.Rows)
			{
				filters.Add(
					new PeriodFilter(row));
			}

			return filters;
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
