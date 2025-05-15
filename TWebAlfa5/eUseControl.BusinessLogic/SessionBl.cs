using System.ServiceModel.Channels;
using eUseControl.BusinessLogic.Core;
using eUseControl.BusinessLogic.Interfaces;

namespace eUseControl.BusinessLogic
{
    public class SessionBL : UserApi, ISession
    {
        public SessionBL(int id)
        {
            Id = id;
        }

        public void SetSession(string session)
        {
            throw new System.NotImplementedException();
        }
        public string GetSession()
        {
            throw new System.NotImplementedException();
        }

        public int Id { get; set; } // Implementing ISession.Id

        string ISession.Id => throw new System.NotImplementedException();
    }

}