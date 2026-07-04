using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Equipments.DeviceIOTParas.ViewModles
{
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(MDCDetailViewModleCriteria))]
    /// <summary>
    /// MDC接口视图
    /// </summary>
    [Label("MDC接口视图")]
    public class MDCDetailViewModle : ViewModel
    {
        #region 设备编码 AssetsCode
        /// <summary>
        /// 设备编码
        /// </summary>
        [Label("设备编码")]
        public static readonly Property<string> AssetsCodeProperty = P<MDCDetailViewModle>.Register(e => e.EquipmentCode);

        /// <summary>
        /// 设备编码
        /// </summary>
        public string EquipmentCode
        {
            get { return this.GetProperty(AssetsCodeProperty); }
            set { this.SetProperty(AssetsCodeProperty, value); }
        }
        #endregion

        #region 型号 MesModel
        /// <summary>
        /// 型号
        /// </summary>
        [Label("型号")]
        public static readonly Property<string> MesModelProperty = P<MDCDetailViewModle>.Register(e => e.MesModel);

        /// <summary>
        /// 型号
        /// </summary>
        public string MesModel
        {
            get { return this.GetProperty(MesModelProperty); }
            set { this.SetProperty(MesModelProperty, value); }
        }
        #endregion

        #region mes设备名 MesDeviceName
        /// <summary>
        /// mes设备名
        /// </summary>
        [Label("mes设备名")]
        public static readonly Property<string> MesDeviceNameProperty = P<MDCDetailViewModle>.Register(e => e.MesDeviceName);

        /// <summary>
        /// mes设备名
        /// </summary>
        public string MesDeviceName
        {
            get { return this.GetProperty(MesDeviceNameProperty); }
            set { this.SetProperty(MesDeviceNameProperty, value); }
        }
        #endregion
    }
}
