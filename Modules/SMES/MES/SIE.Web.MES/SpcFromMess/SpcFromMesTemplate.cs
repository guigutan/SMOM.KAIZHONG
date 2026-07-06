using SIE.MetaModel.View;

namespace SIE.Web.MES.SpcFromMess
{
    /// <summary>
    /// SPC统计过程控制-模板（指定自定义Layout）
    /// </summary>
    public class SpcFromMesTemplate : CodeBlocksTemplate
    {
        protected override AggtBlocks DefineBlocks()
        {
            var rst = base.DefineBlocks();
            rst.Layout = new LayoutMeta("SIE.Web.MES.SpcFromMess.SpcFromMesReadonlyLayout");
            return rst;
        }
    }
}
