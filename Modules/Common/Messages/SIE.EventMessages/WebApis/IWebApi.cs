using SIE.EventMessages.MES.WorkOrders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.WebApis
{
    [Services.Service(FallbackType = typeof(DefaultIWebApi))]

    public interface IWebApi
    {
        /// <summary>
        /// SAP与总控重传
        /// </summary>
        /// <param name="InfNcDataLogGroupId"></param>
        void InfNcDataLogGroupReUpload(List<double> InfNcDataLogGroupIds);
    }

    public class DefaultIWebApi : IWebApi
    {
        /// <summary>
        /// SAP与总控重传
        /// </summary>
        /// <param name="InfNcDataLogGroupId"></param>
        public void InfNcDataLogGroupReUpload(List<double> InfNcDataLogGroupIds)
        { 
            
        }
    }
}
