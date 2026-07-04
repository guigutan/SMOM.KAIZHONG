using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Rbac.InvOrgs;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Download.Warehouses
{
    /// <summary>
    /// ERP子库下载
    /// </summary>
    public class EbsWarehouseController : DomainController
    {
        /// <summary>
        /// 下载ERP子库数据
        /// </summary>
        /// <param name="invOrgId">库存组织</param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_WHORG";//接口编码，接口卡有
            const JobType jobType = JobType.ErpWarehouse;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<SIE.ERPInterface.Common.Datas.EbsData.ErpWarehouseData>(ebsPara);

            var allDatas = RF.GetAll<ErpWarehouse>();
            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code).Contains(x.Secondary_Inventory_Name) && x.Enable_Flag != "Y");

            var invOrgs = RF.GetAll<InvOrg>();

            List<string> names = new List<string>();
            soapResult.XV_RESULT.ForEach(p =>
            {
                p.ErpWarehouse = allDatas.FirstOrDefault(a => a.Code == p.Secondary_Inventory_Name && a.ErpOrgId == p.Organization_Id.ToString());
                if (p.Description.IsNullOrEmpty())
                    p.Description = p.Secondary_Inventory_Name;
                if (names.Contains(p.Description))
                    p.IsRepeat = true;
                else
                {
                    names.Add(p.Description);
                    p.InvOrg = invOrgs.FirstOrDefault(a => a.ExternalId == p.Organization_Id.ToString());
                }
            });

            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
            {    //Copy必改内容
                if (!p.IsRepeat && p.InvOrg != null)
                {
                    if (p.ErpWarehouse == null)
                    {
                        var erpWarehouse = new ErpWarehouse()
                        {
                            ErpOrgId = p.Organization_Id.ToString(),
                            ErpOrgCode = p.Organization_Code,
                            ErpOrgName = p.InvOrg.Name,
                            Code = p.Secondary_Inventory_Name,
                            Name = p.Description,
                            State = State.Enable,
                            WmsInvOrg = p.InvOrg.Code.ToString(),
                        };

                        return erpWarehouse;
                    }
                    else
                    {
                        if (p.Enable_Flag != "Y")
                            p.ErpWarehouse.State = State.Disable;
                        else
                        {
                            p.ErpWarehouse.State = State.Enable;
                            p.ErpWarehouse.Name = p.Description;
                        }
                        return p.ErpWarehouse;
                    }
                }
                return null;
            }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            return result;
        }
    }
}
