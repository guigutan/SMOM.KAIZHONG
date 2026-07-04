using SIE.Modules;
using SIE.ShipPlan;

[assembly: Module(typeof(Module))]

namespace SIE.ShipPlan
{
    /// <summary>
    /// Domian
    /// </summary>
    public class Module : DomainModule
    {
        /// <summary>
        /// Domain
        /// </summary>
        /// <param name="app">app</param>
        public override void Initialize(IApp app)
        {
        }
    }
}