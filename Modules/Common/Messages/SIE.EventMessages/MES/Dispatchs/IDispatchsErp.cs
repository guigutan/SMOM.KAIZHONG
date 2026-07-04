using SIE.Api;
using SIE.EventMessages.MES.PPSNs;
using SIE.Security;
using SIE.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EventMessages.MES.Dispatchs
{
    /// <summary>
    /// 
    /// </summary>
    [Service(FallbackType = typeof(DefaultIDispatchsErp))]
    public interface IDispatchsErp
    {
        /// <summary>
        /// 集团调用方法发给下面的工厂
        /// </summary>
        /// <param name="systemCode"></param>
        /// <param name="infCode"></param>
        /// <param name="operationType"></param>
        /// <param name="dataJsons"></param>
        /// <returns></returns>
        void DistributeMdFactorySO(string systemCode, string infCode, string operationType, string dataJsons, string invOrgId, string groupGuid);
    }

    public class DefaultIDispatchsErp : IDispatchsErp
    {
        public void DistributeMdFactorySO(string systemCode, string infCode, string operationType, string dataJsons, string invOrgId, string groupGuid)
        {
            return;
        }
    }
}
