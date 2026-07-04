using SIE.Domain.Validation;
using SIE.Resources.Employees;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库删除规则
    /// </summary>
    [System.ComponentModel.DisplayName("仓库删除规则")]
    [System.ComponentModel.Description("关联库区不能删除")]
    public class WarehouseNoReferenceRoutingRule : NoReferencedRule<Warehouse>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public WarehouseNoReferenceRoutingRule()
        {
            Properties.Add(StorageArea.WarehouseIdProperty);
        }
    }
    
    /// <summary>
    /// 人员删除规则
    /// </summary>
    [System.ComponentModel.DisplayName("人员删除规则")]
    [System.ComponentModel.Description("关联仓库不能删除")]
    public class WarehouseEmployeeNoReferenceRoutingRule : NoReferencedRule<Employee>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public WarehouseEmployeeNoReferenceRoutingRule()
        {
            Properties.Add(WarehouseEmployee.EmployeeIdProperty);
        }
    }

    /// <summary>
    /// 库区删除规则
    /// </summary>
    [System.ComponentModel.DisplayName("库区删除规则")]
    [System.ComponentModel.Description("关联库位不能删除")]
    public class StorageAreaNoReferenceRoutingRule : NoReferencedRule<StorageArea>
    {
        /// <summary>
        /// 初始化需要进行验证的属性
        /// </summary>
        public StorageAreaNoReferenceRoutingRule()
        {
            Properties.Add(StorageLocation.AreaIdProperty);
        }
    }

    #region 同一仓库中库区编码非重验证规则
    /// <summary>
    /// 同一仓库中库区编码非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("同一仓库中库区编码非重验证规则")]
    [System.ComponentModel.Description("同一仓库中库区编码非重验证规则")]
    public class NotDuplicateStorageArea : NotDuplicateRule<StorageArea>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateStorageArea()
        {
            Properties.Add(StorageArea.WarehouseIdProperty);
            Properties.Add(StorageArea.CodeProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as StorageArea;
                return "同一仓库已经存在相同【{0}】库区编码".L10nFormat(entity.Code);
            };
        }
    }
    #endregion

    #region 同一仓库中库区名称非重验证规则
    /// <summary>
    /// 同一仓库中库区编码非重验证规则
    /// </summary>
    [System.ComponentModel.DisplayName("同一仓库中库区名称非重验证规则")]
    [System.ComponentModel.Description("同一仓库中库区名称非重验证规则")]
    public class NotDuplicateStorageAreaName : NotDuplicateRule<StorageArea>
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public NotDuplicateStorageAreaName()
        {
            Properties.Add(StorageArea.WarehouseIdProperty);
            Properties.Add(StorageArea.NameProperty);
            MessageBuilder = (e) =>
            {
                var entity = e as StorageArea;
                return "同一仓库已经存在相同【{0}】库区名称".L10nFormat(entity.Name);
            };
        }
    }
    #endregion

}
