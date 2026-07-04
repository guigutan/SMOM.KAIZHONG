using DevExpress.Data.Helpers;
using DevExpress.Xpf.Grid;
using SIE.Barcodes.WipBatchs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.MES.BatchWIP;
using SIE.MES.OnOffDuty;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using System;
using System.Linq;

namespace SIE.Wpf.MES.OnOffDuty.Commands
{
    /// <summary>
    /// 选择批次命令
    /// </summary>
    [Command(ImageName = "CheckmarkPencil", Label = "上下岗补录", ToolTip = "上下岗补录", GroupType = CommandGroupType.Edit)]
    public class OnOffDutyInputCommand : DetailViewCommand
    {
        /// <summary>
        /// 是否可执行的逻辑
        /// </summary>
        /// <param name="view">视图对象</param>
        /// <returns>返回是否可执行</returns>
        public override bool CanExecute(DetailLogicalView view)
        {
            return base.CanExecute(view) && (view.Current is OnOffDutyViewModel);
        }

        /// <summary>
        /// 执行具体的逻辑。
        /// </summary>
        /// <param name="view">视图对象</param>
        public override void Execute(DetailLogicalView view)
        {
            var vm = view.Current as OnOffDutyViewModel;
            ValidateWorkCell(vm);

            var template = new ListUITemplate(typeof(OnOffDutyRecrods), OnOffDutyRecrodsViewConfig.ListView, view.ModuleKey);// "SelectList");
            template.BlocksDefined += Template_BlocksDefined;
            var ui = template.CreateUI();
            var listView = ui.MainView as ListLogicalView;
            var control = listView.Control as GridControl;
            var tableView = control.View as TableView;
            tableView.AllowEditing = true;
            
            tableView.ShowCheckBoxSelectorColumn = true;
            listView.IsReadOnly = MetaModel.ReadOnlyStatus.None;
            listView.Control.SelectionMode = MultiSelectMode.MultipleRow;
            ui.MainView.IsReadOnly = MetaModel.ReadOnlyStatus.None;
            ui.MainView.RefreshControl();
            ui.MainView.QueryView?.TryExecuteQuery();
            listView.Control.MouseDoubleClick += (s, e) =>
            {
                listView.IsReadOnly = MetaModel.ReadOnlyStatus.None;
                listView.Control.View.ShowEditor();
                listView.RefreshControl();
            };
            CRT.Workbench.ShowDialog(ui, w =>
              {
                  ui.MainView.IsReadOnly= MetaModel.ReadOnlyStatus.None;
                  w.Title = "上下岗补录".L10N();
                  w.Commands.Add("批量补录时间".L10N());
                  w.Closing += (s, e) =>
                  {
                      if (w.Result == 0 && listView.SelectedEntities.Count < 1)
                      {
                          CRT.MessageService.ShowError("请选择员工".L10N());
                          e.Cancel = true;
                          return;
                      }
                      if (w.Result == 2)//设置上下岗时间
                      {
                          if (listView.SelectedEntities.Count < 1)
                          {
                              CRT.MessageService.ShowError("请选择员工".L10N());
                              e.Cancel = true;
                              return;
                          }

                          OnOffDutyTimeSettingViewModel onOffDutyTimeSettingViewModel = new OnOffDutyTimeSettingViewModel();

                          var template1 = new DetailsUITemplate(typeof(OnOffDutyTimeSettingViewModel), OnOffDutyTimeSettingViewModeliewConfig.DetailsView, view.ModuleKey);
                          var ui1 = template1.CreateUI();
                          ui1.MainView.Data = onOffDutyTimeSettingViewModel;

                          CRT.Workbench.ShowDialog(ui1, ww =>
                          {
                              ww.Title = "批量设置上下岗时间".L10N();
                              ww.Height = 150;
                              ww.Width = 350;
                              ww.Closing += (s2, e2) =>
                              {
                                  if (ww.Result == 0)
                                  {
                                      try
                                      {
                                          if (!onOffDutyTimeSettingViewModel.OnDutyTime.HasValue)
                                          {
                                              ClientRuntime.MessageService.ShowError("上岗补录时间必输！".L10N());
                                              e2.Cancel = true;
                                              e.Cancel = true;
                                              return;
                                          }
                                          if (!onOffDutyTimeSettingViewModel.OffDutyTime.HasValue)
                                          {
                                              ClientRuntime.MessageService.ShowError("下岗补录时间必输！".L10N());
                                              e2.Cancel = true;
                                              e.Cancel = true;
                                              return;
                                          }
                                          if (onOffDutyTimeSettingViewModel.OffDutyTime <= onOffDutyTimeSettingViewModel.OnDutyTime)
                                          {
                                              ClientRuntime.MessageService.ShowError("上岗补录时间不能早于下岗补录时间!".L10N());
                                              e2.Cancel = true;
                                              e.Cancel = true;
                                              return;
                                          }
                                          if (DateTime.Now <= onOffDutyTimeSettingViewModel.OffDutyTime)
                                          {
                                              ClientRuntime.MessageService.ShowError("下岗补录时间不能早于当前时间!".L10N());
                                              e2.Cancel = true;
                                              e.Cancel = true;
                                              return;
                                          }


                                          foreach (var item in listView.SelectedEntities)
                                          {
                                              var selected = item as OnOffDutyRecrods;
                                              if (selected != null)
                                              {
                                                  selected.OnDutyTime = onOffDutyTimeSettingViewModel.OnDutyTime;
                                                  selected.OffDutyTime = onOffDutyTimeSettingViewModel.OffDutyTime;
                                                  selected.OnDutyDuration = (selected.OffDutyTime - selected.OnDutyTime).Value.TotalMinutes;
                                                  selected.OnOffDutyType = OnOffDutyType.OffDuty;
                                                  selected.ResourceId = vm.Workstation.ResourceId.Value;
                                                  selected.StationId = vm.Workstation.StationId.Value;
                                                  selected.ProcessId = vm.Workstation.ProcessId.Value;
                                                  selected.IsAdditionalRecording = true;
                                              }
                                          }
                                          e.Cancel = true;

                                      }
                                      catch (Exception ex)
                                      {
                                          ClientRuntime.MessageService.ShowException(ex);
                                          e2.Cancel = true;
                                      }
                                  }
                                  if (ww.Result == 1)
                                  {
                                      e.Cancel = true;
                                  }
                              };
                          });
                      }
                      if (w.Result == 0)//设置上下岗时间
                      {
                          EntityList<OnOffDutyRecrods> selectedList = new EntityList<OnOffDutyRecrods>();
                          foreach (var item in listView.SelectedEntities)
                          {
                              var selected = item as OnOffDutyRecrods;
                              if (selected != null)
                              {
                                  if (!selected.OnDutyTime.HasValue)
                                  {
                                      ClientRuntime.MessageService.ShowError("上岗补录时间必输！".L10N());
                                      e.Cancel = true;
                                      return;
                                  }
                                  if (!selected.OffDutyTime.HasValue)
                                  {
                                      ClientRuntime.MessageService.ShowError("下岗补录时间必输！".L10N());
                                      e.Cancel = true;
                                      return;
                                  }
                                  if (selected.OffDutyTime <= selected.OnDutyTime)
                                  {
                                      ClientRuntime.MessageService.ShowError("上岗补录时间不能早于下岗补录时间!".L10N());
                                      e.Cancel = true;
                                      return;
                                  }
                                  if (DateTime.Now <= selected.OffDutyTime)
                                  {
                                      ClientRuntime.MessageService.ShowError("下岗补录时间不能早于当前时间!".L10N());
                                      e.Cancel = true;
                                      return;
                                  }
                                  selected.OnDutyDuration = (selected.OffDutyTime - selected.OnDutyTime).Value.TotalMinutes;
                                  selected.OnOffDutyType = OnOffDutyType.OffDuty;
                                  selected.ResourceId = vm.Workstation.ResourceId.Value;
                                  selected.StationId = vm.Workstation.StationId.Value;
                                  selected.ProcessId = vm.Workstation.ProcessId.Value;
                                  selected.IsAdditionalRecording = true;
                                  selectedList.Add(selected);
                              }
                          }
                          if (selectedList.Any())
                          {
                              RT.Service.Resolve<OnOffDutyController>().SetOnOffDuty(selectedList);
                              ClientRuntime.MessageService.ShowMessage("补录完成！".L10N());
                          }
                          else {
                              ClientRuntime.MessageService.ShowError("请选择需要补录的员工!".L10N());
                              e.Cancel = true;
                          }
                      }
                  };
              });
        }

        /// <summary>
        /// 验证工作单元
        /// </summary>
        /// <param name="vm">采集视图模型</param>
        void ValidateWorkCell(OnOffDutyViewModel vm)
        {
            try
            {
                vm.GetWorkcell();
            }
            catch
            {
                throw new ValidationException("工作单元信息不能为空，请维护".L10N());
            }
        }

        /// <summary>
        /// 块定义
        /// </summary>
        /// <param name="sender">所有者</param>
        /// <param name="e">参数</param>
        private void Template_BlocksDefined(object sender, CodeBlocksDefinedEventArgs e)
        {
            e.Blocks.Surrounders.Clear();
            var conditionBlock = new ConditionBlock(typeof(OnOffDutyRecrodsInputCriteria), ViewConfig.QueryView);
            e.Blocks.Surrounders.Add(conditionBlock);
        }
    }
}