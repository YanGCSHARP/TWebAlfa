using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace eUseControl.BusinessLogic.Interfaces
{
    public class BussinesLogic
    {
        public ISession GetSessionBL(int id)
        {
            return new SessionBL(id);
        }
    }
}
