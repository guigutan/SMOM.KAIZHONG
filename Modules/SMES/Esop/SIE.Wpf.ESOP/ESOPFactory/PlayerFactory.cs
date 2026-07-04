using SIE.ESop.Documents;
using SIE.Wpf.ESop.Editors;

namespace SIE.Wpf.ESOP.ESOPFactory
{
    /// <summary>
    /// 文档播放器工厂类
    /// </summary>
    public static class PlayerFactory
    {
        /// <summary>
        /// 创建文档播放器
        /// </summary>
        /// <param name="showControl"></param>
        /// <param name="documentType"></param>
        /// <param name="documentExtension"></param>
        /// <returns></returns>
        public static IPlayer CreatePlayer(FactoryShowControl showControl, DocumentType documentType, DocumentExtension documentExtension = DocumentExtension.None)
        {
            switch (documentType)
            {
                case DocumentType.Img:
                    return new ImagePlayer(showControl);
                case DocumentType.Video:
                    return new MediaPlayer(showControl);
                case DocumentType.Document:
                    if (documentExtension == DocumentExtension.xlsx)
                    {
                        return new ExcelPlayer(showControl);
                    }
                    if (documentExtension == DocumentExtension.pdf)
                    {
                        return new PdfPlayer(showControl);
                    }
                    if (documentExtension == DocumentExtension.docx)
                    {
                        return new WordPlayer(showControl);
                    }
                    break;
            }

            return null;
        }
    }
}
