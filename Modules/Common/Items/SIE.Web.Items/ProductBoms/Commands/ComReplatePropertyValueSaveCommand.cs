using SIE.Items;
using SIE.Items.ProductBoms;
using SIE.Security;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Items.ProductBoms.Commands
{
    /// <summary>
    /// BOM明细属性值保存命令
    /// </summary>
    [JsCommand("SIE.Web.Items.ProductBoms.Commands.ComReplatePropertyValueSaveCommand")]
    [AllowAnonymous]
    public class ComReplatePropertyValueSaveCommand : SaveCommand
    {
        /// <summary>
        /// BOM明细属性值保存执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>BOM明细属性值保存结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var outData = args.Data.ToJsonObject<ComReplatePropertyValueData>();
            if (outData.DetailList == null || !outData.DetailList.Any())
                outData.DetailList = new List<CombinationReplatePropertyValue>();
            RT.Service.Resolve<ProductBomController>().SaveComReplatePropertyValues(outData.DetailList, outData.DetailId);
            return "保存成功";
        }
    }

    /// <summary>
    /// 组合替代 选择的物料属性
    /// </summary>
    [Serializable]
    public class ComReplatePropertyValueData
    {
        /// <summary>
        /// 
        /// </summary>
        public double DetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CombinationReplatePropertyValue> DetailList { get; set; }
    }
}