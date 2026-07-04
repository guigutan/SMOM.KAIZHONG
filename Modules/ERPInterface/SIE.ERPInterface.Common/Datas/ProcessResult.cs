using SIE.Domain;
using SIE.ERPInterface.Common.Logs;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace SIE.ERPInterface.Common.Datas
{
    /// <summary>
    /// 处理结果
    /// </summary>
    [Serializable]
    public class ProcessResult
    {
        /// <summary>
        /// 结果
        /// </summary>
        public bool Result { get { return FailMsg.Count == 0; } }

        /// <summary>
        /// 最大消息长度
        /// </summary>
        public const int MAX_MSG_LEN = 1000;

        /// <summary>
        /// 概况信息
        /// </summary>
        public string HeadMsg { get; set; }

        /// <summary>
        ///  信息
        /// </summary>
        public string TailMsg { get; set; }

        /// <summary>
        /// 消息
        /// </summary>
        public string Msg
        {
            get
            {
                if (HeadMsg.IsNotEmpty())
                    return HeadMsg;
                var msg = "执行总记录[{0}]条，成功[{1}]条，失败[{2}]条。{3}".L10nFormat(TotalCount, SuccessCount, FailCount, TailMsg);
                return msg.Length > MAX_MSG_LEN ? msg.Substring(0, MAX_MSG_LEN) : msg;
            }
        }

        /// <summary>
        /// 成功消息
        /// </summary>
        public ConcurrentBag<string> SuccessMsg { get; private set; } = new ConcurrentBag<string>();

        /// <summary>
        /// 失败消息
        /// </summary>
        public ConcurrentBag<string> FailMsg { get; private set; } = new ConcurrentBag<string>();

        /// <summary>
        /// 成功记录数
        /// </summary>
        public int SuccessCount
        {
            get { return SuccessMsg.Count; }
        }

        /// <summary>
        /// 失败记录数
        /// </summary>
        public int FailCount { get { return FailMsg.Count; } }

        /// <summary>
        /// 总记录数
        /// </summary>
        public int TotalCount { get { return SuccessMsg.Count + FailMsg.Count; } }


        /// <summary>
        /// 成功的记录,事务上传记录Id-SAP凭证
        /// </summary>
        public Dictionary<double, string> SuccessSapKey { get; set; } = new Dictionary<double, string>();

        /// <summary>
        /// 添加成功消息
        /// </summary>
        public void AddSuccessMsg(string msg = "")
        {
            SuccessMsg.Add(msg);
        }

        /// <summary>
        /// 添加失败消息
        /// </summary>
        public void AddFailMsg(string msg)
        {
            FailMsg.Add(msg);
        }

        /// <summary>
        /// 添加失败消息
        /// </summary>
        public void AddFailMsg(Exception ex)
        {
            var errorMsg = "异常消息：{0}".L10nFormat(ex.Message);
            if (!FailMsg.Contains(errorMsg))
                FailMsg.Add(errorMsg);
        }

        /// <summary>
        /// 合并处理结果
        /// </summary>
        /// <param name="combineResult">处理结果</param>
        /// <returns></returns>
        public ProcessResult CombineResult(ProcessResult combineResult)
        {
            combineResult.SuccessMsg.ForEach(p => AddSuccessMsg(p));
            combineResult.FailMsg.ForEach(p => AddFailMsg(p));
            return this;
        }
    }
}
