using SIE.Web.ClientMetaModel;
using SIE.Web.Json;
using System;
using System.Collections.Generic;

namespace SIE.Web.Core.Configs
{
    /// <summary>
    /// 用于列表下拉选择时间（00:00-23:30）
    /// </summary>
    public class SelectTimeConfig : ComboBoxConfig
    {
        public SelectTimeConfig() 
        {
            this.QueryMode = "local";
            this.DisplayField = "text";
            this.ValueField = "text";
        }

        /// <summary>
        /// 初始化数据
        /// </summary>
        [Obsolete]
        public virtual void InitStore()
        {
            InitStores();
        }

        private void InitStores()
        {
            List<EnumModel> arr = new List<EnumModel>();
            TimeSpan time = TimeSpan.Zero;
            while (time < TimeSpan.FromHours(24))
            {
                string text = time.ToString(@"hh\:mm");
               
                arr.Add(new EnumModel { Text = text });

                time = time.Add(TimeSpan.FromMinutes(30));
            }
            arr.Add(new EnumModel { Text = "23:59" });
            this.Store = new ArrayStoreConfig
            {
                Fields = new string[] { "text" },
                Data = arr.ToArray(),
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
   
}
