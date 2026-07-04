/**
 * 工艺路线设计控件
 * @class SIE.Tech.RoutingDesignControl
 * @constructor
 */
Ext.define('SIE.Tech.RoutingDesignControl', {
    extend: 'Ext.panel.Panel',
    xtype: 'techRoutingDesign',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 0,
    defaults: {
        bodyPadding: 0
    },
    region: 'center',
    border: 0,
    items: [{
        title: null,
        padding: 5,
        border: 0,
        items: [{
            xtype: 'panel',
            bodyPadding: 0,
            border: 0,
            html: '<div><B>' + '工艺路线'.L10N() + ':</B><span id="spTitle"></span></div>'
        }, {
            xtype: 'toolbar',
            border: 0,
            margin: '5 0 0 0 ',
            items: []

        }]
    }, {
        title: null,
        autoScroll: true,
        flex: 1,
        bodyStyle: {
            backgroundImage: "url('/images/drawtools/dot_bg.jpg')",
            backgroundRepeat: 'repeat'
        },
        html: '<div style="position:absolute; width:100%; height:100%" id = "techCanvas"></div>'
    }],
    listeners: {
        //选中节点变更事件
        nodeChanged: function (designControl, node) {
        },
        render: function (scop, eOpts) {
            var me = this;
            me.createDesignCanvas();  //必须在控件初始化后执行，不然找不到画布
            var commands = me.items.items[0].items.items[1].items.items;
            me.commands = commands;
        }
    },
    /**
     * 保存工艺路线
     * @property {SIE.Web.Tech.Routings.Commands.SaveRoutingCommand} saveCommand
     */
    saveCommand: null,

    /**
     * 发布工艺路线
     * @property {SIE.Web.Tech.Routings.Commands.PublishCommand} publishCommand
     */
    publishCommand: null,

    /*
     *左右对齐
     * @property {SIE.Web.Tech.Routings.Commands.LeftRightCommand} leftRightCommand
     * */
    leftRightCommand: null,

    /*
     *上下对齐
     */
    upDownCommand: null,

    /*
     *横向分布
     */
    horizontalDistributionCommand: null,

    /*
     * 纵向分布
     */
    verticalDistributionCommand: null,
    /*
     * 放大
     */
    zoomInCommand: null,
    /*
     *缩小
     */
    zoomOutCommand: null,

    /*
     * 还原
     */
    zoomOriginalCommand: null,


    /**
     * 父主视图
     * @property {ListLogicalView} mainView
     */
    mainView: null,

    /**
     * 控件是否已加载
     * @property {bool} isLoaded
     */
    isLoaded: false,

    /**
     * 设计画布
     * @property {DesignCanvas} designCanvas
     */
    designCanvas: null,  //设计界面

    /**
     * 命令集合
     * @property {Array} commands
     */
    commands: [],

    /**
     * 当前选中工艺路线
     * @property {SIE.Tech.Routings.Routing} routing
     */
    routing: null,

    /**
     * 控件初始化
     * @method initComponent
     * @for SIE.Tech.RoutingDesignControl
     */
    initComponent: function () {
        var me = this;
        me.initCommands();
        var config = {
            isReadOnly: true
        };
        me.designCanvas = new DesignCanvas(me.mainView, 'techCanvas', config);  //先初始化，这样外部就可以挂nodeChanged事件
        me.designCanvas.setRoutingDesignControrl(me);
        this.callParent();
    },

    /**
     * 创建设计画布
     * @method createDesignCanvas
     * @for SIE.Tech.RoutingDesignControl
     */
    createDesignCanvas: function () {
        var me = this;
        me.designCanvas.InitDrawViewControl();
    },

    /**
     * 设置工艺路线导航
     * @method setNavigation
     * @for SIE.Tech.RoutingDesignControl
     * @param {SIE.Tech.Routings.Routing} routing 工艺路线
     */
    setNavigation: function (routing) {
        var title = '';
        if (routing !== null)
            title = routing.parentNode.parentNode.get('text') + '/' + routing.parentNode.get('text') + '/' + routing.get('text');
        Ext.query('#spTitle')[0].innerHTML = title;
    },

    /**
     * 初始化命令
     * @method initCommands
     * @for SIE.Tech.RoutingDesignControl
     */
    initCommands: function () {
        var me = this;
        if (me.mainView === null || me.mainView === undefined)
            throw "主视图不能为空".L10N();
        if (me.isLoaded)
            return;
        me.createCommands();
        var commands = me.items[0].items[1].items;
        var publishCommand = commands.first(function (p) { return p.command === 'PublishCommand'; });
        if (publishCommand)
            commands.remove(publishCommand);
        var saveCommand = commands.first(function (p) { return p.command === 'SaveCommand'; });
        if (saveCommand)
            commands.remove(saveCommand);
        //左右对齐
        var leftRightCommand = commands.first(function (p) { return p.command === 'LeftRightCommand'; });
        if (leftRightCommand)
            commands.remove(LeftRightCommand);

        //横向布均
        var upDownCommand = commands.first(function (p) { return p.command === 'UpDownCommand'; });
        if (upDownCommand)
            commands.remove(upDownCommand);

        //横向布均
        var horizontalDistributionCommand = commands.first(function (p) { return p.command === 'HorizontalDistributionCommand'; });
        if (horizontalDistributionCommand)
            commands.remove(horizontalDistributionCommand);

        //纵向分布
        var verticalDistributionCommand = commands.first(function (p) { return p.command === 'VerticalDistributionCommand'; });
        if (verticalDistributionCommand)
            commands.remove(verticalDistributionCommand);

        //放大
        var zoomInCommand = commands.first(function (p) { return p.command === 'ZoomInCommand'; });
        if (zoomInCommand)
            commands.remove(zoomInCommand);

        //缩小
        var zoomOutCommand = commands.first(function (p) { return p.command === 'ZoomOutCommand'; });
        if (zoomOutCommand)
            commands.remove(zoomOutCommand);

        //还原
        var zoomOriginalCommand = commands.first(function (p) { return p.command === 'ZoomOriginalCommand'; });
        if (zoomOriginalCommand)
            commands.remove(zoomOriginalCommand);

        //缩放命令
        commands.unshift(me.getCommandConfig('ZoomOriginalCommand', me.zoomOriginalCommand));
        commands.unshift(me.getCommandConfig('ZoomOutCommand', me.zoomOutCommand));
        commands.unshift(me.getCommandConfig('ZoomInCommand', me.zoomInCommand));
        //布均命令
        commands.unshift(me.getCommandConfig('VerticalDistributionCommand', me.verticalDistributionCommand));
        commands.unshift(me.getCommandConfig('HorizontalDistributionCommand', me.horizontalDistributionCommand));
        commands.unshift(me.getCommandConfig('UpDownCommand', me.upDownCommand));
        commands.unshift(me.getCommandConfig('LeftRightCommand', me.leftRightCommand));

        

        commands.unshift(me.getCommandConfig('PublishCommand', me.publishCommand));
        commands.unshift(me.getCommandConfig('SaveCommand', me.saveCommand));

        me.isLoaded = true;
    },

    /**
     * 创建命令
     * @method createCommands
     * @for SIE.Tech.RoutingDesignControl
     */
    createCommands: function () {
        var me = this;
        me.saveCommand = Ext.create('SIE.Web.Tech.Routings.Commands.SaveRoutingCommand');
        me.publishCommand = Ext.create('SIE.Web.Tech.Routings.Commands.PublishCommand');

        //创建布局命令
        me.leftRightCommand = Ext.create('SIE.Web.Tech.Routings.Commands.LeftRightCommand');
        me.upDownCommand = Ext.create('SIE.Web.Tech.Routings.Commands.UpDownCommand');
        me.verticalDistributionCommand = Ext.create('SIE.Web.Tech.Routings.Commands.VerticalDistributionCommand');
        me.horizontalDistributionCommand = Ext.create('SIE.Web.Tech.Routings.Commands.HorizontalDistributionCommand');

        me.leftRightCommand._ownerView = me.mainView;
        me.upDownCommand._ownerView = me.mainView;
        me.horizontalDistributionCommand._ownerView = me.mainView;
        me.verticalDistributionCommand._ownerView = me.mainView;

        me.zoomInCommand = Ext.create('SIE.Web.Tech.Routings.Commands.ZoomInCommand');
        me.zoomOutCommand = Ext.create('SIE.Web.Tech.Routings.Commands.ZoomOutCommand');
        me.zoomOriginalCommand = Ext.create('SIE.Web.Tech.Routings.Commands.ZoomOriginalCommand');

        me.zoomInCommand._ownerView = me.mainView;
        me.zoomOutCommand._ownerView = me.mainView;
        me.zoomOriginalCommand._ownerView = me.mainView;

        me.saveCommand._ownerView = me.mainView;
        me.publishCommand._ownerView = me.mainView;
    },

    /**
     * 获取命令配置信息
     * @method getCommandConfig
     * @for SIE.Tech.RoutingDesignControl
     * @param {string} name 命令名称
     * @param {JSCommand} command 命令
     * @returns {{}} 命令配置
     */
    getCommandConfig: function (name, command) {
        var me = this;
        return {
            xtype: 'button',
            command: name,
            text: command.config.meta.text,
            tooltipType: "title",
            tooltip: command.config.meta.tooltip,
            disabled: true,
            handler: function () {
                command.tryExecute(me);
            }
        };
    },

    /**
     * 设置工艺路线设计控件命令状态
     * @method setCommandState
     * @for SIE.Tech.RoutingDesignControl
     */
    setCommandState: function () {
        var me = this;
        if (me.routing && me.routing.get('state') === 0) {   //0保存            
            me.setCommandDisabled(false);
        }
        else {
            me.setCommandDisabled(true);
        }
    },

    /**
     * 设置命令是否可用
     * @param {any} flag true/false
     */
    setCommandDisabled: function (flag) {
        var me = this;
        me.designCanvas.setLock(flag);
        me.setMainBlockCommandDisabled('SaveCommand', flag);
        me.setMainBlockCommandDisabled('PublishCommand', flag);
        me.setMainBlockCommandDisabled('LeftRightCommand', true);
        me.setMainBlockCommandDisabled('UpDownCommand', true);
        me.setMainBlockCommandDisabled('VerticalDistributionCommand', true);
        me.setMainBlockCommandDisabled('HorizontalDistributionCommand', true);

        me.setMainBlockCommandDisabled('ZoomInCommand', false);
        me.setMainBlockCommandDisabled('ZoomOutCommand', false);
        me.setMainBlockCommandDisabled('ZoomOriginalCommand', false);

    },

    /**
     * 设置工艺路线设计控件命令状态
     * @method setMainBlockCommandDisabled
     * @for SIE.Tech.RoutingDesignControl
     * @param {string} command 命令名称
     * @param {Boolean} isDisabled 是否可编辑
     */
    setMainBlockCommandDisabled: function (command, isDisabled) {
        var me = this;
        me.commands.forEach(function (cmd) {
            if (cmd.command === command)
                cmd.setDisabled(isDisabled);
        });
    },

    /**
     * 生成工艺路线控件
     * @method drawRouting
     * @for SIE.Tech.RoutingDesignControl
     * @param {SIE.Tech.Routings.Routing} routing 工艺路线
     */
    drawRouting: function (routing) {
        var me = this;
        me.routing = routing;
        me.resetMainBlock();
        me.setNavigation(routing);
        me.setCommandState();
        var token = me.mainView.token;
        SIE.invokeDataQuery({
            type: "SIE.Web.Tech.Routings.TechDataQueryer",
            method: "GetRoutingLayout",
            token: token,
            params: [routing.get('layoutId')],
            success: function (res) {
                if (!res.Success)
                    return;
                me.designCanvas.clearDrawControl();
                me.designCanvas.drawRouting(null);
                me.designCanvas.drawRouting(res.Result);
            }
        });
    },

    /**
     * 重置画布
     * @method resetMainBlock
     * @for SIE.Tech.RoutingDesignControl
     */
    resetMainBlock: function () {
        var me = this;
        //清除画布内容
        me.designCanvas.clearDrawControl();
        //禁用画布命令按钮 
        for (var i = 0; i < me.commands.length; i++) {
            me.commands[i].setDisabled(true);
        }
    }
});