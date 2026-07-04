using SIE.Common.ImportHelper;
using SIE.Core.Import;
using SIE.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace SIE.Core.QmsStaticConst.Import
{
    /// <summary>
    /// MSA常用参数导入 处理逻辑类
    /// </summary>
    [Services.Service(FallbackType = typeof(ImportStaticConstHandle), ServiceLifeStyle = Services.ServiceLifeStyle.Transient)]
    public class ImportStaticConstHandle : IDisposable, IMasterSubordinateDynamicBusinessImport
    {
        private const string codeColumn = "编码";
        private const string nameColumn = "名称";
        private const string tTableName = "t分布数据表";
        private const string d2TableName = "d2^常数表";

        public const string ConstTabName = "统计系数表";
        /// <summary>
        /// 
        /// </summary>
        public ImportStaticConstHandle()
        {
            CreateSubordinateDynamicColumnName();
        }

        #region 业务属性相关
        /// <summary>
        /// 导入主表模板的列头名
        /// </summary>
        public List<string> MasterColumnNameList { get; set; } = new List<string>() { codeColumn, nameColumn };

        /// <summary>
        /// 主表列的标准验证 (列名 列对应验证 )
        /// </summary>
        public Dictionary<string, ValidColumn> MasterColumnValidList { get; set; }

        /// <summary>
        /// 导入从表模板的列头名(key=SheetName)
        /// </summary>
        public Dictionary<string, List<string>> SubordinateColumnNameDic { get; set; }

        /// <summary>
        /// 导入从表列的标准验证 (列名 列对应验证) (key=SheetName)
        /// </summary>
        public Dictionary<string, Dictionary<string, ValidColumn>> SubordinateColumnValidDic { get; set; }

        /// <summary>
        /// 从表关联主表的列名
        /// </summary>
        public string AssociationColumnName { get; set; } = "编码";

        /// <summary>
        /// 动态列的从表
        /// </summary>
        public List<string> SubordinateDynamicList { get; set; }


        /// <summary>
        /// 动态列的孙表
        /// </summary>
        public List<string> GrandsonDynamicList { get; set; }

        /// <summary>
        /// 导入孙表模板的列头名(key=SheetName)
        /// </summary>
        public Dictionary<string, List<string>> GrandsonColumnNameDic { get; set; }

        /// <summary>
        /// 导入孙表列的标准验证 (列名 列对应验证) (key=SheetName)
        /// </summary>
        public Dictionary<string, Dictionary<string, ValidColumn>> GrandsonColumnValidDic { get; set; }

        /// <summary>
        /// 孙表关联从表的列名
        /// </summary>
        public Dictionary<string, Tuple<string, string>> AssociationSubordinateColumnName { get; set; }



        /// <summary>
        /// 创建列验证
        /// </summary>
        /// <returns></returns>
        public IMasterSubordinateDynamicBusinessImport CreateColumnValid()
        {
            this.MasterColumnValidList = new Dictionary<string, ValidColumn>();
            this.MasterColumnValidList.Add(codeColumn, new ValidColumn(ImportDataType._String, true, ValidCode));
            this.MasterColumnValidList.Add(nameColumn, new ValidColumn(ImportDataType._String, true, ValidName));

            this.SubordinateColumnValidDic = new Dictionary<string, Dictionary<string, ValidColumn>>();

            var spcValidColumn = new Dictionary<string, ValidColumn>();
            spcValidColumn.Add("n", new ValidColumn(ImportDataType._Int, true, true));
            spcValidColumn.Add("A", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("A₂", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("A₃", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("B₃", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("B₄", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("B₅", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("B₆", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("C₄", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("D₁", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("D₂", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("D₃", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("D₄", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("d₂", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("d₃", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("d₄", new ValidColumn(ImportDataType._Double, false, true));
            spcValidColumn.Add("E₂", new ValidColumn(ImportDataType._Double, false, true));                       
            spcValidColumn.Add("MeA₂", new ValidColumn(ImportDataType._Double, false, true));
            this.SubordinateColumnValidDic.Add("SPC常数表", spcValidColumn);

            var k1ValidColumn = new Dictionary<string, ValidColumn>();
            k1ValidColumn.Add("试验次数", new ValidColumn(ImportDataType._Int, true, true));
            k1ValidColumn.Add("K1", new ValidColumn(ImportDataType._Double, true, true));
            this.SubordinateColumnValidDic.Add("K1", k1ValidColumn);

            var k2ValidColumn = new Dictionary<string, ValidColumn>();
            k2ValidColumn.Add("评价人数", new ValidColumn(ImportDataType._Int, true, true));
            k2ValidColumn.Add("K2", new ValidColumn(ImportDataType._Double, true, true));
            this.SubordinateColumnValidDic.Add("K2", k2ValidColumn);

            var k3ValidColumn = new Dictionary<string, ValidColumn>();
            k3ValidColumn.Add("样本数", new ValidColumn(ImportDataType._Int, true, true));
            k3ValidColumn.Add("K3", new ValidColumn(ImportDataType._Double, true, true));
            this.SubordinateColumnValidDic.Add("K3", k3ValidColumn);


            var tValidColumn = new Dictionary<string, ValidColumn>();
            this.SubordinateColumnValidDic.Add(tTableName, tValidColumn);

            var d2ValidColumn = new Dictionary<string, ValidColumn>();
            this.SubordinateColumnValidDic.Add(d2TableName, d2ValidColumn);


            return this;
        }

        private void CreateSubordinateDynamicColumnName()
        {
            SubordinateColumnNameDic = new Dictionary<string, List<string>>();
            SubordinateDynamicList = new List<string>() { tTableName, d2TableName };

            SubordinateColumnNameDic["SPC常数表"] = new List<string>() { codeColumn, "n", "A", "A₂", "A₃", "B₃", "B₄", "B₅", "B₆", "C₄", "D₁", "D₂", "D₃", "D₄", "d₂",  "d₃", "d₄", "E₂", "MeA₂" };
            SubordinateColumnNameDic[tTableName] = new List<string>() { };
            SubordinateColumnNameDic[d2TableName] = new List<string>() { };
            SubordinateColumnNameDic["K1"] = new List<string>() { codeColumn, "试验次数", "K1" };
            SubordinateColumnNameDic["K2"] = new List<string>() { codeColumn, "评价人数", "K2" };
            SubordinateColumnNameDic["K3"] = new List<string>() { codeColumn, "样本数", "K3" };

        }

        #endregion

        #region 业务处理
        /// <summary>
        /// 业务处理
        /// </summary>
        /// <param name="correctDataRowDic"></param>
        public void ProcessBusinessDataHandle(Dictionary<string, DataRow[]> correctDataRowDic)
        {
            EntityList<StaticConst> msaConstList = new EntityList<StaticConst>();

            //1.组装数据
            foreach (DataRow dr in correctDataRowDic[ConstTabName])
            {
                StaticConst msaConst = new StaticConst();
                msaConst.GenerateId();
                msaConst.Code = dr.Field<string>(codeColumn);
                msaConst.Name = dr.Field<string>(nameColumn);
                msaConst.ListControlChart.AddRange(CreateControlChartConstList(correctDataRowDic["SPC常数表"], msaConst));
                msaConst.ListT.AddRange(CreateConstTList(correctDataRowDic[tTableName], msaConst));
                msaConst.ListD2.AddRange(CreateConstD2List(correctDataRowDic[d2TableName], msaConst));
                msaConst.ListK1.AddRange(CreateConstK1List(correctDataRowDic["K1"], msaConst));
                msaConst.ListK2.AddRange(CreateConstK2List(correctDataRowDic["K2"], msaConst));
                msaConst.ListK3.AddRange(CreateConstK3List(correctDataRowDic["K3"], msaConst));
                msaConstList.Add(msaConst);
            }

            using (var tran = DB.TransactionScope(CoreEntityDataProvider.ConnectionStringName))
            {
                RF.Save(msaConstList);
                tran.Complete();
            }

        }

        /// <summary>
        /// 创建[SPC常数表]列表数据
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="MsaConst"></param>
        /// <returns></returns>
        private EntityList<ControlChartConst> CreateControlChartConstList(DataRow[] drs, StaticConst MsaConst)
        {
            EntityList<ControlChartConst> result = new EntityList<ControlChartConst>();
            if (drs != null && drs.Length > 0)
            {
                var thisDrs = drs.ToList().FindAll(c => c.Field<string>(this.AssociationColumnName) == MsaConst.Code);
                if (thisDrs.IsNotEmpty())
                {
                    thisDrs.ForEach(dr =>
                    {

                        ControlChartConst entity = new ControlChartConst();
                        entity.GenerateId();
                        entity.MsaConstId = MsaConst.Id;
                        entity.SampleQty = Convert.ToInt32(dr.Field<string>("n"));
                        if (double.TryParse(dr.Field<string>("A"), out double A))
                            entity.A = A;
                        if (double.TryParse(dr.Field<string>("A₂"), out double A2))
                            entity.A2 = A2;
                        if (double.TryParse(dr.Field<string>("A₃"), out double A3))
                            entity.A3 = A3;
                        if (double.TryParse(dr.Field<string>("B₃"), out double B3))
                            entity.B3 = B3;
                        if (double.TryParse(dr.Field<string>("B₄"), out double B4))
                            entity.B4 = B4;
                        if (double.TryParse(dr.Field<string>("B₅"), out double B5))
                            entity.B5 = B5;
                        if (double.TryParse(dr.Field<string>("B₆"), out double B6))
                            entity.B6 = B6;
                        if (double.TryParse(dr.Field<string>("C₄"), out double C4))
                            entity.C4 = C4;
                        if (double.TryParse(dr.Field<string>("D₁"), out double D1))
                            entity.D1 = D1;
                        if (double.TryParse(dr.Field<string>("D₂"), out double D2))
                            entity.D2 = D2;
                        if (double.TryParse(dr.Field<string>("D₃"), out double D3))
                            entity.D3 = D3;
                        if (double.TryParse(dr.Field<string>("D₄"), out double D4))
                            entity.D4 = D4;
                        if (double.TryParse(dr.Field<string>("d₂"), out double D2Nd))
                            entity.D2Nd = D2Nd;
                        if (double.TryParse(dr.Field<string>("d₃"), out double D3Nd))
                            entity.D3Nd = D3Nd;
                        if (double.TryParse(dr.Field<string>("d₄"), out double D4Nd))
                            entity.D4Nd = D4Nd;
                        if (double.TryParse(dr.Field<string>("E₂"), out double E2))
                            entity.E2 = E2;
                        if (double.TryParse(dr.Field<string>("MeA₂"), out double meA2))
                            entity.MeA2 = meA2;
                        entity.IsFixed = false;
                        result.Add(entity);
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 创建[t]列表数据
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="MsaConst"></param>
        /// <returns></returns>
        private EntityList<StaticConstT> CreateConstTList(DataRow[] drs, StaticConst MsaConst)
        {
            EntityList<StaticConstT> result = new EntityList<StaticConstT>();
            if (drs != null && drs.Length > 0)
            {
                var thisDrs = drs.ToList().FindAll(c => c.Field<string>(this.AssociationColumnName) == MsaConst.Code);
                if (thisDrs.IsNotEmpty())
                {
                    var columns = drs[0].Table.Columns;
                    thisDrs.ForEach(dr =>
                    {
                        for (int i = 0; i < columns.Count; i++)
                        {
                            if (columns[i].ColumnName == ImportDataHandle.MessageColumnName
                                || columns[i].ColumnName == ImportDataHandle.RowIndex
                                || columns[i].ColumnName == "Sheet"
                                || columns[i].ColumnName == this.AssociationColumnName
                                || columns[i].ColumnName == "v")
                            {
                                continue;
                            }
                            StaticConstT entity = new StaticConstT();
                            entity.GenerateId();
                            entity.MsaConstId = MsaConst.Id;
                            entity.SampleQty = Convert.ToInt32(dr.Field<string>("v"));
                            entity.Alpha = double.Parse(columns[i].ColumnName);
                            entity.Value = double.Parse(dr.Field<string>(columns[i].ColumnName));
                            entity.IsFixed = false;
                            result.Add(entity);
                        }
                    });
                }
            }
            for (int i = 0; i < result.Count; i++)
            {
                result[i].Value = Math.Round(result[i].Value,5);
            }
            return result;
        }


        /// <summary>
        /// 创建[d2]列表数据
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="MsaConst"></param>
        /// <returns></returns>
        private EntityList<StaticConstD2> CreateConstD2List(DataRow[] drs, StaticConst MsaConst)
        {
            EntityList<StaticConstD2> result = new EntityList<StaticConstD2>();

            if (msaConstD2TypeDic == null)
            {
                msaConstD2TypeDic = ImportExtension.GetEnumLabel(typeof(StaticConstD2Type), string.Empty);
            }
            if (drs != null && drs.Length > 0)
            {
                var thisDrs = drs.ToList().FindAll(c => c.Field<string>(this.AssociationColumnName) == MsaConst.Code);
                if (thisDrs.IsNotEmpty())
                {
                    var columns = drs[0].Table.Columns;
                    thisDrs.ForEach(dr =>
                    {

                        for (int i = 0; i < columns.Count; i++)
                        {
                            if (columns[i].ColumnName == ImportDataHandle.MessageColumnName
                                || columns[i].ColumnName == ImportDataHandle.RowIndex
                                || columns[i].ColumnName == "Sheet"
                                || columns[i].ColumnName == this.AssociationColumnName
                                || columns[i].ColumnName == "子组数量"
                                || columns[i].ColumnName == "类型")
                            {
                                continue;
                            }
                            StaticConstD2 entity = new StaticConstD2();
                            entity.GenerateId();
                            entity.MsaConstId = MsaConst.Id;
                            entity.MsaConstD2Type = (StaticConstD2Type)msaConstD2TypeDic[dr.Field<string>("类型")];
                            if (entity.MsaConstD2Type == StaticConstD2Type.D2 || entity.MsaConstD2Type == StaticConstD2Type.Cd)//这2个类型的子组数量用不到，可能不导入，默认1
                                    entity.SampleQty = 1;
                            else
                                entity.SampleQty = Convert.ToInt32(dr.Field<string>("子组数量"));

                            entity.TestQty = Convert.ToInt32(columns[i].ColumnName);
                            entity.Value = double.Parse(dr.Field<string>(columns[i].ColumnName));
                            entity.IsFixed = false;
                            result.Add(entity);
                        }
                    });
                }
            }
            return result;
        }


        /// <summary>
        /// 创建[K1]列表数据
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="MsaConst"></param>
        /// <returns></returns>
        private EntityList<StaticConstK1> CreateConstK1List(DataRow[] drs, StaticConst MsaConst)
        {
            EntityList<StaticConstK1> result = new EntityList<StaticConstK1>();
            if (drs != null && drs.Length > 0)
            {
                var thisDrs = drs.ToList().FindAll(c => c.Field<string>(this.AssociationColumnName) == MsaConst.Code);
                if (thisDrs.IsNotEmpty())
                {
                    thisDrs.ForEach(dr =>
                    {

                        StaticConstK1 entity = new StaticConstK1();
                        entity.GenerateId();
                        entity.MsaConstId = MsaConst.Id;
                        entity.TestQty = Convert.ToInt32(dr.Field<string>("试验次数"));
                        entity.Value = double.Parse(dr.Field<string>("K1"));
                        entity.IsFixed = false;
                        result.Add(entity);
                    });
                }
            }
            return result;
        }

        /// <summary>
        /// 创建[K2]列表数据
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="MsaConst"></param>
        /// <returns></returns>
        private EntityList<StaticConstK2> CreateConstK2List(DataRow[] drs, StaticConst MsaConst)
        {
            EntityList<StaticConstK2> result = new EntityList<StaticConstK2>();
            if (drs != null && drs.Length > 0)
            {
                var thisDrs = drs.ToList().FindAll(c => c.Field<string>(this.AssociationColumnName) == MsaConst.Code);
                if (thisDrs.IsNotEmpty())
                {
                    thisDrs.ForEach(dr =>
                    {
                        StaticConstK2 entity = new StaticConstK2();
                        entity.GenerateId();
                        entity.MsaConstId = MsaConst.Id;
                        entity.EvaluateQty = Convert.ToInt32(dr.Field<string>("评价人数"));
                        entity.Value = double.Parse(dr.Field<string>("K2"));
                        entity.IsFixed = false;
                        result.Add(entity);
                    });
                }
            }
            return result;
        }


        /// <summary>
        /// 创建[K3]列表数据
        /// </summary>
        /// <param name="drs"></param>
        /// <param name="MsaConst"></param>
        /// <returns></returns>
        private EntityList<StaticConstK3> CreateConstK3List(DataRow[] drs, StaticConst MsaConst)
        {
            EntityList<StaticConstK3> result = new EntityList<StaticConstK3>();
            if (drs != null && drs.Length > 0)
            {
                var thisDrs = drs.ToList().FindAll(c => c.Field<string>(this.AssociationColumnName) == MsaConst.Code);
                if (thisDrs.IsNotEmpty())
                {
                    thisDrs.ForEach(dr =>
                    {
                        StaticConstK3 entity = new StaticConstK3();
                        entity.GenerateId();
                        entity.MsaConstId = MsaConst.Id;
                        entity.SampleQty = Convert.ToInt32(dr.Field<string>("样本数"));
                        entity.Value = double.Parse(dr.Field<string>("K3"));
                        entity.IsFixed = false;
                        result.Add(entity);
                    });
                }
            }
            return result;
        }


        #endregion

        #region 验证

        #region 字典

        private Dictionary<string, double> nameDic { get; set; }

        private Dictionary<string, double> codeDic { get; set; }

        private Dictionary<string, Enum> msaConstD2TypeDic { get; set; }


        #endregion

        /// <summary>
        /// 验证编码是否重复
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool ValidCode(object obj, out string messageTip, DataRow dr)
        {
            string context = obj.ToString();
            messageTip = string.Empty;
            if (codeDic == null)
            {
                codeDic = new Dictionary<string, double>();
            }
            if (!codeDic.ContainsKey(context))
            {
                var MsaConst = RT.Service.Resolve<StaticConstService>().GetMsaConstByCode(context);
                if (MsaConst != null)
                {
                    codeDic.Add(context, MsaConst.Id);
                    messageTip = "已存在于系统".L10N();
                }
                else
                    codeDic.Add(context, -1);//避免excel中的重复

            }
            else
            {
                messageTip = "重复".L10N();
            }

            return messageTip == string.Empty;
        }


        /// <summary>
        /// 验证标准名称是否重复
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="messageTip"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        private bool ValidName(object obj, out string messageTip, DataRow dr)
        {
            string context = obj.ToString();
            messageTip = string.Empty;
            if (nameDic == null)
            {
                nameDic = new Dictionary<string, double>();
            }
            if (!nameDic.ContainsKey(context))
            {
                var MsaConst = RT.Service.Resolve<StaticConstService>().GetMsaConstByName(context);
                if (MsaConst != null)
                {
                    nameDic.Add(context, MsaConst.Id);
                    messageTip = "已存在于系统".L10N();
                }
                else
                    nameDic.Add(context, -1);//避免excel中的重复

            }
            else
            {
                messageTip = "重复".L10N();
            }

            return messageTip == string.Empty;
        }


        #endregion

        #region 释放资源

        /// <summary>
        /// 
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (codeDic != null)
            {
                codeDic.Clear();
                codeDic = null;
            }
            if (nameDic != null)
            {
                nameDic.Clear();
                nameDic = null;
            }
        }

        /// <summary>
        /// 释放资源
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }

}
