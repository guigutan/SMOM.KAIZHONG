SIE.defineCommand('SIE.Web.MES.TaskManagement.Reports.Commands.ReportExportByMonthCommand', {
    meta: { text: "按月导出", group: "export", iconCls: "icon-FileCode icon-green" },

    canExecute: function (view) {
        // 始终允许执行导出操作
        return true;
    },

    execute: function (view, source) {
        var myview = view;

        var record = myview._relations[0]._target.getCurrent();
        var criteria = record.data;

        // 尝试从过滤器中获取查询条件
        var criteriaData = criteria;

        if (!criteriaData) {
            SIE.Msg.showError('未找到查询条件！请在控制台查看调试信息。');
            return;
        }

        if (!criteriaData.ReportTime || !criteriaData.ReportTime.BeginValue || !criteriaData.ReportTime.EndValue) {
            SIE.Msg.showError('请先在查询界面选择报工时间范围！');
            return;
        }

        // 获取时间范围
        var monthStart = new Date(criteriaData.ReportTime.BeginValue);
        var monthEnd = new Date(criteriaData.ReportTime.EndValue);

        // 验证时间范围不超过一个月
        var timeSpan = monthEnd - monthStart;
        var maxDays = 31 * 24 * 60 * 60 * 1000;
        if (timeSpan > maxDays) {
            SIE.Msg.showError('导出时间范围不能超过一个月！');
            return;
        }

        // 显示等待提示
        var mask = SIE.ExportExcelHelper.showMask(Ext.getBody().component, '数据准备中...');

        // 构建查询条件对象
        var criteria = {
            ReportTime: {
                BeginValue: monthStart,
                EndValue: monthEnd
            }
        };

        // 使用 SIE.invokeDataQuery 调用后端 Queryer 方法
        SIE.invokeDataQuery({
            method: 'ExportReportRecordExamineByMonth',
            params: [criteria],
            action: 'queryer',
            sync: true,
            timeout: 100000000,
            type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
            token: myview.getToken(),
            success: function (res) {
                mask.hide();

                // 获取返回的数据
                var records = null;
                if (res.Result && res.Result.data && res.Result.data.items) {
                    records = res.Result.data.items;
                }

                if (records && records.length > 0) {
                    // 添加合计行
                    var ModelClass = SIE.getModel(myview.model);
                    var summaryEntity = new ModelClass();
                    summaryEntity.setNo("合计");
                    summaryEntity.setId(0);

                    // 计算合计值
                    var reportQty = 0, recordOkQty = 0, recordNgQty = 0;
                    var reworkQty = 0, suspectQty = 0;

                    for (var i = 0; i < records.length; i++) {
                        var data = records[i].data;
                        reportQty += data.ReportQty || 0;
                        recordOkQty += data.RecordOkQty || 0;
                        recordNgQty += data.RecordNgQty || 0;
                        reworkQty += data.ReworkQty || 0;
                        suspectQty += data.SuspectQty || 0;
                    }

                    summaryEntity.setReportQty(Math.floor(reportQty * 1000) / 1000);
                    summaryEntity.setRecordOkQty(Math.floor(recordOkQty * 1000) / 1000);
                    summaryEntity.setRecordNgQty(Math.floor(recordNgQty * 1000) / 1000);
                    summaryEntity.setReworkQty(Math.floor(reworkQty * 1000) / 1000);
                    summaryEntity.setSuspectQty(Math.floor(suspectQty * 1000) / 1000);
                    summaryEntity.markSaved();

                    // 将合计行添加到 records 数组
                    records.push(summaryEntity);

                    // 格式化文件名
                    var year = monthStart.getFullYear();
                    var month = monthStart.getMonth() + 1;

                    // 调用 Excel 导出
                    SIE.ExportExcelHelper.exportXls(
                        myview.gridConfig,
                        records,
                        '报工记录_' + year + '年' + month + '月'
                    );

                    SIE.Msg.showInstantMessage("成功导出[{0}]笔数据！".t().format(records.length));
                } else {
                    SIE.Msg.showInstantMessage('该时间范围内没有需要导出的数据！');
                }
            },
            failure: function (error) {
                mask.hide();
                SIE.Msg.showError('导出失败：' + (error.Message || '未知错误'));
            }
        });
    }
});
