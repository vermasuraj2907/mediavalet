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
    public class SupervisorController : ApiController
    {

        [HttpGet]
        [Route("getorder")]
        public Orders Getorder(string AgentId)
        {
            SuperVisorBL superVisorBL = new SuperVisorBL();
            Orders queueOrder = superVisorBL.GetOrder(AgentId);
            return queueOrder;
        }
    }
}
