using SIE.Defects.Measures;
using SIE.Domain;
using SIE.ObjectModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SIE.Wpf.Defects.Editors
{
    /// <summary>
    /// MeasureControl.xaml 的交互逻辑
    /// </summary>
    public partial class MeasureControl : UserControl
    {
        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// MeasureControl
        /// </summary>
        public MeasureControl()
        {
            InitializeComponent();
            SelectedValues.Clear();
            MeasureList = new ObservableCollection<MeasureItem>();
            ctlMeasure.ItemsSource = MeasureList;
            SelectedValues.CollectionChanged += SelectedValues_CollectionChanged;
            Measures.CollectionChanged += Measures_CollectionChanged;
        }

        /// <summary>
        /// 所有维修措施
        /// </summary>
        protected virtual ObservableCollection<MeasureItem> MeasureList { get; }

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public EntityList<RepairMeasure> Measures
        {
            get { return (EntityList<RepairMeasure>)GetValue(MeasuresProperty); }
            set { SetValue(MeasuresProperty, value); }
        }

        /// <summary>
        /// 维修措施列表
        /// </summary>
        public static readonly DependencyProperty MeasuresProperty =
            DependencyProperty.Register("Measures", typeof(EntityList<RepairMeasure>), typeof(MeasureControl), new PropertyMetadata(new EntityList<RepairMeasure>(), (s, e) => { ((MeasureControl)s).OnMeasuresChanged(e); }));

        /// <summary>
        /// OnMeasuresChanged
        /// </summary>
        /// <param name="e">e</param>
        private void OnMeasuresChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as EntityList<RepairMeasure>;
            if (oldvalue != null)
                oldvalue.CollectionChanged -= Measures_CollectionChanged;

            var newvalue = e.NewValue as EntityList<RepairMeasure>;
            if (newvalue != null)
            {
                newvalue.CollectionChanged += Measures_CollectionChanged;
            }
        }

        /// <summary>
        /// Measures_CollectionChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Measures_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            foreach (RepairMeasure measure in e.NewItems)
                OnMeasuresAdded(measure);
        }

        /// <summary>
        /// SelectedValues
        /// </summary>
        public EntityList<RepairMeasure> SelectedValues
        {
            get { return (EntityList<RepairMeasure>)GetValue(SelectedValuesProperty); }
            set { SetValue(SelectedValuesProperty, value); }
        }

        /// <summary>
        /// SelectedValues
        /// </summary>
        public static readonly DependencyProperty SelectedValuesProperty =
            DependencyProperty.Register("SelectedValues", typeof(EntityList<RepairMeasure>), typeof(MeasureControl), new PropertyMetadata(new EntityList<RepairMeasure>(), (s, e) => { ((MeasureControl)s).OnValuesChanged(e); }));

        /// <summary>
        /// OnValuesChanged
        /// </summary>
        /// <param name="e">e</param>
        private void OnValuesChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as EntityList<RepairMeasure>;
            if (oldvalue != null)
                oldvalue.CollectionChanged -= SelectedValues_CollectionChanged;

            var newvalue = e.NewValue as EntityList<RepairMeasure>;
            if (newvalue != null)
            {
                newvalue.CollectionChanged += SelectedValues_CollectionChanged;
            }
        }

        /// <summary>
        /// SelectedValues_CollectionChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void SelectedValues_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add)
            {
                foreach (var item in e.NewItems)
                {
                    var i = MeasureList.FirstOrDefault(p => p.Measure == item);
                    if (i != null)
                        i.IsChecked = true;
                }
            }
        }

        /// <summary>
        /// OnMeasuresAdded
        /// </summary>
        /// <param name="measure">measure</param>
        private void OnMeasuresAdded(RepairMeasure measure)
        {
            if (MeasureList.FirstOrDefault(p => p.Measure == measure) != null) return;
            var measureItem = new MeasureItem() { Control = this, Measure = measure, IsChecked = false };
            MeasureList.Add(measureItem);
        }

        /// <summary>
        /// OnSelectedAdded
        /// </summary>
        /// <param name="measure">measure</param>
        private void OnSelectedAdded(RepairMeasure measure)
        {
            if (SelectedValues.Contains(measure))
                SelectedValues.Remove(measure);
            else
                SelectedValues.Add(measure);
        }

        /// <summary>
        /// BtnMeasure_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnMeasure_Click(object sender, RoutedEventArgs e)
        {
            if (!AllowMultiple && MeasureList.FirstOrDefault(p => p.IsChecked == true) != null)
                return;
            var btn = sender as ToggleButton;
            var measure = btn.Tag as RepairMeasure;
            OnSelectedAdded(measure);
        }
    }

    /// <summary>
    /// 维修措施项
    /// </summary>
    public class MeasureItem : ObservableObject
    {
        /// <summary>
        /// Control
        /// </summary>
        public MeasureControl Control { get; set; }

        /// <summary>
        /// Measure
        /// </summary>
        public RepairMeasure Measure
        {
            get { return GetProperty<RepairMeasure>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// IsChecked
        /// </summary>
        public bool? IsChecked
        {
            get { return GetProperty<bool?>(); }
            set { SetProperty(value); }
        }
    }
}
