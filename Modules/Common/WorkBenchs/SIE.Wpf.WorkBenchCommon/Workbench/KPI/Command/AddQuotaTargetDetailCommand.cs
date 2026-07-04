using SIE.Domain;
using SIE.WorkBenchCommon.Workbench.KPI;
using SIE.Wpf.Command;

namespace SIE.Wpf.WorkBenchCommon.Workbench.KPI
{
    /// <summary>
    /// 明细表添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class AddQuotaTargetDetailCommand : ListAddCommand
    {
        /// <summary>
        /// 执行命令代码块
        /// </summary>
        /// <param name="view">详细逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            base.Execute(view);
        }

        /// <summary>
        /// 赋予父表相同周期类型给实体
        /// </summary>
        /// <param name="entity">指标定义明细</param>
        protected override void OnItemCreated(Entity entity)
        {
            base.OnItemCreated(entity);
            QuotaTargetDetail quotatargetdetail = entity as QuotaTargetDetail;
            QuotaTargetSetting quotatargetsetting = View.Parent.Current as QuotaTargetSetting;
            quotatargetdetail.DataType = quotatargetsetting.DataType;
        }
    }
}
