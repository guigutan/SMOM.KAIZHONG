using SIE.Api;
using SIE.Common.Configs;
using SIE.Domain;
using SIE.Domain.Validation;
using SIE.Equipments.Enums;
using SIE.EMS.FixedAssets.Accounts.Config;
using System;

namespace SIE.EMS.FixedAssets.Accounts
{
    /// <summary>
    /// 固定资产折旧控制器
    /// </summary>
    public partial class FixedAssetsAccountController
    {
        /// <summary>
        /// 执行资产折旧
        /// </summary>
        public virtual void SyncAssetDepreciation()
        {
            var config = ConfigService.GetConfig(new FixedAssetAccountConfig(), typeof(FixedAssetsAccount));
            if (config == null)
            {
                throw new ValidationException("未找到固定资产台账配置项,请检查配置".L10N());
            }
            //折旧方式
            var depreciationWay = config.DepreciationWay;
            //加载已审核的固定资产台账
            var fixedAssetsAccounts = Query<FixedAssetsAccount>().Where(m => m.ReviewStatus == ApprovalStatus.Audited).ToList();

            foreach (var fixedAssetsAccount in fixedAssetsAccounts)
            {
                //计算资产折旧
                ComputeAssetDepreciation(fixedAssetsAccount, depreciationWay);
            }
        }

        /// <summary>
        /// 计算资产折旧
        /// </summary>
        /// <param name="fixedAssetsAccount">固定资产</param>
        /// <param name="depreciationWay"></param>
        public virtual void ComputeAssetDepreciation(FixedAssetsAccount fixedAssetsAccount, DepreciationWay depreciationWay)
        {
            if (fixedAssetsAccount == null)
            {
                throw new ValidationException("【{0}】为空".L10nFormat(nameof(fixedAssetsAccount)));
            }
            
            if (fixedAssetsAccount.ReviewStatus != ApprovalStatus.Audited)
            {
                return;
            }

            var dbTime = RF.Find<FixedAssetsAccount>().GetDbTime();

            //当前月份已执行过折旧则不在执行
            if (fixedAssetsAccount.LastDepreciartion.HasValue && fixedAssetsAccount.LastDepreciartion.Value.Year == dbTime.Year && fixedAssetsAccount.LastDepreciartion.Value.Month == dbTime.Month)
            {
                return;
            }
            var monthNum = 0;//转固后月份数
                             //审核状态为【已审批】且当前时间的年月比转固日期的年月大2个月的固定资产进行计算，每个固定资产执行一遍
            monthNum = ((dbTime.Year - fixedAssetsAccount.FixedAssetsTransferDate.Year) * 12) + dbTime.Month - fixedAssetsAccount.FixedAssetsTransferDate.Month;

            //当前时间的年月比转固日期的年月大2个月的固定资产进行计算，每个固定资产执行一遍
            if (monthNum >= 2 && fixedAssetsAccount.DepreciationYear > 0)
            {
                //当前折旧期数：第E年F月；从转固日期的后第二个月开始算第1年第1月，
                ComputeParamters computeParamters = new ComputeParamters();
                computeParamters.A = fixedAssetsAccount.OriginalAssetsValue;//资产原值
                computeParamters.X = fixedAssetsAccount.NetAssetValue;//资产净值
                computeParamters.B = fixedAssetsAccount.DepreciationResidualValue;//资产折旧原值
                computeParamters.C = fixedAssetsAccount.DepreciationYear;//折旧年限
                computeParamters.CurrentMonth = monthNum - 1;//当前折旧期数
                computeParamters.K = fixedAssetsAccount.BeginNetWorthValue;//年初资产净值

                computeParamters.depreciationWay = depreciationWay;//折旧方式
                //根据折旧方式计算折旧
                ComputeDepreciation(computeParamters);
                fixedAssetsAccount.BeginNetWorthValue = computeParamters.K;//年初净值
                fixedAssetsAccount.NetAssetValue = computeParamters.X;//资产净值
                fixedAssetsAccount.LastDepreciartion = dbTime;//上次折旧日期
                //保存数据库
                RF.Save(fixedAssetsAccount);
            }
        }

        /// <summary>
        /// 计算折旧
        /// </summary>
        /// <param name="computeParamters"></param>
        /// <returns></returns>
        private ComputeParamters ComputeDepreciation(ComputeParamters computeParamters)
        {
            switch (computeParamters.depreciationWay)
            {
                case DepreciationWay.AverageAge:
                    {
                        //平均年限法
                        AverageAgeDepreciation(computeParamters);
                        break;
                    }
                case DepreciationWay.DoubleDecliningBalance:
                    {
                        //双倍余额递减法
                        DoubleDecliningBalanceDepreciation(computeParamters);
                        break;
                    }
                case DepreciationWay.SumOfYears:
                    {
                        //年限总和法
                        SumOfYearsDepreciation(computeParamters);
                        break;
                    }

                default:
                    break;
            }
            return computeParamters;
        }


        /// <summary>
        /// 平均年限折旧
        /// </summary>
        /// <param name="computeParamters">计算参数</param>
        /// <returns></returns>
        private void AverageAgeDepreciation(ComputeParamters computeParamters)
        {
            /* 如果E=C，且F=12，本次折旧金额G=X-B
               否则，G=（A-B）/（C*12），保留2位小数 折旧后的资产净值X=X-G
             */
            if (computeParamters.C == computeParamters.E && computeParamters.F == 12)
            {
                computeParamters.G = computeParamters.X.Value - computeParamters.B;
            }
            else
            {
                computeParamters.G = (computeParamters.A - computeParamters.B) / (computeParamters.C * 12);
            }

            computeParamters.X = Math.Round(computeParamters.X.Value - computeParamters.G, 2);

            //避免出现负数
            if (computeParamters.X < 0)
            {
                computeParamters.X = 0;
            }
        }


        /// <summary>
        /// 年限总和法
        /// </summary>
        /// <param name="computeParamters">计算参数</param>
        /// <returns></returns>
        private void SumOfYearsDepreciation(ComputeParamters computeParamters)
        {
            /*
              如果E=C，且F=12，本次折旧金额G=X-B
              否则：本次折旧金额G=[（A-B）*（C-E+1）]/[C*(C+1)]/24，保留2位小数
              折旧后的资产净值X=X-G
             */
            if (computeParamters.C == computeParamters.E && computeParamters.F == 12)
            {
                computeParamters.G = computeParamters.X.Value - computeParamters.B;
            }
            else
            {
                computeParamters.G = ((computeParamters.A - computeParamters.B)
                    * (computeParamters.C - computeParamters.E + 1)) / (computeParamters.C * (computeParamters.C + 1)) / 24;
            }

            computeParamters.X = Math.Round(computeParamters.X.Value - computeParamters.G, 2);

            if (computeParamters.X < 0)//避免出现负数
            {
                computeParamters.X = 0;
            }
        }


        /// <summary>
        /// 双倍余额递减法
        /// </summary>
        /// <param name="computeParamters">计算参数</param>
        /// <returns></returns>
        private void DoubleDecliningBalanceDepreciation(ComputeParamters computeParamters)
        {
            if (computeParamters.C == computeParamters.E && computeParamters.F == 12)
            {
                computeParamters.G = computeParamters.X.Value - computeParamters.B;
            }
            else
            {
                if (computeParamters.C <= 2)
                {
                    computeParamters.G = (computeParamters.A - computeParamters.B) / (computeParamters.C * 12);
                }

                if (computeParamters.C > 2)
                {
                    if (computeParamters.E <= computeParamters.C - 2)
                    {
                        computeParamters.G = (computeParamters.K.Value * 2) / (computeParamters.C * 12);
                    }

                    if (computeParamters.E == computeParamters.C - 1)
                    {
                        computeParamters.G = (computeParamters.K.Value - computeParamters.B) / 24;
                    }

                    if (computeParamters.E == computeParamters.C)
                    {
                        computeParamters.G = (computeParamters.K.Value - computeParamters.B) / 12;
                    }
                }
            }
            computeParamters.X = Math.Round(computeParamters.X.Value - computeParamters.G, 2);
            if (computeParamters.X < 0)//避免出现负数
            {
                computeParamters.X = 0;
            }
        }


        /// <summary>
        /// 固定资产折旧测试代码
        /// </summary>
        /// <param name="computeParamters"></param>
        /// <returns></returns>
        [ApiService("固定资产折旧")]
        [return: ApiReturn("折旧参数")]
        public virtual ComputeParamters ComputeDepreciationApi([ApiParameter("折旧参数")] ComputeParamters computeParamters)
        {
            return ComputeDepreciation(computeParamters);
        }
    }

    /// <summary>
    /// 折旧计算参数
    /// </summary>
    [Serializable]
    public class ComputeParamters
    {
        /// <summary>
        /// 
        /// </summary>

        private decimal? _K = null;

        /// <summary>
        /// 
        /// </summary>
        private decimal? _X = null;

        /// <summary>
        /// 资产原值
        /// </summary>
        public decimal A { get; set; }

        /// <summary>
        /// 资产净值：X，为空时等于A
        /// </summary>
        public decimal? X { get => !_X.HasValue ? A : _X; set { _X = value; } }

        /// <summary>
        /// 年初净值（增加字段，后台不显示）：K，为空时等于A，每当F=1时更新K=X
        /// </summary>
        public decimal? K
        {
            get
            {
                if (F == 1)//每当F=1时更新K=X
                {
                    _K = X;
                    return _K;
                }
                else
                {
                    if (!_K.HasValue)//K，为空时等于A
                    {
                        _K = A;
                        return _K;
                    }
                    else
                    {
                        return _K;
                    }
                }

            }
            set
            {
                _K = value;
            }
        }


        /// <summary>
        /// 资产折旧残值
        /// </summary>
        public decimal B { get; set; }

        /// <summary>
        /// 折旧年限
        /// </summary>
        public int C { get; set; }


        /// <summary>
        ///  当前折旧期数：第E年F月；从转固日期的后第二个月开始算第1年第1月，
        /// </summary>

        public int CurrentMonth { get; set; }

        /// <summary>
        /// 第E年 取出年份
        /// </summary>
        public int E
        {
            get
            {
                return (CurrentMonth / 12) + ((CurrentMonth % 12) == 0 ? 0 : 1);
            }
        }

        /// <summary>
        /// 第F月  取出月份
        /// </summary>
        public int F
        {
            get
            {
                return (CurrentMonth % 12 == 0 ? 12 : CurrentMonth % 12);
            }
        }

        /// <summary>
        /// 本次折旧金额
        /// </summary>

        public decimal G { get; set; }

        /// <summary>
        /// 折旧方式
        /// </summary>
        public DepreciationWay depreciationWay { get; set; }
    }
}
