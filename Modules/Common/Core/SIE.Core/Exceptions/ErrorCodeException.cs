using SIE.Domain.Validation;
using SIE.ObjectModel;
using System;
using System.Reflection;
using System.Runtime.Serialization;

namespace SIE.Core.Exceptions
{
    /// <summary>
    /// 错误码异常
    /// </summary>
    [Serializable]
    public class ErrorCodeException<T> : ValidationException where T : Enum
    {
        /// <summary>
        /// 错误码
        /// </summary>
        protected T ErrorCode { get; }

        /// <summary>
        /// 参数集合
        /// </summary>
        protected object[] ParamList { get; }

        /// <summary>
        /// 直接指定错误信息
        /// </summary>
        private string _errorMsg { get; }

        /// <summary>
        /// 错误码验证 
        /// </summary>
        /// <param name="errCode">泛型枚举的错误码</param>
        public ErrorCodeException(T errCode)
        {
            ErrorCode = errCode;
        }

        /// <summary>
        /// 错误码验证 
        /// </summary>
        /// <param name="errCode">泛型枚举的错误码</param>
        /// <param name="paramList">参数</param>
        public ErrorCodeException(T errCode, params object[] paramList)
        {
            ErrorCode = errCode;
            ParamList = paramList;
        }

        /// <summary>
        /// 错误码验证 
        /// </summary>
        /// <param name="msg">异常信息</param>
        /// <param name="paramList">参数</param>
        public ErrorCodeException(string msg, params object[] paramList)
        {
            _errorMsg = msg;
            ParamList = paramList;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="info"></param>
        /// <param name="context"></param>
        protected ErrorCodeException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }

        /// <summary>
        /// 重写异常信息
        /// </summary>
        public override string Message
        {
            get
            {
                var type = typeof(T);
                var msgType = type.GetCustomAttribute<LabelAttribute>()?.Label;
                string msg;
                if (ParamList?.Length > 0)
                {
                    //有参数
                    if (_errorMsg == null)  //不指定异常信息，即指定错误码
                        msg = $"[{msgType}{Convert.ToInt32(ErrorCode)}]{ErrorCode.ToLabel()}";
                    else   //不指定错误码
                        msg = $"[{msgType}]{_errorMsg}"; 
                    return msg.L10nFormat(ParamList);
                }
                else
                {
                    //没有参数
                    if (_errorMsg == null)  //不指定异常信息，即指定错误码
                        return $"[{msgType}{Convert.ToInt32(ErrorCode)}]{ErrorCode.ToLabel()}".L10N();
                    else   //不指定错误码
                        return $"[{msgType}]{_errorMsg}".L10N();
                }
            }
        }
    }
}
