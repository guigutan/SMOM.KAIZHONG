using SIE.Domain;
using SIE.Domain.Validation;
using SIE.KZ.Base.Interfaces.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.SmomControl
{
    /// <summary>
    /// GUID与工厂关系
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("GUID与工厂关系")]
    public class GuidFactoryRelastion : DataEntity
    {
        #region 类型 InfType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<InfType> InfTypeProperty = P<GuidFactoryRelastion>.Register(e => e.InfType);

        /// <summary>
        /// 类型
        /// </summary>
        public InfType InfType
        {
            get { return this.GetProperty(InfTypeProperty); }
            set { this.SetProperty(InfTypeProperty, value); }
        }
        #endregion

        #region GUID Guid
        /// <summary>
        /// GUID
        /// </summary>
        [Label("GUID")]
        public static readonly Property<string> GuidProperty = P<GuidFactoryRelastion>.Register(e => e.Guid);

        /// <summary>
        /// GUID
        /// </summary>
        public string Guid
        {
            get { return this.GetProperty(GuidProperty); }
            set { this.SetProperty(GuidProperty, value); }
        }
        #endregion

        #region 来源工厂 SourceFactory
        /// <summary>
        /// 来源工厂
        /// </summary>
        [Label("来源工厂")]
        public static readonly Property<string> SourceFactoryProperty = P<GuidFactoryRelastion>.Register(e => e.SourceFactory);

        /// <summary>
        /// 来源工厂
        /// </summary>
        public string SourceFactory
        {
            get { return this.GetProperty(SourceFactoryProperty); }
            set { this.SetProperty(SourceFactoryProperty, value); }
        }
        #endregion

    }

    internal class GuidFactoryRelastionConfig:EntityConfig<GuidFactoryRelastion>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(GuidFactoryRelastion.GuidProperty, new NotDuplicateRule());
        }

        protected override void ConfigMeta()
        {
            Meta.MapTable("GUID_FACTORY_REL").MapAllProperties();
            Meta.EnableInvOrg();
            Meta.EnablePhantoms();
        }
    }
}
