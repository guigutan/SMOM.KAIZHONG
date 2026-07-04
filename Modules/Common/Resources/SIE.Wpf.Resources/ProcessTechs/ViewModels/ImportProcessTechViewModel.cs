using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.ProcessTechs;
using SIE.Wpf.Common;
using SIE.Wpf.Resources.ProcessTechs.Commands;

namespace SIE.Wpf.Resources.ProcessTechs.ViewModels
{
    /// <summary>
    /// 制程工艺导入ViewModel
    /// </summary>
    [RootEntity]
    [Label("制程工艺导入")]
    public class ImportProcessTechViewModel : ViewModel
    {
        #region 导入模板文件路径 ImportFilePath
        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        [Label("导入文件")]
        public static readonly Property<string> ImportFilePathProperty = P<ImportProcessTechViewModel>.Register(e => e.ImportFilePath);

        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        public string ImportFilePath
        {
            get { return this.GetProperty(ImportFilePathProperty); }
            set { this.SetProperty(ImportFilePathProperty, value); }
        }
        #endregion

        #region 导入成功数量 ImportSuccessAmount
        /// <summary>
        /// 导入成功数量
        /// </summary>
        [Label("导入成功数量")]
        public static readonly Property<int> ImportSuccessAmountProperty = P<ImportProcessTechViewModel>.Register(e => e.ImportSuccessAmount);

        /// <summary>
        /// 导入成功数量
        /// </summary>
        public int ImportSuccessAmount
        {
            get { return this.GetProperty(ImportSuccessAmountProperty); }
            set { this.SetProperty(ImportSuccessAmountProperty, value); }
        }
        #endregion

        #region 导入失败数量 ImportFailAmount
        /// <summary>
        /// 导入失败数量
        /// </summary>
        [Label("导入失败数量")]
        public static readonly Property<int> ImportFailAmountProperty = P<ImportProcessTechViewModel>.Register(e => e.ImportFailAmount);

        /// <summary>
        /// 导入失败数量
        /// </summary>
        public int ImportFailAmount
        {
            get { return this.GetProperty(ImportFailAmountProperty); }
            set { this.SetProperty(ImportFailAmountProperty, value); }
        }
        #endregion

        #region 导入处理消息 ImportProcessMsg
        /// <summary>
        /// 导入处理消息
        /// </summary>
        [Label("导入处理消息")]
        public static readonly Property<string> ImportProcessMsgProperty = P<ImportProcessTechViewModel>.Register(e => e.ImportProcessMsg);

        /// <summary>
        /// 导入处理消息
        /// </summary>
        public string ImportProcessMsg
        {
            get { return this.GetProperty(ImportProcessMsgProperty); }
            set { this.SetProperty(ImportProcessMsgProperty, value); }
        }
        #endregion

        #region 导入失败数据 ImportDataViewModelList
        /// <summary>
        /// 导入失败数据
        /// </summary>
        [Label("导入失败数据")]
        public static readonly ListProperty<EntityList<ImportProcessTechDetailViewModel>> ImportDataViewModelListProperty = P<ImportProcessTechViewModel>.RegisterList(e => e.ImportDataViewModelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation
        });

        /// <summary>
        /// 导入失败数据
        /// </summary>
        public EntityList<ImportProcessTechDetailViewModel> ImportDataViewModelList
        {
            get { return this.GetLazyList(ImportDataViewModelListProperty); }
        }
        #endregion

    }
    /// <summary>
    /// 产品分类检验标准导入视图配置
    /// </summary>
    internal class ImportCtgInspStdViewConfig : WPFViewConfig<ImportProcessTechViewModel>
    {
        /// <summary>
        /// 配置默认试图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProcessTech));
        }

        /// <summary>
        /// 配置明细试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(6);
            View.ClearCommands();
            View.UseCommands(typeof(DownloadProcessTechViewModelCommand), typeof(ImportProcessTechViewModelCommand));
            View.Property(p => p.ImportFilePath).ShowInDetail(columnSpan: 4).UseFileSelectEditor(p => p.Filter = "xlsx|*.xlsx|xls|*.xls");
            View.Property(p => p.ImportSuccessAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportFailAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportProcessMsg).ShowInDetail(columnSpan: 6).Readonly();
            View.ChildrenProperty(p => p.ImportDataViewModelList);
        }
    }
}
