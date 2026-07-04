using SapNwRfc;

namespace SIE.ERPInterface.Sap.Datas
{
    /// <summary>
    /// SAP物料信息返回结果
    /// </summary>
    public class ItemResult
    {
        /// <summary>
        ///  物料信息集合
        /// </summary>
        [SapName("IT_LIST")]
        public ItemInfo[] ItemList { get; set; }
    }
}
