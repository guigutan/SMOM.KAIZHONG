using SIE.ObjectModel;

namespace SIE.ESop.Displays
{
    public enum Key
    {
        //
        // 摘要:
        //     No key pressed.
        [Label("None")]
        None = 0,
        //
        // 摘要:
        //     The Cancel key.
        [Label("Cancel")]
        Cancel = 1,
        //
        // 摘要:
        //     The Backspace key.
        [Label("Back")]
        Back = 2,
        //
        // 摘要:
        //     The Tab key.
        [Label("Tab")]
        Tab = 3,
        //
        // 摘要:
        //     The Linefeed key.
        [Label("LineFeed")]
        LineFeed = 4,
        //
        // 摘要:
        //     The Clear key.
        [Label("Clear")]
        Clear = 5,
        //
        // 摘要:
        //     The Return key.
        [Label("Return")]
        Return = 6,
        //
        // 摘要:
        //     The Enter key.
        [Label("Enter")]
        Enter = 6,
        //
        // 摘要:
        //     The Pause key.
        [Label("Pause")]
        Pause = 7,
        //
        // 摘要:
        //     The Caps Lock key.
        [Label("Capital")]
        Capital = 8,
        //
        // 摘要:
        //     The Caps Lock key.
        [Label("CapsLock")]
        CapsLock = 8,
        //
        // 摘要:
        //     The IME Kana mode key.
        [Label("KanaMode")]
        KanaMode = 9,
        //
        // 摘要:
        //     The IME Hangul mode key.
        [Label("HangulMode")]
        HangulMode = 9,
        //
        // 摘要:
        //     The IME Junja mode key.
        [Label("JunjaMode")]
        JunjaMode = 10,
        //
        // 摘要:
        //     The IME Final mode key.
        [Label("FinalMode")]
        FinalMode = 11,
        //
        // 摘要:
        //     The IME Hanja mode key.
        [Label("HanjaMode")]
        HanjaMode = 12,
        //
        // 摘要:
        //     The IME Kanji mode key.
        [Label("KanjiMode")]
        KanjiMode = 12,
        //
        // 摘要:
        //     The ESC key.
        [Label("Escape")]
        Escape = 13,
        //
        // 摘要:
        //     The IME Convert key.
        [Label("ImeConvert")]
        ImeConvert = 14,
        //
        // 摘要:
        //     The IME NonConvert key.
        [Label("ImeNonConvert")]
        ImeNonConvert = 15,
        //
        // 摘要:
        //     The IME Accept key.
        [Label("ImeAccept")]
        ImeAccept = 16,
        //
        // 摘要:
        //     The IME Mode change request.
        [Label("ImeModeChange")]
        ImeModeChange = 17,
        //
        // 摘要:
        //     The Spacebar key.
        [Label("Space")]
        Space = 18,
        //
        // 摘要:
        //     The Page Up key.
        [Label("Prior")]
        Prior = 19,
        //
        // 摘要:
        //     The Page Up key.
        [Label("PageUp")]
        PageUp = 19,
        //
        // 摘要:
        //     The Page Down key.
        [Label("Next")]
        Next = 20,
        //
        // 摘要:
        //     The Page Down key.
        [Label("PageDown")]
        PageDown = 20,
        //
        // 摘要:
        //     The End key.
        [Label("End")]
        End = 21,
        //
        // 摘要:
        //     The Home key.
        [Label("Home")]
        Home = 22,
        //
        // 摘要:
        //     The Left Arrow key.
        [Label("Left")]
        Left = 23,
        //
        // 摘要:
        //     The Up Arrow key.
        [Label("Up")]
        Up = 24,
        //
        // 摘要:
        //     The Right Arrow key.
        [Label("Right")]
        Right = 25,
        //
        // 摘要:
        //     The Down Arrow key.
        [Label("Down")]
        Down = 26,
        //
        // 摘要:
        //     The Select key.
        [Label("Select")]
        Select = 27,
        //
        // 摘要:
        //     The Print key.
        [Label("Print")]
        Print = 28,
        //
        // 摘要:
        //     The Execute key.
        [Label("Execute")]
        Execute = 29,
        //
        // 摘要:
        //     The Print Screen key.
        [Label("Snapshot")]
        Snapshot = 30,
        //
        // 摘要:
        //     The Print Screen key.
        [Label("PrintScreen")]
        PrintScreen = 30,
        //
        // 摘要:
        //     The Insert key.
        [Label("Insert")]
        Insert = 31,
        //
        // 摘要:
        //     The Delete key.
        [Label("Delete")]
        Delete = 32,
        //
        // 摘要:
        //     The Help key.
        [Label("Help")]
        Help = 33,
        //
        // 摘要:
        //     The 0 (zero) key.
        [Label("D0")]
        D0 = 34,
        //
        // 摘要:
        //     The 1 (one) key.
        [Label("D1")]
        D1 = 35,
        //
        // 摘要:
        //     The 2 key.
        [Label("D2")]
        D2 = 36,
        //
        // 摘要:
        //     The 3 key.
        [Label("D3")]
        D3 = 37,
        //
        // 摘要:
        //     The 4 key.
        [Label("D4")]
        D4 = 38,
        //
        // 摘要:
        //     The 5 key.
        [Label("D5")]
        D5 = 39,
        //
        // 摘要:
        //     The 6 key.
        [Label("D6")]
        D6 = 40,
        //
        // 摘要:
        //     The 7 key.
        [Label("D7")]
        D7 = 41,
        //
        // 摘要:
        //     The 8 key.
        [Label("D8")]
        D8 = 42,
        //
        // 摘要:
        //     The 9 key.
        [Label("D9")]
        D9 = 43,
        //
        // 摘要:
        //     The A key.
        [Label("A")]
        A = 44,
        //
        // 摘要:
        //     The B key.
        [Label("B")]
        B = 45,
        //
        // 摘要:
        //     The C key.
        [Label("C")]
        C = 46,
        //
        // 摘要:
        //     The D key.
        [Label("D")]
        D = 47,
        //
        // 摘要:
        //     The E key.
        [Label("E")]
        E = 48,
        //
        // 摘要:
        //     The F key.
        [Label("F")]
        F = 49,
        //
        // 摘要:
        //     The G key.
        [Label("G")]
        G = 50,
        //
        // 摘要:
        //     The H key.
        [Label("H")]
        H = 51,
        //
        // 摘要:
        //     The I key.
        [Label("I")]
        I = 52,
        //
        // 摘要:
        //     The J key.
        [Label("J")]
        J = 53,
        //
        // 摘要:
        //     The K key.
        [Label("K")]
        K = 54,
        //
        // 摘要:
        //     The L key.
        [Label("L")]
        L = 55,
        //
        // 摘要:
        //     The M key.
        [Label("M")]
        M = 56,
        //
        // 摘要:
        //     The N key.
        [Label("N")]
        N = 57,
        //
        // 摘要:
        //     The O key.
        [Label("O")]
        O = 58,
        //
        // 摘要:
        //     The P key.
        [Label("P")]
        P = 59,
        //
        // 摘要:
        //     The Q key.
        [Label("Q")]
        Q = 60,
        //
        // 摘要:
        //     The R key.
        [Label("R")]
        R = 61,
        //
        // 摘要:
        //     The S key.
        [Label("S")]
        S = 62,
        //
        // 摘要:
        //     The T key.
        [Label("T")]
        T = 63,
        //
        // 摘要:
        //     The U key.
        [Label("U")]
        U = 64,
        //
        // 摘要:
        //     The V key.
        [Label("V")]
        V = 65,
        //
        // 摘要:
        //     The W key.
        [Label("W")]
        W = 66,
        //
        // 摘要:
        //     The X key.
        [Label("X")]
        X = 67,
        //
        // 摘要:
        //     The Y key.
        [Label("Y")]
        Y = 68,
        //
        // 摘要:
        //     The Z key.
        [Label("Z")]
        Z = 69,
        //
        // 摘要:
        //     The left Windows logo key (Microsoft Natural Keyboard).
        [Label("LWin")]
        LWin = 70,
        //
        // 摘要:
        //     The right Windows logo key (Microsoft Natural Keyboard).
        [Label("RWin")]
        RWin = 71,
        //
        // 摘要:
        //     The Application key (Microsoft Natural Keyboard).
        [Label("Apps")]
        Apps = 72,
        //
        // 摘要:
        //     The Computer Sleep key.
        [Label("Sleep")]
        Sleep = 73,
        //
        // 摘要:
        //     The 0 key on the numeric keypad.
        [Label("NumPad0")]
        NumPad0 = 74,
        //
        // 摘要:
        //     The 1 key on the numeric keypad.
        [Label("NumPad1")]
        NumPad1 = 75,
        //
        // 摘要:
        //     The 2 key on the numeric keypad.
        [Label("NumPad2")]
        NumPad2 = 76,
        //
        // 摘要:
        //     The 3 key on the numeric keypad.
        [Label("NumPad3")]
        NumPad3 = 77,
        //
        // 摘要:
        //     The 4 key on the numeric keypad.
        [Label("NumPad4")]
        NumPad4 = 78,
        //
        // 摘要:
        //     The 5 key on the numeric keypad.
        [Label("NumPad5")]
        NumPad5 = 79,
        //
        // 摘要:
        //     The 6 key on the numeric keypad.
        [Label("NumPad6")]
        NumPad6 = 80,
        //
        // 摘要:
        //     The 7 key on the numeric keypad.
        [Label("NumPad7")]
        NumPad7 = 81,
        //
        // 摘要:
        //     The 8 key on the numeric keypad.
        [Label("NumPad8")]
        NumPad8 = 82,
        //
        // 摘要:
        //     The 9 key on the numeric keypad.
        [Label("NumPad9")]
        NumPad9 = 83,
        //
        // 摘要:
        //     The Multiply key.
        [Label("Multiply")]
        Multiply = 84,
        //
        // 摘要:
        //     The Add key.
        [Label("Add")]
        Add = 85,
        //
        // 摘要:
        //     The Separator key.
        [Label("Separator")]
        Separator = 86,
        //
        // 摘要:
        //     The Subtract key.
        [Label("Subtract")]
        Subtract = 87,
        //
        // 摘要:
        //     The Decimal key.
        [Label("Decimal")]
        Decimal = 88,
        //
        // 摘要:
        //     The Divide key.
        [Label("Divide")]
        Divide = 89,
        //
        // 摘要:
        //     The F1 key.
        [Label("F1")]
        F1 = 90,
        //
        // 摘要:
        //     The F2 key.
        [Label("F2")]
        F2 = 91,
        //
        // 摘要:
        //     The F3 key.
        [Label("F3")]
        F3 = 92,
        //
        // 摘要:
        //     The F4 key.
        [Label("F4")]
        F4 = 93,
        //
        // 摘要:
        //     The F5 key.
        [Label("F5")]
        F5 = 94,
        //
        // 摘要:
        //     The F6 key.
        [Label("F6")]
        F6 = 95,
        //
        // 摘要:
        //     The F7 key.
        [Label("F7")]
        F7 = 96,
        //
        // 摘要:
        //     The F8 key.
        [Label("F8")]
        F8 = 97,
        //
        // 摘要:
        //     The F9 key.
        [Label("F9")]
        F9 = 98,
        //
        // 摘要:
        //     The F10 key.
        [Label("F10")]
        F10 = 99,
        //
        // 摘要:
        //     The F11 key.
        [Label("F11")]
        F11 = 100,
        //
        // 摘要:
        //     The F12 key.
        [Label("F12")]
        F12 = 101,
        //
        // 摘要:
        //     The F13 key.
        [Label("F13")]
        F13 = 102,
        //
        // 摘要:
        //     The F14 key.
        [Label("F14")]
        F14 = 103,
        //
        // 摘要:
        //     The F15 key.
        [Label("F15")]
        F15 = 104,
        //
        // 摘要:
        //     The F16 key.
        [Label("F16")]
        F16 = 105,
        //
        // 摘要:
        //     The F17 key.
        [Label("F17")]
        F17 = 106,
        //
        // 摘要:
        //     The F18 key.
        [Label("F18")]
        F18 = 107,
        //
        // 摘要:
        //     The F19 key.
        [Label("F19")]
        F19 = 108,
        //
        // 摘要:
        //     The F20 key.
        [Label("F20")]
        F20 = 109,
        //
        // 摘要:
        //     The F21 key.
        [Label("F21")]
        F21 = 110,
        //
        // 摘要:
        //     The F22 key.
        [Label("F22")]
        F22 = 111,
        //
        // 摘要:
        //     The F23 key.
        [Label("F23")]
        F23 = 112,
        //
        // 摘要:
        //     The F24 key.
        [Label("F24")]
        F24 = 113,
        //
        // 摘要:
        //     The Num Lock key.
        [Label("NumLock")]
        NumLock = 114,
        //
        // 摘要:
        //     The Scroll Lock key.
        [Label("Scroll")]
        Scroll = 115,
        //
        // 摘要:
        //     The left Shift key.
        [Label("LeftShift")]
        LeftShift = 116,
        //
        // 摘要:
        //     The right Shift key.
        [Label("RightShift")]
        RightShift = 117,
        //
        // 摘要:
        //     The left CTRL key.
        [Label("LeftCtrl")]
        LeftCtrl = 118,
        //
        // 摘要:
        //     The right CTRL key.
        [Label("RightCtrl")]
        RightCtrl = 119,
        //
        // 摘要:
        //     The left ALT key.
        [Label("LeftAlt")]
        LeftAlt = 120,
        //
        // 摘要:
        //     The right ALT key.
        [Label("RightAlt")]
        RightAlt = 121,
        //
        // 摘要:
        //     The Browser Back key.
        [Label("BrowserBack")]
        BrowserBack = 122,
        //
        // 摘要:
        //     The Browser Forward key.
        [Label("BrowserForward")]
        BrowserForward = 123,
        //
        // 摘要:
        //     The Browser Refresh key.
        [Label("BrowserRefresh")]
        BrowserRefresh = 124,
        //
        // 摘要:
        //     The Browser Stop key.
        [Label("BrowserStop")]
        BrowserStop = 125,
        //
        // 摘要:
        //     The Browser Search key.
        [Label("BrowserSearch")]
        BrowserSearch = 126,
        //
        // 摘要:
        //     The Browser Favorites key.
        [Label("BrowserFavorites")]
        BrowserFavorites = 127,
        //
        // 摘要:
        //     The Browser Home key.
        [Label("BrowserHome")]
        BrowserHome = 128,
        //
        // 摘要:
        //     The Volume Mute key.
        [Label("VolumeMute")]
        VolumeMute = 129,
        //
        // 摘要:
        //     The Volume Down key.
        [Label("VolumeDown")]
        VolumeDown = 130,
        //
        // 摘要:
        //     The Volume Up key.
        [Label("VolumeUp")]
        VolumeUp = 131,
        //
        // 摘要:
        //     The Media Next Track key.
        [Label("MediaNextTrack")]
        MediaNextTrack = 132,
        //
        // 摘要:
        //     The Media Previous Track key.
        [Label("MediaPreviousTrack")]
        MediaPreviousTrack = 133,
        //
        // 摘要:
        //     The Media Stop key.
        [Label("MediaStop")]
        MediaStop = 134,
        //
        // 摘要:
        //     The Media Play Pause key.
        [Label("MediaPlayPause")]
        MediaPlayPause = 135,
        //
        // 摘要:
        //     The Launch Mail key.
        [Label("LaunchMail")]
        LaunchMail = 136,
        //
        // 摘要:
        //     The Select Media key.
        [Label("SelectMedia")]
        SelectMedia = 137,
        //
        // 摘要:
        //     The Launch Application1 key.
        [Label("LaunchApplication1")]
        LaunchApplication1 = 138,
        //
        // 摘要:
        //     The Launch Application2 key.
        [Label("LaunchApplication2")]
        LaunchApplication2 = 139,
        //
        // 摘要:
        //     The OEM 1 key.
        [Label("Oem1")]
        Oem1 = 140,
        //
        // 摘要:
        //     The OEM Semicolon key.
        [Label("OemSemicolon")]
        OemSemicolon = 140,
        //
        // 摘要:
        //     The OEM Addition key.
        [Label("OemPlus")]
        OemPlus = 141,
        //
        // 摘要:
        //     The OEM Comma key.
        [Label("OemComma")]
        OemComma = 142,
        //
        // 摘要:
        //     The OEM Minus key.
        [Label("OemMinus")]
        OemMinus = 143,
        //
        // 摘要:
        //     The OEM Period key.
        [Label("OemPeriod")]
        OemPeriod = 144,
        //
        // 摘要:
        //     The OEM 2 key.
        [Label("Oem2")]
        Oem2 = 145,
        //
        // 摘要:
        //     The OEM Question key.
        [Label("OemQuestion")]
        OemQuestion = 145,
        //
        // 摘要:
        //     The OEM 3 key.
        [Label("Oem3")]
        Oem3 = 146,
        //
        // 摘要:
        //     The OEM Tilde key.
        [Label("OemTilde")]
        OemTilde = 146,
        //
        // 摘要:
        //     The ABNT_C1 (Brazilian) key.
        [Label("AbntC1")]
        AbntC1 = 147,
        //
        // 摘要:
        //     The ABNT_C2 (Brazilian) key.
        [Label("AbntC2")]
        AbntC2 = 148,
        //
        // 摘要:
        //     The OEM 4 key.
        [Label("Oem4")]
        Oem4 = 149,
        //
        // 摘要:
        //     The OEM Open Brackets key.
        [Label("OemOpenBrackets")]
        OemOpenBrackets = 149,
        //
        // 摘要:
        //     The OEM 5 key.
        [Label("Oem5")]
        Oem5 = 150,
        //
        // 摘要:
        //     The OEM Pipe key.
        [Label("OemPipe")]
        OemPipe = 150,
        //
        // 摘要:
        //     The OEM 6 key.
        [Label("Oem6")]
        Oem6 = 151,
        //
        // 摘要:
        //     The OEM Close Brackets key.
        [Label("OemCloseBrackets")]
        OemCloseBrackets = 151,
        //
        // 摘要:
        //     The OEM 7 key.
        [Label("Oem7")]
        Oem7 = 152,
        //
        // 摘要:
        //     The OEM Quotes key.
        [Label("OemQuotes")]
        OemQuotes = 152,
        //
        // 摘要:
        //     The OEM 8 key.
        [Label("Oem8")]
        Oem8 = 153,
        //
        // 摘要:
        //     The OEM 102 key.
        [Label("Oem102")]
        Oem102 = 154,
        //
        // 摘要:
        //     The OEM Backslash key.
        [Label("OemBackslash")]
        OemBackslash = 154,
        //
        // 摘要:
        //     A special key masking the real key being processed by an IME.
        [Label("ImeProcessed")]
        ImeProcessed = 155,
        //
        // 摘要:
        //     A special key masking the real key being processed as a system key.
        [Label("System")]
        System = 156,
        //
        // 摘要:
        //     The OEM ATTN key.
        [Label("OemAttn")]
        OemAttn = 157,
        //
        // 摘要:
        //     The DBE_ALPHANUMERIC key.
        [Label("DbeAlphanumeric")]
        DbeAlphanumeric = 157,
        //
        // 摘要:
        //     The OEM FINISH key.
        [Label("OemFinish")]
        OemFinish = 158,
        //
        // 摘要:
        //     The DBE_KATAKANA key.
        [Label("DbeKatakana")]
        DbeKatakana = 158,
        //
        // 摘要:
        //     The OEM COPY key.
        [Label("OemCopy")]
        OemCopy = 159,
        //
        // 摘要:
        //     The DBE_HIRAGANA key.
        [Label("DbeHiragana")]
        DbeHiragana = 159,
        //
        // 摘要:
        //     The OEM AUTO key.
        [Label("OemAuto")]
        OemAuto = 160,
        //
        // 摘要:
        //     The DBE_SBCSCHAR key.
        [Label("DbeSbcsChar")]
        DbeSbcsChar = 160,
        //
        // 摘要:
        //     The OEM ENLW key.
        [Label("OemEnlw")]
        OemEnlw = 161,
        //
        // 摘要:
        //     The DBE_DBCSCHAR key.
        [Label("DbeDbcsChar")]
        DbeDbcsChar = 161,
        //
        // 摘要:
        //     The OEM BACKTAB key.
        [Label("OemBackTab")]
        OemBackTab = 162,
        //
        // 摘要:
        //     The DBE_ROMAN key.
        [Label("DbeRoman")]
        DbeRoman = 162,
        //
        // 摘要:
        //     The ATTN key.
        [Label("Attn")]
        Attn = 163,
        //
        // 摘要:
        //     The DBE_NOROMAN key.
        [Label("DbeNoRoman")]
        DbeNoRoman = 163,
        //
        // 摘要:
        //     The CRSEL key.
        [Label("CrSel")]
        CrSel = 164,
        //
        // 摘要:
        //     The DBE_ENTERWORDREGISTERMODE key.
        [Label("DbeEnterWordRegisterMode")]
        DbeEnterWordRegisterMode = 164,
        //
        // 摘要:
        //     The EXSEL key.
        [Label("ExSel")]
        ExSel = 165,
        //
        // 摘要:
        //     The DBE_ENTERIMECONFIGMODE key.
        [Label("DbeEnterImeConfigureMode")]
        DbeEnterImeConfigureMode = 165,
        //
        // 摘要:
        //     The ERASE EOF key.
        [Label("EraseEof")]
        EraseEof = 166,
        //
        // 摘要:
        //     The DBE_FLUSHSTRING key.
        [Label("DbeFlushString")]
        DbeFlushString = 166,
        //
        // 摘要:
        //     The PLAY key.
        [Label("Play")]
        Play = 167,
        //
        // 摘要:
        //     The DBE_CODEINPUT key.
        [Label("DbeCodeInput")]
        DbeCodeInput = 167,
        //
        // 摘要:
        //     The ZOOM key.
        [Label("Zoom")]
        Zoom = 168,
        //
        // 摘要:
        //     The DBE_NOCODEINPUT key.
        [Label("DbeNoCodeInput")]
        DbeNoCodeInput = 168,
        //
        // 摘要:
        //     A constant reserved for future use.
        [Label("NoName")]
        NoName = 169,
        //
        // 摘要:
        //     The DBE_DETERMINESTRING key.
        [Label("DbeDetermineString")]
        DbeDetermineString = 169,
        //
        // 摘要:
        //     The PA1 key.
        [Label("Pa1")]
        Pa1 = 170,
        //
        // 摘要:
        //     The DBE_ENTERDLGCONVERSIONMODE key.
        [Label("DbeEnterDialogConversionMode")]
        DbeEnterDialogConversionMode = 170,
        //
        // 摘要:
        //     The OEM Clear key.
        [Label("OemClear")]
        OemClear = 171,
        //
        // 摘要:
        //     The key is used with another key to create a single combined character.
        [Label("DeadCharProcessed")]
        DeadCharProcessed = 172
    }
}
