using SIE.Defects;
using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace SIE.Wpf.Defects.Editors
{
    /// <summary>
    /// ResponsibilityControl.xaml 的交互逻辑
    /// </summary>
    public partial class ResponsibilityControl : UserControl
    {
        #region Data
        /// <summary>
        /// 所有缺陷责任
        /// </summary>
        protected virtual List<DefectResponsibilityItem> ResponsibilityList { get; }

        /// <summary>
        /// 当前列表中的缺陷责任
        /// </summary>
        protected virtual ObservableCollection<DefectResponsibilityItem> CurrentResponsibilityList { get; }

        /// <summary>
        /// 所有缺陷责任分类
        /// </summary>
        protected virtual List<ResponsibilityCategoryItem> CategoryList { get; }

        /// <summary>
        /// 选中的缺陷责任分类
        /// </summary>
        protected virtual ObservableCollection<ResponsibilityCategoryItem> SelectedCategoryList { get; }

        /// <summary>
        /// 当前列表中的缺陷责任分类
        /// </summary>
        protected virtual ObservableCollection<ResponsibilityCategoryItem> CurrentCategoryList { get; }

        #endregion

        #region 备注
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark
        {
            get { return (string)GetValue(RemarkProperty); }
            set { SetValue(RemarkProperty, value); }
        }

        /// <summary>
        /// 备注
        /// </summary>
        public static readonly DependencyProperty RemarkProperty =
            DependencyProperty.Register("Remark", typeof(string), typeof(ResponsibilityControl), new PropertyMetadata(string.Empty));
        #endregion

        #region SelectedValue

        /// <summary>
        /// 选中的值
        /// </summary>
        public ObservableCollection<DefectResponsibilityItem> SelectedValue
        {
            get { return (ObservableCollection<DefectResponsibilityItem>)GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        /// <summary>
        /// 选中的值
        /// </summary>
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(ObservableCollection<DefectResponsibilityItem>), typeof(ResponsibilityControl), new PropertyMetadata(new ObservableCollection<DefectResponsibilityItem>(), (s, e) => { ((ResponsibilityControl)s).OnValueChanged(e); }));

        /// <summary>
        /// OnValueChanged
        /// </summary>
        /// <param name="e">e</param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as ObservableCollection<DefectResponsibilityItem>;
            if (oldvalue != null)
                oldvalue.CollectionChanged -= SelectedValue_CollectionChanged;

            var newvalue = e.NewValue as ObservableCollection<DefectResponsibilityItem>;
            if (newvalue != null)
            {
                newvalue.CollectionChanged += SelectedValue_CollectionChanged;
                ctlSelect.ItemsSource = newvalue;
            }
        }

        /// <summary>
        /// SelectedValue_CollectionChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void SelectedValue_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (DefectResponsibilityItem item in e.OldItems)
                {
                    item.Qty = 0;
                }
            }
        }

        #endregion

        #region Defects

        /// <summary>
        /// 缺陷责任列表
        /// </summary>
        public EntityList<DefectResponsibility> Responsibilities
        {
            get { return (EntityList<DefectResponsibility>)GetValue(ResponsibilitiesProperty); }
            set { SetValue(ResponsibilitiesProperty, value); }
        }

        /// <summary>
        /// 缺陷责任列表
        /// </summary>
        public static readonly DependencyProperty ResponsibilitiesProperty =
            DependencyProperty.Register("Defects", typeof(EntityList<DefectResponsibility>), typeof(ResponsibilityControl), new PropertyMetadata(new EntityList<DefectResponsibility>(), (s, e) => { ((ResponsibilityControl)s).OnDefectsChanged(e); }));

        /// <summary>
        /// OnDefectsChanged
        /// </summary>
        /// <param name="e">e</param>
        protected virtual void OnDefectsChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as EntityList<DefectResponsibility>;
            if (oldvalue != null)
                oldvalue.CollectionChanged -= Defects_CollectionChanged;

            var newvalue = e.NewValue as EntityList<DefectResponsibility>;
            if (newvalue != null)
            {
                Clear();
                newvalue.CollectionChanged += Defects_CollectionChanged;
            }
        }

        /// <summary>
        /// Defects_CollectionChanged
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Defects_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (DefectResponsibility defect in e.NewItems)
                    OnDefectAdded(defect);
            }

            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (DefectResponsibility defect in e.OldItems)
                    OnDefectRemoved(defect);
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Clear();
                if (e.NewItems != null)
                    foreach (DefectResponsibility defect in e.NewItems)
                        OnDefectAdded(defect);
            }
        }

        /// <summary>
        /// 清理
        /// </summary>
        void Clear()
        {
            CategoryList.Clear();
            CurrentCategoryList.Clear();
            SelectedCategoryList.Clear();
            allCategoryItem.Clear();
            ResponsibilityList.Clear();
            CurrentResponsibilityList.Clear();
            SelectedValue.Clear();
        }

        #endregion

        /// <summary>
        /// 是否允许多选
        /// </summary>
        public bool AllowMultiple { get; set; }

        /// <summary>
        /// 重新加载责任分类
        /// </summary>
        bool _reloadCategory;

        /// <summary>
        /// ResponsibilityControl
        /// </summary>
        public ResponsibilityControl()
        {
            InitializeComponent();
            SelectedValue = new ObservableCollection<DefectResponsibilityItem>();
            SelectedValue.CollectionChanged += SelectedValue_CollectionChanged;

            ResponsibilityList = new List<DefectResponsibilityItem>();
            CurrentResponsibilityList = new ObservableCollection<DefectResponsibilityItem>();

            CategoryList = new List<ResponsibilityCategoryItem>();
            CurrentCategoryList = new ObservableCollection<ResponsibilityCategoryItem>();
            SelectedCategoryList = new ObservableCollection<ResponsibilityCategoryItem>();

            ctlCategory.ItemsSource = CurrentCategoryList;
            ctlDefect.ItemsSource = CurrentResponsibilityList;
            ctlNavigation.ItemsSource = SelectedCategoryList;
            ctlSelect.ItemsSource = SelectedValue;
            Responsibilities.CollectionChanged += Defects_CollectionChanged;
            Binding remarkBinding = new Binding("Remark") { Source = this };
            this.txtRemark.SetBinding(TextBox.TextProperty, remarkBinding);
        }

        /// <summary>
        /// OnDefectRemoved
        /// </summary>
        /// <param name="defect">defect</param>
        protected virtual void OnDefectRemoved(DefectResponsibility defect)
        {
            var item = ResponsibilityList.FirstOrDefault(p => p.Responsibility.Id == defect.Id);
            if (item != null)
                ResponsibilityList.Remove(item);
            if (ResponsibilityList.Count == 0)
                Clear();
        }

        /// <summary>
        /// OnDefectAdded
        /// </summary>
        /// <param name="responsibility">responsibility</param>
        protected virtual void OnDefectAdded(DefectResponsibility responsibility)
        {
            AddCategory(responsibility.CategoryId);
            var item = new DefectResponsibilityItem { Responsibility = responsibility, Control = this };
            ResponsibilityList.Add(item);
            CurrentResponsibilityList.Add(item);
        }

        /// <summary>
        /// 缺陷责任分类项目 字典
        /// </summary>
        Dictionary<double, ResponsibilityCategoryItem> allCategoryItem = new Dictionary<double, ResponsibilityCategoryItem>();

        /// <summary>
        /// 缺陷责任分类 字典
        /// </summary>
        Dictionary<double, DefectResponsibilityCategory> _allCategory;

        /// <summary>
        /// 缺陷责任分类 字典
        /// </summary>
        Dictionary<double, DefectResponsibilityCategory> AllCategory
        {
            get
            {
                if (_allCategory == null || _reloadCategory)
                {
                    _allCategory = new Dictionary<double, DefectResponsibilityCategory>();
                    var all = RT.Service.Resolve<DefectController>().GetAllDefectResponsibilityCategory();
                    all.EachNode(p =>
                    {
                        var c = p as DefectResponsibilityCategory;
                        _allCategory.Add(c.Id, c);
                        return false;
                    });
                    _reloadCategory = false;
                }

                return _allCategory;
            }
        }

        /// <summary>
        /// AddCategory
        /// </summary>
        /// <param name="categoryId">categoryId</param>
        /// <returns>ResponsibilityCategoryItem</returns>
        protected virtual ResponsibilityCategoryItem AddCategory(double categoryId)
        {
            ResponsibilityCategoryItem item;
            if (!allCategoryItem.TryGetValue(categoryId, out item))
            {
                DefectResponsibilityCategory category;
                if (!AllCategory.TryGetValue(categoryId, out category))
                {
                    //新增的缺陷责任分类需要重新加载
                    _reloadCategory = true;
                    category = AllCategory[categoryId];
                }
                item = new ResponsibilityCategoryItem { Category = category };
                allCategoryItem.Add(item.Category.Id, item);
                var pid = category.GetTreePId();
                DefectResponsibilityCategory parent;
                if (pid == null || !AllCategory.TryGetValue(pid.ConvertTo<double>(), out parent))
                {
                    CategoryList.Add(item);
                    CurrentCategoryList.Add(item);
                }
                else
                {
                    ResponsibilityCategoryItem itemParent;
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
            var selected = btn.Tag as ResponsibilityCategoryItem;
            OnCategorySelected(selected);
        }

        /// <summary>
        /// 选中分类
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnCategorySelected(ResponsibilityCategoryItem selected)
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
        protected virtual void ShowCurrentDefect(ResponsibilityCategoryItem selected = null)
        {
            CurrentResponsibilityList.Clear();
            if (selected == null)
            {
                foreach (var item in ResponsibilityList)
                    CurrentResponsibilityList.Add(item);
            }
            else
            {
                foreach (var item in FindItems(selected))
                    CurrentResponsibilityList.Add(item);
            }
        }

        /// <summary>
        /// FindItems
        /// </summary>
        /// <param name="selected">select</param>
        /// <returns>缺陷责任项目 集合</returns>
        IEnumerable<DefectResponsibilityItem> FindItems(ResponsibilityCategoryItem selected)
        {
            foreach (var item in ResponsibilityList.Where(p => p.Responsibility.CategoryId == selected.Category.Id))
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
            var selected = btn.Tag as DefectResponsibilityItem;
            OnDefectSelected(selected);
        }

        /// <summary>
        /// 取消选中缺陷责任
        /// </summary>
        /// <param name="selected">select</param>
        protected virtual void OnDefectUnselected(DefectResponsibilityItem selected)
        {
            selected.Qty = 0;
            SelectedValue.Remove(selected);
        }

        /// <summary>
        /// 选中缺陷责任
        /// </summary>
        /// <param name="selected">select</param>
        protected virtual void OnDefectSelected(DefectResponsibilityItem selected)
        {
            AddSelected(selected);
        }

        /// <summary>
        /// AddSelected
        /// </summary>
        /// <param name="selected">select</param>
        void AddSelected(DefectResponsibilityItem selected)
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
            var selected = btn.Tag as ResponsibilityCategoryItem;
            OnNavigationChanged(selected);
        }

        /// <summary>
        /// OnNavigationChanged
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnNavigationChanged(ResponsibilityCategoryItem selected)
        {
            SelectedCategoryList.Clear();
            CurrentCategoryList.Clear();
            ResponsibilityCategoryItem cate = selected;
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
            var selected = btn.Tag as DefectResponsibilityItem;
            OnDefectUnselectd(selected);
        }

        /// <summary>
        /// 取消选中缺陷责任
        /// </summary>
        /// <param name="selected">selected</param>
        protected virtual void OnDefectUnselectd(DefectResponsibilityItem selected)
        {
            SelectedValue.Remove(selected);
        }
    }

    /// <summary>
    /// 缺陷责任分类项目
    /// </summary>
    public class ResponsibilityCategoryItem
    {
        /// <summary>
        /// 缺陷责任分类
        /// </summary>
        public DefectResponsibilityCategory Category { get; set; }

        /// <summary>
        /// 缺陷责任分类项目 集合
        /// </summary>
        public ObservableCollection<ResponsibilityCategoryItem> Children { get; } = new ObservableCollection<ResponsibilityCategoryItem>();

        /// <summary>
        /// 缺陷责任分类项目
        /// </summary>
        public ResponsibilityCategoryItem Parent { get; set; }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            var item = obj as ResponsibilityCategoryItem;
            if (item == null) return false;
            return Category?.Id == item.Category?.Id;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            if (Category == null) return 0;
            return Category.GetHashCode();
        }
    }

    /// <summary>
    /// 缺陷责任项目
    /// </summary>
    public class DefectResponsibilityItem : ObservableObject
    {
        /// <summary>
        /// Control
        /// </summary>
        public ResponsibilityControl Control { get; set; }

        /// <summary>
        /// 缺陷责任
        /// </summary>
        public DefectResponsibility Responsibility
        {
            get { return GetProperty<DefectResponsibility>(); }
            set { SetProperty(value); }
        }

        /// <summary>
        /// 缺陷责任位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 不良数量,不允许输入数量时,返回1
        /// </summary>
        public double Qty
        {
            get
            {
                return GetProperty<double>();
            }

            set
            {
                SetProperty(value);
            }
        }

        /// <summary>
        /// Equals
        /// </summary>
        /// <param name="obj">obj</param>
        /// <returns>bool</returns>
        public override bool Equals(object obj)
        {
            var item = obj as DefectResponsibilityItem;
            if (item == null) return false;
            return Responsibility?.Id == item.Responsibility?.Id;
        }

        /// <summary>
        /// GetHashCode
        /// </summary>
        /// <returns>int</returns>
        public override int GetHashCode()
        {
            if (Responsibility == null) return 0;
            return Responsibility.GetHashCode();
        }
    }
}