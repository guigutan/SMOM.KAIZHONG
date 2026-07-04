using SIE.ObjectModel;

namespace SIE.Wpf.MES.Workbench.AlertLights
{
    /// <summary>
    /// 安灯呼叫的异常类型信息类
    /// </summary>
    public class AlertExceptionTypeElement : ObservableObject
    {
        /// <summary>
        /// 无参构造方法
        /// </summary>
        public AlertExceptionTypeElement()
        {
        }

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="expTypeId">异常类型Id</param>
        /// <param name="expTypeCode">异常类型编码</param>
        /// <param name="expTypeName">异常类型名称</param>
        public AlertExceptionTypeElement(double expTypeId, string expTypeCode, string expTypeName)
        {
            this.ExpTypeId = expTypeId;
            this.ExpTypeCode = expTypeCode;
            this.ExpTypeName = expTypeName;
        }

        /// <summary>
        /// 异常类型Id
        /// </summary>
        public double ExpTypeId
        {
            get { return GetProperty<double>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 异常类型编码
        /// </summary>
        public string ExpTypeCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        ///  异常类型名称
        /// </summary>
        public string ExpTypeName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /*/// <summary>
        /// 呼叫类型的编码
        /// </summary>
        public string CallTypeCode
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 呼叫类型的名称
        /// </summary>
        public string CallTypeName
        {
            get { return GetProperty<string>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 呼叫类型
        /// </summary>
        public AlertCallType CallType
        {
            get { return GetProperty<AlertCallType>(); }
            set { SetProperty(value); }
        }*/
    }
}
