using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.EMS.Equipments.Boms
{
    /// <summary>
    /// 选择设备BOM查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("选择设备BOM查询实体")]
    public class EquipBomSelectCriteria : Criteria
    {
        #region 编码 Code
        /// <summary>
        /// 编码
        /// </summary>
        [Label("编码")]
        public static readonly Property<string> CodeProperty = P<EquipBomSelectCriteria>.Register(e => e.Code);

        /// <summary>
        /// 编码
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 名称 Name
        /// <summary>
        /// 名称
        /// </summary>
        [Label("名称")]
        public static readonly Property<string> NameProperty = P<EquipBomSelectCriteria>.Register(e => e.Name);

        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        #endregion

        #region 已选Ids ExceptIds
        /// <summary>
        /// 已选Ids
        /// </summary>
        [Label("已选Ids")]
        public static readonly Property<string> ExceptIdsProperty = P<EquipBomSelectCriteria>.Register(e => e.ExceptIds);

        /// <summary>
        /// 已选Ids
        /// </summary>
        public string ExceptIds
        {
            get { return this.GetProperty(ExceptIdsProperty); }
            set { this.SetProperty(ExceptIdsProperty, value); }
        }
        #endregion


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<EquipBomController>().QuerySelectEquipBom(this);
        }
    }
}
