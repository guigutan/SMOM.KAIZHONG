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
    /// 物料下载控制器
    /// </summary>
    public class SapItemController : DomainController
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
            var jobTime = ctl.GetDownloadJobTime(JobType.Item);
            var jobTimeDetail = new DownloadJobTimeDetail();
            jobTimeDetail.GenerateId();

            //连接SAP下载数据
            SapResult sapResult = null;
            sapResult = this.SapDownloadItems(keyWord, jobTime?.LastDownloadDate);

            //执行保存中间表
            var result = ctl.SaveInfData<ItemInf, ItemInfo>(sapResult, p =>
             {
                 var item = new ItemInf();
                 item.IsManual = isManual;
                 item.Code = p.MATNR;
                 item.ErpKey = p.MATNR;
                 item.Name = p.MAKTX;
                 item.Description = p.MAKTX;
                 item.Unit = p.MEINS;
                 item.ItemType = p.MTART;
                 item.MrpPerson = p.DISPO;
                 return item;

             }, JobType.Item, jobTime, jobTimeDetail, isManual);

            return result;
        }

        /// <summary>
        /// 物料下载接口
        /// </summary>
        /// <param name="itemCode">物料编码</param>
        /// <param name="lastUpdateTime"></param>
        /// <returns></returns>
        private SapResult SapDownloadItems(string itemCode, DateTime? lastUpdateTime)
        {
            //构建查询参数
            var parameter = new ItemParameter()
            {
                InvOrgId = RT.InvOrg.Value,
                ItemCode = itemCode,
                LastUpdateTime = lastUpdateTime
            };

            var result = RfcHelper.Sap<ItemResult, ItemParameter>("ZFM_MES_MATERIAL", parameter, p =>
              {
                  //转换结果数据列表
                  if (p.ItemList != null)
                      return p.ItemList.ToList();
                  else
                      return new List<ItemInfo>();
              });
            return result;
        }
    }
}
