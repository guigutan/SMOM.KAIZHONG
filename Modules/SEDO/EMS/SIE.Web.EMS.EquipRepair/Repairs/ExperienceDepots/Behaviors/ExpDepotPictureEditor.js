Ext.define('SIE.Web.EMS.EquipRepair.Repairs.ExperienceDepots.Behaviors.ExpDepotPictureEditor', {
    extend: 'Ext.form.FieldContainer',
    id: 'expDepotPictueFieldContainer',
    alias: 'widget.expDepotPictureEditor',
    layout: 'vbox',
    _imageDom: null,
    items: [
        {
            xtype: 'image',
            id: 'expDepotPicture',
            beforeRender: function () {
                this.src = "";
                this.width = 300;
                this.height = 300;
            },
            listeners: {
                el: {
                    click: 'onClick'
                }
            },
            onClick: function () {
            }
        }
    ],
});