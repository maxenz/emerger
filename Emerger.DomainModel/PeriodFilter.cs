using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

		#endregion
	}
}
