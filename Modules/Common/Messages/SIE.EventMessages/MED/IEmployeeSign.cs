namespace SIE.EventMessages
{
    /// <summary>  
    /// 获取签名人图片
    /// </summary>
    [Services.Service(FallbackType = typeof(DefalitIReceiptInterface))]
    public interface IEmployeeSign
    {
        /// <summary>
        /// 获取签名人图片
        /// </summary>
        /// <param name="tableName">数据表</param>
        /// <param name="dataId">数据Id</param>
        /// <param name="empId">员工Id</param>
        /// <param name="empNos">工号</param>
        /// <returns></returns>
        byte[] GetUseEmployeeSign(string tableName, string dataId, string empId, string empNos);
    }

    /// <summary>
    /// 获取签名人图片
    /// </summary>
    class DefalitIEmployeeSignInterface : IEmployeeSign
    {
        public byte[] GetUseEmployeeSign(string tableName, string dataId, string empId, string empNos)
        {
            byte[] result = new byte[0];
            return result;
        }
    }
}
