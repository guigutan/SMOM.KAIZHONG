using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SIE.CrossPlatform.Collect.Models
{
    [Serializable]
    public class DefectItem
    {
        /// <summary>
        /// 缺陷
        /// </summary>
        public Defect Defect
        {
            get;
            set;
        }

        /// <summary>
        /// 缺陷位置
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// 不良数量,不允许输入数量时,返回1
        /// </summary>
        public double Qty
        {
            get;
            set;
        }

        /// <summary>
        /// 比较缺陷分类ID是否一致
        /// </summary>
        /// <param name="obj">需要比较的对象</param>
        /// <returns>返回是否与传入的对象的缺陷分类ID一致</returns>
        public override bool Equals(object obj)
        {
            var item = obj as DefectItem;
            if (item == null) return false;
            return Defect?.Id == item.Defect?.Id;
        }

        /// <summary>
        /// 获取缺陷分类的哈希编码
        /// </summary>
        /// <returns>返回缺陷分类的哈希编码</returns>
        public override int GetHashCode()
        {
            if (Defect == null) return 0;
            return Defect.GetHashCode();
        }
    }
    [Serializable]
    public class Defect
    {
        /// <summary>
        /// Id
        /// </summary>
        public double Id
        {
            get;
            set;
        }

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get;
            set;
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get;
            set;
        }

        /// <summary>
        /// 质量类型
        /// </summary>
        public QualityType QualityType
        {
            get;
            set;
        }

        /// <summary>
        /// 缺陷分类Id
        /// </summary>
        public double DefectCategoryId
        {
            get;
            set;
        }


        /// <summary>
        /// 缺陷分类
        /// </summary>
        public DefectCategory DefectCategory
        {
            get;
            set;
        }

        /// <summary>
        /// 缺陷等级Id
        /// </summary>
        public double DefectGradeId
        {
            get;
            set;
        }
        /// <summary>
        /// 缺陷等级
        /// </summary>
        public DefectGrade DefectGrade
        {
            get;
            set;
        }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        public string DefectLevel
        {
            get;
            set;
        }

        /// <summary>
        /// 严重度
        /// </summary>
        public DefectSeverity DefectSeverity
        {
            get;
            set;
        }

        /// <summary>
        /// 分类编码
        /// </summary>
        public string CategoryCode
        {
            get;
            set;
        }
        /// <summary>
        /// 描述
        /// </summary>
        public string CategoryDescription
        {
            get;
            set;
        }
    }
}
