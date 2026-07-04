using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Xml;

namespace SIE.ERPInterface.Ebs.Download.Items
{
    /// <summary>
    /// 物料下载控制器
    /// </summary>
    public class SoapItemController : DomainController
    {
        /// <summary>
        /// 下载到中间表
        /// </summary>
        /// <param name="isManual">是否手工下载</param>
        /// <param name="keyWord">查询关键字</param>
        /// <returns>处理结果</returns>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();

            var jobTime = ctl.GetDownloadJobTime(JobType.Item);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            var soapPara = SoapHelper.GetSoapParameter();
            soapPara.P_SERVICE_NAME = "GET_INVENTORY_ITEM";
            soapPara.P_BATCH_ID = jobTimeDetail.Id;
            if (isManual)
            {
                soapPara.SoapDownloadParameter.Code_Label = "ITEM_CODE";
                soapPara.SoapDownloadParameter.Code = keyWord;
            }

            soapPara.SoapDownloadParameter.LAST_UPDATE_DATE_FROM = jobTime?.LastDownloadDate?.ToString();

            //ERP数据获取
            var soapResult = SoapHelper.ExecuteSoap(soapPara, true);

            var result = ctl.SaveInfData<ItemInf>(soapResult, p =>
            {
                XmlNode xmlCode = p.SelectSingleNode("ITEM_CODE");
                XmlNode xmlName = p.SelectSingleNode("ITEM_NAME");

                ////XmlNode xmlIsLotControl = p.SelectSingleNode("LOT_CONTROL");
                XmlNode xmlUnit = p.SelectSingleNode("STOCK_UNIT_CODE");
                XmlNode xmlLastUpdateDate = p.SelectSingleNode("LAST_UPDATE_DATE");

                var itemInf = new ItemInf();
                itemInf.Code = xmlCode.InnerText;
                itemInf.Name = xmlName.InnerText;
                itemInf.Description = xmlName.InnerText;
                itemInf.Unit = xmlUnit.InnerText;
                itemInf.LastUpdateDate = DateTime.Parse(xmlLastUpdateDate.InnerText);
                itemInf.ErpKey = itemInf.Code;
                itemInf.IsManual = isManual;
                return itemInf;

            }, JobType.Item, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }
    }
}
