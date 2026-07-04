using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Core.ApiLogs
{
    /// <summary>
    /// API日志查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("API日志查询实体")]
    public class ApiLogCriteria : Criteria
    {
        #region 接口名 ApiName
        /// <summary>
        /// 接口名
        /// </summary>
        [Label("接口名")]
        public static readonly Property<string> ApiNameProperty = P<ApiLogCriteria>.Register(e => e.ApiName);

        /// <summary>
        /// 接口名
        /// </summary>
        public string ApiName
        {
            get { return this.GetProperty(ApiNameProperty); }
            set { this.SetProperty(ApiNameProperty, value); }
        }
        #endregion

        #region 控制器 Controller
        /// <summary>
        /// 控制器
        /// </summary>
        [Label("控制器")]
        public static readonly Property<string> ControllerProperty = P<ApiLogCriteria>.Register(e => e.Controller);

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
        public static readonly Property<string> MethodProperty = P<ApiLogCriteria>.Register(e => e.Method);

        /// <summary>
        /// 方法
        /// </summary>
        public string Method
        {
            get { return this.GetProperty(MethodProperty); }
            set { this.SetProperty(MethodProperty, value); }
        }
        #endregion

        #region 用户名 CreateByName
        /// <summary>
        /// 用户名
        /// </summary>
        [Label("用户名")]
        public static readonly Property<string> CreateByNameProperty = P<ApiLogCriteria>.Register(e => e.CreateByName);

        /// <summary>
        /// 用户名
        /// </summary>
        public string CreateByName
        {
            get { return this.GetProperty(CreateByNameProperty); }
            set { this.SetProperty(CreateByNameProperty, value); }
        }
        #endregion

        #region 开始时间 StartTime
        /// <summary>
        /// 开始时间
        /// </summary>
        [Label("开始时间")]
        public static readonly Property<DateRange> StartTimeProperty = P<ApiLogCriteria>.Register(e => e.StartTime);

        /// <summary>
        /// 开始时间
        /// </summary>
        public DateRange StartTime
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
        public static readonly Property<DateRange> EndTimeProperty = P<ApiLogCriteria>.Register(e => e.EndTime);

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateRange EndTime
        {
            get { return this.GetProperty(EndTimeProperty); }
            set { this.SetProperty(EndTimeProperty, value); }
        }
        #endregion

        #region 关键字 KeyValue
        /// <summary>
        /// 关键字
        /// </summary>
        [Label("关键字")]
        public static readonly Property<string> KeyValueProperty = P<ApiLogCriteria>.Register(e => e.KeyValue);

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyValue
        {
            get { return this.GetProperty(KeyValueProperty); }
            set { this.SetProperty(KeyValueProperty, value); }
        }
        #endregion

        /// <summary>
        /// 重写此方法实现查询
        /// </summary>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ApiLogController>().GetApiLogs(this);
        }

    }
}
