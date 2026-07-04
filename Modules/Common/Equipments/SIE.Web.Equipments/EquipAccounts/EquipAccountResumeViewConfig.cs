using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
    /// 设备履历视图配置
    /// </summary>
    internal class EquipAccountResumeViewConfig : WebViewConfig<EquipAccountResume>
    {
        /// <summary>
        /// 字体显示宽度
        /// </summary>
        private const int charDisplayWidth = 20;
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.Equipments.EquipAccounts.Scripts.EquipAccountResumeBehavior");
            View.UseCommands("SIE.Web.Equipments.EquipAccounts.Commands.OpenResumeBillViewCommand",
                "SIE.Web.Equipments.EquipAccounts.Commands.EquipResumeSearchCommand");

            View.Property(p => p.CreateDate).Readonly().HasLabel("事件发生时间");
            View.Property(p => p.State).Readonly().ShowInList(width: charDisplayWidth * 5);
            View.Property(p => p.ResumeType).Readonly().ShowInList(width: charDisplayWidth * 4);
            View.Property(p => p.No).Readonly().ShowInList(width: charDisplayWidth * 10);
            View.Property(p => p.Changed).Readonly().ShowInList(width: charDisplayWidth * 4);
            View.Property(p => p.Remark).Readonly().ShowInList(width: charDisplayWidth*10);
            View.Property(p => p.CreateByName).Readonly().HasLabel("事件发起人").ShowInList(width: charDisplayWidth * 5);
            View.Property(p => p.UpdateByName).Readonly().HasLabel("修改人").ShowInList(width: charDisplayWidth * 5);
        }
    }
}