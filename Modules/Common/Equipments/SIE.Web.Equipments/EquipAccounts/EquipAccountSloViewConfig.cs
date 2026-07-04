using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.Equipments.EquipAccounts
{
    /// <summary>
    /// 缸槽视图配置
    /// </summary>
    public class EquipAccountSloViewConfig : WebViewConfig<EquipAccountSlot>
    {
        /// <summary>
        /// 设备台账缸槽列表视图
        /// </summary>
        public const string EquipAccountSloListViewGroup = "EquipAccountSloListViewGroup";

        /// <summary>
        /// 计量设备缸槽列表视图
        /// </summary>
        public const string MeteringEquipmentSloListViewGroup = "EquipAccountSloListViewGroup";

        /// <summary>
        /// 特种设备缸槽列表视图
        /// </summary>
        public const string SpecialEquipmentAccountViewGroup = "SpecialEquipmentAccountViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.InlineEdit();
            View.HasDelegate(EquipAccountSlot.NameProperty);
            View.AssignAuthorize(typeof(EquipAccount));

            View.DeclareExtendViewGroup(new string[] { EquipAccountSloListViewGroup, MeteringEquipmentSloListViewGroup, SpecialEquipmentAccountViewGroup });
            if (ViewGroup == EquipAccountSloListViewGroup)
                EquipAccountSloListView();
            if (ViewGroup == MeteringEquipmentSloListViewGroup)
                EquipAccountSloListView();
            if (ViewGroup == SpecialEquipmentAccountViewGroup)
                EquipAccountSloListView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();            
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ChemicalAddWay);
            View.Property(p => p.Volume).UseSpinEditor(c => { c.MinValue = 0.01; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 2; });
            View.Property(p => p.Unit);
            View.Property(p => p.Description);

        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.EquipAccountName);
            View.Property(p => p.ChemicalAddWay);
            View.Property(p => p.Volume);
            View.Property(p => p.Unit);
        }

        /// <summary>
        /// 查询视图配置
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.EquipAccount).HasLabel("设备台账");
        }

        /// <summary>
        /// 设备台账缸槽列表视图
        /// </summary>
        public void EquipAccountSloListView()
        {
            View.UseCommands(WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete);
            View.UseCommand(typeof(EquipAccountSlotSaveCommand).FullName);
            View.InlineEdit();
            using (View.OrderProperties())
            {
                View.Property(p => p.Code).Show();
                View.Property(p => p.Name).Show();
                View.Property(p => p.ChemicalAddWay).Show();
                View.Property(p => p.Volume).UseSpinEditor(c => { c.MinValue = 0.01; c.AllowNegative = false; c.AllowDecimals = true; c.DecimalPrecision = 2; }).Show();
                View.Property(p => p.Unit).Show();
                View.Property(p => p.Description).Show();

                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.CreateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
            }
        }
    }
}