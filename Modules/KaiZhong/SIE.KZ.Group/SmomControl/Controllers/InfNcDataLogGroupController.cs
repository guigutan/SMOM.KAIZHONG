using Newtonsoft.Json;
using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Datas;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.KZ.Base.SmomControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Group.SmomControl.Controllers
{
    public class InfNcDataLogGroupController : DomainController
    {
        #region 跨组织物料标签同步

        public virtual string SyncFactoryItemLabelToFactory()
        {
            List<string> strs = new List<string>();
            var fail = 0;
            var syncItemLabels = Query<SyncItemLabel>().ToList(null, new EagerLoadOptions().LoadWithViewProperty());
            var dic = syncItemLabels.GroupBy(p => new { p.SourceFactory, p.ToFactory }).ToDictionary(p => p.Key, p => p.ToList());
            foreach (var d in dic)
            {
                var smomControlSetting = RT.Service.Resolve<SmomBaseController>().GetSmomControlSettingByFactoryCode(d.Key.SourceFactory);
                OuterSystemRetVO result = new OuterSystemRetVO();

                var erpDataInfLog = RT.Service.Resolve<InfDataLogController>().SaveErpDataInfLog(InfType.SyncItemLabel, string.Empty, DateTime.Now, CallDirection.FactoryToFactory, CallResult.Success, 0);

                try
                {
                    var itemCodes = d.Value.Select(p => p.ItemCode).Distinct().ToList();

                    var smomParam = new List<SmomParam>(){
                    new SmomParam { Value =itemCodes },
                                    new SmomParam { Value =d.Key.SourceFactory },
                                    new SmomParam { Value =d.Key.ToFactory }
                                 }.ToArray();
                    //记录json
                    erpDataInfLog.RequestContent = JsonConvert.SerializeObject(smomParam);

                    if (smomControlSetting == null || smomControlSetting.FactoryUrl.IsNullOrEmpty())
                        throw new ValidationException("未配置总控{0}Url地址!".L10nFormat(d.Key.SourceFactory));

                    result = SmomControlHepler.SmomPost<OuterSystemRetVO>("KZWebApiController", "SyncItemLabel", smomControlSetting.FactoryUrl, smomParam);
                    var str = "[{0}]->[{1}]:".L10nFormat(d.Key.SourceFactory, d.Key.ToFactory);
                    if (!result.errorMsg.IsNullOrEmpty())
                    {
                        str = result.errorMsg + ",";
                    }
                    str += "成功{0},失败{1}".L10nFormat(result.sucessList.Count, result.errorList.Count);
                    strs.Add(str);

                    erpDataInfLog.CallResult = result.success ? CallResult.Success : CallResult.Fail;
                    erpDataInfLog.ErrorMsg = result.errorMsg;
                }
                catch (Exception ex)
                {
                    erpDataInfLog.ErrorMsg = ex.GetBaseException().Message;
                    erpDataInfLog.CallResult = CallResult.Fail;
                    throw new ValidationException(ex.GetBaseException().Message);
                }
                finally
                {
                    erpDataInfLog.ResponseContent = JsonConvert.SerializeObject(result);
                    erpDataInfLog.EndDate = DateTime.Now;
                    erpDataInfLog.Qty = result.errorList.Count + result.sucessList.Count;
                    RF.Save(erpDataInfLog);
                }
            }

            return string.Join(";", strs);
        }

        #endregion
    }
}
