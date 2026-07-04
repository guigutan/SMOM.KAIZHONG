using SIE.MES.DashBoard.KzBoard.RegionBoards;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.Datas
{
    [Serializable]
    public class FactoryData
    {
        public string label { get; set; }

        public string value { get; set; }

        public RegionBoardType regionBoardType { get; set; }
    }
}
