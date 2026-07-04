using SIE.Common.Utils;
using SIE.Wpf.Common.Editors;
using SIE.Wpf.Windows;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace SIE.Wpf.ESop.Editors
{
    /// <summary>
    /// 文档选择编辑器
    /// </summary>
    public class DocumentSelectEditor : FileSelectEditor
    {
        /// <summary>
        /// 编辑器名称
        /// </summary>
        public const string EditorNameEX = "FileBrowserEditor";

        /// <summary>
        /// 文件大小
        /// </summary>
        public const string MaxSize = "MaxSize";

        /// <summary>
        /// 文件内容
        /// </summary>
        public const string FileContent = "Content";

        /// <summary>
        /// 创建编辑器样式
        /// </summary>
        /// <returns>FrameworkElement</returns>
        protected override FrameworkElement CreateEditingElement()
        {
            base.Config.ButtonImage = IconManager.GetPackIcon("Search", 16, 16);
            FrameworkElement controlElement = base.CreateEditingElement();
            if (((controlElement as DockPanel)?.Children?[1] as TextBox) != null)
            {
                ((controlElement as DockPanel).Children[1] as TextBox).TextWrapping = TextWrapping.NoWrap;
            }

            return controlElement;
        }

        /// <summary>
        /// 选择文件时触发
        /// </summary>
        /// <param name="fileName">选择的文件路径</param>
        /// <returns>返回是否可继续执行</returns>
        protected override bool FileSelecting(string fileName)
        {
            bool isValid = true;
            double maxSize = base.Config.GetPropertyOrDefault(MaxSize, 0d);
            if (maxSize > 0)
            {
                FileInfo info = new FileInfo(fileName);
                if ((long)(maxSize * 1024 * 1024) < info.Length)
                {
                    CRT.MessageService.ShowMessage("最大只能允许{0} Mb".L10nFormat(FileHelper.FormatFileSize((long)maxSize)));
                    isValid = false;
                }
            }

            return isValid;
        }

        /// <summary>
        /// 选择文件后触发
        /// </summary>
        /// <param name="fileName">选择的文件路径</param>
        protected override void FileSelected(string fileName)
        {
            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                BinaryReader r = new BinaryReader(stream);
                r.BaseStream.Seek(0, SeekOrigin.Begin);    //将文件指针设置到文件开
                Context.CurrentObject.LocalContext.SetExtendedProperty(FileContent, r.ReadBytes((int)r.BaseStream.Length));
                r.Close();
                stream.Close();
            }
        }
    }
}