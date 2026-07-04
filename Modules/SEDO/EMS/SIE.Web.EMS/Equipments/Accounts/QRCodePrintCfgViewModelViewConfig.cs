using SIE.Common.Prints;
using SIE.Domain;
using SIE.EMS.Equipments.Accounts.Printables;
using SIE.EMS.Equipments.Accounts.ViewModels;
using SIE.Equipments.EquipAccounts;
using System;

namespace SIE.Web.EMS.Equipments.Accounts.ViewModels
{
    /// <summary>
    /// 条码打印视图
    /// </summary>
    public class QRCodePrintCfgViewModelViewConfig : WebViewConfig<QRCodePrintCfgViewModel>
    {
        /// <summary>
        /// 打印批次视图
        /// </summary>
        public const string PRINT_VIEW = "PRINT_VIEW";

        /// <summary>
        /// 默认视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.FormEdit();
            View.AssignAuthorize(typeof(EquipAccount));
            View.DeclareExtendViewGroup(PRINT_VIEW);
            if (ViewGroup == PRINT_VIEW)
            {
                ConfigPrintView();
            }
        }



        /// <summary>
        /// 打印视图
        /// </summary>
        protected void ConfigPrintView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(1);
            View.Property(p => p.Template).UseDataSource((e, p, r) =>
            {
                string qualifiedName = typeof(EquipAccountPrintable).GetQualifiedName();
                var viewModel = e as QRCodePrintCfgViewModel;
                if (viewModel == null)
                    return new EntityList<PrintTemplate>();
                return RT.Service.Resolve<PrintsController>().GetPrintTemplates(qualifiedName, true);
            }).Show(ShowInWhere.Detail);
        }
    }
}
