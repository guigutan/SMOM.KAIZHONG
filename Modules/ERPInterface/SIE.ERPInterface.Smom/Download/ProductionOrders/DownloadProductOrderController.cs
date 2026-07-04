using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.ProductionOrders;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 生产订单下载控制器
    /// </summary>
    public class DownloadProductOrderController : DomainController
    {
        /// <summary>
        /// 从API下载生产订单到业务表
        /// </summary>
        /// <param name="poDatas">生产订单数据</param>
        /// <param name="invOrg">库存组织</param>
        /// <returns></returns>
        public virtual ApiResult DownloadProOrderToBusiness(List<ProductOrderData> poDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ProductOrderData>(
                poDatas,
                p => this.DownLoadProductOrders(p.OrderByLastUpdateDate()),
                JobType.ProductOrderBom,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载生产订单到业务表
        /// </summary>
        public virtual ProcessResult DownloadProOrderInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ProductOrderInf>(
                () => ctl.GetUnprocessedDatas<ProductOrderInf>(),       // 生产订单中间表数据
                p =>
                {
                    //执行业务接口
                    var paras = this.GenerateProOrderPara(p);
                    return this.DownLoadProductOrders(paras.OrderByLastUpdateDate());
                },
                JobType.Customer, isManual);
        }

        /// <summary>
        /// 生成生产订单实体
        /// </summary>
        /// <param name="poInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<ProductOrderData> GenerateProOrderPara(IEnumerable<ProductOrderInf> poInfs)
        {
            var paras = new List<ProductOrderData>();

            poInfs.ForEach(p =>
            {
                var data = new ProductOrderData();
                data.ErpKey = p.ErpKey;
                data.Code = p.Code;
                data.Name = string.Empty;
                data.Infkey = p.Id;
                data.IsDelete = p.IsDelete;
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;

                data.ItemCode = p.ItemCode;
                data.Qty = p.Qty;
                data.Priority = p.Priority;
                data.RouteCode = p.RouteCode;
                //data.OrderType = (int)p.OrderType;
                data.FactoryCode = p.FactoryCode;
                data.SaleNo = p.SaleNo;
                data.CustomerCode = p.CustomerCode;
                data.RequireDelivery = p.RequireDelivery;
                data.PromiseDelivery = p.PromiseDelivery;
                data.RawMaterialDate = p.RawMaterialDate;
                data.SuggestStart = p.SuggestStart;
                data.SuggestEnd = p.SuggestEnd;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 手动下载
        /// </summary>
        /// <param name="keyWord">查询关键字</param>
        public virtual string DownloadManual(string keyWord)
        {
            ProcessResult result = new ProcessResult();
            string resultMsg = string.Empty;

            try
            {
                if (keyWord.IsNullOrEmpty())
                    throw new ValidationException("唯一主键不能为空".L10N());
                using (var trans = DB.TransactionScope(InterfaceEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<SoapProductOrderController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadProOrderInfToBusiness(true);           //执行业务表下载
                    trans.Complete();
                }
            }
            catch (Exception ex)
            {
                result.AddFailMsg(ex.GetBaseException());
            }

            if (!result.Result) resultMsg = result.FailMsg.FirstOrDefault();
            return resultMsg;
        }

        /// <summary>
        /// 保存生产订单接口
        /// </summary>
        /// <param name="datas">生产订单数据</param>
        /// <returns>返回错误列表</returns>
        public virtual List<ErpErrorData> DownLoadProductOrders(List<ProductOrderData> datas)
        {
            SaveProductOrderHandle handle = new SaveProductOrderHandle();
            return handle.SaveProductOrders(datas);
        }
    }
}
