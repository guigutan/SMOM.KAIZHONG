using SIE.EMS.DevicePurs;
using SIE.Equipments.Enums;
using SIE.Utils;
using SIE.Web.ClientMetaModel;
using System.Collections.Generic;
using System.Linq;

namespace SIE.Web.EMS.Purchases._Extensions_
{
    /// <summary>
    /// 采购对象的编辑器配置
    /// </summary>
    public class PurchaseObjectBoxConfig : EnumBoxConfig
    {
        /// <summary>
        /// 过滤枚举
        /// </summary>
        /// <param name="models">枚举值</param>
        /// <returns>枚举值</returns>
        public override List<EnumViewModel> FilterEnum(List<EnumViewModel> models)
        {
            var newModels = new List<EnumViewModel>();
            var list = RT.Service.Resolve<DevicePurController>().GetUserPurchaseObjects(RT.Identity.UserId);
            foreach (var item in models)
            {
                if (list.Any(p => p == (PurchaseObjectType)item.EnumValue))
                {
                    newModels.Add(item);
                }
            }
            return newModels;
        }
    }
}
