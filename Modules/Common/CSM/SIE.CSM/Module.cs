using SIE.CSM;
using SIE.Modules;

[assembly: Module(typeof(Module))]

namespace SIE.CSM
{
    /// <summary>
    /// Domian
    /// </summary>
    internal class Module : DomainModule
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
