using System;
using System.Collections.Generic;
using System.Linq;

namespace SIE.MES.WIP.Runtime
{
    /// <summary>
    /// 工艺路线
    /// </summary>
    [Serializable]
    public class routing
    {
        /// <summary>
        /// 当前已完成工序ID,用于序列化
        /// </summary>
        private double _currentId;

        /// <summary>
        /// 当前已完成的工序，不序列化
        /// </summary>
        private process _current;

        /// <summary>
        /// 构造函数
        /// </summary>
        public routing()
        {
            Processes = new List<process>();
            Next = new List<double>();
        }

        /// <summary>
        /// 当前已完成工序ID,用于序列化
        /// </summary>
        public double CurrentId
        {
            get
            {
                return _currentId;
            }

            set
            {
                _currentId = value;
                _current = null;
            }
        }

        /// <summary>
        /// 当前已完成的工序，不序列化
        /// </summary>
        [Newtonsoft.Json.JsonIgnore]
        public process Current
        {
            get
            {
                return _current ?? (_current = Processes.FirstOrDefault(p => p.Id == CurrentId));
            }

            set
            {
                _current = value;
                CurrentId = value.Id;
            }
        }

        /// <summary>
        /// 后工序列表
        /// </summary>
        public List<double> Next { get; }

        /// <summary>
        /// 获取后工序
        /// </summary>
        /// <returns>返回后面的工序列表</returns>
        public IEnumerable<process> GetNext()
        {
            return Next.Select(p => Processes.FirstOrDefault(q => p == q.Id));
        }

        /// <summary>
        /// 获取首工序
        /// </summary>
        /// <returns>首工序</returns>
        public process GetBeginProcess()
        {
            return Processes.FirstOrDefault(p => p.IsStart);
        }

        /// <summary>
        /// 工序清单
        /// </summary>
        public List<process> Processes { get; }

        /// <summary>
        /// 最后过站时间
        /// </summary>
        public DateTime LastMoveDateTime { get; set; }
    }
}
