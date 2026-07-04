using SIE.Domain;
using SIE.Items;
using SIE.Web.Command;
using System;

namespace SIE.Web.Items.Items.Commands
{
    /// <summary>
    /// 添加扩展属性命令
    /// </summary>
    [JsCommand("SIE.Web.Items.Items.Commands.ItemPropertyValueAddCommand")]
    public class ItemPropertyValueAddCommand : ViewCommand
    {
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args">args</param>
        /// <param name="scope">scope</param>
        /// <returns>结果</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var data = args.Data.ToJsonObject<DataModel>();
            if (null == data)
            {
                throw new ArgumentNullException("{0}数据参数不能为空".L10nFormat(args.Data));
            }
            var itemPropertyValue = new ItemPropertyValue()
            {
                ItemId = data.ItemId,
                DefinitionId = data.DefinitionId,
                Value = data.Value,
                PropertyGroup = data.PropertyGroup
            };
            RF.Save(itemPropertyValue);
            var item = RF.GetById<Item>(data.ItemId);
            if (item != null)
            {
                item.EnableExtendProperty = true;
                RF.Save(item);
            }
            return itemPropertyValue;
        }
    }


    /// <summary>
    /// 保存数据类
    /// </summary>
    public class DataModel
    {
        /// <summary>
        /// 物料ID
        /// </summary>
        public double ItemId
        {
            get; set;
        }

        /// <summary>
        /// 物料属性定义ID
        /// </summary>
        public double DefinitionId
        {
            get; set;
        }

        /// <summary>
        /// 物料属性值
        /// </summary>
        public string Value
        {
            get; set;
        }

        /// <summary>
        /// 属性组
        /// </summary>
        public string PropertyGroup
        {
            get; set;
        }
    }
}
