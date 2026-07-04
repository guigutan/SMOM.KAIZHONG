Ext.define("SIE.Web.MES.ProjectDesigns.Reports.Scripts.ReportBehavior", {
    beforeCreate: function (view, entity) {
        var gridConfig = view.gridConfig;
        gridConfig.columns.forEach(function (columnConfig) {
            if (columnConfig.dataIndex === "ProjectMaintain") {
                var config = {
                    renderer: function (value, cellmeta, record, rowIndex, colIndex, store, view) {
                        cellmeta.style = "color:blue";
                        return value;
                    }
                };
                Ext.merge(columnConfig, config);
            }
            
        })
    },
    onViewReady: function (view) {
        var me = this;
        var gridPanel = view.getControl();
        var grid = gridPanel.ownerGrid;
        grid.mon(grid, 'cellDblClick', me.cellDblClick, view);
    },

    /**
    * 界面双击事件
    * @method cellDblClick
    * @param 
    */
    cellDblClick: function (grid, td, cellIndex, record, tr, rowIndex, e, eOpts) {
        //列字段名
        var clickedDataIndex = grid.getHeaderAtIndex(cellIndex).dataIndex;
        if (clickedDataIndex == "ProjectMaintain") {
            var recordId = Ext.Number.parseFloat(record.getId());
            CRT.Workbench.addPage({
                entityType: "SIE.MES.ProjectDesigns.ProjectDesignDetail",
                title: "查看-项目号需求设计".t(),
                viewGroup: "LookUpViewGroup",
                module: "SIE.MES.ProjectDesigns.ProjectDesign,SIE.MES",
                recordId: recordId,
                isDetail: true,
                ignoreQuery: true,
            })
        }
    }
})