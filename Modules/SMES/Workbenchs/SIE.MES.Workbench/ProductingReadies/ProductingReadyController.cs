using SIE.Domain;
using System.Linq;

namespace SIE.MES.Workbench.ProductingReadies
{
    public class ProductingReadyController : DomainController
    {
        public virtual EntityList<ProductingReady> GetProductingReadies()
        {
            return Query<ProductingReady>().ToList();
        }
    }
}
