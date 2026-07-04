using SIE.MES.InspectionStandards;
using SIE.Wpf.Command;

namespace SIE.Wpf.MES.InspectionStandards.Commands
{
    /// <summary>
    /// 添加机型检验项目命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", GroupType = 10)]
    public class AddModelInspectionItemCommand : ListAddCommand
    {
        /*/// <summary>
        /// 执行
        /// </summary>
        /// <param name="view">列表逻辑视图</param>
        public override void Execute(ListLogicalView view)
        {
            CRT.Workbench.ShowView(Guid.NewGuid().ToString(), v =>
            {
                v.Title = Meta.Label.L10N();
                var template = new DetailsUITemplate(typeof(ModelInspectionItem), ModelInspectionItemViewConfig.AddConfig);
                var ui = template.CreateUI();
                var detailView = ui.MainView as DetailLogicalView;
                var model = new ModelInspectionItem();
                detailView.Data = model;
                ////退出时，数据已被添加且未保存时，提示用户
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
                ////添加之后刷新界面
                if (view.DataLoader.AnyLoaded == true)
                {
                    view.DataLoader.ReloadDataAsync();
                }
                else
                {
                    view.DataLoader.LoadDataAsync();
                }

                return ui;
            });
        }*/

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <returns>返回UI控件</returns>
        protected override ControlResult CreateUI()
        {
            ////return base.CreateUI();
            var addTemplate = new DetailsUITemplate<ModelInspectionItem>(View.ModuleKey);
            addTemplate.ViewGroup = ModelInspectionItemViewConfig.AddConfig;
            return addTemplate.CreateUI();
        }
    }
}