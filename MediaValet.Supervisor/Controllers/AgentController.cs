using MediaValet.BL;
using MediaValet.Modal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace MediaValet.Supervisor.Controllers
{
    public class AgentController : ApiController
    {
        [HttpPost]
        [Route("agentordersave")]
        public void saveorder(ConfirmationOrder confirmationOrder)
        {
            AgentBL agentBL = new AgentBL();
            agentBL.SaveAgentOrder(confirmationOrder);
        }
    }
}
