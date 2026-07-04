using SIE.Kit.MES.Storages;
using SIE.Web.Command;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Kit.MES.Storages.Commands
{
    /// <summary>
    /// 产线物料货位属性值保存命令
    /// </summary>
    [JsCommand("SIE.Web.Kit.MES.Storages.Commands.ItemStoragePropertySaveCommand")]
    public class ItemStoragePropertySaveCommand : SaveCommand
    {
        /// <summary>
        /// 产线物料货位属性值保存执行方法
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>保存结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var outData = args.Data.ToJsonObject<ItemStorageOutData>();
            if (outData.DetailList == null || !outData.DetailList.Any())
                outData.DetailList = new List<ItemStoragePropertyValue>();
            RT.Service.Resolve<StorageController>().SaveItemStoragePropertys(outData.DetailList, outData.DetailId);
            return "保存成功";
        }
    }

    /// <summary>
    /// 属性值信息
    /// </summary>
    [Serializable]
    public class ItemStorageOutData
    {
        /// <summary>
        /// 列表行id
        /// </summary>
        public double DetailId { get; set; }

        /// <summary>
        /// 属性值列表
        /// </summary>
        public List<ItemStoragePropertyValue> DetailList { get; set; }
    }
}
