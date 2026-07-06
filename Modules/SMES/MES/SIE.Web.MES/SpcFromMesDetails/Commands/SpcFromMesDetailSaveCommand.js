/**
 * SPC明细表保存命令
 * 功能：子表保存后，自动刷新store并实时更新父视图的以下字段：
 *   - 样本组数(Qty)、总组数(TotalQty)
 *   - 均值上限(AverageUsl)、均值中心限(AverageSl)、均值下限(AverageLsl)
 *   - 极差上限(RangeUsl)、极差中心限(RangeSl)、极差下限(RangeLsl)
 *
 * 计算依据：SPC Xbar-R控制图（均值-极差控制图）
 *   参考标准：GB/T 4091 (ISO 7870) / AIAG SPC手册第二版（刘飞刘总给的，或者自己百度）
 *   常数A2、D3、D4来源于统计学中的Xbar-R控制图系数表，
 *   索引对应子组大小n（n=2~25），用于根据样本极差估计过程标准差。
 */
SIE.defineCommand('SIE.Web.MES.SpcFromMesDetails.Commands.SpcFromMesDetailSaveCommand', {
    extend: 'SIE.cmd.Save',
    meta: { text: "保存", group: "edit", iconCls: "icon-SaveEntity icon-blue" },

    // Xbar-R控制图系数A2：用于计算均值控制限
    // 公式：均值上限UCL = X̄ + A2 × R̄，均值下限LCL = X̄ - A2 × R̄
    // 索引0和1无意义，从索引2开始对应子组大小n=2~25
    // 来源：GB/T 4091附表 / AIAG SPC手册 / ISO 7870（刘飞刘总给的，或者自己百度）
    SPC_A2: [0, 0, 1.88, 1.023, 0.729, 0.577, 0.483, 0.419, 0.373, 0.337, 0.308, 0.285, 0.266, 0.249, 0.235, 0.223, 0.212, 0.203, 0.194, 0.187, 0.18, 0.173, 0.167, 0.162, 0.157, 0.153],

    // Xbar-R控制图系数D3：用于计算极差下控制限
    // 公式：极差下限LCL = D3 × R̄
    // 当n<7时D3=0，表示极差图无下控制限
    // 来源：同上
    SPC_D3: [0, 0, 0, 0, 0, 0, 0, 0.076, 0.136, 0.184, 0.223, 0.256, 0.283, 0.307, 0.328, 0.347, 0.363, 0.378, 0.391, 0.403, 0.415, 0.425, 0.434, 0.443, 0.451, 0.459],

    // Xbar-R控制图系数D4：用于计算极差上控制限
    // 公式：极差上限UCL = D4 × R̄
    // 来源：同上
    SPC_D4: [0, 0, 3.267, 2.574, 2.282, 2.114, 2.004, 1.924, 1.864, 1.816, 1.777, 1.744, 1.717, 1.693, 1.672, 1.653, 1.637, 1.622, 1.608, 1.597, 1.585, 1.575, 1.566, 1.557, 1.548, 1.541],

    /**
     * 保存成功后回调
     * 处理流程：注册store的load事件 -> reload store -> load完成后更新父视图字段
     * 使用store.on('load', ..., {single:true})确保在数据真正加载完成后再读取最新数据
     */
    onSaved: function (view, res) {
        var me = this;
        me.callParent(arguments);

        var store = view.getStore ? view.getStore() : view.getData();
        if (!store) return;

        var updateParentFields = function () {
            var parentView = view._parent;
            if (!parentView) return;

            // 使用getTotalCount获取总记录数（含分页），避免getCount只返回当前页数量
            var qty = store.getTotalCount ? store.getTotalCount() : store.getCount();

            var parentData = parentView.getData();
            if (!parentData) return;

            var groupCount = parentData.get('GroupCount') || 0;
            var totalQty = groupCount * qty;

            // 更新父实体model数据
            parentData.set('Qty', qty);
            parentData.set('TotalQty', totalQty);

            // 计算SPC六字段并更新父实体
            var spcResult = me.calculateSpcLimits(store, groupCount);
            parentData.set('AverageUsl', spcResult.averageUcl);
            parentData.set('AverageSl', spcResult.averageCl);
            parentData.set('AverageLsl', spcResult.averageLcl);
            parentData.set('RangeUsl', spcResult.rangeUcl);
            parentData.set('RangeSl', spcResult.rangeCl);
            parentData.set('RangeLsl', spcResult.rangeLcl);

            // 同步更新父视图表单控件的显示值
            var form = parentView.getControl();
            if (form) {
                var fields = {
                    'Qty': qty,
                    'TotalQty': totalQty,
                    'AverageUsl': spcResult.averageUcl,
                    'AverageSl': spcResult.averageCl,
                    'AverageLsl': spcResult.averageLcl,
                    'RangeUsl': spcResult.rangeUcl,
                    'RangeSl': spcResult.rangeCl,
                    'RangeLsl': spcResult.rangeLcl
                };
                for (var name in fields) {
                    var field = form.down('[name=' + name + ']');
                    if (field) {
                        field.setValue(fields[name]);
                        field.setRawValue(fields[name]);
                    }
                }
            }
        };

        // 先注册load事件（single:true表示只触发一次），再reload确保拿到服务端最新数据
        store.on('load', updateParentFields, me, { single: true });
        store.reload();
    },

    /**
     * 计算SPC Xbar-R控制图的6个控制限
     * @param {Ext.data.Store} store - 明细表store（包含所有样本组记录）
     * @param {Number} groupCount - 子组大小（每组观测值个数）
     * @return {Object} 包含6个控制限值：
     *   averageUcl - 均值上控制限 = X̄ + A2 × R̄
     *   averageCl  - 均值中心线   = X̄（所有样本组均值的平均值）
     *   averageLcl - 均值下控制限 = X̄ - A2 × R̄
     *   rangeUcl   - 极差上控制限 = D4 × R̄
     *   rangeCl    - 极差中心线   = R̄（所有样本组极差的平均值）
     *   rangeLcl   - 极差下控制限 = D3 × R̄
     */
    calculateSpcLimits: function (store, groupCount) {
        var me = this;
        var result = { averageUcl: 0, averageCl: 0, averageLcl: 0, rangeUcl: 0, rangeCl: 0, rangeLcl: 0 };

        // 子组大小必须在2~25范围内（常数表有效范围）
        if (groupCount < 2 || groupCount > 25) return result;

        var records = store.getRange ? store.getRange() : [];
        if (records.length === 0) return result;

        var sumMean = 0;   // 累计各组均值之和
        var sumRange = 0;  // 累计各组极差之和
        var count = 0;     // 有效样本组数

        for (var r = 0; r < records.length; r++) {
            var record = records[r];
            var values = [];
            // 取当前行中GroupCount个有效观测值
            for (var i = 1; i <= groupCount; i++) {
                var val = record.get('ObservedValue' + i);
                if (val !== null && val !== undefined && val !== '') {
                    var num = parseFloat(val);
                    if (!isNaN(num)) {
                        values.push(num);
                    }
                }
            }
            if (values.length === 0) continue;

            // 计算当前样本组的均值和极差
            var sum = 0;
            var min = values[0];
            var max = values[0];
            for (var j = 0; j < values.length; j++) {
                sum += values[j];
                if (values[j] < min) min = values[j];
                if (values[j] > max) max = values[j];
            }
            var mean = sum / values.length;  // 当前组均值 x̄ᵢ
            var range = max - min;           // 当前组极差 Rᵢ

            sumMean += mean;
            sumRange += range;
            count++;
        }

        if (count === 0) return result;

        // X̄（X-double-bar）：所有样本组均值的平均值
        var xBar = sumMean / count;
        // R̄（R-bar）：所有样本组极差的平均值
        var rBar = sumRange / count;

        // 根据子组大小查表获取对应系数
        var A2 = me.SPC_A2[groupCount] || 0;
        var D3 = me.SPC_D3[groupCount] || 0;
        var D4 = me.SPC_D4[groupCount] || 0;

        // 计算控制限，保留6位小数
        result.averageUcl = Math.round((xBar + A2 * rBar) * 1000000) / 1000000;  // 均值上限
        result.averageCl = Math.round(xBar * 1000000) / 1000000;                  // 均值中心限
        result.averageLcl = Math.round((xBar - A2 * rBar) * 1000000) / 1000000;  // 均值下限
        result.rangeUcl = Math.round((D4 * rBar) * 1000000) / 1000000;           // 极差上限
        result.rangeCl = Math.round(rBar * 1000000) / 1000000;                    // 极差中心限
        result.rangeLcl = Math.round((D3 * rBar) * 1000000) / 1000000;           // 极差下限

        return result;
    }
});
