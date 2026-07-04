using SIE.Domain;
using SIE.ESop.Documents;
using SIE.MetaModel.View;
using SIE.Wpf.Command;

namespace SIE.Wpf.ESop.Documents.Commands
{
    /// <summary>
    /// 文档添加命令
    /// </summary>
    [Command(ImageName = "AddEntity", Label = "添加", ToolTip = "添加文档", Gestures = "Ctrl+Shift+N", GroupType = 10)]
    public class AddDocumentCommand : ListAddCommand
    {
        /// <summary>
        /// 新实体创建后-提供扩展
        /// </summary>
        /// <param name="entity">新实体</param>
        protected override void OnItemCreated(Entity entity)
        {
            DocumentPropertyChanged documentPropertyChanged = RT.Service.Resolve<DocumentPropertyChanged>();
            (entity as Document).DocumentCollection = View.Parent.Current as DocumentCollection;
            entity.PropertyChanged -= documentPropertyChanged.OnDocumentPropertyChanged;
            entity.PropertyChanged += documentPropertyChanged.OnDocumentPropertyChanged;
            base.OnItemCreated(entity);
        }
    }

    /// <summary>
    /// 文档编辑命令
    /// </summary>
    [Command(Label = "编辑", GroupType = 10, ImageName = "EditEntity", Location = CommandLocation.All, Gestures = "Ctrl+Shift+E")]
    public class EditDocumentCommand : ListEditCommand
    {
        /// <summary>
        /// 执行前
        /// </summary>
        /// <param name="editEntity">实体</param>
        protected override void OnEditting(Entity editEntity)
        {
            DocumentPropertyChanged documentPropertyChanged = RT.Service.Resolve<DocumentPropertyChanged>();
            editEntity.PropertyChanged -= documentPropertyChanged.OnDocumentPropertyChanged;
            editEntity.PropertyChanged += documentPropertyChanged.OnDocumentPropertyChanged;
            base.OnEditting(editEntity);
        }
    }
}