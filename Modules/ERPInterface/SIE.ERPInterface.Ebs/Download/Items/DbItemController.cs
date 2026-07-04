using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Ebs.Connection;
using System;
using System.Data;

namespace SIE.ERPInterface.Ebs.Download.Items
{
    public class DbItemController : DomainController
    {
        /// <summary>
        /// 从ERP下载物料到中间表
        /// </summary>
        public virtual ProcessResult DownloadToInf(bool isManual = false, string keyWord = null)
        {
            var ctl = RT.Service.Resolve<DownloadInfBaseController>();

            var jobTime = ctl.GetDownloadJobTime(JobType.Item);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();
            jobTimeDetail.RequestStr = "CUX_ERP_MES_INTERFACE_PKG.ERP_MES_ITEM";
            var dt = DbHelper.ExecuteProcedure("CUX_ERP_MES_INTERFACE_PKG.ERP_MES_ITEM", jobTime?.LastDownloadDate, keyWord);
            var result = ctl.SaveInfData<ItemInf, DownloadBaseEntity>(dt.Select(), CreateInfData, null, JobType.Item, jobTime, jobTimeDetail, DateTime.Now, isManual);
            return result;
        }

        /// <summary>
        /// 创建接口数据
        /// </summary>
        /// <param name="row">数据行</param>
        /// <returns>接口数据</returns>
        private ItemInf CreateInfData(DataRow row)
        {
            var itemInf = new ItemInf();
            itemInf.Code = row["ITEM_CODE"].ToString();
            itemInf.Name = row["ITEM_NAME"].ToString();
            itemInf.Description = row["ITEM_NAME"].ToString();
            itemInf.Unit = row["STOCK_UNIT_CODE"].ToString();
            itemInf.LastUpdateDate = DateTime.Parse(row["LAST_UPDATE_DATE"].ToString());
            itemInf.ErpKey = itemInf.Code;
            return itemInf;
        }
    }
}
