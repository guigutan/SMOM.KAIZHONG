using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Fixtures.Models.ApiModels
{
    /// <summary>
    /// 工治具编码基础数据
    /// </summary>
    [Serializable]
    public class FixtureEncodeBaseInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 管控方式
        /// </summary>
        public ManageMode ManageMode { get; set; }
    }
}
