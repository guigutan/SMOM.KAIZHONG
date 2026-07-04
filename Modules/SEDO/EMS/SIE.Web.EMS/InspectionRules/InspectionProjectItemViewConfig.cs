using SIE.EMS.InspectionRules;
using SIE.Web.Core.Common.Commands;
using SIE.Web.EMS.InspectionRules.Commands;

namespace SIE.Web.EMS.InspectionRules
{
    /// <summary>
    /// 检验项目视图配置
    /// </summary>
    internal class InspectionProjectItemViewConfig : WebViewConfig<InspectionProjectItem>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {   
            View.AssignAuthorize(typeof(InspectionRule));
            View.UseCommands(typeof(SelProjectCheckCommand).FullName, typeof(ImmediateDeleteCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.ProjectName).HasLabel("项目名称").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Part).HasLabel("部位").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Consumable).HasLabel("项目耗材").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Method).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.Standard).Show(ShowInWhere.All);
                View.Property(p => p.MinValue).HasLabel("最小值").Show(ShowInWhere.All);
                View.Property(p => p.MaxValue).HasLabel("最大值").Show(ShowInWhere.All);
                View.Property(p => p.Unit).Show(ShowInWhere.All).Readonly();
                View.Property(p => p.UseTime).Show(ShowInWhere.All).Readonly();
                //View.Property(p => p.ProjectCheckCategory).HasLabel("检验类型").Show(ShowInWhere.All).Readonly();
                View.Property(p => p.CycleType).HasLabel("周期类型").Show(ShowInWhere.All).Readonly();
            }
        }
    }
}