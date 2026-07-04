using SIE.Items;
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
    [JsCommand("SIE.Web.Items.ProductBoms.Commands.PropertyValueSaveCommand")]
    [AllowAnonymous]
    public class PropertyValueSaveCommand : SaveCommand
    {
        /// <summary>
        /// BOM明细属性值保存执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>BOM明细属性值保存结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var outData = args.Data.ToJsonObject<OutData>();
            if (outData.DetailList == null || !outData.DetailList.Any())
                outData.DetailList = new List<ProductBomDetailPropertyValue>();
            //RT.Service.Resolve<ProductBomController>().SaveProductBomDetailPropertyValues(outData.DetailList, outData.DetailId);
            return "保存成功";
        }
    }

    /// <summary>
    /// 
    /// </summary>
    [Serializable]
    public class OutData
    {
        /// <summary>
        /// 
        /// </summary>
        public double DetailId { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<ProductBomDetailPropertyValue> DetailList { get; set; }
    }
}
