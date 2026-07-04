using SIE.Domain;
using SIE.ObjectModel;
using System;

namespace SIE.MES.TeamManagement.ScoreRecords
{
    /// <summary>
    /// 评分记录
    /// </summary>
    [RootEntity, Serializable]
    [Label("App图片")]
    public partial class ScoreAttachBase : DataEntity
    {
        #region 文件名 FileName
        /// <summary>
        /// 文件名
        /// </summary>
        [Label("文件名")]
        public static readonly Property<string> FileNameProperty = P<ScoreAttachBase>.Register(e => e.FileName);

        /// <summary>
        /// 文件名
        /// </summary>
        public string FileName
        {
            get { return this.GetProperty(FileNameProperty); }
            set { this.SetProperty(FileNameProperty, value); }
        }
        #endregion

        #region 图片 FileContent
        /// <summary>
        /// 图片
        /// </summary>
        [Label("图片")]
        public static readonly Property<byte[]> FileContentProperty = P<ScoreAttachBase>.Register(e => e.FileContent);

        /// <summary>
        /// 图片
        /// </summary>
        public byte[] FileContent
        {
            get { return this.GetProperty(FileContentProperty); }
            set { this.SetProperty(FileContentProperty, value); }
        }
        #endregion

        #region 文件扩展名 FileExtesion
        /// <summary>
        /// 文件扩展名
        /// </summary>
        [Label("文件扩展名")]
        public static readonly Property<string> FileExtesionProperty = P<ScoreAttachBase>.Register(e => e.FileExtesion);

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string FileExtesion
        {
            get { return this.GetProperty(FileExtesionProperty); }
            set { this.SetProperty(FileExtesionProperty, value); }
        }
        #endregion        
    }
}