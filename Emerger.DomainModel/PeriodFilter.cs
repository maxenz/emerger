using System;
using System.Data;

namespace Emerger.DomainModel
{
	public class PeriodFilter : Filter
	{
		#region Properties

		public DateTime DateFrom { get; set; }

		public DateTime DateTo { get; set; }

		#endregion

		#region Constructors

		public PeriodFilter(int id, string description, DateTime dateFrom, DateTime dateTo)
		: base(id, description)
		{
			this.DateFrom = dateFrom;
			this.DateTo = dateTo;
		}

		public PeriodFilter(DataRow row) : base(row)
		{
			this.Description = row["PeriodoStr"].ToString();
			this.DateFrom = Convert.ToDateTime(row["FecDesde"]);
			this.DateTo = Convert.ToDateTime(row["FecHasta"]);
		}

		#endregion
	}
}
