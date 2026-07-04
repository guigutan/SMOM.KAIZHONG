using SIE.MetaModel.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Web.Andon.AndonAverageTime
{
    /// <summary>
    /// 已经弃用,
    /// </summary>
    public class AndonAverageTimeUITemplate : CodeBlocksTemplate
    {
        /// <summary>
        /// 已经弃用
        /// </summary>
        /// <returns></returns>
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.Andon.AndonAverageTime.Scripts.AndonAverageTimeLayout");
            return rst;
        }
    }
}