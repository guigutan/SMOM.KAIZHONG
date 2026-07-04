using SIE.Utils;
using SIE.Web.ClientMetaModel;
using SIE.Web.Json;
using System.Linq;

namespace SIE.Web.Inventory.Common
{
    /// <summary>
    /// 多分类过滤枚举配置
    /// </summary>
    public class MultiFilterEnumBoxConfig : EnumBoxConfig
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorName = "WmsMultiFilterEnumBoxConfig";

        /// <summary>
        /// 分类过滤条件
        /// </summary>
        public string[] Filters { get; set; }

        /// <summary>
        /// 初始化数据源
        /// </summary>
        [System.Obsolete("已过时")]
        public override void InitStore()
        {
            Check.NotNull(EnumType, nameof(EnumType));
            var models = EnumViewModel.GetByEnumType(EnumType);
            if (Filters.Length > 0)
            {
                models = models.Where(p => Filters.Contains(p.Category)).ToList();
            }

            var data = models.Select(vm => new EnumModel
            {
                Text = vm.Label,
                Value = (int)(object)vm.EnumValue
            }).ToList();
            if (AllowBlank)
                data.Insert(0, new EnumModel() { Text = string.Empty, Value = null });
            this.Store = new ArrayStoreConfig
            {
                Fields = new string[] { "text", "value" },
                Data = data.ToArray()
            };
        }
    }
}