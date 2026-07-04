using SIE.Andon.Andons;
using SIE.Defects;
using SIE.Domain;
using SIE.Equipments.EquipAccounts;
using SIE.MES.WorkOrders;
using SIE.Wpf.Andon.Commands;

namespace SIE.Wpf.Andon
{
    internal class AndonManageViewConfig : WPFViewConfig<AndonManage>
    {
        /// <summary>
        /// 事件报告
        /// </summary>
        public const string EventReportViewGroup = "EventReportViewGroup";

        /// <summary>
        /// 验收视图
        /// </summary>
        public const string AcceptViewGroup = "AcceptViewGroup";

        /// <summary>
        /// 安灯触发视图
        /// </summary>
        public const string AndonTriggerViewGroup = "AndonTriggerViewGroup";

        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(new string[]
            {
                EventReportViewGroup, AcceptViewGroup, AndonTriggerViewGroup
            });

            if (ViewGroup == EventReportViewGroup)
            {
                ConfigEventReportViewGroup();
            }
            else if (ViewGroup == AcceptViewGroup)
            {
                ConfigAcceptViewGroup();
            }
            else if (ViewGroup == AndonTriggerViewGroup)
            {
                ConfigAndonTriggerViewGroup();
            }
        }

        /// <summary>
        /// 验收视图配置
        /// </summary>        
        private void ConfigAcceptViewGroup()
        {
            View.ClearCommands();
            View.UseDetail(columnCount: 1);

            //弹窗填写实际影响时间：大于0，保留1位小数，默认为【当前时间减去触发时间】
            View.Property(p => p.ActualTime).UseSpinEditor(c =>
            {
                c.Decimals = 1;
                c.MinValue = 0;
            }).ShowInDetail();
        }

        /// <summary>
        /// 事件报告视图配置
        /// </summary>        
        private void ConfigEventReportViewGroup()
        {
            View.AssignAuthorize(typeof(AndonManageViewModel));
            View.ClearCommands();
            View.UseDetail(columnCount: 1);
            View.Property(p => p.Reason).Readonly();
            View.Property(p => p.HandleMethod).Readonly();
            View.Property(p => p.Measures).Readonly();
        }

        /// <summary>
        /// 表单视图配置
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(AndonManageViewModel));
            View.ClearCommands();
            View.UseCommands(typeof(AndonManageCanelCommand), typeof(AndonManageAcceptCommand),
                typeof(AndonManageRejectCommand), typeof(ViewAttachmentCommand),typeof(AndonManageResponseCommand),typeof(AndonManageHandleCommand));

            View.UseDetail(columnCount: 4);
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonManageCode).Readonly();
                View.Property(p => p.AndonManageClass).Readonly();
                View.Property(p => p.AndonType).Readonly();
                View.Property(p => p.AndonName).Readonly();

                View.Property(p => p.Department).Readonly();
                View.Property(p => p.State).Readonly();
                View.Property(p => p.Priority).Readonly();
                View.Property(p => p.Defect).Readonly();

                View.Property(p => p.TriggerByName).Readonly();
                View.Property(p => p.TriggerTime).Readonly().UseFormSetting(m => m.HelpInfo = "点击触发按钮的时间");
                View.Property(p => p.HandlerName).Readonly();
                View.Property(p => p.ProblemDesc).Readonly().ShowInDetail(columnSpan: 4);

                View.Property(p => p.Factory).Readonly();
                View.Property(p => p.WorkShop).Readonly();
                View.Property(p => p.WipResource).Readonly();
                View.Property(p => p.WipResourceName).Readonly();
                View.Property(p => p.Station).Readonly();

                View.Property(p => p.EquipAccount).Readonly();
                View.Property(p => p.EquipAccountName).Readonly();
                View.Property(p => p.WorkGroup).Readonly();
                View.Property(p => p.WorkOrder).Readonly();

                View.Property(p => p.Process).Readonly();
                View.Property(p => p.BarCode).Readonly();
                View.Property(p => p.LineStop).Readonly();
                View.Property(p => p.AskMaterial).Readonly();

                View.ChildrenProperty(p => p.OperateLogList).OrderNo = 1;

                View.AttachDetailChildrenProperty(typeof(AndonManage), (c) =>
                {
                    return c.Parent as AndonManage;
                }, EventReportViewGroup).HasLabel("事件报告").OrderNo = 3;

                View.ChildrenProperty(p => p.MessageSendList).OrderNo = 4;
            }
        }

        /// <summary>
        /// 安灯触发表单视图
        /// </summary>
        private void ConfigAndonTriggerViewGroup()
        {
            View.AssignAuthorize(typeof(AndonManageViewModel));
            View.ClearCommands();
            View.AddBehavior(typeof(AndonManageTriggerBehavior));
            View.UseCommands(typeof(AttachmentUploadCommand));

            View.UseDetail(columnCount: 2);

            using (View.OrderProperties())
            {

                View.Property(p => p.AndonManageCode).Readonly().ShowInDetail();
                View.Property(p => p.AndonManageClass).Readonly().ShowInDetail();

                View.Property(p => p.AndonType).Readonly().ShowInDetail();
                View.Property(p => p.Andon).Readonly().ShowInDetail();

                View.Property(p => p.Solution).Readonly().ShowInDetail();
                View.Property(p => p.ProblemDesc).ShowInDetail(columnSpan:2);

                View.Property(p => p.Defect).ShowInDetail().UseDefectMultiSelectEditor(p =>
                {
                    p.BindingField = AndonManage.DefectProperty.Name;
                    p.LinkField = AndonManage.DefectIdsProperty.Name;
                    p.DisplayField = Defect.DescriptionProperty.Name;
                    p.Model = typeof(Defect).FullName;
                    p.Separator = ",";
                });

                //View.Property(p => p.FaultTime).ShowInDetail().UseDateTimeEditor();

                View.Property(p => p.EquipAccount).UseDataSource((e, p,k) => {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetEquipAccounts(source.WipResourceId);
                    }
                    return new EntityList<EquipAccount>();
                }).ShowInDetail().UsePagingLookUpEditor();
                View.Property(p => p.EquipAccount.Name).Readonly().ShowInDetail().HasLabel("设备名称");

                View.Property(p => p.WorkOrder).UseDataSource((e, p, k) =>
                {
                    var source = e as AndonManage;
                    if (source != null)
                    {
                        return RT.Service.Resolve<AndonManageController>().GetWorkOrdersByWip(source, p, k);
                    }
                    return new EntityList<WorkOrder>();
                }).ShowInDetail();

                View.Property(p => p.BarCode).ShowInDetail();

                View.Property(p => p.LineStop)
                    .Readonly(p => p.LineStopFlag != SIE.Andon.Andons.Enum.AndonYesOrNo.Artificial)
                    .ShowInDetail();

                View.Property(p => p.AskMaterial)
                    .Readonly(p => p.AskMaterialFlag != SIE.Andon.Andons.Enum.AndonYesOrNo.Artificial)
                    .ShowInDetail();

                View.Property(p => p.PhotoFile).Readonly().ShowInDetail(columnSpan: 2);
                View.Property(p => p.GeneralProbDtl).Show().UsePagingLookUpEditor().UseDataSource((e, p, k) =>
                {
                    var entity = e as AndonManage;
                    var list = RT.Service.Resolve<AndonManageController>().GetGeneralProbDtlsByAndonId(entity.AndonId, p, k);
                    if (list.Count > 0)
                        return list;
                    return new EntityList<GeneralProbDtl>();
                });
                View.ChildrenProperty(p => p.ItemDetail).Show(ChildShowInWhere.All);
            }
        }
    }
}
