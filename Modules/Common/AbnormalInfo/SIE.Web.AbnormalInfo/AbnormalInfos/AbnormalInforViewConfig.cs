using SIE.Defects;
using SIE.AbnormalInfo.AbnormalInfos;
using SIE.MetaModel.View;
using SIE.Web.AbnormalInfo._Extentions_;
using SIE.Web.AbnormalInfo.AbnormalInfos.Commands;

namespace SIE.Web.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常信息视图配置
    /// </summary>
    public class AbnormalInforViewConfig : WebViewConfig<AbnormalInfor>
    {
        /// <summary>
        /// 确认异常视图
        /// </summary>
        public const string ConfirmView = "ConfirmView";
        /// <summary>
        /// 查看异常视图
        /// </summary>
        public const string ReadOnlyView = "ReadOnlyView";

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.DeclareExtendViewGroup(ConfirmView, ReadOnlyView);
            if (View.EntityViewMeta.ViewGroup == ConfirmView)
            {
                ConfigConfirmView();
            }
            else
            {
                ConfigReadOnlyView();
            }
        }

        /// <summary>
        /// 列表视图配置
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(AbnormalInfoCommands.ConfirmAbnormalInfoCommand, AbnormalInfoCommands.ViewAbnormalInfoCommand, WebCommandNames.ExportXls);
            using (View.OrderProperties())
            {
                View.Property(p => p.No);
                View.Property(p => p.AbnormalStatus);
                View.Property(p => p.AbnormalInfoDefinitionDesc).HasLabel("异常信息");
                View.Property(p => p.AbnormalInfoCategoryDesc);
                View.Property(p => p.InspectionNo);
                View.Property(p => p.HandlersDisplay);
                View.Property(p => p.JoinProcessNames);
                View.Property(p => p.LineId);
                View.Property(p => p.WorkShopId);
                View.Property(p => p.ItemId);
                View.Property(p => p.ItemName);
                View.Property(p => p.JoinDefectCodes);
                View.Property(p => p.JoinDefectCodeDescriptions);
                View.Property(p => p.EquipmentId);
                View.Property(p => p.EquipmentName);
                View.Property(p => p.ProjectNg);
                View.Property(p => p.ProjectDesc);
                View.Property(p => p.CreateByName).HasLabel("创建任务时间");
            }
        }

        /// <summary>
        /// 确认异常视图
        /// </summary>
        protected void ConfigConfirmView()
        {
            View.AddBehavior("SIE.Web.AbnormalInfo.AbnormalInfos.Behaviors.AbnormalInfoDetailBehavior");
            View.HasDetailColumnsCount(5);
            View.UseCommands(typeof(SaveAbnormalInfoCommand).FullName, typeof(SubmitAbnormalInfoCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalInfoDefinitionCode).HasLabel("异常编码").Readonly().ShowInDetail();
                View.Property(p => p.AbnormalInfoDefinitionDesc).HasLabel("异常描述").Readonly().ShowInDetail(columnSpan: 2);
                View.Property(p => p.JoinDefectCodes).Readonly(p => !p.IsSendPdca || p.AbnormalInfoDefinitionSource == AbnormalSource.FirstInspection
                    || p.AbnormalInfoDefinitionSource == AbnormalSource.PatrolInspBill)
                    .ShowInDetail().UseDefectLookupEditor(p =>
                   {
                       p.BindDisplayField = Defect.CodeProperty.Name;
                       p.ValueField = Defect.CodeProperty.Name;
                       p.Editable = false;
                       p.DicLinkField = new System.Collections.Generic.Dictionary<string, string>()
                       {
                            { nameof(AbnormalInfor.JoinDefectCodeDescriptions),nameof(Defect.Description)},
                            { nameof(AbnormalInfor.DefectIds),nameof(Defect.Id) }
                       };
                   }).UseListSetting(e => { e.HelpInfo = "来源来首检过程整改或者抽检过程整改，或者不发送PDCA时，不可编辑"; });
                View.Property(p => p.JoinDefectCodeDescriptions).Readonly().ShowInDetail();
                View.Property(p => p.EquipmentId).Readonly().ShowInDetail();
                View.Property(p => p.EquipmentName).Readonly().ShowInDetail();
                View.Property(p => p.JoinProcessNames).Readonly().ShowInDetail();
                View.Property(p => p.LineId).Readonly().ShowInDetail();
                View.Property(p => p.WorkShopId).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalInfoCategoryDesc).Readonly().ShowInDetail();
                View.Property(p => p.ItemId).Readonly().ShowInDetail();
                View.Property(p => p.ItemName).Readonly().ShowInDetail();
                View.Property(p => p.IsStop).Readonly().ShowInDetail();
                View.Property(p => p.IsSendPdca).Readonly(p => p.IsRectificationTask).ShowInDetail(columnSpan: 5);

                View.Property(p => p.ReasonAnalysis).UseMemoEditor().HasLabel("原因分析(必填)").ShowInDetail(columnSpan: 5);
                View.Property(p => p.Measure).UseMemoEditor().HasLabel("改善对策(必填)").ShowInDetail(columnSpan: 5);
                View.Property(p => p.Experience).UseMemoEditor().HasLabel("经验总结(选填)").ShowInDetail(columnSpan: 5);
            }
        }

        /// <summary>
        /// 查看异常视图
        /// </summary>
        protected void ConfigReadOnlyView()
        {
            View.AddBehavior("SIE.Web.AbnormalInfo.AbnormalInfos.Behaviors.AbnormalInfoDetailBehavior");
            View.HasDetailColumnsCount(5);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalInfoDefinitionCode).HasLabel("异常编码").Readonly().ShowInDetail();
                View.Property(p => p.AbnormalInfoDefinitionDesc).HasLabel("异常描述").Readonly().ShowInDetail(columnSpan: 2);
                View.Property(p => p.JoinDefectCodes).Readonly().ShowInDetail();
                View.Property(p => p.JoinDefectCodeDescriptions).Readonly().ShowInDetail();
                View.Property(p => p.EquipmentId).Readonly().ShowInDetail();
                View.Property(p => p.EquipmentName).Readonly().ShowInDetail();
                View.Property(p => p.JoinProcessNames).Readonly().ShowInDetail();
                View.Property(p => p.LineId).Readonly().ShowInDetail();
                View.Property(p => p.WorkShopId).Readonly().ShowInDetail();
                View.Property(p => p.AbnormalInfoCategoryDesc).Readonly().ShowInDetail();
                View.Property(p => p.ItemId).Readonly().ShowInDetail();
                View.Property(p => p.ItemName).Readonly().ShowInDetail();
                View.Property(p => p.IsStop).Readonly().ShowInDetail();
                View.Property(p => p.IsSendPdca).Readonly().ShowInDetail(columnSpan: 5);

                View.Property(p => p.ReasonAnalysis).UseMemoEditor().HasLabel("原因分析(必填)").Readonly().ShowInDetail(columnSpan: 5);
                View.Property(p => p.Measure).UseMemoEditor().HasLabel("改善对策(必填)").Readonly().ShowInDetail(columnSpan: 5);
                View.Property(p => p.Experience).UseMemoEditor().HasLabel("经验总结(选填)").Readonly().ShowInDetail(columnSpan: 5);
            }
        }
    }
}
