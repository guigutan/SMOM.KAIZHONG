using SIE.Domain;
using SIE.ObjectModel;
using SIE.Kit.MES.SingleLabels;

namespace SIE.Web.Kit.Mes.SingleLabels
{
    /// <summary>
    /// 单体条码导入ViewModel
    /// </summary>
    [RootEntity]
    [Label("数据导入")]
    public class ImportSingleLabelViewModel : ViewModel
    {
        #region ImportFilePath 导入模板文件路径

        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        [Label("导入文件")]
        public static readonly Property<string> ImportFilePathProperty = P<ImportSingleLabelViewModel>.Register(e => e.ImportFilePath);

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
        public static readonly Property<int> ImportSuccessAmountProperty = P<ImportSingleLabelViewModel>.Register(e => e.ImportSuccessAmount);

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
        public static readonly Property<int> ImportFailAmountProperty = P<ImportSingleLabelViewModel>.Register(e => e.ImportFailAmount);

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
        public static readonly Property<string> ImportProcessMsgProperty = P<ImportSingleLabelViewModel>.Register(e => e.ImportProcessMsg);

        /// <summary>
        /// 导入处理消息
        /// </summary>
        public string ImportProcessMsg
        {
            get { return this.GetProperty(ImportProcessMsgProperty); }
            set { this.SetProperty(ImportProcessMsgProperty, value); }
        }

        #endregion

        #region 导入产品检验标准数据集 ImportDataViewModelList
        /// <summary>
        /// 导入产品检验标准数据集
        /// </summary>
        [Label("属性名")]
        public static readonly ListProperty<EntityList<ImportSingleLabelDetailModel>> ImportDataViewModelListProperty = P<ImportSingleLabelViewModel>.RegisterList(e => e.ImportDataViewModelList, new ListPropertyMeta
        {
            HasManyType = HasManyType.Aggregation,
            DataProvider = e => (e as ImportSingleLabelViewModel).LoadProductCategoryList()
        });

        /// <summary>
        /// 导入产品检验标准数据集
        /// </summary>
        public EntityList<ImportSingleLabelDetailModel> ImportDataViewModelList
        {
            get { return this.GetLazyList(ImportDataViewModelListProperty); }
        }

        /// <summary>
        /// 加载产品类型列表
        /// </summary>
        /// <returns>单体条码导入失败明细</returns>
        private EntityList<ImportSingleLabelDetailModel> LoadProductCategoryList()
        {
            return new EntityList<ImportSingleLabelDetailModel>();
        }
        #endregion
    }

    /// <summary>
    /// 单体条码导入导入视图配置
    /// </summary>
    internal class ImportSingleLabelViewConfig : WPFViewConfig<ImportSingleLabelViewModel>
    {
        /// <summary>
        /// 配置明细试图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.AssignAuthorize(typeof(SingleLabel));
            View.HasDetailColumnsCount(6);
            View.ClearCommands();
            ////View.UseCommands(typeof(DownloadCommand), typeof(ImportSingleLabelCommand));
            View.Property(p => p.ImportFilePath).ShowInDetail(columnSpan: 4)/*.UseFileSelectEditor(p => p.Filter = "xlsx|*.xlsx|xls|*.xls")*/;
            View.Property(p => p.ImportSuccessAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportFailAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportProcessMsg).ShowInDetail(columnSpan: 6).Readonly();
            View.ChildrenProperty(p => p.ImportDataViewModelList);
        }
    }
}