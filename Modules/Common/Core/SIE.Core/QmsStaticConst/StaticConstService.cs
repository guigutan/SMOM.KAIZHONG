using SIE.Core.Common;
using SIE.Core.Common.Service;
using SIE.Domain;
using SIE.Domain.Validation;
using System;
using System.Linq;

namespace SIE.Core.QmsStaticConst
{
    /// <summary>
    /// MSA常用参数服务
    /// </summary>
    public class StaticConstService : DomainService
    {
        private readonly StaticConstDao _msaConstDao;
        private readonly StaticConstTDao _msaConstTDao;
        private readonly StaticConstD2Dao _msaConstD2Dao;
        private readonly ControlChartConstDao _controlChartConstDao;
        private readonly StaticConstK1Dao _msaConstK1Dao;
        private readonly StaticConstK2Dao _msaConstK2Dao;
        private readonly StaticConstK3Dao _msaConstK3Dao;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="msaConstDao"></param>
        /// <param name="msaConstTDao"></param>
        /// <param name="msaConstD2Dao"></param>
        /// <param name="controlChartConstDao"></param>
        /// <param name="msaConstK1Dao"></param>
        /// <param name="msaConstK2Dao"></param>
        /// <param name="msaConstK3Dao"></param>
        public StaticConstService(StaticConstDao msaConstDao, StaticConstTDao msaConstTDao, StaticConstD2Dao msaConstD2Dao, ControlChartConstDao controlChartConstDao, StaticConstK1Dao msaConstK1Dao, StaticConstK2Dao msaConstK2Dao, StaticConstK3Dao msaConstK3Dao)
        {
            _msaConstDao = msaConstDao;
            _msaConstTDao = msaConstTDao;
            _msaConstD2Dao = msaConstD2Dao;
            _controlChartConstDao = controlChartConstDao;
            _msaConstK1Dao = msaConstK1Dao;
            _msaConstK2Dao = msaConstK2Dao;
            _msaConstK3Dao = msaConstK3Dao;
        }

        #region 查询

        /// <summary>
        /// 根据名称获取MSA常用参数
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual StaticConst GetMsaConstByName(string name)
        {
            return _msaConstDao.GetMsaConstByName(name);
        }

        /// <summary>
        /// 根据编码获取MSA常用参数
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual StaticConst GetMsaConstByCode(string code)
        {
            return _msaConstDao.GetMsaConstByCode(code);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<ControlChartConst> GetControlChartConsts(double msaConstId)
        {
            return _controlChartConstDao.GetControlChartConsts(msaConstId);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<StaticConstK1> GetMsaConstK1s(double msaConstId)
        {
            return _msaConstK1Dao.GetMsaConstK1s(msaConstId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<StaticConstK2> GetMsaConstK2s(double msaConstId)
        {
            return _msaConstK2Dao.GetMsaConstK2s(msaConstId);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="msaConstId"></param>
        /// <returns></returns>
        public virtual EntityList<StaticConstK3> GetMsaConstK3s(double msaConstId)
        {
            return _msaConstK3Dao.GetMsaConstK3s(msaConstId);
        }



        #endregion

        #region 保存

        /// <summary>
        /// 保存验证
        /// </summary>
        /// <param name="msaConsts"></param>
        private void SaveValidation(EntityList<StaticConst> msaConsts)
        {
            foreach (var msaConst in msaConsts)
            {
                msaConst.GetAllChildData<StaticConst, ControlChartConst>();
                msaConst.GetAllChildData<StaticConst, StaticConstK1>();
                msaConst.GetAllChildData<StaticConst, StaticConstK2>();
                msaConst.GetAllChildData<StaticConst, StaticConstK3>();

                var sampleQtyList = msaConst.ListControlChart.Select(c => c.SampleQty).ToList();
                if (sampleQtyList.Count != sampleQtyList.Distinct().Count())
                {
                    throw new ValidationException("编码为[{0}]的常用参数的[控制图参数]的n值不能重复！".L10nFormat(msaConst.Code));
                }
                var testQtyList = msaConst.ListK1.Select(c => c.TestQty).ToList();
                if (testQtyList.Count != testQtyList.Distinct().Count())
                {
                    throw new ValidationException("编码为[{0}]的常用参数的[K1]的试验次数不能重复！".L10nFormat(msaConst.Code));
                }
                var evaluateQtyList = msaConst.ListK2.Select(c => c.EvaluateQty).ToList();
                if (evaluateQtyList.Count != evaluateQtyList.Distinct().Count())
                {
                    throw new ValidationException("编码为[{0}]的常用参数的[K2]的评价人数不能重复！".L10nFormat(msaConst.Code));
                }
                var k3SampleQtyList = msaConst.ListK3.Select(c => c.SampleQty).ToList();
                if (k3SampleQtyList.Count != k3SampleQtyList.Distinct().Count())
                {
                    throw new ValidationException("编码为[{0}]的常用参数的[K3]的样本数量不能重复！".L10nFormat(msaConst.Code));
                }
                msaConst.ListD2.ForEach(c=> {
                    if (c.MsaConstD2Type == StaticConstD2Type.D2s && c.Value == 0) {
                        throw new ValidationException("编码为[{0}]的常用参数的[d2*]的[d₂*]不能为0！".L10nFormat(msaConst.Code));
                    }
                });
            }

        }


        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="msaConsts"></param>
        public virtual void Save(EntityList<StaticConst> msaConsts)
        {

            //验证
            SaveValidation(msaConsts);


            foreach (var msaConst in msaConsts)
            {
                //处理t值表数据
                if (msaConst.ListT.Count > 0)
                {
                    var list = _msaConstTDao.FindMany(c => c.MsaConstId == msaConst.Id);

                    msaConst.ListT.ForEach(t =>
                    {
                        var single = list.FirstOrDefault(c => c.SampleQty == t.SampleQty && c.Alpha == t.Alpha);
                        if (single == null)
                        {
                            t.PersistenceStatus = PersistenceStatus.New;
                        }
                        else
                        {
                            t.Id = single.Id;
                            if (t.Value != single.Value)
                            {
                                t.PersistenceStatus = PersistenceStatus.Modified;
                            }
                            else
                            {
                                t.PersistenceStatus = PersistenceStatus.Unchanged;
                            }
                        }
                    });
                    list.ForEach(t =>
                    {
                        var single = msaConst.ListT.FirstOrDefault(c => c.SampleQty == t.SampleQty && c.Alpha == t.Alpha);

                        if (single == null)
                        {
                            t.PersistenceStatus = PersistenceStatus.Deleted;
                            msaConst.ListT.Add(t);
                        }

                    });

                }


                //处理D2表数据
                if (msaConst.ListD2.Count > 0)
                {
                    var list = _msaConstD2Dao.FindMany(c => c.MsaConstId == msaConst.Id);

                    msaConst.ListD2.ForEach(t =>
                    {
                        var single = list.FirstOrDefault(c => c.SampleQty == t.SampleQty && c.TestQty == t.TestQty && c.MsaConstD2Type == t.MsaConstD2Type);
                        if (single == null)
                        {
                            t.PersistenceStatus = PersistenceStatus.New;
                        }
                        else
                        {
                            t.Id = single.Id;
                            if (t.Value != single.Value)
                            {
                                t.PersistenceStatus = PersistenceStatus.Modified;
                            }
                            else
                            {
                                t.PersistenceStatus = PersistenceStatus.Unchanged;
                            }
                        }
                    });
                    list.ForEach(t =>
                    {
                        var single = msaConst.ListD2.FirstOrDefault(c => c.SampleQty == t.SampleQty && c.TestQty == t.TestQty && c.MsaConstD2Type == t.MsaConstD2Type);

                        if (single == null)
                        {
                            t.PersistenceStatus = PersistenceStatus.Deleted;
                            msaConst.ListD2.Add(t);
                        }

                    });

                }
            }

            _msaConstDao.Save(msaConsts);

        }

        #endregion
    }
}
