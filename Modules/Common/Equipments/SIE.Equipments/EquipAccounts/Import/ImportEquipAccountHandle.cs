using DocumentFormat.OpenXml.InkML;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.VariantTypes;
using DocumentFormat.OpenXml.Wordprocessing;
using NPOI.SS.Formula.Functions;
using NPOI.Util;
using SIE.Common.Catalogs;
using SIE.Common.ImportHelper;
using SIE.Core.Enums;
using SIE.Core.Equipments;
using SIE.CSM.Suppliers;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.Equipments.EquipAccounts;
using SIE.Resources.Employees;
using SIE.Resources.Enterprises;
using SIE.Tech.Processs;
using SIE.Warehouses;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using EquipAccount = SIE.Equipments.EquipAccounts.EquipAccount;

namespace SIE.Equipments.EquipModels.ImportEquipModel
{
    /// <summary>
    /// 设备台账-导入逻辑处理
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportEquipAccountHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportEquipAccountHandle : IBusinessImport
    {
        private const string EMPTY_FORMAT = "{0}为空";
        #region 基础验证

        #region 暂存数据集
        /// <summary>
        /// 是否属性
        /// </summary>
        private Dictionary<string, Enum> dicYesNo { get; set; } = new Dictionary<string, Enum>();

        /// <summary>
        /// 管理状态
        /// </summary>
        private Dictionary<string, Enum> dicUseState { get; set; } = new Dictionary<string, Enum>();

        /// <summary>
        /// 资产来源
        /// </summary>
        private Dictionary<string, Enum> dicProprietorship { get; set; } = new Dictionary<string, Enum>();

        /// <summary>
        /// 设备类型编码集
        /// </summary>
        private Dictionary<string, double> dicEquipTypeCode { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 型号编码集
        /// </summary>
        private Dictionary<string, double> dicEquipModelCode { get; set; } = null;

        /// <summary>
        /// 部门名称集
        /// </summary>
        private Dictionary<string, double> dicFactoryName { get; set; } = null;

        /// <summary>
        /// 仓库名称集
        /// </summary>
        private Dictionary<string, double> dicWarehouseName { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 员工名称集
        /// </summary>
        private Dictionary<string, double> dicEmployeeName { get; set; } = new Dictionary<string, double>();

        /// <summary>
        /// 供应商编码集
        /// </summary>
        private Dictionary<string, double> dicSupplierCode { get; set; } = new Dictionary<string, double>();

        #endregion

        #region 枚举数据验证

        /// <summary>
        /// 验证是否属性
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidYesNo(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(EMPTY_FORMAT.L10nFormat(nameof(obj)));
            }

            return ValidEnum<YesNo>(dicYesNo, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证管理状态
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidAccountUseState(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                messageTip = string.Empty;
                return true;//为空不验证
            }

            return ValidEnum<AccountUseState>(dicUseState, obj.ToString(), out messageTip);
        }

        /// <summary>
        /// 验证产权归属
        /// </summary>
        /// <param name="obj">枚举值</param>
        /// <param name="messageTip">错误信息</param>
        /// <param name="dr">验证数据的当前行对象</param>
        /// <returns>返回是否验证通过</returns>
        public bool ValidProprietorship(object obj, out string messageTip, DataRow dr)
        {
            if (obj == null)
            {
                throw new ValidationException(EMPTY_FORMAT.L10nFormat(nameof(obj)));
            }

            return ValidEnum<Proprietorship>(dicProprietorship, obj.ToString(), out messageTip);
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
                dicEquipModelCode = new Dictionary<string, double>();

            if (!dicEquipModelCode.ContainsKey(content))
            {
                var equipModel = RT.Service.Resolve<EquipModelController>().GetEquipModelByCode(content);
                if (equipModel != null)
                    dicEquipModelCode.Add(content, equipModel.Id);
                else
                {
                    messageTip = "不存在于系统".L10N();
                    return false;
                }
            }

            return true;
        }
        #endregion  

        #region 工厂验证
        /// <summary>
        /// 工厂验证
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="messageTip">提示语</param>
        /// <param name="dr">行</param>
        /// <returns>true/false</returns>
        public bool ValidFactoryName(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            if (obj == null || obj.ToString() == string.Empty)
            {
                messageTip = "不能为空".L10N();
                return false;
            }
            string content = obj.ToString();

            if (dicFactoryName == null)
                dicFactoryName = new Dictionary<string, double>();

            if (!dicFactoryName.ContainsKey(content))
            {
                var factory = RT.Service.Resolve<EnterpriseController>().GetFactoryByName(content);
                if (factory != null)
                    dicFactoryName.Add(content, factory.Id);
                else
                {
                    messageTip = "不存在于系统".L10N();
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 员工验证
        /// <summary>
        /// 员工验证
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="messageTip">提示语</param>
        /// <param name="dr">行</param>
        /// <returns>true/false</returns>
        public bool ValidEmployee(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            if (obj == null || obj.ToString() == string.Empty)
            {
                return true;//为空不验证
            }
            string content = obj.ToString();

            if (dicEmployeeName == null)
                dicEmployeeName = new Dictionary<string, double>();

            if (!dicEmployeeName.ContainsKey(content))
            {
                var employee = RT.Service.Resolve<EmployeeController>().GetEmployeeByName(content);
                if (employee != null)
                    dicEmployeeName.Add(content, employee.Id);
                else
                {
                    messageTip = "不存在于系统".L10N();
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 供应商验证
        /// <summary>
        /// 供应商验证
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="messageTip">提示语</param>
        /// <param name="dr">行</param>
        /// <returns>true/false</returns>
        public bool ValidSupplier(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            if (obj == null || obj.ToString() == string.Empty)
            {
                return true;//为空不验证
            }
            string content = obj.ToString();

            if (dicSupplierCode == null)
                dicSupplierCode = new Dictionary<string, double>();

            if (!dicSupplierCode.ContainsKey(content))
            {
                var employee = RT.Service.Resolve<SupplierController>().GetSupplier(content);
                if (employee != null)
                    dicSupplierCode.Add(content, employee.Id);
                else
                {
                    messageTip = "不存在于系统".L10N();
                    return false;
                }
            }

            return true;
        }
        #endregion

        #region 仓库验证
        /// <summary>
        /// 供应商验证
        /// </summary>
        /// <param name="obj">对象</param>
        /// <param name="messageTip">提示语</param>
        /// <param name="dr">行</param>
        /// <returns>true/false</returns>
        public bool ValidWarehouse(object obj, out string messageTip, DataRow dr)
        {
            messageTip = string.Empty;
            if (obj == null || obj.ToString() == string.Empty)
            {
                return true;//为空不验证
            }
            string content = obj.ToString();

            if (dicWarehouseName == null)
                dicWarehouseName = new Dictionary<string, double>();

            if (!dicWarehouseName.ContainsKey(content))
            {
                var warehouse = RT.Service.Resolve<WarehouseController>().GetWarehouseByName(content);
                if (warehouse != null)
                    dicWarehouseName.Add(content, warehouse.Id);
                else
                {
                    messageTip = "不存在于系统".L10N();
                    return false;
                }
            }

            return true;
        }
        #endregion

        #endregion
        /// <summary>
        /// 导入模板的列头名
        /// </summary>
        public List<string> ColumnNameList { get; set; } = new List<string>{

            "设备型号编码*",
            "设备编码*",
            "设备名称*",
            "设备别名",
            "管理状态",
            "管理部门*",
            "工厂*",
            "使用部门*",
            "海关监管*",
            "车间",
            "产线"  ,
            "虚拟设备*"   ,
            "使用责任人"   ,
            "原厂序列号"   ,
            "RFID"  ,
            "ABC分类" ,
            "资产来源"    ,
            "租赁/客供单位" ,
            "资产责任人"   ,
            "入厂日期" ,
            "生产厂家"  ,
            "供应商编码"   ,
            "使用年限"  ,
            "保修期" ,
            "位置"  ,
            "仓库名称" ,
            "库位名称",
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
            this.ColumnValidList.Add("设备型号编码*", new ValidColumn(ImportDataType._String, true, ValidEquipModelCode));
            this.ColumnValidList.Add("设备编码*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("设备名称*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("管理状态", new ValidColumn(ImportDataType._String, false, ValidAccountUseState));
            this.ColumnValidList.Add("使用部门*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("工厂*", new ValidColumn(ImportDataType._String, true, ValidFactoryName));
            this.ColumnValidList.Add("管理部门*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("海关监管*", new ValidColumn(ImportDataType._String, true, 80));
            this.ColumnValidList.Add("虚拟设备*", new ValidColumn(ImportDataType._String, true, ValidYesNo));
            this.ColumnValidList.Add("使用责任人", new ValidColumn(ImportDataType._String, false, ValidEmployee));
            this.ColumnValidList.Add("资产来源", new ValidColumn(ImportDataType._String, false, ValidProprietorship));
            this.ColumnValidList.Add("仓库名称", new ValidColumn(ImportDataType._String, false, ValidWarehouse));
            this.ColumnValidList.Add("资产责任人", new ValidColumn(ImportDataType._String, false, ValidEmployee));
            this.ColumnValidList.Add("入厂日期", new ValidColumn(ImportDataType._Date, false, true));
            this.ColumnValidList.Add("供应商编码", new ValidColumn(ImportDataType._String, false, ValidSupplier));
            this.ColumnValidList.Add("使用年限", new ValidColumn(ImportDataType._Int, false, true));
            this.ColumnValidList.Add("保修期", new ValidColumn(ImportDataType._Date, false, true));

            return this;
        }

        /// <summary>
        /// 验证每行数据
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="error"></param>
        /// <param name="storageLocationId"></param>
        /// <param name="warehouseId"></param>
        /// <param name="factoryId"></param>
        /// <param name="useDepartmentId"></param>
        /// <param name="manageDepartmentId"></param>
        /// <param name="workShopId"></param>
        /// <param name="lineId"></param>
        /// <returns></returns>
        private bool ValidationData(DataRow dr, out string error, out double? storageLocationId, out double? warehouseId, out double factoryId, out double? useDepartmentId, out double? manageDepartmentId, out double? workShopId, out double? lineId)
        {
            error = string.Empty;
            storageLocationId = null;
            warehouseId = null;
            factoryId = 0;
            useDepartmentId = null;
            manageDepartmentId = null;
            workShopId = null;
            lineId = null;
            //库位
            var warehouseName = dr.Field<string>(ColIndex("仓库名称"));
            var storageLocationName = dr.Field<string>(ColIndex("库位名称"));
            Warehouse warehouse = null;

            if (warehouseName.IsNotEmpty())
            {
                warehouse = WarehouseList.FirstOrDefault(c => c.Name == warehouseName);
                if (warehouse == null)
                {
                    error = "仓库不存在".L10N();
                    return false;
                }
                warehouseId = warehouse.Id;
            }

            if (warehouseName.IsNullOrEmpty() && storageLocationName.IsNotEmpty())
            {
                error = "请填写仓库方可填写库位".L10N();
                return false;
            }

            if (storageLocationName.IsNotEmpty())
            {
                List<double> singleAreaIds = StorageAreaList.Where(c => c.WarehouseId == warehouse.Id).Select(c => c.Id).ToList();
                if (singleAreaIds.IsNullOrEmpty())
                {
                    error = "库位不属于仓库".L10N();
                    return false;
                }

                var storageLocation = StorageLocationList.FirstOrDefault(c => singleAreaIds.Contains(c.AreaId) && c.Name == storageLocationName);
                if (storageLocation == null)
                {
                    error = "库位不属于仓库".L10N();
                    return false;
                }
                storageLocationId = storageLocation.Id;

            }

            var useDepartmentName = dr.Field<string>(ColIndex("使用部门*"));
            var factoryName = dr.Field<string>(ColIndex("工厂*"));
            var manageDepartmentName = dr.Field<string>(ColIndex("管理部门*"));
            var workShopName = dr.Field<string>(ColIndex("车间"));
            var lineName = dr.Field<string>(ColIndex("产线"));

            var factory = EnterpriseList.FirstOrDefault(c => c.Name == factoryName && c.LevelType == EnterpriseType.Plant);
            if (factory == null)
            {
                error = "工厂不存在".L10N();
                return false;
            }
            factoryId = factory.Id;

            var useDepartment = EnterpriseList.FirstOrDefault(c => c.Name == useDepartmentName && c.LevelType == EnterpriseType.Department && c.TreePId == factory.Id);
            if (useDepartment == null)
            {
                error = "使用部门不属于工厂".L10N();
                return false;
            }
            useDepartmentId = useDepartment.Id;

            var manageDepartment = EnterpriseList.FirstOrDefault(c => c.Name == manageDepartmentName && c.LevelType == EnterpriseType.Department && c.TreePId == factory.Id);
            if (manageDepartment == null)
            {
                error = "管理部门不属于工厂".L10N();
                return false;
            }
            manageDepartmentId = manageDepartment.Id;


            Enterprise workShop = null;
            if (workShopName.IsNotEmpty())
            {
                List<double?> singleDepartmentIds = EnterpriseList.Where(c => c.TreePId == factory.Id).Select(c => (double?)c.Id).ToList();
                if (singleDepartmentIds.IsNullOrEmpty())
                {
                    error = "车间不属于工厂".L10N();
                    return false;
                }
                workShop = EnterpriseList.FirstOrDefault(c => singleDepartmentIds.Contains(c.TreePId) && c.Name == workShopName);
                if (workShop == null)
                {
                    error = "车间不属于工厂".L10N();
                    return false;
                }
                workShopId = workShop.Id;
            }
            if (lineName.IsNotEmpty())
            {
                if (workShop == null)
                {
                    error = "请填写车间方可填写产线".L10N();
                    return false;
                }

                var line = EnterpriseList.FirstOrDefault(c => c.Name == lineName && c.LevelType == EnterpriseType.Line && c.TreePId == workShop.Id);
                if (line == null)
                {
                    error = "产线不属于车间".L10N();
                    return false;
                }
                lineId = line.Id;
            }

            return true;
        }

        private EntityList<Warehouse> WarehouseList = new EntityList<Warehouse>();
        private EntityList<StorageArea> StorageAreaList = new EntityList<StorageArea>();
        private EntityList<StorageLocation> StorageLocationList = new EntityList<StorageLocation>();
        private EntityList<Enterprise> EnterpriseList = new EntityList<Enterprise>();

        /// <summary>
        /// 导入数据处理
        /// </summary>
        /// <param name="drs"></param>
        public void ProcessBusinessDataHandle(DataRow[] drs)
        {
            if (drs.Length < 1)
                return;
            var equipAccountCtr = RT.Service.Resolve<EquipAccountController>();
            var warehouseCtr = RT.Service.Resolve<WarehouseController>();
            var enterpriseCtr = RT.Service.Resolve<EnterpriseController>();

            var drsList = drs.ToList();
            var codeList = drsList.Select(c => c.Field<string>(ColIndex("设备编码*"))).ToList();
            var oldEquipAccountList = equipAccountCtr.GetEquipAccountsByCode(codeList);
            //仓库和库位数据
            var warehouseNameList = drsList.Select(c => c.Field<string>(ColIndex("仓库名称"))).Where(c=>c.IsNotEmpty()).ToList();
            WarehouseList = warehouseCtr.GetWarehouseListByNames(warehouseNameList);
            StorageAreaList = new EntityList<StorageArea>();
            StorageLocationList = new EntityList<StorageLocation>();
            if (WarehouseList.IsNotEmpty())
            {
                StorageAreaList = warehouseCtr.GetStorageAreasByWarehouseIds(WarehouseList.Select(c => c.Id).ToList());
                if (StorageAreaList.IsNotEmpty())
                    StorageLocationList = warehouseCtr.GetStorageLocationsByAreaIds(StorageAreaList.Select(c => c.Id).ToList());
            }

            //企业模型数据
            EnterpriseList = enterpriseCtr.GetEnterpriseAll();


            EntityList<EquipAccount> equipAccountList = new EntityList<EquipAccount>();



            foreach (var dr in drs)
            {
                if (!ValidationData(dr, out string error, out double? storageLocationId, out double? warehouseId, out double factoryId, out double? useDepartmentId, out double? manageDepartmentId, out double? workShopId, out double? lineId))
                {
                    dr[ImportDataHandle.MessageColumnName] = error;
                    continue;
                }
                var equipModelCode = dr.Field<string>(ColIndex("设备型号编码*"));
                var code = dr.Field<string>(ColIndex("设备编码*"));
                var name = dr.Field<string>(ColIndex("设备名称*"));
                var alias = dr.Field<string>(ColIndex("设备别名"));
                var useStateLabel = dr.Field<string>(ColIndex("管理状态"));
                var useDepartmentName = dr.Field<string>(ColIndex("使用部门*"));
                var factoryName = dr.Field<string>(ColIndex("工厂*"));
                var manageDepartmentName = dr.Field<string>(ColIndex("管理部门*"));
                var isCustomsSupervision = dr.Field<string>(ColIndex("海关监管*"));
                var workShopName = dr.Field<string>(ColIndex("车间"));
                var lineName = dr.Field<string>(ColIndex("产线"));
                var isVirtualLabel = dr.Field<string>(ColIndex("虚拟设备*"));
                var userName = dr.Field<string>(ColIndex("使用责任人"));
                var originalSerialNumber = dr.Field<string>(ColIndex("原厂序列号"));
                var rfid = dr.Field<string>(ColIndex("RFID"));
                var useLevel = dr.Field<string>(ColIndex("ABC分类"));
                var proprietorshipLabel = dr.Field<string>(ColIndex("资产来源"));
                var purchaseUnit = dr.Field<string>(ColIndex("租赁/客供单位"));
                var warehouseName = dr.Field<string>(ColIndex("仓库名称"));
                var storageLocationName = dr.Field<string>(ColIndex("库位名称"));
                var resPersonName = dr.Field<string>(ColIndex("资产责任人"));
                var enterDateStr = dr.Field<string>(ColIndex("入厂日期"));
                var manufacturer = dr.Field<string>(ColIndex("生产厂家"));
                var supplierCode = dr.Field<string>(ColIndex("供应商编码"));
                var usefulLifeStr = dr.Field<string>(ColIndex("使用年限"));
                var warrantyPeriodStr = dr.Field<string>(ColIndex("保修期"));
                var installationLocation = dr.Field<string>(ColIndex("位置"));

                // 判断主数据是否存在
                var equipAccount = oldEquipAccountList.FirstOrDefault(c => c.Code == code);
                if (equipAccount == null)
                    equipAccount = new EquipAccount();

                equipAccount.Code = code;
                equipAccount.Name = name;
                equipAccount.EquipModelId = dicEquipModelCode[equipModelCode];
                equipAccount.Alias = alias;
                equipAccount.UseState = useStateLabel.IsNotEmpty() ? (AccountUseState)dicUseState[useStateLabel] : AccountUseState.InIdle;
                equipAccount.UseDepartmentId = useDepartmentId;
                equipAccount.FactoryId = factoryId;
                equipAccount.ManageDepartmentId = manageDepartmentId;
                equipAccount.IsCustomsSupervision = isCustomsSupervision == "是" ? true : false;
                equipAccount.WorkShopId = workShopId;
                equipAccount.ResourceId = lineId;
                equipAccount.IsVirtual = (YesNo)dicYesNo[isVirtualLabel];
                if (userName.IsNotEmpty())
                    equipAccount.UserId = dicEmployeeName[userName];
                equipAccount.OriginalSerialNumber = originalSerialNumber;
                equipAccount.RFID = rfid;
                equipAccount.UseLevel = useLevel;
                if (proprietorshipLabel.IsNotEmpty())
                    equipAccount.Proprietorship = (Proprietorship)dicProprietorship[proprietorshipLabel]; ;
                equipAccount.PurchaseUnit = purchaseUnit;
                equipAccount.WarehouseId = warehouseId;
                equipAccount.StorageLocationId = storageLocationId;
                if (resPersonName.IsNotEmpty())
                    equipAccount.ResPersonId = dicEmployeeName[resPersonName];
                if (DateTime.TryParse(enterDateStr, out DateTime enterDate))
                    equipAccount.EnterDate = enterDate;
                equipAccount.Manufacturer = manufacturer;
                if (supplierCode.IsNotEmpty())
                    equipAccount.SupplierId = dicSupplierCode[supplierCode];
                if (Decimal.TryParse(usefulLifeStr, out decimal usefulLife))
                    equipAccount.UsefulLife = usefulLife;
                if (DateTime.TryParse(warrantyPeriodStr, out DateTime warrantyPeriod))
                    equipAccount.WarrantyPeriod = warrantyPeriod;
                equipAccount.InstallationLocation = installationLocation;

                equipAccountList.Add(equipAccount);
            }

            // 如果不能新增记录错误信息
            try
            {
                using (var tran = DB.TransactionScope(EquipmentEntityDataProvider.ConnectionStringName))
                {
                    RF.Save(equipAccountList);

                    tran.Complete();
                }
            }
            catch (Exception ex)
            {
                string strMsg = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                ImportExtension.BatchAppendText(drs.ToList(), ImportDataHandle.MessageColumnName, strMsg);
            }

        }
    }
}
