using SIE.ObjectModel;

namespace SIE.Wpf.MES.Workbench.AlertLights
{
    /// <summary>
    /// Andon呼叫类型信息类
    /// </summary>
    public class AlertCallTypeElement : ObservableObject
    {
        /// <summary>
        /// 呼叫类型Id
        /// </summary>
        public double CallTypeId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 呼叫类型编码(呼叫类型值)
        /// </summary>
        public string CallTypeCode { get; set; }

        /// <summary>
        /// 呼叫类型名称
        /// </summary>
        public string CallTypeName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 呼叫类型得状态
        /// false: 待呼叫、true:恢复
        /// </summary>
        public bool CallTypeState
        {
            get { return GetProperty<bool>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 呼叫类型ImgSrc
        /// </summary>
        public string CallTypeImgSrc
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 呼叫类型Lbel内容
        /// </summary>
        public string CallTypeLblContent
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }
    }
}
