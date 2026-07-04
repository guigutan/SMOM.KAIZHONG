namespace SIE.Wpf.Tech.Processs.Commands
{
    ///// <summary>
    ///// 保存工序参数列表
    ///// </summary>
    //[Command(ImageName = "SaveEntity",
    //Label = "保存",
    //ToolTip = "保存数据",
    //Gestures = "Ctrl+S",
    //GroupType = CommandGroupType.Edit)]
    //public class ProcessParameterCommand : ListSaveCommand
    //{
    //    /// <summary>
    //    /// 执行命令
    //    /// </summary>
    //    /// <param name="view">当前列表逻辑视图</param>
    //    public override void Execute(ListLogicalView view)
    //    {
    //        var listView = view.CastTo<ListLogicalView>();
    //        var list = listView.Data.OfType<ProcessParameter>();
    //        if (list.Count() > 1 && list.Any(p => p.Type == ResultTypeForDesign.Any))
    //            throw new ValidationException("参数中存在任意的结果，请删除其它结果".L10N());
    //        base.Execute(view);
    //    }
    //}
}
