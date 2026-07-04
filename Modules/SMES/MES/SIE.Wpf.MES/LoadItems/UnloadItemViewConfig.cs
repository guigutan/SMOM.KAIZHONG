using SIE.Domain;
using SIE.MES.LoadItems;
using SIE.Wpf.MES.BatchWIP.Assemblys;
using SIE.Wpf.MES.WIP.Assemblys;
using SIE.Wpf.MES.WIP.Repairs;
using System;

namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 下料视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    internal class UnloadItemViewConfig : WPFViewConfig<UnloadItem>
    {
        #region 下料时间 UnloadItemDate
        /// <summary>
        /// 下料时间
        /// </summary>
        public static readonly Property<DateTime> UnloadItemDateProperty = P<UnloadItem>.RegisterExtensionReadOnly("UnloadItemDate", typeof(UnloadItemViewConfig),
            GetCreateDate, UnloadItem.IdProperty);

        /// <summary>
        /// 获取创建时间
        /// </summary>
        /// <param name="me">下料</param>
        /// <returns>创建时间</returns>
        public static DateTime GetCreateDate(UnloadItem me)
        {
            return me.CreateDate;
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(AssemblyViewModel), typeof(RepairViewModel), typeof(BatchAssemblyViewModel));
            using (View.OrderProperties())
            {
                View.Property(p => p.ItemCode).HasLabel("物料编码").Show(ShowInWhere.All);
                View.Property(p => p.ItemName).HasLabel("物料名称").Show(ShowInWhere.All); 
                View.Property(p => p.ItemExtPropName).HasLabel("物料属性").Show(ShowInWhere.All);
                View.Property(p => p.ProjectNo).HasLabel("项目号").Show(ShowInWhere.All);
                View.Property(p => p.SourceCode).HasLabel("标签号").Show(ShowInWhere.All);
                View.Property(p => p.SourceType).HasLabel("来源类型").Show(ShowInWhere.All);
                View.Property(p => p.Qty).HasLabel("下料数量").Show(ShowInWhere.All);
                View.Property(p => p.CreateDate).HasLabel("下料时间").UseListSetting(e => e.ListGridWidth = 150).Show(ShowInWhere.All);
                View.Property(p => p.CreateByName).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateDate).Show(ShowInWhere.Hide);
                View.Property(p => p.UpdateByName).Show(ShowInWhere.Hide);
                View.ChildrenProperty(p => p.DefectList).Show(ChildShowInWhere.Hide);
                
            }
        }
    }
}