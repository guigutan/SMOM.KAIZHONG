using System;
using SIE.Common.Employees;
using SIE.Common.Users;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API接口日志
    /// </summary>   
    [RootEntity, Serializable]    
    [ConditionQueryType(typeof(ApiLogCriteria))]
    [Label("API接口日志")]
    public class ApiLog : Entity<double>
    {
        #region 日志ID LogId
        /// <summary>
        /// 日志ID
        /// </summary>
        [Label("日志ID")]
        public static readonly Property<string> LogIdProperty = P<ApiLog>.Register(e => e.LogId);

        /// <summary>
        /// 日志ID
        /// </summary>
        public string LogId
        {
            get { return this.GetProperty(LogIdProperty); }
            set { this.SetProperty(LogIdProperty, value); }
        }
        #endregion

        #region 控制器 Controller
        /// <summary>
        /// 控制器
        /// </summary>
        [Label("控制器")]
        public static readonly Property<string> ControllerProperty = P<ApiLog>.Register(e => e.Controller);

        /// <summary>
        /// 控制器
        /// </summary>
        public string Controller
        {
            get { return this.GetProperty(ControllerProperty); }
            set { this.SetProperty(ControllerProperty, value); }
        }
        #endregion

        #region 方法 Method
        /// <summary>
        /// 方法
        /// </summary>
        [Label("方法")]
        public static readonly Property<string> MethodProperty = P<ApiLog>.Register(e => e.Method);

        /// <summary>
        /// 方法
        /// </summary>
        public string Method
        {
            get { return this.GetProperty(MethodProperty); }
            set { this.SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 接口名 ApiName
        /// <summary>
        /// 接口名
        /// </summary>
        [Label("接口名")]
        public static readonly Property<string> ApiNameProperty = P<ApiLog>.Register(e => e.ApiName);

        /// <summary>
        /// 接口名
        /// </summary>
        public string ApiName
        {
            get { return this.GetProperty(ApiNameProperty); }
            set { this.SetProperty(ApiNameProperty, value); }
        }
        #endregion

        #region 请求参数 Request
        /// <summary>
        /// 请求参数
        /// </summary>
        [Label("请求参数")]
        [MaxLength(4000)]
        public static readonly Property<string> RequestProperty = P<ApiLog>.Register(e => e.Request);

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Request
        {
            get { return this.GetProperty(RequestProperty); }
            set { this.SetProperty(RequestProperty, value); }
        }
        #endregion

        #region 返回结果 Response
        /// <summary>
        /// 返回结果
        /// </summary>
        [Label("返回结果")]
        [MaxLength(4000)]
        public static readonly Property<string> ResponseProperty = P<ApiLog>.Register(e => e.Response);

        /// <summary>
        /// 返回结果
        /// </summary>
        public string Response
        {
            get { return this.GetProperty(ResponseProperty); }
            set { this.SetProperty(ResponseProperty, value); }
        }
        #endregion

        #region 是否成功 IsSuccess
        /// <summary>
        /// 是否成功
        /// </summary>
        [Label("是否成功")]
        public static readonly Property<YesNo> IsSuccessProperty = P<ApiLog>.Register(e => e.IsSuccess);

        /// <summary>
        /// 是否成功
        /// </summary>
        public YesNo IsSuccess
        {
            get { return this.GetProperty(IsSuccessProperty); }
            set { this.SetProperty(IsSuccessProperty, value); }
        }
        #endregion

        #region 是否异常 HasException
        /// <summary>
        /// 是否异常
        /// </summary>
        [Label("是否异常")]
        public static readonly Property<YesNo> HasExceptionProperty = P<ApiLog>.Register(e => e.HasException);

        /// <summary>
        /// 是否异常
        /// </summary>
        public YesNo HasException
        {
            get { return this.GetProperty(HasExceptionProperty); }
            set { this.SetProperty(HasExceptionProperty, value); }
        }
        #endregion

        #region 开始时间 StartTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateTime> StartTimeProperty = P<ApiLog>.Register(e => e.StartTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime StartTime
        {
            get { return this.GetProperty(StartTimeProperty); }
            set { this.SetProperty(StartTimeProperty, value); }
        }
        #endregion

        #region 结束时间 EndTime
        /// <summary>
        /// 结束时间
        /// </summary>
        [Label("结束时间")]
        public static readonly Property<DateTime> EndTimeProperty = P<ApiLog>.Register(e => e.EndTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 耗时 TimeSpanMilliseconds
        /// <summary>
        /// 耗时
        /// </summary>
        [Label("耗时(毫秒)")]
        public static readonly Property<double> TimeSpanMillisecondsProperty = P<ApiLog>.Register(e => e.TimeSpanMilliseconds);

        /// <summary>
        /// 耗时
        /// </summary>
        public double TimeSpanMilliseconds
        {
            get { return this.GetProperty(TimeSpanMillisecondsProperty); }
            set { this.SetProperty(TimeSpanMillisecondsProperty, value); }
        }
        #endregion

        #region 库存组织 InvOrgId
        /// <summary>
        /// 库存组织
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<int?> InvOrgIdProperty = P<ApiLog>.Register(e => e.InvOrgId);

        /// <summary>
        /// 库存组织
        /// </summary>
        public int? InvOrgId
        {
            get { return this.GetProperty(InvOrgIdProperty); }
            set { this.SetProperty(InvOrgIdProperty, value); }
        }
        #endregion

        #region 库存组织名称 InvOrgName
        /// <summary>
        /// 库存组织名称
        /// </summary>
        [Label("库存组织")]
        public static readonly Property<string> InvOrgNameProperty = P<ApiLog>.Register(e => e.InvOrgName);

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string InvOrgName
        {
            get { return this.GetProperty(InvOrgNameProperty); }
            set { this.SetProperty(InvOrgNameProperty, value); }
        }
        #endregion

        #region 员工 Employee
        /// <summary>
        /// 员工Id
        /// </summary>
        [Label("员工")]
        public static readonly IRefIdProperty EmployeeIdProperty =
            P<ApiLog>.RegisterRefId(e => e.EmployeeId, ReferenceType.Normal);

        /// <summary>
        /// 员工Id
        /// </summary>
        public double? EmployeeId
        {
            get { return (double?)this.GetRefNullableId(EmployeeIdProperty); }
            set { this.SetRefNullableId(EmployeeIdProperty, value); }
        }

        /// <summary>
        /// 员工
        /// </summary>
        public static readonly RefEntityProperty<Employee> EmployeeProperty =
            P<ApiLog>.RegisterRef(e => e.Employee, EmployeeIdProperty);

        /// <summary>
        /// 员工
        /// </summary>
        public Employee Employee
        {
            get { return this.GetRefEntity(EmployeeProperty); }
            set { this.SetRefEntity(EmployeeProperty, value); }
        }
        #endregion

        #region 用户 User
        /// <summary>
        /// 用户Id
        /// </summary>
        [Label("用户名称")]
        public static readonly IRefIdProperty UserIdProperty =
            P<ApiLog>.RegisterRefId(e => e.UserId, ReferenceType.Normal);

        /// <summary>
        /// 用户Id
        /// </summary>
        public double? UserId
        {
            get { return (double?)this.GetRefNullableId(UserIdProperty); }
            set { this.SetRefNullableId(UserIdProperty, value); }
        }

        /// <summary>
        /// 用户
        /// </summary>
        public static readonly RefEntityProperty<User> UserProperty =
            P<ApiLog>.RegisterRef(e => e.User, UserIdProperty);

        /// <summary>
        /// 用户
        /// </summary>
        public User User
        {
            get { return this.GetRefEntity(UserProperty); }
            set { this.SetRefEntity(UserProperty, value); }
        }
        #endregion

        #region 关键字1 Key1
        /// <summary>
        /// 关键字1
        /// </summary>
        [Label("关键字1")]
        [MaxLength(2000)]
        public static readonly Property<string> Key1Property = P<ApiLog>.Register(e => e.Key1);

        /// <summary>
        /// 关键字1
        /// </summary>
        public string Key1
        {
            get { return this.GetProperty(Key1Property); }
            set { this.SetProperty(Key1Property, value); }
        }
        #endregion

        #region 关键字2 Key2
        /// <summary>
        /// 关键字2
        /// </summary>
        [Label("关键字2")]
        [MaxLength(2000)]
        public static readonly Property<string> Key2Property = P<ApiLog>.Register(e => e.Key2);

        /// <summary>
        /// 关键字2
        /// </summary>
        public string Key2
        {
            get { return this.GetProperty(Key2Property); }
            set { this.SetProperty(Key2Property, value); }
        }
        #endregion

        #region 关键字3 Key3
        /// <summary>
        /// 关键字3
        /// </summary>
        [Label("关键字3")]
        [MaxLength(2000)]
        public static readonly Property<string> Key3Property = P<ApiLog>.Register(e => e.Key3);

        /// <summary>
        /// 关键字3
        /// </summary>
        public string Key3
        {
            get { return this.GetProperty(Key3Property); }
            set { this.SetProperty(Key3Property, value); }
        }
        #endregion

        #region 关键字4 Key4
        /// <summary>
        /// 关键字4
        /// </summary>
        [Label("关键字4")]
        [MaxLength(2000)]
        public static readonly Property<string> Key4Property = P<ApiLog>.Register(e => e.Key4);

        /// <summary>
        /// 关键字4
        /// </summary>
        public string Key4
        {
            get { return this.GetProperty(Key4Property); }
            set { this.SetProperty(Key4Property, value); }
        }
        #endregion

        #region 关键字5 Key5
        /// <summary>
        /// 关键字5
        /// </summary>
        [Label("关键字5")]
        [MaxLength(2000)]
        public static readonly Property<string> Key5Property = P<ApiLog>.Register(e => e.Key5);

        /// <summary>
        /// 关键字5
        /// </summary>
        public string Key5
        {
            get { return this.GetProperty(Key5Property); }
            set { this.SetProperty(Key5Property, value); }
        }
        #endregion

        #region 用户名 EmployeeName
        /// <summary>
        /// 用户名
        /// </summary>
        [Label("用户名")]
        public static readonly Property<string> EmployeeNameProperty = P<ApiLog>.RegisterView(e => e.EmployeeName, p => p.Employee.Name);

        /// <summary>
        /// 用户名
        /// </summary>
        public string EmployeeName
        {
            get { return this.GetProperty(EmployeeNameProperty); }
        }
        #endregion


    }

    /// <summary>
    /// API日志 实体配置
    /// </summary>
    internal class ApiLogConfig : EntityConfig<ApiLog>
    {
        /// <summary>
        /// 配置数据库映射
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("API_LOG").MapAllProperties();
            Meta.Property(ApiLog.RequestProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(ApiLog.ResponseProperty).ColumnMeta.HasLength("MAX");
            Meta.Property(ApiLog.Key1Property).ColumnMeta.HasLength(4000);
            Meta.Property(ApiLog.Key2Property).ColumnMeta.HasLength(4000);
            Meta.Property(ApiLog.Key3Property).ColumnMeta.HasLength(4000);
            Meta.Property(ApiLog.Key4Property).ColumnMeta.HasLength(4000);
            Meta.Property(ApiLog.Key5Property).ColumnMeta.HasLength(4000);
            Meta.DisablePhantoms();
            Meta.DisableInvOrg();
        }
    }
}
