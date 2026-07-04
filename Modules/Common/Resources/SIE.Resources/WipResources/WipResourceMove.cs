using SIE.Domain;
using SIE.Domain.Query;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.CalendarSchemes;
using System;

namespace SIE.Resources.WipResources
{
    /// <summary>
    /// 过站产线
    /// </summary>
    [RootEntity, Serializable]
    [Label("过站产线")]
    public partial class WipResourceMove : Entity<double>
    {
        #region 资源编号 Code
        /// <summary>
        /// 资源编号
        /// </summary>
        [Label("资源编号")]
        public static readonly Property<string> CodeProperty = P<WipResourceMove>.Register(e => e.Code);

        /// <summary>
        /// 资源编号
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 资源名称 Name
        /// <summary>
        /// 资源名称
        /// </summary>
        [Label("资源名称")]
        public static readonly Property<string> NameProperty = P<WipResourceMove>.Register(e => e.Name);

        /// <summary>
        /// 资源名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 日历方案 Scheme
        /// <summary>
        /// 日历方案Id
        /// </summary>
        [Label("日历方案")]
        public static readonly IRefIdProperty SchemeIdProperty = P<WipResourceMove>.RegisterRefId(e => e.SchemeId, ReferenceType.Normal);

        /// <summary>
        /// 日历方案Id
        /// </summary>
        public double SchemeId
        {
            get { return (double)GetRefId(SchemeIdProperty); }
            set { SetRefId(SchemeIdProperty, value); }
        }

        /// <summary>
        /// 日历方案
        /// </summary>
        public static readonly RefEntityProperty<CalendarScheme> SchemeProperty = P<WipResourceMove>.RegisterRef(e => e.Scheme, SchemeIdProperty);

        /// <summary>
        /// 日历方案
        /// </summary>
        public CalendarScheme Scheme
        {
            get { return GetRefEntity(SchemeProperty); }
            set { SetRefEntity(SchemeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 过站产线 实体配置
    /// </summary>
    internal class WipResourceMoveConfig : EntityConfig<WipResourceMove>
    {
        /// <summary>
        /// 实体元数据配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Func<IQuery> view = () => DB.Query<WipResource>().Select((a) => new
            {
                a.Id,
                a.Code,
                a.Name,
                Scheme_Id = a.SchemeId
            }).ToQuery();
            Meta.MapView(view).MapAllProperties();
        }
    }
}
