using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.ProcessTechs;
using SIE.Wpf.Command;

namespace SIE.Wpf.Resources.ProcessTechs.ViewModels
{
    /// <summary>
    /// 制程工艺导入失败明细ViewModel
    /// </summary>
    [RootEntity]
    [Label("制程工艺导入失败明细")]
    public class ImportProcessTechDetailViewModel : ViewModel
    {
        #region 导入失败原因 ErrorMessage
        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }
        #endregion

        #region 制程编码 Code
        /// <summary>
        /// 制程编码
        /// </summary>
        [Label("制程编码")]
        public static readonly Property<string> CodeProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.Code);

        /// <summary>
        /// 制程编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 制程名称 Name
        /// <summary>
        /// 制程名称
        /// </summary>
        [Label("制程名称")]
        public static readonly Property<string> NameProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.Name);

        /// <summary>
        /// 制程名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 制程类型 ProcessTechType
        /// <summary>
        /// 制程类型
        /// </summary>
        [Label("制程类型")]
        public static readonly Property<string> ProcessTechTypeProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.ProcessTechType);

        /// <summary>
        /// 制程类型
        /// </summary>
        public string ProcessTechType
        {
            get { return this.GetProperty(ProcessTechTypeProperty); }
            set { this.SetProperty(ProcessTechTypeProperty, value); }
        }
        #endregion

        #region 工段 ProcessSegment
        /// <summary>
        /// 工段
        /// </summary>
        [Label("工段")]
        public static readonly Property<string> ProcessSegmentProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.ProcessSegment);

        /// <summary>
        /// 工段
        /// </summary>
        public string ProcessSegment
        {
            get { return this.GetProperty(ProcessSegmentProperty); }
            set { this.SetProperty(ProcessSegmentProperty, value); }
        }
        #endregion

        #region 是否瓶颈 IsBottleneck
        /// <summary>
        /// 是否瓶颈
        /// </summary>
        [Label("是否瓶颈")]
        public static readonly Property<string> IsBottleneckProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.IsBottleneck);

        /// <summary>
        /// 是否瓶颈
        /// </summary>
        public string IsBottleneck
        {
            get { return this.GetProperty(IsBottleneckProperty); }
            set { this.SetProperty(IsBottleneckProperty, value); }
        }
        #endregion

        #region 转款时间(秒) TransferTime
        /// <summary>
        /// 转款时间(秒)
        /// </summary>
        [Label("转款时间(秒)")]
        public static readonly Property<string> TransferTimeProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.TransferTime);

        /// <summary>
        /// 转款时间
        /// </summary>
        public string TransferTime
        {
            get { return this.GetProperty(TransferTimeProperty); }
            set { this.SetProperty(TransferTimeProperty, value); }
        }
        #endregion

        #region 默认工艺定额(秒/单位) SAM
        /// <summary>
        /// 默认工艺定额(秒/单位)
        /// </summary>
        [Label("默认工艺定额(秒/单位)")]
        public static readonly Property<string> SAMProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.SAM);

        /// <summary>
        /// 默认工艺定额(秒/单位)
        /// </summary>
        public string SAM
        {
            get { return this.GetProperty(SAMProperty); }
            set { this.SetProperty(SAMProperty, value); }
        }
        #endregion

        #region 工作时长(时) WorkingHours
        /// <summary>
        /// 工作时长(时)
        /// </summary>
        [Label("工作时长(时)")]
        public static readonly Property<string> WorkingHoursProperty = P<ImportProcessTechDetailViewModel>.Register(e => e.WorkingHours);

        /// <summary>
        /// 工作时长(时)
        /// </summary>
        public string WorkingHours
        {
            get { return this.GetProperty(WorkingHoursProperty); }
            set { this.SetProperty(WorkingHoursProperty, value); }
        }
        #endregion

    }
    /// <summary>
    /// 制程工艺导入失败明细视图配置
    /// </summary>
    public class ImportProcessTechDetailViewConfig : WPFViewConfig<ImportProcessTechDetailViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProcessTech));
        }

        /// <summary>
        /// 配置列表试图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(ExportCommand));
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ProcessTechType);
            View.Property(p => p.ProcessSegment);
            View.Property(p => p.IsBottleneck);
            View.Property(p => p.TransferTime);
            View.Property(p => p.SAM);
            View.Property(p => p.WorkingHours);
            View.Property(p => p.ErrorMessage);
        }
    }
}
