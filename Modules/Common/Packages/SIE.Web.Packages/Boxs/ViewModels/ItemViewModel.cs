using SIE.Domain;
using SIE.Items;
using SIE.ObjectModel;

namespace SIE.Web.Packages.Boxs.ViewModels
{
    /// <summary>
    /// 物料ViewModel 用于周转箱
    /// </summary>
    [RootEntity]
    [CriteriaQuery]
    [Label("物料")]
    public class ItemViewModel : ViewModel
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<ItemViewModel>.Register(e => e.Code);

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
        [Required]
        [NotDuplicate]
        [MaxLength(240)]
        [Label("产品名称")]
        public static readonly Property<string> NameProperty = P<ItemViewModel>.Register(e => e.Name);

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
        [Label("描述")]
        [MaxLength(4000)]
        public static readonly Property<string> DescriptionProperty = P<ItemViewModel>.Register(e => e.Description);

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return GetProperty(DescriptionProperty); }
            set { SetProperty(DescriptionProperty, value); }
        }
        #endregion

        #region 状态 State
        /// <summary>
        /// 状态
        /// </summary>
        [Label("状态")]
        public static readonly Property<State> StateProperty = P<ItemViewModel>.Register(e => e.State);

        /// <summary>
        /// 状态
        /// </summary>
        public State State
        {
            get { return GetProperty(StateProperty); }
            set { SetProperty(StateProperty, value); }
        }
        #endregion

        #region 类型 Type
        /// <summary>
        /// 类型
        /// </summary>
        [Label("类型")]
        public static readonly Property<ItemType> TypeProperty = P<ItemViewModel>.Register(e => e.Type);

        /// <summary>
        /// 类型
        /// </summary>
        public ItemType Type
        {
            get { return GetProperty(TypeProperty); }
            set { SetProperty(TypeProperty, value); }
        }
        #endregion

    }

    /// <summary>
    /// 物料ViewModel 视图配置
    /// </summary>
    internal class ItemViewModelViewConfig : WebViewConfig<ItemViewModel>
    {
        protected override void ConfigListView()
        {
            View.Property(p => p.Code);
            View.Property(p => p.Name);
            View.Property(p => p.Description);
            View.Property(p => p.Type);
        }
    }
}
