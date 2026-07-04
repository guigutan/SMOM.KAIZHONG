using SIE.Common.ImportHelper;
using SIE.Defects.ImportInspection;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Defects.ImportHandle
{
    /// <summary>
    /// 导入 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(DefectsHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class DefectsHandle : IDisposable, IBusinessImport
    {

        #region 私有属性

        /// <summary>
        /// 单据
        /// </summary>
        private Defect InspBill;

        #endregion

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>
        {
            "编码", "描述", "质量类型", "缺陷分类","缺陷等级"
        };

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 缺陷等级
        /// </summary>
        private Dictionary<string, double> _defectGrade = new Dictionary<string, double>();

        /// <summary>
        /// 质量类型
        /// </summary>
        private Dictionary<string, Enum> qualityType;

        /// <summary>
        /// 缺陷分类
        /// </summary>
        private Dictionary<string, double> _defectCategory = new Dictionary<string, double>();

        /// <summary>
        /// 编码
        /// </summary>
        private Dictionary<string, double> codeDic { get; set; }

        /// <summary>
        /// 创建导入对象
        /// </summary>
        /// <returns>返回当前对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("编码", new ValidColumn(ImportDataType._String, true, ValidCode));
            this.ColumnValidList.Add("描述", new ValidColumn(ImportDataType._String, true, true));
            this.ColumnValidList.Add("质量类型", new ValidColumn(ImportDataType._Enum, true, ValidQualityType));
            this.ColumnValidList.Add("缺陷分类", new ValidColumn(ImportDataType._Custom, true, ValidDefectCategory));
            this.ColumnValidList.Add("缺陷等级", new ValidColumn(ImportDataType._Custom, true, ValidDefectGrade));
            return this;
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放数据
        /// </summary>
        protected virtual void Dispose(bool disposing)
        {
            if (InspBill != null)
            {
                InspBill = null;
            }
            if (_defectGrade != null)
            {
                _defectGrade.Clear();
                _defectGrade = null;
            }
            if (qualityType != null)
            {
                qualityType.Clear();
                qualityType = null;
            }
            if (_defectCategory != null)
            {
                _defectCategory.Clear();
                _defectCategory = null;
            }
            if (codeDic != null)
            {
                codeDic.Clear();
                codeDic = null;
            }
        }

        /// <summary>
        /// 处理业务数据
        /// </summary>
        /// <param name="drs">需要处理的数据集</param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var standardDataP = drs.Select(p => new
            {
                Code = p.Field<string>(ColIndex("编码")),
            }).GroupBy(p => new { p.Code }).Select(p => new { Code = p.Key, NameCount = p.Distinct().Count() }).ToList();

            foreach (var item in standardDataP)
            {
                if (item.NameCount > 1)
                {
                    throw new ValidationException("【{0}】不能重复,请检查Excel数据".L10nFormat("编码"));
                }
            }

            var defectData = drs.Select(p => new
            {
                Code = p.Field<string>(ColIndex("编码")),
                Description = p.Field<string>(ColIndex("描述")),
                QualityType = (QualityType)qualityType[p.Field<string>(ColIndex("质量类型"))],
                DefectCategory = p.Field<string>(ColIndex("缺陷分类")),
                DefectGrade = p.Field<string>(ColIndex("缺陷等级")),

            }).GroupBy(p => new { p.Code, p.Description, p.DefectCategory, p.DefectGrade, p.QualityType }).Select(p => new { Code = p.Key.Code, Description = p.Key.Description, QualityType = p.Key.QualityType, DefectCategory = p.Key.DefectCategory, DefectGrade = p.Key.DefectGrade, }).ToList();

            EntityList<Defect> defectList = new EntityList<Defect>();

            foreach (var defect in defectData.ToList())
            {
                Defect newDefect = new Defect()
                {
                    Code = defect.Code,
                    Description = defect.Description,
                    QualityType = defect.QualityType,
                    DefectCategoryId = _defectCategory[defect.DefectCategory],
                    DefectGradeId = _defectGrade[defect.DefectGrade],
                };
                newDefect.GenerateId();
                newDefect.PersistenceStatus = PersistenceStatus.New;
                defectList.Add(newDefect);
            }
            RF.Save(defectList);

        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        #region 基础验证

        /// <summary>
        /// 验证编码
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool ValidCode(object obj, out string messageTip, DataRow dr)
        {
            string context = obj.ToString();
            messageTip = string.Empty;
            if (codeDic == null)
            {
                codeDic = new Dictionary<string, double>();
            }
            if (!codeDic.ContainsKey(context))
            {
                var defect = RT.Service.Resolve<DefectController>().GetDefectByCode(context);
                if (defect != null)
                {
                    codeDic.Add(context, defect.Id);
                    messageTip = "已存在于系统".L10N();
                }
                else
                    codeDic.Add(context, -1);//避免excel中的重复

            }
            else
            {
                messageTip = "重复".L10N();
            }

            return messageTip == string.Empty;
        }


        /// <summary>
        /// 验证缺陷代码分类
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidDefectGrade(object obj, out string messageTip, DataRow dr)
        {
            return BussinessDataValid.ValidDefectGrade(ref _defectGrade, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证质量类型
        /// </summary>
        /// <param name="obj">质量类型枚举</param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool ValidQualityType(object obj, out string messageTip, DataRow dr)
        {
            return BussinessDataValid.ValidQualityType(ref qualityType, obj.ToString(), out messageTip);

        }

        /// <summary>
        /// 验证缺陷代码分类
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        private bool ValidDefectCategory(object obj, out string messageTip, DataRow dr)
        {
            return BussinessDataValid.ValidDefectCategory(ref _defectCategory, obj.ToString(), out messageTip);
        }

        #endregion
    }
}
