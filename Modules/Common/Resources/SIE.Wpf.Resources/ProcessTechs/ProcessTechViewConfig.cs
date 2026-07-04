using SIE.Domain;
using SIE.Resources.ProcessTechs;
using SIE.Wpf.Resources.ProcessTechs.Commands;

namespace SIE.Wpf.Resources.ProcessTechs
{
    /// <summary>
    /// 制程工艺视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ProcessTechViewConfig : WPFViewConfig<ProcessTech>
    {
        #region 判断转款时间是否只读 TransferTimeReadonly
        /// <summary>
        /// 判断转款时间是否只读
        /// </summary>
        internal static readonly Property<bool> TransferTimeReadonly = P<ProcessTech>.RegisterExtensionReadOnly("TransferTimeReadonly", typeof(ProcessTechViewConfig),
            GetTransferTimeReadonly, ProcessTech.TransferTimeProperty);

        /// <summary>
        /// 判断转款时间是否只读
        /// </summary>
        /// <param name="processTech">制程工艺</param>
        /// <returns>true/false</returns>
        internal static bool GetTransferTimeReadonly(ProcessTech processTech)
        {
            if (!processTech.IsScheduling)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 判断默认工艺定额是否只读 SAMReadonly
        /// <summary>
        ///  判断默认工艺定额是否只读
        /// </summary>
        internal static readonly Property<bool> SAMReadonly = P<ProcessTech>.RegisterExtensionReadOnly("SAMReadonly", typeof(ProcessTechViewConfig),
            GetSAMReadonly, ProcessTech.SAMProperty);

        /// <summary>
        /// 判断默认工艺定额是否只读
        /// </summary>
        /// <param name="processTech">制程工艺</param>
        /// <returns>true/false</returns>
        internal static bool GetSAMReadonly(ProcessTech processTech)
        {
            if (!processTech.IsScheduling)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        #region 判断偏移时间是否只读 OffsetTimeReadonly
        /// <summary>
        ///  判断偏移时间是否只读
        /// </summary>
        internal static readonly Property<bool> OffsetTimeReadonly = P<ProcessTech>.RegisterExtensionReadOnly("OffsetTimeReadonly", typeof(ProcessTechViewConfig),
            GetOffsetTimeReadonly, ProcessTech.OffsetTimeProperty);

        /// <summary>
        /// 判断偏移时间是否只读
        /// </summary>
        /// <param name="processTech">制程工艺</param>
        /// <returns>true/false</returns>
        internal static bool GetOffsetTimeReadonly(ProcessTech processTech)
        {
            if (processTech.IsScheduling)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion

        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.HasDelegate(ProcessTech.CodeProperty);
            View.UseDefaultBehaviors();
        }

        /// <summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(AddProcessTechCommand), WPFCommandNames.ListEdit, WPFCommandNames.ListDelete, typeof(CopyProcessTechCommand), typeof(SaveProcessTechCommand), typeof(ImportProcessTechCommand), typeof(ExportProcessTechCommand));
            View.Property(p => p.Code).HasLabel("制程编号");
            View.Property(p => p.Name).HasLabel("制程名称");
            View.Property(p => p.ProcessTechTypeId).HasLabel("制程类型");
            View.Property(p => p.ProcessSegmentId).HasLabel("工段");
            View.Property(p => p.IsBottleneck);
            View.Property(p => p.TransferTime).HasLabel("转款时间(秒)").Readonly(TransferTimeReadonly).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.Decimals = 2;
            });
            View.Property(p => p.SAM).HasLabel("默认工艺定额(秒/单位)").Readonly(SAMReadonly).UseSpinEditor(p =>
            {
                p.MinValue = 0;
                p.Decimals = 2;
            });
            View.Property(p => p.WorkingHours).UseSpinEditor(p => { p.MinValue = 1; p.Decimals = 0; });
        }

        /// <summary>
        /// 配置下拉视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code).HasLabel("制程编号");
            View.Property(p => p.Name).HasLabel("制程名称");
            View.Property(p => p.ProcessTechType).HasLabel("制程类型");
            View.Property(p => p.IsScheduling);
        }
    }
}
