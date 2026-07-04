using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;

namespace SIE.Core.Boxs
{
    /// <summary>
    /// 周转箱型号
    /// </summary>
    [RootEntity, Serializable]
    [Label("周转箱型号")]
    [DisplayMember(nameof(Code))]
    public partial class TurnoverBoxModel : DataEntity
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [MaxLength(30)]
        [Label("编码")]
        [Required, NotDuplicate]
        public static readonly Property<string> CodeProperty = P<TurnoverBoxModel>.Register(e => e.Code);

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
        [MaxLength(80)]
        [Label("名称")]
        [Required]
        public static readonly Property<string> NameProperty = P<TurnoverBoxModel>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return GetProperty(NameProperty); }
            set { SetProperty(NameProperty, value); }
        }
        #endregion

        #region 类型 ToolType
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        [Required]
        public static readonly Property<string> ToolTypeProperty = P<TurnoverBoxModel>.Register(e => e.ToolType);

        /// <summary>
        /// 类型
        /// </summary>
        public string ToolType
        {
            get { return GetProperty(ToolTypeProperty); }
            set { SetProperty(ToolTypeProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 周转工具型号 实体配置
    /// </summary>
    internal class TurnoverBoxModelConfig : EntityConfig<TurnoverBoxModel>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("CNT_TURNOVER_BOX_MODEL").MapAllProperties();
            Meta.Property(TurnoverBoxModel.CodeProperty).ColumnMeta.HasLength(60);
            Meta.Property(TurnoverBoxModel.NameProperty).ColumnMeta.HasLength(160);
            Meta.EnablePhantoms();
        }
    }

    internal class TurnoverBoxModelWebViewConfig : WebViewConfig<TurnoverBoxModel>
    {
        ///<summary>
        /// 配置列表视图
        /// </summary>
        protected override void ConfigListView()
        {
            View.UseDefaultCommands();
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ToolType);
        }

        /// <summary>
        /// 选择视图
        /// </summary>
        protected override void ConfigSelectionView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.ToolType);
        }
    }
}
