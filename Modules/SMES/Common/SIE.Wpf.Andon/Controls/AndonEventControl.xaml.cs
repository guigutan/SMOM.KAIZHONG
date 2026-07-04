using SIE.Andon.Andons;
using SIE.Domain;
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.Andon.Controls
{

    /// <summary>
    /// 显示安灯管理对话框
    /// </summary>
    /// <param name="andonManageId"></param>
    public delegate void ShowAndonManageDialogDelegate(double andonManageId);

    /// <summary>
    /// AndonEventControl.xaml 的交互逻辑
    /// </summary>
    public partial class AndonEventControl : UserControl
    {
        /// <summary>
        /// 
        /// </summary>
        public AndonEventControl()
        {
            InitializeComponent();
            AndonManageList = new ObservableCollection<AndonManage>();

            ctlAndonEvent.ItemsSource = AndonManageList;
        }

        #region 数据


        /// <summary>
        /// 当前列表中的缺陷项目
        /// </summary>
        protected virtual ObservableCollection<AndonManage> AndonManageList { get; }

        /// <summary>
        /// 安灯事件列表
        /// </summary>
        public EntityList<AndonManage> AndonEvents
        {
            get { return (EntityList<AndonManage>)GetValue(AndonEventsProperty); }
            set { SetValue(AndonEventsProperty, value); }
        }

        /// <summary>
        /// 安灯事件列表
        /// </summary>
        public static readonly DependencyProperty AndonEventsProperty =
            DependencyProperty.Register("AndonEvents", typeof(EntityList<AndonManage>), typeof(AndonEventControl), new PropertyMetadata(new EntityList<AndonManage>(),
                (s, e) => { ((AndonEventControl)s).OnValueChanged(e); }));

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="e">e</param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as EntityList<AndonManage>;
            if (oldvalue != null)
            {
                oldvalue.CollectionChanged -= AndonManages_CollectionChanged;
            }

            var newvalue = e.NewValue as EntityList<AndonManage>;
            if (newvalue != null)
            {
                Clear();
                newvalue.CollectionChanged += AndonManages_CollectionChanged;
            }
        }

        /// <summary>
        /// 安灯事件列表变更
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void AndonManages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (AndonManage andonManage in e.NewItems)
                {
                    OnAndonManageAdded(andonManage);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (AndonManage andonManage in e.OldItems)
                {
                    OnDefectRemoved(andonManage);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Clear();
                if (e.NewItems != null)
                {
                    foreach (AndonManage andonManage in e.NewItems)
                    {
                        OnAndonManageAdded(andonManage);
                    }
                }
            }
        }

        /// <summary>
        /// 清空内存数据
        /// </summary>
        void Clear()
        {
            AndonManageList.Clear();
        }


        /// <summary>
        /// 从安灯事件列表移除安灯事件
        /// </summary>
        /// <param name="andonManage">安灯事件</param>
        protected virtual void OnDefectRemoved(AndonManage andonManage)
        {
            var item = AndonManageList.FirstOrDefault(p => p.Id == andonManage.Id);

            if (item != null)
            {
                AndonManageList.Remove(item);
            }
        }

        /// <summary>
        /// 增加安灯事件到安灯事件列表
        /// </summary>
        /// <param name="andonManage">安灯事件</param>
        protected virtual void OnAndonManageAdded(AndonManage andonManage)
        {
            AndonManageList.Add(andonManage);
        }
        #endregion

        /// <summary>
        /// 安灯事件单击处理
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAndonEvent_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            var tagString = btn.Tag.ToString();
            if (tagString.IsNullOrEmpty())
            {
                return;
            }

            var andonManageId = double.Parse(tagString);

            if (ShowAndonManageDialog != null)
            {
                ShowAndonManageDialog(andonManageId);
            }
        }

        /// <summary>
        /// 显示安灯管理对话框
        /// </summary>
        public ShowAndonManageDialogDelegate ShowAndonManageDialog { get; set; }
    }
}
