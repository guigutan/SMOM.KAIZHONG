using SIE.MES.InspectionStandards;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.InspectionStandards.Commands
{
    /// <summary>
    /// 编辑机型检验项目命令
    /// </summary>
    [Command(ImageName = "EditEntity", Label = "修改", GroupType = 10)]
    class EditModelInspectionItemCommand : ListEditCommand
    {
        /*/// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            var itemInsStandard = view.Current as ModelInspectionItem;
            if (itemInsStandard != null)
                itemInsStandard.PropertyChanged += InspectionItem_PropertyChanged;

            string key = CRT.Workbench.CreateKey(ViewConfig.ListView, typeof(ModelInspectionItem), null);
            CRT.Workbench.ShowView(key, v =>
            {
                v.Title = Meta.Label.L10N();
                var template = new DetailsUITemplate(typeof(ModelInspectionItem), ModelInspectionItemViewConfig.EditConfig);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;

                detailView.Data = itemInsStandard;
                ////退出时，数据已被修改且未保存时，提示用户
                v.Closing += (o, e) =>
                {
                    if (ui.MainView.Data.IsDirty)
                    {
                        if (CRT.MessageService.AskQuestion("数据未保存，确定退出吗".L10N()) == true)
                        {
                            e.Cancel = false;
                        }
                        else
                        {
                            e.Cancel = true;
                        }
                    }
                };

                return ui;
            });
        }

        /// <summary>
        /// 属性变更事件
        /// </summary>
        /// <param name="sender">发送者</param>
        /// <param name="e">事件参数</param>
        private void InspectionItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            var editEntity = sender as ModelInspectionItem;
        }
        */

        protected override ControlResult CreateUI()
        {
            ////return base.CreateUI();
            var editTemplate = new DetailsUITemplate<ModelInspectionItem>(View.ModuleKey);
            editTemplate.ViewGroup = ModelInspectionItemViewConfig.EditConfig;
            return editTemplate.CreateUI();
        }
    }
}
