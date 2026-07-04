using SIE.Andon.Andons;
using SIE.Domain;
using SIE.ObjectModel;
using SIE.Resources.Employees;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.Andon.AndonAverageTime
{
    /// <summary>
    /// 安灯平均时长查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("安灯平均时长查询实体")]
    public class AndonAverageTimeViewModelCriteria : Criteria
    {

        /// <summary>
        /// 基于人员
        /// </summary>
        [Label("基于人员")]
        public static readonly IRefIdProperty ByUserIdProperty =
            P<AndonAverageTimeViewModelCriteria>.RegisterRefId(e => e.ByUserId, ReferenceType.Normal);

        /// <summary>
        /// 基于人员
        /// </summary>
        public double? ByUserId
        {
            get { return (double?)this.GetRefNullableId(ByUserIdProperty); }
            set { this.SetRefNullableId(ByUserIdProperty, value); }
        }

        /// <summary>
        /// 基于人员
        /// </summary>
        public static readonly RefEntityProperty<Employee> ByUserProperty =
            P<AndonAverageTimeViewModelCriteria>.RegisterRef(e => e.ByUser, ByUserIdProperty);

        /// <summary>
        /// 基于人员
        /// </summary>
        public Employee ByUser
        {
            get { return this.GetRefEntity(ByUserProperty); }
            set { this.SetRefEntity(ByUserProperty, value); }
        }



        /// <summary>
        /// 创建时间
        /// </summary>
        [Label("创建时间")]
        public static readonly Property<DateRange> CreateTimeProperty = P<AndonAverageTimeViewModelCriteria>.Register(e => e.CreateTime, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date, });

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateRange CreateTime
        {
            get { return GetProperty(CreateTimeProperty); }
            set { SetProperty(CreateTimeProperty, value); }
        }


        /// <summary>
        /// 查询
        /// </summary>
        /// <returns></returns>
        //protected override EntityList Fetch()
        //{
        //    return RT.Service.Resolve<AndonAverageTimeController>().GetAndonAverageTimes(this);
        //}



        /// <summary>
        /// 数据查询
        /// </summary>
        /// <returns></returns>
        protected override EntityList Fetch()
        {
            //return base.Fetch();
            return RT.Service.Resolve<AndonAverageTimeController>().GetAndonAverageTimes(this);
        }


    }
}