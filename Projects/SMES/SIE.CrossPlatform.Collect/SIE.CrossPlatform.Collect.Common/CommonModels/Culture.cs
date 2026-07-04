namespace SIE.CrossPlatform.Collect.Common.CommonModels
{
    [Serializable]
    public class Culture
    {
        /// <summary>
        /// 数据ID
        /// </summary>
        public double Id { get; set; }
        /// <summary>
        /// 编码
        /// 只能使用文化代码，否则该语言将无法使用，详见：
        /// http://msdn.microsoft.com/en-us/goglobal/bb896001.aspx
        /// </summary>
        public string Code
        {
            get; set;
        }
        /// <summary>
        /// 名称
        /// </summary>
        public string Name
        {
            get; set;
        }

        /// <summary>
        /// Bing 翻译引擎对应的语言编码
        /// </summary>
        public string BingApiCode
        {
            get; set;
        }
        /// <summary>
        /// 百度翻译引擎对应的语言编码
        /// </summary>
        public string BaiduApiCode
        {
            get; set;
        }

        /// <summary>
        /// 版本号
        /// </summary>
        public long Version
        {
            get; set;
        }

    }
}
