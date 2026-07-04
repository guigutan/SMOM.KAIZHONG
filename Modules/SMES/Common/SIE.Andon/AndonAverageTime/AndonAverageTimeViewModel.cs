using SIE.Andon.Andons;
using SIE.Domain;
using SIE.MetaModel;
using SIE.ObjectModel;
using SIE.Resources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.AndonAverageTime
{
    /// <summary>
    /// 安灯平均时长
    /// </summary>
    [RootEntity, Serializable]
    [ConditionQueryType(typeof(AndonAverageTimeViewModelCriteria))]
    [NotMapped]  // 告诉 ORM 不要为此类型创建数据库映射
    [Label("安灯平均时长")]
    public class AndonAverageTimeViewModel : ViewModel
    {

        /// <summary>
        /// 基于人员ID
        /// </summary>
        [Label("基于人员ID")]
        public static readonly Property<decimal> ByUserIdProperty = P<AndonAverageTimeViewModel>.Register(e => e.ByUserId);

        /// <summary>
        /// 基于人员ID
        /// </summary>
        public decimal ByUserId
        {
            get { return this.GetProperty(ByUserIdProperty); }
            set { this.SetProperty(ByUserIdProperty, value); }
        }

        /// <summary>
        /// 基于人员姓名
        /// </summary>
        [Label("基于人员姓名")]
        public static readonly Property<string> ByUserNameProperty = P<AndonAverageTimeViewModel>.Register(e => e.ByUserName);

        /// <summary>
        /// 基于人员姓名
        /// </summary>
        public string ByUserName
        {
            get { return this.GetProperty(ByUserNameProperty); }
            set { this.SetProperty(ByUserNameProperty, value); }
        }


        /// <summary>
        /// 基于人员
        /// </summary>
        [Label("基于人员")]
        public static readonly Property<Employee> ByUserProperty = P<AndonAverageTimeViewModel>.Register(e => e.ByUser);

        /// <summary>
        /// 基于人员
        /// </summary>
        public Employee ByUser
        {
            get { return this.GetProperty(ByUserProperty); }
            set { this.SetProperty(ByUserProperty, value); }
        }

        /// <summary>
        /// 处理平均时长
        /// </summary>
        [Label("处理平均时长")]
        public static readonly Property<decimal> HandleDurationAverageProperty = P<AndonAverageTimeViewModel>.Register(e => e.HandleDurationAverage);

        /// <summary>
        /// 处理平均时长
        /// </summary>
        public decimal HandleDurationAverage
        {
            get { return this.GetProperty(HandleDurationAverageProperty); }
            set { this.SetProperty(HandleDurationAverageProperty, value); }
        }

        /// <summary>
        /// 响应平均时长
        /// </summary>
        [Label("响应平均时长")]
        public static readonly Property<decimal> ResponseDurationAverageProperty = P<AndonAverageTimeViewModel>.Register(e => e.ResponseDurationAverage);

        /// <summary>
        /// 响应平均时长
        /// </summary>
        public decimal ResponseDurationAverage
        {
            get { return this.GetProperty(ResponseDurationAverageProperty); }
            set { this.SetProperty(ResponseDurationAverageProperty, value); }
        }

        /// <summary>
        /// 验收平均时长
        /// </summary>
        [Label("验收平均时长")]
        public static readonly Property<decimal> CheckDurationAverageProperty = P<AndonAverageTimeViewModel>.Register(e => e.CheckDurationAverage);

        /// <summary>
        /// 验收平均时长
        /// </summary>
        public decimal CheckDurationAverage
        {
            get { return this.GetProperty(CheckDurationAverageProperty); }
            set { this.SetProperty(CheckDurationAverageProperty, value); }
        }

    }
}