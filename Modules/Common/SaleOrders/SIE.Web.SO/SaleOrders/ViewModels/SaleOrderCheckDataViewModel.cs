using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Web.SO.SaleOrders.ViewModels
{
    /// <summary>
    /// 销售订单 实体
    /// </summary>
    [RootEntity, Serializable]
    public class SaleOrderCheckDataViewModel : ViewModel
    {
        #region 销售订单编号 Code
        /// <summary>
        /// 销售订单编号
        /// </summary>
        [Label("销售订单编号")]
        public static readonly Property<string> CodeProperty = P<SaleOrderCheckDataViewModel>.Register(e => e.Code);
        /// <summary>
        /// 销售订单编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 销售订单 视图配置
    /// </summary>
    class SaleOrderCheckDataViewModelConfig : WebViewConfig<SaleOrderCheckDataViewModel>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            View.DomainName("导入失败数据");
        }

        /// <summary>
        /// 配置列表试图
        /// </summary>
        protected override void ConfigListView()
        {
            View.ClearCommands();
            View.Property(p => p.Code);
        }
    }
}
