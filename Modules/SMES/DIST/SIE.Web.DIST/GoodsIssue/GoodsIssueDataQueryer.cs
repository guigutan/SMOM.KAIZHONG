using SIE.Common;
using SIE.DIST;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Items;
using SIE.Web.Data;
using SIE.Web.Items.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 配送管理查询
    /// </summary>
    public class GoodsIssueDataQueryer : DataQueryer
    {
        /// <summary>
        /// 逻辑处理
        /// </summary>
        readonly GoodsIssueViewModelContrller ctl = new GoodsIssueViewModelContrller();

        /// <summary>
        /// 获取物料属性定义
        /// </summary>
        /// <param name="itemId">物料Id</param>
        /// <param name="pageIndex">分页</param>
        /// <param name="pageSize">大小</param>
        /// <param name="keyword">k</param>
        /// <returns>物料属性定义</returns>
        public List<ItemPropertyDefinition> GetDefinition(double? itemId, int pageIndex, int pageSize, string keyword)
        {
            if (itemId > 0)
            {
                var pagingInfo = new PagingInfo(pageIndex, pageSize, true);
                var result = RT.Service.Resolve<ItemController>().GetItemPropertys(itemId.Value, keyword, pagingInfo)
                    .Select(p => p.Definition).Distinct((x, y) => x.Name == y.Name).ToList();
                return result;
            }

            return new System.Collections.Generic.List<ItemPropertyDefinition>();
        }

        /// <summary>
        /// 获取扫描明细（配送单信息）
        /// </summary>
        /// <param name="goodsId">发料单Id</param>
        /// <returns>配送单信息</returns>
        public EntityList<DistributionBill> GetDistributionBill(double goodsId)
        {
            var list = RT.Service.Resolve<DistributionController>().GetDistributionBillList(goodsId);
            list.SetTotalCount(list.Count);
            return list;
        }

        /// <summary>
        /// 获取发料单属性
        /// </summary>
        /// <param name="goodsId">发料单Id</param>
        /// <returns>发料单属性</returns>
        public EntityList<GoodsIssuePropertyValue> GetGoodIssueProList(double goodsId)
        {
            var goodItem = RF.GetById<GoodsIssue>(goodsId);
            return goodItem?.PropertyValueList;
        }

        /// <summary>
        /// 条码扫描
        /// </summary>
        /// <param name="model">当前实体</param>
        /// <param name="itemLabelList">箱号标签</param>
        /// <returns>数据实体</returns>
        public GoodIssueData BarcodeChange(GoodsIssueViewModel model, List<string> itemLabelList)
        {
            model.ItemLabelList = new List<string>();
            model.ItemLabelList.AddRange(itemLabelList);

            ctl.OnBarcodeChanged(model);
            GoodIssueData dataModel = new GoodIssueData();
            dataModel.GoodsModel = model;
            dataModel.ItemLabelList = new List<string>();
            dataModel.ItemLabelList.AddRange(model.ItemLabelList);
            return dataModel;
        }

        /// <summary>
        /// 获取工单属性值
        /// </summary>
        /// <param name="goodsIssueId">配送管理Id</param>
        /// <returns>属性值</returns>
        public EntityList<PropertyValueViewModel> GetPropertyValueViewModel(double goodsIssueId)
        {
            var goodsIssue = RF.GetById<GoodsIssue>(goodsIssueId, new EagerLoadOptions().LoadWith(GoodsIssue.PropertyValueListProperty));
            if (goodsIssue == null)
                return new EntityList<PropertyValueViewModel>();
            var result = goodsIssue.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel
            {
                DefinitionId = f.Key,
                DefinitionName = RF.GetById<ItemPropertyDefinition>(f.Key)?.Name,
                Values = f.Select(p => p.Value).ToList(),
                Type = f.Select(p => p.GoodsIssue).FirstOrDefault().GetType(),
                ParentId = f.Select(p => p.GoodsIssueId).FirstOrDefault()
            });
            var list = new EntityList<PropertyValueViewModel>();
            list.AddRange(result);
            foreach (var value in list)
            {
                value.ItemId = goodsIssue.ItemId;
                var vas = string.Empty;
                value.Values.ForEach(p =>
                {
                    vas += p + ",";
                });
                value.Value = vas.TrimEnd(',');
                //这里需要赋值一下，下拉框控件才能显示数据,保存的时候不需要用到这个属性值
                value.DefinitionValueId = 1;
            }

            return list;
        }

        /// <summary>
        /// 自定义保存配送管理的属性值标签
        /// </summary>
        /// <param name="proModel">属性实体</param>
        /// <param name="goodIssueId">配送管理ID</param>
        public void SaveGoodIssueProValue(List<PropertyValueViewModel> proModel, double goodIssueId)
        {
            var goodsIssue = RF.GetById<GoodsIssue>(goodIssueId);
            var goodsIssueValues = new EntityList<PropertyValueViewModel>();
            if (proModel.Any(p => p.DefinitionId == 0))
            {
                throw new ValidationException("属性值的物料属性不能为空。".L10N());
            }
            ////工单属性值
            goodsIssueValues.AddRange(proModel);
            if (goodsIssueValues.Any())
            {
                if (goodsIssue.PropertyValueList != null)
                {
                    goodsIssue.PropertyValueList.Clear(); //清空工单属性值列表                 
                }
                foreach (var goodIssueValue in goodsIssueValues)  //工单属性值
                {
                    foreach (var value in goodIssueValue.Values)
                    {
                        GoodsIssuePropertyValue item = new GoodsIssuePropertyValue()
                        {
                            Definition = goodIssueValue.Definition,
                            Value = value,
                            GoodsIssueId = goodsIssue.Id
                        };
                        item.GenerateId();
                        goodsIssue.PropertyValueList.Add(item);
                    }
                }
                RF.Save(goodsIssue);
            }
        }

        /// <summary>
        /// 获取配送单，bs需要做视图属性加载
        /// </summary>
        /// <param name="goodsIssueId">配送单ID</param>
        /// <returns>配送单</returns>
        public GoodsIssue GetGoodsIssue(double goodsIssueId)
        {
            return RT.Service.Resolve<DistributionController>().GetGoodsIssue(goodsIssueId);
        }

        /// <summary>
        /// 获取配送管理信息
        /// </summary>
        /// <param name="goodsIssueId">配送管理Id</param>
        /// <returns>配送管理信息</returns>
        public DistributionInfo GetDistributionInfo(double goodsIssueId)
        {
            var distributionInfo = new DistributionInfo();
            var distCt = RT.Service.Resolve<DistributionController>();
            var bills = distCt.GetDistributionBillList(goodsIssueId);
            var goodsIssue = distCt.GetGoodsIssueWithLoadData(goodsIssueId);
            var propertyValues = goodsIssue.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Definition = f.Select(p => p.Definition).FirstOrDefault(), Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.GoodsIssue).FirstOrDefault().GetType(), ParentId = f.Select(p => p.GoodsIssueId).FirstOrDefault() });

            distributionInfo.PropertyValueVMs.AddRange(propertyValues);
            distributionInfo.BillList.AddRange(bills);
            return distributionInfo;
        }
    }

    /// <summary>
    /// 载具关联数据通讯
    /// </summary>
    public class GoodIssueData
    {
        /// <summary>
        /// 载具关联物料viewModel
        /// </summary>
        public GoodsIssueViewModel GoodsModel
        {
            get; set;
        }

        /// <summary>
        /// 箱号标签
        /// </summary>
        public List<string> ItemLabelList
        {
            get; set;
        }
    }

    /// <summary>
    /// 配送管理信息
    /// </summary>
    public class DistributionInfo
    {
        /// <summary>
        /// 配送单列表
        /// </summary>
        public EntityList<DistributionBill> BillList { get; set; } = new EntityList<DistributionBill>();

        /// <summary>
        /// 属性值列表
        /// </summary>
        public EntityList<PropertyValueViewModel> PropertyValueVMs { get; set; } = new EntityList<PropertyValueViewModel>();
    }
}
