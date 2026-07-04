using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;
using SIE.Wpf.Common;
using SIE.Wpf.Items.ProductModels.Commands;

namespace SIE.Wpf.Items.ProductModels.ViewModels
{
    /// <summary>
    /// 产品机型ViewModel
    /// </summary>
    [RootEntity]
    [Label("数据导入")]
    public class ImportProductModelCheckViewModel : ViewModel
    {
        #region ImportFilePath 导入模板文件路径
        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        [Label("导入文件")]
        public static readonly Property<string> ImportFilePathProperty = P<ImportProductModelCheckViewModel>.Register(e => e.ImportFilePath);

        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        public string ImportFilePath
        {
            get { return this.GetProperty(ImportFilePathProperty); }
            set { this.SetProperty(ImportFilePathProperty, value); }
        }
        #endregion

        #region ImportSuccessAmount 导入成功数量
        /// <summary>
        /// 导入成功数量
        /// </summary>
        [Label("导入成功数量")]
        public static readonly Property<int> ImportSuccessAmountProperty = P<ImportProductModelCheckViewModel>.Register(e => e.ImportSuccessAmount);

        /// <summary>
        /// 导入成功数量
        /// </summary>
        public int ImportSuccessAmount
        {
            get { return this.GetProperty(ImportSuccessAmountProperty); }
            set { this.SetProperty(ImportSuccessAmountProperty, value); }
        }
        #endregion

        #region ImportFailAmount 导入失败数量
        /// <summary>
        /// 导入失败数量
        /// </summary>
        [Label("导入失败数量")]
        public static readonly Property<int> ImportFailAmountProperty = P<ImportProductModelCheckViewModel>.Register(e => e.ImportFailAmount);

        /// <summary>
        /// 导入失败数量
        /// </summary>
        public int ImportFailAmount
        {
            get { return this.GetProperty(ImportFailAmountProperty); }
            set { this.SetProperty(ImportFailAmountProperty, value); }
        }
        #endregion

        #region ImportProcessMsg 导入处理消息
        /// <summary>
        /// 导入处理消息
        /// </summary>
        [Label("导入处理消息")]
        public static readonly Property<string> ImportProcessMsgProperty = P<ImportProductModelCheckViewModel>.Register(e => e.ImportProcessMsg);

        /// <summary>
        /// 导入处理消息
        /// </summary>
        public string ImportProcessMsg
        {
            get { return this.GetProperty(ImportProcessMsgProperty); }
            set { this.SetProperty(ImportProcessMsgProperty, value); }
        }
        #endregion

        #region ImportDataViewModelList 导入产品机型集
        /// <summary>
        /// 导入工单数据集
        /// </summary>
        public static readonly ListProperty<EntityList<ProductModelCheckDataViewModel>> ImportDataViewModelListProperty =
            P<ImportProductModelCheckViewModel>.RegisterList(e => e.ImportDataViewModelList, new ListPropertyMeta
            {
                HasManyType = HasManyType.Aggregation,
            });

        /// <summary>
        /// 导入工单数据集
        /// </summary>
        public EntityList<ProductModelCheckDataViewModel> ImportDataViewModelList
        {
            get { return this.GetLazyList(ImportDataViewModelListProperty); }
        }
        #endregion
    }

    /// <summary>
    /// 工单导入ViewModel视图配置
    /// </summary>
    class ImportWorkOrderCheckViewModelConfig : WPFViewConfig<ImportProductModelCheckViewModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ProductModel));
        }

        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(6);
            View.ClearCommands();
            View.UseCommands(typeof(DownLoadImportTemplateCommand), typeof(ImportProductModelDataCommand));
            View.Property(p => p.ImportFilePath).ShowInDetail(columnSpan: 4).UseFileSelectEditor(p => p.Filter = "xlsx|*.xlsx|xls|*.xls");
            View.Property(p => p.ImportSuccessAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportFailAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportProcessMsg).ShowInDetail(columnSpan: 6).Readonly();
            View.ChildrenProperty(p => p.ImportDataViewModelList);
        }
    }
}