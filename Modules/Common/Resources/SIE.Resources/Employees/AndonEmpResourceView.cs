using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Resources.Employees
{
    /// <summary>
    /// 安灯区域员工产线查询查询实体 - 视图实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("安灯区域员工产线查询实体")]
    public class AndonEmpResourceView : DataEntity
    {

        #region 区域编号 AndonArea
        /// <summary>
        /// 区域编号
        /// </summary>
        [Label("区域编号")]
        public static readonly Property<string> AndonAreaProperty = P<AndonEmpResourceView>.Register(e => e.AndonArea);

        /// <summary>
        /// 区域编号
        /// </summary>
        public string AndonArea
        {
            get { return GetProperty(AndonAreaProperty); }
            set { SetProperty(AndonAreaProperty, value); }
        }
        #endregion

        #region 产线编号 ProdLine
        /// <summary>
        /// 产线编号
        /// </summary>
        [Label("产线编号")]
        public static readonly Property<string> ProdLineProperty = P<AndonEmpResourceView>.Register(e => e.ProdLine);

        /// <summary>
        /// 产线编号
        /// </summary>
        public string ProdLine
        {
            get { return GetProperty(ProdLineProperty); }
            set { SetProperty(ProdLineProperty, value); }
        }
        #endregion

        #region 安灯员工ID EmployeeId
        /// <summary>
        /// 安灯员工ID
        /// </summary>
        [Label("安灯员工ID")]
        public static readonly Property<double> EmployeeIdProperty = P<AndonEmpResourceView>.Register(e => e.EmployeeId);

        /// <summary>
        /// 安灯员工ID
        /// </summary>
        public double EmployeeId
        {
            get { return GetProperty(EmployeeIdProperty); }
            set { SetProperty(EmployeeIdProperty, value); }
        }
        #endregion

        #region 产线ID ResourceId
        /// <summary>
        /// 产线ID
        /// </summary>
        [Label("产线ID")]
        public static readonly Property<double> ResourceIdProperty = P<AndonEmpResourceView>.Register(e => e.ResourceId);

        /// <summary>
        /// 产线ID
        /// </summary>
        public double ResourceId
        {
            get { return GetProperty(ResourceIdProperty); }
            set { SetProperty(ResourceIdProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 报废明细查询实体 实体配置
    /// </summary>
    internal class AndonEmpResourceViewEntityConfig : EntityConfig<AndonEmpResourceView>
    {
        /// <summary>
        /// 配置元数据
        /// </summary>
        protected override void ConfigMeta()
        {
            // 映射到视图而不是表
            Meta.MapTable("V_ANDON_RESOURCE_INFO").MapAllProperties();

            // 或者使用SQL视图定义（如果使用SQL视图）
            // Meta.IsView = true;
            // Meta.ViewSql = @"SELECT ... FROM ..."; // 在这里定义视图的SQL
            //Meta.EnableSort();
        }
    }
}
