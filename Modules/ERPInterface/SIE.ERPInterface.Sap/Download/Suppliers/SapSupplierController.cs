using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Common.Logs;
using SIE.ERPInterface.Sap.Connection;
using SIE.ERPInterface.Sap.Datas;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Sap.Download.Items
{
    /// <summary>
    /// 供应商下载控制器
    /// </summary>
    public class SapSupplierController : DomainController
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

            //获取下载时间戳
            var jobTime = ctl.GetDownloadJobTime(JobType.Supplier);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            //连接SAP下载数据
            SapResult sapResult = null;
            sapResult = this.SapDownloadSuppliers(keyWord, jobTime?.LastDownloadDate);

            //执行保存中间表
            var result = ctl.SaveInfData<SupplierInf, SupplierInfo>(sapResult, p =>
             {
                 var supplier = new SupplierInf();
                 supplier.IsManual = isManual;
                 supplier.Code = p.LIFNR;
                 supplier.Name = p.NAME1;
                 supplier.ErpKey = p.LIFNR;

                 return supplier;
             }, JobType.Item, jobTime, jobTimeDetail, isManual);

            return result;
        }

        /// <summary>
        ///供应商下载接口
        /// </summary>
        /// <param name="supplierCode">供应商编码</param>
        /// <param name="lastUpdateTime"></param>
        /// <returns></returns>
        private SapResult SapDownloadSuppliers(string supplierCode, DateTime? lastUpdateTime)
        {
            //构建查询参数
            var parameter = new SupplierParameter();
            parameter.Selection = new Selection()
            {
                ExtCode = supplierCode,
                LastUpdateTime = lastUpdateTime
            };

            var result = RfcHelper.Sap<SupplierResult, SupplierParameter>("ZFM_MES_SUPPLIER", parameter, p =>
           {
                  //转换结果数据列表
                  if (p.SupplierList != null)
                   return p.SupplierList.ToList();
               else
                   return new List<SupplierInfo>();
           });
            return result;
        }
    }
}
