using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.Warehouses
{
    /// <summary>
    /// 仓库查询
    /// </summary>
    [QueryEntity, Serializable]
    [Label("仓库查询")]
    public partial class InWarehouseEmployeeSelectCriteria : WarehouseCriteria
    {
        #region 库存组织 InvOrgId
        /// <summary>
        /// 编码
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<InWarehouseEmployeeSelectCriteria>.Register(e => e.InvOrgId);

        /// <summary>
        /// 编码
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        /// <summary>
        /// 查询逻辑
        /// </summary>
        /// <returns>返回查询后的数据</returns>
        protected override EntityList Fetch()
        {
            var result = RT.Service.Resolve<WarehouseController>().GetAllWarehouseData(this);
            return result;
        }
    }
}
