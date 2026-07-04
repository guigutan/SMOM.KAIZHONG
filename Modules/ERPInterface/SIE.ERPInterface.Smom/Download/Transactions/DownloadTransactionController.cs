using SIE.ERPInterface.Common.Controller;

namespace SIE.ERPInterface.Smom.Download
{
    /// <summary>
    /// ERP事务下载控制器
    /// </summary>
    public class DownloadTransactionController : DownloadBusBaseController
    {
        //#region 功能下载

        ///// <summary>
        ///// 从中间表下载功能到业务表
        ///// </summary>
        //public virtual ProcessResult DownloadFunctionToBusiness(bool isManual = false)
        //{
        //    var baseDataCtl = RT.Service.Resolve<BaseDataController>();             //接口基础数据控制器

        //    //功能中间表数据
        //    var datas = this.GetUnprocessedDatas<FunctionInf>();
        //    var functionCodes = datas.Select(p => p.Code).Distinct().ToList();

        //    //功能业务表数据
        //    var functions = baseDataCtl.GetBusinessDataEntitys<Function>(functionCodes, Function.CodeProperty.Name);
        //    var functionDict = functions.ToDictionary(p => p.Code, p => p);

        //    //执行业务逻辑
        //    var result = this.SaveBusinessData(datas, p =>
        //    {
        //        //获取业务实体
        //        var function = this.GenerateFunctionEntity(p, functionDict);
        //        if (p.IsDelete)
        //            function.PersistenceStatus = PersistenceStatus.Deleted;

        //        return function;
        //    },
        //    p => new EntityList<Function>(), JobType.Function, isManual);

        //    return result;
        //}

        ///// <summary>
        ///// 生成功能实体
        ///// </summary>
        ///// <param name="functionInf">中间表实体数据</param>
        ///// <param name="functionDict">业务功能 字典</param>
        ///// <returns></returns>
        //private Function GenerateFunctionEntity(FunctionInf functionInf, Dictionary<string, Function> functionDict)
        //{
        //    var function = this.GenerateEntity(functionDict, functionInf.Code);     //查找或生成功能实体
        //    function.Code = functionInf.Code;
        //    function.Name = functionInf.Name;
        //    function.Description = functionInf.Description;
        //    function.IsQc = functionInf.IsQc;
        //    function.SourceType = SIE.Common.SourceType.External;

        //    return function;
        //}

        //#endregion

        //#region 事务下载

        ///// <summary>
        ///// 从中间表下载事务到业务表
        ///// </summary>
        //public virtual ProcessResult DownloadTransactionToBusiness(bool isManual = false)
        //{
        //    var baseDataCtl = RT.Service.Resolve<BaseDataController>();             //接口基础数据控制器

        //    //事务中间表数据
        //    var datas = this.GetUnprocessedDatas<TransactionInf>();
        //    var transactionCodes = datas.Select(p => p.Code).Distinct().ToList();

        //    //事务业务表数据
        //    var transactions = baseDataCtl.GetBusinessDataEntitys<Transaction>(transactionCodes, Transaction.CodeProperty.Name);
        //    var transactionDict = transactions.ToDictionary(p => p.Code, p => p);

        //    //执行业务逻辑
        //    var result = this.SaveBusinessData(datas, p =>
        //    {
        //        //获取业务实体
        //        var transaction = this.GenerateTransactionEntity(p, transactionDict);
        //        if (p.IsDelete)
        //            transaction.PersistenceStatus = PersistenceStatus.Deleted;
        //        return transaction;
        //    },
        //    p => new EntityList<Transaction>(), JobType.Transaction, isManual);

        //    return result;
        //}

        ///// <summary>
        ///// 生成事务实体
        ///// </summary>
        ///// <param name="FunctionInf">中间表实体数据</param>
        ///// <param name="FunctionDict">业务事务 字典</param>
        ///// <returns></returns>
        //private Transaction GenerateTransactionEntity(TransactionInf transactionInf, Dictionary<string, Transaction> transactionDict)
        //{
        //    var transaction = this.GenerateEntity(transactionDict, transactionInf.Code);     //查找或生成事务实体
        //    transaction.Code = transactionInf.Code;
        //    transaction.Name = transactionInf.Name;
        //    transaction.Description = transactionInf.Description;
        //    transaction.IsInternalUse = transactionInf.IsInternalUse;
        //    transaction.MesProcessName = transactionInf.MesProcessName;
        //    transaction.RfcProcessName = transactionInf.RfcProcessName;
        //    transaction.IsUpload = transactionInf.IsUpload;
        //    transaction.SortOut = transactionInf.SortOut;
        //    transaction.SourceType = SIE.Common.SourceType.External;

        //    return transaction;
        //}

        //#endregion
    }
}
