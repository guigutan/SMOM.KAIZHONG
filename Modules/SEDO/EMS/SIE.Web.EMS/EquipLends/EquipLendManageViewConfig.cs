using SIE.CSM.Suppliers;
using SIE.EMS.EquipLends;
using SIE.Equipments.EquipAccounts;
using SIE.MetaModel.View;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Web.EMS.EquipLends.Commands;
using System.Collections.Generic;

namespace SIE.Web.EMS.EquipLends
{
    /// <summary>
    /// 设备借还管理视图配置
    /// </summary>
    public class EquipLendManageViewConfig : WebViewConfig<EquipLendManage>
    {
        /// <summary>
        /// 视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.AddBehavior("SIE.Web.EMS.EquipLends.Scrpits.EquipLendManageBehavior");
            View.UseCommands(typeof(EquipLendAddCommand).FullName, "SIE.Web.EMS.EquipLends.Commands.EquipLendEditCommand",
                typeof(EquipLendDeleteCommand).FullName, typeof(EquipLendSubmitCommand).FullName,
                typeof(EquipLendReturnCommand).FullName, typeof(EquipLendExamineCommand).FullName,
                typeof(EquipLendImportCommand).FullName, WebCommandNames.ExportXls, WebCommandNames.ExportXlsAll);
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInList(width: 150);
                View.Property(p => p.EquipAccount).HasLabel("设备编码").Readonly().ShowInList();
                View.Property(p => p.EquipAccountName).Readonly().ShowInList();
                View.Property(p => p.LendState).Readonly().ShowInList();
                View.Property(p => p.LendObject).Readonly().ShowInList();
                View.Property(p => p.LendEmployee).Readonly().ShowInList();
                View.Property(p => p.LendEnterprise).Readonly().ShowInList();
                View.Property(p => p.Reason).Readonly().ShowInList(width: 200);
                View.Property(p => p.Supplier).HasLabel("供应商编码").Readonly().ShowInList();
                View.Property(p => p.SupplierName).Readonly().ShowInList();
                View.Property(p => p.ReturnRemark).Readonly().ShowInList(width: 200);
                View.Property(p => p.FixedAssetCode).Readonly().ShowInList();
                View.Property(p => p.RFID).Readonly().ShowInList();
                View.Property(p => p.ModelCode).Readonly().ShowInList();
                View.Property(p => p.ModelName).Readonly().ShowInList();
                View.Property(p => p.Remark).Readonly().ShowInList(width: 200);
                View.ChildrenProperty(p => p.AttachmentList);
                View.ChildrenProperty(p => p.ExamineRecordList);
            }
        }

        /// <summary>
        /// 表单视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(4);
            View.AddBehavior("SIE.Web.EMS.EquipLends.Scrpits.EquipLendManageDetailBehavior");
            View.UseCommands(typeof(EquipLendSaveSubmitCommand).FullName, typeof(EquipLendSaveCommand).FullName, "SIE.Web.EMS.EquipLends.Commands.EquipLendCancelCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.No).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.LendObject).ShowInDetail(columnSpan: 1);
                View.Property(p => p.LendEnterprise).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EnterpriseController>().GetDepartmentsNoTree(p, k);
                }).ShowInDetail(columnSpan: 1).Visibility(p => p.LendObject == SIE.EMS.EquipLends.Enums.LendObject.Internal);
                View.Property(p => p.LendEmployee).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EmployeeController>().GetJobEmployees(p, k);
                }).ShowInDetail(columnSpan: 1).Visibility(p => p.LendObject == SIE.EMS.EquipLends.Enums.LendObject.Internal);
                View.Property(p => p.Supplier).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<SupplierController>().GetSuppliers(p, k);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.SupplierName), nameof(e.Supplier.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("供应商编码").ShowInDetail(columnSpan: 1).Visibility(p => p.LendObject == SIE.EMS.EquipLends.Enums.LendObject.External);
                View.Property(p => p.SupplierName).Readonly().ShowInDetail(columnSpan: 1).Visibility(p => p.LendObject == SIE.EMS.EquipLends.Enums.LendObject.External);
                View.Property(p => p.EquipAccount).UseDataSource((e, p, k) =>
                {
                    return RT.Service.Resolve<EquipAccountController>().GetAllEquipAccounts(p, k, null, new List<SIE.Core.Enums.AccountUseState> { SIE.Core.Enums.AccountUseState.Scrap, SIE.Core.Enums.AccountUseState.ToAccepted, SIE.Core.Enums.AccountUseState.DisposedOf});
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> keyValues = new Dictionary<string, string>();
                    keyValues.Add(nameof(e.EquipAccountName), nameof(e.EquipAccount.Name));
                    m.DicLinkField = keyValues;
                }).HasLabel("设备编码").ShowInDetail(columnSpan: 1);
                View.Property(p => p.EquipAccountName).Readonly().ShowInDetail(columnSpan: 1);
                View.Property(p => p.Remark).ShowInDetail(columnSpan: 2);
                View.Property(p => p.Reason).UseMemoEditor(p => p.AllowBlank = false).ShowInDetail(columnSpan: 4);
                View.ChildrenProperty(p => p.AttachmentList);
            }
        }
    }
}
