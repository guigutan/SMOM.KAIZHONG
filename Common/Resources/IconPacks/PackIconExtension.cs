using System;
using System.Windows.Markup;
using System.Windows.Media.Animation;

namespace Resources.IconPacks
{
    /// <summary>
    /// 图标扩展
    /// </summary>
    [MarkupExtensionReturnType(typeof(PackIcon))]
    public class PackIconExtension : MarkupExtension
    {
        /// <summary>
        /// 图标
        /// </summary>
        [ConstructorArgument("kind")]
        public PackIconKind Kind { get; set; }

        /// <summary>
        /// 宽度
        /// </summary>
        public double? Width { get; set; }

        /// <summary>
        /// 高度
        /// </summary>
        public double? Height { get; set; }

        /// <summary>
        /// 对齐方式
        /// </summary>
        public PackIconFlipOrientation? Flip { get; set; }

        /// <summary>
        /// 旋转弧度
        /// </summary>
        public double? Rotation { get; set; }

        /// <summary>
        /// 是否旋转
        /// </summary>
        public bool? Spin { get; set; }

        /// <summary>
        /// 是否旋转自动反转
        /// </summary>
        public bool? SpinAutoReverse { get; set; }

        /// <summary>
        /// 旋转功能
        /// </summary>
        public IEasingFunction SpinEasingFunction { get; set; }

        /// <summary>
        /// 旋转持续时间
        /// </summary>
        public double? SpinDuration { get; set; }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public PackIconExtension()
        {
        }

        /// <summary>
        /// 有参构造函数
        /// </summary>
        /// <param name="kind">图标库</param>
        public PackIconExtension(PackIconKind kind)
        {
            Kind = kind;
        }

        /// <summary>
        /// 提供值
        /// </summary>
        /// <param name="serviceProvider">IServiceProvider对象</param>
        /// <returns>object对象</returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return GetPackIcon<PackIcon, PackIconKind>(Kind);
        }

        private PackIcon<TKind> GetPackIcon<TPack, TKind>(TKind kind) where TPack : PackIcon<TKind>, new()
        {
            var packIcon = new TPack {Kind = kind};
            if (Width != null)
                packIcon.Width = Width.Value;
            if (Height != null)
                packIcon.Height = Height.Value;
            if (Flip != null)
                packIcon.Flip = Flip.Value;
            if (Rotation != null)
                packIcon.Rotation = Rotation.Value;
            if (Spin != null)
                packIcon.Spin = Spin.Value;
            if (SpinAutoReverse != null)
                packIcon.SpinAutoReverse = SpinAutoReverse.Value;
            if (SpinEasingFunction != null)
                packIcon.SpinEasingFunction = SpinEasingFunction;
            if (SpinDuration != null)
                packIcon.SpinDuration = SpinDuration.Value;
            return packIcon;
        }
    }
}