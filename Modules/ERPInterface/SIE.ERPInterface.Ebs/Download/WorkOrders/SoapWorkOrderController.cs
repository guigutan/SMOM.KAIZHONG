using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Xml;

namespace SIE.ERPInterface.Download.WorkOrders
{
    /// <summary>
    /// 工单下载控制器
    /// </summary>
    public class SoapWorkOrderController : DomainController
    {
        #region 声明变量

        /// <summary>
        /// 接口类型
        /// </summary>
        public const string CodeLabel = "WIP_ENTITY_NAME";

        #endregion

        /// <summary>
        /// 执行数据保存中间表
        /// </summary>
        /// <param name="soapPara"></param>
        /// <param name="isManual"></param>
        /// <param name="keyWord">查询关键字</param>
        private ProcessResult ExecuteSaveInfData(SoapParameter soapPara, bool isManual, string keyWord = null)
        {
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();

            //下载记录时间
            var jobTime = ctl.GetDownloadJobTime(JobType.WorkOrder);
            var jobTimeDetail = new DownloadJobTimeDetail();

            //构建报文参数
            jobTimeDetail.GenerateId();
            soapPara.P_SERVICE_NAME = "GET_WIP_ENTITY";
            soapPara.P_BATCH_ID = jobTimeDetail.Id;
            soapPara.SoapDownloadParameter.Code_Label = CodeLabel;
            if (isManual)
            {
                soapPara.SoapDownloadParameter.Code_Label = "WIP_ENTITY_NAME";
                soapPara.SoapDownloadParameter.Code = keyWord;
            }

            soapPara.SoapDownloadParameter.LAST_UPDATE_DATE_FROM = jobTime?.LastDownloadDate?.ToString();

            //webservice获取ERP数据
            var soapResult = SoapHelper.ExecuteSoap(soapPara, true);

            var result = ctl.SaveInfData<WorkOrderInf>(soapResult, p =>
            {
                XmlNode xmlNo = p.SelectSingleNode("WIP_ENTITY_NAME");
                XmlNode xmlProductCode = p.SelectSingleNode("SEGMENT1");
                XmlNode xmlPlanQty = p.SelectSingleNode("START_QUANTITY");
                XmlNode xmlOrderQty = p.SelectSingleNode("NET_QUANTITY");
                XmlNode xmlPlanBeginDate = p.SelectSingleNode("SCHEDULED_START_DATE");
                XmlNode xmlPlanEndDate = p.SelectSingleNode("SCHEDULED_COMPLETION_DATE");
                ////XmlNode xmlSate = p.SelectSingleNode("STATUS_TYPE");
                ////XmlNode xmlType = p.SelectSingleNode("CLASS_CODE");
                ////XmlNode xmlInvOrg = p.SelectSingleNode("ORGANIZATION_ID");
                XmlNode xmlLastUpdateDate = p.SelectSingleNode("LAST_UPDATE_DATE");

                var workOrderInf = new WorkOrderInf();
                workOrderInf.No = xmlNo.InnerText;
                workOrderInf.ProductCode = xmlProductCode.InnerText;
                workOrderInf.PlanQty = decimal.Parse(xmlPlanQty.InnerText);
                workOrderInf.OrderQty = decimal.Parse(xmlOrderQty.InnerText);
                workOrderInf.PlanBeginDate = DateTime.Parse(xmlPlanBeginDate.InnerText);
                workOrderInf.PlanEndDate = DateTime.Parse(xmlPlanEndDate.InnerText);
                workOrderInf.MakeDate = DateTime.Now;
                workOrderInf.LastUpdateDate = DateTime.Parse(xmlLastUpdateDate.InnerText);
                workOrderInf.ErpKey = workOrderInf.No;
                ////workOrderInf.WorkOrderState = xmlSate.InnerText;
                ////workOrderInf.WorkOrderType = workOrderInf.No;       //需要从工单号取首字母，不能确认取几位，所以整个赋值
                workOrderInf.IsManual = isManual;

                return workOrderInf;
            }, JobType.WorkOrder, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }

        /// <summary>
        /// 下载到中间表
        /// </summary>
        /// <param name="isManual">是否手工下载</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>处理结果</returns>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            var soapPara = SoapHelper.GetSoapParameter();
            var result = ExecuteSaveInfData(soapPara, isManual, keyWord);
            return result;
        }

        /// <summary>
        /// 从ERP下载工单BOM中间表
        /// </summary>
        public virtual ProcessResult DownloadWorkOrderBomERPToInf()
        {
            return null;
        }
    }
}
