using SIE.Common.ImportHelper;
using SIE.Core.Enums;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.EMS.Equipments.Models.ImportEquipModel
{
    /// <summary>
    /// 设备型号-导入逻辑处理
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipModelHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipModelHandle : IBusinessImport
    {
        private const string PARAMETER_IS_EMPTY = "{0}为空";

        #region 基础验证

        #region 暂存数据集
        /// <summary>
        /// 行业属性
        /// </summary>
        private Dictionary<string, Enum> dicIndustryCategory;

        /// <summary>
        /// 轨道类型
        /// </summary>
        private Dictionary<string, Enum> dicRailType;

        /// <summary>
        /// 老化方式
        /// </summary>
        private Dictionary<string, Enum> dicAgingType;

        /// <summary>
        /// 产品生产模式
        /// </summary>
        private Dictionary<string, Enum> dicProductionType;

        /// <summary>
        /// 是否
        /// </summary>
        private Dictionary<string, Enum> dicYesNo;

        /// <summary>
        /// 启用/禁用
        /// </summary>
        private Dictionary<string, Enum> dicState;


        /// <summary>
        ///站位类型
        /// </summary>
        private Dictionary<string, Enum> dicStanceType;

        /// <summary>
        /// 设备类型编码集
        /// </summary>
        private Dictionary<double, string> dicEquipTypeCode;

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
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            return ValidEnum<OrbitType>(dicIndustryCategory, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证轨道类型
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidRailType(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            return ValidEnum<OrbitType>(dicRailType, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证老化类型
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidAgingType(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            return ValidEnum<AgingMode>(dicAgingType, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证产品生产模式
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidProductionType(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            return ValidEnum<ProductionMode>(dicProductionType, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证是否
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidYesNO(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            return ValidEnum<YesNo>(dicYesNo, obj.ToString(), out messageTip);
        }


        /// <summary>
        /// 验证状态
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidState(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            return ValidEnum<State>(dicState, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证站位类型
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidStanceType(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            return ValidEnum<StanceType>(dicStanceType, obj.ToString(), out messageTip);
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
            if (checkTagRange == null)
            {
                checkTagRange = ImportExtension.GetEnumLabel(typeof(T), string.Empty);
            }

            bool isValid = true;
            messageTip = string.Empty;

            if (checkTagRange == null)
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
            
            if (obj == null)
            {
                throw new ValidationException(PARAMETER_IS_EMPTY.L10nFormat(nameof(obj)));
            }

            var context = obj.ToString();

            if (dicEquipTypeCode == null)
            {
                dicEquipTypeCode = RT.Service.Resolve<CoreEquipController>().GetEquipTypeList(null).ToDictionary(c => c.Id, c => c.TypeCode);
            }

            if (!dicEquipTypeCode.ContainsValue(context))
            {
                messageTip = "不存在于系统".L10N();
                isValid = false;
            }

            return isValid;
        }

        /// <summary>
        /// 获取行业属性
        /// </summary>
        /// <param name="industryCategory">行业属性</param>
        /// <returns></returns>
        public IndustryCategory GetIndustryCategory(string industryCategory)
        {
            if (!dicIndustryCategory.ContainsKey(industryCategory))
            {
                throw new ValidationException("【{0}】不是正确的{1}。"
                    .L10nFormat(industryCategory, nameof(industryCategory)));
            }

            return (IndustryCategory)dicIndustryCategory[industryCategory];
        }
        #endregion

        #endregion

        private List<string> columnNameList = new List<string>{
            "设备型号编码*",
            "设备型号名称*",
            "技术规格",
            "生产厂商",
            "设备类型编码*",
            "行业属性*",
            "轨道类型",
            "虚拟设备",
            "是否Feeder绑定",
            "启用站位防错",
            "启用Feeder防错",
            "禁用",
            "老化方式",
            "产品生产模式",
            "分区",
            "大站位",
            "站位",
            "站位类型"
        };

        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList
        {
            get { return columnNameList; }
            set { columnNameList = value; }
        }

        /// <summary>
        /// 获取指定列名的索引
        /// </summary>
        /// <param name="columnName">指定列名</param>
        /// <returns>返回对应索引</returns>
        private int ColIndex(string columnName)
        {
            return columnNameList.IndexOf(columnName);
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
            this.ColumnValidList.Add("设备型号编码*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("设备型号名称*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("技术规格", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("生产厂商", new ValidColumn(ImportDataType._String, false, true));
            this.ColumnValidList.Add("设备类型编码*", new ValidColumn(ImportDataType._String, true, ValidEquipTypeCode));
            this.ColumnValidList.Add("行业属性*", new ValidColumn(ImportDataType._Enum, true, ValidIndustryCategory));

            this.ColumnValidList.Add("轨道类型", new ValidColumn(ImportDataType._Enum, false, ValidRailType));
            this.ColumnValidList.Add("虚拟设备", new ValidColumn(ImportDataType._Enum, false, ValidYesNO));
            this.ColumnValidList.Add("是否Feeder绑定", new ValidColumn(ImportDataType._Enum, false, ValidYesNO));
            this.ColumnValidList.Add("启用站位防错", new ValidColumn(ImportDataType._Enum, false, ValidState));
            this.ColumnValidList.Add("启用Feeder防错", new ValidColumn(ImportDataType._Enum, false, ValidState));
            this.ColumnValidList.Add("禁用", new ValidColumn(ImportDataType._Enum, false, ValidYesNO));
            this.ColumnValidList.Add("老化方式", new ValidColumn(ImportDataType._Enum, false, ValidAgingType));
            this.ColumnValidList.Add("产品生产模式", new ValidColumn(ImportDataType._Enum, false, ValidProductionType));

            this.ColumnValidList.Add("分区", new ValidColumn(ImportDataType._String, false, false));
            this.ColumnValidList.Add("大站位", new ValidColumn(ImportDataType._String, false, false));
            this.ColumnValidList.Add("站位", new ValidColumn(ImportDataType._String, false, false));
            this.ColumnValidList.Add("站位类型", new ValidColumn(ImportDataType._String, false, false));

            return this;
        }

        /// <summary>
        /// 导入数据处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            // 1、按检验类型、产品编码、客户、检验标准名称、检验标准版本、备注分组
            var List = from g in drs
                       group g by new
                       {
                           Code = g.Field<string>(ColIndex("设备型号编码*")),
                           Name = g.Field<string>(ColIndex("设备型号名称*")),
                           Specifications = g.Field<string>(ColIndex("技术规格")),
                           Manufacturers = g.Field<string>(ColIndex("生产厂商")),
                           EquipTypeCode = g.Field<string>(ColIndex("设备类型编码*")),
                           IndustryCategory = g.Field<string>(ColIndex("行业属性*")),

                           RailType = g.Field<string>(ColIndex("轨道类型")),
                           VirtualDevice = g.Field<string>(ColIndex("虚拟设备")),
                           FeederBinding = g.Field<string>(ColIndex("是否Feeder绑定")),
                           FeederLocFailSafe = g.Field<string>(ColIndex("启用站位防错")),
                           FeederBarcodeFailSafe = g.Field<string>(ColIndex("启用Feeder防错")),
                           IsDisabled = g.Field<string>(ColIndex("禁用")),
                           AgingType = g.Field<string>(ColIndex("老化方式")),
                           ProductionType = g.Field<string>(ColIndex("产品生产模式")),
                       } into proInspStandard
                       select new
                       {
                           proInspStandard.Key.Code,
                           proInspStandard.Key.Name,
                           proInspStandard.Key.Specifications,
                           proInspStandard.Key.Manufacturers,
                           proInspStandard.Key.EquipTypeCode,
                           proInspStandard.Key.IndustryCategory,
                           DetailInfo = proInspStandard,

                           proInspStandard.Key.RailType,
                           proInspStandard.Key.FeederBinding,
                           proInspStandard.Key.FeederLocFailSafe,
                           proInspStandard.Key.FeederBarcodeFailSafe,
                           proInspStandard.Key.VirtualDevice,
                           proInspStandard.Key.IsDisabled,

                           proInspStandard.Key.AgingType,
                           proInspStandard.Key.ProductionType,
                       };
            var ctr = RT.Service.Resolve<ElecEquipController>();

            ///// 循环检验每一行主数据
            foreach (var mainDataItem in List)
            {
                // 判断主数据是否存在
                var itemEquipModel = ctr.GetEquipModel(mainDataItem.Code, mainDataItem.EquipTypeCode);
                if (itemEquipModel == null)
                {
                    itemEquipModel = new EquipModel();
                    itemEquipModel.GenerateId();
                    itemEquipModel.Code = mainDataItem.Code;
                    itemEquipModel.Name = mainDataItem.Name;
                    itemEquipModel.Specifications = mainDataItem.Specifications;
                    itemEquipModel.Manufacturers = mainDataItem.Manufacturers;
                    itemEquipModel.EquipTypeId = dicEquipTypeCode.FirstOrDefault(c => c.Value == mainDataItem.EquipTypeCode).Key;
                    itemEquipModel.IndustryCategory = GetIndustryCategory(mainDataItem.IndustryCategory);

                    if (dicRailType != null && dicRailType.ContainsKey(mainDataItem.RailType))
                    {
                        itemEquipModel.RailType = (OrbitType)dicRailType[mainDataItem.RailType];
                    }
                    itemEquipModel.FeederBinding = (YesNo)dicYesNo[mainDataItem.FeederBinding];
                    itemEquipModel.FeederLocFailSafe = (State)dicState[mainDataItem.FeederLocFailSafe];
                    itemEquipModel.FeederBarcodeFailSafe = (State)dicState[mainDataItem.FeederBarcodeFailSafe];
                    itemEquipModel.VirtualDevice = (YesNo)dicYesNo[mainDataItem.VirtualDevice];
                    itemEquipModel.IsDisabled = (YesNo)dicYesNo[mainDataItem.IsDisabled];

                    if (dicAgingType != null && dicAgingType.ContainsKey(mainDataItem.AgingType))
                    {
                        itemEquipModel.AgingType = (AgingMode?)dicAgingType[mainDataItem.AgingType];
                    }

                    if (dicProductionType != null && dicProductionType.ContainsKey(mainDataItem.ProductionType))
                    {
                        itemEquipModel.ProductionType = (ProductionMode)dicProductionType[mainDataItem.ProductionType];
                    }
                }

                // 如果不能新增记录错误信息
                try
                {
                    using (var tran = DB.TransactionScope(EmsEntityDataProvider.ConnectionStringName))
                    {
                        var EquipModelLocationList = ProcessDetailData(itemEquipModel, mainDataItem.DetailInfo.ToList());
                        RF.Save(itemEquipModel);
                        RF.Save(EquipModelLocationList);
                        tran.Complete();
                    }
                }
                catch (Exception ex)
                {
                    string strMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                    ImportExtension.BatchAppendText(mainDataItem.DetailInfo.ToList(), ImportDataHandle.MessageColumnName, strMsg);
                }
            }
        }

        /// <summary>
        /// 处理产品检验项目明细的数据
        /// </summary>
        /// <param name="mainData">主表数据对象</param>
        /// <param name="detailRows">明细数据集合</param>
        private EntityList<EquipModelLocation> ProcessDetailData(EquipModel mainData, List<DataRow> detailRows)
        {
            var EquipModelLocationList = new EntityList<EquipModelLocation>();
            // 循环检验每一行子数据
            foreach (DataRow detailRow in detailRows)
            {
                string Subarea = detailRow[ColIndex("分区")].ToString();
                string BigStance = detailRow[ColIndex("大站位")].ToString();
                string Stance = detailRow[ColIndex("站位")].ToString();
                string stanceType = detailRow[ColIndex("站位类型")].ToString();
                if ((!Subarea.IsNullOrWhiteSpace() || !BigStance.IsNullOrWhiteSpace() || !Stance.IsNullOrWhiteSpace()) && stanceType.IsNullOrWhiteSpace())
                {
                    throw new ValidationException("填写了分区/大站位/站位时一定要填写站位类型".L10N());
                }

                var EquipModelLocation = new EquipModelLocation();
                EquipModelLocation.GenerateId();
                EquipModelLocation.EquipModel = mainData;
                EquipModelLocation.BigStance = BigStance;
                EquipModelLocation.Stance = Stance;
                EquipModelLocation.StanceType = GetStanceType(stanceType);
                EquipModelLocation.Subarea = Subarea;
                EquipModelLocationList.Add(EquipModelLocation);
            }
            return EquipModelLocationList;
        }

        /// <summary>
        /// 获取站位类型枚举值
        /// </summary>
        /// <param name="content">内容上下文</param>
        /// <returns>站位类型</returns>
        private StanceType GetStanceType(string content)
        {
            StanceType stanceType;
            if (StanceType.Banding.ToLabel() == content)
            {
                stanceType = StanceType.Banding;
            }
            else if (StanceType.HandStick.ToLabel() == content)
            {
                stanceType = StanceType.HandStick;
            }
            else if (StanceType.Tray.ToLabel() == content)
            {
                stanceType = StanceType.Tray;
            }
            else
            {
                stanceType = StanceType.Tube;
            }

            return stanceType;
        }
    }
}
