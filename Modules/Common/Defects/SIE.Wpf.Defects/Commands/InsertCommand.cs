using SIE.Wpf.Command;

namespace SIE.Wpf.Defects.Commands
{
    /// <summary>
    /// 插入命令
    /// </summary>
    [Command(ImageName = "ExitToApp", Label = "插入", Location = MetaModel.View.CommandLocation.All, GroupType = 10)]
    public class InsertCommand : TreeInsertCommand
    {
    }
}
