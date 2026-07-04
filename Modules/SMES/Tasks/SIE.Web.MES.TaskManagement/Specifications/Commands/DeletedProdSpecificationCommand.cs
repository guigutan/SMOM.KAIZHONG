using SIE.MES.TaskManagement.Specifications;
using SIE.Web.Command;
using System.Linq;

namespace SIE.Web.MES.TaskManagement.Specifications.Commands
{
    [JsCommand("SIE.Web.MES.TaskManagement.Specifications.Commands.DeletedProdSpecificationCommand")]
    public class DeletedProdSpecificationCommand : ViewCommand<double[]>
    {
        protected override object Excute(double[] args, string scope)
        {
            RT.Service.Resolve<ProductSpecificationController>().DeletedProdSpecifications(args.ToList());
            return "删除成功";
        }
    }
}
