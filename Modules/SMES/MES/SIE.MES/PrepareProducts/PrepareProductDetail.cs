using SIE.Domain;
using SIE.Items;
using SIE.ManagedProperty;
using SIE.MES.PrepareProducts.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Tech.Processs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.MES.PrepareProducts
{
    /// <summary>
    /// 产品产前准备设置工序子表
    /// </summary>
    [ChildEntity, Serializable]
    [Label("产品产前准备设置工序子表")]
    public class PrepareProductDetail : DataEntity
    {
        #region 工序 Process
        /// <summary>
        /// 工序Id
        /// </summary>
        [Label("工序")]
        public static readonly IRefIdProperty ProcessIdProperty =
            P<PrepareProductDetail>.RegisterRefId(e => e.ProcessId, ReferenceType.Normal);

        /// <summary>
        /// 工序Id
        /// </summary>
        public double? ProcessId
        {
            get { return (double?)this.GetRefNullableId(ProcessIdProperty); }
            set { this.SetRefNullableId(ProcessIdProperty, value); }
        }

        /// <summary>
        /// 工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ProcessProperty =
            P<PrepareProductDetail>.RegisterRef(e => e.Process, ProcessIdProperty);

        /// <summary>
        /// 工序
        /// </summary>
        public Process Process
        {
            get { return this.GetRefEntity(ProcessProperty); }
            set { this.SetRefEntity(ProcessProperty, value); }
        }
        #endregion

        #region 项目准备 PrepareProject
        /// <summary>
        /// 项目准备Id
        /// </summary>
        [Label("项目准备")]
        public static readonly IRefIdProperty PrepareProjectIdProperty =
            P<PrepareProductDetail>.RegisterRefId(e => e.PrepareProjectId, ReferenceType.Normal);

        /// <summary>
        /// 项目准备Id
        /// </summary>
        public double PrepareProjectId
        {
            get { return (double)this.GetRefId(PrepareProjectIdProperty); }
            set { this.SetRefId(PrepareProjectIdProperty, value); }
        }

        /// <summary>
        /// 项目准备
        /// </summary>
        public static readonly RefEntityProperty<PrepareProject> PrepareProjectProperty =
            P<PrepareProductDetail>.RegisterRef(e => e.PrepareProject, PrepareProjectIdProperty);

        /// <summary>
        /// 项目准备
        /// </summary>
        public PrepareProject PrepareProject
        {
            get { return this.GetRefEntity(PrepareProjectProperty); }
            set { this.SetRefEntity(PrepareProjectProperty, value); }
        }
        #endregion

        #region 产品产前准备设置 PrepareProduct
        /// <summary>
        /// 产品产前准备设置Id
        /// </summary>
        [Label("产品产前准备设置")]
        public static readonly IRefIdProperty PrepareProductIdProperty =
            P<PrepareProductDetail>.RegisterRefId(e => e.PrepareProductId, ReferenceType.Parent);

        /// <summary>
        /// 产品产前准备设置Id
        /// </summary>
        public double PrepareProductId
        {
            get { return (double)this.GetRefId(PrepareProductIdProperty); }
            set { this.SetRefId(PrepareProductIdProperty, value); }
        }

        /// <summary>
        /// 产品产前准备设置
        /// </summary>
        public static readonly RefEntityProperty<PrepareProduct> PrepareProductProperty =
            P<PrepareProductDetail>.RegisterRef(e => e.PrepareProduct, PrepareProductIdProperty);

        /// <summary>
        /// 产品产前准备设置
        /// </summary>
        public PrepareProduct PrepareProduct
        {
            get { return this.GetRefEntity(PrepareProductProperty); }
            set { this.SetRefEntity(PrepareProductProperty, value); }
        }
        #endregion

        #region 视图属性
        #region 工序名称 ProcessName
        /// <summary>
        /// 工序名称
        /// </summary>
        [Label("工序名称")]
        public static readonly Property<string> ProcessNameProperty = P<PrepareProductDetail>.RegisterView(e => e.ProcessName, p => p.Process.Name);

        /// <summary>
        /// 工序名称
        /// </summary>
        public string ProcessName
        {
            get { return this.GetProperty(ProcessNameProperty); }
        }
        #endregion

        #region 项目编码 PrepareProjectCode 
        /// <summary>
        /// 项目编码
        /// </summary>
        [Label("项目编码")]
        public static readonly Property<string> PrepareProjectCodeProperty = P<PrepareProductDetail>.RegisterView(e => e.PrepareProjectCode, p => p.PrepareProject.ProCode);

        /// <summary>
        /// 项目编码
        /// </summary>
        public string PrepareProjectCode
        {
            get { return this.GetProperty(PrepareProjectCodeProperty); }
        }
        #endregion

        #region 项目名称 PrepareProjectName
        /// <summary>
        /// 项目名称
        /// </summary>
        [Label("项目名称")]
        public static readonly Property<string> PrepareProjectNameProperty = P<PrepareProductDetail>.RegisterView(e => e.PrepareProjectName, p => p.PrepareProject.ProName);

        /// <summary>
        /// 项目名称
        /// </summary>
        public string PrepareProjectName
        {
            get { return this.GetProperty(PrepareProjectNameProperty); }
        }
        #endregion

        #region 项目类型 PrepareProjectType
        /// <summary>
        /// 项目类型
        /// </summary>
        [Label("项目类型")]
        public static readonly Property<PrepareProjectType?> PrepareProjectTypeProperty = P<PrepareProductDetail>.RegisterView(e => e.PrepareProjectType, p => p.PrepareProject.ProType);

        /// <summary>
        /// 项目类型
        /// </summary>
        public PrepareProjectType? PrepareProjectType
        {
            get { return this.GetProperty(PrepareProjectTypeProperty); }
        }
        #endregion

        #region 项目描述 PrepareProjectDesc
        /// <summary>
        /// 项目描述
        /// </summary>
        [Label("项目描述")]
        public static readonly Property<string> PrepareProjectDescProperty = P<PrepareProductDetail>.RegisterView(e => e.PrepareProjectDesc, p => p.PrepareProject.ProDesc);

        /// <summary>
        /// 项目描述
        /// </summary>
        public string PrepareProjectDesc
        {
            get { return this.GetProperty(PrepareProjectDescProperty); }
        }
        #endregion

        #endregion

        #region 不映射数据库属性
        #region 产品族id ProFamiliyId
        /// <summary>
        /// 产品族id
        /// </summary>
        [Label("产品族id")]
        public static readonly Property<double?> ProFamiliyIdProperty = P<PrepareProductDetail>.Register(e => e.ProFamiliyId);

        /// <summary>
        /// 产品族id
        /// </summary>
        public double? ProFamiliyId
        {
            get { return this.GetProperty(ProFamiliyIdProperty); }
            set { this.SetProperty(ProFamiliyIdProperty, value); }
        }
        #endregion

        #region 产品id ProductId
        /// <summary>
        /// 产品id
        /// </summary>
        [Label("产品id")]
        public static readonly Property<double?> ProductIdProperty = P<PrepareProductDetail>.Register(e => e.ProductId);

        /// <summary>
        /// 产品id
        /// </summary>
        public double? ProductId
        {
            get { return this.GetProperty(ProductIdProperty); }
            set { this.SetProperty(ProductIdProperty, value); }
        }
        #endregion

        #endregion
    }

    /// <summary>
    /// 数据配置
    /// </summary>
    public class PrepareProductDetailConfig : EntityConfig<PrepareProductDetail>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("MES_PREPRO_DET").MapAllProperties();
            Meta.Property(PrepareProductDetail.ProFamiliyIdProperty).DontMapColumn();
            Meta.Property(PrepareProductDetail.ProductIdProperty).DontMapColumn();
            Meta.EnablePhantoms();
        }
    }

    ///// <summary>
    ///// 扩展产品产前准备属性
    ///// </summary>
    //[CompiledPropertyDeclarer]
    //public class PrepareProductDetailExtProperty
    //{
    //    /// <summary>
    //    /// 产品产前准备子表 扩展属性。
    //    /// </summary>
    //    public static readonly ListProperty<EntityList<PrepareProductDetail>> PrepareProductDetailListProperty =
    //        P<PrepareProduct>.RegisterExtensionList<EntityList<PrepareProductDetail>>("PrepareProductDetailList", typeof(PrepareProductDetailExtProperty));

    //    /// <summary>
    //    /// 获取 产品产前准备子表 属性的值。
    //    /// </summary>
    //    /// <param name="me">要获取扩展属性值的对象。</param>
    //    public static EntityList<PrepareProductDetail> GetPrepareProductDetailList(PrepareProduct me)
    //    {
    //        return me.GetProperty(PrepareProductDetailListProperty);
    //    }

    //    /// <summary>
    //    /// 设置 产品产前准备子表 属性的值。
    //    /// </summary>
    //    /// <param name="me"></param>
    //    /// <param name="value"></param>
    //    public static void SetPrepareProductDetailList(PrepareProduct me, EntityList<PrepareProductDetail> value)
    //    {
    //        me.SetProperty(PrepareProductDetailListProperty, value);
    //    }

    //    /// <summary>
    //    /// 扩展产品产前准备属性
    //    /// </summary>
    //    internal class PrepareProductDetailExtPropertyConfig : EntityConfig<PrepareProduct>
    //    {
    //        /// <summary>
    //        /// 属性元数据配置
    //        /// </summary>
    //        protected override void ConfigMeta()
    //        {
    //            Meta.Property(PrepareProductDetailExtProperty.PrepareProductDetailListProperty).DontMapColumn();
    //        }
    //    }
    //}
}
