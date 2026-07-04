using SIE.Tech.Processs;

namespace SIE.Tech.Routings.Technologys
{
    /// <summary>
	/// 规则接口
	/// </summary>
	public interface IRule : IChildElement
    {
        /// <summary>
        /// 工序参数Id
        /// </summary>
        double ParameterId { get; set; }

        /// <summary>
        /// 工序结果类型
        /// </summary>
        ResultTypeForDesign ParamResultType { get; set; }

        /// <summary>
        /// 脚本
        /// </summary>
        string Expression { get; set; }

        /// <summary>
        /// 开始位置
        /// </summary>
        Point StartPoint { get; set; }

        /// <summary>
        /// 位置1
        /// </summary>
        Point Point1 { get; set; }

        /// <summary>
        /// 位置2
        /// </summary>
        Point Point2 { get; set; }

        /// <summary>
        /// 结束位置
        /// </summary>
        Point EndPoint { get; set; }

        /// <summary>
        /// 结束活动
        /// </summary>
        IActivity EndActivity { get; set; }

        /// <summary>
        /// 开始活动
        /// </summary>
        IActivity BeginActivity { get; set; }

        /// <summary>
        /// 规则类型
        /// </summary>
        RuleType Type { get; set; }

        /// <summary>
        /// 规则起点距容器左侧长度
        /// </summary>
        double BeginLeft { get; set; }

        /// <summary>
        /// 规则起点距容器顶部长度
        /// </summary>
        double BeginTop { get; set; }

        /// <summary>
        /// 规则终点距容器左侧长度
        /// </summary>
        double EndLeft { get; set; }

        /// <summary>
        /// 规则终点距容器顶部长度
        /// </summary>
        double EndTop { get; set; }

        /// <summary>
        /// 规则名称距容器左侧长度
        /// </summary>
        double TextLeft { get; set; }

        /// <summary>
        /// 规则名称距容器顶部长度
        /// </summary>
        double TextTop { get; set; }

        /// <summary>
        /// 位置1距离容器左侧长度
        /// </summary>
        double Left1 { get; set; }

        /// <summary>
        /// 位置1距离容器顶部长度
        /// </summary>
        double Top1 { get; set; }

        /// <summary>
        /// 位置2距离容器左侧长度
        /// </summary>
        double Left2 { get; set; }

        /// <summary>
        /// 位置2距离容器顶部长度
        /// </summary>
        double Top2 { get; set; }

        /// <summary>
        /// 线条颜色
        /// </summary>
        string Color { get; set; }

        /// <summary>
        /// 源活动节点ID
        /// </summary>
        string SourceActivityId { get; set; }

        /// <summary>
        /// 设置开始活动节点
        /// </summary>
        /// <param name="activity">活动节点</param>
        void SetBeginActivity(IActivity activity);

        /// <summary>
        /// 设置结束活动节点
        /// </summary>
        /// <param name="activity">活动节点</param>
        void SetEndActivity(IActivity activity);

        /// <summary>
        /// 设置交叉点
        /// </summary>
        /// <param name="beginPoint">开始点</param>
        /// <param name="endPoint">结束点</param>
        /// <param name="type">规则移动类型</param>
        /// <param name="activity">活动节点</param>
        void SetPointOfIntersection(Point beginPoint, Point endPoint, RuleMoveType type, IActivity activity);

        /// <summary>
        /// 活动节点移动
        /// </summary>
        /// <param name="activity">活动节点</param>
        void OnActivityMove(IActivity activity);
    }
}
