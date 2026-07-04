using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Controls;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Resources.IconPacks
{
    /// <summary>
    /// Enum PackIconFlipOrientation for the Flip property of the PackIcon
    /// </summary>
    public enum PackIconFlipOrientation
    {
        /// <summary>
        /// No flip
        /// </summary>
        Normal,

        /// <summary>
        /// Flip the icon horizontal
        /// </summary>
        Horizontal,

        /// <summary>
        /// Flip the icon vertical
        /// </summary>
        Vertical,

        /// <summary>
        /// Flip the icon vertical and horizontal
        /// </summary>
        Both
    }

    /// <summary>
    /// 图标基类
    /// </summary>
    public abstract class PackIconBase : Control
    {
        internal abstract void UpdateData();
    }

    /// <summary>
    /// Base class for creating an icon control for icon packs.
    /// </summary>
    /// <typeparam name="TKind"></typeparam>
    public abstract class PackIconBase<TKind> : PackIconBase
    {
        private static Lazy<IDictionary<TKind, string>> _dataIndex;

        /// <param name="dataIndexFactory">
        /// Inheritors should provide a factory for setting up the path data index (per icon kind).
        /// The factory will only be utilised once, across all closed instances (first instantiation wins).
        /// </param>
        protected PackIconBase(Func<IDictionary<TKind, string>> dataIndexFactory)
        {
            if (dataIndexFactory == null) throw new ArgumentNullException(nameof(dataIndexFactory));

            if (_dataIndex == null)
                _dataIndex = new Lazy<IDictionary<TKind, string>>(dataIndexFactory);
        }

        /// <summary>
        /// Kind
        /// </summary>
        public static readonly DependencyProperty KindProperty
            = DependencyProperty.Register(nameof(Kind), typeof(TKind), typeof(PackIconBase<TKind>), new PropertyMetadata(default(TKind), KindPropertyChangedCallback));

        private static void KindPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs dependencyPropertyChangedEventArgs)
        {
            ((PackIconBase)dependencyObject).UpdateData();
        }

        /// <summary>
        /// Gets or sets the icon to display.
        /// </summary>
        public TKind Kind
        {
            get { return (TKind)GetValue(KindProperty); }
            set { SetValue(KindProperty, value); }
        }
        private static readonly DependencyPropertyKey DataPropertyKey
            = DependencyProperty.RegisterReadOnly(nameof(Data), typeof(string), typeof(PackIconBase<TKind>), new PropertyMetadata(""));

        /// <summary>
        /// Data数据
        /// </summary>
        public static readonly DependencyProperty DataProperty = DataPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the icon path data for the current <see cref="Kind"/>.
        /// </summary>
        [TypeConverter(typeof(GeometryConverter))]
        public string Data
        {
            get { return (string)GetValue(DataProperty); }
            private set { SetValue(DataPropertyKey, value); }
        }

        /// <summary>
        /// 仅应用模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            UpdateData();
        }

        internal override void UpdateData()
        {
            string data = null;
            if (_dataIndex.Value != null)
                _dataIndex.Value.TryGetValue(Kind, out data);
            Data = data;
        }
    }

    /// <summary>
    /// Class PackIcon which is the custom base class of Resources.IconPacks.
    /// </summary>
    /// <typeparam name="TKind">The type of the enum kind.</typeparam>
    /// <seealso cref="PackIconBase{TKind}" />
    public class PackIcon<TKind> : PackIconBase<TKind>
    {
        static PackIcon()
        {
            OpacityProperty.OverrideMetadata(typeof(PackIcon<TKind>), new UIPropertyMetadata(1d, (d, e) => { d.CoerceValue(SpinProperty); }));
            VisibilityProperty.OverrideMetadata(typeof(PackIcon<TKind>), new UIPropertyMetadata(Visibility.Visible, (d, e) => { d.CoerceValue(SpinProperty); }));
        }

        /// <summary>
        /// 带参的构造函数
        /// </summary>
        /// <param name="dataIndexFactory">参数集合</param>
        public PackIcon(Func<IDictionary<TKind, string>> dataIndexFactory) : base(dataIndexFactory)
        {
        }

        /// <summary>
        /// 仅应用模板
        /// </summary>
        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            this.CoerceValue(SpinProperty);
        }
        /// <summary>
        /// Identifies the Flip dependency property.
        /// </summary>
        public static readonly DependencyProperty FlipProperty
            = DependencyProperty.Register(
                "Flip",
                typeof(PackIconFlipOrientation),
                typeof(PackIcon<TKind>),
                new PropertyMetadata(PackIconFlipOrientation.Normal));

        /// <summary>
        /// Gets or sets the flip orientation.
        /// </summary>
        public PackIconFlipOrientation Flip
        {
            get { return (PackIconFlipOrientation)this.GetValue(FlipProperty); }
            set { this.SetValue(FlipProperty, value); }
        }

        /// <summary>
        /// Identifies the Rotation dependency property.
        /// </summary>
        public static readonly DependencyProperty RotationProperty
            = DependencyProperty.Register(
                "Rotation",
                typeof(double),
                typeof(PackIcon<TKind>),
                new PropertyMetadata(0d, null, RotationPropertyCoerceValueCallback));

        private static object RotationPropertyCoerceValueCallback(DependencyObject dependencyObject, object value)
        {
            var val = (double)value;
            return val < 0 ? 0d : (val > 360 ? 360d : value);
        }

        /// <summary>
        /// Gets or sets the rotation (angle).
        /// </summary>
        /// <value>The rotation.</value>
        public double Rotation
        {
            get { return (double)this.GetValue(RotationProperty); }
            set { this.SetValue(RotationProperty, value); }
        }

        /// <summary>
        /// Identifies the Spin dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinProperty
            = DependencyProperty.Register(
                "Spin",
                typeof(bool),
                typeof(PackIcon<TKind>),
                new PropertyMetadata(default(bool), SpinPropertyChangedCallback, SpinPropertyCoerceValueCallback));

        private static object SpinPropertyCoerceValueCallback(DependencyObject dependencyObject, object value)
        {
            var packIcon = dependencyObject as PackIcon<TKind>;
            if (packIcon != null && (!packIcon.IsVisible || packIcon.Opacity <= 0 || packIcon.SpinDuration <= 0.0))
            {
                return false;
            }
            return value;
        }

        private static void SpinPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var packIcon = dependencyObject as PackIcon<TKind>;
            if (packIcon != null && e.OldValue != e.NewValue && e.NewValue is bool)
            {
                var spin = (bool)e.NewValue;
                if (spin)
                {
                    packIcon.BeginSpinAnimation();
                }
                else
                {
                    packIcon.StopSpinAnimation();
                }
            }
        }

        private static readonly string SpinnerStoryBoardName = $"{typeof(PackIcon<TKind>).Name}SpinnerStoryBoard";

        private FrameworkElement _innerGrid;
        private FrameworkElement InnerGrid => this._innerGrid ?? (this._innerGrid = this.GetTemplateChild("PART_InnerGrid") as FrameworkElement);

        private void BeginSpinAnimation()
        {
            var element = this.InnerGrid;
            if (null == element)
            {
                return;
            }
            var transformGroup = element.RenderTransform as TransformGroup ?? new TransformGroup();
            var rotateTransform = transformGroup.Children.OfType<RotateTransform>().LastOrDefault();

            if (rotateTransform != null)
            {
                rotateTransform.Angle = 0;
            }
            else
            {
                transformGroup.Children.Add(new RotateTransform());
                element.RenderTransform = transformGroup;
            }

            var storyboard = new Storyboard();
            var animation = new DoubleAnimation
            {
                From = 0,
                To = 360,
                AutoReverse = this.SpinAutoReverse,
                EasingFunction = this.SpinEasingFunction,
                RepeatBehavior = RepeatBehavior.Forever,
                Duration = new Duration(TimeSpan.FromSeconds(this.SpinDuration))
            };
            storyboard.Children.Add(animation);

            Storyboard.SetTarget(animation, element);
            Storyboard.SetTargetProperty(animation, new PropertyPath("(0).(1)[2].(2)", RenderTransformProperty, TransformGroup.ChildrenProperty, RotateTransform.AngleProperty));

            element.Resources.Add(SpinnerStoryBoardName, storyboard);
            storyboard.Begin();
        }

        private void StopSpinAnimation()
        {
            var element = this.InnerGrid;
            if (null == element)
            {
                return;
            }
            var storyboard = element.Resources[SpinnerStoryBoardName] as Storyboard;
            if (storyboard != null)
            {
                storyboard.Stop();
                element.Resources.Remove(SpinnerStoryBoardName);
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the inner icon is spinning.
        /// </summary>
        /// <value><c>true</c> if spin; otherwise, <c>false</c>.</value>
        public bool Spin
        {
            get { return (bool)this.GetValue(SpinProperty); }
            set { this.SetValue(SpinProperty, value); }
        }

        /// <summary>
        /// Identifies the SpinDuration dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinDurationProperty
            = DependencyProperty.Register("SpinDuration",
                typeof(double),
                typeof(PackIcon<TKind>),
                new PropertyMetadata(1d, SpinDurationPropertyChangedCallback, SpinDurationCoerceValueCallback));

        private static void SpinDurationPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var packIcon = dependencyObject as PackIcon<TKind>;
            if (packIcon != null && e.OldValue != e.NewValue && packIcon.Spin && e.NewValue is double)
            {
                packIcon.StopSpinAnimation();
                packIcon.BeginSpinAnimation();
            }
        }
        
        private static object SpinDurationCoerceValueCallback(DependencyObject dependencyObject, object value)
        {
            var val = (double)value;
            return val < 0 ? 0d : value;
        }

        /// <summary>
        /// Gets or sets the duration of the spinning animation (in seconds). This will also restart the spin animation.
        /// </summary>
        /// <value>The duration of the spin in seconds.</value>
        public double SpinDuration
        {
            get { return (double)this.GetValue(SpinDurationProperty); }
            set { this.SetValue(SpinDurationProperty, value); }
        }

        /// <summary>
        /// Identifies the SpinEasingFunction dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinEasingFunctionProperty
            = DependencyProperty.Register("SpinEasingFunction",
                typeof(IEasingFunction),
                typeof(PackIcon<TKind>),
                new PropertyMetadata(null, SpinEasingFunctionPropertyChangedCallback));

        private static void SpinEasingFunctionPropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var packIcon = dependencyObject as PackIcon<TKind>;
            if (packIcon != null && e.OldValue != e.NewValue && packIcon.Spin)
            {
                packIcon.StopSpinAnimation();
                packIcon.BeginSpinAnimation();
            }
        }

        /// <summary>
        /// Gets or sets the EasingFunction of the spinning animation. This will also restart the spin animation.
        /// </summary>
        /// <value>The spin easing function.</value>
        public IEasingFunction SpinEasingFunction
        {
            get { return (IEasingFunction)this.GetValue(SpinEasingFunctionProperty); }
            set { this.SetValue(SpinEasingFunctionProperty, value); }
        }

        /// <summary>
        /// Identifies the SpinAutoReverse dependency property.
        /// </summary>
        public static readonly DependencyProperty SpinAutoReverseProperty
            = DependencyProperty.Register("SpinAutoReverse",
                typeof(bool),
                typeof(PackIcon<TKind>),
                new PropertyMetadata(default(bool), SpinAutoReversePropertyChangedCallback));

        private static void SpinAutoReversePropertyChangedCallback(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var packIcon = dependencyObject as PackIcon<TKind>;
            if (packIcon != null && e.OldValue != e.NewValue && packIcon.Spin && e.NewValue is bool)
            {
                packIcon.StopSpinAnimation();
                packIcon.BeginSpinAnimation();
            }
        }

        /// <summary>
        /// Gets or sets the AutoReverse of the spinning animation. This will also restart the spin animation.
        /// </summary>
        /// <value><c>true</c> if [spin automatic reverse]; otherwise, <c>false</c>.</value>
        public bool SpinAutoReverse
        {
            get { return (bool)this.GetValue(SpinAutoReverseProperty); }
            set { this.SetValue(SpinAutoReverseProperty, value); }
        }
    }

    /// <summary>
    /// 图标
    /// </summary>
    public class PackIcon : PackIcon<PackIconKind>
    {
        static PackIcon()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PackIcon), new FrameworkPropertyMetadata(typeof(PackIcon)));
        }

        /// <summary>
        /// 无参构造函数
        /// </summary>
        public PackIcon() : base(PackIconDataFactory.Create)
        {
        }
    }
}