using SIE.Defects;
using System;

namespace SIE.Wpf.Defects.Commands
{
    /// <summary>
    /// 缺陷责任分类维护命令
    /// </summary>
    [Command(ImageName = "Repair", Label = "分类维护", ToolTip = "缺陷责任分类维护", GroupType = 50)]
    public class DefectRespCategoryCommand : ServiceDataCommand
    {
        /// <summary>
        /// 维护数据的实体类型
        /// </summary>
        protected override Type EntityType => typeof(DefectResponsibilityCategory);

        /// <summary>
        /// 标题
        /// </summary>
        protected override string Title => "缺陷责任分类维护";
    }
}