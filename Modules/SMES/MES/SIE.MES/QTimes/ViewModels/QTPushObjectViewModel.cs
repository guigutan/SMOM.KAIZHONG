using SIE.Domain;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes.ViewModels
{
    /// <summary>
    /// 推送对象显示视图
    /// </summary>
    [RootEntity,Serializable]
    [Label("推送对象显示视图")]
    public class QTPushObjectViewModel : ViewModel
    {
        #region 对象Id ObjectId
        /// <summary>
        /// 对象Id
        /// </summary>
        [Label("对象Id")]
        public static readonly Property<double> ObjectIdProperty = P<QTPushObjectViewModel>.Register(e => e.ObjectId);

        /// <summary>
        /// 对象Id
        /// </summary>
        public double ObjectId
        {
            get { return this.GetProperty(ObjectIdProperty); }
            set { this.SetProperty(ObjectIdProperty, value); }
        }
        #endregion

        #region 对象编码 ObjectCode
        /// <summary>
        /// 对象编码
        /// </summary>
        [Label("对象编码")]
        public static readonly Property<string> ObjectCodeProperty = P<QTPushObjectViewModel>.Register(e => e.ObjectCode);

        /// <summary>
        /// 对象编码
        /// </summary>
        public string ObjectCode
        {
            get { return this.GetProperty(ObjectCodeProperty); }
            set { this.SetProperty(ObjectCodeProperty, value); }
        }
        #endregion

        #region 对象名称 ObjectName
        /// <summary>
        /// 对象名称
        /// </summary>
        [Label("对象名称")]
        public static readonly Property<string> ObjectNameProperty = P<QTPushObjectViewModel>.Register(e => e.ObjectName);

        /// <summary>
        /// 对象名称
        /// </summary>
        public string ObjectName
        {
            get { return this.GetProperty(ObjectNameProperty); }
            set { this.SetProperty(ObjectNameProperty, value); }
        }
        #endregion

    }
}
