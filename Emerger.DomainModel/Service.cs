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

		public DateTime Date { get; set; }

		public string Number { get; set; }

		public string Concept { get; set; }

		public string Client { get; set; }

		public string Patient { get; set; }

		public string Origin { get; set; }

		public string Destiny { get; set; }

		public int Kilometers { get; set; }

		public double Amount  { get; set; }

		public ServiceStateType State { get; set; }

		#endregion

		#region Constructors

		public Service() { }

		public Service(DataRow row)
		{
			if (row["Fecha"] != DBNull.Value)
			{
				this.Date = Convert.ToDateTime(row["Fecha"]);
			}

			if (row["Numero"] != DBNull.Value)
			{
				this.Number = row["Numero"].ToString();
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

			if (row["Origen"] != DBNull.Value)
			{
				this.Origin = row["Origen"].ToString();
			}

			if (row["Destino"] != DBNull.Value)
			{
				this.Destiny = row["Destino"].ToString();
			}

			if (row["Kilometros"] != DBNull.Value)
			{
				this.Kilometers = Convert.ToInt32(row["Kilometros"]);
			}

			if (row["Importe"] != DBNull.Value)
			{
				this.Amount = Convert.ToDouble(row["Importe"]);
			}

			if (row["Estado"] != DBNull.Value)
			{
				this.State = (ServiceStateType) Convert.ToInt32(row["Estado"]);
			}
		}

		#endregion

	}
}
