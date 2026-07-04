using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ESop.Displays;
using SIE.ManagedProperty;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources.WipResources;
using SIE.Wpf.Dashboard;
using System;
using System.Windows;
using System.Windows.Forms;

namespace SIE.Wpf.ESop
{
    /// <summary>
    /// 工作单元信息选择
    /// </summary>
    [RootEntity]
    [Label("工序选择")]
    public class ESopWorkstationSelector : ViewModel
    {
        #region 资源 Resource 
        /// <summary>
        /// 产线Id
        /// </summary>
        [Label("资源")]
        [Required]
        public static readonly IRefIdProperty ResourceIdProperty = P<ESopWorkstationSelector>.RegisterRefId(e => e.ResourceId, ReferenceType.Normal);

        /// <summary>
        /// 资源Id
        /// </summary>
        public double? ResourceId
        {
            get { return (double?)this.GetRefNullableId(ResourceIdProperty); }
            set { this.SetRefNullableId(ResourceIdProperty, value); }
        }

        /// <summary>
        /// 资源
        /// </summary>
        public static readonly RefEntityProperty<WipResource> ResourceProperty = P<ESopWorkstationSelector>.RegisterRef(e => e.Resource, ResourceIdProperty);

        /// <summary>
        /// 资源
        /// </summary>
        public WipResource Resource
        {
            get { return this.GetRefEntity(ResourceProperty); }
            set { this.SetRefEntity(ResourceProperty, value); }
        }
        #endregion

        #region 显示点 DisplayPoint
        /// <summary>
        /// 显示点Id
        /// </summary>
        [Label("显示点")]
        [Required]
        public static readonly IRefIdProperty DisplayPointIdProperty = P<ESopWorkstationSelector>.RegisterRefId(e => e.DisplayPointId, ReferenceType.Normal);

        /// <summary>
        /// 显示点Id
        /// </summary>
        public double? DisplayPointId
        {
            get { return (double?)this.GetRefNullableId(DisplayPointIdProperty); }
            set { this.SetRefNullableId(DisplayPointIdProperty, value); }
        }

        /// <summary>
        /// 显示点
        /// </summary>
        public static readonly RefEntityProperty<DisplayPoint> DisplayPointProperty =
            P<ESopWorkstationSelector>.RegisterRef(e => e.DisplayPoint, DisplayPointIdProperty);

        /// <summary>
        /// 显示点
        /// </summary>
        public DisplayPoint DisplayPoint
        {
            get { return this.GetRefEntity(DisplayPointProperty); }
            set { this.SetRefEntity(DisplayPointProperty, value); }
        }
        #endregion

        /// <summary>
        /// 工作站信息
        /// </summary>
        public EsopWorkstation Workstation { get; set; }

        /// <summary>
        /// 构造函数
        /// </summary>
        public ESopWorkstationSelector() { }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="workstation">工作站信息</param>
        public ESopWorkstationSelector(EsopWorkstation workstation)
        {
            Workstation = workstation;
            ResourceId = workstation.ResourceId;
            DisplayPointId = workstation.DisplayPointId;
        }

        /// <summary>
        /// 属性变更时触发
        /// </summary>
        /// <param name="e">事件参数</param>
        protected override void OnPropertyChanged(ManagedPropertyChangedEventArgs e)
        {
            if (e.Property == ESopWorkstationSelector.ResourceProperty)
                this.DisplayPoint = null;
            base.OnPropertyChanged(e);
        }

        /// <summary>
        /// 选择工作单元
        /// </summary>
        /// <param name="workstation">工作站信息</param>
        /// <param name="window">窗口</param>
        /// <param name="player">操作器</param>
        public static void SelectOperation(EsopWorkstation workstation, Window window = null, DashboardPlayerControl player = null)
        {
            var model = new ESopWorkstationSelector(workstation);
            var template = new DetailsUITemplate(typeof(ESopWorkstationSelector));
            var ui = template.CreateUI();
            ui.MainView.Data = model;

            CRT.Workbench.ShowDialog(Guid.NewGuid().ToString(), ui.Control, w =>
            {
                //设置显示点弹出框显示在最顶层
                (w as Workbench.DialogContent).Topmost = true;
                w.Title = "选择显示点".L10N();
                w.Width = 400;

                w.Height = 200;
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        var broken = model.Validate(ValidatorActions.None);
                        if (broken.Count > 0)
                        {
                            CRT.MessageService.ShowMessage(broken.ToString());
                            e.Cancel = true;
                        }
                        else
                        {
                            //赋值顺序不能更换，工位信息选择触发相应的事件
                            workstation.Resource = model.Resource;
                            workstation.DisplayPoint = model.DisplayPoint;
                            if (window != null)
                            {
                                var screenNum = (model.DisplayPoint.PlayScreenNum == null || !model.DisplayPoint.PlayScreenNum.HasValue) ? 0 : model.DisplayPoint.PlayScreenNum.Value;
                                var left = SetWindowIndex(screenNum, window);
                                var meta = WPFModuleMetaExt.GetModuleMeta(player) as WPFParameterModuleMeta;
                                if (player != null && meta != null && meta.IsFullScreen)//如果是全屏 由于框架会执行一次全屏，所以导致此处需要还原全屏再执行全屏
                                {
                                    player.ShiftFullScreen((int)left);
                                    if (meta.IsFullScreen)
                                    {
                                        player.ShiftFullScreen((int)left);
                                    }
                                }
                                window.Left = left;
                            }

                        }
                    }
                };
            });
        }

        /// <summary>
        /// 设置窗口位置
        /// </summary>
        /// <param name="numScreen"></param>
        /// <param name="window"></param>
        /// <exception cref="ValidationException"></exception>
        public static double SetWindowIndex(int numScreen, Window window)
        {
            var screenQty = Screen.AllScreens.Length;

            if (numScreen >= screenQty)
            {

                throw new ValidationException("屏幕索引超出屏幕数".L10N());
            }
            window.Hide();
            var scr = Screen.AllScreens[numScreen];
            window.ShowActivated = true;
            window.WindowState = WindowState.Normal;
            window.Left = scr.Bounds.Location.X;//此处赋值可能无效 故传出LEFT
            window.Show();
            window.WindowState = WindowState.Maximized;
            return scr.Bounds.Location.X;
        }
    }



    /// <summary>
    /// 工作单元信息视图配置
    /// </summary>
    public class WorkstationSelectorViewConfig : WPFViewConfig<ESopWorkstationSelector>
    {
        /// <summary>
        /// 配置默认视图
        /// </summary>
        protected override void ConfigView()
        {
            //重写父类
        }

        /// <summary>
        /// 配置明细视图
        /// </summary>
        protected override void ConfigDetailsView()
        {
            View.ClearCommands();
            View.HasDetailColumnsCount(1);
            View.Property(p => p.Resource).UseESopEnterpriseLookUpEditor(x => x.DisplayMember = WipResource.NameProperty.Name);
            View.Property(p => p.DisplayPoint).UseDisplayPointLookUpEditor(x =>
            {
                x.ReloadDataOnPopping = true;
                x.DisplayMember = DisplayPoint.NameProperty.Name;
            });
        }
    }
}