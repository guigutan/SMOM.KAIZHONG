using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.Wpf.MES.LoadItems.Commands;
using SIE.Wpf.MES.WIP.Repairs;
using SIE.Wpf.MES.WIP.TemporaryRepairs;
using System;

namespace SIE.Wpf.MES.WIP.Assemblys
{
    /// <summary>
    /// 下料视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class UnloadItemViewModelViewConfig : WPFViewConfig<UnloadItem>
    {
        #region UnloadItemType 下料类型
        /// <summary>
        /// 下料类型
        /// </summary>
        public static readonly Property<string> UnloadItemTypeProperty = P<UnloadItem>.RegisterExtensionReadOnly("UnloadItemType", typeof(UnloadItemViewModelViewConfig),
            GetUnloadItemType, UnloadItem.IsNgProperty);

        /// <summary>
        /// 获取下料类型
        /// </summary>
        /// <param name="me">下料</param>
        /// <returns>下料类型</returns>
        public static string GetUnloadItemType(UnloadItem me)
        {
            return (!me.IsNg ? "正常下料" : "不良下料").L10N();
        }
        #endregion

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel));
            View.AssignAuthorize(typeof(RepairViewModel));
            View.AssignAuthorize(typeof(TemporaryRepairViewModel));
            View.UseCommands(typeof(RefreshUnloadItemCommand));
            View.Property(UnloadItemViewModelViewConfig.UnloadItemTypeProperty).ShowInList().HasLabel("下料类型").HasOrderNo(100);
            View.Property(p => p.CreateDate).HasLabel("下料时间").Show(ShowInWhere.All).HasOrderNo(101);
        }
    }
}