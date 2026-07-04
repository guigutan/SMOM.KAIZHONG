using SIE.Common.Configs;
using SIE.Domain;
using SIE.MetaModel;

namespace SIE.AbnormalInfo.AbnormalMonitors.AbnormalMonitorTasks
{
    /// <summary>
    /// 异常任务扩展
    /// </summary>
    [SIE.ManagedProperty.CompiledPropertyDeclarer]
	public static class AbnormalMonitorTaskExt
    {
		#region string EntityType (来源类型)
		/// <summary>
		/// 属性注释 扩展属性。
		/// </summary>
		[MaxLength(200)]
		public static readonly Property<string> EntityTypeProperty =
			P<AbnormalMonitorTask>.RegisterExtension<string>("EntityType", typeof(AbnormalMonitorTaskExt));

		/// <summary>
		/// 获取 属性注释 属性的值。
		/// </summary>
		/// <param name="me">要获取扩展属性值的对象。</param>
		public static string GetEntityType(this AbnormalMonitorTask me)
		{
			return me.GetProperty(EntityTypeProperty);
		}

		/// <summary>
		/// 设置 属性注释 属性的值。
		/// </summary>
		/// <param name="me">要设置扩展属性值的对象。</param>
		/// <param name="value">设置的值。</param>
		public static void SetEntityType(this AbnormalMonitorTask me, string value)
		{
			me.SetProperty(EntityTypeProperty, value);
		}
		#endregion

		#region string ExtName (扩展名称)
		/// <summary>
		/// 属性注释 扩展属性。
		/// </summary>
		[MaxLength(200)]
		public static readonly Property<string> ExtNameProperty =
			P<AbnormalMonitorTask>.RegisterExtension<string>("ExtName", typeof(AbnormalMonitorTaskExt));

		/// <summary>
		/// 获取 属性注释 属性的值。
		/// </summary>
		/// <param name="me">要获取扩展属性值的对象。</param>
		public static string GetExtName(this AbnormalMonitorTask me)
		{
			return me.GetProperty(ExtNameProperty);
		}

		/// <summary>
		/// 设置 属性注释 属性的值。
		/// </summary>
		/// <param name="me">要设置扩展属性值的对象。</param>
		/// <param name="value">设置的值。</param>
		public static void SetExtName(this AbnormalMonitorTask me, string value)
		{
			me.SetProperty(ExtNameProperty, value);
		}
		#endregion

		#region string TypeName (扩展实体类型)
		/// <summary>
		/// 扩展实体类型 扩展属性。
		/// </summary>
		[MaxLength(200)]
		public static readonly Property<string> TypeNameProperty =
			P<AbnormalMonitorTask>.RegisterExtension<string>("TypeName", typeof(AbnormalMonitorTaskExt));

		/// <summary>
		/// 获取 扩展实体类型 属性的值。
		/// </summary>
		/// <param name="me">要获取扩展属性值的对象。</param>
		public static string GetTypeName(this AbnormalMonitorTask me)
		{
			return me.GetProperty(TypeNameProperty);
		}

		/// <summary>
		/// 设置 扩展实体类型 属性的值。
		/// </summary>
		/// <param name="me">要设置扩展属性值的对象。</param>
		/// <param name="value">设置的值。</param>
		public static void SetTypeName(this AbnormalMonitorTask me, string value)
		{
			me.SetProperty(TypeNameProperty, value);
		}
		#endregion

		#region string Value (属性注释)
		/// <summary>
		/// 属性注释 扩展属性。
		/// </summary>
		[MaxLength(4000)]
		public static readonly Property<string> ValueProperty =
			P<AbnormalMonitorTask>.RegisterExtension<string>("Value", typeof(AbnormalMonitorTaskExt));

		/// <summary>
		/// 获取 属性注释 属性的值。
		/// </summary>
		/// <param name="me">要获取扩展属性值的对象。</param>
		public static string GetConfigValue(this AbnormalMonitorTask me)
		{
			return me.GetProperty(ValueProperty);
		}

		/// <summary>
		/// 设置 属性注释 属性的值。
		/// </summary>
		/// <param name="me">要设置扩展属性值的对象。</param>
		/// <param name="value">设置的值。</param>
		public static void SetConfigValue(this AbnormalMonitorTask me, string value)
		{
			me.SetProperty(ValueProperty, value);
		}
		#endregion

    }

    /// <summary>
    ///  实体配置
    /// </summary>
    internal class AbnormalMonitorTaskConfig : EntityConfig<AbnormalMonitorTask>
    {
        protected override void ConfigMeta()
        {
            Meta.MapTable("ABNORMAL_MONITOR_TASK").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
