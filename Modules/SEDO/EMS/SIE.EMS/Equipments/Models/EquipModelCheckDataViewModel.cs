using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.EMS.Equipments.Models
{
    /// <summary>
    ///  实体
    /// </summary>
    [RootEntity, Serializable]
    public class EquipModelCheckDataViewModel : ViewModel
    {
        #region 设备型号编码 Code
        /// <summary>
        /// 设备型号编码
        /// </summary>
        [Label("设备型号编码")]
        public static readonly Property<string> CodeProperty = P<EquipModelCheckDataViewModel>.Register(e => e.Code);
        /// <summary>
        /// 设备型号编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    ///  视图配置
    /// </summary>
    class SchedulingConfigureCheckDataViewModelConfig : WebViewConfig<EquipModelCheckDataViewModel>
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
