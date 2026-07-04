using SIE.EMS.Equipments.Boms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.Equipments.Boms
{
    /// <summary>
    /// 选择设备bom
    /// </summary>
    public class EquipBomSelectViewConfig : WebViewConfig<EquipBomSelect>
    {

        /// <summary>
        /// 选择视图
        /// </summary>
        public const string ShowSelectViewStr = "ShowSelectViewStr";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(EquipBom));
            View.DeclareExtendViewGroup(ShowSelectViewStr);
            if (ViewGroup == ShowSelectViewStr)
            {
                ShowSelectView();

            }
        }


        /// <summary>
		/// 复制新增弹框选择
		/// </summary>
		private void ShowSelectView()
        {
            View.WithoutPaging();
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipModel).ShowInList().Readonly();
                View.Property(p => p.EquipModelName).ShowInList().Readonly();
                View.Property(p => p.EquipTypeName).ShowInList().Readonly();
            }
        }
    }
}
