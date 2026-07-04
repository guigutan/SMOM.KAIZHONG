using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Maintains.Plans.ViewModels
{
    /// <summary>
    /// 保养计划 实体
    /// </summary>
    [RootEntity, Serializable]
    public class MaintainPlanCheckDataViewModel : ViewModel
    {
        //#region 保养单号
        ///// <summary>
        ///// 保养单号
        ///// </summary>
        //[Label("保养单号")]
        //public static readonly Property<string> CodeProperty = P<MaintainPlanCheckDataViewModel>.Register(e => e.Code);
        ///// <summary>
        ///// 保养单号
        ///// </summary>
        //public string Code
        //{
        //    get { return GetProperty(CodeProperty); }
        //    set { SetProperty(CodeProperty, value); }
        //}
        //#endregion
    }

    /// <summary>
    /// 保养计划 视图配置
    /// </summary>
    class MaintainPlanCheckDataViewModelConfig : WebViewConfig<MaintainPlanCheckDataViewModel>
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
            //View.Property(p => p.Code);
        }
    }
}
