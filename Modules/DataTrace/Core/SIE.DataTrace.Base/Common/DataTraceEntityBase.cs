using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.Text;

namespace SIE.DataTrace.Base.Common
{
    /// <summary>
    /// 数据追溯实体基类
    /// </summary>
    [RootEntity, Serializable]
    public class DataTraceEntityBase : DataEntity
    {
        #region 创建人 CreateUser
        /// <summary>
        /// 创建人
        /// </summary>
        [Label("创建人")]
        public static readonly IRefIdProperty SrcCreateByProperty =
            P<DataTraceEntityBase>.RegisterRefId(e => e.SrcCreateBy, ReferenceType.Normal);

        /// <summary>
        /// 创建人
        /// </summary>
        public double SrcCreateBy
        {
            get { return (double)this.GetRefId(SrcCreateByProperty); }
            set { this.SetRefId(SrcCreateByProperty, value); }
        }

        /// <summary>
        /// 创建人
        /// </summary>
        static readonly RefEntityProperty<Employee> CreateUserProperty = P<DataTraceEntityBase>.RegisterRef(e => e.CreateUser, SrcCreateByProperty);

        /// <summary>
        /// 创建人
        /// </summary>
        Employee CreateUser
        {
            get { return this.GetRefEntity(CreateUserProperty); }
            set { this.SetRefEntity(CreateUserProperty, value); }
        }


        #endregion

        #region 修改人 UpdateUser
        /// <summary>
        /// 修改人
        /// </summary>
        [Label("修改人")]
        public static readonly IRefIdProperty SrcUpdateByProperty =
            P<DataTraceEntityBase>.RegisterRefId(e => e.SrcUpdateBy, ReferenceType.Normal);

        /// <summary>
        /// 修改人
        /// </summary>
        public double SrcUpdateBy
        {
            get { return (double)this.GetRefId(SrcUpdateByProperty); }
            set { this.SetRefId(SrcUpdateByProperty, value); }
        }

        /// <summary>
        /// 修改人
        /// </summary>
        static readonly RefEntityProperty<Employee> UpdateUserProperty = P<DataTraceEntityBase>.RegisterRef(e => e.UpdateUser, SrcUpdateByProperty);

        /// <summary>
        /// 修改人
        /// </summary>
        Employee UpdateUser
        {
            get { return this.GetRefEntity(UpdateUserProperty); }
            set { this.SetRefEntity(UpdateUserProperty, value); }
        }


        #endregion

        #region 创建时间

        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateTime> SrcCreateDateProperty = P<DataTraceEntityBase>.Register(e => e.SrcCreateDate);
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime SrcCreateDate
        {
            get { return this.GetProperty(SrcCreateDateProperty); }
            set { this.SetProperty(SrcCreateDateProperty, value); }
        }

        #endregion

        #region 更新日期

        /// <summary>
        /// 更新日期
        /// </summary>
        [Label("更新日期")]
        public static readonly Property<DateTime> SrcUpdateDateProperty = P<DataTraceEntityBase>.Register(e => e.SrcUpdateDate);
        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime SrcUpdateDate
        {
            get { return this.GetProperty(SrcUpdateDateProperty); }
            set { this.SetProperty(SrcUpdateDateProperty, value); }
        }

        #endregion
    }
}
