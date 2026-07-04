using SIE.Kit.APS.TargetCapacitys;
using SIE.Resources.Enterprises;
using SIE.Web.Resources;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.APS.TargetCapacitys
{
    /// <summary>
    /// 目标产能视图配置
    /// </summary>
    internal class TargetCapacityViewConfig : WebViewConfig<TargetCapacity>
    {
        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
        }
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            using (View.OrderProperties())
            {
                // View.Property(p => p.EnterpriseId).UseFactoryEditor();
                View.Property(p => p.EnterpriseId).HasLabel("工厂编码").UseDataSource((e, p, r) =>
                {
                    var enterprises = RT.Service.Resolve<EnterpriseController>().GetEnterprises(EnterpriseType.Plant, p, r);
                    enterprises.ForEach(enterprise => { enterprise.TreePId = null; });
                    return enterprises;
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.EnterpriseName), nameof(e.Enterprise.Name));
                    m.DicLinkField = dic;
                    m.DisplayField = nameof(Enterprise.Code);
                });

                View.Property(p => p.EnterpriseName).HasLabel("工厂名称").Readonly();

                View.Property(p => p.Year);
                View.Property(p => p.M1).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M2).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M3).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M4).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M5).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M6).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M7).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M8).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M9).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M10).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M11).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
                View.Property(p => p.M12).UseSpinEditor(p =>
                {
                    p.MinValue = 0;
                    p.AllowDecimals = true;
                });
            }
        }
    }
}