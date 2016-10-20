using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp.Portable;
using RestSharp.Portable.WebRequest;
using System.Net;
namespace DirectTVRemote.Models {
    public class RestCommunicationService {
        #region Members

        RestClient m_Client;

        CookieContainer m_LocalCookieContainer;

        #endregion

        #region Constructors

        public RestCommunicationService(string url) {
            m_Client = new RestClient(url);

        }

        #endregion

        #region Authenticate Async


        #endregion

        #region Send Requests

        public async Task<IRestResponse> SendRequestAsync(IRestRequest request) {


            return await m_Client.Execute(request);
        }

        #endregion

        #region Nested Classes



        #endregion
    }
}
