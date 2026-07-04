using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Command;
using System;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 指标定义明细保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存当前数据", Gestures = "Ctrl+S", GroupType = 10)]
    class SaveQuotaTargetDetailCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存逻辑
        /// </summary>
        /// <param name="entity">实体</param>
        protected override void DoSave(Entity entity)
        {
            base.DoSave(entity);
            var asntype = typeof(EntitySavedEvent<>).MakeGenericType(typeof(QuotaTargetSetting));
            var @asnevent = Activator.CreateInstance(asntype, new QuotaTargetSetting());
            RT.EventBus.Publish(@asnevent);
        }
    }
}
