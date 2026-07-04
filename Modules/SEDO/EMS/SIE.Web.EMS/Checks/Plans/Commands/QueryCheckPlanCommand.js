SIE.defineCommand('SIE.Web.EMS.Checks.Plans.Commands.QueryCheckPlanCommand', {
    extend: 'SIE.cmd.ExecuteQuery',
    meta: { text: "查询" },
    _mainView: null,
    //是否点击
    _isClick: false,
    execute: function (view) {
        this._mainView = view.getResultView();

        var record = view.getCurrent();

        if (record.data.Month == null) {
            SIE.Msg.showError("查询条件【年月】不能为空，请检查！".t());
            return false;
        }

        this.setViewColumns(view.getResultView(), record.data.Month);

        var emsCommonHelper = new SIE.Web.EMS.Common.Script.EmsCommonHelper();
        var mask = emsCommonHelper.showMask(view.getResultView().getControl());

        delete record.data['CriteriaModuleKey'];
        delete record.data['CriteriaType'];
        delete record.data["CriteriaString"];
        delete record.data["LinkData"];

        //查询复用URL参数传递：EntityType等，2021-3-10 Ridge
        var urlParams = CRT.Context.PageContext.getParams();
        if (urlParams) {
            for (var item in urlParams) {
                if (!record.data[item])
                    record.data[item] = urlParams[item];
            }
        }

        var istrue = true;
        view.getControl().items.items.forEach(function (item) {
            if (item.validate && !item.validate()) {
                istrue = false;
            }
        });
        if (istrue) {
            view.tryExecuteQuery({
                clearSort: true,
                action: 'entity'
            });
        }

        mask.hide();
    },
    /**
     * 视图关联后方法
     * @method onViewReady
     * @param {ListLogicView} view 产品族视图
     */
    setViewColumns: function (view, datetime) {
        var me = this;

        //日期部份删除
        var gridPanel = view.getControl();
        var grid = gridPanel.actionables[0].grid;
        var gridColumns = grid.config.columns;
        gridColumns.splice(8);

        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer",
            method: "GetCheckPlanColumns",
            params: [datetime],
            async: false,
            token: view.token,
            success: function (res) {

                for (const dataColumn of gridColumns) {
                    dataColumn.lockable=false;
                }
                for (const dataColumn of res.Result) {
                    var DayNum = dataColumn.DayNum;
                    var BeginTime = dataColumn.BeginTime == null ? "" : ("<br>(" + dataColumn.BeginTime + "-");
                    var EndTime = dataColumn.EndTime == null ? "" : (dataColumn.EndTime + ")");
                    var ShiftName = dataColumn.ShiftName;
                    var dataIndex = DayNum + BeginTime + EndTime;
                    var colName = Ext.isEmpty(ShiftName) ? dataIndex : DayNum + ("<br>(" + ShiftName + ")");
                    var colWidth = (!Ext.isEmpty(ShiftName) ? 58 : 104);
                    colWidth = BeginTime == "" ? 44 : colWidth;
                    var column = {
                        dataIndex: dataIndex,
                        text: colName,
                        header: colName,
                        width: colWidth,
                        align: 'center',
                        sortable: false,
                        lockable: false,
                        renderer: function (value, meta, record, rowIndex, colIndex, store, view) {
                            var columnName = gridColumns[colIndex + 8].dataIndex;
                            var index = columnName.indexOf('-');
                            if (index != -1)
                                columnName = columnName.slice(0, index + 1);
                            if (record.data.DataJsonString != null && record.data.DataJsonString.length > 0) {
                                var columns = JSON.parse(record.data.DataJsonString)

                                for (const col of columns) {
                                    if (col.ColumnName == columnName) {
                                        if (col.ExeState == 0) {
                                            meta.tdStyle = "border-right: 1px solid white; background: yellow;";
                                        } else if (col.ExeState == 1) {
                                            meta.tdStyle = "border-right: 1px solid white; background: lightgreen;";
                                        } else if (col.ExeState == 2) {
                                            meta.tdStyle = "border-right: 1px solid white; background: red;";
                                        } else if (col.ExeState == 4) {
                                            meta.tdStyle = "border-right: 1px solid white; background: orange;";
                                        } else if (col.ExeState == 5) {
                                            meta.tdStyle = "border-right: 1px solid white; background: lightblue;";
                                        }
                                        return col.ExeResult;
                                    }
                                }
                            }
                            return '';
                        },
                    };
                    gridColumns.push(column);
                }
            }
        });
        grid.reconfigure(grid.store, gridColumns);

        grid.mon(grid, 'cellDblClick', me.cellDblClick, me);

        me.showTodayCell(grid);
    },
    // 计算当天列
    showTodayCell: function (grid) {
        var today = (new Date()).getDate();
        var cellWidth = grid.getColumnManager().getColumns()[9].getWidth();
        var len = (today - 1) * cellWidth;
        var counter = 1;
        for (var i = 9; i < grid.getColumnManager().getColumns().length - 1; i++) {
            var curCell = grid.getColumnManager().getColumns()[i].text;
            var nextCell = grid.getColumnManager().getColumns()[i + 1].text;
            if (curCell.split("<")[0] !== nextCell.split("<")[0]) {
                break;
            }
            else {
                counter++;
            }
        }
        if (grid.getColumnManager().getColumns().length > 40) {
            len = len * counter;
        }
        grid.scrollByDeltaX(len, true);
    },
    /**
    * 
    * @method 
    * @param
    */
    compatibleWithIE: function () {
        //设置状态对应样式
        // IE 兼容方法
        if (typeof Object.assign != 'function') {
            Object.assign = function (target) {
                'use strict';
                if (target == null) {
                    throw new TypeError('Cannot convert undefined or null to object');
                }

                target = Object(target);
                for (var index = 1; index < arguments.length; index++) {
                    var source = arguments[index];
                    if (source != null) {
                        for (var key in source) {
                            if (Object.prototype.hasOwnProperty.call(source, key)) {
                                target[key] = source[key];
                            }
                        }
                    }
                }
                return target;
            };
        }
    },

    /**
    * 界面双击事件
    * @method cellDblClick
    * @param 
    */
    cellDblClick: function (grid, td, cellIndex, record, tr, rowIndex, e, eOpts) {
        var me = this;

        //列字段名
        var clickedDataIndex = grid.getHeaderAtIndex(cellIndex).dataIndex;
        //当双击的字段不是日期对应的数值，跳出。（目前功能，非点检计划数据有以下字段）
        if (clickedDataIndex == 'WorkShopName' || clickedDataIndex == 'ProcessName' || clickedDataIndex == 'EquipAccountCode' || clickedDataIndex == 'ResourceName'
            || clickedDataIndex == 'YearAndMonth' || clickedDataIndex == 'EquipAccountName' || clickedDataIndex == 'EquipModelName' || clickedDataIndex == 'EquipTypeName')
            return;

        //空白单元格，跳出
        if (record.data.DataJsonString == null) {
            return;
        }
        var columns = JSON.parse(record.data.DataJsonString)
        if (columns.all(function (column) { return clickedDataIndex.indexOf(column.BeginTime) < 0; })) {
            return;
        }

        var equipCode = record.data.EquipAccountCode;
        var equipId = record.data.EquipAccountId;
        //从VM信息中，构建当前点击选中单元格对应的[点检开始和结束时间]
        var beginDate = new Date(record.get("YearAndMonth"));
        var endDate = new Date(record.get("YearAndMonth"));

        //通过<br>字符判断点检类型，区分点检时间构建逻辑
        var brIndex = clickedDataIndex.indexOf("<br>");//<br>字符串位置
        if (brIndex == -1) {
            //点检类型为日，clickedDataIndex的值直接为日
            beginDate.setDate(clickedDataIndex);
            beginDate.setHours(0);
            beginDate.setMinutes(0);
            beginDate.setSeconds(0);
            endDate.setDate(clickedDataIndex);
            endDate.setHours(23);
            endDate.setMinutes(59);
            endDate.setSeconds(59);
        }
        else {
            //点检类型非日，clickedDataIndex的值为 "日<br>(开始时间-结束时间)"
            var leftIndex = clickedDataIndex.indexOf("(");
            var splitIndex = clickedDataIndex.indexOf("-");
            var rightIndex = clickedDataIndex.indexOf(")");

            var day = clickedDataIndex.substring(0, brIndex);//获取日
            var startTime = clickedDataIndex.substring(leftIndex + 1, splitIndex).split(":");//获取开始时间
            var endTime = clickedDataIndex.substring(splitIndex + 1, rightIndex).split(":");//获取结束时间

            beginDate.setDate(day);
            beginDate.setHours(startTime[0]);
            beginDate.setMinutes(startTime[1]);
            beginDate.setSeconds(0);
            endDate.setDate(day);
            endDate.setHours(endTime[0]);
            endDate.setMinutes(endTime[1]);
            endDate.setSeconds(0);
        }

        //触发点检计划单选择逻辑
        me.selectCheckPlan(equipId, beginDate, endDate);
    },

    /**
    * 触发点检计划单选择逻辑
    * @method SelectCheckPlan
    * @param  {string} equipCode 设备Id
    * @param  {date} beginDate 点检开始时间
    * @param  {date} endDate 点检结束时间
    * @param  {ListlView} view 生成的view
    */
    selectCheckPlan: function (equipId, beginDate, endDate) {
        var me = this;

        SIE.AutoUI.getMeta({
            model: 'SIE.Web.EMS.Checks.Plans.ViewModels.SelDepartmentViewModel',
            ignoreCommands: true,
            isDetail: false,
            ignoreQuery: false,
            callback: function (res) {
                var mainBlock;
                if (res.mainBlock)
                    mainBlock = res.mainBlock;
                else
                    mainBlock = res;
                var listView = SIE.AutoUI.createListView(mainBlock);
                var ui = listView.getControl();

                //查询当前用户，当前时间段，拥有操作权限的点检单，按部门分类显示
                //理论上，一个部门最多在该时间节点只有一张点检单
                SIE.invokeDataQuery({
                    method: 'GetCheckPlan',
                    params: [equipId, beginDate, endDate],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer',
                    token: me._mainView.token,
                    success: function (res) {
                        if (res.Success) {
                            if (res.Result == null || res.Result.length <= 0) {
                                SIE.MessageBox.showMessage("当前节点没有点检单数据".t());
                                return;
                            } 
                            if (res.Result.length === 1) {
                                //当只有一张检验单，直接打开页签
                                if ((res.Result[0].State === 3 || res.Result[0].State === 5 || res.Result[0].State === 7) && res.Result[0].IfOpenConfirmationTab === true) {
                                    if (res.Result[0].IsConfirm) {
                                        //已评分或者待确认则打开确认界面
                                        me.showConfirmationView(res.Result[0], beginDate, endDate);
                                    }
                                    else {
                                        SIE.Msg.showMessage("当前登陆人设备与人员权限没有点检确认权限".t());
                                    }
                                } else {
                                    //其他状态则打开执行界面
                                    me.showExecutionView(res.Result[0], beginDate, endDate);
                                }
                            } else {
                                //赋值界面数据
                                res.Result.forEach(function (item) {
                                    listView.getData().add(item);
                                });

                                //存在多张检验单，打开点检单选择页面
                                me.showCheckPlanSelectionView(ui, listView, beginDate, endDate);
                            }
                        }
                    },
                })
            },
        });
    },

    /**
    * 弹出点检单选择页面
    * @method showCheckPlanSelectionView
    * @param  {ctl} ui 界面UI控件
    * @param  {ListlView} view 点检单选择界面view
    * @param  {date} beginDate 点检开始时间
    * @param  {date} endDate 点检结束时间
    * @param  {ListlView} view 点检计划主界面view
    */
    showCheckPlanSelectionView: function (ui, listView, beginDate, endDate) {
        var me = this;
        var win = SIE.Window.show({
            title: "选择点检单".t(),
            width: 500,
            height: 500,
            items: ui,
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var current = listView.getCurrent();
                    if (current == null)
                        SIE.Msg.showMessage("请选择点检单".t());

                    //依据执行状态打开对应的页签
                    if ((current.data.State === 3 || current.data.State === 5 || current.data.State === 7) && current.data.IfOpenConfirmationTab === true) {
                        if (current.data.IsConfirm) {
                            //已评分或者待确认则打开确认界面
                            me.showConfirmationView(current.data, beginDate, endDate);
                        }
                        else {
                            SIE.Msg.showMessage("当前登陆人设备与人员权限没有点检确认权限".t());
                        }
                    } else {
                        //其他状态则打开执行界面
                        me.showExecutionView(current.data, beginDate, endDate);
                    }

                    win.close()

                    //返回false，确定后不关闭窗口
                    return false;
                }
            }
        });
    },

    /**
    * 弹出“点检计划执行”页签效果
    * @method showExecutionView
    * @param  {SelDepartmentViewModel} data 部门选择VM
    * @param  {date} beginDate 点检开始时间
    * @param  {date} endDate 点检结束时间
    * @param  {ListlView} view 生成的view
    */
    showExecutionView: function (data, beginDate, endDate) {
        var me = this;

        //判断是否已打开对应的点检执行，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {         
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        }

        if (me._isClick) {
            return;
        }

        me._isClick = true;

        //初始化选中点检单数据，用以生成点检项目，查询上一次点检小结
        SIE.invokeDataQuery({
            method: 'InitExeCheckPlan',
            params: [data.CheckPlanId, data.EquipAccountId, data.DepartmentId],
            action: 'queryer',
            type: 'SIE.Web.EMS.Checks.Plans.DataQuery.CheckPlanDataQueryer',
            token: me._mainView.token,
            success: function (res) {
                if (res.Success) {
                    CRT.Workbench.addPage({
                        entityType: "SIE.EMS.Checks.Plans.CheckPlan",
                        recordId: data.CheckPlanId,
                        title: "点检执行".t(),
                        viewGroup: "PlanExecuteViewGroup",
                        module: me._mainView.module,
                        isDetail: true,
                        ignoreQuery: true,
                        params: {
                            SelectBeginTime: beginDate,
                            SelectEndTime: endDate,
                            LastCheckSummary: res.Result
                        },
                    });
                }

                me._isClick = false;
            },
            error: function (res) {
                if (!res.Success) {
                    me._isClick = false;
                }
            },
            failure: function (response, opts) {
                //http state
                if ('communication failure' === response.statusText) {
                    SIE.Msg.showWarning('请求时间超时'.t());
                } else if (response.statusText === '') {
                    //do nothing
                }
                else {
                    var res = response.responseJson;
                    if (!res && response.responseText) {
                        res = Ext.decode(response.responseText);
                        SIE.Msg.showError(res.Message);
                    }
                }

                me._isClick = false;
            }
        });
    },

    /**
    * 弹出“点检计划确认”页签效果
    * @method showConfirmationView
    * @param  {SelDepartmentViewModel} data 部门选择VM
    * @param  {date} beginDate 点检开始时间
    * @param  {date} endDate 点检结束时间
    * @param  {ListlView} view 生成的view
    */
    showConfirmationView: function (data, beginDate, endDate) {
        var me = this;
        //判断是否已打开对应的点检执行，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {
            me.isTabExist = true;
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        } else {
            me.isTabExist = false;
        }

        if (!this.isTabExist) {
            //打开“点检确认”明细页签
            SIE.invokeDataQuery({
                method: 'InitConfirmCheckPlan',
                params: [data.CheckPlanId, data.CheckPlanNo, data.EquipAccountId, data.DepartmentId],
                action: 'queryer',
                type: 'SIE.Web.EMS.Checks.Confirmations.DataQuery.CheckConfirmationQueryer',
                token: me._mainView.token,
                success: function (res) {
                    if (res.Success) {
                        CRT.Workbench.addPage({
                            entityType: "SIE.EMS.Checks.Plans.CheckPlan",
                            recordId: data.CheckPlanId,
                            title: "点检确认".t(),
                            module: me._mainView.module,
                            viewGroup: "PlanConfirmViewGroup",
                            isDetail: true,
                            ignoreQuery: true,
                            token: me._mainView.token,
                            params: {
                                RecordId: data.CheckPlanId,
                                CheckPlanNo: data.CheckPlanNo,
                                DepartmentId: data.DepartmentId,
                                DepartmentName: data.DepartmentName,
                                EquipAccountId: data.EquipAccountId,
                                ConfirmResult: res.Result.ConfirmResult,
                                ConfirmNote: res.Result.ConfirmNote,
                                CheckExeState: res.Result.CheckExeState
                            }
                        });
                    }
                }
            })
        }
    }

});