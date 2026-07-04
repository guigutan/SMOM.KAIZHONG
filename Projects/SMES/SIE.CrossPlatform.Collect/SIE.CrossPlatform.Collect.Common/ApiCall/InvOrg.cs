namespace SIE.CrossPlatform.Collect.Common.ApiCall
{
    public class InvOrg
    {
        /// <summary>
        /// 数据Id
        /// </summary>
        public double Id { get; set; }

        /// <summary>
        /// 库存组织编码
        /// </summary>
        public int Code { get; set; }

        /// <summary>
        /// 库存组织名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 外部Id
        /// </summary>
        public string ExternalId { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
