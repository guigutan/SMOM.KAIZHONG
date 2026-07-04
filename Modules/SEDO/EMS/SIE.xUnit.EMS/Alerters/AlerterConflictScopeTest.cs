using SIE.Alert;
using System;
using Xunit;

namespace SIE.xUnit.EMS.Alerters
{
    /// <summary>
    /// 预警范围是否冲突单元测试
    /// </summary>
    public class AlerterConflictScopeTest : EmsControllerTestBase
    {
        public SeverityScopeRule Rule { get; set; } = new SeverityScopeRule();

        /// <summary>
        /// 范围冲突，等于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 3, null)]
        [InlineData(Operator.Greater, 2, null)]
        [InlineData(Operator.Less, 3, null)]
        [InlineData(Operator.Less, 4, null)]
        [InlineData(Operator.Between, 2, 3)]
        [InlineData(Operator.Between, 2, 4)]
        public void AlerterConflictScopeTestEqual1(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Equal;
            a.MinVal = 3;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.True(res);
        }

        /// <summary>
        /// 范围不冲突，等于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 2, null)]
        [InlineData(Operator.Greater, 3, null)]
        [InlineData(Operator.Greater, 4, null)]
        [InlineData(Operator.Less, 2, null)]
        [InlineData(Operator.Between, 3, 4)]
        public void AlerterConflictScopeTestEqual2(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Equal;
            a.MinVal = 3;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.False(res);
        }

        /// <summary>
        /// 范围冲突，大于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 4, null)]
        [InlineData(Operator.Greater, 2, null)]
        [InlineData(Operator.Greater, 3, null)]
        [InlineData(Operator.Greater, 4, null)]
        [InlineData(Operator.Less, 4, null)]
        [InlineData(Operator.Between, 3, 4)]
        [InlineData(Operator.Between, 2, 4)]
        public void AlerterConflictScopeTestLarge1(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Greater;
            a.MinVal = 3;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.True(res);
        }

        /// <summary>
        /// 范围不冲突，大于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 3, null)]
        [InlineData(Operator.Equal, 2, null)]
        [InlineData(Operator.Less, 3, null)]
        [InlineData(Operator.Less, 2, null)]
        [InlineData(Operator.Between, 2, 3)]
        public void AlerterConflictScopeTestLarge2(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Greater;
            a.MinVal = 3;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.False(res);
        }

        /// <summary>
        /// 范围冲突，小于等于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 2, null)]
        [InlineData(Operator.Equal, 3, null)]
        [InlineData(Operator.Less, 3, null)]
        [InlineData(Operator.Less, 4, null)]
        [InlineData(Operator.Less, 2, null)]
        [InlineData(Operator.Greater, 2, null)]
        [InlineData(Operator.Between, 2, 4)]
        [InlineData(Operator.Between, 2, 3)]
        public void AlerterConflictScopeTestLess1(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Less;
            a.MinVal = 3;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.True(res);
        }

        /// <summary>
        /// 范围不冲突，小于等于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 4, null)]
        [InlineData(Operator.Greater, 3, null)]
        [InlineData(Operator.Greater, 4, null)]
        [InlineData(Operator.Between, 3, 4)]
        public void AlerterConflictScopeTestLess2(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Less;
            a.MinVal = 3;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.False(res);
        }

        /// <summary>
        /// 范围冲突，介于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 4, null)]
        [InlineData(Operator.Equal, 3, null)]
        [InlineData(Operator.Greater, 2, null)]
        [InlineData(Operator.Greater, 3, null)]
        [InlineData(Operator.Less, 3, null)]
        [InlineData(Operator.Less, 4, null)]
        [InlineData(Operator.Between, 1, 3)]
        [InlineData(Operator.Between, 2, 3)]
        [InlineData(Operator.Between, 2, 4)]
        [InlineData(Operator.Between, 2, 5)]
        [InlineData(Operator.Between, 3, 5)]
        [InlineData(Operator.Between, 1, 5)]
        public void AlerterConflictScopeTestBetween1(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Between;
            a.MinVal = 2;
            a.MaxVal = 4;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.True(res);
        }

        /// <summary>
        /// 范围不冲突，介于
        /// </summary>
        /// <param name="opB"></param>
        /// <param name="minValB"></param>
        /// <param name="maxValB"></param>
        [Theory]
        [InlineData(Operator.Equal, 2, null)]
        [InlineData(Operator.Greater, 4, null)]
        [InlineData(Operator.Less, 2, null)]
        [InlineData(Operator.Between, 1, 2)]
        [InlineData(Operator.Between, 0, 1)]
        [InlineData(Operator.Between, 2, 2)]
        [InlineData(Operator.Between, 4, 4)]
        [InlineData(Operator.Between, 4, 5)]
        [InlineData(Operator.Between, 5, 6)]
        public void AlerterConflictScopeTestBetween2(Operator opB, double minValB, double? maxValB)
        {
            var a = new Severity();
            var b = new Severity();
            a.Operator = Operator.Between;
            a.MinVal = 2;
            a.MaxVal = 4;
            b.Operator = opB;
            b.MinVal = (decimal)minValB;
            b.MaxVal = (decimal?)maxValB;
            var res = Rule.IsConflictScope(a, b);
            Assert.False(res);
        }
    }
}
