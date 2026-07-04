using Newtonsoft.Json;
using SIE.XPCJ.Models.Enums;
using SIE.XPCJ.Models.WIP;
using SIE.XPCJ.Models.WIP.Entity;
using SIE.XPCJ.Models.WIP.Repairs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.XPCJ.Models
{
    /// <summary>
    /// 采集数据
    /// </summary>
    [Serializable]
    public class CollectData : ICloneable
    {
        /// <summary>
        /// 静态空采集数据
        /// </summary>
        public static CollectData Empty { get; } = new CollectData();

        /// <summary>
        /// 构造函数
        /// </summary>
        public CollectData()
        {
             Defects = new List<DefectData>();
            RepairUseItems = new List<RepairUseItem>();
            RepairDefects = new List<RepairDefect>();
            Result = ResultType.Pass;
            State = WipProductProcessState.Finish;
            Context = new CollectionContext();
            //InspectionItems = new List<InspectionItem>();
            //LoadItems = new List<LoadItem>();
            CollectBarcode = new CollectBarcode();
            //PackingData = new PackingData();
            //ReworkData = new ReworkData();
            CombinedCode = new CombinedCode();
        }
        /// <summary>
        /// 维修缺陷
        /// </summary>
        public List<RepairDefect> RepairDefects { get; set; }
        /// <summary>
        /// 维修用料
        /// </summary>
        public List<RepairUseItem> RepairUseItems { get; set; }

        /// <summary>
        /// 不验证采集步骤
        /// </summary>
        public bool NoValidateStep { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public WipProductProcessState State { get; set; }

        /// <summary>
        /// 采集结果
        /// </summary>
        public ResultType Result { get; set; }

        /// <summary>
        /// 产品等级
        /// </summary>
        public ProductGrade? Grade { get; set; }

        /// <summary>
        /// 不合格数量
        /// </summary>
        //[ConditionItem(displayName: "不合格数量", path: "NgQty")]
        public decimal NgQty { get; set; }

        /// <summary>
        /// 报废数量
        /// </summary>
        //[ConditionItem(displayName:"报废数量",path: "ScrapQty")]
        public decimal ScrapQty { get; set; }

        /// <summary>
        /// 是否工艺复判
        /// </summary>
        //[ConditionItem(displayName: "是否工艺复判", path: "IsRecheck")]
        public bool IsRecheck { get; set; }

        /// <summary>
        /// 报废重用
        /// </summary>
        //[ConditionItem(displayName: "报废重用", path: "CanScrapReuse")]
        public bool CanScrapReuse { get; set; }

        /// <summary>
        /// 缺陷
        /// </summary>
        public List<DefectData> Defects { get; }

        ///// <summary>
        ///// 维修用料
        ///// </summary>
        //public List<RepairUseItem> RepairUseItems { get; }

        ///// <summary>
        ///// 维修缺陷
        ///// </summary>
        //public List<RepairDefect> RepairDefects { get; }

        /// <summary>
        /// 采集数据
        /// </summary>
        public CollectBarcode CollectBarcode { get; set; }

        ///// <summary>
        ///// 转出批次列表
        ///// </summary>
        //public OutputBatch OutputBatch { get; set; }

        ///// <summary>
        ///// 包装采集数据
        ///// </summary>
        //public PackingData PackingData { get; set; }

        ///// <summary>
        ///// 返工采集数据
        ///// </summary>
        //public ReworkData ReworkData { get; set; }

        /// <summary>
        /// 上下文
        /// </summary>
        public CollectionContext Context { get; }

        ///// <summary>
        ///// 检验项目
        ///// </summary>
        //public List<InspectionItem> InspectionItems { get; }

        ///// <summary>
        ///// 上料清单
        ///// </summary>
        //public List<LoadItem> LoadItems { get; set; }

        ///// <summary>
        ///// 图片数据
        ///// </summary>
        //public List<ImageData> ImageDatas { get; set; }

        /// <summary>
        /// 拼板码绑定
        /// </summary>
        public CombinedCode CombinedCode { get; set; }

        private List<SingleLabelInfo> singleLabelInfos;
        /// <summary>
        /// 单体条码信息
        /// </summary>
        public List<SingleLabelInfo> SingleLabelInfos
        {
            get
            {
                if (singleLabelInfos == null)
                {
                    singleLabelInfos = new List<SingleLabelInfo>();
                }
                return singleLabelInfos;
            }
            set
            {
                singleLabelInfos = value;
            }
        }

        /// <summary>
        /// 克隆方法
        /// </summary>
        /// <returns>返回克隆的采集产品模型</returns>
        public CollectData Clone()
        {
            var json = JsonConvert.SerializeObject(this);
            return JsonConvert.DeserializeObject<CollectData>(json);
        }

        /// <summary>
        /// 克隆方法
        /// </summary>
        /// <returns>返回克隆的采集产品模型</returns>
        object ICloneable.Clone()
        {
            return Clone();
        }
    }
}
