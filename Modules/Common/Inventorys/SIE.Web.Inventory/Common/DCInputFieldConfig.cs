using SIE.Web.Json;
using SIE.Web.MetaModelPortal.ClientMetaModel.UIBlock.Form;

namespace SIE.Web.Inventory.Common
{
    /// <summary>
    /// D/C输入编辑器属性配置
    /// </summary>
    public class DCInputFieldConfig : TextButtonFieldConfig
    {
        /// <summary>
        /// 年周
        /// </summary>
        public bool IsYearWeek { get; set; } = true;

        /// <summary>
        /// 周年
        /// </summary>
        public bool IsWeekYear { get; set; }

        /// <summary>
        /// 年月日
        /// </summary>
        public bool IsYearMonthDay { get; set; }

        /// <summary>
        /// 数据转换成Json
        /// </summary>
        /// <param name="json">Json数据</param>
        protected override void ToJson(LiteJsonWriter json)
        {
            json.WritePropertyIf("IsYearWeek", IsYearWeek);
            json.WritePropertyIf("IsWeekYear", IsWeekYear);
            json.WritePropertyIf("IsYearMonthDay", IsYearMonthDay);
            base.ToJson(json);
        }
    }
}
