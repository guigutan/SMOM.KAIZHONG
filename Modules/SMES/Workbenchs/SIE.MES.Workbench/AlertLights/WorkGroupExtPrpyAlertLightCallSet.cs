using SIE.Domain;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.Resources.Employees;

namespace SIE.MES.Workbench.AlertLights
{
    /// <summary>
    /// 托管属性类声明
    /// 作为属性注册到类WorkGroup中
    /// </summary>
    [CompiledPropertyDeclarer]
    public class WorkGroupExtPrpyAlertLightCallSet
    {
        #region EntityList<EmpCallSetting> EmpCallSettingExt (安灯预警员工设置)
        /// <summary>
        /// 安灯预警员工设置 扩展属性。
        /// </summary>
        public static ListProperty<EntityList<EmpCallSetting>> EmpCallSettingExtProperty { get; }
            = P<WorkGroup>.RegisterExtensionList<EntityList<EmpCallSetting>>("EmpCallSettingExt", typeof(WorkGroupExtPrpyAlertLightCallSet));

        /// <summary>
        /// 获取 安灯预警员工设置 属性的值。
        /// </summary>
        /// <param name="me">要获取扩展属性值的对象。</param>
        /// <returns>员工呼叫设置实体</returns>
        public static EntityList<EmpCallSetting> GetEmpCallSettingExt(WorkGroup me)
        {
            return me.GetLazyList(EmpCallSettingExtProperty) as EntityList<EmpCallSetting>;
            ////return (EntityList<EmpCallSetting>)me.GetProperty(EmpCallSettingExtProperty);
        }
        #endregion

        /// <summary>
        /// 实体配置
        /// </summary>
        internal class EmpCallSettingExtConfig : EntityConfig<WorkGroup>
        {
            /// <summary>
            /// 员工呼叫设置扩展视图配置
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.Property(EmpCallSettingExtProperty).DontMapColumn();
            }
        }
    }
}
