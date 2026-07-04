namespace SIE.Wpf.MES.LoadItems
{
    /// <summary>
    /// 上料接口
    /// </summary>
    public interface ILoadableItem
    {
        /// <summary>
        /// 刷新上料
        /// </summary>
        void RefreshLoadItem();

        /// <summary>
        /// 刷新挪料
        /// </summary>
        void RefreshMoveItem();

        /// <summary>
        /// 刷新下料
        /// </summary>
        void RefreshUnoadItem();
    }
}
