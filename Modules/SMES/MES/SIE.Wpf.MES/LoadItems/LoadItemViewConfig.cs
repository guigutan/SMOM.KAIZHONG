using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.Wpf.Common.Sort;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.LoadItems.Commands;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Repairs;
using System;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 上料视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class LoadItemViewConfig : WPFViewConfig<LoadItem>
    {
        #region 上料时间 LoadItemDate
        /// <summary>
        /// 上料时间
        /// </summary>
        public static readonly Property<DateTime> LoadItemDateProperty = P<LoadItem>.RegisterExtensionReadOnly("LoadItemDate", typeof(LoadItemViewConfig),
            GetCreateDate, LoadItem.IdProperty);

        /// <summary>
        /// 获取创建时间
        /// </summary>
        /// <param name="me">上料</param>
        /// <returns>创建时间</returns>
        public static DateTime GetCreateDate(LoadItem me)
        {
            return me.CreateDate;
        }
        #endregion

        /// <summary>
        /// 通用视图配置
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel));
            View.AssignAuthorize(typeof(RepairViewModel));
            View.AssignAuthorize(typeof(BatchAssemblyViewModel));
            View.AssignAuthorize(typeof(WIP.TemporaryRepairs.TemporaryRepairViewModel));
            View.UseCommands(typeof(UnloadItemCommand), typeof(UnloadDefectItemCommand), typeof(UnloadAllItemCommand), typeof(RefreshLoadItemCommand));
            View.ReplaceCommands(typeof(MoveTopCommand), typeof(CusMoveTopCommand));
            View.ReplaceCommands(typeof(MoveUpCommand), typeof(CusMoveUpCommand));
            View.ReplaceCommands(typeof(MoveDownCommand), typeof(CusMoveDownCommand));
            View.ReplaceCommands(typeof(MoveBottomCommand), typeof(CusMoveBottomCommand));
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All);
                View.Property(p => p.ItemExtPropName).HasLabel("物料扩展属性").Show(ShowInWhere.All);
                View.Property(p => p.ProjectNo).HasLabel("项目号").Show(ShowInWhere.All);
                View.Property(p => p.WorkOrderId).HasLabel("上料工单").Show(ShowInWhere.All);
                View.Property(p => p.SourceCode).HasLabel("标签号").Show(ShowInWhere.All);
                View.Property(p => p.SourceType).HasLabel("来源类型").Show(ShowInWhere.All);
                View.Property(p => p.LoadQty).HasLabel("上料数量").Show(ShowInWhere.All);
                View.Property(p => p.Qty).HasLabel("剩余数量").Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).HasLabel("上料时间").UseListSetting(e => e.ListGridWidth = 150).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
            }
        }
    }
}
