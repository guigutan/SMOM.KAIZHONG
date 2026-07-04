using SIE.Domain;
using SIE.EMS.Purchases.PurchaseOrders;
using SIE.MetaModel.View;
using SIE.Web.Common;
using SIE.Web.Common.Sort.Commands;
using SIE.Web.EMS.Purchases.PurchaseOrders.Commands;
using System;

namespace SIE.Web.EMS.Purchases.PurchaseOrders
{
    /// <summary>
    /// 付款条件视图配置
    /// </summary>
    public class PaymentTermsViewConfig : WebViewConfig<PaymentTerms>
    {
        /// <summary>
        /// 编辑视图
        /// </summary>
        public const string EditView = "EditView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EditView);
            if (ViewGroup == EditView)
            {
                ConfigEditView();
            }
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.Purchases.PurchaseOrders.PaymentTermsBehavior");
            View.ClearCommands();
            View.DisableEditing();
            View.Property(p => p.Phase).UseCatalogEditor(e => { e.CatalogType = PaymentTerms.PhaseCatalog; e.CatalogReloadData = true; }).ShowInList(120);
            View.Property(p => p.Percent).ShowInList(100);
            View.Property(p => p.Amount).ShowInList(130);
            View.Property(p => p.CumulativeAmount).ShowInList(130);
            View.Property(p => p.CumulativePercent).ShowInList(100);
            View.Property(p => p.Currency).ShowInList(60);
            View.Property(p => p.AmountUnit).ShowInList(80);
            View.Property(p => p.State).ShowInList(60);
            View.Property(p => p.Condition).ShowInList(80);
            View.Property(p => p.PaymentMethod).UseCatalogEditor(e => { e.CatalogType = PaymentTerms.PaymentMethodCatalog; e.CatalogReloadData = true; }).ShowInList(100);
            View.Property(p => p.PaymentDate).UseDateEditor().ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Phase).UseCatalogEditor(e => { e.CatalogType = PaymentTerms.PhaseCatalog; e.CatalogReloadData = true; }).ShowInList(120);
            View.Property(p => p.Percent).ShowInList(100);
            View.Property(p => p.Amount).ShowInList(130);
        }

        /// <summary>
        /// 编辑视图
        /// </summary>
        protected void ConfigEditView()
        {
            View.AssignAuthorize(typeof(PurchaseOrder));
            View.AddBehavior("SIE.Web.EMS.Purchases.PurchaseOrders.PaymentTermsBehavior");
            View.UseCommands("SIE.Web.EMS.Purchases.PurchaseOrders.Commands.AddPaymentTermsCommand", WebCommandNames.Delete);
            View.ReplaceCommands(typeof(MoveUpCommand).FullName, typeof(PaymentMoveUpCommand).FullName);
            View.ReplaceCommands(typeof(MoveTopCommand).FullName, typeof(PaymentMoveTopCommand).FullName);
            View.ReplaceCommands(typeof(MoveDownCommand).FullName, typeof(PaymentMoveDownCommand).FullName);
            View.ReplaceCommands(typeof(MoveBottomCommand).FullName, typeof(PaymentMoveBottomCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.Phase).UseCatalogEditor(e => { e.CatalogType = PaymentTerms.PhaseCatalog; e.CatalogReloadData = true; }).ShowInList(120);
                View.Property(p => p.Percent).UseSpinEditor(p =>
                {
                    p.MinValue = 0.01;
                    p.MaxValue = 100;
                    p.DecimalPrecision = 2;
                }).HasLabel("付款比例(%)".L10N()+"*").ShowInList(100);
                View.Property(p => p.Amount).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.DecimalPrecision = 2;
                }).ShowInList(130);
                View.Property(p => p.CumulativeAmount).UseSpinEditor(p => { p.DecimalPrecision = 2; }).ShowInList(130).Readonly();
                View.Property(p => p.CumulativePercent).UseSpinEditor(p => { p.DecimalPrecision = 2; }).ShowInList(100).Readonly();
                View.Property(p => p.Currency).Readonly().ShowInList(60);
                View.Property(p => p.AmountUnit).HasLabel("单位").ShowInList(80).Readonly();
                View.Property(p => p.State).ShowInList(60).Readonly();
                View.Property(p => p.Condition).ShowInList(80);
                View.Property(p => p.PaymentMethod).UseCatalogEditor(e => { e.CatalogType = PaymentTerms.PaymentMethodCatalog; e.CatalogReloadData = true; }).ShowInList(100);
                View.Property(p => p.PaymentDate).UseDateEditor().ShowInList(150);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}