using SIE.MES.WIP;
using SIE.Tech.Processs;
using System;

namespace SIE.xUnit.MES.WIP
{
    [Serializable]
    public class WipContext
    {
        public WipContext()
        {
            CollectData = new CollectData();
        }
        public string Barcode { get; set; }

        public ProcessType ProcessType { get; set; }
        public bool IsBuckleMaterial { get; set; }
        public CollectData CollectData { get; set; } 
        public Workcell Workcell { get; set; }

        public WipController Controller { get; set; } = RT.Service.Resolve<WipController>(); 
    }
}