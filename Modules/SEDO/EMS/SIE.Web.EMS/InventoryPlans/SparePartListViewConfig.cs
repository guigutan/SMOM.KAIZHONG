using SIE.EMS.InventoryPlans;
using SIE.MetaModel.View;
using SIE.Web.EMS.InventoryPlans.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.EMS.InventoryPlans
{
    /// <summary>
    /// 备件清单视图
    /// </summary>
    internal class SparePartListViewConfig : WebViewConfig<SparePartList>
    {
        /// <summary>
        /// 添加备件清单视图
        /// </summary>
        public const string AddSpareGroup = "AddSpareGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddSpareGroup);
            if (ViewGroup == AddSpareGroup)
            {
                AddSparePartView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelectSparePartCommand).FullName, WebCommandNames.ExportXlsAll, typeof(DeleteSpareListCommand).FullName, typeof(ImportSpareListCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCode);
                View.Property(p => p.SparePartName);
                View.Property(p => p.SpartPartSpec);
               View.Property(p => p.ItemCateName);
                View.Property(p => p.SpartType);
                View.Property(p => p.TypeCode);
                View.Property(p => p.SpartEquipModel);
                View.Property(p => p.SparePartState);
                View.Property(p => p.ControlMethod);
            }
        }

        /// <summary>
        /// 添加设备清单视图
        /// </summary>
        void AddSparePartView()
        {
         /*   View.UseCommands(typeof(SelectSparePartCommand).FullName, WebCommandNames.ExportXlsAll, typeof(DeleteSpareListCommand).FullName, typeof(ImportSpareListCommand).FullName);*/
            using (View.OrderProperties())
            {
                View.Property(p => p.SparePartCode).Show();
                View.Property(p => p.SparePartName).Show();
                View.Property(p => p.SpartPartSpec).Show();
                View.Property(p => p.ItemCateName).Show();
                View.Property(p => p.SpartType).Show();
                View.Property(p => p.TypeCode).Show();
                View.Property(p => p.SpartEquipModel).Show();
                View.Property(p => p.SparePartState).Show();
                View.Property(p => p.ControlMethod).Show();
            }
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.SparePart.SparePartCode).HasLabel("备件编码");
                View.PropertyRef(p => p.SparePart.SparePartName).HasLabel("备件名称");
            }
        }
    }
}
