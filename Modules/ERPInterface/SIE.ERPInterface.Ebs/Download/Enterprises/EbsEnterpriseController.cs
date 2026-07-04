using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Datas.EbsData;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Resources.Enterprises;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.Enterprises
{
    /// <summary>
    /// 企业模型数据下载
    /// </summary>
    public class EbsEnterpriseController : DomainController
    {
        /// <summary>
        /// 企业模型数据下载
        /// </summary>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId.Value;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_ENTERPRISE";//接口编码，接口卡有
            const JobType jobType = JobType.Enterprise;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<EnterpriseDataEbs>(ebsPara);

            var allDatas = RF.GetAll<SIE.Resources.Enterprises.Enterprise>();

            soapResult.XV_RESULT.RemoveAll(x => allDatas.Select(a => a.Code).Contains(x.Enterprise_Code) || x.Enable_Flag != "Y" || x.Description.IsNullOrEmpty());
            soapResult.XV_RESULT.RemoveAll(x => x.Level_Code >= 2 && x.Organization_Id != invOrgId);//去掉跟当前库存组织无关的数据
            var levels = RT.Service.Resolve<EbsEnterpriseLevelController>().InitLevel();
            List<string> errorStr = new List<string>();
            soapResult.XV_RESULT.ForEach(f =>
            {
                switch (f.Level_Code)
                {
                    case 0:
                        f.LevelId = levels.FirstOrDefault(a => a.Type == EnterpriseType.Group)?.Id; break;
                    case 1:
                        f.LevelId = levels.FirstOrDefault(a => a.Type == EnterpriseType.Company)?.Id; break;
                    case 2:
                        f.LevelId = levels.FirstOrDefault(a => a.Type == EnterpriseType.Plant)?.Id; break;
                    case 3:
                        f.LevelId = levels.FirstOrDefault(a => a.Type == EnterpriseType.Department)?.Id; break;
                    case 4:
                        f.LevelId = levels.FirstOrDefault(a => a.Type == EnterpriseType.Shop)?.Id; break;
                    default:
                        errorStr.Add(f.Description);
                        break;
                }
            });
            if (errorStr.Any())
            {
                var rwt = new ProcessResult();
                rwt.AddFailMsg("以下数据没有对应的企业层级" + string.Join(',', errorStr));
                return rwt;
            }
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (p.LevelId != null)
                 {
                     var data = new SIE.Resources.Enterprises.Enterprise()
                     {
                         Code = p.Enterprise_Code,
                         Name = p.Description,
                         ErpOrgId = p.Organization_Id,
                         IsResource = p.Is_Resource == "Y",
                         LevelId = p.LevelId.Value,
                         InvOrgId = p.Organization_Id,
                     };

                     switch (p.Level_Code)
                     {
                         case 0:
                             data.LevelType = Resources.Enterprises.EnterpriseType.Group; break;
                         case 1:
                             data.LevelType = Resources.Enterprises.EnterpriseType.Company; break;
                         case 2:
                             data.LevelType = Resources.Enterprises.EnterpriseType.Plant; break;
                         case 3:
                             data.LevelType = Resources.Enterprises.EnterpriseType.Department; break;
                         case 4:
                             data.LevelType = Resources.Enterprises.EnterpriseType.Shop; break;
                         default: break;
                     }
                     if (data.LevelType == EnterpriseType.Group || data.LevelType == EnterpriseType.Company)
                         data.InvOrgId = 0;
                     data.GenerateId();
                     return data;
                 }
                 return null;
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);

            var ents = Query<Enterprise>().Where(p => p.InvOrgId == invOrgId || p.InvOrgId == 0).ToList();
            soapResult.XV_RESULT.Where(a => a.Parent_Code.IsNotEmpty()).GroupBy(a => a.Parent_Code).ForEach(a =>
              {
                  var parEnt = ents.FirstOrDefault(p => p.Code == a.Key);
                  if (parEnt != null)
                  {
                      var sonCodes = a.Select(p => p.Enterprise_Code).ToList();
                      ents.Where(p => sonCodes.Contains(p.Code)).ForEach(p => p.TreePId = parEnt.Id);
                      sonCodes.SplitDataExecute(sc =>
                      {
                          DB.Update<Enterprise>().Set(f => f.TreePId, parEnt.Id).Where(f => sc.Contains(f.Code)).Execute();
                      });
                  }
              });
             
            //批量更新企业模型的父id
            return result;
        }
    }
}
