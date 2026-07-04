/**
* 借调明细流程
*/
Ext.define('SIE.Web.MES.TeamManagement.Editors.FlowEditor', {
    extend: 'Ext.form.FieldContainer',
    alias: 'widget.flowEditor',
    layout: {
        type: 'hbox',
    },

    style: 'margin: 50px 0px 0px 0px; overflow-x: auto; overflow-y: hidden;',

    /**
     * 界面绘制前处理方法
     * @param {}
     * @returns {}
    */
    beforeRender: function () {
        this.callParent();
        var me = this;
        var form = this.up('form');
        if (form) {
            var view = form.SIEView;
            this.mon(view, 'dataChanged', this._currentChanged, this);
        }
    },

    /**
     * 数据变更处理事件
     * @param {} currentData
     * @returns {}
     */
    _currentChanged: function (currentData) {
        if (currentData) {
            var detail = currentData.value.data;
            var onLoanId = detail.OnLoanId;
            if (onLoanId != null) {
                this._initFlow(onLoanId);
            } else {
                this._clearFlow();
            }
        }
    },

    /**
    * 生成借调明细的流程图
    * @param {} onLoanId
    * @returns {}
    */
    _initFlow: function (onLoanId) {
        var me = this;
        me._clearFlow();
        SIE.invokeDataQuery({
            method: 'GetOnLoanDetails',
            params: [onLoanId],
            action: 'queryer',
            type: 'SIE.Web.MES.TeamManagement.OnLoans.OnLoanDetailDataQuery',
            token: me.up().SIEView.token,
            success: function (res) {
                details = res.Result.data.items;
                var len = details.length;
                if (len >= 1) {
                    var flowArrays = []; ////new Array();
                    me._addFlowImage("/images/MesImages/Begin.png", flowArrays); ////"url('/images/drawtools/dot_bg.jpg')"
                    for (var index = 0; index < len; index++) {
                        var curDetail = details[index].data;
                        if (curDetail.Id != null) {
                            me._addFlowItem(curDetail, len, flowArrays);
                        }
                    }
                    me._addFlowImage("/images/MesImages/End.png", flowArrays);

                    me.add(flowArrays);
                    flowArrays = null;
                }
            }
        });
    },

    /**
     * 清空借调明细流程图
     * @param {}
     * @returns {}
    */
    _clearFlow: function () {
        this.removeAll();
    },

    /**
    * 生成借调明细的流程单元块组(箭头+流程单元块+箭头)
    * @param {} curDetail, len, flowArrays
    * @returns {} 
    */
    _addFlowItem: function (curDetail, len, flowArrays) {
        var me = this;
        if (curDetail.RowIndex == len && (curDetail.State == 5 || curDetail.State == 6)) {
            me._addFlowImage("/images/MesImages/FlowBlue.png", flowArrays);
            me._addOnLoanDetail(curDetail, len, flowArrays);
        }
        else if (curDetail.State != 5 && curDetail.State != 6) {
            me._addFlowImage("/images/MesImages/FlowGray.png", flowArrays);
            me._addOnLoanDetail(curDetail, len, flowArrays);
        };

        if (curDetail.RowIndex == len) {
            //if (curDetail.State == 1 || curDetail.State == 4) {
            //    me._addFlowImage("/images/MesImages/FlowGray.png", flowArrays);
            //}
            me._addFlowImage("/images/MesImages/FlowGray.png", flowArrays);
        }
    },

    /**
    * 生成借调明细的流程单元块(只生成流程单元块)
    * @param {} curDetail, len, flowArrays
    * @returns {}
    */
    _addOnLoanDetail: function (curDetail, len, flowArrays) {
        var me = this;
        me.setHtml('');
        var curHeaderBackColor = me._getHeaderColorValue(curDetail, len);
        var state = me._getApprovalState(curDetail.State);
        var operator = curDetail.OperatorName;
        var remark = '';
        var operateDate = '';
        if (curDetail.State != 5 && curDetail.State != 6) {
            remark = curDetail.Remark;
            operateDate = curDetail.OperateDate.toLocaleDateString() + " " + curDetail.OperateDate.toTimeString().substr(0, 8);
        }
        curPanel = Ext.create({
            xtype: 'panel',
            title: state,
            titleAlign: 'center',
            style: 'border: 1px solid #bfbfbf; border-bottom-left-radius: 15px; border-bottom-right-radius: 15px',
            bodyStyle: 'border-width: 1px 1px 1px 1px; padding:5px 5px 5px 5px',
            header: {
                style: "background-color:" + curHeaderBackColor,
                border: false
            },
            html: Ext.String.format('{0}<br/>{1}<br/>{2}', operator, remark, operateDate),
            autoHeight: true,
            width: 150,
            align: 'center',
            margin: '10 10 10 10'
        });
        //me.add(curPanel);
        flowArrays.push(curPanel);
    },

    /**
     * 获取流程单元块的Header背景色
     * @param {} curDetail,len
     * @returns {} curColor
    */
    _getHeaderColorValue: function (curDetail, len) {
        var grayColor = "#bfbfbf";//淡灰色
        var blueColor = "#37af08";//浅绿色
        var curColor = grayColor;
        if (curDetail.RowIndex == len && (curDetail.State == 5 || curDetail.State == 6))
            curColor = blueColor;
        else
            curColor = grayColor;
        return curColor;
    },

    /**
     * 添加流程图的图片(开始、结束、箭头)
     * @param {} curSrc
     * @returns {}
     */
    _addFlowImage: function (curSrc, flowArrays) {
        var me = this;
        var curFlowImage = Ext.create({
            xtype: 'component', //'component'或者'box'
            width: 50,
            height: 50,
            align: 'center',
            margin: '40 10 10 10',
            autoEl: {
                tag: 'img',
                src: curSrc
            }
        });
        //me.add(curFlowImage);
        flowArrays.push(curFlowImage);
    },

    /**
     *获取审核状态Label
     * @param {} approvalState
     * @returns {} stateLabel
     */
    _getApprovalState: function (approvalState) {
        var stateLabel = "";
        switch (approvalState) {
            case 0:
                stateLabel = "发起".t();
                break;
            case 1:
                stateLabel = "同意".t();
                break;
            case 2:
                stateLabel = "拒绝".t();
                break;
            case 3:
                stateLabel = "修改".t();
                break;
            case 4:
                stateLabel = "撤销".t();
                break;
            case 5:
                stateLabel = "审核中".t();
                break;
            case 6:
                stateLabel = "修改中".t();
                break;
        }
        return stateLabel;
    },

});