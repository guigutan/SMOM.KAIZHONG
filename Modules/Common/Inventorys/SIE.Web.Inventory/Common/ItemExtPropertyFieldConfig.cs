using SIE.Inventory.Commom;
using SIE.Web.Json;
using SIE.Web.MetaModelPortal.ClientMetaModel.UIBlock.Form;
using System;

namespace SIE.Web.Inventory.Common
{
    /// <summary>
    /// 物料扩展属性栏位配置
    /// </summary>
    public class ItemExtPropertyFieldConfig : TextButtonFieldConfig
    {
        /// <summary>
        /// 设置当前实体物料Id或产品Id的属性名，默认是ItemId
        /// </summary>
        public string ItemIdField { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public ItemExtPropFunctionType FunctionType { get; set; }

        /// <summary>
        /// 是否校验每个选项都必须有值
        /// </summary>
        public bool IsAllRequired { get; set; }

        /// <summary>
        /// 保存到数据库的字段
        /// </summary>
        public string DbField { get; set; }

        /// <summary>
        /// 确认回调查询器
        /// </summary>
        public string DataQuery { get; set; }

        /// <summary>
        /// 确认回调方法
        /// </summary>
        public string DataQueryMethod { get; set; }

        /// <summary>
        /// 转换Json数据
        /// </summary>
        /// <param name="json">Json数据</param>
        protected override void ToJson(LiteJsonWriter json)
        {
            json.WritePropertyIf("ItemIdField", ItemIdField.IsNullOrEmpty() ? "ItemId" : ItemIdField);
            json.WritePropertyIf("FunctionType", (int)FunctionType);
            json.WritePropertyIf("IsAllRequired", IsAllRequired);
            json.WritePropertyIf("DbField", DbField);
            json.WritePropertyIf("DataQuery", DataQuery);
            json.WritePropertyIf("DataQueryMethod", DataQueryMethod);
            base.ToJson(json);
        }
    }
}
