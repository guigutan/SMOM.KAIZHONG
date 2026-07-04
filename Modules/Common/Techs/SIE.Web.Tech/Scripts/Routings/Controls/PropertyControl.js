/**
 * 工序属性控件
 * @class SIE.Tech.PropertyControl
 * @constructor
 */
Ext.define('SIE.Tech.PropertyControl', {
    extend: 'Ext.form.Panel',
    xtype: 'techProperty',
    autoScroll: true,
    fieldDefaults: {
        labelAlign: 'right',
        labelWidth: 80
    },
    defaults: {
        anchor: '100%',
        style: 'border-bottom: 1px solid #d9d9d9;padding:5px;margin-bottom:0px'
    },

    viewModel: {},
    items: [],
    /**
     * 父主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 重置属性控件
     * @method resetPropertyControl
     * @for SIE.Tech.PropertyControl
     */
    resetPropertyControl: function () {
        var me = this;
        me.hideNodePropertyData();
        SIE.Web.Tech.Common.Routings.PropertyExt.properties.forEach(function (property) {
            property.resetControl(me);
        });
    },

    /**
     * 隐藏节点属性窗口的项
     * @method hideNodePropertyData
     * @for SIE.Tech.PropertyControl
     */
    hideNodePropertyData: function () {
        var me = this;
        me.getViewModel().setData({ m: null });
        me.items.items.forEach(function (item) {
            me.setVisible(item, false);
        });
    },

    /**
     * 工序节点选中事件
     * @method nodeChanged
     * @for SIE.Tech.PropertyControl
     * @param {Object} node 工序节点信息
     * @param {回调} disableCallback 工序属性是否可编辑
     */
    nodeChanged: function (node, isDisable) {
        var me = this;
        //更新属性 
        SIE.Web.Tech.Common.Routings.PropertyExt.properties.forEach(function (property) {
            property.loadData(me, node, isDisable);
        });
    }, 

    /**
     * 设置属性控件是否显示
     * @method setVisible
     * @for SIE.Tech.PropertyControl
     * @param {Ext.form.field.Display} property 字段控件
     * @param {bool} isVisible 是否显示
     */
    setVisible: function (property, isVisible) {
        if (isVisible)
            property.show();
        else
            property.hide();
    },

    /**
      * 设置属性控件是否可编辑
      * @method setDisable
      * @for SIE.Tech.PropertyControl
      * @param {Ext.form.field.Display} property 字段控件
      * @param {bool} isDisable 是否可编辑
      */
    setDisable: function (property, isDisable) {
        property.setDisabled(isDisable);
    },

    /**
      * 判断是否批次工序
      * @method isBatchProcess
      * @for SIE.Tech.PropertyControl 
      * @param {String} processType 工序类型
      * @returns {bool} 批次工序返回true，否则返回false
      */
    isBatchProcess: function (processType) {
        return processType === 'BatchAssembly' || processType === 'BatchPqc' || processType === 'BatchFix' || processType === 'BatchPacking';
    }
});