namespace SIE.MES.CollectTest
{
    /// <summary>
    /// 执行模式
    /// </summary>
    public interface IRunMode
    {
        /// <summary>
        /// 帮助
        /// </summary>
        string Help { get; }
        /// <summary>
        /// 名称
        /// </summary>
        string Name { get; }
        /// <summary>
        /// 执行
        /// </summary>
        /// <param name="args"></param>
        void Run(string[] args);
    }
}
