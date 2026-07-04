using SIE.Web.Json;
using SIE.Web.MetaModelPortal.ClientMetaModel.UIBlock.Form;
using System;

namespace SIE.Web.Items.Common
{
    /// <summary>
    /// 物料扩展属性栏位配置
    /// </summary>
    public class ItemExtPropRecordsFieldConfig : TextButtonFieldConfig
    {
        /// <summary>
        /// 设置当前实体物料Id或产品Id的属性名，默认是ItemId
        /// </summary>
        public string ItemIdField { get; set; }

        /// <summary>
        /// 功能类型
        /// </summary>
        public string SourceEntityType { get; set; }

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
        /// 产品BOM ID字段名称（如果产品BOM ID大于0，则筛选出该BOM明细内出现过的扩展属性）（不输入则不进行筛选）
        /// </summary>
        public string ProductBomIdField { get; set; }

        /// <summary>
        /// 转换Json数据
        /// </summary>
        /// <param name="json">Json数据</param>
        protected override void ToJson(LiteJsonWriter json)
        {
            json.WritePropertyIf("ItemIdField", ItemIdField.IsNullOrEmpty() ? "ItemId" : ItemIdField);
            json.WritePropertyIf("SourceEntityType", SourceEntityType);
            json.WritePropertyIf("IsAllRequired", IsAllRequired);
            json.WritePropertyIf("DbField", DbField);
            json.WritePropertyIf("DataQuery", DataQuery);
            json.WritePropertyIf("DataQueryMethod", DataQueryMethod);
            json.WritePropertyIf("ProductBomIdField", ProductBomIdField);
            base.ToJson(json);
        }
    }
}
