Ext.define('SIE.Web.EMS.Checks.Records.Scripts.CheckRecordBehavior', {
    isTabExist: false, //填写报告页签是否已打开
    tab: null,  //填写报告页签
    /*
     * view生命周期函数--view生成前
     * @param {*} meta 实体视图元数据
     * @param {*} curEntity 当前操作实体(可空)
     */
    beforeCreate: function (meta, curEntity) {
        var me = this;
        if (!meta) return;
        if (meta.model != 'SIE.EMS.Checks.Records.CheckRecord') return;
        var gridConfig = meta.gridConfig;

        //meta.storeConfig.pageSize = 99999;//目前是超过1w行不分页   
        //gridConfig.columns.splice(0, 0, { xtype: 'rownumberer' });  //添加序号列     

        var render = {
            renderer: function (value, meta, record, rowIndex, colIndex, store, view) {
                if (value == 0) {
                    //未执行
                    meta.tdStyle = "border-right: 1px solid white; background: yellow;";
                } else if (value == 2) {
                    //超期
                    meta.tdStyle = "border-right: 1px solid white; background: red;";
                }
                return record.data.ExeStateName.t();
            }

        };

        //移除分页
        //gridConfig.dockedItems[0] = null;

        //背景描述
        gridConfig.dockedItems[1].items.push({
            xtype: 'tbtext',
            style: 'background: yellow; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '未执行'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: red; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '超期'.t()
        });

        //设置背景色
        gridConfig.columns.forEach(function (e) {
            //执行状态的列要设置背景色
            if (e.dataIndex === "ExeState") {
                me.compatibleWithIE();
                e = Object.assign(e, render);
            }
        })

    },

    /**
     * view生命周期函数--view生成后
     * @param {DetailView} view 生成的view
     */
    onCreated: function (view) {
        var me = this;
        var mainView = view;
    },

    /**
     * 视图关联后方法
     * @method onViewReady
     * @param {ListLogicView} view 产品族视图
     */
    onViewReady: function (view) {
        var me = this;
        //后台无法ReplaceCommand，手动移除原查询按钮
        //var cmds = view._relations[0]._target.formConfig.tbar;
        //var qryCmd = cmds.filter(function (e) {
        //    return e.command === 'SIE.cmd.ExecuteQuery';
        //});
        //if (qryCmd.length > 0)
        //    cmds.splice(cmds.indexOf(qryCmd[0]), 1);
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
    onShow: function (view) {
        var params = CRT.Context.PageContext.getParams();
        if (params) {
            //获取查询视图
            var conditionView = view.getConditionView();
            //获取查询实体元数据
            var criteria = conditionView.getData();
            //赋值传递过来的保养单号
            criteria.setCheckPlanNo(params.CheckPlanNo);
            //清空所有时间范围控件的开始结束时间
            var dateRangeCtls = conditionView.getControl().items.items.filter(function (e) { return e.xtype === "dateRange"; })
            if (dateRangeCtls.length > 0) {
                dateRangeCtls.forEach(function (ctl) {
                    ctl.setDataRangValue(null, null);
                });
            }
            //执行查询
            conditionView.tryExecuteQuery();
        }
    }

});