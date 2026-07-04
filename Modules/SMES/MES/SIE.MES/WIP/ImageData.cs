using System;

namespace SIE.MES.WIP
{
    /// <summary>
    /// 图片数据
    /// </summary>
    [Serializable]
    public class ImageData
    {
        /// <summary>
        /// 图片路径
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 缺陷ID
        /// </summary>
        public double? DefectId { get; set; }

        /// <summary>
        /// 点位
        /// </summary>
        public string Point { get; set; }
    }
}