using SIE.Web.ClientMetaModel;
using SIE.Web.Json;

namespace SIE.Web.Tech.Routings.Editors
{
    /// <summary>
    /// 工艺路线显示编辑器配置
    /// </summary>
    public class RoutingEditorConfig : FieldConfig
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public RoutingEditorConfig()
        {
            XType = "routingDisplayEditor";
        }

        /// <summary>
        /// 画布div id，所有编辑器该属性必须指定且不能重复
        /// </summary>
        public string Canvas { get; set; }

        /// <summary>
        /// 编辑器绑定属性名称，即当前实体对象的工艺路线布局Id，必须制定
        /// </summary>
        public string Property { get; set; }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="json">json对象</param>
        protected override void ToJson(LiteJsonWriter json)
        {
            json.WritePropertyIf("canvas", Canvas);
            json.WritePropertyIf("property", Property);
            base.ToJson(json);
        }
    }
}