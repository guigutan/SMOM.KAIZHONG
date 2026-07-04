using SIE.Core.Common;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ERPInterface.Common;
using SIE.ERPInterface.Common.Controller;
using SIE.ERPInterface.Common.Datas;
using SIE.ERPInterface.Common.Enums;
using SIE.ERPInterface.Common.InfDataEntitys.Download;
using SIE.ERPInterface.Ebs.Download.ProductBoms;
using SIE.Items;
using SIE.Items.ProductBoms;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// 产品BOM下载控制器
    /// </summary>
    public class DownloadProductBomController : DomainController
    {
        /// <summary>
        /// 从API下载产品BOM到业务表
        /// </summary>
        /// <param name="productBomDatas"></param>
        /// <param name="invOrg"></param>
        /// <returns></returns>
        public virtual ApiResult DownloadProductBomToBusiness(List<ProductBomData> productBomDatas, int invOrg)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.ApiSaveBusinessData<ProductBomData>(
                productBomDatas,
                p => this.SaveProductBoms(p.OrderByLastUpdateDate()),
                JobType.ProductBom,
                invOrg);
        }

        /// <summary>
        /// 从中间表下载产品BOM到业务表
        /// </summary>
        public virtual ProcessResult DownloadProductBomInfToBusiness(bool isManual = false)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            return ctl.SaveBusinessData<ProductBomInf, ProductBomDetailInf>(
                () => ctl.GetUnprocessedDatas<ProductBomInf>(),
                p =>
                {
                    //产品BOM明细中间表数据
                    var productBomCodes = p.Select(y => y.Code).Distinct().ToList();
                    var whereDtl = productBomCodes.CreateContainsExpression<ProductBomDetailInf>("x", ProductBomDetailInf.ProductBomCodeProperty.Name);
                    var dtlDatas = ctl.GetUnprocessedDatas(whereDtl);

                    return dtlDatas;
                },
                (x, y) =>
                {
                    //构建明细数据嵌套字典
                    var dtlDataDicts = ctl.GenerateDictionarys<string, ProductBomDetailInf>(y, ProductBomDetailInf.ProductBomCodeProperty);

                    //调用业务接口
                    var paras = this.GenerateProductBomPara(x, dtlDataDicts);
                    return this.SaveProductBoms(paras.OrderByLastUpdateDate());
                },
                JobType.ProductBom, JobType.ProductBomDtl, isManual);
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
                    RT.Service.Resolve<SoapProductBomController>().DownloadToInf(true, keyWord);                     //执行中间表下载
                    result = DownloadProductBomInfToBusiness(true);           //执行业务表下载
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
        /// 生成产品BOM实体
        /// </summary>
        /// <param name="productBomInfs">中间表实体数据</param>
        /// <param name="bomDtlInfs">中间表明细实体数据</param>
        /// <returns></returns>
        private List<ProductBomData> GenerateProductBomPara(IEnumerable<ProductBomInf> productBomInfs, Dictionary<string, List<ProductBomDetailInf>> bomDtlInfs)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();

            var paras = new List<ProductBomData>();
            var dtlCtl = RT.Service.Resolve<DownloadProductBomDtlController>();

            productBomInfs.ForEach(p =>
            {
                //构建子列表
                List<ProductBomDetailInf> details;
                if (bomDtlInfs.TryGetValue(p.Code, out details))
                    bomDtlInfs.Remove(p.Code);      //由于来源数据集允许重复数据，已取值明细清除，避免重复构建浪费资源
                else
                    details = new List<ProductBomDetailInf>();
                ctl.GenerateChildren(p, details);

                var bomDetailDatas = dtlCtl.GenerateProductBomDtlPara(details);

                //构建主数据
                var data = new ProductBomData();
                data.LastUpdateDate = p.LastUpdateDate.HasValue ? p.LastUpdateDate.Value : DateTime.Now;
                data.IsDelete = p.IsDelete;
                data.Infkey = p.Id;
                data.Code = p.Code;
                data.Name = p.Name;
                data.ItemCode = p.ProductCode;
                data.Version = p.Version;
                data.ErpKey = p.ErpKey;
                data.DetailData = bomDetailDatas;               //附加子列表

                paras.Add(data);
            });

            return paras;
        }

        /// <summary>
        /// ERP保存产品BOM数据
        /// </summary>
        /// <param name="datas">通信数据类</param>
        /// <returns>错误信息</returns>
        public virtual List<ErpErrorData> SaveProductBoms(List<ProductBomData> datas)
        {
            var errors = new List<ErpErrorData>();
            if (datas.Count == 0)
                return errors;

            var ctl = RT.Service.Resolve<ProductBomController>();

            //获取BOM数据
            var boms = ctl.GetProductBoms(datas.Select(p => p.Code).Distinct().ToList());
            var dicBom = boms.ToDictionary(p => p.Code);    //<bom编码,bom>

            //获取BOM明细数据
            var bomDetails = ctl.GetProductBomDetails(boms.Select(p => p.Id).ToList(), null);
            var dicBomDetails = bomDetails.GroupBy(p => p.ProductBom.Code).ToDictionary(p => p.Key, p => p.ToList());    //<bom编码,bom明细列表>

            //物料字典数据
            var itemCodes = datas.Select(p => p.ItemCode).ToList();
            itemCodes.AddRange(datas.SelectMany(p => p.DetailData).Select(d => d.ItemCode));
            var items = RT.Service.Resolve<ItemController>().GetItems(itemCodes.Distinct().ToList());
            var dicItem = items.ToDictionary(p => p.Code);

            //按顺序处理数据
            foreach (var p in datas)
            {
                try
                {
                    SaveProductBom(p, dicBom, dicItem, dicBomDetails);
                }
                catch (Exception ex)
                {
                    errors.Add(new ErpErrorData() { ErrMsg = ex.Message, Infkey = p.Infkey });
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
        /// <param name="dicBomDetails">数据字典</param>
        private void SaveProductBom(ProductBomData data, Dictionary<string, ProductBom> dic, Dictionary<string, Item> dicItem, Dictionary<string, List<ProductBomDetail>> dicBomDetails)
        {
            var ctl = RT.Service.Resolve<DownloadBusBaseController>();
            var dtlCtl = RT.Service.Resolve<DownloadProductBomDtlController>();

            //启用事务，保存主从数据完整性
            using (var trans = DB.TransactionScope(ItemEntityDataProvider.ConnectionStringName))
            {
                ProductBom entity;
                var key = data.Code;
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
                    dic.Add(key, new ProductBom());
                entity = dic[key];
                if (data.ItemCode.IsNullOrEmpty() || !dicItem.ContainsKey(data.ItemCode))
                    throw new ValidationException("产品编码[{0}]不存在".L10nFormat(data.ItemCode));

                entity.Product = dicItem[data.ItemCode];
                entity.Code = data.Code;
                entity.Name = data.Name;
                entity.Version = data.Version;
                RF.Save(entity);

                //处理明细
                var detailDatas = data.DetailData;
                if (!dicBomDetails.ContainsKey(key))
                    dicBomDetails.Add(key, new List<ProductBomDetail>());
                var dicDetail = dicBomDetails[key].ToDictionary(p => p.Item.Code);
                foreach (var detail in detailDatas)
                {
                    dtlCtl.SaveProductBomDetail(detail, dicDetail, dicItem, entity);
                }
                trans.Complete();
            }
        }

    }
}
