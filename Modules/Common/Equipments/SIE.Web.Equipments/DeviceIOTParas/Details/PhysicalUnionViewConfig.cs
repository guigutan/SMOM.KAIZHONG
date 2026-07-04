using SIE.Equipments.DeviceIOTParas.Details;
using SIE.Equipments.DeviceIOTParas.Enums;
using SIE.MetaModel.View;
using SIE.Web.Equipments.EquipAccounts.Commands;

namespace SIE.Web.Equipments.DeviceIOTParas.Details
{

    /// <summary>
    /// 物联参数视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class PhysicalUnionViewConfig : WebViewConfig<PhysicalUnion>
    {
        /// <summary>
        /// 设备台账物联参数视图
        /// </summary>
        public const string AccountPhysicalUnionViewGroup = "AccountPhysicalUnionViewGroup";

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(AccountPhysicalUnionViewGroup);
            if (ViewGroup == AccountPhysicalUnionViewGroup)
                ConfigAccountPhysicalUnionView();
        }

        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            //View.UseDefaultCommands();
            View.UseCommands(
                "SIE.Web.Equipments.DeviceIOTParas.Commands.SelectMDCCommand",
                WebCommandNames.Add, WebCommandNames.Edit, WebCommandNames.Delete, WebCommandNames.ExportXls);
            View.Property(p => p.Enable).UseCheckEditor();
            View.Property(p => p.MDCVariableName).Readonly();
            View.Property(p => p.PararCode).Readonly(p=>p.From==FromType.Interface);
            View.Property(p => p.ParaName);
            View.Property(p => p.InitialValue);
            View.Property(p => p.MaxValue);
            View.Property(p => p.MinValue);
            View.Property(p => p.Unit);
            View.Property(p => p.MDCVariableCode);
            View.Property(p => p.From).DefaultValue(1);
            View.Property(p => p.IsCheck).UseCheckEditor().Readonly(p=> !p.Enable);
        }

        /// <summary>
        /// 设备台账物联参数视图
        /// </summary>
        public void ConfigAccountPhysicalUnionView()
        {
            View.UseCommand(typeof(GetRealTimeDataCommand).FullName);
            using (View.OrderProperties())
            {
                View.Property(p => p.PararCode).Show();
                View.Property(p => p.ParaName).Show();
                View.Property(p => p.MDCVariableName).Show();
                View.Property(p => p.RealValue).Show();
                View.Property(p => p.MaxValue).Show();
                View.Property(p => p.MinValue).Show();
                View.Property(p => p.Unit).Show();
                View.Property(p => p.AutoGetDate).Show();
            }
        }

        ///<summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Enable).Readonly();
            View.Property(p => p.MDCVariableName).Readonly();
            View.Property(p => p.PararCode).Readonly();
            View.Property(p => p.ParaName).Readonly();
            View.Property(p => p.InitialValue).Readonly();
            View.Property(p => p.MaxValue).Readonly();
            View.Property(p => p.MinValue).Readonly();
            View.Property(p => p.Unit).Readonly();
            View.Property(p => p.MDCVariableCode).Readonly();
        }
    }
}
