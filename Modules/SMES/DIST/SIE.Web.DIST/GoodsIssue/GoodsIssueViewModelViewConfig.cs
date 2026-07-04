using SIE.DIST;
using SIE.Domain;
using SIE.Web.Items.ViewModels;
using System;
using System.Linq;

namespace SIE.Web.DIST
{
    /// <summary>
    /// 载具关联视图模型视图配置
    /// </summary>
    class GoodsIssueViewModelViewConfig : WebViewConfig<GoodsIssueViewModel>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AddBehavior("SIE.Web.DIST.GoodsIssueViewModelBahavor");
            View.AssignAuthorize(typeof(GoodsIssue));
            View.ClearCommands();
            View.UseCommands("SIE.Web.DIST.SubmitCommand", "SIE.Web.DIST.RestartCommand");
            using (View.OrderProperties())
            {
                using (View.DeclareGroup(string.Empty, 2))
                {
                    View.Property(p => p.TipsDisplay).UseDisplayEditor(p => p.XType = "GoodsIssueTipsEditor").ShowInDetail(columnSpan: 2).Readonly();
                }

                using (View.DeclareGroup("请扫描产品条码", 4))
                {
                    View.Property(p => p.BarcodeDisplay).UseDisplayEditor(p => p.XType = "GoodsIssueSnEditor").ShowInDetail(columnSpan: 2);
                }

                using (View.DeclareGroup("工单信息", 4, true))
                {
                    View.Property(p => p.WorkOrderNo).HasLabel("工单号").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.ItemCode).HasLabel("物料编码").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.ItemModel).HasLabel("物料规格").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.GoodsQty).HasLabel("仓库发货数").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.RemainQty).HasLabel("剩余数量").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.DistributionQty).HasLabel("累计配送数").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.DefectQty).HasLabel("缺陷数量").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.UnitName).HasLabel("单位").Show(ShowInWhere.All).Readonly();
                    View.Property(p => p.Qty).UseSpinEditor(e => e.MinValue = 0).ShowInDetail().Readonly(GoodsIssueViewModel.QtyReadOnlyProperty)
                        .UseListSetting(e => { e.HelpInfo = "配送数量只读不可编辑"; });
                    View.Property(p => p.Resource).UsePagingLookUpEditor().Show(ShowInWhere.All);
                }

                View.AttachChildrenProperty(typeof(DistributionBill), (o) =>
                {
                    var args = o as ChildPagingDataArgs;
                    var goodsIssueVM = args.Parent.CastTo<GoodsIssueViewModel>();
                    if (goodsIssueVM == null) return new EntityList<DistributionBill>();
                    double goodsIssueId = Convert.ToDouble(goodsIssueVM.Id);
                    return RT.Service.Resolve<DistributionController>().GetDistributionBillList(goodsIssueId);
                }, DistributionBillViewConfig.Distribution).Show(ChildShowInWhere.All).HasLabel("扫描明细").OrderNo = -1;

                View.AttachChildrenProperty(typeof(PropertyValueViewModel), (o) =>
                {
                    var propertyValueVMList = new EntityList<PropertyValueViewModel>();
                    var args = o as ChildPagingDataArgs;
                    var goodsIssueVM = args.Parent.CastTo<GoodsIssueViewModel>();
                    if (goodsIssueVM == null) return new EntityList<PropertyValueViewModel>();
                    double goodsIssueId = Convert.ToDouble(goodsIssueVM.Id);
                    var goodsIssue = RT.Service.Resolve<DistributionController>().GetGoodsIssueWithLoadData(goodsIssueId);
                    var propertyValues = goodsIssue.PropertyValueList.GroupBy(p => p.DefinitionId).Select(f => new PropertyValueViewModel { DefinitionId = f.Key, Definition = f.Select(p => p.Definition).FirstOrDefault(), Values = f.Select(p => p.Value).ToList(), Type = f.Select(p => p.GoodsIssue).FirstOrDefault().GetType(), ParentId = f.Select(p => p.GoodsIssueId).FirstOrDefault() });
                    propertyValueVMList.AddRange(propertyValues);
                    return propertyValueVMList;
                }, GoodsIssuePropertyValueExtViewConfig.DistributionView)
               .Show(ChildShowInWhere.All).HasLabel("发料属性").OrderNo = -1;
            }
        }
    }
}
