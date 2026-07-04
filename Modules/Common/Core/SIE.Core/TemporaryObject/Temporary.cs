namespace SIE.Core.TemporaryObject
{
    /// <summary>
    ///  查询数据库时临时对象（没实际意义 需要根据上下文解析）
    /// </summary>
    public class IdIntObject
    {
        /// <summary>
        /// 临时ID
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int? IntObj { get; set; }
    }

    /// <summary>
    ///  查询数据库时临时对象（没实际意义 需要根据上下文解析）
    /// </summary>
    public class TwoStringObject
    {
        /// <summary>
        /// 临时编号
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 临时名称
        /// </summary>
        public string Name { get; set; }
    }
}