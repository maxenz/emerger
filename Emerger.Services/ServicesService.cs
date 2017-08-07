using Emerger.DomainModel;
using ShamanExpressDLL;
using System.Collections.Generic;
using System.Data;

namespace Emerger.Services
{
	public class ServicesService
	{
		public List<Service> GetServices(long companyId, long periodId, long stateId)
		{
			conLiquidacionesPrestadores liqPrest = new conLiquidacionesPrestadores();
			conLiquidacionesPrestadoresIncidentes liqPrestInc = new conLiquidacionesPrestadoresIncidentes();
			conLiqPrestadoresIncidentesConceptos liqPrestIncCon = new conLiqPrestadoresIncidentesConceptos();

			List<Service> services = new List<Service>();

			long idLiqPrestador = liqPrest.GetIdByIndex(periodId, companyId);
			DataTable dt = liqPrestInc.GetByLiquidacionId(idLiqPrestador);

			foreach(DataRow row in dt.Rows)
			{
				Service service = new Service(row);
				DataTable dtServiceDetails = liqPrestIncCon.GetByLiquidacionIncidenteId(service.Id);
				if (dtServiceDetails != null)
				{
					service.SetDetails(dtServiceDetails);
				}
				services.Add(new Service(row));
			}

			return services;
		}
	}
}
