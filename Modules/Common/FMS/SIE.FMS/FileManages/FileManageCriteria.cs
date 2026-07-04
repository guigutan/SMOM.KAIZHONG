using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.FMS
{
    /// <summary>
    /// 文件管理查询实体
    /// </summary>
    [QueryEntity, Serializable]
    [Label("文件管理查询实体")]
    public partial class FileManageCriteria : Criteria
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        public FileManageCriteria()
        {
            CreateDate = new DateRange();
            CreateDate.DateTimePart = DateTimePart.Date;
            CreateDate.DateRangeType = DateRangeType.All;
        }

        #region 文件编码 Code
        /// <summary>
        /// 文件编号
        /// </summary>
        [Label("文件编码")]
        public static readonly Property<string> CodeProperty = P<FileManageCriteria>.Register(e => e.Code);

        /// <summary>
        /// 文件编号
        /// </summary>
        public string Code
        {
            get { return this.GetProperty(CodeProperty); }
            set { this.SetProperty(CodeProperty, value); }
        }
        #endregion

        #region 关键字 KeyWord
        /// <summary>
        /// 关键字
        /// </summary>
        [Label("关键字")]
        public static readonly Property<string> KeyWordProperty = P<FileManageCriteria>.Register(e => e.KeyWord);

        /// <summary>
        /// 关键字
        /// </summary>
        public string KeyWord
        {
            get { return this.GetProperty(KeyWordProperty); }
            set { this.SetProperty(KeyWordProperty, value); }
        }
        #endregion

        #region 文件状态 FileState
        /// <summary>
        /// 文件状态
        /// </summary>
        [Label("文件状态")]
        public static readonly Property<FileState?> FileStateProperty = P<FileManageCriteria>.Register(e => e.FileState);

        /// <summary>
        /// 文件状态
        /// </summary>
        public FileState? FileState
        {
            get { return this.GetProperty(FileStateProperty); }
            set { this.SetProperty(FileStateProperty, value); }
        }
        #endregion

        #region 时间 CreateDate
        /// <summary>
        /// 时间
        /// </summary>
        [Label("时间")]
        public static readonly Property<DateRange> CreateDateProperty = P<FileManageCriteria>.Register(e => e.CreateDate, new PropertyMetadata<DateRange>() { DateTimePart = DateTimePart.Date });

        /// <summary>
        /// 时间
        /// </summary>
        public DateRange CreateDate
        {
            get { return GetProperty(CreateDateProperty); }
            set { SetProperty(CreateDateProperty, value); }
        }
        #endregion      
    }
}
