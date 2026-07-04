using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.Items.ProductModels.Commands;
using System;

namespace SIE.Wpf.Items.ProductModels.ViewModels
{
    /// <summary>
    /// 导入失败ViewModel
    /// </summary>
    [RootEntity, Serializable]
    [Label("导入失败数据")]
    public class ProductModelCheckDataViewModel : ViewModel
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("机型编码")]
        public static readonly Property<string> CodeProperty = P<ProductModelCheckDataViewModel>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("机型名称")]
        public static readonly Property<string> NameProperty = P<ProductModelCheckDataViewModel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 产线工时(单位/小时) LineWorkingHours
        /// <summary>
        /// 工时(单位/小时)
        /// </summary>
        [Label("产线工时（单位/小时）")]
        public static readonly Property<string> LineWorkingHoursProperty = P<ProductModelCheckDataViewModel>.Register(e => e.LineWorkingHours);

        /// <summary>
        /// 工时(单位/小时)
        /// </summary>
        public string LineWorkingHours
        {
            get { return GetProperty(LineWorkingHoursProperty); }
            set { SetProperty(LineWorkingHoursProperty, value); }
        }
        #endregion

        #region 配送周期（小时） SendingHours
        /// <summary>
        /// 配送周期（小时）
        /// </summary>
        [Label("配送周期（小时）")]
        public static readonly Property<string> SendingHoursProperty = P<ProductModelCheckDataViewModel>.Register(e => e.SendingHours);

        /// <summary>
        /// 配送周期（小时）
        /// </summary>
        public string SendingHours
        {
            get { return GetProperty(SendingHoursProperty); }
            set { SetProperty(SendingHoursProperty, value); }
        }
        #endregion

        #region 工时 WorkingHours
        /// <summary>
        /// 工时
        /// </summary>
        [Label("工时（单位/小时）")]
        public static readonly Property<string> WorkingHoursProperty = P<ProductModelCheckDataViewModel>.Register(e => e.WorkingHours);

        /// <summary>
        /// 工时
        /// </summary>
        public string WorkingHours
        {
            get { return GetProperty(WorkingHoursProperty); }
            set { SetProperty(WorkingHoursProperty, value); }
        }
        #endregion

        #region 资源 Resource
        /// <summary>
        /// 资源
        /// </summary>
        [Label("资源")]
        public static readonly Property<string> ResourceProperty = P<ProductModelCheckDataViewModel>.Register(e => e.Resource);
        /// <summary>
        /// 资源
        /// </summary>
        public string Resource
        {
            get { return this.GetProperty(ResourceProperty); }
            set { this.SetProperty(ResourceProperty, value); }
        }
        #endregion

        #region ErrorMessage 导入失败原因
        /// <summary>
        /// 导入失败原因
        /// </summary>
        [Label("导入失败原因")]
        public static readonly Property<string> ErrorMessageProperty = P<ProductModelCheckDataViewModel>.Register(e => e.ErrorMessage);

        /// <summary>
        /// 导入失败原因
        /// </summary>
        public string ErrorMessage
        {
            get { return this.GetProperty(ErrorMessageProperty); }
            set { this.SetProperty(ErrorMessageProperty, value); }
        }
        #endregion
    }

    class ProductModelCheckDataViewModelViewConfig : WPFViewConfig<ProductModelCheckDataViewModel>
    {
        /// <summary>
        /// 默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.AssignAuthorize(typeof(ImportProductModelCheckViewModel));
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseCommands(typeof(ExportFailedDataCommand));
            View.Property(p => p.ErrorMessage);
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.WorkingHours);
            View.Property(p => p.SendingHours);
            View.Property(p => p.Resource);
            View.Property(p => p.LineWorkingHours);
        }
    }
}
