using SIE.Equipments.EquipAccountCarrieds;
using System.Collections.Generic;

namespace SIE.Web.Equipments.EquipAccountCarrieds
{
    /// <summary>
    /// 设备台账-载位视图
    /// </summary>
    public class EquipAccountCarriedViewConfig : WebViewConfig<EquipAccountCarried>
    {
        /// <summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            base.ConfigView();
        }

        /// <summary>
        /// 列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();

            View.Property(p => p.WipResouceId).UsePagingLookUpEditor((m, e) =>
            {
                var keyValues = new Dictionary<string, string>();
                keyValues.Add(nameof(e.WipResouceName), nameof(e.WipResouce.Name));                
                m.DicLinkField = keyValues;
            });

            View.Property(p => p.WipResouceName);
            View.Property(p => p.Code);
            View.Property(p => p.CarriedType);
            View.Property(p => p.CurCarried).Readonly();
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.CarriedType);
            View.Property(p => p.CurCarried);
        }

        /// <summary>
        /// 查询条件
        /// </summary>
        protected override void ConfigQueryView()
        {
            View.Property(p => p.WipResouceId);
            View.Property(p => p.Code);
            View.Property(p => p.CarriedType);
        }
    }
}
