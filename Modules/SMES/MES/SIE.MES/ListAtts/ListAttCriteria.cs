using SIE.Domain;
using SIE.MES.OrgLevels;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ListAtts
{

    /// <summary>
    /// 考勤数据查询实体
    /// </summary>
    [QueryEntity, Serializable]
    public class ListAttCriteria: Criteria
    {

        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]       
        public static readonly Property<string> DeptNameProperty = P<ListAttCriteria>.Register(e => e.DeptName);
        /// <summary>
        /// 部门名称
        /// </summary>
        public string DeptName
        {
            get { return this.GetProperty(DeptNameProperty); }
            set { this.SetProperty(DeptNameProperty, value); }
        }
        /// <summary>
        /// 区域名称
        /// </summary>
        [Label("区域名称")]
        public static readonly Property<string> AreaNameProperty = P<ListAttCriteria>.Register(e => e.AreaName);
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName
        {
            get { return this.GetProperty(AreaNameProperty); }
            set { this.SetProperty(AreaNameProperty, value); }
        }

        /// <summary>
        /// 人员编号
        /// </summary>
        [Label("人员编号")]
        public static readonly Property<string> PinProperty = P<ListAttCriteria>.Register(e => e.Pin);
        /// <summary>
        /// 人员编号
        /// </summary>
        public string Pin
        {
            get { return this.GetProperty(PinProperty); }
            set { this.SetProperty(PinProperty, value); }
        }

        /// <summary>
        /// 人员姓名
        /// </summary>
        [Label("人员姓名")]
        public static readonly Property<string> NameProperty = P<ListAttCriteria>.Register(e => e.Name);
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }

        /// <summary>
        /// 记录设备触发时间-开始
        /// </summary>
        [Label("记录设备触发时间-开始")]
        public static readonly Property<DateTime?> EventTimeSProperty = P<ListAttCriteria>.Register(e => e.EventTimeS);
        /// <summary>
        /// 记录设备触发时间-开始
        /// </summary>
        public DateTime? EventTimeS
        {
            get { return this.GetProperty(EventTimeSProperty); }
            set { this.SetProperty(EventTimeSProperty, value); }
        }
        /// <summary>
        /// 记录设备触发时间-结束
        /// </summary>
        [Label("记录设备触发时间-结束")]
        public static readonly Property<DateTime?> EventTimeEProperty = P<ListAttCriteria>.Register(e => e.EventTimeE);
        /// <summary>
        /// 记录设备触发时间-结束
        /// </summary>
        public DateTime? EventTimeE
        {
            get { return this.GetProperty(EventTimeEProperty); }
            set { this.SetProperty(EventTimeEProperty, value); }
        }

        /// <summary>
        /// 查询方法
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            return RT.Service.Resolve<ListAttController>().Fetch(this);
        }
    }
}
