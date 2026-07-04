using SIE.Defects.Measures;
using SIE.Domain;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.Defects.Measures.Commands
{
    /// <summary>
    /// 维修措施复制新增按钮
    /// </summary>
    [Command(ImageName = "ContentCopy", Label = "复制添加", ToolTip = "复制并添加数据", Location = CommandLocation.All, GroupType = CommandGroupType.Edit)]
    public class RepairMeasureCopyCommand : ListCopyCommand
    {
        /// <summary>
        /// 是否可以执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        /// <returns>true/false</returns>
        public override bool CanExecute(ListLogicalView view)
        {
            return view.Current != null && base.CanExecute(view);
        }

        /// <summary>
        /// 执行复制新增
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            var sourceRepairMeasure = View.Current as RepairMeasure;
            var desRepairMeasure = entity as RepairMeasure;
            desRepairMeasure.Code = sourceRepairMeasure.Code + "-复制".L10N();
            desRepairMeasure.Name = sourceRepairMeasure.Name + "-复制".L10N();
            desRepairMeasure.Description = sourceRepairMeasure.Description;
        }
    }
}
