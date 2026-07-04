using SIE.Andon.Andons;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Web.Andon.Andons.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Andon.Andons
{
    /// <summary>
    /// 安灯经验库视图配置
    /// </summary>
    public class AndonExperienceViewConfig : WebViewConfig<AndonExperience>
    {
        /// <summary>
        /// 事件报告
        /// </summary>
        public const string EventViewGroup = "EventViewGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(EventViewGroup);
            if (ViewGroup == EventViewGroup)
            {
                EventView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(AndonExpRemoveCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.AndonManageCode);
                View.Property(p => p.AndonManageClass);
                View.Property(p => p.AndonType);
                View.Property(p => p.Andon);
                View.Property(p => p.ProblemDesc);
                View.Property(p => p.Priority);
                View.Property(p => p.Defect);
                View.Property(p => p.Department);
                View.Property(p => p.State);
                View.Property(p => p.FaultTime);
                View.Property(p => p.Trigger);
                View.Property(p => p.TriggerTime);
                View.Property(p => p.Handler);
                View.Property(p => p.CloseTime);
                View.Property(p => p.LastTime);
                View.Property(p => p.ActualTime);
                View.Property(p => p.Factory);
                View.Property(p => p.WorkShop);
                View.Property(p => p.WipResource);
                View.Property(p => p.Station);
                View.Property(p => p.EquipAccount);
                View.Property(p => p.EquipAccountName);
                View.Property(p => p.WorkGroup);
                View.Property(p => p.WorkOrder);
                View.Property(p => p.ProductCode);
                View.Property(p => p.ProductName);
                View.Property(p => p.Process);
                View.Property(p => p.BarCode);
                View.Property(p => p.LineStop).Readonly();
                View.Property(p => p.AskMaterial).Readonly();
                View.AttachDetailChildrenProperty(typeof(AndonExperience), (c) =>
                {
                    var andonExperience = c.Parent as AndonExperience;
                    andonExperience = RF.GetById<AndonExperience>(andonExperience.Id, new EagerLoadOptions().LoadWithViewProperty());
                    return andonExperience;
                }, EventViewGroup).HasLabel("事件报告").Show(ChildShowInWhere.List);
            }
        }
        /// <summary>
        /// 事件报告视图配置
        /// </summary>
        protected void EventView()
        {
            View.DisableEditing();
            View.UseCommands(typeof(AndonManageAttachmentCommand).FullName);
            View.HasDetailColumnsCount(2);
            using (View.OrderProperties())
            {
                View.Property(p => p.Reason).ShowInDetail(columnSpan: 2);
                View.Property(p => p.HandleMethod).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Measures).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Attachment).ShowInDetail(columnSpan: 2);
            }
        }
    }
}
