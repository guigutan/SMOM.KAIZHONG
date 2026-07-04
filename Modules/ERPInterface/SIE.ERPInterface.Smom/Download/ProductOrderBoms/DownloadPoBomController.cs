using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.ProductOrderBoms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 生产订单BOM下载控制器
    /// </summary>
    public class DownloadPoBomController : DomainController
    {
        /// <summary>
        /// 从API下载生产订单BOM到业务表
        /// </summary>
        /// <param name="poBomDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadPoBomToBusiness(List<ProductionOrderBomData> poBomDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ProductionOrderBomData>(
                poBomDatas,
                p => this.SaveProductionOrderBoms(p.OrderByLastUpdateDate()),
                JobType.ProductOrderBom,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载生产订单BOM到业务表
        /// </summary>
        public virtual ProcessResult DownloadPoBomInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ProductOrderBomInf>(
                () => ctl.GetUnprocessedDatas<ProductOrderBomInf>(),       // 生产订单BOM中间表数据
                p =>
                {
                    //执行业务接口
                    var paras = this.GeneratePoBomPara(p);
                    return this.SaveProductionOrderBoms(paras.OrderByLastUpdateDate());
                },
                JobType.Customer, isManual);
        }

        /// <summary>
        /// 生成生产订单BOM实体
        /// </summary>
        /// <param name="poBomInfs">中间表实体数据</param>
        /// <returns></returns>
        private List<ProductionOrderBomData> GeneratePoBomPara(IEnumerable<ProductOrderBomInf> poBomInfs)
        {
            var paras = new List<ProductionOrderBomData>();

            poBomInfs.ForEach(p =>
            {
                var data = new ProductionOrderBomData();
                data.ErpKey = p.ErpKey;
                data.Code = p.Code;
                data.Name = string.Empty;
                data.Infkey = p.Id;
                data.IsDelete = p.IsDelete;
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;

                data.ItemCode = p.ItemCode;
                data.SpecificationDesc = p.SpecificationDesc;
                //data.ReplateItemType =(int) p.ReplateItemType;
                data.MainMaterialCode = p.MainMaterialCode;
                data.ElementNo = p.ElementNo;
                data.RequireQty = p.RequireQty;
                data.ProcessTech = p.ProcessTech;
                data.Remark = p.Remark;

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
                    RT.Service.Resolve<SoapPoBomController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadPoBomInfToBusiness(true);           //执行业务表下载
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
        /// 保存生产订单BOM接口
        /// </summary>
        /// <param name="datas">生产订单BOM数据</param>
        public virtual List<ErpErrorData> SaveProductionOrderBoms(List<ProductionOrderBomData> datas)
        {
            SaveProductionOrderBomsHandle handle = new SaveProductionOrderBomsHandle();
            return handle.SaveProductionOrderBoms(datas);
        }
    }
}