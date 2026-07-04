using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.TaskManagement.Dispatchs.Datas
{
    /// <summary>
    /// 称重工序BOM明细
    /// </summary>
    [Serializable]
    public class CsWeighingProcessBomInfo: INotifyPropertyChanged
    {
        /// <summary>
        /// 工序BomId
        /// </summary>
        public double ProcessBomId { get; set; }

        /// <summary>
        /// 产品名称
        /// </summary>
        public string ItemName { get; set; }

        /// <summary>
        /// 产品编码
        /// </summary>
        public string ItemCode { get; set; }

        /// <summary>
        /// 需求量
        /// </summary>
        public decimal? RequireQty { get; set; }

        /// <summary>
        /// 单位耗用量
        /// </summary>
        public decimal SingleQty { get; set; }

        /// <summary>
        /// 单位
        /// </summary>
        public string Unit { get; set; }

        /// <summary>
        /// 工序
        /// </summary>
        public string Process { get; set; }

        public decimal? _newWeight { get; set; }

        public decimal? _oldWeight { get; set; }

        /// <summary>
        /// 取样净重
        /// </summary>
        public decimal? Weight {
            get { return _newWeight; }//=> _newWeight;
            set
            {
                _newWeight = value;
                if (_newWeight != _oldWeight)
                {
                    IsDirty = true; // 修改时标记为已变更
                }
                else
                {
                    IsDirty = false;
                }
            }
        }

        /// <summary>
        /// 取样净重范围值
        /// </summary>
        public string WeightScope { get; set; }

        /// <summary>
        /// 脏数据
        /// </summary>
        public bool? IsDirty { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
