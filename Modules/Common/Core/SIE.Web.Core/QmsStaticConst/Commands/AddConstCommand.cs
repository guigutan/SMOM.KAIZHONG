using DevExpress.Office.Utils;
using Newtonsoft.Json;
using SIE.Core.QmsStaticConst;
using SIE.Web.Command;
using System.Collections.Generic;

namespace SIE.Web.Core.QmsStaticConst.Commands
{
    /// <summary>
    ///
    /// </summary>
    public class AddConstCommand : ViewCommand<ViewArgs>
    {
        /// <summary>
        /// 执行方法
        /// </summary>
        /// <param name="args">视图参数</param>
        /// <param name="scope">SIE.MetaModel.View.Block.EntityType</param>
        /// <returns>object</returns>
        protected override object Excute(ViewArgs args, string scope)
        {
            var listControlChart = CreateControlChartConstList();
            var listD2 = CreateD2List();
            var listK1 = CreateK1List();
            var listK2 = CreateK2List();
            var listK3 = CreateK3List();
            var listT = CreateTList();

            return new
            {
                listControlChart = listControlChart,
                listD2 = listD2,
                listK1 = listK1,
                listK2 = listK2,
                listK3 = listK3,
                listT = listT
            };



        }
        private List<ControlChartConst> CreateControlChartConstList()
        {
            var result = new List<ControlChartConst>();
            result.Add(new ControlChartConst() { SampleQty = 2, A = 2.1210,  A2 = 1.88,  A3 = 2.659, B3 = 0,     B4 = 3.267, B5 = 0,      B6 = 2.6060, C4 = 0.7979, D1 = 0,      D2 = 3.6860,  D3 = 0,     D4 = 3.267, D2Nd = 1.128, D3Nd = 0.8525, D4Nd = 0.954,  E2 = 2.66,  MeA2 = 1.88 });
            result.Add(new ControlChartConst() { SampleQty = 3, A = 1.7320,  A2 = 1.023, A3 = 1.954, B3 = 0,     B4 = 2.568, B5 = 0,      B6 = 2.2760, C4 = 0.8862, D1 = 0,      D2 = 4.3580,  D3 = 0,     D4 = 2.574, D2Nd = 1.693, D3Nd = 0.8884, D4Nd = 1.588,  E2 = 1.772, MeA2 = 1.187 });
            result.Add(new ControlChartConst() { SampleQty = 4, A = 1.5000,  A2 = 0.729, A3 = 1.628, B3 = 0,     B4 = 2.266, B5 = 0,      B6 = 2.0880, C4 = 0.9213, D1 = 0,      D2 = 4.6980,  D3 = 0,     D4 = 2.282, D2Nd = 2.059, D3Nd = 0.8798, D4Nd = 1.978,  E2 = 1.457, MeA2 = 0.796 });
            result.Add(new ControlChartConst() { SampleQty = 5, A = 1.3420,  A2 = 0.577, A3 = 1.427, B3 = 0,     B4 = 2.089, B5 = 0,      B6 = 1.9640, C4 = 0.94,   D1 = 0,      D2 = 4.9180,  D3 = 0,     D4 = 2.114, D2Nd = 2.326, D3Nd = 0.8641, D4Nd = 2.257,  E2 = 1.29,  MeA2 = 0.691 });
            result.Add(new ControlChartConst() { SampleQty = 6, A = 1.2250,  A2 = 0.483, A3 = 1.287, B3 = 0.03,  B4 = 1.97,  B5 = 0.0290, B6 = 1.8740, C4 = 0.9515, D1 = 0,      D2 = 5.0790,  D3 = 0,     D4 = 2.004, D2Nd = 2.534, D3Nd = 0.8480, D4Nd = 2.472,  E2 = 1.184, MeA2 = 0.548 });
            result.Add(new ControlChartConst() { SampleQty = 7, A = 1.1340,  A2 = 0.419, A3 = 1.182, B3 = 0.118, B4 = 1.882, B5 = 0.1130, B6 = 1.8060, C4 = 0.9594, D1 = 0.2050, D2 = 5.2040,  D3 = 0.076, D4 = 1.924, D2Nd = 2.704, D3Nd = 0.8332, D4Nd = 2.645,  E2 = 1.109, MeA2 = 0.508 });
            result.Add(new ControlChartConst() { SampleQty = 8, A = 1.0610,  A2 = 0.373, A3 = 1.099, B3 = 0.185, B4 = 1.815, B5 = 0.1790, B6 = 1.7510, C4 = 0.965,  D1 = 0.3880, D2 = 05.3070, D3 = 0.136, D4 = 1.864, D2Nd = 2.847, D3Nd = 0.8198, D4Nd = 2.791,  E2 = 1.054, MeA2 = 0.432 });
            result.Add(new ControlChartConst() { SampleQty = 9, A = 1.0000,  A2 = 0.337, A3 = 1.032, B3 = 0.239, B4 = 1.761, B5 = 0.2320, B6 = 1.7070, C4 = 0.9693, D1 = 0.5470, D2 = 05.3940, D3 = 0.184, D4 = 1.816, D2Nd = 2.97,  D3Nd = 0.8078, D4Nd = 2.915,  E2 = 1.01,  MeA2 = 0.412 });
            result.Add(new ControlChartConst() { SampleQty = 10, A = 0.9490, A2 = 0.308, A3 = 0.975, B3 = 0.284, B4 = 1.716, B5 = 0.2760, B6 = 1.6690, C4 = 0.9727, D1 = 0.6860, D2 = 05.4690, D3 = 0.223, D4 = 1.777, D2Nd = 3.078, D3Nd = 0.7971, D4Nd = 3.121,  E2 = 0.975, MeA2 = 0.362 });
            result.Add(new ControlChartConst() { SampleQty = 11, A = 0.9050, A2 = 0.285, A3 = 0.927, B3 = 0.321, B4 = 1.679, B5 = 0.3130, B6 = 1.6370, C4 = 0.9754, D1 = 0.8110, D2 = 05.5350, D3 = 0.256, D4 = 1.744, D2Nd = 3.173, D3Nd = 0.7873, D4Nd = 3.2070, E2 = 0.945, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 12, A = 0.8660, A2 = 0.266, A3 = 0.886, B3 = 0.354, B4 = 1.646, B5 = 0.3460, B6 = 1.6100, C4 = 0.9776, D1 = 0.9230, D2 = 05.5940, D3 = 0.283, D4 = 1.717, D2Nd = 3.258, D3Nd = 0.7785, D4Nd = 3.2850, E2 = 0.921, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 13, A = 0.8320, A2 = 0.249, A3 = 0.85,  B3 = 0.382, B4 = 1.618, B5 = 0.3740, B6 = 1.5850, C4 = 0.9794, D1 = 1.0250, D2 = 5.6470,  D3 = 0.307, D4 = 1.693, D2Nd = 3.336, D3Nd = 0.7704, D4Nd = 3.3560, E2 = 0.899, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 14, A = 0.8020, A2 = 0.235, A3 = 0.817, B3 = 0.406, B4 = 1.594, B5 = 0.3990, B6 = 1.5630, C4 = 0.981,  D1 = 1.1180, D2 = 5.6960,  D3 = 0.328, D4 = 1.672, D2Nd = 3.407, D3Nd = 0.7630, D4Nd = 3.4220, E2 = 0.88,  MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 15, A = 0.7750, A2 = 0.223, A3 = 0.789, B3 = 0.428, B4 = 1.572, B5 = 0.42100,B6 = 1.5440, C4 = 0.9823, D1 = 1.2030, D2 = 5.7400,  D3 = 0.347, D4 = 1.653, D2Nd = 3.472, D3Nd = 0.7562, D4Nd = 3.4820, E2 = 0.864, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 16, A = 0.7500, A2 = 0.212, A3 = 0.763, B3 = 0.448, B4 = 1.552, B5 = 0.4400, B6 = 1.5260, C4 = 0.9835, D1 = 1.2820, D2 = 5.7820,  D3 = 0.363, D4 = 1.637, D2Nd = 3.532, D3Nd = 0.7499, D4Nd = 3.5380, E2 = 0.849, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 17, A = 0.7280, A2 = 0.203, A3 = 0.739, B3 = 0.466, B4 = 1.534, B5 = 0.4580, B6 = 1.5110, C4 = 0.9845, D1 = 1.3560, D2 = 5.8200,  D3 = 0.378, D4 = 1.622, D2Nd = 3.588, D3Nd = 0.7441, D4Nd = 3.5910, E2 = 0.836, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 18, A = 0.7070, A2 = 0.194, A3 = 0.718, B3 = 0.482, B4 = 1.518, B5 = 0.4750, B6 = 1.4960, C4 = 0.9854, D1 = 1.4240, D2 = 5.8560,  D3 = 0.391, D4 = 1.608, D2Nd = 3.64,  D3Nd = 0.7386, D4Nd = 3.6400, E2 = 0.824, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 19, A = 0.6880, A2 = 0.187, A3 = 0.698, B3 = 0.497, B4 = 1.503, B5 = 0.4900, B6 = 1.4830, C4 = 0.9862, D1 = 1.4890, D2 = 5.8890,  D3 = 0.403, D4 = 1.597, D2Nd = 3.689, D3Nd = 0.7335, D4Nd = 3.6860, E2 = 0.813, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 20, A = 0.6710, A2 = 0.18,  A3 = 0.68,  B3 = 0.51,  B4 = 1.49,  B5 = 0.5040, B6 = 1.4700, C4 = 0.9869, D1 = 1.5490, D2 = 5.9210,  D3 = 0.415, D4 = 1.585, D2Nd = 3.735, D3Nd = 0.7287, D4Nd = 3.7300, E2 = 0.803, MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 21, A = 0.6550, A2 = 0.173, A3 = 0.663, B3 = 0.523, B4 = 1.477, B5 = 0.5160, B6 = 1.4590, C4 = 0.9876, D1 = 1.6060, D2 = 5.9510,  D3 = 0.425, D4 = 1.575, D2Nd = 3.778, D3Nd = 0.7242, D4Nd = 3.7710, E2 = null,     MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 22, A = 0.6400, A2 = 0.167, A3 = 0.647, B3 = 0.534, B4 = 1.466, B5 = 0.5280, B6 = 1.4480, C4 = 0.9882, D1 = 1.6600, D2 = 5.9790,  D3 = 0.434, D4 = 1.566, D2Nd = 3.819, D3Nd = 0.7199, D4Nd = 3.8110, E2 = null,     MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 23, A = 0.6260, A2 = 0.162, A3 = 0.633, B3 = 0.545, B4 = 1.455, B5 = 0.5390, B6 = 1.4380, C4 = 0.9887, D1 = 1.7110, D2 = 6.0060,  D3 = 0.443, D4 = 1.557, D2Nd = 3.858, D3Nd = 0.7159, D4Nd = 3.8470, E2 = null,     MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 24, A = 0.6120, A2 = 0.157, A3 = 0.619, B3 = 0.555, B4 = 1.445, B5 = 0.5490, B6 = 1.4290, C4 = 0.9892, D1 = 1.7590, D2 = 6.0320,  D3 = 0.451, D4 = 1.548, D2Nd = 3.895, D3Nd = 0.7121, D4Nd = 3.8830, E2 = null,     MeA2 = null, });
            result.Add(new ControlChartConst() { SampleQty = 25, A = 0.6000, A2 = 0.153, A3 = 0.606, B3 = 0.565, B4 = 1.435, B5 = 0.5590, B6 = 1.4200, C4 = 0.9896, D1 = 1.8050, D2 = 6.0560,  D3 = 0.459, D4 = 1.541, D2Nd = 3.931, D3Nd = 0.7084, D4Nd = null,      E2 = null,     MeA2 = null, });


            return result;
        }

        private List<StaticConstD2> CreateD2List()
        {
            var result = new List<StaticConstD2>();

            #region D2
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 2, Value = 1.12838 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 3, Value = 1.69257 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 4, Value = 2.05875 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 5, Value = 2.32593 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 6, Value = 2.53441 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 7, Value = 2.70436 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 8, Value = 2.8472 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 9, Value = 2.97003 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 10, Value = 3.07751 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 11, Value = 3.17287 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 12, Value = 3.25846 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 13, Value = 3.33598 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 14, Value = 3.40676 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 15, Value = 3.47193 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 16, Value = 3.53198 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 17, Value = 3.58788 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 18, Value = 3.64006 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 19, Value = 3.68896 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2, SampleQty = 1, TestQty = 20, Value = 3.735 });
            #endregion

            #region Cd

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 2, Value = 0.876 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 3, Value = 1.815 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 4, Value = 2.7378 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 5, Value = 3.623 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 6, Value = 4.4658 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 7, Value = 5.2673 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 8, Value = 6.0305 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 9, Value = 6.7582 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 10, Value = 7.4539 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 11, Value = 8.1207 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 12, Value = 8.7602 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 13, Value = 9.3751 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 14, Value = 9.9679 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 15, Value = 10.5396 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 16, Value = 11.0913 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 17, Value = 11.6259 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 18, Value = 12.144 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 19, Value = 12.6468 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.Cd, SampleQty = 1, TestQty = 20, Value = 13.1362 });

            #endregion

            #region V
            // V类型数据 (MsaConstD2Type=2)
            // 样本数=1
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 2, Value = 1.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 3, Value = 2.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 4, Value = 2.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 5, Value = 3.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 6, Value = 4.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 7, Value = 5.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 8, Value = 6.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 9, Value = 7.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 10, Value = 7.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 11, Value = 8.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 12, Value = 9.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 13, Value = 9.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 14, Value = 10.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 15, Value = 10.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 16, Value = 11.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 17, Value = 11.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 18, Value = 12.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 19, Value = 12.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 1, TestQty = 20, Value = 13.4 });

            // 样本数=2
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 2, Value = 1.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 3, Value = 3.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 4, Value = 5.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 5, Value = 7.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 6, Value = 9.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 7, Value = 10.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 8, Value = 12.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 9, Value = 13.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 10, Value = 15.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 11, Value = 16.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 12, Value = 17.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 13, Value = 19.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 14, Value = 20.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 15, Value = 21.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 16, Value = 22.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 17, Value = 23.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 18, Value = 24.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 19, Value = 25.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 2, TestQty = 20, Value = 26.5 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 2, Value = 2.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 3, Value = 5.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 4, Value = 8.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 5, Value = 11.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 6, Value = 13.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 7, Value = 16.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 8, Value = 18.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 9, Value = 20.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 10, Value = 22.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 11, Value = 24.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 12, Value = 26.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 13, Value = 28.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 14, Value = 30.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 15, Value = 31.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 16, Value = 33.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 17, Value = 35.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 18, Value = 36.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 19, Value = 38.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 3, TestQty = 20, Value = 39.7 });


            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 2, Value = 3.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 3, Value = 7.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 4, Value = 11.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 5, Value = 14.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 6, Value = 18.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 7, Value = 21.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 8, Value = 24.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 9, Value = 27.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 10, Value = 30.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 11, Value = 32.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 12, Value = 35.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 13, Value = 37.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 14, Value = 40.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 15, Value = 42.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 16, Value = 44.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 17, Value = 46.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 18, Value = 48.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 19, Value = 50.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 4, TestQty = 20, Value = 52.8 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 2, Value = 4.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 3, Value = 9.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 4, Value = 13.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 5, Value = 18.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 6, Value = 22.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 7, Value = 26.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 8, Value = 30.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 9, Value = 34.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 10, Value = 37.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 11, Value = 40.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 12, Value = 44.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 13, Value = 47.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 14, Value = 50.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 15, Value = 52.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 16, Value = 55.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 17, Value = 58.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 18, Value = 61.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 19, Value = 63.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 5, TestQty = 20, Value = 65.9 });


            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 2, Value = 5.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 3, Value = 11.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 4, Value = 16.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 5, Value = 22.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 6, Value = 27.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 7, Value = 31.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 8, Value = 36.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 9, Value = 40.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 10, Value = 45.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 11, Value = 49.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 12, Value = 52.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 13, Value = 56.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 14, Value = 60.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 15, Value = 63.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 16, Value = 66.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 17, Value = 70.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 18, Value = 73.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 19, Value = 76.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 6, TestQty = 20, Value = 79.1 });


            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 2, Value = 6.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 3, Value = 12.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 4, Value = 19.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 5, Value = 25.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 6, Value = 31.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 7, Value = 37.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 8, Value = 42.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 9, Value = 47.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 10, Value = 52.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 11, Value = 57.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 12, Value = 61.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 13, Value = 65.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 14, Value = 70.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 15, Value = 74.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 16, Value = 77.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 17, Value = 81.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 18, Value = 85.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 19, Value = 88.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 7, TestQty = 20, Value = 92.2 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 2, Value = 7.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 3, Value = 14.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 4, Value = 22.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 5, Value = 29.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 6, Value = 36.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 7, Value = 42.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 8, Value = 48.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 9, Value = 54.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 10, Value = 59.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 11, Value = 65.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 12, Value = 70.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 13, Value = 75.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 14, Value = 80.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 15, Value = 84.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 16, Value = 89.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 17, Value = 93.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 18, Value = 97.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 19, Value = 101.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 8, TestQty = 20, Value = 105.3 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 2, Value = 8.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 3, Value = 16.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 4, Value = 24.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 5, Value = 32.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 6, Value = 40.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 7, Value = 47.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 8, Value = 54.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 9, Value = 61.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 10, Value = 67.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 11, Value = 73.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 12, Value = 79.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 13, Value = 84.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 14, Value = 90.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 15, Value = 95.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 16, Value = 100.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 17, Value = 104.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 18, Value = 109.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 19, Value = 114.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 9, TestQty = 20, Value = 118.5 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 2, Value = 9.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 3, Value = 18.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 4, Value = 27.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 5, Value = 36.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 6, Value = 44.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 7, Value = 52.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 8, Value = 60.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 9, Value = 67.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 10, Value = 74.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 11, Value = 81.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 12, Value = 87.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 13, Value = 94.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 14, Value = 99.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 15, Value = 105.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 16, Value = 111.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 17, Value = 116.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 18, Value = 121.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 19, Value = 126.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 10, TestQty = 20, Value = 131.6 });


            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 2, Value = 9.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 3, Value = 20.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 4, Value = 30.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 5, Value = 40.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 6, Value = 49.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 7, Value = 58.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 8, Value = 66.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 9, Value = 74.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 10, Value = 82.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 11, Value = 89.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 12, Value = 96.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 13, Value = 103.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 14, Value = 109.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 15, Value = 116.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 16, Value = 122.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 17, Value = 128.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 18, Value = 133.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 19, Value = 139.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 11, TestQty = 20, Value = 144.7 });


            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 2, Value = 10.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 3, Value = 22.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 4, Value = 33.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 5, Value = 43.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 6, Value = 53.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 7, Value = 63.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 8, Value = 72.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 9, Value = 81.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 10, Value = 89.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 11, Value = 97.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 12, Value = 105.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 13, Value = 112.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 14, Value = 119.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 15, Value = 126.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 16, Value = 133.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 17, Value = 139.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 18, Value = 146.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 19, Value = 152.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 12, TestQty = 20, Value = 157.9 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 2, Value = 11.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 3, Value = 23.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 4, Value = 35.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 5, Value = 47.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 6, Value = 58.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 7, Value = 68.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 8, Value = 78.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 9, Value = 88.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 10, Value = 97.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 11, Value = 105.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 12, Value = 114.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 13, Value = 122.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 14, Value = 129.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 15, Value = 137.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 16, Value = 144.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 17, Value = 151.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 18, Value = 158.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 19, Value = 164.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 13, TestQty = 20, Value = 171.0 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 2, Value = 12.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 3, Value = 25.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 4, Value = 38.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 5, Value = 51.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 6, Value = 62.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 7, Value = 74.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 8, Value = 84.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 9, Value = 94.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 10, Value = 104.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 11, Value = 113.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 12, Value = 122.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 13, Value = 131.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 14, Value = 139.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 15, Value = 147.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 16, Value = 155.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 17, Value = 163.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 18, Value = 170.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 19, Value = 177.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 14, TestQty = 20, Value = 184.2 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 2, Value = 13.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 3, Value = 27.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 4, Value = 41.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 5, Value = 54.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 6, Value = 67.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 7, Value = 79.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 8, Value = 90.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 9, Value = 101.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 10, Value = 112.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 11, Value = 122.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 12, Value = 131.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 13, Value = 140.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 14, Value = 149.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 15, Value = 158.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 16, Value = 166.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 17, Value = 174.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 18, Value = 182.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 19, Value = 190.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 15, TestQty = 20, Value = 197.3 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 2, Value = 14.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 3, Value = 29.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 4, Value = 44.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 5, Value = 58.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 6, Value = 71.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 7, Value = 84.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 8, Value = 96.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 9, Value = 108.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 10, Value = 119.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 11, Value = 130.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 12, Value = 140.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 13, Value = 150.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 14, Value = 159.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 15, Value = 168.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 16, Value = 177.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 17, Value = 186.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 18, Value = 194.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 19, Value = 202.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 16, TestQty = 20, Value = 210.4 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 2, Value = 15.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 3, Value = 31.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 4, Value = 46.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 5, Value = 61.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 6, Value = 76.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 7, Value = 89.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 8, Value = 102.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 9, Value = 115.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 10, Value = 127.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 11, Value = 138.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 12, Value = 149.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 13, Value = 159.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 14, Value = 169.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 15, Value = 179.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 16, Value = 188.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 17, Value = 197.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 18, Value = 206.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 19, Value = 215.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 17, TestQty = 20, Value = 223.6 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 2, Value = 16.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 3, Value = 32.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 4, Value = 49.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 5, Value = 65.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 6, Value = 80.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 7, Value = 95.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 8, Value = 108.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 9, Value = 121.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 10, Value = 134.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 11, Value = 146.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 12, Value = 157.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 13, Value = 169.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 14, Value = 179.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 15, Value = 190.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 16, Value = 199.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 17, Value = 209.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 18, Value = 218.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 19, Value = 227.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 18, TestQty = 20, Value = 236.7 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 2, Value = 16.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 3, Value = 34.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 4, Value = 52.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 5, Value = 69.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 6, Value = 85.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 7, Value = 100.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 8, Value = 114.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 9, Value = 128.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 10, Value = 141.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 11, Value = 154.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 12, Value = 166.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 13, Value = 178.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 14, Value = 189.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 15, Value = 200.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 16, Value = 211.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 17, Value = 221.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 18, Value = 231.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 19, Value = 240.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 19, TestQty = 20, Value = 249.8 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 2, Value = 17.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 3, Value = 36.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 4, Value = 55.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 5, Value = 72.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 6, Value = 89.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 7, Value = 105.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 8, Value = 120.9 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 9, Value = 135.4 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 10, Value = 149.3 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 11, Value = 162.7 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 12, Value = 175.5 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 13, Value = 187.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 14, Value = 199.6 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 15, Value = 211.0 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 16, Value = 222.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 17, Value = 232.8 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 18, Value = 243.1 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 19, Value = 253.2 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.V, SampleQty = 20, TestQty = 20, Value = 263.0 });

            #endregion

            #region D2s
            // D2s类型数据 (MsaConstD2Type=3)
            // 样本数=1
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 2, Value = 1.41421 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 3, Value = 1.91155 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 4, Value = 2.23887 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 5, Value = 2.48124 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 6, Value = 2.67253 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 7, Value = 2.82981 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 8, Value = 2.96288 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 9, Value = 3.07794 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 10, Value = 3.17905 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 11, Value = 3.26909 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 12, Value = 3.35016 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 13, Value = 3.42378 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 14, Value = 3.49116 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 15, Value = 3.55333 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 16, Value = 3.61071 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 17, Value = 3.66422 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 18, Value = 3.71424 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 19, Value = 3.76118 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 1, TestQty = 20, Value = 3.80537 });

            // 样本数=2
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 2, Value = 1.27931 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 3, Value = 1.80538 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 4, Value = 2.15069 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 5, Value = 2.40484 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 6, Value = 2.60438 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 7, Value = 2.76779 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 8, Value = 2.90562 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 9, Value = 3.02446 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 10, Value = 3.12869 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 11, Value = 3.22134 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 12, Value = 3.30463 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 13, Value = 3.38017 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 14, Value = 3.44922 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 15, Value = 3.51287 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 16, Value = 3.57156 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 17, Value = 3.62625 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 18, Value = 3.67734 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 19, Value = 3.72524 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 2, TestQty = 20, Value = 3.77032 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 2, Value = 1.23105 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 3, Value = 1.76858 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 4, Value = 2.12049 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 5, Value = 2.37883 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 6, Value = 2.58127 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 7, Value = 2.74681 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 8, Value = 2.88628 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 9, Value = 3.00643 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 10, Value = 3.11173 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 11, Value = 3.20526 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 12, Value = 3.28931 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 13, Value = 3.3655 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 14, Value = 3.43512 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 15, Value = 3.49927 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 16, Value = 3.55842 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 17, Value = 3.61351 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 18, Value = 3.66495 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 19, Value = 3.71319 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 3, TestQty = 20, Value = 3.75857 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 2, Value = 1.20621 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 3, Value = 1.74989 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 4, Value = 2.10522 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 5, Value = 2.36571 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 6, Value = 2.56964 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 7, Value = 2.73626 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 8, Value = 2.87656 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 9, Value = 2.99737 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 10, Value = 3.10321 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 11, Value = 3.1972 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 12, Value = 3.28163 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 13, Value = 3.35815 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 14, Value = 3.42805 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 15, Value = 3.49246 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 16, Value = 3.55183 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 17, Value = 3.60712 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 18, Value = 3.65875 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 19, Value = 3.70715 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 4, TestQty = 20, Value = 3.75268 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 2, Value = 1.19105 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 3, Value = 1.73857 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 4, Value = 2.09601 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 5, Value = 2.35781 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 6, Value = 2.56263 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 7, Value = 2.72991 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 8, Value = 2.87071 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 9, Value = 2.99192 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 10, Value = 3.09808 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 11, Value = 3.19235 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 12, Value = 3.27701 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 13, Value = 3.35372 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 14, Value = 3.42381 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 15, Value = 3.48836 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 16, Value = 3.54787 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 17, Value = 3.60328 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 18, Value = 3.65502 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 19, Value = 3.70352 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 5, TestQty = 20, Value = 3.74914 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 2, Value = 1.18083 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 3, Value = 1.73099 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 4, Value = 2.08985 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 5, Value = 2.35253 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 6, Value = 2.55795 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 7, Value = 2.72567 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 8, Value = 2.8668 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 9, Value = 2.98829 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 10, Value = 3.09467 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 11, Value = 3.18911 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 12, Value = 3.27392 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 13, Value = 3.35077 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 14, Value = 3.42097 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 15, Value = 3.48563 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 16, Value = 3.54522 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 17, Value = 3.60072 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 18, Value = 3.65253 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 19, Value = 3.70109 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 6, TestQty = 20, Value = 3.74678 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 2, Value = 1.17348 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 3, Value = 1.72555 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 4, Value = 2.08543 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 5, Value = 2.34875 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 6, Value = 2.5546 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 7, Value = 2.72263 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 8, Value = 2.86401 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 9, Value = 2.98568 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 10, Value = 3.09222 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 11, Value = 3.18679 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 12, Value = 3.27172 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 13, Value = 3.34866 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 14, Value = 3.41894 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 15, Value = 3.48368 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 16, Value = 3.54333 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 17, Value = 3.59888 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 18, Value = 3.65075 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 19, Value = 3.69936 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 7, TestQty = 20, Value = 3.74509 });


            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 2, Value = 1.16794 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 3, Value = 1.72147 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 4, Value = 2.08212 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 5, Value = 2.34591 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 6, Value = 2.55208 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 7, Value = 2.72036 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 8, Value = 2.86192 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 9, Value = 2.98373 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 10, Value = 3.09039 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 11, Value = 3.18506 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 12, Value = 3.27006 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 13, Value = 3.34708 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 14, Value = 3.41742 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 15, Value = 3.48221 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 16, Value = 3.54192 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 17, Value = 3.59751 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 18, Value = 3.64941 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 19, Value = 3.69806 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 8, TestQty = 20, Value = 3.74382 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 2, Value = 1.16361 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 3, Value = 1.71828 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 4, Value = 2.07953 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 5, Value = 2.3437 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 6, Value = 2.55013 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 7, Value = 2.71858 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 8, Value = 2.86028 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 9, Value = 2.98221 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 10, Value = 3.08896 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 11, Value = 3.1837 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 12, Value = 3.26878 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 13, Value = 3.34858 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 14, Value = 3.41624 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 15, Value = 3.48107 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 16, Value = 3.54081 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 17, Value = 3.59644 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 18, Value = 3.64838 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 19, Value = 3.69705 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 9, TestQty = 20, Value = 3.74284 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 2, Value = 1.16014 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 3, Value = 1.71573 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 4, Value = 2.07746 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 5, Value = 2.34192 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 6, Value = 2.54856 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 7, Value = 2.71717 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 8, Value = 2.85898 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 9, Value = 2.981 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 10, Value = 3.08781 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 11, Value = 3.18262 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 12, Value = 3.26775 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 13, Value = 3.34486 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 14, Value = 3.41529 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 15, Value = 3.48016 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 16, Value = 3.53993 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 17, Value = 3.59559 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 18, Value = 3.64755 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 19, Value = 3.69625 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 10, TestQty = 20, Value = 3.74205 });


            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 2, Value = 1.15729 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 3, Value = 1.71363 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 4, Value = 2.07577 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 5, Value = 2.34048 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 6, Value = 2.54728 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 7, Value = 2.716 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 8, Value = 2.85791 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 9, Value = 2.98 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 10, Value = 3.08688 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 11, Value = 3.18174 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 12, Value = 3.2669 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 13, Value = 3.34406 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 14, Value = 3.41452 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 15, Value = 3.47941 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 16, Value = 3.53921 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 17, Value = 3.59489 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 18, Value = 3.64687 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 19, Value = 3.69558 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 11, TestQty = 20, Value = 3.74141 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 2, Value = 1.1549 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 3, Value = 1.71189 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 4, Value = 2.07436 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 5, Value = 2.33927 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 6, Value = 2.54621 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 7, Value = 2.71504 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 8, Value = 2.85702 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 9, Value = 2.97917 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 10, Value = 3.0861 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 11, Value = 3.181 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 12, Value = 3.2662 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 13, Value = 3.34339 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 14, Value = 3.41387 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 15, Value = 3.47879 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 16, Value = 3.53861 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 17, Value = 3.5943 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 18, Value = 3.6463 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 19, Value = 3.69503 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 12, TestQty = 20, Value = 3.74087 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 2, Value = 1.15289 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 3, Value = 1.71041 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 4, Value = 2.07316 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 5, Value = 2.33824 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 6, Value = 2.54530 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 7, Value = 2.71422 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 8, Value = 2.85627 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 9, Value = 2.97847 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 10, Value = 3.08544 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 11, Value = 3.18037 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 12, Value = 3.26561 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 13, Value = 3.34282 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 14, Value = 3.41333 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 15, Value = 3.47826 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 16, Value = 3.53810 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 17, Value = 3.59381 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 18, Value = 3.64582 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 19, Value = 3.69457 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 13, TestQty = 20, Value = 3.74041 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 2, Value = 1.15115 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 3, Value = 1.70914 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 4, Value = 2.07213 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 5, Value = 2.33737 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 6, Value = 2.54452 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 7, Value = 2.71351 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 8, Value = 2.85562 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 9, Value = 2.97787 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 10, Value = 3.08487 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 11, Value = 3.17984 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 12, Value = 3.2651 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 13, Value = 3.34233 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 14, Value = 3.41286 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 15, Value = 3.47781 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 16, Value = 3.53766 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 17, Value = 3.59339 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 18, Value = 3.64541 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 19, Value = 3.69417 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 14, TestQty = 20, Value = 3.74002 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 2, Value = 1.14965 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 3, Value = 1.70804 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 4, Value = 2.07125 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 5, Value = 2.33661 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 6, Value = 2.54385 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 7, Value = 2.7129 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 8, Value = 2.85506 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 9, Value = 2.97735 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 10, Value = 3.08438 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 11, Value = 3.17938 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 12, Value = 3.26465 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 13, Value = 3.34191 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 14, Value = 3.41245 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 15, Value = 3.47742 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 16, Value = 3.53728 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 17, Value = 3.59302 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 18, Value = 3.64505 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 19, Value = 3.69382 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 15, TestQty = 20, Value = 3.73969 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 2, Value = 1.14833 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 3, Value = 1.70708 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 4, Value = 2.07047 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 5, Value = 2.33594 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 6, Value = 2.54326 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 7, Value = 2.71237 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 8, Value = 2.85457 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 9, Value = 2.97689 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 10, Value = 3.08395 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 11, Value = 3.17897 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 12, Value = 3.26427 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 13, Value = 3.34154 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 14, Value = 3.4121 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 15, Value = 3.47707 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 16, Value = 3.53695 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 17, Value = 3.5927 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 18, Value = 3.64474 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 19, Value = 3.69351 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 16, TestQty = 20, Value = 3.73939 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 2, Value = 1.14717 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 3, Value = 1.70623 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 4, Value = 2.06978 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 5, Value = 2.33535 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 6, Value = 2.54274 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 7, Value = 2.7119 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 8, Value = 2.85413 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 9, Value = 2.97649 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 10, Value = 3.08358 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 11, Value = 3.17861 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 12, Value = 3.26393 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 13, Value = 3.34121 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 14, Value = 3.41178 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 15, Value = 3.47677 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 16, Value = 3.53666 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 17, Value = 3.59242 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 18, Value = 3.64447 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 19, Value = 3.69325 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 17, TestQty = 20, Value = 3.73913 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 2, Value = 1.14613 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 3, Value = 1.70547 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 4, Value = 2.06917 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 5, Value = 2.33483 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 6, Value = 2.54228 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 7, Value = 2.71148 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 8, Value = 2.85375 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 9, Value = 2.97613 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 10, Value = 3.08324 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 11, Value = 3.17829 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 12, Value = 3.26362 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 13, Value = 3.34092 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 14, Value = 3.4115 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 15, Value = 3.4765 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 16, Value = 3.5364 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 17, Value = 3.59216 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 18, Value = 3.64422 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 19, Value = 3.69301 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 18, TestQty = 20, Value = 3.7389 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 2, Value = 1.1452 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 3, Value = 1.7048 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 4, Value = 2.06862 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 5, Value = 2.33436 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 6, Value = 2.54187 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 7, Value = 2.71111 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 8, Value = 2.85341 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 9, Value = 2.97581 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 10, Value = 3.08294 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 11, Value = 3.17801 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 12, Value = 3.26335 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 13, Value = 3.34066 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 14, Value = 3.41125 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 15, Value = 3.47626 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 16, Value = 3.53617 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 17, Value = 3.59194 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 18, Value = 3.644 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 19, Value = 3.6928 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 19, TestQty = 20, Value = 3.73869 });

            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 2, Value = 1.14437 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 3, Value = 1.70419 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 4, Value = 2.06813 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 5, Value = 2.33394 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 6, Value = 2.54149 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 7, Value = 2.71077 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 8, Value = 2.8531 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 9, Value = 2.97552 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 10, Value = 3.08267 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 11, Value = 3.17775 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 12, Value = 3.26311 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 13, Value = 3.34042 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 14, Value = 3.41103 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 15, Value = 3.47605 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 16, Value = 3.53596 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 17, Value = 3.59174 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 18, Value = 3.6438 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 19, Value = 3.6926 });
            result.Add(new StaticConstD2() { MsaConstD2Type = StaticConstD2Type.D2s, SampleQty = 20, TestQty = 20, Value = 3.7385 });

            #endregion

            return result;
        }

        private List<StaticConstK1> CreateK1List()
        {
            var result = new List<StaticConstK1>();
            result.Add(new StaticConstK1() { TestQty = 2, Value = 0.8862 });
            result.Add(new StaticConstK1() { TestQty = 3, Value = 0.5908 });
            return result;
        }

        private List<StaticConstK2> CreateK2List()
        {
            var result = new List<StaticConstK2>();
            result.Add(new StaticConstK2() { EvaluateQty = 2, Value = 0.7071 });
            result.Add(new StaticConstK2() { EvaluateQty = 3, Value = 0.5231 });
            return result;
        }

        private List<StaticConstK3> CreateK3List()
        {
            var result = new List<StaticConstK3>();
            result.Add(new StaticConstK3() { SampleQty = 10, Value = 0.2146 });
            result.Add(new StaticConstK3() { SampleQty = 9, Value = 0.3249 });
            result.Add(new StaticConstK3() { SampleQty = 8, Value = 0.3375 });
            result.Add(new StaticConstK3() { SampleQty = 7, Value = 0.3534 });
            result.Add(new StaticConstK3() { SampleQty = 6, Value = 0.3742 });
            result.Add(new StaticConstK3() { SampleQty = 5, Value = 0.403 });
            result.Add(new StaticConstK3() { SampleQty = 4, Value = 0.4467 });
            result.Add(new StaticConstK3() { SampleQty = 3, Value = 0.5231 });
            result.Add(new StaticConstK3() { SampleQty = 2, Value = 0.7071 });
            return result;
        }



        private List<StaticConstT> CreateTList()
        {
            var result = new List<StaticConstT>();

            #region SampleQty=1
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.25, Value = 1.0 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.2, Value = 1.37638 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.15, Value = 1.96261 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.1, Value = 3.07768 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.05, Value = 6.31375 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.025, Value = 12.7062 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.01, Value = 31.82052 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.005, Value = 63.65674 });
            result.Add(new StaticConstT() { SampleQty = 1, Alpha = 0.003, Value = 127.32134 });
            #endregion

            #region SampleQty=2
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.25, Value = 0.8165 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.2, Value = 1.06066 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.15, Value = 1.38621 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.1, Value = 1.88562 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.05, Value = 2.91999 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.025, Value = 4.30265 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.01, Value = 6.96456 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.005, Value = 9.92484 });
            result.Add(new StaticConstT() { SampleQty = 2, Alpha = 0.003, Value = 14.08905 });
            #endregion

            #region SampleQty=3
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.25, Value = 0.76489 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.2, Value = 0.97847 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.15, Value = 1.24978 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.1, Value = 1.63774 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.05, Value = 2.35336 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.025, Value = 3.18245 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.01, Value = 4.5407 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.005, Value = 5.84091 });
            result.Add(new StaticConstT() { SampleQty = 3, Alpha = 0.003, Value = 7.45332 });
            #endregion

            #region SampleQty=4
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.25, Value = 0.7407 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.2, Value = 0.94096 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.15, Value = 1.18957 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.1, Value = 1.53321 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.05, Value = 2.13185 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.025, Value = 2.77645 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.01, Value = 3.74695 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.005, Value = 4.60409 });
            result.Add(new StaticConstT() { SampleQty = 4, Alpha = 0.003, Value = 5.59757 });
            #endregion

            #region SampleQty=5
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.25, Value = 0.72669 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.2, Value = 0.91954 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.15, Value = 1.15577 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.1, Value = 1.47588 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.05, Value = 2.01505 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.025, Value = 2.57058 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.01, Value = 3.36493 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.005, Value = 4.03214 });
            result.Add(new StaticConstT() { SampleQty = 5, Alpha = 0.003, Value = 4.77334 });
            #endregion

            #region SampleQty=6
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.25, Value = 0.71756 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.2, Value = 0.9057 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.15, Value = 1.13416 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.1, Value = 1.43976 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.05, Value = 1.94318 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.025, Value = 2.44691 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.01, Value = 3.14267 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.005, Value = 3.70743 });
            result.Add(new StaticConstT() { SampleQty = 6, Alpha = 0.003, Value = 4.31683 });
            #endregion

            #region SampleQty=7
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.25, Value = 0.71114 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.2, Value = 0.89603 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.15, Value = 1.11916 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.1, Value = 1.41492 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.05, Value = 1.89458 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.025, Value = 2.36462 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.01, Value = 2.99795 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.005, Value = 3.49948 });
            result.Add(new StaticConstT() { SampleQty = 7, Alpha = 0.003, Value = 4.02934 });
            #endregion

            #region SampleQty=8
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.25, Value = 0.70639 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.2, Value = 0.88889 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.15, Value = 1.10815 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.1, Value = 1.39682 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.05, Value = 1.85955 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.025, Value = 2.306 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.01, Value = 2.89646 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.005, Value = 3.35539 });
            result.Add(new StaticConstT() { SampleQty = 8, Alpha = 0.003, Value = 3.83252 });
            #endregion

            #region SampleQty=9
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.25, Value = 0.70272 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.2, Value = 0.8834 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.15, Value = 1.09972 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.1, Value = 1.38303 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.05, Value = 1.83311 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.025, Value = 2.26216 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.01, Value = 2.82144 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.005, Value = 3.24984 });
            result.Add(new StaticConstT() { SampleQty = 9, Alpha = 0.003, Value = 3.68966 });
            #endregion

            #region SampleQty=10
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.25, Value = 0.69981 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.2, Value = 0.87906 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.15, Value = 1.09306 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.1, Value = 1.37218 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.05, Value = 1.81246 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.025, Value = 2.22814 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.01, Value = 2.76377 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.005, Value = 3.16927 });
            result.Add(new StaticConstT() { SampleQty = 10, Alpha = 0.003, Value = 3.58141 });
            #endregion

            #region SampleQty=11
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.25, Value = 0.69745 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.2, Value = 0.87553 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.15, Value = 1.08767 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.1, Value = 1.36343 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.05, Value = 1.79588 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.025, Value = 2.20099 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.01, Value = 2.71808 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.005, Value = 3.10581 });
            result.Add(new StaticConstT() { SampleQty = 11, Alpha = 0.003, Value = 3.49661 });
            #endregion

            #region SampleQty=12
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.25, Value = 0.69548 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.2, Value = 0.87261 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.15, Value = 1.08321 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.1, Value = 1.35622 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.05, Value = 1.78229 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.025, Value = 2.17881 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.01, Value = 2.681 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.005, Value = 3.05454 });
            result.Add(new StaticConstT() { SampleQty = 12, Alpha = 0.003, Value = 3.42844 });
            #endregion

            #region SampleQty=13
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.25, Value = 0.69383 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.2, Value = 0.87015 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.15, Value = 1.07947 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.1, Value = 1.35017 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.05, Value = 1.77093 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.025, Value = 2.16037 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.01, Value = 2.65031 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.005, Value = 3.01228 });
            result.Add(new StaticConstT() { SampleQty = 13, Alpha = 0.003, Value = 3.37247 });
            #endregion

            #region SampleQty=14
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.25, Value = 0.69242 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.2, Value = 0.86805 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.15, Value = 1.07628 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.1, Value = 1.34503 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.05, Value = 1.76131 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.025, Value = 2.14479 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.01, Value = 2.62449 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.005, Value = 2.97684 });
            result.Add(new StaticConstT() { SampleQty = 14, Alpha = 0.003, Value = 3.3257 });
            #endregion

            #region SampleQty=15
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.25, Value = 0.6912 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.2, Value = 0.86624 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.15, Value = 1.07353 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.1, Value = 1.34061 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.05, Value = 1.75305 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.025, Value = 2.13145 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.01, Value = 2.60248 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.005, Value = 2.94671 });
            result.Add(new StaticConstT() { SampleQty = 15, Alpha = 0.003, Value = 3.28604 });
            #endregion

            #region SampleQty=16
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.25, Value = 0.69013 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.2, Value = 0.86467 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.15, Value = 1.07114 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.1, Value = 1.33676 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.05, Value = 1.74588 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.025, Value = 2.11991 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.01, Value = 2.58349 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.005, Value = 2.92078 });
            result.Add(new StaticConstT() { SampleQty = 16, Alpha = 0.003, Value = 3.25199 });
            #endregion

            #region SampleQty=17
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.25, Value = 0.6892 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.2, Value = 0.86328 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.15, Value = 1.06903 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.1, Value = 1.33338 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.05, Value = 1.73961 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.025, Value = 2.10982 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.01, Value = 2.56693 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.005, Value = 2.89823 });
            result.Add(new StaticConstT() { SampleQty = 17, Alpha = 0.003, Value = 3.22245 });
            #endregion

            #region SampleQty=18
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.25, Value = 0.68836 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.2, Value = 0.86205 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.15, Value = 1.06717 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.1, Value = 1.33039 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.05, Value = 1.73406 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.025, Value = 2.10092 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.01, Value = 2.55238 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.005, Value = 2.87844 });
            result.Add(new StaticConstT() { SampleQty = 18, Alpha = 0.003, Value = 3.19657 });
            #endregion

            #region SampleQty=19
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.25, Value = 0.68762 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.2, Value = 0.86095 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.15, Value = 1.06551 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.1, Value = 1.32773 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.05, Value = 1.72913 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.025, Value = 2.09302 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.01, Value = 2.53948 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.005, Value = 2.86093 });
            result.Add(new StaticConstT() { SampleQty = 19, Alpha = 0.003, Value = 3.17372 });
            #endregion

            #region SampleQty=20
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.25, Value = 0.68695 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.2, Value = 0.85996 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.15, Value = 1.06402 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.1, Value = 1.32534 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.05, Value = 1.72472 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.025, Value = 2.08596 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.01, Value = 2.52798 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.005, Value = 2.84534 });
            result.Add(new StaticConstT() { SampleQty = 20, Alpha = 0.003, Value = 3.1534 });
            #endregion

            #region SampleQty=21
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.25, Value = 0.68635 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.2, Value = 0.85907 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.15, Value = 1.06267 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.1, Value = 1.32319 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.05, Value = 1.72074 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.025, Value = 2.07961 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.01, Value = 2.51765 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.005, Value = 2.83136 });
            result.Add(new StaticConstT() { SampleQty = 21, Alpha = 0.003, Value = 3.13521 });
            #endregion

            #region SampleQty=22
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.25, Value = 0.68581 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.2, Value = 0.85827 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.15, Value = 1.06145 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.1, Value = 1.32124 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.05, Value = 1.71714 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.025, Value = 2.07387 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.01, Value = 2.50832 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.005, Value = 2.81876 });
            result.Add(new StaticConstT() { SampleQty = 22, Alpha = 0.003, Value = 3.11882 });
            #endregion

            #region SampleQty=23
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.25, Value = 0.68531 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.2, Value = 0.85753 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.15, Value = 1.06034 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.1, Value = 1.31946 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.05, Value = 1.71387 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.025, Value = 2.06866 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.01, Value = 2.49987 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.005, Value = 2.80734 });
            result.Add(new StaticConstT() { SampleQty = 23, Alpha = 0.003, Value = 3.104 });
            #endregion

            #region SampleQty=24
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.25, Value = 0.68485 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.2, Value = 0.85686 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.15, Value = 1.05932 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.1, Value = 1.31784 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.05, Value = 1.71088 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.025, Value = 2.0639 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.01, Value = 2.49216 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.005, Value = 2.79694 });
            result.Add(new StaticConstT() { SampleQty = 24, Alpha = 0.003, Value = 3.09051 });
            #endregion

            #region SampleQty=25
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.25, Value = 0.68443 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.2, Value = 0.85624 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.15, Value = 1.05838 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.1, Value = 1.31635 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.05, Value = 1.70814 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.025, Value = 2.05954 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.01, Value = 2.48511 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.005, Value = 2.78744 });
            result.Add(new StaticConstT() { SampleQty = 25, Alpha = 0.003, Value = 3.0782 });
            #endregion

            #region SampleQty=26
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.25, Value = 0.68404 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.2, Value = 0.85567 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.15, Value = 1.05752 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.1, Value = 1.31497 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.05, Value = 1.70562 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.025, Value = 2.05553 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.01, Value = 2.47863 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.005, Value = 2.77871 });
            result.Add(new StaticConstT() { SampleQty = 26, Alpha = 0.003, Value = 3.06691 });
            #endregion

            #region SampleQty=27
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.25, Value = 0.68368 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.2, Value = 0.85514 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.15, Value = 1.05673 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.1, Value = 1.3137 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.05, Value = 1.70329 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.025, Value = 2.05183 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.01, Value = 2.47266 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.005, Value = 2.77068 });
            result.Add(new StaticConstT() { SampleQty = 27, Alpha = 0.003, Value = 3.05652 });
            #endregion

            #region SampleQty=28
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.25, Value = 0.68335 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.2, Value = 0.85465 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.15, Value = 1.05599 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.1, Value = 1.31253 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.05, Value = 1.70113 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.025, Value = 2.04841 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.01, Value = 2.46714 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.005, Value = 2.76326 });
            result.Add(new StaticConstT() { SampleQty = 28, Alpha = 0.003, Value = 3.04693 });
            #endregion

            #region SampleQty=29
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.25, Value = 0.68304 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.2, Value = 0.85419 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.15, Value = 1.0553 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.1, Value = 1.31143 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.05, Value = 1.69913 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.025, Value = 2.04523 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.01, Value = 2.46202 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.005, Value = 2.75639 });
            result.Add(new StaticConstT() { SampleQty = 29, Alpha = 0.003, Value = 3.03805 });
            #endregion

            #region SampleQty=30
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.25, Value = 0.68276 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.2, Value = 0.85377 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.15, Value = 1.05466 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.1, Value = 1.31042 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.05, Value = 1.69726 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.025, Value = 2.04227 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.01, Value = 2.45726 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.005, Value = 2.75 });
            result.Add(new StaticConstT() { SampleQty = 30, Alpha = 0.003, Value = 3.0298 });
            #endregion

            #region SampleQty=31
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.25, Value = 0.68249 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.2, Value = 0.85337 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.15, Value = 1.05406 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.1, Value = 1.30946 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.05, Value = 1.69552 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.025, Value = 2.03951 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.01, Value = 2.45282 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.005, Value = 2.74404 });
            result.Add(new StaticConstT() { SampleQty = 31, Alpha = 0.003, Value = 3.02212 });
            #endregion

            #region SampleQty=32
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.25, Value = 0.68223 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.2, Value = 0.853 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.15, Value = 1.0535 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.1, Value = 1.30857 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.05, Value = 1.69389 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.025, Value = 2.03693 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.01, Value = 2.44868 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.005, Value = 2.73848 });
            result.Add(new StaticConstT() { SampleQty = 32, Alpha = 0.003, Value = 3.01495 });
            #endregion

            #region SampleQty=33
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.25, Value = 0.682 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.2, Value = 0.85265 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.15, Value = 1.05298 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.1, Value = 1.30774 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.05, Value = 1.69236 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.025, Value = 2.03452 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.01, Value = 2.44479 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.005, Value = 2.73328 });
            result.Add(new StaticConstT() { SampleQty = 33, Alpha = 0.003, Value = 3.00824 });
            #endregion

            #region SampleQty=34
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.25, Value = 0.68177 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.2, Value = 0.85232 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.15, Value = 1.05248 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.1, Value = 1.30695 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.05, Value = 1.69092 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.025, Value = 2.03224 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.01, Value = 2.44115 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.005, Value = 2.72839 });
            result.Add(new StaticConstT() { SampleQty = 34, Alpha = 0.003, Value = 3.00195 });
            #endregion

            #region SampleQty=35
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.25, Value = 0.68156 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.2, Value = 0.85201 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.15, Value = 1.05202 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.1, Value = 1.30621 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.05, Value = 1.68957 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.025, Value = 2.03011 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.01, Value = 2.43772 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.005, Value = 2.72381 });
            result.Add(new StaticConstT() { SampleQty = 35, Alpha = 0.003, Value = 2.99605 });
            #endregion

            #region SampleQty=36
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.25, Value = 0.68137 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.2, Value = 0.85172 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.15, Value = 1.05158 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.1, Value = 1.30551 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.05, Value = 1.6883 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.025, Value = 2.02809 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.01, Value = 2.43449 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.005, Value = 2.71948 });
            result.Add(new StaticConstT() { SampleQty = 36, Alpha = 0.003, Value = 2.99049 });
            #endregion

            #region SampleQty=37
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.25, Value = 0.68118 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.2, Value = 0.85144 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.15, Value = 1.05117 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.1, Value = 1.30485 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.05, Value = 1.68709 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.025, Value = 2.02619 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.01, Value = 2.43145 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.005, Value = 2.71541 });
            result.Add(new StaticConstT() { SampleQty = 37, Alpha = 0.003, Value = 2.98524 });
            #endregion

            #region SampleQty=38
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.25, Value = 0.681 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.2, Value = 0.85118 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.15, Value = 1.05077 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.1, Value = 1.30423 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.05, Value = 1.68595 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.025, Value = 2.02439 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.01, Value = 2.42857 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.005, Value = 2.71156 });
            result.Add(new StaticConstT() { SampleQty = 38, Alpha = 0.003, Value = 2.98029 });
            #endregion

            #region SampleQty=39
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.25, Value = 0.68083 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.2, Value = 0.85094 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.15, Value = 1.0504 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.1, Value = 1.30364 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.05, Value = 1.68488 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.025, Value = 2.02269 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.01, Value = 2.42584 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.005, Value = 2.70791 });
            result.Add(new StaticConstT() { SampleQty = 39, Alpha = 0.003, Value = 2.97561 });
            #endregion

            #region SampleQty=40
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.25, Value = 0.68067 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.2, Value = 0.8507 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.15, Value = 1.05005 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.1, Value = 1.30308 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.05, Value = 1.68385 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.025, Value = 2.02108 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.01, Value = 2.42326 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.005, Value = 2.70446 });
            result.Add(new StaticConstT() { SampleQty = 40, Alpha = 0.003, Value = 2.97117 });
            #endregion

            #region SampleQty=41
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.25, Value = 0.68052 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.2, Value = 0.85048 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.15, Value = 1.04971 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.1, Value = 1.30254 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.05, Value = 1.68288 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.025, Value = 2.01954 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.01, Value = 2.4208 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.005, Value = 2.70118 });
            result.Add(new StaticConstT() { SampleQty = 41, Alpha = 0.003, Value = 2.96696 });
            #endregion

            #region SampleQty=42
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.25, Value = 0.68038 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.2, Value = 0.85026 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.15, Value = 1.04939 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.1, Value = 1.30204 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.05, Value = 1.68195 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.025, Value = 2.01808 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.01, Value = 2.41847 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.005, Value = 2.69807 });
            result.Add(new StaticConstT() { SampleQty = 42, Alpha = 0.003, Value = 2.96296 });
            #endregion

            #region SampleQty=43
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.25, Value = 0.68024 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.2, Value = 0.85006 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.15, Value = 1.04908 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.1, Value = 1.30155 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.05, Value = 1.68107 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.025, Value = 2.01669 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.01, Value = 2.41625 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.005, Value = 2.6951 });
            result.Add(new StaticConstT() { SampleQty = 43, Alpha = 0.003, Value = 2.95916 });
            #endregion

            #region SampleQty=44
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.25, Value = 0.68011 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.2, Value = 0.84987 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.15, Value = 1.04879 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.1, Value = 1.30109 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.05, Value = 1.68023 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.025, Value = 2.01537 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.01, Value = 2.41413 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.005, Value = 2.69228 });
            result.Add(new StaticConstT() { SampleQty = 44, Alpha = 0.003, Value = 2.95553 });
            #endregion

            #region SampleQty=45
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.25, Value = 0.67998 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.2, Value = 0.84968 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.15, Value = 1.04852 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.1, Value = 1.30065 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.05, Value = 1.67943 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.025, Value = 2.0141 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.01, Value = 2.41212 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.005, Value = 2.68959 });
            result.Add(new StaticConstT() { SampleQty = 45, Alpha = 0.003, Value = 2.95208 });
            #endregion

            #region SampleQty=46
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.25, Value = 0.67986 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.2, Value = 0.84951 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.15, Value = 1.04825 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.1, Value = 1.30023 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.05, Value = 1.67866 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.025, Value = 2.0129 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.01, Value = 2.41019 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.005, Value = 2.68701 });
            result.Add(new StaticConstT() { SampleQty = 46, Alpha = 0.003, Value = 2.94878 });
            #endregion

            #region SampleQty=47
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.25, Value = 0.67975 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.2, Value = 0.84934 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.15, Value = 1.048 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.1, Value = 1.29982 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.05, Value = 1.67793 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.025, Value = 2.01174 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.01, Value = 2.40835 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.005, Value = 2.68456 });
            result.Add(new StaticConstT() { SampleQty = 47, Alpha = 0.003, Value = 2.94563 });
            #endregion

            #region SampleQty=48
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.25, Value = 0.67964 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.2, Value = 0.84917 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.15, Value = 1.04775 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.1, Value = 1.29944 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.05, Value = 1.67722 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.025, Value = 2.01063 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.01, Value = 2.40658 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.005, Value = 2.6822 });
            result.Add(new StaticConstT() { SampleQty = 48, Alpha = 0.003, Value = 2.94262 });
            #endregion

            #region SampleQty=49
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.25, Value = 0.67953 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.2, Value = 0.84902 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.15, Value = 1.04752 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.1, Value = 1.29907 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.05, Value = 1.67655 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.025, Value = 2.00958 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.01, Value = 2.40489 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.005, Value = 2.67995 });
            result.Add(new StaticConstT() { SampleQty = 49, Alpha = 0.003, Value = 2.93973 });
            #endregion

            #region SampleQty=50
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.25, Value = 0.67943 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.2, Value = 0.84887 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.15, Value = 1.04729 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.1, Value = 1.29871 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.05, Value = 1.67591 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.025, Value = 2.00856 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.01, Value = 2.40327 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.005, Value = 2.67779 });
            result.Add(new StaticConstT() { SampleQty = 50, Alpha = 0.003, Value = 2.93696 });
            #endregion

            #region SampleQty=51
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.25, Value = 0.67933 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.2, Value = 0.84873 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.15, Value = 1.04708 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.1, Value = 1.29837 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.05, Value = 1.67528 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.025, Value = 2.00758 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.01, Value = 2.40172 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.005, Value = 2.67572 });
            result.Add(new StaticConstT() { SampleQty = 51, Alpha = 0.003, Value = 2.93431 });
            #endregion

            #region SampleQty=52
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.25, Value = 0.67924 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.2, Value = 0.84859 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.15, Value = 1.04687 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.1, Value = 1.29805 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.05, Value = 1.67469 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.025, Value = 2.00665 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.01, Value = 2.40022 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.005, Value = 2.67373 });
            result.Add(new StaticConstT() { SampleQty = 52, Alpha = 0.003, Value = 2.93176 });
            #endregion

            #region SampleQty=53
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.25, Value = 0.67915 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.2, Value = 0.84846 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.15, Value = 1.04667 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.1, Value = 1.29773 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.05, Value = 1.67412 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.025, Value = 2.00575 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.01, Value = 2.39879 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.005, Value = 2.67182 });
            result.Add(new StaticConstT() { SampleQty = 53, Alpha = 0.003, Value = 2.92932 });
            #endregion

            #region SampleQty=54
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.25, Value = 0.67906 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.2, Value = 0.84833 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.15, Value = 1.04648 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.1, Value = 1.29743 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.05, Value = 1.67356 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.025, Value = 2.00488 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.01, Value = 2.39741 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.005, Value = 2.66998 });
            result.Add(new StaticConstT() { SampleQty = 54, Alpha = 0.003, Value = 2.92696 });
            #endregion

            #region SampleQty=55
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.25, Value = 0.67898 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.2, Value = 0.84821 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.15, Value = 1.0463 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.1, Value = 1.29713 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.05, Value = 1.67303 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.025, Value = 2.00404 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.01, Value = 2.39608 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.005, Value = 2.66822 });
            result.Add(new StaticConstT() { SampleQty = 55, Alpha = 0.003, Value = 2.9247 });
            #endregion

            #region SampleQty=56
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.25, Value = 0.6789 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.2, Value = 0.84809 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.15, Value = 1.04612 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.1, Value = 1.29685 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.05, Value = 1.67252 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.025, Value = 2.00324 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.01, Value = 2.3948 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.005, Value = 2.66651 });
            result.Add(new StaticConstT() { SampleQty = 56, Alpha = 0.003, Value = 2.92252 });
            #endregion

            #region SampleQty=57
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.25, Value = 0.67882 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.2, Value = 0.84797 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.15, Value = 1.04595 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.1, Value = 1.29658 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.05, Value = 1.67203 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.025, Value = 2.00247 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.01, Value = 2.39357 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.005, Value = 2.66487 });
            result.Add(new StaticConstT() { SampleQty = 57, Alpha = 0.003, Value = 2.92042 });
            #endregion

            #region SampleQty=58
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.25, Value = 0.67874 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.2, Value = 0.84786 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.15, Value = 1.04578 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.1, Value = 1.29632 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.05, Value = 1.67155 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.025, Value = 2.00172 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.01, Value = 2.39238 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.005, Value = 2.66329 });
            result.Add(new StaticConstT() { SampleQty = 58, Alpha = 0.003, Value = 2.91839 });
            #endregion

            #region SampleQty=59
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.25, Value = 0.67867 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.2, Value = 0.84776 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.15, Value = 1.04562 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.1, Value = 1.29607 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.05, Value = 1.67109 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.025, Value = 2.001 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.01, Value = 2.39123 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.005, Value = 2.66176 });
            result.Add(new StaticConstT() { SampleQty = 59, Alpha = 0.003, Value = 2.91644 });
            #endregion

            #region SampleQty=60
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.25, Value = 0.6786 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.2, Value = 0.84765 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.15, Value = 1.04547 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.1, Value = 1.29582 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.05, Value = 1.67065 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.025, Value = 2.0003 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.01, Value = 2.39012 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.005, Value = 2.66028 });
            result.Add(new StaticConstT() { SampleQty = 60, Alpha = 0.003, Value = 2.91455 });
            #endregion

            #region SampleQty=61
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.25, Value = 0.67853 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.2, Value = 0.84755 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.15, Value = 1.04532 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.1, Value = 1.29558 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.05, Value = 1.67022 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.025, Value = 1.99962 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.01, Value = 2.38905 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.005, Value = 2.65886 });
            result.Add(new StaticConstT() { SampleQty = 61, Alpha = 0.003, Value = 2.91273 });
            #endregion

            #region SampleQty=62
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.25, Value = 0.67847 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.2, Value = 0.84746 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.15, Value = 1.04518 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.1, Value = 1.29536 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.05, Value = 1.66980 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.025, Value = 1.99897 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.01, Value = 2.38801 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.005, Value = 2.65748 });
            result.Add(new StaticConstT() { SampleQty = 62, Alpha = 0.003, Value = 2.91097 });
            #endregion

            #region SampleQty=63
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.25, Value = 0.67840 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.2, Value = 0.84736 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.15, Value = 1.04504 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.1, Value = 1.29513 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.05, Value = 1.66940 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.025, Value = 1.99834 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.01, Value = 2.38701 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.005, Value = 2.65615 });
            result.Add(new StaticConstT() { SampleQty = 63, Alpha = 0.003, Value = 2.90926 });
            #endregion

            #region SampleQty=64
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.25, Value = 0.67834 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.2, Value = 0.84727 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.15, Value = 1.04490 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.1, Value = 1.29492 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.05, Value = 1.66901 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.025, Value = 1.99773 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.01, Value = 2.38604 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.005, Value = 2.65485 });
            result.Add(new StaticConstT() { SampleQty = 64, Alpha = 0.003, Value = 2.90761 });
            #endregion

            #region SampleQty=65
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.25, Value = 0.67828 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.2, Value = 0.84719 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.15, Value = 1.04477 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.1, Value = 1.29471 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.05, Value = 1.66864 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.025, Value = 1.99714 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.01, Value = 2.38510 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.005, Value = 2.65360 });
            result.Add(new StaticConstT() { SampleQty = 65, Alpha = 0.003, Value = 2.90602 });
            #endregion

            #region SampleQty=66
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.25, Value = 0.67823 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.2, Value = 0.84710 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.15, Value = 1.04464 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.1, Value = 1.29451 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.05, Value = 1.66827 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.025, Value = 1.99656 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.01, Value = 2.38419 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.005, Value = 2.65239 });
            result.Add(new StaticConstT() { SampleQty = 66, Alpha = 0.003, Value = 2.90447 });
            #endregion

            #region SampleQty=67
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.25, Value = 0.67817 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.2, Value = 0.84702 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.15, Value = 1.04452 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.1, Value = 1.29432 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.05, Value = 1.66792 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.025, Value = 1.99601 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.01, Value = 2.38330 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.005, Value = 2.65122 });
            result.Add(new StaticConstT() { SampleQty = 67, Alpha = 0.003, Value = 2.90297 });
            #endregion

            #region SampleQty=68
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.25, Value = 0.67811 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.2, Value = 0.84694 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.15, Value = 1.04440 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.1, Value = 1.29413 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.05, Value = 1.66757 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.025, Value = 1.99547 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.01, Value = 2.38245 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.005, Value = 2.65008 });
            result.Add(new StaticConstT() { SampleQty = 68, Alpha = 0.003, Value = 2.90151 });
            #endregion

            #region SampleQty=69
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.25, Value = 0.67806 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.2, Value = 0.84686 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.15, Value = 1.04428 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.1, Value = 1.29394 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.05, Value = 1.66724 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.025, Value = 1.99495 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.01, Value = 2.38161 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.005, Value = 2.64898 });
            result.Add(new StaticConstT() { SampleQty = 69, Alpha = 0.003, Value = 2.90010 });
            #endregion

            #region SampleQty=70
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.25, Value = 0.67801 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.2, Value = 0.84679 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.15, Value = 1.04417 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.1, Value = 1.29376 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.05, Value = 1.66691 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.025, Value = 1.99444 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.01, Value = 2.38081 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.005, Value = 2.64790 });
            result.Add(new StaticConstT() { SampleQty = 70, Alpha = 0.003, Value = 2.89873 });
            #endregion

            #region SampleQty 71
            result.Add(new StaticConstT() { Alpha = 0.25, SampleQty = 71, Value = 0.67796 });
            result.Add(new StaticConstT() { Alpha = 0.2, SampleQty = 71, Value = 0.84671 });
            result.Add(new StaticConstT() { Alpha = 0.15, SampleQty = 71, Value = 1.04406 });
            result.Add(new StaticConstT() { Alpha = 0.1, SampleQty = 71, Value = 1.29359 });
            result.Add(new StaticConstT() { Alpha = 0.05, SampleQty = 71, Value = 1.6666 });
            result.Add(new StaticConstT() { Alpha = 0.025, SampleQty = 71, Value = 1.99394 });
            result.Add(new StaticConstT() { Alpha = 0.01, SampleQty = 71, Value = 2.38002 });
            result.Add(new StaticConstT() { Alpha = 0.005, SampleQty = 71, Value = 2.64686 });
            result.Add(new StaticConstT() { Alpha = 0.003, SampleQty = 71, Value = 2.8974 });
            #endregion

            #region SampleQty 72
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.25, Value = 0.67791 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.2, Value = 0.84664 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.15, Value = 1.04395 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.1, Value = 1.29342 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.05, Value = 1.66629 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.025, Value = 1.99346 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.01, Value = 2.37926 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.005, Value = 2.64585 });
            result.Add(new StaticConstT() { SampleQty = 72, Alpha = 0.003, Value = 2.89611 });
            #endregion

            #region SampleQty 73
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.25, Value = 0.67787 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.2, Value = 0.84657 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.15, Value = 1.04385 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.1, Value = 1.29326 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.05, Value = 1.666 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.025, Value = 1.993 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.01, Value = 2.37852 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.005, Value = 2.64487 });
            result.Add(new StaticConstT() { SampleQty = 73, Alpha = 0.003, Value = 2.89486 });
            #endregion

            #region SampleQty 74
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.25, Value = 0.67782 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.2, Value = 0.84651 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.15, Value = 1.04375 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.1, Value = 1.2931 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.05, Value = 1.66571 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.025, Value = 1.99254 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.01, Value = 2.3778 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.005, Value = 2.64391 });
            result.Add(new StaticConstT() { SampleQty = 74, Alpha = 0.003, Value = 2.89364 });
            #endregion

            #region SampleQty 75
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.25, Value = 0.67778 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.2, Value = 0.84644 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.15, Value = 1.04365 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.1, Value = 1.29294 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.05, Value = 1.66543 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.025, Value = 1.9921 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.01, Value = 2.3771 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.005, Value = 2.64298 });
            result.Add(new StaticConstT() { SampleQty = 75, Alpha = 0.003, Value = 2.89245 });
            #endregion

            #region SampleQty 76
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.25, Value = 0.67773 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.2, Value = 0.84638 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.15, Value = 1.04355 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.1, Value = 1.29279 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.05, Value = 1.66515 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.025, Value = 1.99167 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.01, Value = 2.37642 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.005, Value = 2.64208 });
            result.Add(new StaticConstT() { SampleQty = 76, Alpha = 0.003, Value = 2.8913 });
            #endregion

            #region SampleQty 77
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.25, Value = 0.67769 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.2, Value = 0.84631 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.15, Value = 1.04346 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.1, Value = 1.29264 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.05, Value = 1.66488 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.025, Value = 1.99125 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.01, Value = 2.37576 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.005, Value = 2.6412 });
            result.Add(new StaticConstT() { SampleQty = 77, Alpha = 0.003, Value = 2.89017 });
            #endregion

            #region SampleQty 78
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.25, Value = 0.67765 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.2, Value = 0.84625 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.15, Value = 1.04337 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.1, Value = 1.2925 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.05, Value = 1.66462 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.025, Value = 1.99085 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.01, Value = 2.37511 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.005, Value = 2.64034 });
            result.Add(new StaticConstT() { SampleQty = 78, Alpha = 0.003, Value = 2.88908 });
            #endregion

            #region SampleQty 79
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.25, Value = 0.67761 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.2, Value = 0.84619 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.15, Value = 1.04328 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.1, Value = 1.29236 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.05, Value = 1.66437 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.025, Value = 1.99045 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.01, Value = 2.37448 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.005, Value = 2.6395 });
            result.Add(new StaticConstT() { SampleQty = 79, Alpha = 0.003, Value = 2.88801 });
            #endregion

            #region SampleQty 80
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.25, Value = 0.67757 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.2, Value = 0.84614 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.15, Value = 1.0432 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.1, Value = 1.29222 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.05, Value = 1.66412 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.025, Value = 1.99006 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.01, Value = 2.37387 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.005, Value = 2.63869 });
            result.Add(new StaticConstT() { SampleQty = 80, Alpha = 0.003, Value = 2.88697 });
            #endregion

            #region SampleQty 81
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.25, Value = 0.67753 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.2, Value = 0.84608 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.15, Value = 1.04311 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.1, Value = 1.29209 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.05, Value = 1.66388 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.025, Value = 1.98969 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.01, Value = 2.37327 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.005, Value = 2.6379 });
            result.Add(new StaticConstT() { SampleQty = 81, Alpha = 0.003, Value = 2.88596 });
            #endregion

            #region SampleQty 82
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.25, Value = 0.67749 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.2, Value = 0.84603 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.15, Value = 1.04303 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.1, Value = 1.29196 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.05, Value = 1.66365 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.025, Value = 1.98932 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.01, Value = 2.37269 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.005, Value = 2.63712 });
            result.Add(new StaticConstT() { SampleQty = 82, Alpha = 0.003, Value = 2.88497 });
            #endregion

            #region SampleQty 83
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.25, Value = 0.67746 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.2, Value = 0.84597 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.15, Value = 1.04295 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.1, Value = 1.29183 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.05, Value = 1.66342 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.025, Value = 1.98896 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.01, Value = 2.37212 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.005, Value = 2.63637 });
            result.Add(new StaticConstT() { SampleQty = 83, Alpha = 0.003, Value = 2.88401 });
            #endregion

            #region SampleQty 84
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.25, Value = 0.67742 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.2, Value = 0.84592 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.15, Value = 1.04287 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.1, Value = 1.29171 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.05, Value = 1.6632 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.025, Value = 1.98861 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.01, Value = 2.37156 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.005, Value = 2.63563 });
            result.Add(new StaticConstT() { SampleQty = 84, Alpha = 0.003, Value = 2.88307 });
            #endregion

            #region SampleQty 85
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.25, Value = 0.67739 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.2, Value = 0.84587 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.15, Value = 1.0428 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.1, Value = 1.29159 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.05, Value = 1.66298 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.025, Value = 1.98827 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.01, Value = 2.37102 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.005, Value = 2.63491 });
            result.Add(new StaticConstT() { SampleQty = 85, Alpha = 0.003, Value = 2.88215 });
            #endregion

            #region SampleQty 86
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.25, Value = 0.67735 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.2, Value = 0.84582 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.15, Value = 1.04272 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.1, Value = 1.29147 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.05, Value = 1.66277 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.025, Value = 1.98793 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.01, Value = 2.37049 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.005, Value = 2.63421 });
            result.Add(new StaticConstT() { SampleQty = 86, Alpha = 0.003, Value = 2.88126 });
            #endregion

            #region SampleQty 87
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.25, Value = 0.67732 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.2, Value = 0.84577 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.15, Value = 1.04265 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.1, Value = 1.29136 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.05, Value = 1.66256 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.025, Value = 1.98761 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.01, Value = 2.36998 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.005, Value = 2.63353 });
            result.Add(new StaticConstT() { SampleQty = 87, Alpha = 0.003, Value = 2.88039 });
            #endregion

            #region SampleQty 88
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.25, Value = 0.67729 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.2, Value = 0.84572 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.15, Value = 1.04258 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.1, Value = 1.29125 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.05, Value = 1.66235 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.025, Value = 1.98729 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.01, Value = 2.36947 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.005, Value = 2.63286 });
            result.Add(new StaticConstT() { SampleQty = 88, Alpha = 0.003, Value = 2.87953 });
            #endregion

            #region SampleQty 89
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.25, Value = 0.67726 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.2, Value = 0.84568 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.15, Value = 1.04251 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.1, Value = 1.29114 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.05, Value = 1.66216 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.025, Value = 1.98698 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.01, Value = 2.36898 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.005, Value = 2.6322 });
            result.Add(new StaticConstT() { SampleQty = 89, Alpha = 0.003, Value = 2.87870 });
            #endregion

            #region SampleQty 90
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.25, Value = 0.67723 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.2, Value = 0.84563 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.15, Value = 1.04244 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.1, Value = 1.29103 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.05, Value = 1.66196 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.025, Value = 1.98667 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.01, Value = 2.36850 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.005, Value = 2.63157 });
            result.Add(new StaticConstT() { SampleQty = 90, Alpha = 0.003, Value = 2.87788 });
            #endregion

            #region SampleQty 91
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.25, Value = 0.67720 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.2, Value = 0.84559 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.15, Value = 1.04237 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.1, Value = 1.29092 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.05, Value = 1.66177 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.025, Value = 1.98638 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.01, Value = 2.36803 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.005, Value = 2.63094 });
            result.Add(new StaticConstT() { SampleQty = 91, Alpha = 0.003, Value = 2.87709 });
            #endregion

            #region SampleQty 92
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.25, Value = 0.67717 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.2, Value = 0.84555 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.15, Value = 1.04231 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.1, Value = 1.29082 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.05, Value = 1.66159 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.025, Value = 1.98609 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.01, Value = 2.36757 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.005, Value = 2.63033 });
            result.Add(new StaticConstT() { SampleQty = 92, Alpha = 0.003, Value = 2.87631 });
            #endregion

            #region SampleQty 93
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.25, Value = 0.67714 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.2, Value = 0.84550 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.15, Value = 1.04224 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.1, Value = 1.29072 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.05, Value = 1.66140 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.025, Value = 1.98580 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.01, Value = 2.36712 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.005, Value = 2.62973 });
            result.Add(new StaticConstT() { SampleQty = 93, Alpha = 0.003, Value = 2.87555 });
            #endregion

            #region SampleQty 94
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.25, Value = 0.67711 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.2, Value = 0.84546 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.15, Value = 1.04218 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.1, Value = 1.29062 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.05, Value = 1.66123 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.025, Value = 1.98552 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.01, Value = 2.36667 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.005, Value = 2.62915 });
            result.Add(new StaticConstT() { SampleQty = 94, Alpha = 0.003, Value = 2.87480 });
            #endregion

            #region SampleQty 95
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.25, Value = 0.67708 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.2, Value = 0.84542 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.15, Value = 1.04212 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.1, Value = 1.29053 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.05, Value = 1.66105 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.025, Value = 1.98525 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.01, Value = 2.36624 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.005, Value = 2.62858 });
            result.Add(new StaticConstT() { SampleQty = 95, Alpha = 0.003, Value = 2.87407 });
            #endregion

            #region SampleQty 96
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.25, Value = 0.67705 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.2, Value = 0.84538 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.15, Value = 1.04206 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.1, Value = 1.29043 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.05, Value = 1.66088 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.025, Value = 1.98498 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.01, Value = 2.36582 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.005, Value = 2.62802 });
            result.Add(new StaticConstT() { SampleQty = 96, Alpha = 0.003, Value = 2.87336 });
            #endregion

            #region SampleQty 97
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.25, Value = 0.67703 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.2, Value = 0.84534 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.15, Value = 1.04200 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.1, Value = 1.29034 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.05, Value = 1.66071 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.025, Value = 1.98472 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.01, Value = 2.36541 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.005, Value = 2.62747 });
            result.Add(new StaticConstT() { SampleQty = 97, Alpha = 0.003, Value = 2.87266 });
            #endregion

            #region SampleQty 98
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.25, Value = 0.67700 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.2, Value = 0.84530 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.15, Value = 1.04195 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.1, Value = 1.29025 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.05, Value = 1.66055 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.025, Value = 1.98447 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.01, Value = 2.36500 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.005, Value = 2.62693 });
            result.Add(new StaticConstT() { SampleQty = 98, Alpha = 0.003, Value = 2.87198 });
            #endregion

            #region SampleQty 99
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.25, Value = 0.67698 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.2, Value = 0.84527 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.15, Value = 1.04189 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.1, Value = 1.29016 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.05, Value = 1.66039 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.025, Value = 1.98422 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.01, Value = 2.36461 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.005, Value = 2.62641 });
            result.Add(new StaticConstT() { SampleQty = 99, Alpha = 0.003, Value = 2.87131 });
            #endregion

            #region SampleQty 100
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.25, Value = 0.67695 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.2, Value = 0.84523 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.15, Value = 1.04184 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.1, Value = 1.29007 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.05, Value = 1.66023 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.025, Value = 1.98397 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.01, Value = 2.36422 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.005, Value = 2.62589 });
            result.Add(new StaticConstT() { SampleQty = 100, Alpha = 0.003, Value = 2.87065 });
            #endregion

            return result;
        }
    }


}
