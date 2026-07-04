using SIE.Common.Prints;
using SIE.Domain;
using SIE.Packages;
using SIE.Packages.Packages;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Packages
{
    /// <summary>
    /// 包装规则明细视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class PackageRuleDetailViewConfig : WebViewConfig<PackageRuleDetail>
    {
        /// <summary>
        /// 包装规则明细 - 主信息 ViewGroup名
        /// </summary>
        public const string DetailMainViewGroup = "PackageRuleDetailMaster";

        /// <summary>
        /// 包装规则明细 - 附加 ViewGroup名
        /// </summary>
        public const string DetailSubViewGroup = "PackageRuleDetailSub";

        #region 只读控制,主单位不能编辑 IsReadOnly
        /// <summary>
        /// 只读控制
        /// </summary> 
        public static readonly Property<bool> IsReadOnlyProperty = P<PackageRuleDetail>.RegisterExtensionReadOnly("IsReadOnly", typeof(PackageRuleDetailViewConfig),
            GetIsReadOnly, PackageRuleDetail.PackageUnitIdProperty);

        /// <summary>
        /// 只读控制
        /// </summary>
        /// <param name="me">包装规则明细</param>
        /// <returns>bool</returns>
        public static bool GetIsReadOnly(PackageRuleDetail me)
        {
            return me.IsMasterUnit;
        }
        #endregion

        public const string strReadOnly = "p => p.data.IsMasterUnit == true";

        public const string strReadOnlyNoMaster = "p => p.data.IsMasterUnit == false";

        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DeclareExtendViewGroup(DetailMainViewGroup, DetailSubViewGroup);
            if (ViewGroup == DetailMainViewGroup)
            {
                DetailMainConfigView();
            }
            else if (ViewGroup == DetailSubViewGroup)
            {
                DetailSubConfigView();
            }
            else
            {
                //
            }
        }

        /// <summary>
        /// 包装规则明细主信息视图
        /// </summary>
        private void DetailSubConfigView()
        {
            View.InlineEdit();
            View.IsNotCopy();
            View.DomainName("附加信息");
            View.ClearCommands().UseCommands("SIE.Web.Packages.Packages.Commands.PackageRuleDetailAttachEditCommand");
            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnitName).ShowInList().Readonly();
                View.Property(p => p.Length).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Width).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Height).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Volume).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.Weight).UseSpinEditor(p => p.MinValue = 0).ShowInList();
                View.Property(p => p.CreateByName);
                View.Property(p => p.CreateDate).ShowInList(150);
                View.Property(p => p.UpdateByName);
                View.Property(p => p.UpdateDate).ShowInList(150);
            }
        }


        /// <summary>
        /// 包装规则明细附加信息视图
        /// </summary>
        private void DetailMainConfigView()
        {
            View.InlineEdit();
            View.DomainName("主信息");
            View.ClearCommands();
            View.UseCommands("SIE.Web.Packages.Packages.Commands.PackageRuleDetailAddCommand",
                             "SIE.Web.Packages.Packages.Commands.PackageRuleDetailEditCommand",
                             "SIE.Web.Packages.Packages.Commands.PackageRuleDetailDeleteCommand",
                             "SIE.Web.Packages.Packages.Commands.PackageRuleDetailMoveTopCommand",
                             "SIE.Web.Packages.Packages.Commands.PackageRuleDetailMoveUpCommand",
                             "SIE.Web.Packages.Packages.Commands.PackageRuleDetailMoveDownCommand",
                             "SIE.Web.Packages.Packages.Commands.PackageRuleDetailMoveBottomCommand");

            using (View.OrderProperties())
            {
                View.Property(p => p.PackageUnitId).UseDataSource((e, c, r) =>
                {
                    return RT.Service.Resolve<PackingUnitController>().GetUnitExceptMaster(r, c);
                }).UsePagingLookUpEditor((m, e) =>
                {
                    Dictionary<string, string> dic = new Dictionary<string, string>();
                    dic.Add(nameof(e.PackageUnitName), nameof(e.PackageUnit.Name));
                    m.DicLinkField = dic;
                }).Readonly(strReadOnly).ShowInList();
                View.Property(p => p.PackageUnitName).ShowInList().Readonly();
                View.Property(p => p.LevelQty).ShowInList().UseSpinEditor(p => { p.DecimalPrecision = 0; p.MinValue = 0; }).Readonly(strReadOnly);
                View.Property(p => p.Qty).ShowInList().UseSpinEditor(p => { p.DecimalPrecision = 0; p.MinValue = 0; }).Readonly(strReadOnlyNoMaster);
                View.Property(p => p.Description).ShowInList();
                View.Property(p => p.NumberRule).ShowInList(150);
                View.Property(p => p.IsPrint).ShowInList().Readonly(strReadOnly);
                View.Property(p => p.PrintTemplate).UsePagingLookUpEditor()
                    .UseDataSource((source, pagingInfo, keyword) =>
                {
                    var rule = source as PackageRuleDetail;
                    var templates = new EntityList<PrintTemplate>();
                    if (rule == null || rule.NumberRule == null)
                        return templates;

                    return RT.Service.Resolve<PackageController>().GetPrintTemplatesByRuleId(rule.NumberRuleId.Value, pagingInfo, keyword);
                }).UseListSetting(e => { e.HelpInfo = "显示可用编码规则模板"; }).ShowInList();
                View.Property(p => p.IsPackage).ShowInList();
                View.Property(p => p.IsOutStockLabel).UseCheckEditor(p => p.ColumnXType = "itempackageruledetailcheckeditor").ShowInList();
                View.Property(p => p.IsInStockLabel).UseCheckEditor(p => p.ColumnXType = "itempackageruledetailcheckeditor").ShowInList();
                View.Property(p => p.IsSequence).UseCheckEditor(p => p.ColumnXType = "itempackageruledetailcheckeditor").ShowInList();
                View.Property(p => p.PackageUnitType).ShowInList().Readonly();
                View.Property(p => p.CreateByName);
                View.Property(p => p.CreateDate).ShowInList(150);
                View.Property(p => p.UpdateByName);
                View.Property(p => p.UpdateDate).ShowInList(150);
            }
        }

        /// <summary>
        /// 选择视图配置
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.PackageUnitId);
            View.Property(p => p.PackageUnitName);
            View.Property(p => p.LevelQty);
            View.Property(p => p.Qty);
            View.Property(p => p.Description);
        }
    }
}