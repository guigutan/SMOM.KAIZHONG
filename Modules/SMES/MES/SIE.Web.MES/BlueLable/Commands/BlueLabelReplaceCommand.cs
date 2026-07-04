using DocumentFormat.OpenXml.Drawing;
using SIE.Common.ImportHelper;
using SIE.MES.BlueLabel.Handles;
using SIE.MES.BlueLable;
using SIE.Web.Command;
using SIE.Web.Common.Import.Commands;
using System;
using System.Data;

namespace SIE.Web.MES.BlueLabel.Commands
{
    /// <summary>
    /// 蓝标替换
    /// </summary>
    public class BlueLabelReplaceCommand : ViewCommand
    {
        protected override object Excute(ViewArgs args, string scope)
        {
            //RT.Service.Resolve<BlueLabelReplaceController>().BlueLabelReplace(abnormal);
            throw new NotImplementedException();
        }
    }
}
