using DocumentFormat.OpenXml.Office2016.Drawing.ChartDrawing;
using SIE.Domain;

using SIE.MetaModel;
using SIE.ObjectModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SIE.MES.ListAtts
{
    /// <summary>
    /// 考勤数据实体
    /// </summary>
    [RootEntity, Serializable]
    [Label("考勤数据实体")]
    [ConditionQueryType(typeof(ListAttCriteria))]
    public class ListAtt:DataEntity
    {

        /// <summary>
        /// 对应的考勤数据的原始ID
        /// </summary>
        [Label("对应的考勤数据的原始ID")]
        [Required]
        [NotDuplicate]
        public static readonly Property<string> DataIdProperty = P<ListAtt>.Register(e => e.DataId);
        /// <summary>
        /// 对应的考勤数据的原始ID
        /// </summary>
        public string DataId
        {
            get { return this.GetProperty(DataIdProperty); }
            set { this.SetProperty(DataIdProperty, value); }
        }
        /// <summary>
        /// 记录设备触发时间
        /// </summary>
        [Label("记录设备触发时间")]
        public static readonly Property<DateTime?> EventTimeProperty = P<ListAtt>.Register(e => e.EventTime);
        /// <summary>
        /// 记录设备触发时间
        /// </summary>
        public DateTime? EventTime
        {
            get { return this.GetProperty(EventTimeProperty); }
            set { this.SetProperty(EventTimeProperty, value); }
        }
        /// <summary>
        /// 人员编号
        /// </summary>
        [Label("人员编号")]
        public static readonly Property<string> PinProperty = P<ListAtt>.Register(e => e.Pin);
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
        public static readonly Property<string> NameProperty = P<ListAtt>.Register(e => e.Name);
        /// <summary>
        /// 人员姓名
        /// </summary>
        public string Name
        {
            get { return this.GetProperty(NameProperty); }
            set { this.SetProperty(NameProperty, value); }
        }
        /// <summary>
        /// 人员姓名，英文下才有用
        /// </summary>
        [Label("人员姓名，英文下才有用")]
        public static readonly Property<string> LastNameProperty = P<ListAtt>.Register(e => e.LastName);
        /// <summary>
        /// 人员姓名，英文下才有用
        /// </summary>
        public string LastName
        {
            get { return this.GetProperty(LastNameProperty); }
            set { this.SetProperty(LastNameProperty, value); }
        }

        /// <summary>
        /// 部门名称
        /// </summary>
        [Label("部门名称")]
        public static readonly Property<string> DeptNameProperty = P<ListAtt>.Register(e => e.DeptName);
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
        public static readonly Property<string> AreaNameProperty = P<ListAtt>.Register(e => e.AreaName);
        /// <summary>
        /// 区域名称
        /// </summary>
        public string AreaName
        {
            get { return this.GetProperty(AreaNameProperty); }
            set { this.SetProperty(AreaNameProperty, value); }
        }

        /// <summary>
        /// 卡号
        /// </summary>
        [Label("卡号")]
        public static readonly Property<string> CardNoProperty = P<ListAtt>.Register(e => e.CardNo);
        /// <summary>
        /// 卡号
        /// </summary>
        public string CardNo
        {
            get { return this.GetProperty(CardNoProperty); }
            set { this.SetProperty(CardNoProperty, value); }
        }
        /// <summary>
        /// 设备序列号
        /// </summary>
        [Label("设备序列号")]
        public static readonly Property<string> DevSnProperty = P<ListAtt>.Register(e => e.DevSn);
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DevSn
        {
            get { return this.GetProperty(DevSnProperty); }
            set { this.SetProperty(DevSnProperty, value); }
        }

        /// <summary>
        /// 验证方式名称
        /// </summary>
        [Label("验证方式名称")]
        public static readonly Property<string> VerifyModeNameProperty = P<ListAtt>.Register(e => e.VerifyModeName);
        /// <summary>
        /// 验证方式名称
        /// </summary>
        public string VerifyModeName
        {
            get { return this.GetProperty(VerifyModeNameProperty); }
            set { this.SetProperty(VerifyModeNameProperty, value); }
        }

        /// <summary>
        /// 事件描述
        /// </summary>
        [Label("事件描述")]
        public static readonly Property<string> EventNameProperty = P<ListAtt>.Register(e => e.EventName);
        /// <summary>
        /// 事件描述
        /// </summary>
        public string EventName
        {
            get { return this.GetProperty(EventNameProperty); }
            set { this.SetProperty(EventNameProperty, value); }
        }
        /// <summary>
        /// 事件出发点名称
        /// </summary>
        [Label("事件出发点名称")]
        public static readonly Property<string> EventPointNameProperty = P<ListAtt>.Register(e => e.EventPointName);
        /// <summary>
        /// 事件出发点名称
        /// </summary>
        public string EventPointName
        {
            get { return this.GetProperty(EventPointNameProperty); }
            set { this.SetProperty(EventPointNameProperty, value); }
        }
        /// <summary>
        /// 读头名称
        /// </summary>
        [Label("读头名称")]
        public static readonly Property<string> ReaderNameProperty = P<ListAtt>.Register(e => e.ReaderName);
        /// <summary>
        /// 读头名称
        /// </summary>
        public string ReaderName
        {
            get { return this.GetProperty(ReaderNameProperty); }
            set { this.SetProperty(ReaderNameProperty, value); }
        }

        /// <summary>
        /// 门禁区域名称
        /// </summary>
        [Label("门禁区域名称")]
        public static readonly Property<string> AccZoneProperty = P<ListAtt>.Register(e => e.AccZone);
        /// <summary>
        /// 门禁区域名称
        /// </summary>
        public string AccZone
        {
            get { return this.GetProperty(AccZoneProperty); }
            set { this.SetProperty(AccZoneProperty, value); }
        }
        /// <summary>
        /// 设备名称
        /// </summary>
        [Label("设备名称")]
        public static readonly Property<string> DevNameProperty = P<ListAtt>.Register(e => e.DevName);
        /// <summary>
        /// 设备名称
        /// </summary>
        public string DevName
        {
            get { return this.GetProperty(DevNameProperty); }
            set { this.SetProperty(DevNameProperty, value); }
        }

        /// <summary>
        /// 事件索引值
        /// </summary>
        [Label("事件索引值")]
        public static readonly Property<string> LogIdProperty = P<ListAtt>.Register(e => e.LogId);
        /// <summary>
        /// 事件索引值
        /// </summary>
        public string LogId
        {
            get { return this.GetProperty(LogIdProperty); }
            set { this.SetProperty(LogIdProperty, value); }
        }
        /// <summary>
        /// 位置
        /// </summary>
        [Label("位置")]
        public static readonly Property<string> AttPlaceProperty = P<ListAtt>.Register(e => e.AttPlace);
        /// <summary>
        /// 位置
        /// </summary>
        public string AttPlace
        {
            get { return this.GetProperty(AttPlaceProperty); }
            set { this.SetProperty(AttPlaceProperty, value); }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [Label("备注")]
        public static readonly Property<string> MarkProperty = P<ListAtt>.Register(e => e.Mark);
        /// <summary>
        /// 备注
        /// </summary>
        public string Mark
        {
            get { return this.GetProperty(MarkProperty); }
            set { this.SetProperty(MarkProperty, value); }
        }


        internal class ListAttConfig : EntityConfig<ListAtt>
        {
            /// <summary>
            /// 配置数据库映射
            /// </summary>
            protected override void ConfigMeta()
            {
                Meta.MapTable("LIST_ATT").MapAllProperties();
                Meta.EnablePhantoms();
                Meta.DisableInvOrg();//不启用库存组织
            }

        }

        public static implicit operator ListAtt(ListAttInfo v)
        {
            throw new NotImplementedException();
        }
    }
}
