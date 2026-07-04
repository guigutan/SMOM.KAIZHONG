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
    /// AndonButtonControl.xaml 的交互逻辑
    /// </summary>
    public partial class AndonButtonControl : UserControl
    {

        /// <summary>
        /// 显示安灯触发对话框的委托
        /// </summary>
        /// <param name="andonId"></param>
        public delegate void ShowAndonTriggerDialogDelegate(object andonId);

        /// <summary>
        /// 
        /// </summary>
        public AndonButtonControl()
        {
            InitializeComponent();

            AndonListOfPerson = new ObservableCollection<SIE.Andon.Andons.Andon>();
            AndonListOfMachine = new ObservableCollection<SIE.Andon.Andons.Andon>();
            AndonListOfMaterial = new ObservableCollection<SIE.Andon.Andons.Andon>();
            AndonListOfMethod = new ObservableCollection<SIE.Andon.Andons.Andon>();
            AndonListOfEvnironment = new ObservableCollection<SIE.Andon.Andons.Andon>();
            AndonListOfTest = new ObservableCollection<SIE.Andon.Andons.Andon>();

            ctlAndonPerson.ItemsSource = AndonListOfPerson;
            ctlAndonMachine.ItemsSource = AndonListOfMachine;
            ctlAndonMaterial.ItemsSource = AndonListOfMaterial;
            ctlAndonMethod.ItemsSource = AndonListOfMethod;
            ctlAndonEvnironment.ItemsSource = AndonListOfEvnironment;
            ctlAndonTest.ItemsSource = AndonListOfTest;
        }

        #region 数据
        /// <summary>
        /// 大类为人的安灯列表
        /// </summary>
        protected virtual ObservableCollection<SIE.Andon.Andons.Andon> AndonListOfPerson { get; }

        /// <summary>
        /// 大类为机的安灯列表
        /// </summary>
        protected virtual ObservableCollection<SIE.Andon.Andons.Andon> AndonListOfMachine { get; }

        /// <summary>
        /// 大类为料的安灯列表
        /// </summary>
        protected virtual ObservableCollection<SIE.Andon.Andons.Andon> AndonListOfMaterial { get; }

        /// <summary>
        /// 大类为法的安灯列表
        /// </summary>
        protected virtual ObservableCollection<SIE.Andon.Andons.Andon> AndonListOfMethod { get; }

        /// <summary>
        /// 大类为环的安灯列表
        /// </summary>
        protected virtual ObservableCollection<SIE.Andon.Andons.Andon> AndonListOfEvnironment { get; }

        /// <summary>
        /// 大类为测的安灯列表
        /// </summary>
        protected virtual ObservableCollection<SIE.Andon.Andons.Andon> AndonListOfTest { get; }

        /// <summary>
        /// 安灯项目列表
        /// </summary>
        public EntityList<SIE.Andon.Andons.Andon> Andons
        {
            get { return (EntityList<SIE.Andon.Andons.Andon>)GetValue(AndonsProperty); }
            set { SetValue(AndonsProperty, value); }
        }

        /// <summary>
        /// 安灯项目列表依赖属性
        /// </summary>
        public static readonly DependencyProperty AndonsProperty =
            DependencyProperty.Register("Andons", typeof(EntityList<SIE.Andon.Andons.Andon>), typeof(AndonButtonControl), new PropertyMetadata(new EntityList<SIE.Andon.Andons.Andon>(),
                (s, e) => { ((AndonButtonControl)s).OnValueChanged(e); }));

        /// <summary>
        /// 值变更
        /// </summary>
        /// <param name="e">e</param>
        protected virtual void OnValueChanged(DependencyPropertyChangedEventArgs e)
        {
            var oldvalue = e.OldValue as EntityList<SIE.Andon.Andons.Andon>;
            if (oldvalue != null)
            {
                oldvalue.CollectionChanged -= Andons_CollectionChanged;
            }

            var newvalue = e.NewValue as EntityList<SIE.Andon.Andons.Andon>;
            if (newvalue != null)
            {
                Clear();
                newvalue.CollectionChanged += Andons_CollectionChanged;
            }
        }

        /// <summary>
        /// 安灯项目列表变更
        /// </summary>
        /// <param name="sender">sender</param>
        /// <param name="e">e</param>
        private void Andons_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Add || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (SIE.Andon.Andons.Andon andon in e.NewItems)
                {
                    OnAndonAdded(andon);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Remove || e.Action == NotifyCollectionChangedAction.Replace)
            {
                foreach (SIE.Andon.Andons.Andon andon in e.OldItems)
                {
                    OnAndonRemoved(andon);
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Reset)
            {
                Clear();
                if (e.NewItems != null)
                {
                    foreach (SIE.Andon.Andons.Andon andon in e.NewItems)
                    {
                        OnAndonAdded(andon);
                    }
                }
            }
        }

        /// <summary>
        /// 清空内存数据
        /// </summary>
        void Clear()
        {
            AndonListOfPerson.Clear();
            AndonListOfMachine.Clear();
            AndonListOfMaterial.Clear();
            AndonListOfMethod.Clear();
            AndonListOfEvnironment.Clear();
            AndonListOfTest.Clear();
        }


        /// <summary>
        /// 从安灯事件列表移除安灯事件
        /// </summary>
        /// <param name="andon">安灯事件</param>
        protected virtual void OnAndonRemoved(SIE.Andon.Andons.Andon andon)
        {
            var item = AndonListOfPerson.FirstOrDefault(p => p.Id == andon.Id);

            if (item != null)
            {
                AndonListOfPerson.Remove(item);
            }

            var itemMachine = AndonListOfMachine.FirstOrDefault(p => p.Id == andon.Id);

            if (itemMachine != null)
            {
                AndonListOfMachine.Remove(item);
            }

            var itemMaterial = AndonListOfMaterial.FirstOrDefault(p => p.Id == andon.Id);

            if (itemMaterial != null)
            {
                AndonListOfMaterial.Remove(item);
            }

            var itemMethod = AndonListOfMethod.FirstOrDefault(p => p.Id == andon.Id);

            if (itemMethod != null)
            {
                AndonListOfMethod.Remove(item);
            }

            var itemEvnironment = AndonListOfEvnironment.FirstOrDefault(p => p.Id == andon.Id);

            if (itemEvnironment != null)
            {
                AndonListOfEvnironment.Remove(item);
            }

            var itemTest = AndonListOfTest.FirstOrDefault(p => p.Id == andon.Id);
            if (itemTest != null)
            {
                AndonListOfTest.Remove(item);
            }
        }

        /// <summary>
        /// 增加安灯到安灯列表
        /// </summary>
        /// <param name="andon">安灯</param>
        protected virtual void OnAndonAdded(SIE.Andon.Andons.Andon andon)
        {
            switch (andon.AndonClass)
            {
                case SIE.Andon.Andons.Enum.AndonTypeClass.Person:
                    AndonListOfPerson.Add(andon);
                    break;
                case SIE.Andon.Andons.Enum.AndonTypeClass.Machine:
                    AndonListOfMachine.Add(andon);
                    break;
                case SIE.Andon.Andons.Enum.AndonTypeClass.Material:
                    AndonListOfMaterial.Add(andon);
                    break;
                case SIE.Andon.Andons.Enum.AndonTypeClass.Method:
                    AndonListOfMethod.Add(andon);
                    break;
                case SIE.Andon.Andons.Enum.AndonTypeClass.Ring:
                    AndonListOfEvnironment.Add(andon);
                    break;
                case SIE.Andon.Andons.Enum.AndonTypeClass.Test:
                    AndonListOfTest.Add(andon);
                    break;
                default:
                    break;
            }
        }
        #endregion


        /// <summary>
        /// 显示安灯管理对话框
        /// </summary>
        public ShowAndonTriggerDialogDelegate ShowAndonTriggerDialog { get; set; }

        private void btnAndon_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as System.Windows.Controls.Button;
            var tagString = btn.Tag;
            if (tagString.ToString().IsNullOrEmpty())
            {
                return;
            }

            var andonManageId = tagString;

            if (ShowAndonTriggerDialog != null)
            {
                ShowAndonTriggerDialog(andonManageId);
            }
        }
    }
}
