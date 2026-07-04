using SIE.Core.Equipments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Core.Equipments
{
    /// <summary>
    /// 设备台账视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class EquipAccountViewConfig : WebViewConfig<EquipAccount>
    {
        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.Property(p => p.Code).Show();
            View.Property(p => p.Name).Show();
            View.Property(p => p.ModelCode).Show();
            View.Property(p => p.ModelName).Show();
            View.Property(p => p.State).Show();

        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).Show().Readonly();
            View.Property(p => p.Name).Show().Readonly();
            View.Property(p => p.ModelCode).Show().Readonly();
            View.Property(p => p.ModelName).Show().Readonly();
            View.Property(p => p.State).Show().Readonly();
        }
    }
}
