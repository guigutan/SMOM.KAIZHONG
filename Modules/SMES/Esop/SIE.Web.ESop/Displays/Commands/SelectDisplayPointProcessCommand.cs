using SIE.Domain;
using SIE.ESop.Displays;
using SIE.Web.Command;
using System;
using System.Collections.Generic;

namespace SIE.Web.ESop.Displays.Commands
{
    /// <summary>
    /// 选择工序命令
    /// </summary>
    public class SelectDisplayPointProcessCommand : ViewCommand
    {
        /// <summary>
        /// 选择工序命令执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">作用域</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var processDisplayPointList = args.Data.ToJsonObject<List<DisplayPointProcess>>();
            Check.NotNullOrEmpty(processDisplayPointList, nameof(processDisplayPointList));
            if (processDisplayPointList == null || processDisplayPointList.Count == 0)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(nameof(processDisplayPointList)));
            }

            foreach (var item in processDisplayPointList)
            {
                var processDisplayPoint = new DisplayPointProcess();
                processDisplayPoint.DisplayPointId = item.DisplayPointId;
                processDisplayPoint.ProcessId = item.ProcessId;
                RF.Save(processDisplayPoint);
            }

            return true;
        }
    }
}
