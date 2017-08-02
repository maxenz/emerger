using Emerger.DomainModel;
using ShamanExpressDLL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
				//DataTable dtConcepto = liqPrestIncCon.GetByLiquidacionIncidenteId(service.Id);
				//service.SetDetails(dtConcepto.Rows[0]);
				services.Add(new Service(row));
			}

			return services;
		}
	}
}
