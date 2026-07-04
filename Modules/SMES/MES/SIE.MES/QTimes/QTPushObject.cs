using SIE.Domain;
using SIE.MES.QTimes.Enums;
using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.QTimes
{
    /// <summary>
    /// QT标准维护推送对象
    /// </summary>
    [ChildEntity, Serializable]
    [Label("QT标准维护推送对象")]
    public class QTPushObject : DataEntity
    {
        #region QT标准 QTStandard
        /// <summary>
        /// QT标准Id
        /// </summary>
        [Label("QT标准")]
        public static readonly IRefIdProperty QTStandardIdProperty =
            P<QTPushObject>.RegisterRefId(e => e.QTStandardId, ReferenceType.Parent);

        /// <summary>
        /// QT标准Id
        /// </summary>
        public double QTStandardId
        {
            get { return (double)this.GetRefId(QTStandardIdProperty); }
            set { this.SetRefId(QTStandardIdProperty, value); }
        }

        /// <summary>
        /// QT标准
        /// </summary>
        public static readonly RefEntityProperty<QTimeStandard> QTStandardProperty =
            P<QTPushObject>.RegisterRef(e => e.QTStandard, QTStandardIdProperty);

        /// <summary>
        /// QT标准
        /// </summary>
        public QTimeStandard QTStandard
        {
            get { return this.GetRefEntity(QTStandardProperty); }
            set { this.SetRefEntity(QTStandardProperty, value); }
        }
        #endregion

        #region 对象类型 ObjectType
        /// <summary>
        /// 对象类型
        /// </summary>
        [Label("对象类型")]
        public static readonly Property<QTPushType> ObjectTypeProperty = P<QTPushObject>.Register(e => e.ObjectType);

        /// <summary>
        /// 对象类型
        /// </summary>
        public QTPushType ObjectType
        {
            get { return this.GetProperty(ObjectTypeProperty); }
            set { this.SetProperty(ObjectTypeProperty, value); }
        }
        #endregion

        #region 对象Id ObjectId
        /// <summary>
        /// 对象Id
        /// </summary>
        [Label("对象Id")]
        public static readonly Property<double> ObjectIdProperty = P<QTPushObject>.Register(e => e.ObjectId);

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
        public static readonly Property<string> ObjectCodeProperty = P<QTPushObject>.Register(e => e.ObjectCode);

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
        public static readonly Property<string> ObjectNameProperty = P<QTPushObject>.Register(e => e.ObjectName);

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

    /// <summary>
    /// 推送对象配置
    /// </summary>
    public class QTPushObjectConfig : EntityConfig<QTPushObject>
    {
        /// <summary>
        /// 数据库配置
        /// </summary>
        protected override void ConfigMeta()
        {
            Meta.MapTable("QTPUSH_OBJECT").MapAllProperties();
            Meta.EnablePhantoms();
        }
    }
}
