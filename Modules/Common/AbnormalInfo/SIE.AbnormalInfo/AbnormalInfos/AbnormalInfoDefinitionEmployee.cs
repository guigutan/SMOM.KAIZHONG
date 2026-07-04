using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;

namespace SIE.AbnormalInfo.AbnormalInfos
{
    /// <summary>
    /// 异常定义与员工关系
    /// </summary>
    [ChildEntity, Serializable]
    [Label("异常定义与员工关系")]
    public class AbnormalInfoDefinitionEmployee : DataEntity
    {
        #region 异常处理人 Handler
        /// <summary>
        /// 异常处理人Id
        /// </summary>
        public static readonly IRefIdProperty HandlerIdProperty = P<AbnormalInfoDefinitionEmployee>.RegisterRefId(e => e.HandlerId, ReferenceType.Normal);

        /// <summary>
        /// 异常处理人Id
        /// </summary>
        public double HandlerId
        {
            get { return (double)GetRefId(HandlerIdProperty); }
            set { SetRefId(HandlerIdProperty, value); }
        }

        /// <summary>
        /// 异常处理人
        /// </summary>
        public static readonly RefEntityProperty<Employee> HandlerProperty = P<AbnormalInfoDefinitionEmployee>.RegisterRef(e => e.Handler, HandlerIdProperty);

        /// <summary>
        /// 异常处理人
        /// </summary>
        public Employee Handler
        {
            get { return GetRefEntity(HandlerProperty); }
            set { SetRefEntity(HandlerProperty, value); }
        }
        #endregion

        #region 异常定义 AbnormalInfoDefinition
        /// <summary>
        /// 异常定义Id
        /// </summary>
        [Label("异常定义")]
        public static readonly IRefIdProperty AbnormalInfoDefinitionIdProperty =
            P<AbnormalInfoDefinitionEmployee>.RegisterRefId(e => e.AbnormalInfoDefinitionId, ReferenceType.Parent);

        /// <summary>
        /// 异常定义Id
        /// </summary>
        public double AbnormalInfoDefinitionId
        {
            get { return (double)this.GetRefId(AbnormalInfoDefinitionIdProperty); }
            set { this.SetRefId(AbnormalInfoDefinitionIdProperty, value); }
        }

        /// <summary>
        /// 异常定义
        /// </summary>
        public static readonly RefEntityProperty<AbnormalInfoDefinition> AbnormalInfoDefinitionProperty =
            P<AbnormalInfoDefinitionEmployee>.RegisterRef(e => e.AbnormalInfoDefinition, AbnormalInfoDefinitionIdProperty);

        /// <summary>
        /// 异常定义
        /// </summary>
        public AbnormalInfoDefinition AbnormalInfoDefinition
        {
            get { return this.GetRefEntity(AbnormalInfoDefinitionProperty); }
            set { this.SetRefEntity(AbnormalInfoDefinitionProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 异常处理人编码 Code
        /// <summary>
        /// 异常处理人编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<AbnormalInfoDefinitionEmployee>.RegisterView(e => e.Code, p => p.Handler.Code);

        /// <summary>
        /// 异常处理人编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
        }
        #endregion


        #region 异常处理人名称 Name
        /// <summary>
        /// 异常处理人名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<AbnormalInfoDefinitionEmployee>.RegisterView(e => e.Name, p => p.Handler.Name);

        /// <summary>
        /// 异常处理人名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
        }
        #endregion
        #endregion

    }

    /// <summary>
    /// 异常定义与员工关系 实体配置
    /// </summary>
    internal class FailedListAuditEmployeeConfig : EntityConfig<AbnormalInfoDefinitionEmployee>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QMS_ABR_DEF_EMP").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
