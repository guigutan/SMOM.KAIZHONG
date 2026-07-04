using SIE.Domain;
using SIE.ObjectModel;
using SIE.Tech.Routings.ViewModels;
using SIE.Wpf.Tech.Routings.Commands;
using System;

namespace SIE.Wpf.Tech.Routings.ViewModels
{
    /// <summary>
    /// 导入工艺路线 实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("导入失败数据")]
    public class RoutingCheckDataViewModel : ProcessViewModel
    {
        #region 产品族分类 Category
        /// <summary>
        /// 产品族分类
        /// </summary>
        [Label("产品族分类")]
        public static readonly Property<string> CategoryProperty = P<RoutingCheckDataViewModel>.Register(e => e.Category);

        /// <summary>
        /// 产品族分类
        /// </summary>
        public string Category
        {
            get { return this.GetProperty(CategoryProperty); }
            set { this.SetProperty(CategoryProperty, value); }
        }
        #endregion

        #region 工艺路线名称 RoutingName
        /// <summary>
        /// 工艺路线名称
        /// </summary>
        [Label("工艺路线名称")]
        public static readonly Property<string> RoutingNameProperty = P<RoutingCheckDataViewModel>.Register(e => e.RoutingName);

        /// <summary>
        /// 工艺路线名称
        /// </summary>
        public string RoutingName
        {
            get { return this.GetProperty(RoutingNameProperty); }
            set { this.SetProperty(RoutingNameProperty, value); }
        }
        #endregion

        #region 工艺路线描述 RoutingDesc
        /// <summary>
        /// 工艺路线描述
        /// </summary>
        [Label("工艺路线描述")]
        public static readonly Property<string> RoutingDescProperty = P<RoutingCheckDataViewModel>.Register(e => e.RoutingDesc);

        /// <summary>
        /// 工艺路线描述
        /// </summary>
        public string RoutingDesc
        {
            get { return this.GetProperty(RoutingDescProperty); }
            set { this.SetProperty(RoutingDescProperty, value); }
        }
        #endregion

        #region 导入失败原因 ErrorMessage 
        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<RoutingCheckDataViewModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }
        #endregion

        #region 是否导入成功，汇总导入成功数量 IsSuccess
        /// <summary>
        /// 是否导入成功，汇总导入成功数量
        /// </summary>
        [Label("是否导入成功")]
        public static readonly Property<bool?> IsSuccessProperty = P<RoutingCheckDataViewModel>.Register(e => e.IsSuccess);

        /// <summary>
        /// 是否导入成功，汇总导入成功数量
        /// </summary>
        public bool? IsSuccess
        {
            get { return this.GetProperty(IsSuccessProperty); }
            set { this.SetProperty(IsSuccessProperty, value); }
        }
        #endregion  
    }

    /// <summary>
    /// 导入工艺路线 视图配置
    /// </summary>
    internal class RoutingCheckDataDetailViewModelConfig : WPFViewConfig<RoutingCheckDataViewModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ImportRoutingCheckViewModel));
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.UseCommands(typeof(ExportFailedDataCommand), typeof(ClearImportDateCommand));
            View.Property(p => p.ErrorMessage).UseListSetting(e => e.ListGridWidth = 200);
            View.Property(p => p.Category);
            View.Property(p => p.RoutingName);
            View.Property(p => p.RoutingDesc);
            View.Property(p => p.ProcessName);
            View.Property(p => p.StrSortOrder);
            View.Property(p => p.StrSortOrderBack);
            View.Property(p => p.StrResult);
            View.Property(p => p.ResultDesc);
            View.Property(p => p.StrCanChoose);
            View.Property(p => p.StrIsRepeat);
            View.Property(p => p.StrIsSku);
        }
    }
}