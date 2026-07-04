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

namespace SIE.KZ.Base.Interfaces
{
    /// <summary>
    /// 基础数据接口信息
    /// </summary>
    [RootEntity, Serializable]
    [CriteriaQuery]
    [Label("基础数据接口信息")]
    public class InfMapping : DataEntity
    {
        #region 接口名 InfType
        /// <summary>
        /// 接口名
        /// </summary>
        [Label("接口名")]
        [NotDuplicate]
        public static readonly Property<InfType> InfTypeProperty = P<InfMapping>.Register(e => e.InfType);

        /// <summary>
        /// 接口名
        /// </summary>
        public InfType InfType
        {
            get { return this.GetProperty(InfTypeProperty); }
            set { this.SetProperty(InfTypeProperty, value); }
        }
        #endregion 接口名 InfType

        #region 接口代码 InfCode
        /// <summary>
        /// 接口代码
        /// </summary>
        [Label("接口代码")]
        public static readonly Property<string> InfCodeProperty = P<InfMapping>.Register(e => e.InfCode);

        /// <summary>
        /// 接口代码
        /// </summary>
        public string InfCode
        {
            get { return this.GetProperty(InfCodeProperty); }
            set { this.SetProperty(InfCodeProperty, value); }
        }
        #endregion 接口代码 InfCode

        #region 控制器名 ApiType
        /// <summary>
        /// 控制器名
        /// </summary>
        [Label("控制器名")]
        public static readonly Property<string> ApiTypeProperty = P<InfMapping>.Register(e => e.ApiType);

        /// <summary>
        /// 控制器名
        /// </summary>
        public string ApiType
        {
            get { return this.GetProperty(ApiTypeProperty); }
            set { this.SetProperty(ApiTypeProperty, value); }
        }
        #endregion

        #region 方法名 Method
        /// <summary>
        /// 方法名
        /// </summary>
        [Label("方法名")]
        public static readonly Property<string> MethodProperty = P<InfMapping>.Register(e => e.Method);

        /// <summary>
        /// 方法名
        /// </summary>
        public string Method
        {
            get { return this.GetProperty(MethodProperty); }
            set { this.SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 备注 Remark
        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> RemarkProperty = P<InfMapping>.Register(e => e.Remark);

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return GetProperty(RemarkProperty); }
            set { SetProperty(RemarkProperty, value); }
        }

        #endregion 备注 Remark
    }

    /// <summary>
    /// 基础数据接口信息配置 实体配置
    /// </summary>
    internal class InfMappingConfig : EntityConfig<InfMapping>
    {
        protected override void AddValidations(IValidationDeclarer rules)
        {
            base.AddValidations(rules);
            rules.AddRule(InfMapping.InfTypeProperty, new NotDuplicateRule());
            rules.AddRule(InfMapping.InfCodeProperty, new RequiredRule());
            rules.AddRule(InfMapping.InfCodeProperty, new NotDuplicateRule());
        }

        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("Inf_Map").MapAllProperties();
            Meta.Property(InfMapping.RemarkProperty).ColumnMeta.HasLength("800");
            Meta.EnablePhantoms();
        }
    }
}
