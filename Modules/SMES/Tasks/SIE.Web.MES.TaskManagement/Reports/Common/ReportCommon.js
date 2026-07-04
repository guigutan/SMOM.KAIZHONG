Ext.define('SIE.Web.MES.TaskManagement.Reports.ReportCommon', {
    statics: {
        saveReportRecord: function (view) {
            //保存报工记录，不做任何验证
            var record = view.getCurrent().data;
            if (!record)
                throw new '报工记录为空'.L10N();
            var assTasks = this.getAssTaskReportInfo();
            var defectIds = this.getReportDefectIds(view);
            this.invokeDataQuery('SaveReportRecord', [record, defectIds, assTasks], view.token, function (result) {
                SIE.Msg.showInstantMessage('保存成功'.L10N());
                this.refreshReportRecord(view, false); //保存报工记录不刷任务列表数据
            });
        },

        invokeDataQuery: function (method, params, token, callback) {
            SIE.invokeDataQuery({
                method: method,
                params: params,
                action: 'queryer',
                sync: true,
                type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
                token: token,
                success: function (res) {
                    callback(res);
                }
            });
        },
        refreshReportRecord: function (view, refreshTaskData) {
            var cmd = Ext.create(
                'SIE.Web.MES.TaskManagement.Reports.ReportRefreshCommand', {});
            cmd._setOwnerView(view);
            cmd.command = Ext.getClassName(cmd);
            cmd.tryExecute(cmd);
            if (refreshTaskData)
                view._parent.reloadData();
        },
        getReportDefectIds: function (view) {
            //var mainDefect = Ext.getCmp('mainDefectValue');
            //var defectIds = [];
            //if (mainDefect && mainDefect.valueIds && mainDefect.valueIds.length > 0) {
            //    defectIds = mainDefect.valueIds;
            //}
            var defectIds = [];
            var task = view._parent.getCurrent().data;
            if (view.dicConfig[task.Id])
                defectIds = view.dicConfig[task.Id];
            return defectIds;
        },
        //---------------------------------

        validateReportQty: function (mainRecord, assTasks) {
            var me = this;
            if (!me.validateQty(mainRecord.OkQty, "主任务单合格数"))
                return false;
            if (!me.validateQty(mainRecord.NgQty, "主任务单不合格数"))
                return false;
            if (!me.validateQty(mainRecord.Hour, "统计工时"))
                return false;
            if (assTasks && assTasks.length > 0) {
                assTasks.forEach(function (assTask) {
                    if (!me.validateQty(assTask.ReportOkQty, "辅任务单合格数"))
                        return false;
                    if (!me.validateQty(assTask.ReportNgQty, "辅任务单不合格数"))
                        return false;
                });
            }
            return true;
        },
        validateQty: function (qty, msg) {
            if (qty === null) {
                SIE.Msg.showError(Ext.String.format("{0}不能为空".L10N(), msg));
                return false;
            }
            if (qty < 0) {
                SIE.Msg.showError(Ext.String.format("{0}不能小于0".L10N(), msg));
                return false;
            }
            return true;
        },

        /**
         * 保存或提交报工信息
         * @param {any} view 报工视图
         * @param {any} isReport 是否直接报工
         */
        saveOrSubmmitReport: function (view, isReport, command) {
            var task = view._parent.getCurrent();
            if (!task) {
                SIE.Msg.showError("未找到报工任务单".L10N());
                return;
            }
            var mainRecord = view.getCurrent().data;
            var assTask = SIE.Web.MES.TaskManagement.Reports.ReportCommon.getAssTaskReportInfo();
            if (!this.validateReportQty(mainRecord, assTask))
                return;
            var mainDefectIds = [];
            var task = view._parent.getCurrent().data;
            if (view.dicConfig[task.Id])
                mainDefectIds = view.dicConfig[task.Id];
            var signdata = {
                command: command,
                entityType: view.model,
                parentType: view.getParent() ? view.getParent().model : ""
            }
            var recordId = 0;
            SIE.invokeDataQuery({
                method: 'Report',
                params: [mainRecord, mainDefectIds, assTask, isReport],
                action: 'queryer',
                async: false,
                type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
                token: view.token,
                logInfo: signdata,
                success: function (res) {
                    recordId = res.Result;
                    SIE.Msg.showInstantMessage('操作成功'.L10N());
                    var cmd = Ext.create(
                        'SIE.Web.MES.TaskManagement.Reports.ReportRefreshCommand', {});
                    cmd._setOwnerView(view);
                    cmd.command = Ext.getClassName(cmd);
                    cmd.tryExecute(cmd);
                    if (isReport)
                        view._parent.reloadData();
                }
            });
            return recordId;
        },
        /**获取辅单报工数据
         * */
        getAssTaskReportInfo: function () {
            var rst = [];
            var assCtl = Ext.getCmp('reportTaskRightControl');
            if (assCtl.items.items.length == 0)
                return rst;
            assCtl.items.items.where(function (p) { return p.name == 'formContent'; }).forEach(function (p) {
                var taskNoFiled = p.query('[name=taskNo]')[0];
                var taskId = taskNoFiled.taskId;
                var recordId = taskNoFiled.recordId;
                var workOrderId = taskNoFiled.workOrderId;
                var processId = taskNoFiled.processId;
                var reportOkQtyField = p.query('[name=reportOkQty]')[0];
                var reportOkQty = reportOkQtyField.value;
                var reportNgQtyField = p.query('[name=reportNgQty]')[0];
                var reportNgQty = reportNgQtyField.value;
                var batchNoField = p.query('[name=batchNo]')[0];
                var batchNo = batchNoField.value;
                var quexianValueField = p.query('[name=quexianValue]')[0];
                var defectIds = quexianValueField.valueIds;
                if (reportOkQty < 0 || reportNgQty < 0) return false;
                rst.push({
                    TaskId: taskId,
                    ReportOkQty: reportOkQty,
                    ReportNgQty: reportNgQty,
                    BatchNo: batchNo,
                    IsSyntype: true,
                    RecordId: recordId,
                    DefectIds: defectIds,
                    WorkOrderId: workOrderId,
                    ProcessId: processId
                })
            });
            return rst;
        },

        /*获取共模任务单信息*/
        getCommonModeInfo: function (view) {
            var mainView = view;
            SIE.invokeDataQuery({
                method: 'GetCommonModeInfo',
                params: [view._parent.getCurrent().data.Id],
                async: false,
                action: 'queryer',
                sync: true,
                type: 'SIE.Web.MES.TaskManagement.Reports.ReportDataQueryer',
                token: view.token,
                success: function (res) {
                    var rightItems = [];
                    var id = 'reportTaskRightControl';
                    Ext.getCmp(id).removeAll();
                    if (res.Result && res.Result.length > 0) {
                        var fir = true;
                        document.getElementById(id).style.borderLeftWidth = "1px";
                        res.Result.forEach(function (p) {
                            if (fir) {
                                rightItems.push({
                                    xtype: 'checkboxfield',
                                    name: 'shareTaskCheckBox',
                                    id: 'IsSyntypeField',
                                    readOnly: true,
                                    value: p.IsSyntype,
                                    hideLabel: true,
                                    style: 'margin-top:5px;width:auto;',
                                    boxLabel: '共模任务按共模比报工',
                                })
                            }
                            rightItems.push(SIE.Web.MES.TaskManagement.Reports.ReportLayout.initRightItem(mainView, p, fir));
                            fir = false;
                        });
                        Ext.getCmp(id).add(rightItems);
                    }
                    document.getElementById(id + '-innerCt').style.removeProperty('width');
                }
            });
        },

        /**
         * 共模报工主单更改合格数量，联动修改辅单数量
         * @param {any} newValue 主单合格数量+不合格数量
         */
        setCommonQty: function (newValue) {
            var isSynt = Ext.getCmp('IsSyntypeField');
            if (isSynt && isSynt.getValue() == true) {
                var mainField = Ext.getCmp('MainProportionField');
                if (mainField) {
                    var mainPro = mainField.getValue();
                    if (mainPro > 0) {
                        var assCtl = Ext.getCmp('reportTaskRightControl');
                        assCtl.items.items.where(function (p) { return p.name == 'formContent'; }).forEach(function (p) {
                            var reportOkQtyField = p.query('[name=reportOkQty]')[0];
                            var assPro = reportOkQtyField.proportion;
                            var val = Math.floor((newValue * assPro) * 1.0 / mainPro);
                            var reportNgQtyField = p.query('[name=reportNgQty]')[0];
                            var reportNgQty = reportNgQtyField.value;
                            reportNgQtyField.maxValue = val;
                            if (val - reportNgQty > 0)
                                reportOkQtyField.setValue(val - reportNgQty);
                            else {
                                reportOkQtyField.setValue(0);
                                reportNgQtyField.setValue(val);
                            }
                        });
                    }
                }
            }
        }
    }
});