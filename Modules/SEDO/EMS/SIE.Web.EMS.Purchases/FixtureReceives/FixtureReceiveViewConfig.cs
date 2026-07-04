using SIE.Domain;
using SIE.EMS.Purchases.FixtureReceives;
using SIE.MetaModel.View;
using SIE.Web.EMS.Extensions;
using SIE.Web.EMS.Purchases.FixtureReceives.Commands;
using SIE.Web.Resources;
using System;

namespace SIE.Web.EMS.Purchases.FixtureReceives
{
    /// <summary>
    /// 工治具接收视图配置
    /// </summary>
    internal class FixtureReceiveViewConfig : WebViewConfig<FixtureReceive>
    {

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultCommands();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.DisableEditing();
            View.FormEdit();
            View.RemoveCommands(WebCommandNames.Save,WebCommandNames.Copy);
            View.ReplaceCommands(WebCommandNames.Add, "SIE.Web.EMS.Purchases.FixtureReceives.Commands.AddFixtureReceiveCommand");
            View.ReplaceCommands(WebCommandNames.Edit, "SIE.Web.EMS.Purchases.FixtureReceives.Commands.EditFixtureReceiveCommand");
            View.ReplaceCommands(WebCommandNames.Delete, "SIE.Web.EMS.Purchases.FixtureReceives.Commands.DeleteFixtureReceiveCommand");
            View.UseCommand("SIE.Web.EMS.Purchases.FixtureReceives.Commands.ReceiveScanCommand");
            View.UseCommand(typeof(SubmitFixtureReceiveCommand).FullName);

            View.Property(p => p.FactoryId).ShowInList(120);
            View.Property(p => p.DepartmentId).ShowInList(120);
            View.Property(p => p.ReceiveNo).ShowInList(160);
            View.Property(p => p.ReceiveBillStatus).ShowInList(60).HasLabel("状态");
            View.Property(p => p.ReceiveType).ShowInList(80);
            View.Property(p => p.VarietyQuantity).ShowInList(60);
            View.Property(p => p.TotalQty).ShowInList(60);
            View.Property(p => p.ReceiverId);
            View.Property(p => p.ReceiveDateTime).ShowInList(150);
            View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            View.ChildrenProperty(p => p.FixtureReceiveDetailList).HasLabel("工治具明细").HasOrderNo(1);
            View.AttachChildrenProperty(typeof(FixtureReceiveSn), w =>
            {
                var args = w as ChildPagingDataArgs;
                var parent = args.Parent as FixtureReceive;
                if (parent == null)
                {
                    return new EntityList<FixtureReceiveSn>();
                }
                return RT.Service.Resolve<FixtureReceiveController>().GetReceiveSnInfo(parent.Id, args.SortInfo, args.PagingInfo);
            }).HasLabel("序列号明细").HasOrderNo(2);
        }

        ///<summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AddBehavior("SIE.Web.EMS.Purchases.FixtureReceives.FixturesReceiveBehavior");
            View.ClearCommands();
            View.UseCommand(typeof(SaveFixtureReceiveCommand).FullName);
            View.UseDetail(4);
            View.Property(p => p.ReceiveNo).Readonly();
            View.Property(p => p.FactoryId).UseFactoryEditor().Cascade(p => p.DepartmentId, null);
            View.Property(p => p.DepartmentId).UseUserBussinessDepartmentEditor();
            View.Property(p => p.ReceiveType);
            View.Property(p => p.VarietyQuantity).Readonly();
            View.Property(p => p.TotalQty).Readonly();
            View.ChildrenProperty(p => p.FixtureReceiveDetailList).UseViewGroup(FixtureReceiveDetailViewConfig.EditView).HasLabel("接收明细").HasOrderNo(1);

        }
    }
}