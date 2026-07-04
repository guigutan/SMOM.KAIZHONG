using SIE.Fixtures;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Fixtures.Models.Commands
{
    /// <summary>
    /// 同步工治具编码
    /// </summary>
    [JsCommand("SIE.Web.Fixtures.Models.Commands.SyncFixtureEncodeCommand")]
    public class SyncFixtureEncodeCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行同步工治具编码
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>执行结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<SyncFixtureEncodeArgsData>();
            RT.Service.Resolve<CoreFixtureController>().SyncFixtureEncode(data.IsCompatible, data.FixtureEncodeIdList);
            return true;
        }
    }

    /// <summary>
    /// 同步工治具编码参数类
    /// </summary>
    public class SyncFixtureEncodeArgsData
    {
        /// <summary>
        /// 是否兼容
        /// </summary>
        public bool IsCompatible { get; set; }

        /// <summary>
        /// 工治具编码Id列表
        /// </summary>
        public List<double> FixtureEncodeIdList { get; set; }
    }
}
