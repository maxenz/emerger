using System;
using System.Data;

namespace Emerger.DomainModel
{
	public class ServiceDetail
	{
		#region Properties

		public long Id { get; set; }

		public string Concept { get; set; }

		public long Quantity { get; set; }

		public double Amount { get; set; }

		public double Total
		{
			get
			{
				return Quantity * Amount;
			}
		}

		#endregion

		#region Constructors

		public ServiceDetail(DataRow row)
		{
			if (row["ID"] != DBNull.Value)
			{
				this.Id = Convert.ToInt64(row["ID"]);
			}

			if (row["Concepto"] != DBNull.Value)
			{
				this.Concept = Convert.ToString(row["Concepto"]);
			}

			if (row["Cantidad"] != DBNull.Value)
			{
				this.Quantity = Convert.ToInt64(row["Cantidad"]);
			}

			if (row["Importe"] != DBNull.Value)
			{
				this.Amount = Convert.ToDouble(row["Importe"]);
			}
		}

		#endregion
	}
}
