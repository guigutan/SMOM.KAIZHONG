using SIE.Web.ClientMetaModel;
using SIE.Web.Json;

namespace SIE.Web.Core
{
    /// <summary>
    /// 年月编辑器配置
    /// </summary>
    public class YearMonthConfig : FieldConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public YearMonthConfig()
        {
            this.XType = "yearMonthField";           
            this.Format = "Y-m";
        }

        /// <summary>
        /// 日期格式
        /// </summary>
        public string Format { get; set; }
         
        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="json"></param>
        protected override void ToJson(LiteJsonWriter json)
        {           
            json.WritePropertyIf("format", Format);            
            base.ToJson(json);
        }
    }
}
