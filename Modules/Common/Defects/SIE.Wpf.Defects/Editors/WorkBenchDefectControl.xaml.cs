using SIE.Defects;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Wpf.Common.Editors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Defects.Editors
{
    /// <summary>
    /// WorkBenchDefectControl.xaml 的交互逻辑
    /// </summary>
    public partial class WorkBenchDefectControl : UserControl
    {
        #region Data
        /// <summary>
        /// 所有缺陷项目
        /// </summary>
        protected virtual List<DefectItemInfo> DefectList { get; }

        /// <summary>
        /// 当前列表中的缺陷项目
        /// </summary>
        protected virtual ObservableCollection<DefectItemInfo> CurrentDefectList { get; }

        /// <summary>
        /// 所有分类
        /// </summary>
        protected virtual List<DefectCategoryItemInfo> CategoryList { get; }

        /// <summary>
        /// 选中的分类
        /// </summary>
        protected virtual ObservableCollection<DefectCategoryItemInfo> SelectedCategoryList { get; }

        /// <summary>
        /// 当前列表中的分类
        /// </summary>
        protected virtual ObservableCollection<DefectCategoryItemInfo> CurrentCategoryList { get; }
        #endregion

        #region SelectedValue

        /// <summary>
        /// 选中的值
        /// </summary>
        public ObservableCollection<DefectItemInfo> SelectedValue
        {
            get { return (ObservableCollection<DefectItemInfo>)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        /// <summary>
        /// 选中的值
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(ObservableCollection<DefectItemInfo>), typeof(WorkBenchDefectControl), new PropertyMetadata(new ObservableCollection<DefectItemInfo>(), (s, e) => { ((WorkBenchDefectControl)s).OnValueChanged(e); }));

        /// <summary>
        /// 值变更通知
        /// </summary>
        /// <param name="e">DependencyPropertyChangedEventArgs</param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as ObservableCollection<DefectItemInfo>;
            if (oldvalue != null)
                oldvalue.CollectionChanged -= SelectedValue_CollectionChanged;

            var newvalue = e.NewValue as ObservableCollection<DefectItemInfo>;
            if (newvalue != null)
            {
                newvalue.CollectionChanged += SelectedValue_CollectionChanged;
                ctlSelect.ItemsSource = newvalue;
            }
        }

        /// <summary>
        /// 值选择变更
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void SelectedValue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (DefectItemInfo item in e.OldItems)
                {
                    item.Qty = 0;
                }
            }
        }

        #endregion

        #region Defects

        /// <summary>
        /// 缺陷列表
        /// </summary>
        public EntityList<Defect> Defects
        {
            get { return (EntityList<Defect>)GetValue(DefectsProperty); }
            set { SetValue(DefectsProperty, value); }
        }

        /// <summary>
        /// 缺陷列表
        /// </summary>
        public static readonly DependencyProperty DefectsProperty =
            DependencyProperty.Register("Defects", typeof(EntityList<Defect>), typeof(WorkBenchDefectControl), new PropertyMetadata(new EntityList<Defect>(), (s, e) => { ((WorkBenchDefectControl)s).OnDefectsChanged(e); }));

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="e">e</param>
        protected virtual void OnDefectsChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as EntityList<Defect>;
            if (oldvalue != null)
                oldvalue.CollectionChanged -= Defects_CollectionChanged;

            var newvalue = e.NewValue as EntityList<Defect>;
            if (newvalue != null)
            {
                Clear();
                newvalue.CollectionChanged += Defects_CollectionChanged;
            }
        }

        /// <summary>
        /// 缺陷列表变更
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Defects_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (Defect defect in e.NewItems)
                    OnDefectAdded(defect);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (Defect defect in e.OldItems)
                    OnDefectRemoved(defect);
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Clear();
                if (e.NewItems != null)
                    foreach (Defect defect in e.NewItems)
                        OnDefectAdded(defect);
            }
        }

        /// <summary>
        /// 清空内存数据
        /// </summary>
        void Clear()
        {
            CategoryList.Clear();
            CurrentCategoryList.Clear();
            SelectedCategoryList.Clear();
            allCategoryItem.Clear();
            DefectList.Clear();
            CurrentDefectList.Clear();
            SelectedValue.Clear();
        }

        #endregion

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// 是否允许输入数量
        /// </summary>
        public bool AllowQty { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public WorkBenchDefectControl()
        {
            InitializeComponent();

            SelectedValue = new ObservableCollection<DefectItemInfo>();
            SelectedValue.CollectionChanged += SelectedValue_CollectionChanged;

            DefectList = new List<DefectItemInfo>();
            CurrentDefectList = new ObservableCollection<DefectItemInfo>();

            CategoryList = new List<DefectCategoryItemInfo>();
            CurrentCategoryList = new ObservableCollection<DefectCategoryItemInfo>();
            SelectedCategoryList = new ObservableCollection<DefectCategoryItemInfo>();

            ctlCategory.ItemsSource = CurrentCategoryList;
            ctlDefect.ItemsSource = CurrentDefectList;
            ctlNavigation.ItemsSource = SelectedCategoryList;
            ctlSelect.ItemsSource = SelectedValue;
            Defects.CollectionChanged += Defects_CollectionChanged;
        }

        /// <summary>
        /// 从缺陷列表移除缺陷
        /// </summary>
        /// <param name="defect">缺陷对象</param>
        protected virtual void OnDefectRemoved(Defect defect)
        {
            var item = DefectList.FirstOrDefault(p => p.Defect.Id == defect.Id);
            if (item != null)
                DefectList.Remove(item);
            if (DefectList.Count == 0)
                Clear();
        }

        /// <summary>
        /// 增加缺陷到缺陷列表
        /// </summary>
        /// <param name="defect">缺陷对象</param>
        protected virtual void OnDefectAdded(Defect defect)
        {
            AddCategory(defect.DefectCategoryId);
            var item = new DefectItemInfo { Defect = defect, Control = this };
            DefectList.Add(item);
            CurrentDefectList.Add(item);
        }

        /// <summary>
        /// 缺陷分类项目 字典
        /// </summary>
        Dictionary<double, DefectCategoryItemInfo> allCategoryItem = new Dictionary<double, DefectCategoryItemInfo>();

        /// <summary>
        /// 缺陷分类 字典
        /// </summary>
        Dictionary<double, DefectCategory> _allCategory;

        /// <summary>
        /// 缺陷分类 字典
        /// </summary>
        Dictionary<double, DefectCategory> AllCategory
        {
            get
            {
                if (_allCategory == null)
                {
                    _allCategory = new Dictionary<double, DefectCategory>();
                    var all = RF.GetAll<DefectCategory>();
                    all.EachNode(p =>
                    {
                        var c = p as DefectCategory;
                        _allCategory.Add(c.Id, c);
                        return false;
                    });
                }

                return _allCategory;
            }
        }

        /// <summary>
        /// AddCategory
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <returns>DefectCategoryItemInfo</returns>
        protected virtual DefectCategoryItemInfo AddCategory(double categoryId)
        {
            DefectCategoryItemInfo item;
            if (!allCategoryItem.TryGetValue(categoryId, out item))
            {
                DefectCategory category = AllCategory[categoryId];
                item = new DefectCategoryItemInfo { Category = category };
                allCategoryItem.Add(item.Category.Id, item);
                var pid = category.GetTreePId();
                DefectCategory parent;
                if (pid == null || !AllCategory.TryGetValue(pid.ConvertTo<double>(), out parent))
                {
                    CategoryList.Add(item);
                    CurrentCategoryList.Add(item);
                }
                else
                {
                    DefectCategoryItemInfo itemParent;
                    if (!allCategoryItem.TryGetValue(parent.Id, out itemParent))
                        itemParent = AddCategory(parent.Id);

                    itemParent.Children.Add(item);
                    item.Parent = itemParent;
                }
            }

            return item;
        }

        /// <summary>
        /// BtnCategory_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnCategory_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var selected = btn.Tag as DefectCategoryItemInfo;
            OnCategorySelected(selected);
        }

        /// <summary>
        /// 选中分类
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnCategorySelected(DefectCategoryItemInfo selected)
        {
            CurrentCategoryList.Clear();
            if (selected.Children.Count > 0)
            {
                foreach (var child in selected.Children)
                    CurrentCategoryList.Add(child);
            }
            else
            {
                CurrentCategoryList.Add(selected);
            }

            if (!SelectedCategoryList.Contains(selected))
                SelectedCategoryList.Add(selected);
            ShowCurrentDefect(selected);
        }

        /// <summary>
        /// ShowCurrentDefect
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void ShowCurrentDefect(DefectCategoryItemInfo selected = null)
        {
            CurrentDefectList.Clear();
            if (selected == null)
            {
                foreach (var item in DefectList)
                    CurrentDefectList.Add(item);
            }
            else
            {
                foreach (var item in FindItems(selected))
                    CurrentDefectList.Add(item);
            }
        }

        /// <summary>
        /// FindItems
        /// </summary>
        /// <param name="selected">selected</param>
        /// <returns>DefectItemInfo列表</returns>
        IEnumerable<DefectItemInfo> FindItems(DefectCategoryItemInfo selected)
        {
            foreach (var item in DefectList.Where(p => p.Defect.DefectCategoryId == selected.Category.Id))
                yield return item;
            foreach (var category in selected.Children)
                foreach (var item in FindItems(category))
                    yield return item;
        }

        /// <summary>
        /// BtnDefect_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnDefect_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var selected = btn.Tag as DefectItemInfo;
            OnDefectSelected(selected);
        }

        /// <summary>
        /// 取消选中缺陷
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnDefectUnselected(DefectItemInfo selected)
        {
            selected.Qty = 0;
            SelectedValue.Remove(selected);
        }

        /// <summary>
        /// 选中缺陷
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnDefectSelected(DefectItemInfo selected)
        {
            if (AllowQty)
            {
                var calculator = new Calculator();
                var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), calculator, w =>
                {
                    w.Width = 400;
                    w.Height = 400;
                    w.Closing += (s, e) =>
                    {
                        e.Cancel = calculator.HasError;
                    };
                });
                if (result == 0)
                {
                    selected.Qty = calculator.Value;
                    if (selected.Qty > 0)
                    {
                        AddSelected(selected);
                    }
                }
            }
            else
                AddSelected(selected);
        }

        /// <summary>
        /// AddSelected
        /// </summary>
        /// <param name="selected">selected</param>
        void AddSelected(DefectItemInfo selected)
        {
            if (!AllowMultiple)
                SelectedValue.Clear();
            if (!SelectedValue.Contains(selected))
                SelectedValue.Add(selected);
        }

        /// <summary>
        /// BtnNavigation_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnNavigation_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var selected = btn.Tag as DefectCategoryItemInfo;
            OnNavigationChanged(selected);
        }

        /// <summary>
        /// OnNavigationChanged
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnNavigationChanged(DefectCategoryItemInfo selected)
        {
            SelectedCategoryList.Clear();
            CurrentCategoryList.Clear();
            DefectCategoryItemInfo cate = selected;
            while (cate != null)
            {
                SelectedCategoryList.Insert(0, cate);
                cate = cate.Parent;
            }
            if (selected != null)
            {
                if (selected.Children.Count == 0)
                {
                    CurrentCategoryList.Add(selected);
                }
                else
                {
                    foreach (var child in selected.Children)
                        CurrentCategoryList.Add(child);
                }
            }
            ShowCurrentDefect(selected);
        }

        /// <summary>
        /// BtnAll_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {
            SelectedCategoryList.Clear();
            CurrentCategoryList.Clear();
            foreach (var c in CategoryList)
                CurrentCategoryList.Add(c);
            ShowCurrentDefect();
        }

        /// <summary>
        /// BtnSelected_Click
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void BtnSelected_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var selected = btn.Tag as DefectItemInfo;
            if (AllowQty)
            {
                var calculator = new Calculator();
                calculator.Value = selected.Qty;
                var result = CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), calculator, w =>
                {
                    w.Width = 400;
                    w.Height = 400;
                    w.Closing += (s, x) =>
                    {
                        x.Cancel = calculator.HasError;
                    };
                });
                if (result == 0)
                {
                    selected.Qty = calculator.Value;
                    if (selected.Qty <= 0)
                        OnDefectUnselectd(selected);
                }
            }
            else
                OnDefectUnselectd(selected);
        }

        /// <summary>
        /// 取消选中缺陷
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnDefectUnselectd(DefectItemInfo selected)
        {
            SelectedValue.Remove(selected);
        }
    }

    /// <summary>
    /// 缺陷分类项目
    /// </summary>
    public class DefectCategoryItemInfo
    {
        /// <summary>
        /// 缺陷分类
        /// </summary>
        public DefectCategory Category { get; set; }

        /// <summary>
        /// 缺陷分类集合
        /// </summary>
        public ObservableCollection<DefectCategoryItemInfo> Children { get; } = new ObservableCollection<DefectCategoryItemInfo>();

        /// <summary>
        /// 缺陷分类项目
        /// </summary>
        public DefectCategoryItemInfo Parent { get; set; }

        /// <summary>
        /// 比较缺陷分类ID是否一致
        /// </summary>
        /// <param name="obj">需要比较的对象</param>
        /// <returns>返回是否与传入的对象的缺陷分类ID一致</returns>
        public override bool Equals(object obj)
        {
            var item = obj as DefectCategoryItemInfo;
            if (item == null) return false;
            return Category?.Id == item.Category?.Id;
        }

        /// <summary>
        /// 获取缺陷分类的哈希编码
        /// </summary>
        /// <returns>返回缺陷分类的哈希编码</returns>
        public override int GetHashCode()
        {
            if (Category == null) return 0;
            return Category.GetHashCode();
        }
    }

    /// <summary>
    /// 缺陷项目
    /// </summary>
    public class DefectItemInfo : ObservableObject
    {
        /// <summary>
        /// Control
        /// </summary>
        public WorkBenchDefectControl Control { get; set; }

        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get { return GetProperty<Defect>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 不良数量,不允许输入数量时,返回1
        /// </summary>
        public double Qty
        {
            get
            {
                if (!Control.AllowQty)
                    return 1;
                return GetProperty<double>();
            }

            set
            {
                SetProperty(value);
            }
        }

        /// <summary>
        /// 比较缺陷分类ID是否一致
        /// </summary>
        /// <param name="obj">需要比较的对象</param>
        /// <returns>返回是否与传入的对象的缺陷分类ID一致</returns>
        public override bool Equals(object obj)
        {
            var item = obj as DefectItemInfo;
            if (item == null) return false;
            return Defect?.Id == item.Defect?.Id;
        }

        /// <summary>
        /// 获取缺陷分类的哈希编码
        /// </summary>
        /// <returns>返回缺陷分类的哈希编码</returns>
        public override int GetHashCode()
        {
            if (Defect == null) return 0;
            return Defect.GetHashCode();
        }
    }
}