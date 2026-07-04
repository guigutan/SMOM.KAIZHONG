using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.ERPInterface.Common.InfDataEntitys.Download
{
    /// <summary>
    /// 单据大类
    /// </summary>
    [RootEntity, Serializable]
    //[CriteriaQuery]
    [Label("单据大类")]
    public partial class FunctionInf : DownloadBaseEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<FunctionInf>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return GetProperty(CodeProperty); }
            set { SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<FunctionInf>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 描述 Description
        /// <summary>
        /// 描述
        /// </summary>
        [MaxLength(480)]
        [Label("描述")]
        public static readonly Property<string> DescriptionProperty = P<FunctionInf>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 是否IQC报检 IsQc
        /// <summary>
        /// 是否IQC报检
        /// </summary>
        [Label("是否IQC报检")]
        public static readonly Property<bool> IsQcProperty = P<FunctionInf>.Register(e => e.IsQc);

        /// <summary>
        /// 是否IQC报检
        /// </summary>
        public bool IsQc
        {
            get { return GetProperty(IsQcProperty); }
            set { SetProperty(IsQcProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 单据大类 实体配置
    /// </summary>
    internal class FunctionInfConfig : EntityConfig<FunctionInf>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("INF_FUNCTION").MapAllProperties();
            Meta.Property(FunctionInf.DescriptionProperty).ColumnMeta.HasLength(960);
            Meta.EnablePhantoms();
        }
    }
}