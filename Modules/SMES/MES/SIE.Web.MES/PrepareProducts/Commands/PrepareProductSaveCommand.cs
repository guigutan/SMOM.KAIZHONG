using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES;
using SIE.MES.PrepareProducts;
using SIE.MES.PrepareProducts.Services;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.Web.MES.PrepareProducts.Commands
{
    /// <summary>
    /// 产品产前准备主子表保存命令
    /// </summary>
    public class PrepareProductSaveCommand : SaveCommand
    {
        /// <summary>
        /// 保存前事件
        /// </summary>
        /// <param name="data"></param>
        protected override void OnSaving(EntityList data)
        {
            RT.Service.Resolve<PrepareProductService>().PrepareProductSave(data);
            base.OnSaving(data);
        }
        ///// <summary>
        ///// 执行
        ///// </summary>
        ///// <param name="args"></param>
        ///// <param name="scope"></param>
        ///// <returns></returns>
        ///// <exception cref="NotImplementedException"></exception>
        //protected override object Excute(ViewArgs args, string scope)
        //{
        //    var data = args.Data.ToJsonObject<PrepareProductWithChild>();
        //    if (!data.PrepareProductList.Any() && !data.PrepareProductDetailList.Any())
        //    {
        //        throw new ValidationException("无提交的数据！");
        //    }
        //    if (data.PrepareProductList.Any(p => (p.ProductId == null || p.ProductId == 0) && (p.ProductFamilyId == null || p.ProductFamilyId == 0)))
        //    {
        //        throw new ValidationException("产品和产品族必须维护一个！");
        //    }
        //    if (data.PrepareProductDetailList.Any(p => (p.ProcessId == null || p.ProcessId == 0)))
        //    {
        //        throw new ValidationException("工序不能为空！");
        //    }
        //    var preProductIds = data.PrepareProductList.Select(p => p.Id).ToList();
        //    var preProductDetailIds = data.PrepareProductDetailList.Select(p => p.Id).ToList();
        //    // 数据库中的主表
        //    var preProductDBList = RT.Service.Resolve<PrepareProductService>().GetPrepareProductListByIds(preProductIds);
        //    // 数据库中的子表
        //    var preProductDetailDBList = RT.Service.Resolve<PrepareProductService>().GetPrepareProductDetailListByIds(preProductDetailIds);

        //    var productIds = data.PrepareProductList.Where(p => p.ProductId != null).Select(p => p.ProductId).ToList();
        //    var productFamilyIds = data.PrepareProductList.Where(p => p.ProductFamilyId != null).Select(p => p.ProductFamilyId).ToList();
        //    List<double?> repeatList = new List<double?>();
        //    if (productIds.Any())
        //    {
        //        var dataBase = RT.Service.Resolve<PrepareProductService>().GetPrepareProductListByProductIds(productIds);
        //        repeatList.AddRange(productIds);
        //        repeatList.AddRange(dataBase.Select(p => p.ProductId).ToList());
        //        if (repeatList.Count != repeatList.Distinct().Count())
        //        {
        //            throw new ValidationException("产品重复！");
        //        }
        //    }
        //    if (productFamilyIds.Any())
        //    {
        //        var dataBase = RT.Service.Resolve<PrepareProductService>().GetPrepareProductListByFamilyIds(productFamilyIds);
        //        repeatList = new List<double?>();
        //        repeatList.AddRange(productFamilyIds);
        //        repeatList.AddRange(dataBase.Select(p => p.ProductFamilyId).ToList());
        //        if (repeatList.Count != repeatList.Distinct().Count())
        //        {
        //            throw new ValidationException("产品族重复！");
        //        }
        //    }
        //    if (data.PrepareProductDetailList.Any())
        //    {
        //        EntityList<PrepareProductDetail> repeatDetailList = new EntityList<PrepareProductDetail>();
        //        repeatDetailList.AddRange(data.PrepareProductDetailList);
        //        repeatDetailList.AddRange(RT.Service.Resolve<PrepareProductService>().GetPrepareProductDetailList(preProductIds));
        //        var groupByData = repeatDetailList.GroupBy(p => new { p.ProcessId, p.PrepareProjectId}).ToList();
        //        if (groupByData.Exists(p => p.Count() > 1))
        //        {
        //            throw new ValidationException("工序+项目唯一！");
        //        }
        //    }

        //    using(var tran = DB.TransactionScope(MesCoreEntityDataProvider.ConnectionStringName))
        //    {
        //        data.PrepareProductList.ForEach(pre =>
        //        {
        //            var parent = preProductDBList.FirstOrDefault(p => p.Id == pre.Id);
        //            pre.PersistenceStatus = parent != null ? PersistenceStatus.Modified : PersistenceStatus.New;
        //        });
        //        RF.Save(data.PrepareProductList);
        //        RF.Save(data.PrepareProductDetailList);
        //        tran.Complete();
        //    }
        //    return true;
        //}

        ///// <summary>
        ///// 主子表
        ///// </summary>
        //[Serializable]
        //public class PrepareProductWithChild
        //{
        //    /// <summary>
        //    /// 主表
        //    /// </summary>
        //    public EntityList<PrepareProduct> PrepareProductList { get; set; }

        //    /// <summary>
        //    /// 子表
        //    /// </summary>
        //    public EntityList<PrepareProductDetail> PrepareProductDetailList { get; set;}
        //}
    }
}
