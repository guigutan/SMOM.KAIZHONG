using SIE.Domain;
using SIE.ManagedProperty;
using SIE.WorkBenchCommon.Workbench.TargetWarn;
using SIE.Wpf.Command;

namespace SIE.Wpf.WorkBenchCommon.Workbench.TargetWarn
{
    /// <summary>
    /// 达成率区间视图配置
    /// </summary>
    [CompiledPropertyDeclarer]
    public class TargetWarnDetailViewConfig : WPFViewConfig<TargetWarnDetail>
    {
        #region 只读控制,小于或者大于状态下最大值不能编辑 IsMaxvalueReadOnly
        /// <summary>
        /// 只读控制,主单位不能编辑
        /// </summary> 
        public static readonly Property<bool> IsMaxvalueReadOnlyProperty = P<TargetWarnDetail>.RegisterExtensionReadOnly("IsMaxvalueReadOnly", typeof(TargetWarnDetailViewConfig),
            GetIsMaxvalueReadOnly, TargetWarnDetail.MaxValueProperty);

        /// <summary>
        /// 只读控制,小于或者大于状态下最大值不能编辑
        /// </summary>
        /// <param name="me">参数</param>
        /// <returns>bool</returns>
        public static bool GetIsMaxvalueReadOnly(TargetWarnDetail me)
        {
            if (me.TargetOpetators != TargetOpetators.LessOrEqual)
                return false;
            return true;
        }

        /// <summary>
        /// 只读控制,主单位不能编辑
        /// </summary> 
        public static readonly Property<bool> IsMinvalueReadOnlyProperty = P<TargetWarnDetail>.RegisterExtensionReadOnly("IsMinvalueReadOnly", typeof(TargetWarnDetailViewConfig),
            GetIsMinvalueReadOnly, TargetWarnDetail.MinValueProperty);

        /// <summary>
        /// 只读控制,小于或者大于状态下最大值不能编辑
        /// </summary>
        /// <param name="me">参数</param>
        /// <returns>bool</returns>
        public static bool GetIsMinvalueReadOnly(TargetWarnDetail me)
        {
            if (me.TargetOpetators != TargetOpetators.GreaterOrEqual)
                return false;
            return true;
        }
        #endregion

        /// <summary>
        /// 有按钮的达成率目标设定视图名字
        /// </summary>
        public const string ComTargetWarn = "comTargetWarn";

        /// <summary>
        /// 无按钮的达成率目标设定视图名字
        /// </summary>
        protected override void ConfigView()
        {
            View.UseDefaultBehaviors();
            View.DeclareExtendViewGroup(ComTargetWarn);
            if (ViewGroup == ComTargetWarn)
            {
                View.ReplaceCommands(typeof(ListAddCommand), typeof(TargetWarnDetailAddCommand));
                View.UseCommands(typeof(ListEditCommand), typeof(ListDeleteCommand));
                ConfigComTargetWarn();
            }
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.AddBehavior(typeof(TarGetWarnDetailColorBehaviors));
            using (View.OrderProperties())
            {
                View.Property(p => p.TargetOpetators).HasLabel("条件");
                View.Property(p => p.MinValue).ShowInList().HasLabel("值1(%)");
                View.Property(p => p.MaxValue).ShowInList().HasLabel("值2(%)");
                View.Property(p => p.TargetColor).HasLabel("颜色");
                View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
            }
        }

        /// <summary>
        /// 配置有按钮的列表视图
        /// </summary>
        private void ConfigComTargetWarn()
        {
            View.AddBehavior(typeof(TarGetWarnDetailColorBehaviors));
            View.AddBehavior(typeof(TargetWarnDetailTypeChange));
            using (View.OrderProperties())
            {
                View.Property(p => p.TargetOpetators).ShowInList().HasLabel("条件");
                View.Property(p => p.MinValue).ShowInList().HasLabel("值1(%)").Readonly(IsMinvalueReadOnlyProperty)
                .UseListSetting(e => { e.HelpInfo = "目标条件[大于或等于]不可编辑"; });
                View.Property(p => p.MaxValue).ShowInList().HasLabel("值2(%)").Readonly(IsMaxvalueReadOnlyProperty)
                .UseListSetting(e => { e.HelpInfo = "目标条件[小于或等于]不可编辑"; });
                View.Property(p => p.TargetColor).ShowInList().HasLabel("颜色").Readonly();
                View.Property(DataEntity.UpdateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateByNameProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.CreateDateProperty).Show(ShowInWhere.Hide);
                View.Property(DataEntity.UpdateByNameProperty).Show(ShowInWhere.Hide);
            }
        }
    }
}