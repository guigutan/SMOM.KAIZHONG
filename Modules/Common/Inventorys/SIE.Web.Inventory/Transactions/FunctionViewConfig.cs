using SIE.Domain;
using SIE.Inventory.Transactions;
using SIE.MetaModel.View;
using SIE.Web.Inventory.Transactions.Commands;
using System;
using System.Linq;

namespace SIE.Web.Inventory.Transactions
{
    /// <summary>
    /// 功能视图配置
    /// </summary>
    public class FunctionViewConfig : WebViewConfig<Function>
    {
        public const string InitView = "InitView";
        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            if (ViewGroup == InitView)
            {
                ConfigInitView();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(InitFunctionCommand).FullName);
            View.UseCommands(WebCommandNames.Save);
            View.UseCommands(WebCommandNames.ExportXls, WebCommandNames.ExportXlsSelection, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.FunctionType).Readonly();
                View.Property(p => p.Code).Readonly().ShowInList(150);
                View.Property(p => p.Name).Readonly();
                View.Property(p => p.State).Readonly();
                View.Property(p => p.NumberRuleId).ShowInList(120);
                View.Property(p => p.BillTemplateId).ShowInList(120);
                View.Property(p => p.ConfigCollectType).ShowInList(200).Readonly(p=>p.FunctionType != FunctionType.Receipt).UseListSetting(p => p.HelpInfo = "当不启用送货明细且收货实到数<预约数拆分收货明细的方式".L10N());
                View.Property(p => p.IsQc).Readonly(p => p.FunctionType != FunctionType.Receipt).ShowInList(120);
                View.Property(p => p.IsCollectByDelivery).Readonly(p => p.FunctionType != FunctionType.Receipt).ShowInList(120);
                View.Property(p => p.IsCrossPick).Readonly(p => p.FunctionType != FunctionType.Receipt);
                View.Property(p => p.IsReceiveNg).Readonly(p => p.FunctionType != FunctionType.Receipt);
                View.Property(p => p.IsAllowOut).Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.IsOqc).Readonly(p => p.FunctionType != FunctionType.Shipment).ShowInList(120);
                View.Property(p => p.IsAutoDelivery).Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.IsPickByPackage).Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.IsPartDelivery).Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.OutUpLimitMultiple).ShowInList().UseSpinEditor(p => p.MinValue = 0);
                View.Property(p => p.MaxOutUpLimit).ShowInList().UseSpinEditor(p => p.MinValue = 0);
                View.Property(p => p.IsDirectAllocate).ShowInList().Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.IsTwoAllocate).ShowInList().Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.IsAcrossAllocate).ShowInList(120).Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.AllocateSn).ShowInList(150).UseListSetting(p => p.HelpInfo = "两步调拨、跨组织调拨发货成功后，除了创建调拨入库单，还把序列号也导入到调拨入库单的序列号页签".L10N()).Readonly(p => p.FunctionType != FunctionType.Shipment);
                View.Property(p => p.SourceType).Readonly();
                View.Property(p => p.Description).Readonly();
            }

            View.AttachChildrenProperty(typeof(ErpFunctionFunction), o =>
            {
                var function = o.Parent as Function;
                var resultList = new EntityList<ErpFunctionFunction>();
                if (function != null)
                {
                    var erpFunctionFunctionList = (RF.Find<ErpFunctionFunction>().GetAll() as EntityList<ErpFunctionFunction>).Where(p => p.FunctionId == function.Id);
                    if (erpFunctionFunctionList != null)
                        resultList.AddRange(erpFunctionFunctionList);
                }

                return resultList;
            }).HasLabel("对应功能").Visible(false);

            View.AttachChildrenProperty(typeof(FunctionTransaction), o =>
        {
            var args = o as ChildPagingDataArgs;
            var function = o.Parent as Function;
            if (function != null)
            {
                return RT.Service.Resolve<TransactionController>().GetFunctionTransactions(function.Id, args.PagingInfo, args.SortInfo);
            }

            return null;
        }).HasLabel("对应小类");
            View.ChildrenProperty(p => p.EmployeeList).OrderNo = 3;
        }

        /// <summary>
        /// 配置选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.SourceType);
            View.Property(p => p.State);
        }

        /// <summary>
        /// 配置查询视图
        /// </summary>
        protected override void ConfigQueryView()
        {
            base.ConfigQueryView();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.SourceType);
            View.Property(p => p.State);
            View.Property(p => p.FunctionType);
        }

        /// <summary>
        /// 初始化视图
        /// </summary>
        protected void ConfigInitView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
        }
    }
}
