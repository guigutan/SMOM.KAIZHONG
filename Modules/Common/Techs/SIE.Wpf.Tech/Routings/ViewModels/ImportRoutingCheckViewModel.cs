using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Routings;
using SIE.Wpf.Common;
using SIE.Wpf.Tech.Routings.Commands;
using System;

namespace SIE.Wpf.Tech.Routings.ViewModels
{
    /// <summary>
    /// 工艺路线导入ViewModel
    /// </summary>
    [RootEntity]
    public class ImportRoutingCheckViewModel : ViewModel
    {
        /// <summary>
        /// 导入完成事件
        /// </summary>
        public event EventHandler ImportFinishEvent;

        #region ImportFilePath 导入模板文件路径
        /// <summary>
        /// 导入模板文件路径
        /// </summary>
        [Label("导入文件")]
        public static readonly Property<string> ImportFilePathProperty = P<ImportRoutingCheckViewModel>.Register(e => e.ImportFilePath);

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
        public static readonly Property<int> ImportSuccessAmountProperty = P<ImportRoutingCheckViewModel>.Register(e => e.ImportSuccessAmount);

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
        public static readonly Property<int> ImportFailAmountProperty = P<ImportRoutingCheckViewModel>.Register(e => e.ImportFailAmount);

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
        public static readonly Property<string> ImportProcessMsgProperty = P<ImportRoutingCheckViewModel>.Register(e => e.ImportProcessMsg);

        /// <summary>
        /// 导入处理消息
        /// </summary>
        public string ImportProcessMsg
        {
            get { return this.GetProperty(ImportProcessMsgProperty); }
            set { this.SetProperty(ImportProcessMsgProperty, value); }
        }
        #endregion

        #region ImportDataViewModelList 导入数据集
        /// <summary>
        /// 导入工单数据集
        /// </summary>
        public static readonly ListProperty<EntityList<RoutingCheckDataViewModel>> ImportDataViewModelListProperty =
            P<ImportRoutingCheckViewModel>.RegisterList(e => e.ImportDataViewModelList, new ListPropertyMeta
            {
                HasManyType = HasManyType.Aggregation,
                DataProvider = e => new EntityList<RoutingCheckDataViewModel>()
            });

        /// <summary>
        /// 导入工单数据集
        /// </summary>
        public EntityList<RoutingCheckDataViewModel> ImportDataViewModelList
        {
            get { return this.GetLazyList(ImportDataViewModelListProperty); }
        }
        #endregion 

        /// <summary>
        /// 导入完成方法
        /// </summary>
        public void ImportFinish()
        {
            ImportFinishEvent?.Invoke(this, EventArgs.Empty);
        }
    }

    /// <summary>
    /// 工单导入ViewModel视图配置
    /// </summary>
    public class ImportRoutingCheckViewModelConfig : WPFViewConfig<ImportRoutingCheckViewModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("数据导入");
            View.AssignAuthorize(typeof(Routing));
        }

        /// <summary>
        /// 明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.HasDetailColumnsCount(6);
            View.ClearCommands();
            View.UseCommands(typeof(DownloadRoutingCommand), typeof(ImportRoutingDataCommand));
            View.Property(p => p.ImportFilePath).ShowInDetail(columnSpan: 4).UseFileSelectEditor(p => p.Filter = "xlsx|*.xlsx|xls|*.xls");
            View.Property(p => p.ImportSuccessAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportFailAmount).ShowInDetail(columnSpan: 1).Readonly();
            View.Property(p => p.ImportProcessMsg).ShowInDetail(columnSpan: 6).Readonly();
            View.ChildrenProperty(p => p.ImportDataViewModelList);
        }
    }
}