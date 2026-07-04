using SIE.Utils;
using SIE.Web.ClientMetaModel;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Dock
{
    /// <summary>
    /// 下拉列表筛选编辑器
    /// </summary>
    public class SelectUseHoursConfig : ComboBoxConfig
    {
        /// <summary>
        /// 初始化数据
        /// </summary>
        [Obsolete("已过时")]
        private void InitStore()
        {
            List<UseHourModel> data = new List<UseHourModel>();

            for (int i = 0; i < 47; i++)
            {
                data.Insert(i, new UseHourModel() { Text = ((i + 1) * 0.5).ToString("f1"), Value = (i + 1) * 0.5 });
            }

            if (AllowBlank)
                data.Insert(0, new UseHourModel() { Text = "", Value = null });
            this.Store = new ArrayStoreConfig
            {
                Fields = new string[] { "text", "value" },
                Data = data.ToArray()
            };
        }

        /// <summary>
        /// 序列化
        /// </summary>
        /// <param name="json"></param>
        protected override void ToJson(LiteJsonWriter json)
        {
            InitStore();
            base.ToJson(json);
        }
    }

    //
    // 摘要:
    //     枚举
    public class UseHourModel : JsonModel
    {
        //
        // 摘要:
        //     text
        public string Text { get; set; }

        //
        // 摘要:
        //     value
        public double? Value { get; set; }

        //
        // 摘要:
        //     toJson
        //
        // 参数:
        //   json:
        protected override void ToJson(LiteJsonWriter json)
        {
            json.WriteProperty("text", Text);
            json.WriteProperty("value", Value);
        }
    }
}
