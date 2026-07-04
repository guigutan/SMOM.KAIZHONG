using DevExpress.Mvvm.Native;
using SIE.Andon.Andons;
using SIE.Andon.Andons.Configs;
using SIE.Common;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.ObjectModel;
using SIE.Resources;
using SIE.Resources.WipResources;
using SIE.Security;
using SIE.Tech.Stations;
using SIE.Wpf.MES.WIP;
using SIE.Wpf.Workbench;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Threading;

namespace SIE.Wpf.Andon
{
    /// <summary>
    /// 安灯管理
    /// </summary>
    [RootEntity, Serializable]
    [EntityWithConfig(typeof(RefreshAndonManageConfig))]
    [Label("安灯管理")]
    public class AndonManageViewModel : KZDataCollectionViewModel
    {
        /// <summary>
        /// 安灯管理构造函数
        /// </summary>
        public AndonManageViewModel()
        {
            InitWorkstation();
        }

        public AndonManageViewModel(bool isWindow)
        {
            InitWorkstationNotWindow();
        }

        /// <summary>
        /// 加载
        /// </summary>
        public override void Onload()
        {
            base.Onload();

            timer.Tick += new EventHandler(Timer_Tick);

            //重置定时器
            SetRefreshTimer();
        }

        /// <summary>
        /// 关闭
        /// </summary>
        public override void OnClose()
        {
            base.OnClose();

            if (timer.IsEnabled)
            {
                timer.Stop();
            }

            timer.Tick -= new EventHandler(Timer_Tick);
        }

        #region 工作单元信息

        /// <summary>
        /// 员工变更事件
        /// </summary>
        /// <param name="employee"></param>
        protected override void EmployeeChanged(Employee employee)
        {
            base.EmployeeChanged(employee);

            //异步加载安灯事件列表            
            AsyncExecute(() =>
            {
                GetAndons();

            });
        }

        /// <summary>
        /// 获取用户有权限且启用的安灯类型下启用的安灯
        /// </summary>
        public void GetAndons()
        {
            //var workcell = this.GetWorkcell();

            AndonList.Clear();

            if (KZWorkstation.EmployeeId != 0)
            {
                var andons = RT.Service.Resolve<AndonManageController>()
                    .GetAndonsByEmployeeId((double)KZWorkstation.EmployeeId);

                AndonList.Clear();

                AndonList.AddRange(andons);
            }
        }

        #endregion

        /// <summary>
        /// 定时器
        /// </summary>
        protected DispatcherTimer timer = new DispatcherTimer();

        /// <summary>
        /// 计时器触发一次
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            //异步加载安灯事件列表
            AsyncExecute(() =>
            {
                GetAndonManageList();
            });
        }

        /// <summary>
        /// 上料刷新计时器设置
        /// </summary>
        protected virtual void SetRefreshTimer()
        {
            if (timer.IsEnabled)
            {
                timer.Stop();
            }

            var config = ConfigService.GetConfig(new RefreshAndonManageConfig(), typeof(AndonManageViewModel));

            if (config.RefreshTime > 0)
            {
                timer.Interval = TimeSpan.FromSeconds(config.RefreshTime); //设置刷新的间隔时间
                timer.Start();
            }
        }

        /// <summary>
        /// 安灯事件列表
        /// </summary>
        public EntityList<AndonManage> AndonEventList { get; set; } = new EntityList<AndonManage>();

        /// <summary>
        /// 安灯列表
        /// </summary>
        public EntityList<SIE.Andon.Andons.Andon> AndonList { get; set; } = new EntityList<SIE.Andon.Andons.Andon>();

        /// <summary>
        /// 查询安灯事件
        /// </summary>
        public void GetAndonManageList()
        {
            //var workcell = this.GetWorkcell();
            if (KZWorkstation.EmployeeId != 0 && KZWorkstation.ResourceId != 0)
            {
                var andonManages = RT.Service.Resolve<AndonManageController>()
                    .GetAndonManages((double)KZWorkstation.EmployeeId, (double)KZWorkstation.ResourceId);

                AndonEventList.Clear();

                AndonEventList.AddRange(andonManages.OrderBy(x => x.CreateDate));
            }
        }

        /// <summary>
        /// 显示安灯管理对话框
        /// </summary>
        /// <param name="andonManageId"></param>
        public void ShowAndonManageDialog(double andonManageId)
        {
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(AndonManageViewModel));

            var andonManage = RT.Service.Resolve<AndonManageController>().GetAndonManage(andonManageId);

            var template = new DetailsUITemplate(typeof(AndonManage), ViewConfig.DetailsView, moduleKey);
            var ui = template.CreateUI();
            var orderByList = andonManage.MessageSendList.OrderByDescending(a => a.MessageSendTime).AsEntityList();
            andonManage.MessageSendList.Clear();
            andonManage.MessageSendList.AddRange(orderByList);
            ui.MainView.Data = andonManage;

            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "安灯管理".L10N();
                w.Width = 1000;
                w.Height = 700;
                var dc = (w as DialogContent);
                dc.Loaded += (s, e) => { WipLayoutHelper.ResizeChildrenStyle(dc); };
                w.Closed += (o, e) =>
                {
                    //关闭对话框时，重新加载安灯事件列表
                    //异步加载安灯事件列表
                    AsyncExecute(() =>
                    {
                        GetAndonManageList();
                    });
                };
            });
        }

        /// <summary>
        /// 工位变更时，查询安灯事件列表
        /// </summary>
        /// <param name="station"></param>
        protected override void StationChanged(Station station)
        {
            base.StationChanged(station);

            //异步加载安灯事件列表
            AsyncExecute(() =>
            {
                GetAndonManageList();
            });

            //异步加载安灯事件列表            
            AsyncExecute(() =>
            {
                GetAndons();

            });
        }

        protected override void ResourceChanged(WipResource resource)
        {
            base.ResourceChanged(resource);
            //异步加载安灯事件列表
            AsyncExecute(() =>
            {
                GetAndonManageList();
            });

            //异步加载安灯事件列表            
            AsyncExecute(() =>
            {
                GetAndons();

            });
        }

        /// <summary>
        /// 显示安灯触发对话框
        /// </summary>
        /// <param name="andonId"></param>
        public void ShowAndonTriggerDialog(object andonId)
        {
            var moduleKey = RT.Service.Resolve<IFindModule>().FindModuleKey(typeof(AndonManageViewModel));

            //var workCell = GetWorkcell();
            var andonManage = RT.Service.Resolve<AndonManageController>().CreateAndonManage(andonId, 0, 0, (double)KZWorkstation.ResourceId);

            //andonManage.ProcessId = workCell.ProcessId;

            var template = new DetailsUITemplate(typeof(AndonManage), AndonManageViewConfig.AndonTriggerViewGroup, moduleKey);
            var ui = template.CreateUI();
            ui.MainView.Data = andonManage;
            // 叫料生成备料单
            var isAskMaterial = andonManage.AskMaterial;
            ui.MainView.ChildrenViews[0].IsVisible = isAskMaterial;
            CRT.Workbench.ShowDialog(ui, w =>
            {
                w.Title = "安灯触发".L10N();
                w.Height = isAskMaterial ? 600 : 450;
                w.Width = 1000;
                w.Height = 600;
                var dc = (w as DialogContent);
                dc.Loaded += (s, e) => { WipLayoutHelper.ResizeChildrenStyle(dc); };
                w.Closing += (s, e) =>
                {
                    if (w.Result == 0)
                    {
                        try
                        {
                            EntityList<AndonManageCallMaterial> andonManageCallMaterials = ui.MainView.ChildrenViews[0].Data as EntityList<AndonManageCallMaterial>;
                            foreach (var item in andonManageCallMaterials)
                            {
                                item.ConsumeType = item.ConsumeModeView;
                            }

                            List<string> problemDescs = new List<string>();
                            if (!andonManage.ProblemDesc.IsNullOrEmpty())
                                problemDescs.Add(andonManage.ProblemDesc);
                            if (andonManage.GeneralProbDtlId > 0)
                                problemDescs.Add(andonManage.GeneralProbDtl.Desc);
                            andonManage.ProblemDesc = string.Join(";", problemDescs);

                            RT.Service.Resolve<AndonManageController>().SaveAndonAndItemDetailAsync(andonManage, andonManageCallMaterials);
                            //异步加载安灯事件列表
                            AsyncExecute(() =>
                            {
                                GetAndonManageList();
                            });
                        }
                        catch (Exception ex)
                        {
                            e.Cancel = true;
                            ShowError(ex);
                            throw new ValidationException($"发送失败:"+ex.Message);
                        }
                    }
                };
            });
        }
    }
}
