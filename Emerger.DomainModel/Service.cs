using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emerger.DomainModel
{
	/// <summary>
	/// Clase que representa un servicio
	/// </summary>
	public class Service
	{
		#region Properties

		public long Id { get; set; }

		public DateTime Date { get; set; }

		public string Number { get; set; }

		public string Concept { get; set; }

		public string Client { get; set; }

		public string Patient { get; set; }

		public string Origin { get; set; }

		public string Destiny { get; set; }

		public int Kilometers { get; set; }

		public double Amount { get; set; }

		public ServiceStateType State { get; set; }

		public List<ServiceDetail> Details { get; set; }

		public double TotalServiceDetails
		{
			get
			{
				if (Details != null)
				{
					return Details.Sum(x => x.Total);
				}

				return 0;
			}
		}

		#endregion

		#region Constructors

		public Service() { }

		public Service(DataRow row)
		{
			if (row["IncidenteId"] != DBNull.Value)
			{
				this.Id = Convert.ToInt64(row["IncidenteId"]);
			}

			if (row["FecIncidente"] != DBNull.Value)
			{
				this.Date = Convert.ToDateTime(row["FecIncidente"]);
			}

			if (row["NroIncidente"] != DBNull.Value)
			{
				this.Number = row["NroIncidente"].ToString();
			}

			if (row["Concepto"] != DBNull.Value)
			{
				this.Concept = row["Concepto"].ToString();
			}

			if (row["Cliente"] != DBNull.Value)
			{
				this.Client = row["Cliente"].ToString();
			}

			if (row["Paciente"] != DBNull.Value)
			{
				this.Patient = row["Paciente"].ToString();
			}

			if (row["LocOrigen"] != DBNull.Value)
			{
				this.Origin = row["LocOrigen"].ToString();
			}

			if (row["LocDestino"] != DBNull.Value)
			{
				this.Destiny = row["LocDestino"].ToString();
			}

			if (row["Kilometraje"] != DBNull.Value)
			{
				this.Kilometers = Convert.ToInt32(row["Kilometraje"]);
			}

			if (row["Importe"] != DBNull.Value)
			{
				this.Amount = Convert.ToDouble(row["Importe"]);
			}

			//if (row["Estado"] != DBNull.Value)
			//{
			//	this.State = (ServiceStateType) Convert.ToInt32(row["Estado"]);
			//}
		}

		#endregion

		#region Public Methods

		public void SetDetails(DataTable table)
		{
			this.Details = new List<ServiceDetail>();
			foreach(DataRow row in table.Rows)
			{
				this.Details.Add(new ServiceDetail(row));
			}
		}

		#endregion

	}
}
