using SIE.Web.ClientMetaModel;
using SIE.Web.Json;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    /// 年编辑器配置
    /// </summary>
    public class YearConfig : DateConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public YearConfig()
        {
            this.XType = "yearField";
            this.Format = "Y";
            this.AllowBlank = true;
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="json"></param>
        protected override void ToJson(LiteJsonWriter json)
        {
            base.ToJson(json);
        }
    }
}
