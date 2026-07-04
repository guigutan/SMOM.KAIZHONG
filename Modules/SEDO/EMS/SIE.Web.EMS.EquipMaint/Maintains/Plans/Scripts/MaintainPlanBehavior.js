Ext.define('SIE.Web.EMS.EquipMaint.Maintains.Plans.Scripts.MaintainPlanBehavior', {
    /**
    * view生命周期函数--view生成前
    * @param {*} meta 实体视图元数据
    * @param {*} curEntity 当前操作实体(可空)
    */
    beforeCreate: function (meta, curEntity) {
        var me = this;

        if (!meta) {
            return;
        }

        if (meta.model != 'SIE.EMS.Maintains.Plans.ViewModels.MaintainPlanViewModel') {
            return;
        }

        var render = {
            renderer: function (value, meta) {
                if (value === 0) {
                    meta.tdStyle = "border-right: 1px solid white; background: lightgray;";
                } else if (value === 1 || value === 3) {
                    meta.tdStyle = "border-right: 1px solid white; background: lightgreen;";
                } else if (value === 2) {
                    meta.tdStyle = "border-right: 1px solid white; background: yellow;";
                } else {
                    meta.tdStyle = "border-right: 1px solid white; background: white;";
                }
                return '';
            }
        };
        //设置状态对应样式
        var bg = false;
        meta.gridConfig.columns.forEach(function (e) {
            if (e.dataIndex === 'January') bg = true;//从1月往后开始的所有列全部设置背景色
            if (bg) {
                //me.compatibleWithIE();
                e = Object.assign(e, render);
            }
        })
        //背景描述
        meta.gridConfig.dockedItems[1].items.push({
            xtype: 'tbtext',
            style: 'background: lightgreen; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '已执行'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: yellow; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '未执行'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: orange; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '执行中'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: lightblue; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '保养待确认'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: red; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '超期'.t()
        });        
    },
    /**
    * view生命周期函数--view生成后
    * @param {DetailView} view 生成的view
    */
    onCreated: function (view) {
        me = this;
        mainView = view;
    },

    /**
     * 视图关联后方法
     * @method onViewReady
     * @param {ListLogicView} view 产品族视图
     */
    onViewReady: function (view) {
        var me = this;
        var gridPanel = view.getControl();
        var grid = gridPanel.ownerGrid;
        var gridColumns = grid.config.columns;
        var currDate = new Date();
        SIE.invokeDataQuery({
            type: "SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer",
            method: "GetMaintainPlanColumns",
            params: [],
            async: false,
            token: view.token,
            callback: function (res) {
                for (const dataColumn of res.Result)  {
                    var DayNum = "W" + dataColumn.DayNum;
                    var BeginTime = dataColumn.BeginTime + "-";
                    var EndTime = dataColumn.EndTime;
                    var dataIndex = BeginTime + EndTime;
                    var colName = DayNum;
                    var colWidth = 72;
                    var column = {
                        dataIndex: dataIndex,
                        text: colName,
                        header: colName,
                        width: colWidth,
                        align: 'center',
                        sortable: false,
                        renderer: function (value, meta, record, rowIndex, colIndex, store, view) {

                            var columnName = gridColumns[colIndex + 7].dataIndex;

                            var colBeginDate = new Date(gridColumns[colIndex + 7].dataIndex.split("-")[0]);

                            if (record.data.DataJsonString != null) {
                                var columns = JSON.parse(record.data.DataJsonString)

                                for (const col of columns) {
                                    if (col.ColumnName == columnName) {
                                        if (colBeginDate <= currDate) {
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
                                        }

                                        return col.ShiftName + col.ExeResult;
                                    }
                                }
                            }
                            else {
                                return '';
                            }
                            
                        }
                    }
                    gridColumns.push(column);
                }
            }
        });
        grid.reconfigure(grid.store, gridColumns);
        grid.mon(grid, 'cellDblClick', me.cellDblClick, view);

    },
    onDataLoaded: function (view) {
        var me = this;
        var gridPanel = view.getControl();
        var grid = gridPanel.ownerGrid;
        me.showToWeekCell(grid);
    },
    // 计算当天列
    showToWeekCell: function (grid) {
        var currentDate = new Date();

        // 获取当前年份的第一天
        var firstDayOfYear = new Date(currentDate.getFullYear(), 0, 1);

        // 获取当前日期是本年度的第几天
        var dayOfYear = Math.floor((currentDate - firstDayOfYear) / (24 * 60 * 60 * 1000)) + 1;

        // 获取当前年份的第一天是星期几
        var firstDayOfWeek = firstDayOfYear.getDay(); // 0表示星期日，1表示星期一，依此类推

        // 计算当前日期是本年度的第几周
        var weekOfYear = Math.ceil((dayOfYear + firstDayOfWeek) / 7);
        var cellWidth = grid.getColumnManager().getColumns()[8].getWidth();
        var xcell = grid.getScrollX();
        var scrollLength = (weekOfYear - 1) * cellWidth;
        grid.scrollByDeltaX((scrollLength - xcell), true);
    },
    AddDays: function (date, days) {
        var nd = date;
        nd = nd.valueOf();
        nd = nd + days * 24 * 60 * 60 * 1000;
        nd = new Date(nd);
        var y = nd.getFullYear();
        var m = nd.getMonth() + 1;
        var d = nd.getDate();
        if (m <= 9) m = "0" + m;
        if (d <= 9) d = "0" + d;
        var cdate = y + "-" + m + "-" + d;
        return new Date(cdate);
    },
    getYearWeek: function () {
        var me = this;
        var weeks = 53;
        var dtNow = new Date();
        var dtBeginDate = new Date(dtNow.getFullYear() + "-01" + "-01");
        var dtEndDate = new Date(dtNow.getFullYear() + "-01" + "-01");
        for (var i = 1; i < weeks; i++) {
            dtEndDate = me.AddDays(dtBeginDate, 6);
            if (dtBeginDate <= dtNow && dtNow <= dtEndDate) {
                return i;
            }
            dtBeginDate = me.AddDays(dtEndDate, 1);
        }
        return 0;
    },
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
        //列字段名
        var clickedDataIndex = grid.getHeaderAtIndex(cellIndex).dataIndex;
        //当双击的字段不是日期对应的数值，跳出。（目前功能，非点检计划数据有以下字段）
        if (clickedDataIndex == 'EquipAccountCode' || clickedDataIndex == 'EquipAccountName' || clickedDataIndex == 'EquipModelName' || clickedDataIndex == 'EquipTypeName'
            || clickedDataIndex == 'WorkShopName' || clickedDataIndex == 'ResourceName' || clickedDataIndex == 'ProcessName')
            return;

        var date = clickedDataIndex.split("-");


        var n = (clickedDataIndex.split('-')).length - 1;
        //计划开始时间和结束时间
        var beginDate;
        var endDate;
        if (n == 1) {
            //计划开始时间和结束时间
            beginDate = new Date(date[0]);
            endDate = new Date(date[1]);
        } else {
            beginDate = new Date(date[0] + "/" + date[1] + "/" + date[2]);
            endDate = new Date(date[3] + "/" + date[4] + "/" + date[5]);
        }

        var equipId = record.data.EquipAccountId;

        //触发保养计划单选择逻辑
        me.selectMaintainPlan(equipId, beginDate, endDate, this);

    },

    /**
    * 保养计划单选择逻辑
    * @method selectMaintainPlan
    * @param  {string} equipId 设备Id
    * @param  {date} beginDate 保养开始时间
    * @param  {date} endDate 保养结束时间
    * @param  {ListlView} view 生成的view
    */
    selectMaintainPlan: function (equipId, beginDate, endDate, view) {
        SIE.AutoUI.getMeta({
            model: 'SIE.Web.EMS.EquipMaint.Maintains.Plans.ViewModels.SelDepartmentViewModel',
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
                    method: 'GetMaintainPlans',
                    params: [equipId, beginDate, endDate],
                    action: 'queryer',
                    type: 'SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer',
                    token: view.token,
                    success: function (res) {
                        if (res.Success) {
                            if (res.Result == null || res.Result.length == 0) {
                                SIE.MessageBox.showMessage("当前节点没有保养单数据".t());
                                return;
                            }

                            if (res.Result.length == 1) {
                                //当只有一张检验单，直接打开页签
                                if ((res.Result[0].State === 3 || res.Result[0].State === 5 || res.Result[0].State === 7) && res.Result[0].IfOpenConfirmationTab === true) {
                                    if (res.Result[0].IsConfirm) {
                                        //已评分或者待确认则打开确认界面
                                        me.showConfirmationView(res.Result[0], beginDate, endDate, view);
                                    }
                                    else {
                                        SIE.Msg.showMessage("当前登陆人设备与人员权限没有保养确认权限".t());
                                    }
                                } else {
                                    //其他状态则打开执行界面
                                    me.showExecutionView(res.Result[0], beginDate, endDate, view);
                                }
                            }
                            else {
                                //赋值界面数据
                                res.Result.forEach(function (item) {
                                    listView.getData().add(item);
                                });

                                //存在多张检验单，打开保养单选择页面
                                me.showMaintainPlanSelectionView(ui, listView, beginDate, endDate, view);
                            }
                        }
                    }
                })
            }
        });
    },

    /**
    * 弹出保养单选择界面
    * @method showMaintainPlanSelectionView
    * @param  {ctl} ui 界面UI控件
    * @param  {ListlView} view 保养单选择界面view
    * @param  {date} beginDate 保养开始时间
    * @param  {date} endDate 保养结束时间
    * @param  {ListlView} view 保养计划主界面view
    */
    showMaintainPlanSelectionView: function (ui, listView, beginDate, endDate, view) {
        var win = SIE.Window.show({
            title: "选择保养单".t(),
            width: 500,
            height: 500,
            items: ui,
            callback: function (btn) {
                if (btn == "确定".t()) {
                    var current = listView.getCurrent();
                    if (current == null)
                        SIE.Msg.showMessage("请选择保养单".t());

                    //依据执行状态打开对应的页签
                    if ((current.data.State === 3 || current.data.State === 5 || current.data.State === 7) && current.data.IfOpenConfirmationTab === true) {
                        if (current.data.IsConfirm) {
                            //已评分或者待确认则打开确认界面
                            me.showConfirmationView(current.data, beginDate, endDate, view);
                        }
                        else {
                            SIE.Msg.showMessage("当前登陆人设备与人员权限没有保养确认权限".t());
                        }
                    } else {
                        //其他状态则打开执行界面
                        me.showExecutionView(current.data, beginDate, endDate, view);
                    }

                    win.close()

                    //返回false，确定后不关闭窗口
                    return false;
                }
            }
        });
    },

    /**
    * 弹出“保养计划执行”页签效果
    * @method showExecutionView
    * @param  {SelDepartmentViewModel} data 部门选择VM
    * @param  {date} beginDate 保养开始时间
    * @param  {date} endDate 保养结束时间
    * @param  {ListlView} view 生成的view
    */
    showExecutionView: function (data, beginDate, endDate, view) {
        var me = this;
        var meta = null;
        //判断是否已打开对应的保养执行页签，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {
            me.isTabExist = true;
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        } else {
            me.isTabExist = false;
        }

        if (!this.isTabExist) {
            //初始化选中保养单数据，用以生成保养项目，查询上一次保养小结
            SIE.invokeDataQuery({
                method: 'InitExeMaintainPlan',
                params: [data.MaintainPlanId, data.EquipAccountId, data.DepartmentId],
                action: 'queryer',
                type: 'SIE.Web.EMS.EquipMaint.Maintains.Plans.DataQueryers.MaintainPlanQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        var precisePlanBeginDate = beginDate;
                        var precisePlanEndDate = endDate;

                        //判断指定日期是否为空
                        if (res.Result.PrecisePlanBeginDate != null && res.Result.PrecisePlanEndDate != null) {
                            precisePlanBeginDate = res.Result.PrecisePlanBeginDate;
                            precisePlanEndDate = res.Result.PrecisePlanEndDate;
                        }
                        CRT.Workbench.addPage({
                            entityType: "SIE.EMS.Maintains.Plans.MaintainPlan",
                            recordId: data.MaintainPlanId,
                            title: "保养执行".t(),
                            viewGroup: "PlanExecuteViewGroup",
                            module: view.module,
                            isDetail: true,
                            ignoreQuery: true,
                            params: {
                                PrecisePlanBeginDate: precisePlanBeginDate,
                                PrecisePlanEndDate: precisePlanEndDate,
                                UpMaintainSummary: res.Result.UpMaintainSummary,
                            },
                        });
                    }
                }
            });
        }
    },

    /**
    * 弹出“保养计划确认”页签效果
    * @method showConfirmationView
    * @param  {SelDepartmentViewModel} data 部门选择VM
    * @param  {date} beginDate 保养开始时间
    * @param  {date} endDate 保养结束时间
    * @param  {ListlView} view 生成的view
    */
    showConfirmationView: function (data, beginDate, endDate, view) {
        var me = this;
        var meta = null;
        //判断是否已打开对应的保养确认页签，如果已打开，则不重新打开，避免重复事件绑定处理
        if (me.tab) {
            me.isTabExist = true;
            var tabPanel = portal.getTabPanel();
            tabPanel.setActiveTab(me.tab);
            return;
        } else {
            me.isTabExist = false;
        }

        if (!this.isTabExist) {
            //打开“保养确认”明细页签
            SIE.invokeDataQuery({
                method: 'InitConfirmMaintainPlan',
                params: [data.MaintainPlanId, data.MaintainPlanNo, data.EquipAccountId, data.DepartmentId],
                action: 'queryer',
                type: 'SIE.Web.EMS.EquipMaint.Maintains.Confirmations.DataQuery.MaintainConfirmationQueryer',
                token: view.token,
                success: function (res) {
                    if (res.Success) {
                        CRT.Workbench.addPage({
                            entityType: "SIE.EMS.Maintains.Plans.MaintainPlan",
                            recordId: data.MaintainPlanId,
                            title: "保养确认".t(),
                            module: view.module,
                            viewGroup: "PlanConfirmViewGroup",
                            isDetail: true,
                            ignoreQuery: true,
                            token: view.token,
                            params: {
                                RecordId: data.MaintainPlanId,
                                MaintainPlanNo: data.MaintainPlanNo,
                                DepartmentId: data.DepartmentId,
                                DepartmentName: data.DepartmentName,
                                EquipAccountId: data.EquipAccountId,
                                UpMaintainSummary: res.Result.UpMaintainSummary,
                                ConfirmResult: res.Result.ConfirmResult,
                                ConfirmNote: res.Result.ConfirmNote,
                                PrecisePlanBeginDate: res.Result.PrecisePlanBeginDate,
                                PrecisePlanEndDate: res.Result.PrecisePlanEndDate,
                                MaintianExeState: res.Result.MaintianExeState,
                            }
                        });
                    }
                }
            })
        }
    }

});