using DevExpress.XtraWaitForm;
using SIE.Domain;
using SIE.ESop.Documents;
using SIE.MetaModel.View;
using SIE.Wpf.Command;
using SIE.Wpf.ESop.Editors;
using System;
using DevExpress.Utils;
using SIE.Wpf.Controls.WaitProgress;
using System.Threading;
using DevExpress.XtraScheduler.Outlook.Interop;
using System.Threading.Tasks;
using DevExpress.XtraBars.Customization;
using System.Windows.Threading;
using SIE.View.Workbench;
using System.ComponentModel;
using System.Windows;

namespace SIE.Wpf.ESop.Documents.Commands
{
    /// <summary>
    /// 文档集添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加数据", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class AddDocumentCollectionCommand : ListAddCommand
    {
        /// <summary>
        /// 当前添加实体对象
        /// </summary>
        private DocumentCollection docSet;

        /// <summary>
        /// 此处与父类方法一致，唯一不同点在于修改了tab页签的标题，进行了多语言化
        /// </summary>
        /// <param name="entity"></param>
        protected override void ShowView(Entity entity)
        {
            string key = ClientRuntime.Workbench.CreateKey("DetailsView", entity.GetType(), entity);
            if (base.View.Parent != null)
            {
                ControlResult ui = CreateUI();
                if (ui.MainView.CommandsContainer != null)
                {
                    ui.MainView.CommandsContainer.Visibility = Visibility.Collapsed;
                }

                BindData(ui, entity);
                ClientRuntime.Workbench.ShowDialog(key, ui.Control, delegate (IDialogContent w)
                {
                    w.Title = GetEditViewTitle(entity);//变更处
                    OnDialogShowing(w, ui);
                    w.Closing += delegate (object? o, CancelEventArgs e)
                    {
                        if (w.Result == 0)
                        {
                            try
                            {
                                if (base.View.CanFormSave())
                                {
                                    FromDoSave(entity);
                                    ui.MainView.Data.MarkSaved();
                                }

                                base.View.Data.Add(entity);
                                if (base.View.CanFormSave() && base.View.Parent != null)
                                {
                                    base.View.Parent.Current.MarkSaved();
                                    base.View.Parent.QueryView?.TryExecuteQuery();
                                }

                                return;
                            }
                            catch (System.Exception ex)
                            {
                                ClientRuntime.MessageService.ShowException(ex);
                                e.Cancel = true;
                                return;
                            }
                        }

                        IDomainComponent data = ui.MainView.Data;
                        if (data != null && data.IsDirty && !ClientRuntime.MessageService.AskQuestion("数据未保存，是否继续退出?".L10N(), "保存提示".L10N()))
                        {
                            e.Cancel = true;
                        }
                    };
                });
            }
            else
            {
                ClientRuntime.Workbench.ShowView(key, delegate (IViewContent v)
                {
                    ControlResult controlResult = CreateUI();
                    BindData(controlResult, entity);
                    OnViewShowing(v, controlResult);
                    return controlResult;
                });
            }
        }

        /// <summary>
        /// 此处与父类方法一致，唯一不同点在于对View的label进行了多语言化
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override string GetEditViewTitle(Entity entity = null)
        {
            if (entity == null || entity.PersistenceStatus == PersistenceStatus.New)
            {
                return "{0}-{1}".FormatArgs(base.Label, base.View.Meta.Label.L10N());//变更处
            }

            string text = string.Empty;
            if (entity.HasId)
            {
                text = entity.GetId().ToString();
            }

            if (base.View.Meta.TitleProperty != null)
            {
                text = entity.GetProperty(base.View.Meta.TitleProperty.PropertyMeta.ManagedProperty).ToString();
            }

            return "{0}-{1}[{2}]".FormatArgs(base.Label, base.View.Meta.Label.L10N(), text);
        }
        /// <summary>
        /// 新实体创建后-提供扩展
        /// </summary>
        /// <param name="entity">新实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            var documentPropertyChanged = RT.Service.Resolve<DocumentPropertyChanged>();
            docSet = (entity as DocumentCollection);
            docSet.PropertyChanged -= documentPropertyChanged.OnDocumentCollectionPropertyChanged;
            docSet.PropertyChanged += documentPropertyChanged.OnDocumentCollectionPropertyChanged;
            base.OnItemCreated(entity);
        }

        /// <summary>
        /// 视图关闭
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        /// <param name="ui">界面结果</param>
        protected override void OnEditViewClosed(object sender, EventArgs e, ControlResult ui)
        {
            base.OnEditViewClosed(sender, e, ui);
            docSet.PropertyChanged -= RT.Service.Resolve<DocumentPropertyChanged>().OnDocumentCollectionPropertyChanged;
        }
    }

    /// <summary>
    /// 文档集编辑命令
    /// </summary>
    [Command(Label = "修改", GroupType = 10, ImageName = "EditEntity", Location = CommandLocation.All, Gestures = "Ctrl+Shift+E")]
    public class EditDocumentCollectionCommand : ListEditCommand
    {
        /// <summary>
        /// 当前修改实体对象
        /// </summary>
        private DocumentCollection docSet;
        /// <summary>
        /// 此处与父类方法一致，唯一不同点在于修改了tab页签的标题，进行了多语言化
        /// </summary>
        /// <param name="entity"></param>
        protected override void ShowView(Entity entity)
        {
            string key = ClientRuntime.Workbench.CreateKey("DetailsView", entity.GetType(), entity);
            if (base.View.Parent != null)
            {
                ControlResult ui = CreateUI();
                if (ui.MainView.CommandsContainer != null)
                {
                    ui.MainView.CommandsContainer.Visibility = Visibility.Collapsed;
                }

                BindData(ui, entity);
                
                int num = ClientRuntime.Workbench.ShowDialog(key, ui.Control, delegate (IDialogContent w)
                {
                    w.Title = GetEditViewTitle(entity);//添加处
                    OnDialogShowing(w, ui);
                });
                int index = base.View.Data.IndexOf(base.View.Data.Find(entity.GetId()));
                if (num == 0)
                {
                    base.View.Data.UpdateItem(index, entity);
                    bool flag = base.View.CanFormSave();
                    if (entity.PersistenceStatus == PersistenceStatus.Unchanged && !flag)
                    {
                        base.View.Data.DeletedList.RemoveAt(index);
                    }

                    if (flag)
                    {
                        RepositoryFactory.Save(entity);
                        base.View.Data.DeletedList.RemoveAt(index);
                    }
                }
            }
            else
            {
                ClientRuntime.Workbench.ShowView(key, delegate (IViewContent v)
                {
                    ControlResult controlResult = CreateUI();
                    BindData(controlResult, entity);
                    OnViewShowing(v, controlResult);
                    return controlResult;
                });
            }
        }

        /// <summary>
        /// 此处与父类方法一致，唯一不同点在于对View的label进行了多语言化
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        protected override string GetEditViewTitle(Entity entity = null)
        {
            if (entity == null || entity.PersistenceStatus == PersistenceStatus.New)
            {
                return "{0}-{1}".FormatArgs(base.Label, base.View.Meta.Label.L10N());//变更处
            }

            string text = string.Empty;
            if (entity.HasId)
            {
                text = entity.GetId().ToString();
            }

            if (base.View.Meta.TitleProperty != null)
            {
                text = entity.GetProperty(base.View.Meta.TitleProperty.PropertyMeta.ManagedProperty).ToString();
            }

            return "{0}-{1}[{2}]".FormatArgs(base.Label, base.View.Meta.Label.L10N(), text);
        }
        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="editEntity">实体</param>
        protected override void OnEditting(Entity editEntity)
        {
            var documentPropertyChanged = RT.Service.Resolve<DocumentPropertyChanged>();
            docSet = (editEntity as DocumentCollection);
            docSet.PropertyChanged -= documentPropertyChanged.OnDocumentCollectionPropertyChanged;
            docSet.PropertyChanged += documentPropertyChanged.OnDocumentCollectionPropertyChanged;
            base.OnEditting(editEntity);
        }

        /// <summary>
        /// 视图关闭
        /// </summary>
        /// <param name="sender">事件对象</param>
        /// <param name="e">事件参数</param>
        /// <param name="ui">界面结果</param>
        protected override void OnEditViewClosed(object sender, EventArgs e, ControlResult ui)
        {
            base.OnEditViewClosed(sender, e, ui);
            docSet.PropertyChanged -= RT.Service.Resolve<DocumentPropertyChanged>().OnDocumentCollectionPropertyChanged;
        }
    }

    /// <summary>
    /// 文档集保存命令
    /// </summary>
    [Command(ImageName = "SaveEntity", Label = "保存", ToolTip = "保存数据", Gestures = "Ctrl+S", GroupType = 10)]
    public class SaveDocumentCollectionCommand : FormSaveCommand
    {
        /// <summary>
        /// 保存前
        /// </summary>
        /// <param name="view">当前明细逻辑视图</param>
        protected override async void OnSaving(DetailLogicalView view)
        {
            var docSet = (view.Current as DocumentCollection);
            if (docSet == null)
            {
                return;
            }

            WaitDialogForm waitForm = new WaitDialogForm("正在保存，请稍后......".L10N(), "提示".L10N());
            try
            {
                waitForm.ShowInTaskbar = false;
                await ExecutAsycAsync(docSet, waitForm);
            }
            finally {
                waitForm.Close();
            }
        }

        private async Task ExecutAsycAsync(DocumentCollection docSet, WaitDialogForm waitForm)
        {
            await Task.Run(() =>
            {
                docSet.Content = docSet.LocalContext.GetPropertyOrDefault<byte[]>(DocumentSelectEditor.FileContent, null);
                foreach (var doc in docSet.DocumentList)
                {
                    doc.Content = doc.LocalContext.GetPropertyOrDefault<byte[]>(DocumentSelectEditor.FileContent, null);
                }

                this.View.Control.Dispatcher.BeginInvoke(() => waitForm.Close());
            });
        }

        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="view">明细视图</param>
        protected override void OnRefresh(DetailLogicalView view)
        {
            View.Data = RF.GetById<DocumentCollection>(view.Current.GetId());
            var documentPropertyChanged = RT.Service.Resolve<DocumentPropertyChanged>();
            View.Data.PropertyChanged -= documentPropertyChanged.OnDocumentCollectionPropertyChanged;
            View.Data.PropertyChanged += documentPropertyChanged.OnDocumentCollectionPropertyChanged;
        }
    }
}