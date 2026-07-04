using SIE.Domain;
using SIE.MetaModel;
using SIE.Resources;
using SIE.Tech.Processs;
using System;

namespace SIE.MES.WIP.Products
{
    /// <summary>
    /// 跳站操作日志
    /// </summary>
    [RootEntity, Serializable]
    public class WipGotoLog : Entity<double>
    {
        #region 前工序 FromProcess
        /// <summary>
        /// 前工序Id
        /// </summary>
        public static readonly IRefIdProperty FromProcessIdProperty =
            P<WipGotoLog>.RegisterRefId(e => e.FromProcessId, ReferenceType.Normal);

        /// <summary>
        /// 前工序Id
        /// </summary>
        public double FromProcessId
        {
            get { return (double)this.GetRefId(FromProcessIdProperty); }
            set { this.SetRefId(FromProcessIdProperty, value); }
        }

        /// <summary>
        /// 前工序
        /// </summary>
        public static readonly RefEntityProperty<Process> FromProcessProperty =
            P<WipGotoLog>.RegisterRef(e => e.FromProcess, FromProcessIdProperty);

        /// <summary>
        /// 前工序
        /// </summary>
        public Process FromProcess
        {
            get { return this.GetRefEntity(FromProcessProperty); }
            set { this.SetRefEntity(FromProcessProperty, value); }
        }
        #endregion

        #region 跳转工序 ToProcess
        /// <summary>
        /// 跳转工序Id
        /// </summary>
        public static readonly IRefIdProperty ToProcessIdProperty =
            P<WipGotoLog>.RegisterRefId(e => e.ToProcessId, ReferenceType.Normal);

        /// <summary>
        /// 跳转工序Id
        /// </summary>
        public double ToProcessId
        {
            get { return (double)this.GetRefId(ToProcessIdProperty); }
            set { this.SetRefId(ToProcessIdProperty, value); }
        }

        /// <summary>
        /// 跳转工序
        /// </summary>
        public static readonly RefEntityProperty<Process> ToProcessProperty =
            P<WipGotoLog>.RegisterRef(e => e.ToProcess, ToProcessIdProperty);

        /// <summary>
        /// 跳转工序
        /// </summary>
        public Process ToProcess
        {
            get { return this.GetRefEntity(ToProcessProperty); }
            set { this.SetRefEntity(ToProcessProperty, value); }
        }
        #endregion

        #region 操作人 User
        /// <summary>
        /// 操作人Id
        /// </summary>
        public static readonly IRefIdProperty UserIdProperty =
            P<WipGotoLog>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 操作人Id
        /// </summary>
        public double UserId
        {
            get { return (double)this.GetRefId(UserIdProperty); }
            set { this.SetRefId(UserIdProperty, value); }
        }

        /// <summary>
        /// 操作人
        /// </summary>
        public static readonly RefEntityProperty<Employee> UserProperty =
            P<WipGotoLog>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 操作人
        /// </summary>
        public Employee User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 记录时间 LogDate
        /// <summary>
        /// 记录时间
        /// </summary>
        public static readonly Property<DateTime> LogDateProperty = P<WipGotoLog>.Register(e => e.LogDate);

        /// <summary>
        /// 记录时间
        /// </summary>
        public DateTime LogDate
        {
            get { return this.GetProperty(LogDateProperty); }
            set { this.SetProperty(LogDateProperty, value); }
        }
        #endregion
    }

    /// <summary>
    /// 跳站操作日志 实体配置
    /// </summary>
    class WipGotoLogConfig : EntityConfig<WipGotoLog>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("WIP_GOTO_LOG").MapAllProperties();
            Meta.DisablePhantoms();
        }
    }
}
