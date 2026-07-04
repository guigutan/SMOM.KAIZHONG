using SIE.MetaModel.View;
using SIE.Web.ClientMetaModel;
using SIE.Web.Json;
using SIE.Web.MetaModelPortal.ClientMetaModel.UIBlock.Form;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.Web.Items.Common
{

    /// <summary>
    /// 物料单位属性栏位配置
    /// </summary>
    public class ItemUintFieldConfig : NumberfieldConfig
    {

        public ItemUintFieldConfig()
        {
            this.XType = "ItemUnitEditor";
        }
        /// <summary>
        /// 设置当前实体物料Id或产品Id的属性名，默认是ItemId
        /// </summary>
        public string ItemIdField { get; set; }


        /// <summary>
        /// 设置当前实体物料单位Id或产品单位Id的属性名，默认是ItemUnitId
        /// </summary>
        public string ItemUnitFileld { get; set; }


        /// <summary>
        /// 最小值
        /// </summary>
        public double? MinValue { get; set; } = 0;

        /// <summary>
        /// 最大值
        /// </summary>
        public double? MaxValue { get; set; }


      

        /// <summary>
        /// 转换Json数据
        /// </summary>
        /// <param name="json">Json数据</param>
        protected override void ToJson(LiteJsonWriter json)
        {
           
            json.WritePropertyIf("ItemIdField", ItemIdField.IsNullOrEmpty() ? "ItemId" : ItemIdField);
            json.WritePropertyIf("ItemUnitFileld", ItemUnitFileld.IsNullOrEmpty() ? "" : ItemUnitFileld);
            json.WritePropertyIf("MinValue", MinValue);
            json.WritePropertyIf("MaxValue", MaxValue);
            base.ToJson(json);
        }
    }
}
