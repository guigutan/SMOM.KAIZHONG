using System;

namespace SIE.XPCJ.Models.WIP.Repairs
{
    [Serializable]
    public class GotoProcessViewModel
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public string Id
        {
            get; set;
        }


        /// <summary>
        /// 工序ID
        /// </summary>
        public double RoutingProcessId
        {
            get;set;
        }

        /// <summary>
        /// 工艺路线路径名称
        /// </summary>
        public string PathName
        {
            get; set;
        }
       

        /// <summary>
        /// 工艺路线路径描述
        /// </summary>
        public string PathDescription
        {
            get; set;
        }
        /// <summary>
        /// 默认
        /// </summary>
        public bool IsDefault
        {
            get; set;
        }

    }
}
