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
    /// 设备清单视图
    /// </summary>
   internal class EquipmentListViewConfig : WebViewConfig<EquipmentList>
    {

        /// <summary>
        /// 添加设备清单视图
        /// </summary>
        public const string AddEquipGroup = "AddEquipGroup";

        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AddEquipGroup);
            if (ViewGroup == AddEquipGroup)
            {
                AddEquipmentView();
            }
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(SelectEquipListCommand).FullName, WebCommandNames.ExportXlsAll, typeof(DeleteEquipListCommand).FullName, typeof(ImportEquipListCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipCode);
                View.Property(p => p.EquipName);
                View.Property(p => p.EquipAlias);
                View.Property(p => p.EquipModelCode);
                View.Property(p => p.EquipModelName);
                View.Property(p => p.EquipUseState);
                View.Property(p => p.EquipTypeCode);
                View.Property(p => p.EquipTypeName);
                View.Property(p => p.EquipModelCategory);
                View.Property(p => p.UseDept);
                View.Property(p => p.WorkShop);
                View.Property(p => p.Manufacturer);
                View.Property(p => p.EnterDate);
                View.Property(p => p.InstallationLocation);
            }   
        }

        /// <summary>
        /// 添加设备清单视图
        /// </summary>
        void AddEquipmentView()
        {
            View.UseCommands(typeof(SelectEquipListCommand).FullName,WebCommandNames.ExportXlsAll, typeof(DeleteEquipListCommand).FullName,typeof(ImportEquipListCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.EquipCode).Show();
                View.Property(p => p.EquipName).Show();
                View.Property(p => p.EquipAlias).Show();
                View.Property(p => p.EquipModelCode).Show();
                View.Property(p => p.EquipModelName).Show();
                View.Property(p => p.EquipUseState).Show();
                View.Property(p => p.EquipTypeCode).Show();
                View.Property(p => p.EquipTypeName).Show();
                View.Property(p => p.EquipModelCategory).Show();
                View.Property(p => p.UseDept).Show();
                View.Property(p => p.WorkShop).Show();
                View.Property(p => p.Manufacturer).Show();
                View.Property(p => p.EnterDate).Show();
                View.Property(p => p.InstallationLocation).Show();
            }
        }

        /// <summary>
        /// 导入视图
        /// </summary>
        protected override void ConfigImportView()
        {
            using (View.OrderProperties())
            {
                View.PropertyRef(p => p.EquipAccout.Code).HasLabel("设备编码");
                View.PropertyRef(p => p.EquipAccout.Name).HasLabel("设备名称");
            }
        }

    }
}
