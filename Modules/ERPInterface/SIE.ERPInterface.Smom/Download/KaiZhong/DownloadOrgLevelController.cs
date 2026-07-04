using Newtonsoft.Json;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MES.OrgLevels;
using SIE.Rbac.Users;
using SIE.Resources.Employees;
using SIE.Security.Authentications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.ERPInterface.Smom.Download.KaiZhong
{
    /// <summary>
    /// 
    /// </summary>
    public class DownloadOrgLevelController : DomainController
    {


        /// <summary>
        /// 从API下载数据到业务表
        /// </summary>
        /// <param name="datas"></param>
        /// <param name="erpDataInfLog"></param>
        /// <returns></returns>
        /// <exception cref="ValidationException"></exception>
        public virtual ApiCommonRes SaveOrgLevels(List<SIE.MES.OrgLevels.OrgLevelInfo> datas, ref InfNcDataLogGroup erpDataInfLog)
        {
            //返回ERP结果
            ApiCommonRes apiResult = new ApiCommonRes() { DataCount = datas.Count };

            List<SIE.MES.OrgLevels.OrgLevelInfo> list = new List<SIE.MES.OrgLevels.OrgLevelInfo>();
            var dataJson = JsonConvert.SerializeObject(datas);
            int failCount = 0;
            var orgcodes = datas.Select(p => p.OrgCode).Distinct().ToList();
            var orglevels = RT.Service.Resolve<SIE.MES.OrgLevels.OrgLevelController>().GetOrgLevelList(orgcodes);          
            var invOrg = Query<Rbac.InvOrgs.InvOrg>().Where(p => p.Code == RT.InvOrg.Value).FirstOrDefault();
            if (invOrg == null)
                throw new ValidationException("库存组织[{0}]不存在".L10nFormat(RT.InvOrg.Value));
            try
            {
                if (datas != null || datas.Count > 0)
                {
                    foreach (var item in datas)
                    {
                        try
                        {
                            if (item != null)
                            {
                                var orglevel = orglevels.FirstOrDefault(p => p.OrgCode == item.OrgCode);
                                orglevel = CreateOrglevel(orglevel, item, invOrg);

                                if (orglevels.All(p => p.Id != orglevel.Id))
                                    orglevels.Add(orglevel);

                                list.Add(item);
                                apiResult.SuccessList.Add(item);
                            }
                        }
                        catch (Exception ex)
                        {
                            apiResult.ErrorList.Add($"编码{item.OrgCode}:" + ex.GetBaseException()?.Message);
                            failCount++;
                            continue;
                        }

                    }

                    apiResult.SuccessCount = list.Count;
                    apiResult.FailCount = failCount;
                }
                else
                {
                    apiResult.ErrorList.Add("同步数据不能未空!".L10N());
                }
            }
            catch (Exception ex)
            {
                apiResult.ErrorList.Clear();
                apiResult.FailCount = datas.Count;
                apiResult.ErrorList.Add(ex.Message);
            }
            finally
            {
                erpDataInfLog = RT.Service.Resolve<SIE.KZ.Base.SmomControl.InfNcDataLogGroupController>().UpdateInfNcDataLogGroupData<SIE.MES.OrgLevels.OrgLevelInfo>(erpDataInfLog, datas.Count, list, apiResult, false);
            }
            return apiResult;

        }






        /// <summary>
        /// 赋值
        /// </summary>
        /// <param name="orglevel"></param>
        /// <param name="item"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        private SIE.MES.OrgLevels.OrgLevel CreateOrglevel(SIE.MES.OrgLevels.OrgLevel orglevel, SIE.MES.OrgLevels.OrgLevelInfo item, Rbac.InvOrgs.InvOrg invOrg)
        {
            if (orglevel == null)
            {
                orglevel=new OrgLevel ();
                orglevel.OrgCode = item.OrgCode;
                orglevel.OrgName = item.OrgName;
                orglevel.ParentLevel = item.ParentLevel;
                orglevel.TheLevel = item.TheLevel;
                orglevel.GenerateId();
                orglevel.PersistenceStatus = PersistenceStatus.New;
                RF.Save(orglevel);                
            }
            RF.Save(orglevel);
            return orglevel;
        }










    }
}
