using SIE.Domain;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using SIE.Items.Units;
using SIE.Packages;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Ebs.Download.Items
{
    /// <summary>
    /// 单位下载
    /// </summary>
    public class EbsUnitController : DomainController
    {
        /// <summary>
        /// 下载单位数据
        /// </summary>
        /// <param name="invOrgId"></param>
        /// <param name="isManual">是否手工下载</param>        
        /// <returns>处理结果</returns>
        public virtual ProcessResult Download(int? invOrgId, bool isManual = false)
        {
            if (invOrgId.HasValue)
                AppRuntime.InvOrg = invOrgId.Value;
            var ebsPara = EbsHelper.GetEbsParameter(false);
            //Copy必改内容
            ebsPara.InterfaceCode = "S_E2W_UNIT";//接口编码，接口卡有
            const JobType jobType = JobType.Unit;
            //End

            var ctl = RT.Service.Resolve<DownloadInfBaseController>();
            var jobTime = ctl.GetDownloadJobTime(jobType);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            if (jobTime?.LastDownloadDate.HasValue == true)
                ebsPara.DownParameter.LastUpdateDate = jobTime.LastDownloadDate.Value;

            DateTime beginTime = DateTime.Now;
            //ERP数据获取
            var soapResult = EbsHelper.ExecuteEbs<ItemUnit>(ebsPara);
            var allDatas = RF.GetAll<SIE.Items.Unit>();
            List<string> excodes = new List<string>();
            List<string> names = new List<string>();
            //移除不存在并且是失效的数据
            soapResult.XV_RESULT.RemoveAll(x => !allDatas.Select(a => a.Code.ToUpper()).Contains(x.Uom_Code.ToUpper()) && x.Enable_Flag != "Y"||x.Uom_Code=="t");
            soapResult.XV_RESULT.ForEach(p =>
            {//单位以名称为准，大家名称一样的就用ERP的编码
                p.Unit = allDatas.FirstOrDefault(a => a.Name.ToUpper() == p.Unit_Of_Measure.ToUpper());
                if (excodes.Contains(p.Uom_Code.ToUpper()))
                {
                    p.IsRepeat = true;
                }
                else
                {
                    excodes.Add(p.Uom_Code.ToUpper());
                }
                if (names.Contains(p.Unit_Of_Measure.ToUpper()))
                {
                    p.IsRepeat = true;
                }
                else
                {
                    names.Add(p.Unit_Of_Measure.ToUpper());
                }
            });
            var result = RT.Service.Resolve<DownloadInfBaseController>().DownloadBusData(soapResult, p =>
             {    //Copy必改内容
                 if (p.IsRepeat)
                     return null;
                 if (p.Unit == null)
                 {
                     var data = new SIE.Items.Unit()
                     {
                         Code = p.Uom_Code,
                         Name = p.Unit_Of_Measure,
                         Type = p.Uom_Class,
                         Precision = 6,
                         UnitSource = SIE.Items.Items.UnitSource.ERP,
                         TradeType = SIE.Items.TradeType.HalfAdjust,
                     };
                     return data;
                 }
                 else
                 {
                     if (p.Enable_Flag == "Y")
                     {
                         p.Unit.Code = p.Uom_Code;
                         p.Unit.Name = p.Unit_Of_Measure;
                         p.Unit.Type = p.Uom_Class;
                         return p.Unit;
                     }
                     else
                         return null;
                 }
             }, jobType, jobTime, jobTimeDetail, beginTime, isManual);
           
            return result;
        }
    }
}
