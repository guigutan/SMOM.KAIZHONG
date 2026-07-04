using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.DashBoard.KzBoard.Datas
{
    /// <summary>
    /// 区域数据
    /// </summary>
    [Serializable]
    public class RegionData
    {
        public string label { get; set; }

        public double value { get; set; }
    }
}
