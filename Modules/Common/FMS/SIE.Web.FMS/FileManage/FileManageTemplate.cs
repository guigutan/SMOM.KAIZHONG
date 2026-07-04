using SIE.MetaModel.View;

namespace SIE.Web.FMS
{
    /// <summary>
    /// 文件管理模板
    /// </summary>
    public class FileManageTemplate : CodeBlocksTemplate
    {
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.FMS.FileManageLayout");
            return rst;
        }
    }
}
