using SIE.MES.TaskManagement.Configs;

namespace SIE.Web.MES.TaskManagement.Configs
{
    /// <summary>
    /// 派工任务配置项视图
    /// </summary>
    internal class DispatchConfigValueViewConfig : WebViewConfig<DispatchConfigValue>
    {
        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.Property(p => p.IsCheckProductionLineTaskList);
            //View.Property(p => p.IsCheckEmployeeSkill);
            //View.Property(p => p.IsCheckPersonnelPermission);
            //View.Property(p => p.IsAllowOverBatchQty);
            View.Property(p => p.IsAllowOverProcessCodes).UseListSetting(p => p.HelpInfo = "多个工序编码使用英文逗号分隔");
            //View.Property(p => p.IsFirstProcess).Show();
            View.Property(p => p.NumberRuleId);
            View.Property(p => p.GoodLabelTemplate).UsePagingLookUpEditor();
            View.Property(p => p.SuspectLabelTemplate).UsePagingLookUpEditor();
            View.Property(p => p.EntangleNumberRuleId).Show();
            View.Property(p => p.EntanglePrintTemplateId).Show();
            View.Property(p => p.UnEntangleNumberRuleId).Show();
            View.Property(p => p.UnEntanglePrintTemplateId).Show();
            //View.Property(p => p.GoodLabel).UseMemoEditor();
            //View.Property(p => p.SuspectLabel).UseMemoEditor();
            //View.Property(p => p.NewMaterialProValid).Show().UseListSetting(p => p.HelpInfo = "多个工序编码使用英文逗号分隔");
            //View.Property(p => p.IsValidScanQty).Show();
        }
    }
}
