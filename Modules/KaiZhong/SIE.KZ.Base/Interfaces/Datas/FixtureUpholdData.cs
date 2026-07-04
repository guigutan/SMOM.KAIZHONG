using Org.BouncyCastle.Bcpg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.KZ.Base.Interfaces.Datas
{
    /// <summary>
    /// 工装数据
    /// </summary>
    [Serializable]
    public class FixtureUpholdData
    {
        /// <summary>
        /// 工厂
        /// </summary>
        public string WERKS { get; set; }

        /// <summary>
        /// 工装唯一码
        /// </summary>
        public string FixtureCode { get; set; }

        /// <summary>
        /// 工装物料描述
        /// </summary>
        public string FixtureName { get; set; }

        /// <summary>
        /// 工装状态
        /// </summary>
        public string FixtureState { get; set; }

        /// <summary>
        /// 工装类型
        /// </summary>
        public string FixtureType { get; set; }

        /// <summary>
        /// 工厂编码
        /// </summary>
        public string FactoryCode { get; set; }

        /// <summary>
        /// 图号
        /// </summary>
        public string Drawn { get; set; }
    }
}
