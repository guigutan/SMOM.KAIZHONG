using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Core.Common.Controllers;
using SIE.Core.Enums;
using SIE.Core.Equipments;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Equipments.EquipModels.ImportEquipModel
{
    /// <summary>
    /// 设备型号-导入逻辑处理
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipModelHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipModelHandle : IBusinessImport
    {
        private const string EMPTY_FORMAT = "{0}为空";
        #region 基础验证

        #region 暂存数据集
        /// <summary>
        /// 行业属性
        /// </summary>
        private Dictionary<string, Enum> dicIndustryCategory { get; set; } = new Dictionary<string, Enum>();

        /// <summary>
        /// 设备类型编码集
        /// </summary>
        private Dictionary<string, double> dicEquipTypeCode { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 型号编码集
        /// </summary>
        private Dictionary<string, string> dicEquipModelCode { get; set; } = null;

        /// <summary>
        /// 类别编码集
        /// </summary>
        private Dictionary<string, string> dicEquipCategory { get; set; }

        #endregion

        #region 枚举数据验证
        /// <summary>
        /// 验证行业属性
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidIndustryCategory(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(EMPTY_FORMAT.L10nFormat(nameof(obj)));
            }

            return ValidEnum<IndustryCategory>(dicIndustryCategory, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证枚举是否有效
        /// </summary>
        /// <typeparam name="T">实体</typeparam>
        /// <param name="checkTagRange">枚举字典</param>
        /// <param name="context">内容上下文</param>
        /// <param name="messageTip">提示文</param>
        /// <returns>true/false</returns>
        public static bool ValidEnum<T>(Dictionary<string, Enum> checkTagRange, string context, out string messageTip)
        {
            if (!checkTagRange.Any())
            {

                var res = ImportExtension.GetEnumLabel(typeof(T), string.Empty);
                if (res.Any())
                {
                    res.ForEach(item =>
                    {
                        checkTagRange.Add(item.Key, item.Value);
                    });
                }

            }

            bool isValid = true;
            messageTip = string.Empty;

            if (checkTagRange.Count == 0)
            {
                throw new ValidationException("验证枚举是否有效时，{0}为空".L10nFormat(nameof(checkTagRange)));
            }

            if (!checkTagRange.ContainsKey(context))
            {
                messageTip = "只能选择".L10N() + string.Join("、", checkTagRange.Keys);
                isValid = false;
            }

            return isValid;
        }

        #endregion

        #region 设备类别验证
        /// <summary>
        /// 设备类别验证
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="messageTip">提示语</param>
        /// <param name="dr">行</param>
        /// <returns>true/false</returns>
        public bool ValidEquipCategory(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;

            if (obj == null || obj.ToString() == string.Empty)
            {
                messageTip = "不能为空".L10N();
                return false;
            }
            string content = obj.ToString();

            if (dicEquipCategory == null)
            {
                var catalogList = RT.Service.Resolve<CatalogController>().GetCatalogList(SIE.Equipments.EquipTypes.EquipType.EquipTypeCatalogType);
                if (catalogList != null)
                    dicEquipCategory = catalogList.ToDictionary(p => p.Code, p => p.Name);
                else
                    dicEquipCategory = new Dictionary<string, string>();
            }

            if (!dicEquipCategory.ContainsKey(content))
            {
                messageTip = "不存在于系统".L10N();
                isValid = false;
            }

            return isValid;
        }
        #endregion

        #region 设备类型编码验证
        /// <summary>
        /// 验证设备类型编码是否有效
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="messageTip">提示语</param>
        /// <param name="dr">行</param>
        /// <returns>true/false</returns>
        public bool ValidEquipTypeCode(object obj, out string messageTip, DataRow dr)
        {
            bool isValid = true;
            messageTip = string.Empty;

            if (obj == null || obj.ToString() == string.Empty)
                return true;//为空不用验证

            var context = obj.ToString();

            if (dicEquipTypeCode == null)
                dicEquipTypeCode = new Dictionary<string, double>();

            if (!dicEquipTypeCode.ContainsKey(context))
            {
                var equipType = RT.Service.Resolve<CoreEquipController>().GetEquipTypeByCode(context);
                if (equipType != null)
                    dicEquipTypeCode.Add(context, equipType.Id);
                else
                {
                    messageTip = "不存在于系统".L10N();
                    isValid = false;
                }
            }

            return isValid;
        }
        #endregion

        #region 型号编码验证
        /// <summary>
        /// 型号编码验证
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="messageTip">提示语</param>
        /// <param name="dr">行</param>
        /// <returns>true/false</returns>
        public bool ValidEquipModelCode(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            if (obj == null || obj.ToString() == string.Empty)
            {
                messageTip = "不能为空".L10N();
                return false;
            }
            string content = obj.ToString();

            if (dicEquipModelCode == null)
                dicEquipModelCode = RT.Service.Resolve<EquipModelController>().GetAllCode().ToDictionary(p => p);

            if (dicEquipModelCode.ContainsKey(content))
            {
                messageTip = "已经存在".L10nFormat(obj.ToString());
                return false;
            }

            dicEquipModelCode.Add(content, content);

            return true;
        }
        #endregion

        #endregion
        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>{
            "型号编码*",
            "型号名称*",
            "设备类型编码",
            "技术规格",
            "设备类别编码*",
            "生产厂商",
            "行业属性*"
        };

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return ColumnNameList.IndexOf(columnName);
        }

        /// <summary>
        /// 列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> ColumnValidList { get; set; }

        /// <summary>
        /// 创建导入对象
        /// </summary>
        /// <returns>导入对象</returns>
        public IBusinessImport CreaetColumnValid()
        {
            this.ColumnValidList = new Dictionary<string, ValidColumn>();
            this.ColumnValidList.Add("型号编码*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("型号名称*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("设备类型编码", new ValidColumn(ImportDataType._String, false, ValidEquipTypeCode));
            this.ColumnValidList.Add("技术规格", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("设备类别编码*", new ValidColumn(ImportDataType._String, true, ValidEquipCategory));
            this.ColumnValidList.Add("生产厂商", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("行业属性*", new ValidColumn(ImportDataType._Enum, true, ValidIndustryCategory));

            return this;
        }

        /// <summary>
        /// 导入数据处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            var ctr = RT.Service.Resolve<EquipModelController>();
            var codeList = drs.ToList().Select(c => c.Field<string>(ColIndex("型号编码*"))).ToList();
            var modelCodes = ctr.GetEquipModelCodes(codeList);
            EntityList<EquipModel> equipModelList = new EntityList<EquipModel>();

            foreach (var dr in drs)
            {
                var code = dr.Field<string>(ColIndex("型号编码*"));
                var name = dr.Field<string>(ColIndex("型号名称*"));
                var equipTypeCode = dr.Field<string>(ColIndex("设备类型编码"));
                double? equipTypeId = equipTypeCode.IsNotEmpty() ? dicEquipTypeCode[equipTypeCode] : null;
                var specifications = dr.Field<string>(ColIndex("技术规格"));
                var equipCategoryCode = dr.Field<string>(ColIndex("设备类别编码*"));
                var manufacturers = dr.Field<string>(ColIndex("生产厂商"));
                var industryCategory = dr.Field<string>(ColIndex("行业属性*"));

                // 判断主数据是否存在
                if (modelCodes.Contains(code))
                {
                    dr[ImportDataHandle.MessageColumnName] = "已存在型号编码[{0}]".L10nFormat(code);
                    continue;
                }
                else
                {
                    var equipModel = new EquipModel();
                    equipModel.Code = code;
                    equipModel.Name = name;
                    equipModel.EquipTypeId = equipTypeId;
                    equipModel.Specifications = specifications;
                    if (dicEquipCategory.ContainsKey(equipCategoryCode))//再验证一次 
                    {
                        equipModel.TypeCategory = equipCategoryCode;
                    }
                    equipModel.Manufacturers = manufacturers;
                    equipModel.IndustryCategory = (IndustryCategory)dicIndustryCategory[industryCategory];
                    equipModelList.Add(equipModel);
                    modelCodes.Add(code);
                }
                    
            }

            // 如果不能新增记录错误信息
            if (equipModelList.Any())
            {
                using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
                {
                    RT.Service.Resolve<CommonController>().BatchInsertSave(equipModelList);
                    tran.Complete();
                }
            }

        }
    }
}
