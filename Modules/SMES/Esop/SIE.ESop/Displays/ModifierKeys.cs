using System;
using SIE.ObjectModel;

namespace SIE.ESop.Displays
{
    //
    // 摘要:
    //     Specifies the set of modifier keys.
    [Flags]
    public enum ModifierKeys
    {
        //
        // 摘要:
        //     No modifiers are pressed.
        [Label("None")]
        None = 0,
        //
        // 摘要:
        //     The ALT key.
        [Label("Alt")]
        Alt = 1,
        //
        // 摘要:
        //     The CTRL key.
        [Label("Control")]
        Control = 2,
        //
        // 摘要:
        //     The SHIFT key.
        [Label("Shift")]
        Shift = 4,
        //
        // 摘要:
        //     The Windows logo key.
        [Label("Windows")]
        Windows = 8
    }
}
