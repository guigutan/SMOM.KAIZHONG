using SIE.Domain;
using SIE.ERPInterface.Api.WebApi.KaiZhongGroup;
using SIE.EventMessages.WebApis;
using SIE.KZ.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Api.WebApi
{
    public class IWebApiController : DomainController, IWebApi
    {
        /// <summary>
        /// SAP与总控重传
        /// </summary>
        /// <param name="InfNcDataLogGroupId"></param>
        public virtual void InfNcDataLogGroupReUpload(List<double> InfNcDataLogGroupIds)
        {
            var logs = Query<InfNcDataLogGroup>().Where(p => InfNcDataLogGroupIds.Contains(p.Id)).ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            foreach (var log in logs)
            {
                log.ErrorMsg = null;

                RT.Service.Resolve<KzGroupBaseDateInfController>().SaveInfDatasToBusiness(log.NcSystemCode, log.NcInfCode, log.NcOperationType, log.DataJsons, log.InfType.Value, log.KeyMsgfive, log.InvOrg, log);

                log.SendState = KZ.Base.Interfaces.Enums.SendState.NoSend;
                log.FactoryErrorMsg = null;
                RF.Save(log);
            }
        }
    }
}
