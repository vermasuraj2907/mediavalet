using MediaValet.Modal;
using MediaValet.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediaValet.BL
{
   public  class AgentBL
    {
        public void SaveAgentOrder(ConfirmationOrder confirmationOrder)
        {
            ConfirmationTable confirmationTable = new ConfirmationTable();
           confirmationTable.SaveOrder(confirmationOrder);
        }
    }
}
