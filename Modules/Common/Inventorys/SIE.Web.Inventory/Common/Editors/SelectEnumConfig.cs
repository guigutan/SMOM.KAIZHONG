using SIE.Utils;
using SIE.Web.ClientMetaModel;
using SIE.Web.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.Inventory.Common
{
    /// <summary>
    /// 枚举筛选编辑器
    /// </summary>
    public class SelectEnumConfig : EnumBoxConfig
    {
        /// <summary>
        /// 显示枚举值集合
        /// </summary>
        public List<int?> ValuesList { get; set; } = new List<int?>();

        /// <summary>
        /// 初始化数据
        /// </summary>
        [Obsolete("已过时")]
        public override void InitStore()
        {
            Check.NotNull(EnumType, nameof(EnumType));
            var models = EnumViewModel.GetByEnumType(EnumType);
            var data = models.Select(vm => new EnumModel
            {
                Text = vm.Label.L10N(),
                Value = (int)(object)vm.EnumValue
            }).ToList();

            if (ValuesList.Count > 0)
            {
                data = data.Where(p => ValuesList.Contains(p.Value)).ToList();
            }

            if (AllowBlank)
                data.Insert(0, new EnumModel() { Text = "", Value = null });
            this.Store = new ArrayStoreConfig
            {
                Fields = new string[] { "text", "value" },
                Data = data.ToArray()
            };
        }
    }
}
