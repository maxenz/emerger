using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Emerger.DomainModel
{
	public enum ServiceStateType
	{
		Agree = 1,

		NotAgree = 2,

		HasComplaintAnswer = 3,

		AcceptedComplaint = 4,

		NotAcceptedComplaint = 5
	}
}
