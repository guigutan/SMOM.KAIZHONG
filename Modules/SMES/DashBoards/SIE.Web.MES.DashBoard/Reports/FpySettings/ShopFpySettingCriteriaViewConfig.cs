using SIE.Domain;
using SIE.MES.DashBoard.Reports.FpySettings;
using SIE.Resources.WipResources;
using System.Collections.Generic;

namespace SIE.Web.MES.DashBoard.Reports.FpySettings
{
    /// <summary>
    /// 车间直通率设置查询实体视图配置
    /// </summary>
    [ManagedProperty.CompiledPropertyDeclarer]
    public class ShopFpySettingCriteriaViewConfig : WebViewConfig<ShopFpySettingCriteria>
    {
        #region 产线清空 ClearLine
        /// <summary>
        /// 产线清空
        /// </summary> 
        public static readonly Property<bool> ClearLineProperty = P<ShopFpySettingCriteria>.RegisterExtensionReadOnly("ClearLine", typeof(ShopFpySettingCriteriaViewConfig),
            GetClearLine, ShopFpySettingCriteria.ShopProperty);

        /// <summary>
        /// 产线清空
        /// </summary>
        public static bool GetClearLine(ShopFpySettingCriteria me)
        {
            me.Resource = null;
            return false;
        }
        #endregion

        ///<summary>
        /// 配置视图
        /// </summary>
        protected override void ConfigView()
        {
            View.Property(p => p.Shop).UseResourceWorkShopEditor().Show();
            View.Property(p => p.Resource).UseDataSource((e, c, r) =>
            {
                var setting = e as ShopFpySettingCriteria;
                var stateList = new List<ResourceState>() { ResourceState.Actived, ResourceState.Stop, ResourceState.Unused };
                if (setting?.Shop != null)
                {
                    return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, setting.ShopId.Value, c, r);
                }

                return RT.Service.Resolve<WipResourceController>().GetWipResources(stateList, new List<SyncSourceType>() { SyncSourceType.Enterprise }, c, r);
            }).UsePagingLookUpEditor().Show().Readonly(ClearLineProperty);
        }
    }
}
