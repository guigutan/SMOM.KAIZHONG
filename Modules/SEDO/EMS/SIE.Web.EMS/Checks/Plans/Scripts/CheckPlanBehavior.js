Ext.define('SIE.Web.EMS.Checks.Plans.Scripts.CheckPlanBehavior', {
    isTabExist: false, //填写报告页签是否已打开
    tab: null,  //填写报告页签
    /*
     * view生命周期函数--view生成前
     * @param {*} meta 实体视图元数据
     * @param {*} curEntity 当前操作实体(可空)
     */
    beforeCreate: function (meta, curEntity) {

        if (!meta) {
            return;
        }

        if (meta.model != 'SIE.EMS.Checks.Plans.ViewModels.CheckPlanViewModel') {
            return;
        }

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
            text: '点检待确认'.t()
        }, {
            xtype: 'tbtext',
            style: 'background: red; padding: 3px 5px 3px 5px; margin: 0px 0px 0px 10px;font-weight: bolder;',
            text: '超期'.t()
        });

        //meta.gridConfig.dockedItems[0] = null;  //移除分页
    },


});