using System;

namespace SIE.Barcodes
{
    /// <summary>
    /// 条码挂起
    /// 因为模块不支持两个相同的实体，所以此处进行继承
    /// 因为视图配置加载时会默认加载父类的视图配置方法 所以此实体的视图配置在BarcodeViewConfig文件中进行
    /// </summary>
    [RootEntity, Serializable]
    public class BarcodePending : Barcode
    {
    }
}