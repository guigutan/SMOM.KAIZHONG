SIE.defineCommand('SIE.Web.MES.TeamManagement.ScoreRecords.AchieveLevelSetEditCommand', {
    extend: 'SIE.cmd.Edit',
    meta: { text: "修改".t(), group: "edit", iconCls: "icon-EditEntity icon-blue" },

    /*
     * 编辑的处理方法
     */
    onEditting: function (entity) {
        if (entity) {
            this.mon(entity, 'propertyChanged', this._onEntityPropertyChanged, this);
        }
    },

    /*
     * 数据变更处理事件
     * param {} e
     * return {}
     */
    _onEntityPropertyChanged: function (e) {
        if (e.property.length > 0) {
            var data = e.entity.data;
            var preData = e.entity.previousValues;
            var curLevelLabel = this._getAchieveLevel(data.AchiLevel);
            var curPreLevelLabel = this._getAchieveLevel(data.AchiLevel - 1);
            var curNextLevelLabel = this._getAchieveLevel(data.AchiLevel + 1);
            var dataItems = this.view.getData().data.items;
            var curMsg = '';
            if (e.property == 'MinValue' && data.MinValue != null) {
                debugger;
                if (data.AchiLevel == 3) {
                    curMsg = Ext.String.format('[{0}]的最小值必须为空!'.t(), curLevelLabel);
                    dataItems[data.RowIndex - 1].setMinValue(preData.MinValue);
                }
                else if (data.MinValue <= dataItems[data.RowIndex].getMaxValue()) {
                    curMsg = Ext.String.format('[{0}]的最小值必须大于[{1}]的最大值!'.t(), curLevelLabel, curNextLevelLabel);
                    dataItems[data.RowIndex - 1].setMinValue(preData.MinValue);
                }
                else if (data.MinValue > data.MaxValue && data.MaxValue != null) {
                    curMsg = "最小值必须 <= 最大值!".t();
                    dataItems[data.RowIndex - 1].setMinValue(preData.MinValue);
                }
            }
            else if (e.property == 'MaxValue' && data.MaxValue != null) {
                if (data.AchiLevel == 0) {
                    curMsg =  Ext.String.format('[{0}]的最大值必须为空!'.t(), curLevelLabel);
                    dataItems[data.RowIndex - 1].setMaxValue(preData.MaxValue);
                }
                else if (data.MaxValue >= dataItems[data.RowIndex - 1 - 1].getMinValue()) {
                    curMsg = Ext.String.format('[{0}]的最大值必须小于[{1}]的最小值!'.t(), curLevelLabel, curPreLevelLabel);
                    dataItems[data.RowIndex - 1].setMaxValue(preData.MaxValue);
                }
                else if (data.MinValue > data.MaxValue && data.MinValue != null) {
                    curMsg = "最小值必须<=最大值!".t();
                    dataItems[data.RowIndex - 1].setMaxValue(preData.MaxValue);
                }
            }
            if (curMsg !== '') {
                Ext.Msg.show({ title: '错误'.t(), message: curMsg, animateTarget: 'AchieveLevelSetEditCommand' });
                curMsg = '';
            }
        }
    },

    /*
     * @获取绩效等级的Label
     * @param {} achiLevel
     * @returns levelLabel
     */
    _getAchieveLevel: function (achiLevel) {
        var levelLabel = "";
        switch (achiLevel) {
            case 0:
                levelLabel = "优秀".t();
                break;
            case 1:
                levelLabel = "良好".t();
                break;
            case 2:
                levelLabel = "及格".t();
                break;
            case 3:
                levelLabel = "不良".t();
                break;
        }
        return levelLabel;
    },

});