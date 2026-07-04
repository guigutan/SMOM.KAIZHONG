using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.Items;
using SIE.Items.ProductBoms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 产品BOM明细下载控制器
    /// </summary>
    public class DownloadProductBomDtlController : DomainController
    {
        /// <summary>
        /// 从API下载产品BOM到业务表
        /// </summary>
        /// <param name="productBomDtlDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadProductBomDtlToBusiness(List<ProductBomDetailData> productBomDtlDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ProductBomDetailData>(
                productBomDtlDatas,
                p => this.SaveProductBomDetails(p.OrderByLastUpdateDate()),
                JobType.ProductBomDtl,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载产品BOM到业务表
        /// </summary>
        public virtual ProcessResult DownloadProductBomDtlInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ProductBomDetailInf>(
                () => ctl.GetUnprocessedDatas<ProductBomDetailInf>(),          //产品BOM明细中间表数据
                p =>
                {
                    var paras = this.GenerateProductBomDtlPara(p);
                    return this.SaveProductBomDetails(paras.OrderByLastUpdateDate());
                },
                JobType.ProductBomDtl, isManual);
        }

        /// <summary>
        /// 生成产品BOM明细实体
        /// </summary>
        /// <param name="productBomDtlInfs">中间表实体数据</param>
        /// <returns></returns>
        public virtual List<ProductBomDetailData> GenerateProductBomDtlPara(IEnumerable<ProductBomDetailInf> productBomDtlInfs)
        {
            var paras = new List<ProductBomDetailData>();

            productBomDtlInfs.ForEach(p =>
            {
                var data = new ProductBomDetailData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.ProductBomCode = p.ProductBomCode;
                data.ItemCode = p.ItemCode;
                data.LossRate = p.LossRate;
                data.UnitQty = p.UnitQty;
                data.Remark = p.Remark;
                data.ErpKey = p.ErpKey;

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// 保存产品BOM明细
        /// </summary>
        /// <param name="datas">ERP数据</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveProductBomDetails(List<ProductBomDetailData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            var ctl = RT.Service.Resolve<ProductBomController>();

            //获取BOM数据
            var boms = ctl.GetProductBoms(datas.Select(p => p.ProductBomCode).Distinct().ToList());
            var dicBom = boms.ToDictionary(p => p.Code);    //<bom编码,bom>

            //获取BOM明细数据
            var bomDetails = ctl.GetProductBomDetails(boms.Select(p => p.Id).ToList(), null);
            var dicBomDetails = bomDetails.GroupBy(p => p.ProductBom.Code).ToDictionary(p => p.Key, p => p.ToList());    //<bom编码,bom明细列表>

            //物料字典数据
            var itemCodes = datas.Select(p => p.ItemCode).ToList();
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes.Distinct().ToList());
            var dicItem = items.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var data in datas)
            {
                try
                {
                    var key = data.ProductBomCode;  //产品BOM编码为主键
                    if (!dicBom.ContainsKey(key))
                        throw new ValidationException("产品BOM[{0}]不存在".L10nFormat(key));
                    var bom = dicBom[key];
                    if (!dicBomDetails.ContainsKey(key))
                        dicBomDetails.Add(key, new List<ProductBomDetail>());
                    var dicDetail = dicBomDetails[key].ToDictionary(p => p.Item.Code);

                    SaveProductBomDetail(data, dicDetail, dicItem, bom);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = data.Infkey });
                }
            }

            return errors;
        }

        /// <summary>
        /// 执行数据保存
        /// </summary>
        /// <param name="data">数据实体</param>
        /// <param name="dic">数据字典</param>
        /// <param name="dicItem">数据字典</param>
        /// <param name="bom">产品BOM</param>
        public virtual void SaveProductBomDetail(ProductBomDetailData data, Dictionary<string, ProductBomDetail> dic, Dictionary<string, Item> dicItem, ProductBom bom)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            ProductBomDetail entity;
            var key = data.ItemCode;
            if (key.IsNullOrEmpty())
                throw new ValidationException("产品BOM[{0}]的明细行物料编码为空".L10nFormat(bom.Code));
            //处理待删除数据
            if (dic.ContainsKey(key))
            {
                if (data.IsDelete)
                {
                    ctl.DeleteEntity(dic, key, dic[key]);
                }
                return;
            }
            if (!dic.ContainsKey(key))
                dic.Add(key, new ProductBomDetail());
            entity = dic[key];

            entity.Item = dicItem[data.ItemCode];
            entity.UnitQty = data.UnitQty;
            entity.LossRate = data.LossRate;
            entity.Remark = data.Remark;
            entity.ProductBom = bom;
            RF.Save(entity);
        }
    }
}
