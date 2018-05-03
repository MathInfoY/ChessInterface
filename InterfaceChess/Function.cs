using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InterfaceChess

{
    public class Functions
    {
//      static public Func<CaseActivite[] ,bool> FunctionPTR = null;

        static public Func<CaseActivite[], bool> [] FunctionLST = { null,
                                                                    F1,F2,F3,F4,F5,F6,F7,F8,F9,F10,F11,F12,F13,F14,F15,F16,F17,F18, // Case #1
                                                                    F19,F20,F21,F22,F23,F24,F25,F26,F27,F28,F29,F30,F31,F32,F33,F34,
                                                                    F35,F36,F37,F38,F39,F40,F41,F42,F43,F44,F45,F46,F47,F48,F49,
                                                                    F50,F51,F52,F53,F54,F55,F56,F57,F58,F59,F60,F61,F62,F63,
                                                                    F64,F65,F66,F67,F68,F69,F70,F71,F72,F73,F74,F75,F76,
                                                                    F77,F78,F79,F80,F81,F82,F83,F84,F85,F86,F87,F88,
                                                                    F89,F90,F91,F92,F93,F94,F95,F96,F97,F98,F99,
                                                                    F100,F101,F102,F103,F104,F105,F106,F107,F108,F109,F110,F111,
                                                                    F112,F113,F114,F115,F116,F117,F118,F119,F120,F121,F122,F123,F124,F125,F126,F127,
                                                                    F128,F129,F130,F131,F132,F133,F134,F135,F136,F137,F138,F139,F140,F141,
                                                                    F142,F143,F144,F145,F146,F147,F148,F149,F150,F151,F152,F153,F154,F155,
                                                                    F156,F157,F158,F159,F160,F161,F162,F163,F164,F165,F166,F167,F168,
                                                                    F169,F170,F171,F172,F173,F174,F175,F176,F177,F178,F179,F180,
                                                                    F181,F182,F183,F184,F185,F186,F187,F188,F189,F190,F191,
                                                                    F192,F193,F194,F195,F196,F197,F198,F199,F200,F201,
                                                                    F202,F203,F204,F205,F206,F207,F208,F209,F210,F211,
                                                                    F212,F213,F214,F215,F216,F217,F218,F219,F220,F221,F222,F223,F224,F225,
                                                                    F226,F227,F228,F229,F230,F231,F232,F233,F234,F235,F236,F237,F238,
                                                                    F239,F240,F241,F242,F243,F244,F245,F246,F247,F248,F249,F250,F251,
                                                                    F252,F253,F254,F255,F256,F257,F258,F259,F260,F261,F262,F263,
                                                                    F264,F265,F266,F267,F268,F269,F270,F271,F272,F273,F274,
                                                                    F275,F276,F277,F278,F279,F280,F281,F282,F283,F284,
                                                                    F285,F286,F287,F288,F289,F290,F291,F292,
                                                                    F293,F294,F295,F296,F297,F298,F299,F300,
                                                                    F301,F302,F303,F304,F305,F306,F307,F308,F309,F310,F311,F312,
                                                                    F313,F314,F315,F316,F317,F318,F319,F320,F321,F322,F323,
                                                                    F324,F325,F326,F327,F328,F329,F330,F331,F332,F333,F334,
                                                                    F335,F336,F337,F338,F339,F340,F341,F342,F343,F344,F345,
                                                                    F346,F347,F348,F349,F350,F351,F352,F353,F354,F355,
                                                                    F356,F357,F358,F359,F360,F361,
                                                                    F362,F363,F364,F365,F366,F367,
                                                                    F368,F369,F370,F371,F372,F373,
                                                                    F374,F375,F376,F377,F378,F379,F380,F381,F382,F383,
                                                                    F384,F385,F386,F387,F388,F389,F390,F391,F392,
                                                                    F393,F394,F395,F396,F397,F398,F399,F400,F401,
                                                                    F402,F403,F404,F405,F406,F407,F408,F409,F410,
                                                                    F411,F412,F413,F414,F415,F416,F417,F418,
                                                                    F419,F420,F421,F422,F423,F424,
                                                                    F425,F426,F427,F428,
                                                                    F429,F430,F431,F432,
                                                                    F433,F434,F435,F436,F437,F438,F439,F440,
                                                                    F441,F442,F443,F444,F445,F446,F447,
                                                                    F448,F449,F450,F451,F452,F453,F454,
                                                                    F455,F456,F457,F458,F459,F460,
                                                                    F461,F462,F463,F464,F465,
                                                                    F466,F467,F468,F469,
                                                                    F470,F471,
                                                                    F472,F473,
                                                                    F474,F475,F476,F477,F478,F479,
                                                                    F480,F481,F482,F483,F484,
                                                                    F485,F486,F487,F488,
                                                                    F489,F490,F491,
                                                                    F492,F493,
                                                                    F494,
                                                                    F495,F496,F497,F498,F499,F500,
                                                                    F501,F502,F503,F504,F505,
                                                                    F506,F507,F508,F509,
                                                                    F510,F511,F512,
                                                                    F513,F514,
                                                                    F515,           // Case 62
                                                                    F516,F517,F518  // Oubli
                                                                  };
// Case #1
// Rangee #1
        static public bool F1(CaseActivite[] activite)
        {
            return (!activite[2].isPiece());
        }
        static public bool F2(CaseActivite[] activite)
        {
            return (!activite[2].isPiece() && !activite[3].isPiece());
        }
        static public bool F3(CaseActivite[] activite)
        {
            return (!activite[2].isPiece() && !activite[3].isPiece() && !activite[4].isPiece());
        }
        static public bool F4(CaseActivite[] activite)
        {
            return (!activite[2].isPiece() && !activite[3].isPiece() && !activite[4].isPiece() && !activite[5].isPiece());
        }
        static public bool F5(CaseActivite[] activite)
        {
            return (!activite[2].isPiece() && !activite[3].isPiece() && !activite[4].isPiece() && !activite[5].isPiece() && !activite[6].isPiece());
        }
        static public bool F6(CaseActivite[] activite)
        {
            return (!activite[2].isPiece() && !activite[3].isPiece() && !activite[4].isPiece() && !activite[5].isPiece() && !activite[6].isPiece() && !activite[7].isPiece());
        }
// Rangee #3
        static public bool F7(CaseActivite[] activite)
        {
            return (!activite[9].isPiece());
        }
        static public bool F8(CaseActivite[] activite)
        {
            return (!activite[10].isPiece());
        }
// Rangee #4
        static public bool F9(CaseActivite[] activite)
        {
            return (!activite[9].isPiece() && !activite[17].isPiece());
        }
        static public bool F10(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[19].isPiece());
        }
// Rangee #5
        static public bool F11(CaseActivite[] activite)
        {
            return (!activite[9].isPiece() && !activite[17].isPiece() && !activite[25].isPiece());
        }
        static public bool F12(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[19].isPiece() && !activite[28].isPiece());
        }

// Rangee #6
        static public bool F13(CaseActivite[] activite)
        {
            return (!activite[9].isPiece() && !activite[17].isPiece() && !activite[25].isPiece() && !activite[33].isPiece());
        }
        static public bool F14(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[19].isPiece() && !activite[28].isPiece() && !activite[37].isPiece());
        }

// Rangee #7
        static public bool F15(CaseActivite[] activite)
        {
            return (!activite[9].isPiece() && !activite[17].isPiece() && !activite[25].isPiece() && !activite[33].isPiece() && !activite[41].isPiece());
        }
        static public bool F16(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[19].isPiece() && !activite[28].isPiece() && !activite[37].isPiece() && !activite[46].isPiece());
        }

// Rangee #8
        static public bool F17(CaseActivite[] activite)
        {
            return (!activite[9].isPiece() && !activite[17].isPiece() && !activite[25].isPiece() && !activite[33].isPiece() && !activite[41].isPiece() && !activite[49].isPiece());
        }
        static public bool F18(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[19].isPiece() && !activite[28].isPiece() && !activite[37].isPiece() && !activite[46].isPiece() && !activite[55].isPiece());
        }

// Case #2 (F19->F34)

// Rangee #1
        static public bool F19(CaseActivite[] activite)
        {
            return (!activite[3].isPiece());
        }
        static public bool F20(CaseActivite[] activite)
        {
            return (!activite[3].isPiece() && !activite[4].isPiece());
        }
        static public bool F21(CaseActivite[] activite)
        {
            return (!activite[3].isPiece() && !activite[4].isPiece() && !activite[5].isPiece());
        }
        static public bool F22(CaseActivite[] activite)
        {
            return (!activite[3].isPiece() && !activite[4].isPiece() && !activite[5].isPiece() && !activite[6].isPiece());
        }
        static public bool F23(CaseActivite[] activite)
        {
            return (!activite[3].isPiece() && !activite[4].isPiece() && !activite[5].isPiece() && !activite[6].isPiece() && !activite[7].isPiece());
        }
// Rangee #3
        static public bool F24(CaseActivite[] activite)
        {
            return (!activite[10].isPiece());
        }
        static public bool F25(CaseActivite[] activite)
        {
            return (!activite[11].isPiece());
        }
// Rangee #4
        static public bool F26(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[18].isPiece());
        }
        static public bool F27(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[20].isPiece());
        }
// Rangee #5
        static public bool F28(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[18].isPiece() && !activite[26].isPiece());
        }
        static public bool F29(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[20].isPiece() && !activite[29].isPiece());
        }

// Rangee #6
        static public bool F30(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[18].isPiece() && !activite[26].isPiece() && !activite[34].isPiece());
        }
        static public bool F31(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[20].isPiece() && !activite[29].isPiece() && !activite[38].isPiece());
        }

// Rangee #7
        static public bool F32(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[18].isPiece() && !activite[26].isPiece() && !activite[34].isPiece() && !activite[42].isPiece());
        }
        static public bool F33(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[20].isPiece() && !activite[29].isPiece() && !activite[38].isPiece() && !activite[47].isPiece());
        }

// Rangee #8
        static public bool F34(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[18].isPiece() && !activite[26].isPiece() && !activite[34].isPiece() && !activite[42].isPiece() && !activite[50].isPiece());
        }

// Case #3 (F35->F49)
// Rangee #1
        static public bool F35(CaseActivite[] activite)
        {
            return (!activite[4].isPiece());
        }
        static public bool F36(CaseActivite[] activite)
        {
            return (!activite[4].isPiece() && !activite[5].isPiece());
        }
        static public bool F37(CaseActivite[] activite)
        {
            return (!activite[4].isPiece() && !activite[5].isPiece() && !activite[6].isPiece());
        }
        static public bool F38(CaseActivite[] activite)
        {
            return (!activite[4].isPiece() && !activite[5].isPiece() && !activite[6].isPiece() && !activite[7].isPiece());
        }
// Rangee #3
        static public bool F39(CaseActivite[] activite)
        {
            return (!activite[10].isPiece());
        }
        static public bool F40(CaseActivite[] activite)
        {
            return (!activite[11].isPiece());
        }
        static public bool F41(CaseActivite[] activite)
        {
            return (!activite[12].isPiece());
        }

// Rangee #4
        static public bool F42(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[19].isPiece());
        }
        static public bool F43(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[21].isPiece());
        }
// Rangee #5
        static public bool F44(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[19].isPiece() && !activite[27].isPiece());
        }
        static public bool F45(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[21].isPiece() && !activite[30].isPiece());
        }
// Rangee #6
        static public bool F46(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[19].isPiece() && !activite[27].isPiece() && !activite[35].isPiece());
        }
        static public bool F47(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[21].isPiece() && !activite[30].isPiece() && !activite[39].isPiece());
        }
// Rangee #7
        static public bool F48(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[19].isPiece() && !activite[27].isPiece() && !activite[35].isPiece() && !activite[43].isPiece());
        }

// Rangee #8
        static public bool F49(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[19].isPiece() && !activite[27].isPiece() && !activite[35].isPiece() && !activite[43].isPiece() && !activite[51].isPiece());
        }

// Case #4 (F50->F63)
// Rangee #1
        static public bool F50(CaseActivite[] activite)
        {
            return (!activite[5].isPiece());
        }
        static public bool F51(CaseActivite[] activite)
        {
            return (!activite[5].isPiece() && !activite[6].isPiece());
        }
        static public bool F52(CaseActivite[] activite)
        {
            return (!activite[5].isPiece() && !activite[6].isPiece() && !activite[7].isPiece());
        }
// Rangee #3
        static public bool F53(CaseActivite[] activite)
        {
            return (!activite[11].isPiece());
        }
        static public bool F54(CaseActivite[] activite)
        {
            return (!activite[12].isPiece());
        }
        static public bool F55(CaseActivite[] activite)
        {
            return (!activite[13].isPiece());
        }

// Rangee #4
        static public bool F56(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[18].isPiece());
        }
        static public bool F57(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[20].isPiece());
        }

        static public bool F58(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[22].isPiece());
        }
// Rangee #5
        static public bool F59(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[20].isPiece() && !activite[28].isPiece());
        }
        static public bool F60(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[22].isPiece() && !activite[31].isPiece());
        }
// Rangee #6
        static public bool F61(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[20].isPiece() && !activite[28].isPiece() && !activite[36].isPiece());
        }
// Rangee #7
        static public bool F62(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[20].isPiece() && !activite[28].isPiece() && !activite[36].isPiece() && !activite[44].isPiece());
        }
// Rangee #8
        static public bool F63(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[20].isPiece() && !activite[28].isPiece() && !activite[36].isPiece() && !activite[44].isPiece() && !activite[52].isPiece());
        }

// Case #5 (F64->F76)
// Rangee #1
        static public bool F64(CaseActivite[] activite)
        {
            return (!activite[6].isPiece());
        }
        static public bool F65(CaseActivite[] activite)
        {
            return (!activite[6].isPiece() && !activite[7].isPiece());
        }
// Rangee #3
        static public bool F66(CaseActivite[] activite)
        {
            return (!activite[12].isPiece());
        }
        static public bool F67(CaseActivite[] activite)
        {
            return (!activite[13].isPiece());
        }
        static public bool F68(CaseActivite[] activite)
        {
            return (!activite[14].isPiece());
        }
// Rangee #4
        static public bool F69(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[19].isPiece());
        }
        static public bool F70(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[21].isPiece());
        }
        static public bool F71(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[23].isPiece());
        }
// Rangee #5
        static public bool F72(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[19].isPiece() && !activite[26].isPiece());
        }
        static public bool F73(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[21].isPiece() && !activite[29].isPiece());
        }
// Rangee #6
        static public bool F74(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[21].isPiece() && !activite[29].isPiece() && !activite[37].isPiece());
        }
// Rangee #7
        static public bool F75(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[21].isPiece() && !activite[29].isPiece() && !activite[37].isPiece() && !activite[45].isPiece());
        }
// Rangee #8
        static public bool F76(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[21].isPiece() && !activite[29].isPiece() && !activite[37].isPiece() && !activite[45].isPiece() && !activite[53].isPiece());
        }

// Case #6 (F77->F88)
// Rangee #1
        static public bool F77(CaseActivite[] activite)
        {
            return (!activite[7].isPiece());
        }
// Rangee #3
        static public bool F78(CaseActivite[] activite)
        {
            return (!activite[13].isPiece());
        }
        static public bool F79(CaseActivite[] activite)
        {
            return (!activite[14].isPiece());
        }
        static public bool F80(CaseActivite[] activite)
        {
            return (!activite[15].isPiece());
        }
 // Rangee #4
        static public bool F81(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[20].isPiece());
        }
        static public bool F82(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[22].isPiece());
        }
// Rangee #5
        static public bool F83(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[20].isPiece() && !activite[27].isPiece());
        }
        static public bool F84(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[22].isPiece() && !activite[30].isPiece());
        }
// Rangee #6
        static public bool F85(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[20].isPiece() && !activite[27].isPiece() && !activite[34].isPiece());
        }
        static public bool F86(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[22].isPiece() && !activite[30].isPiece() && !activite[38].isPiece());
        }
// Rangee #7
        static public bool F87(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[22].isPiece() && !activite[30].isPiece() && !activite[38].isPiece() && !activite[46].isPiece());
        }
// Rangee #8
        static public bool F88(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[22].isPiece() && !activite[30].isPiece() && !activite[38].isPiece() && !activite[46].isPiece() && !activite[54].isPiece());
        }

// Case #7 (F89->F99)
// Rangee #3
        static public bool F89(CaseActivite[] activite)
        {
            return (!activite[14].isPiece());
        }
        static public bool F90(CaseActivite[] activite)
        {
            return (!activite[15].isPiece());
        }
// Rangee #4
        static public bool F91(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[21].isPiece());
        }
        static public bool F92(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[23].isPiece());
        }
// Rangee #5
        static public bool F93(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[21].isPiece() && !activite[28].isPiece());
        }
        static public bool F94(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[23].isPiece() && !activite[31].isPiece());
        }
// Rangee #6
        static public bool F95(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[21].isPiece() && !activite[28].isPiece() && !activite[35].isPiece());
        }
        static public bool F96(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[23].isPiece() && !activite[31].isPiece() && !activite[39].isPiece());
        }
// Rangee #7
        static public bool F97(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[21].isPiece() && !activite[28].isPiece() && !activite[35].isPiece() && !activite[42].isPiece());
        }
        static public bool F98(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[23].isPiece() && !activite[31].isPiece() && !activite[39].isPiece() && !activite[47].isPiece());
        }
// Rangee #8
        static public bool F99(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[23].isPiece() && !activite[31].isPiece() && !activite[39].isPiece() && !activite[47].isPiece() && !activite[55].isPiece());
        }

// Case #8 (F100->F111)
// Rangee #3
        static public bool F100(CaseActivite[] activite)
        {
            return (!activite[15].isPiece());
        }
        static public bool F101(CaseActivite[] activite)
        {
            return (!activite[16].isPiece());
        }
// Rangee #4
        static public bool F102(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[22].isPiece());
        }
        static public bool F103(CaseActivite[] activite)
        {
            return (!activite[16].isPiece() && !activite[24].isPiece());
        }
// Rangee #5
        static public bool F104(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[22].isPiece() && !activite[29].isPiece());
        }
        static public bool F105(CaseActivite[] activite)
        {
            return (!activite[16].isPiece() && !activite[24].isPiece() && !activite[32].isPiece());
        }
// Rangee #6
        static public bool F106(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[22].isPiece() && !activite[29].isPiece() && !activite[36].isPiece());
        }
        static public bool F107(CaseActivite[] activite)
        {
            return (!activite[16].isPiece() && !activite[24].isPiece() && !activite[32].isPiece() && !activite[40].isPiece());
        }
// Rangee #7
        static public bool F108(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[22].isPiece() && !activite[29].isPiece() && !activite[36].isPiece() && !activite[43].isPiece());
        }
        static public bool F109(CaseActivite[] activite)
        {
            return (!activite[16].isPiece() && !activite[24].isPiece() && !activite[32].isPiece() && !activite[40].isPiece() && !activite[48].isPiece());
        }
// Rangee #8
        static public bool F110(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[22].isPiece() && !activite[29].isPiece() && !activite[36].isPiece() && !activite[43].isPiece() && !activite[50].isPiece());
        }
        static public bool F111(CaseActivite[] activite)
        {
            return (!activite[16].isPiece() && !activite[24].isPiece() && !activite[32].isPiece() && !activite[40].isPiece() && !activite[48].isPiece() && !activite[56].isPiece());
        }

// Case #9 (F112->F127)
// Rangee #1
        static public bool F112(CaseActivite[] activite)
        {
            return (!activite[10].isPiece());
        }
        static public bool F113(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[11].isPiece());
        }
        static public bool F114(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[11].isPiece() && !activite[12].isPiece());
        }
        static public bool F115(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[11].isPiece() && !activite[12].isPiece() && !activite[13].isPiece());
        }
        static public bool F116(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[11].isPiece() && !activite[12].isPiece() && !activite[13].isPiece() && !activite[14].isPiece());
        }
        static public bool F117(CaseActivite[] activite)
        {
            return (!activite[10].isPiece() && !activite[11].isPiece() && !activite[12].isPiece() && !activite[13].isPiece() && !activite[14].isPiece() && !activite[15].isPiece());
        }

        // Rangee #4
        static public bool F118(CaseActivite[] activite)
        {
            return (!activite[17].isPiece());
        }
        static public bool F119(CaseActivite[] activite)
        {
            return (!activite[18].isPiece());
        }
        // Rangee #5
        static public bool F120(CaseActivite[] activite)
        {
            return (!activite[17].isPiece() && !activite[25].isPiece());
        }
        static public bool F121(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[27].isPiece());
        }

        // Rangee #6
        static public bool F122(CaseActivite[] activite)
        {
            return (!activite[17].isPiece() && !activite[25].isPiece() && !activite[33].isPiece());
        }
        static public bool F123(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[27].isPiece() && !activite[36].isPiece());
        }

        // Rangee #7
        static public bool F124(CaseActivite[] activite)
        {
            return (!activite[17].isPiece() && !activite[25].isPiece() && !activite[33].isPiece() && !activite[41].isPiece());
        }

        static public bool F125(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[27].isPiece() && !activite[36].isPiece() && !activite[45].isPiece());
        }

        // Rangee #8
        static public bool F126(CaseActivite[] activite)
        {
            return (!activite[17].isPiece() && !activite[25].isPiece() && !activite[33].isPiece() && !activite[41].isPiece() && !activite[49].isPiece());
        }

        static public bool F127(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[27].isPiece() && !activite[36].isPiece() && !activite[45].isPiece() && !activite[54].isPiece());
        }

// Case #10 (F128->F141) 

        // Rangee #1
        static public bool F128(CaseActivite[] activite)
        {
            return (!activite[11].isPiece());
        }
        static public bool F129(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[12].isPiece());
        }
        static public bool F130(CaseActivite[] activite)
        {
            return (!activite[11].isPiece() && !activite[12].isPiece() && !activite[13].isPiece());
        }
        static public bool F131(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[11].isPiece() && !activite[12].isPiece() && !activite[13].isPiece());
        }
        static public bool F132(CaseActivite[] activite)
        {
            return (!activite[15].isPiece() && !activite[11].isPiece() && !activite[12].isPiece() && !activite[13].isPiece() && !activite[14].isPiece());
        }
        // Rangee #4
        static public bool F133(CaseActivite[] activite)
        {
            return (!activite[18].isPiece());
        }
        static public bool F134(CaseActivite[] activite)
        {
            return (!activite[19].isPiece());
        }
        // Rangee #5
        static public bool F135(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[26].isPiece());
        }
        static public bool F136(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[28].isPiece());
        }
        // Rangee #6
        static public bool F137(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[26].isPiece() && !activite[34].isPiece());
        }
        static public bool F138(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[28].isPiece() && !activite[37].isPiece());
        }
        // Rangee #7
        static public bool F139(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[26].isPiece() && !activite[34].isPiece() && !activite[42].isPiece());
        }
        static public bool F140(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[28].isPiece() && !activite[37].isPiece() && !activite[46].isPiece());
        }
        // Rangee #8
        static public bool F141(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[26].isPiece() && !activite[34].isPiece() && !activite[42].isPiece() && !activite[50].isPiece());
        }
// Ajout
        static public bool F516(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[28].isPiece() && !activite[37].isPiece() && !activite[46].isPiece() && !activite[55].isPiece());
        }

// Case #11 (F142->F155) 

        // Rangee #1
        static public bool F142(CaseActivite[] activite)
        {
            return (!activite[12].isPiece());
        }
        static public bool F143(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[13].isPiece());
        }
        static public bool F144(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[13].isPiece() && !activite[14].isPiece());
        }
        static public bool F145(CaseActivite[] activite)
        {
            return (!activite[12].isPiece() && !activite[13].isPiece() && !activite[14].isPiece() && !activite[15].isPiece());
        }
        // Rangee #4
        static public bool F146(CaseActivite[] activite)
        {
            return (!activite[18].isPiece());
        }
        static public bool F147(CaseActivite[] activite)
        {
            return (!activite[19].isPiece());
        }
        static public bool F148(CaseActivite[] activite)
        {
            return (!activite[20].isPiece());
        }
        // Rangee #5
        static public bool F149(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[27].isPiece());
        }
        static public bool F150(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[29].isPiece());
        }
        // Rangee #6
        static public bool F151(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[27].isPiece() && !activite[35].isPiece());
        }
        static public bool F152(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[29].isPiece() && !activite[38].isPiece());
        }
        // Rangee #7
        static public bool F153(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[27].isPiece() && !activite[35].isPiece() && !activite[43].isPiece());
        }
        static public bool F154(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[29].isPiece() && !activite[38].isPiece() && !activite[47].isPiece());
        }
        // Rangee #8
        static public bool F155(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[27].isPiece() && !activite[35].isPiece() && !activite[43].isPiece() && !activite[51].isPiece());
        }

// Case #12 (F156->F168) 

        // Rangee #2
        static public bool F156(CaseActivite[] activite)
        {
            return (!activite[13].isPiece());
        }
        static public bool F157(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[14].isPiece());
        }
        static public bool F158(CaseActivite[] activite)
        {
            return (!activite[13].isPiece() && !activite[14].isPiece() && !activite[15].isPiece());
        }
        // Rangee #4
        static public bool F159(CaseActivite[] activite)
        {
            return (!activite[19].isPiece());
        }
        static public bool F160(CaseActivite[] activite)
        {
            return (!activite[20].isPiece());
        }
        static public bool F161(CaseActivite[] activite)
        {
            return (!activite[21].isPiece());
        }
        // Rangee #5
        static public bool F162(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[26].isPiece());
        }
        static public bool F163(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[28].isPiece());
        }
        static public bool F164(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[30].isPiece());
        }
        // Rangee #6
        static public bool F165(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[28].isPiece() && !activite[36].isPiece());
        }
        static public bool F166(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[30].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F167(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[28].isPiece() && !activite[36].isPiece() && !activite[44].isPiece());
        }
        // Rangee #8
        static public bool F168(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[28].isPiece() && !activite[36].isPiece() && !activite[44].isPiece() && !activite[52].isPiece());
        }

// Case #13 (F169->F180) 

        // Rangee #2
        static public bool F169(CaseActivite[] activite)
        {
            return (!activite[14].isPiece());
        }
        static public bool F170(CaseActivite[] activite)
        {
            return (!activite[14].isPiece() && !activite[15].isPiece());
        }
        // Rangee #4
        static public bool F171(CaseActivite[] activite)
        {
            return (!activite[20].isPiece());
        }
        static public bool F172(CaseActivite[] activite)
        {
            return (!activite[21].isPiece());
        }
        static public bool F173(CaseActivite[] activite)
        {
            return (!activite[22].isPiece());
        }
        // Rangee #5
        static public bool F174(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[27].isPiece());
        }
        static public bool F175(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[29].isPiece());
        }
        static public bool F176(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[31].isPiece());
        }
        // Rangee #6
        static public bool F177(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[27].isPiece() && !activite[34].isPiece());
        }
        static public bool F178(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[29].isPiece() && !activite[37].isPiece());
        }
        // Rangee #7
        static public bool F179(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[29].isPiece() && !activite[37].isPiece() && !activite[45].isPiece());
        }
        // Rangee #8
        static public bool F180(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[29].isPiece() && !activite[37].isPiece() && !activite[45].isPiece() && !activite[53].isPiece());
        }

// Case #14 (F181->F191) 

        // Rangee #2
        static public bool F181(CaseActivite[] activite)
        {
            return (!activite[15].isPiece());
        }
        // Rangee #4
        static public bool F182(CaseActivite[] activite)
        {
            return (!activite[21].isPiece());
        }
        static public bool F183(CaseActivite[] activite)
        {
            return (!activite[22].isPiece());
        }
        static public bool F184(CaseActivite[] activite)
        {
            return (!activite[23].isPiece());
        }
        // Rangee #5
        static public bool F185(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[28].isPiece());
        }
        static public bool F186(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[30].isPiece());
        }
        // Rangee #6
        static public bool F187(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[28].isPiece() && !activite[35].isPiece());
        }
        static public bool F188(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[30].isPiece() && !activite[38].isPiece());
        }
        // Rangee #7
        static public bool F189(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[28].isPiece() && !activite[35].isPiece() && !activite[42].isPiece());
        }
        static public bool F190(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[30].isPiece() && !activite[38].isPiece() && !activite[46].isPiece());
        }
        // Rangee #8
        static public bool F191(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[30].isPiece() && !activite[38].isPiece() && !activite[46].isPiece() && !activite[54].isPiece());
        }

// Case #15 (F192->F201) 

        // Rangee #4
        static public bool F192(CaseActivite[] activite)
        {
            return (!activite[22].isPiece());
        }
        static public bool F193(CaseActivite[] activite)
        {
            return (!activite[23].isPiece());
        }
        // Rangee #5
        static public bool F194(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[29].isPiece());
        }
        static public bool F195(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[31].isPiece());
        }
        // Rangee #6
        static public bool F196(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[29].isPiece() && !activite[36].isPiece());
        }
        static public bool F197(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[31].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F198(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[29].isPiece() && !activite[36].isPiece() && !activite[43].isPiece());
        }
        static public bool F199(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[31].isPiece() && !activite[39].isPiece() && !activite[47].isPiece());
        }
        // Rangee #8
        static public bool F200(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[29].isPiece() && !activite[36].isPiece() && !activite[43].isPiece() && !activite[50].isPiece());
        }
        static public bool F201(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[31].isPiece() && !activite[39].isPiece() && !activite[47].isPiece() && !activite[55].isPiece());
        }

// Case #16 (F202->F211) 

        // Rangee #4
        static public bool F202(CaseActivite[] activite)
        {
            return (!activite[23].isPiece());
        }
        static public bool F203(CaseActivite[] activite)
        {
            return (!activite[24].isPiece());
        }
        // Rangee #5
        static public bool F204(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[30].isPiece());
        }
        static public bool F205(CaseActivite[] activite)
        {
            return (!activite[24].isPiece() && !activite[32].isPiece());
        }
        // Rangee #6
        static public bool F206(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[30].isPiece() && !activite[37].isPiece());
        }
        static public bool F207(CaseActivite[] activite)
        {
            return (!activite[24].isPiece() && !activite[32].isPiece() && !activite[40].isPiece());
        }
        // Rangee #7
        static public bool F208(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[30].isPiece() && !activite[37].isPiece() && !activite[44].isPiece());
        }
        static public bool F209(CaseActivite[] activite)
        {
            return (!activite[24].isPiece() && !activite[32].isPiece() && !activite[40].isPiece() && !activite[48].isPiece());
        }
        // Rangee #8
        static public bool F210(CaseActivite[] activite)
        {
            return (!activite[23].isPiece() && !activite[30].isPiece() && !activite[37].isPiece() && !activite[44].isPiece() && !activite[51].isPiece() && !activite[58].isPiece());
        }
        static public bool F211(CaseActivite[] activite)
        {
            return (!activite[24].isPiece() && !activite[32].isPiece() && !activite[40].isPiece() && !activite[48].isPiece() && !activite[56].isPiece());
        }

// Case #17 (F212->F225) 

        // Rangee #5
        static public bool F212(CaseActivite[] activite)
        {
            return (!activite[18].isPiece());
        }
        static public bool F213(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[19].isPiece());
        }
        static public bool F214(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[19].isPiece() && !activite[20].isPiece());
        }
        static public bool F215(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[19].isPiece() && !activite[20].isPiece() && !activite[21].isPiece());
        }
        static public bool F216(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[19].isPiece() && !activite[20].isPiece() && !activite[21].isPiece() && !activite[22].isPiece());
        }
        static public bool F217(CaseActivite[] activite)
        {
            return (!activite[18].isPiece() && !activite[19].isPiece() && !activite[20].isPiece() && !activite[21].isPiece() && !activite[22].isPiece() && !activite[23].isPiece());
        }
        // Rangee #5
        static public bool F218(CaseActivite[] activite)
        {
            return (!activite[25].isPiece());
        }
        static public bool F219(CaseActivite[] activite)
        {
            return (!activite[26].isPiece());
        }
        // Rangee #6
        static public bool F220(CaseActivite[] activite)
        {
            return (!activite[25].isPiece() && !activite[33].isPiece());
        }
        static public bool F221(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[35].isPiece());
        }
        // Rangee #7
        static public bool F222(CaseActivite[] activite)
        {
            return (!activite[25].isPiece() && !activite[33].isPiece() && !activite[41].isPiece());
        }
        static public bool F223(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[35].isPiece() && !activite[44].isPiece());
        }
        // Rangee #8
        static public bool F224(CaseActivite[] activite)
        {
            return (!activite[25].isPiece() && !activite[33].isPiece() && !activite[41].isPiece() && !activite[49].isPiece());
        }
        static public bool F225(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[35].isPiece() && !activite[44].isPiece() && !activite[53].isPiece());
        }

// Case #18 (F226->F238) 

        // Rangee #3
        static public bool F226(CaseActivite[] activite)
        {
            return (!activite[19].isPiece());
        }
        static public bool F227(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[20].isPiece());
        }
        static public bool F228(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[20].isPiece() && !activite[21].isPiece());
        }
        static public bool F229(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[20].isPiece() && !activite[21].isPiece() && !activite[22].isPiece());
        }
        static public bool F230(CaseActivite[] activite)
        {
            return (!activite[19].isPiece() && !activite[20].isPiece() && !activite[21].isPiece() && !activite[22].isPiece() && !activite[23].isPiece());
        }
        // Rangee #5
        static public bool F231(CaseActivite[] activite)
        {
            return (!activite[26].isPiece());
        }
        static public bool F232(CaseActivite[] activite)
        {
            return (!activite[27].isPiece());
        }
        // Rangee #6
        static public bool F233(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[34].isPiece());
        }
        static public bool F234(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[36].isPiece());
        }
        // Rangee #7
        static public bool F235(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[34].isPiece() && !activite[42].isPiece());
        }
        static public bool F236(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[36].isPiece() && !activite[45].isPiece());
        }
        // Rangee #8
        static public bool F237(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[34].isPiece() && !activite[42].isPiece() && !activite[50].isPiece());
        }
        static public bool F238(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[36].isPiece() && !activite[45].isPiece() && !activite[54].isPiece());
        }

// Case #19 (F239->F251) 

        // Rangee #3
        static public bool F239(CaseActivite[] activite)
        {
            return (!activite[20].isPiece());
        }
        static public bool F240(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[21].isPiece());
        }
        static public bool F241(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[21].isPiece() && !activite[22].isPiece());
        }
        static public bool F242(CaseActivite[] activite)
        {
            return (!activite[20].isPiece() && !activite[21].isPiece() && !activite[22].isPiece() && !activite[23].isPiece());
        }
        // Rangee #5
        static public bool F243(CaseActivite[] activite)
        {
            return (!activite[26].isPiece());
        }
        static public bool F244(CaseActivite[] activite)
        {
            return (!activite[27].isPiece());
        }
        static public bool F245(CaseActivite[] activite)
        {
            return (!activite[28].isPiece());
        }
        // Rangee #6
        static public bool F246(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[35].isPiece());
        }
        static public bool F247(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[37].isPiece());
        }
        // Rangee #7
        static public bool F248(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[35].isPiece() && !activite[43].isPiece());
        }
        static public bool F249(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[37].isPiece() && !activite[46].isPiece());
        }
        // Rangee #8
        static public bool F250(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[35].isPiece() && !activite[43].isPiece() && !activite[51].isPiece());
        }
        static public bool F251(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[37].isPiece() && !activite[46].isPiece() && !activite[55].isPiece());
        }

// Case #20 (F252->F263) 

        // Rangee #3
        static public bool F252(CaseActivite[] activite)
        {
            return (!activite[21].isPiece());
        }
        static public bool F253(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[22].isPiece());
        }
        static public bool F254(CaseActivite[] activite)
        {
            return (!activite[21].isPiece() && !activite[22].isPiece() && !activite[23].isPiece());
        }
        // Rangee #5
        static public bool F255(CaseActivite[] activite)
        {
            return (!activite[27].isPiece());
        }
        static public bool F256(CaseActivite[] activite)
        {
            return (!activite[28].isPiece());
        }
        static public bool F257(CaseActivite[] activite)
        {
            return (!activite[29].isPiece());
        }
        // Rangee #6
        static public bool F258(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[34].isPiece());
        }
        static public bool F259(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[36].isPiece());
        }
        static public bool F260(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[38].isPiece());
        }
        // Rangee #7
        static public bool F261(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[36].isPiece() && !activite[44].isPiece());
        }
        static public bool F262(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[38].isPiece() && !activite[47].isPiece());
        }
        // Rangee #8
        static public bool F263(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[36].isPiece() && !activite[44].isPiece() && !activite[52].isPiece());
        }

// Case #21 (F264->F274) 

        // Rangee #3
        static public bool F264(CaseActivite[] activite)
        {
            return (!activite[22].isPiece());
        }
        static public bool F265(CaseActivite[] activite)
        {
            return (!activite[22].isPiece() && !activite[23].isPiece());
        }
        // Rangee #5
        static public bool F266(CaseActivite[] activite)
        {
            return (!activite[28].isPiece());
        }
        static public bool F267(CaseActivite[] activite)
        {
            return (!activite[29].isPiece());
        }
        static public bool F268(CaseActivite[] activite)
        {
            return (!activite[30].isPiece());
        }
        // Rangee #6
        static public bool F269(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[35].isPiece());
        }
        static public bool F270(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[37].isPiece());
        }
        static public bool F271(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F272(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[35].isPiece() && !activite[42].isPiece());
        }
        static public bool F273(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[37].isPiece() && !activite[45].isPiece());
        }
        // Rangee #8
        static public bool F274(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[37].isPiece() && !activite[45].isPiece() && !activite[53].isPiece());
        }

// Case #22 (F275->F284)

        // Rangee #3
        static public bool F275(CaseActivite[] activite)
        {
            return (!activite[23].isPiece());
        }
        // Rangee #5
        static public bool F276(CaseActivite[] activite)
        {
            return (!activite[29].isPiece());
        }
        static public bool F277(CaseActivite[] activite)
        {
            return (!activite[30].isPiece());
        }
        static public bool F278(CaseActivite[] activite)
        {
            return (!activite[31].isPiece());
        }
        // Rangee #6
        static public bool F279(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[36].isPiece());
        }
        static public bool F280(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[38].isPiece());
        }
        // Rangee #7
        static public bool F281(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[36].isPiece() && !activite[43].isPiece());
        }
        static public bool F282(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[38].isPiece() && !activite[46].isPiece());
        }
        // Rangee #8
        static public bool F283(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[36].isPiece() && !activite[43].isPiece() && !activite[50].isPiece());
        }
        static public bool F284(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[38].isPiece() && !activite[46].isPiece() && !activite[54].isPiece());
        }

// Case #23 (F285->F292)

        // Rangee #5
        static public bool F285(CaseActivite[] activite)
        {
            return (!activite[30].isPiece());
        }
        static public bool F286(CaseActivite[] activite)
        {
            return (!activite[31].isPiece());
        }
        // Rangee #6
        static public bool F287(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[37].isPiece());
        }
        static public bool F288(CaseActivite[] activite)
        {
            return (!activite[31].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F289(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[37].isPiece() && !activite[44].isPiece());
        }
        static public bool F290(CaseActivite[] activite)
        {
            return (!activite[31].isPiece() && !activite[39].isPiece() && !activite[47].isPiece());
        }
        // Rangee #8
        static public bool F291(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[37].isPiece() && !activite[44].isPiece() && !activite[51].isPiece());
        }
        static public bool F292(CaseActivite[] activite)
        {
            return (!activite[31].isPiece() && !activite[39].isPiece() && !activite[47].isPiece() && !activite[55].isPiece());
        }

// Case #24 (F293->F300)

        // Rangee #5
        static public bool F293(CaseActivite[] activite)
        {
            return (!activite[31].isPiece());
        }
        static public bool F294(CaseActivite[] activite)
        {
            return (!activite[32].isPiece());
        }
        // Rangee #6
        static public bool F295(CaseActivite[] activite)
        {
            return (!activite[31].isPiece() && !activite[38].isPiece());
        }
        static public bool F296(CaseActivite[] activite)
        {
            return (!activite[32].isPiece() && !activite[40].isPiece());
        }
        // Rangee #7
        static public bool F297(CaseActivite[] activite)
        {
            return (!activite[31].isPiece() && !activite[38].isPiece() && !activite[45].isPiece());
        }
        static public bool F298(CaseActivite[] activite)
        {
            return (!activite[32].isPiece() && !activite[40].isPiece() && !activite[48].isPiece());
        }
        // Rangee #8
        static public bool F299(CaseActivite[] activite)
        {
            return (!activite[31].isPiece() && !activite[38].isPiece() && !activite[45].isPiece() && !activite[52].isPiece());
        }
        static public bool F300(CaseActivite[] activite)
        {
            return (!activite[32].isPiece() && !activite[40].isPiece() && !activite[48].isPiece() && !activite[56].isPiece());
        }

// Case #25 (F301->F312)

        // Rangee #4
        static public bool F301(CaseActivite[] activite)
        {
            return (!activite[26].isPiece());
        }
        static public bool F302(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[27].isPiece());
        }
        static public bool F303(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[27].isPiece() && !activite[28].isPiece());
        }
        static public bool F304(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[27].isPiece() && !activite[28].isPiece() && !activite[29].isPiece());
        }
        static public bool F305(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[27].isPiece() && !activite[28].isPiece() && !activite[29].isPiece() && !activite[30].isPiece());
        }
        static public bool F306(CaseActivite[] activite)
        {
            return (!activite[26].isPiece() && !activite[27].isPiece() && !activite[28].isPiece() && !activite[29].isPiece() && !activite[30].isPiece() && !activite[31].isPiece());
        }
        // Rangee #6
        static public bool F307(CaseActivite[] activite)
        {
            return (!activite[33].isPiece());
        }
        static public bool F308(CaseActivite[] activite)
        {
            return (!activite[34].isPiece());
        }
        // Rangee #7
        static public bool F309(CaseActivite[] activite)
        {
            return (!activite[33].isPiece() && !activite[41].isPiece());
        }
        static public bool F310(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[43].isPiece());
        }
        // Rangee #8
        static public bool F311(CaseActivite[] activite)
        {
            return (!activite[33].isPiece() && !activite[41].isPiece() && !activite[49].isPiece());
        }
        static public bool F312(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[43].isPiece() && !activite[52].isPiece());
        }

// Case #26 (F313->F323)

        // Rangee #4
        static public bool F313(CaseActivite[] activite)
        {
            return (!activite[27].isPiece());
        }
        static public bool F314(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[27].isPiece());
        }
        static public bool F315(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[28].isPiece() && !activite[29].isPiece());
        }
        static public bool F316(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[28].isPiece() && !activite[29].isPiece() && !activite[30].isPiece());
        }
        static public bool F317(CaseActivite[] activite)
        {
            return (!activite[27].isPiece() && !activite[28].isPiece() && !activite[29].isPiece() && !activite[30].isPiece() && !activite[31].isPiece());
        }
        // Rangee #6
        static public bool F318(CaseActivite[] activite)
        {
            return (!activite[34].isPiece());
        }
        static public bool F319(CaseActivite[] activite)
        {
            return (!activite[35].isPiece());
        }
        // Rangee #7
        static public bool F320(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[42].isPiece());
        }
        static public bool F321(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[44].isPiece());
        }
        // Rangee #8
        static public bool F322(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[42].isPiece() && !activite[50].isPiece());
        }
        static public bool F323(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[44].isPiece() && !activite[53].isPiece());
        }

// Case #27 (F324->F334)

        // Rangee #4
        static public bool F324(CaseActivite[] activite)
        {
            return (!activite[28].isPiece());
        }
        static public bool F325(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[29].isPiece());
        }
        static public bool F326(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[29].isPiece() && !activite[30].isPiece());
        }
        static public bool F327(CaseActivite[] activite)
        {
            return (!activite[28].isPiece() && !activite[29].isPiece() && !activite[30].isPiece() && !activite[31].isPiece());
        }
        // Rangee #6
        static public bool F328(CaseActivite[] activite)
        {
            return (!activite[34].isPiece());
        }
        static public bool F329(CaseActivite[] activite)
        {
            return (!activite[35].isPiece());
        }
        static public bool F330(CaseActivite[] activite)
        {
            return (!activite[36].isPiece());
        }
        // Rangee #7
        static public bool F331(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[43].isPiece());
        }
        static public bool F332(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[45].isPiece());
        }
        // Rangee #8
        static public bool F333(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[43].isPiece() && !activite[51].isPiece());
        }
        static public bool F334(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[45].isPiece() && !activite[54].isPiece());
        }

// Case #28 (F335->F345)

        // Rangee #4
        static public bool F335(CaseActivite[] activite)
        {
            return (!activite[29].isPiece());
        }
        static public bool F336(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[30].isPiece());
        }
        static public bool F337(CaseActivite[] activite)
        {
            return (!activite[29].isPiece() && !activite[30].isPiece() && !activite[31].isPiece());
        }
        // Rangee #6
        static public bool F338(CaseActivite[] activite)
        {
            return (!activite[35].isPiece());
        }
        static public bool F339(CaseActivite[] activite)
        {
            return (!activite[36].isPiece());
        }
        static public bool F340(CaseActivite[] activite)
        {
            return (!activite[37].isPiece());
        }
        // Rangee #7
        static public bool F341(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[42].isPiece());
        }
        static public bool F342(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[44].isPiece());
        }
        static public bool F343(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[46].isPiece());
        }
        // Rangee #8
        static public bool F344(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[44].isPiece() && !activite[52].isPiece());
        }
        static public bool F345(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[46].isPiece() && !activite[55].isPiece());
        }

// Case #29 (F346->F355)

        // Rangee #4
        static public bool F346(CaseActivite[] activite)
        {
            return (!activite[30].isPiece());
        }
        static public bool F347(CaseActivite[] activite)
        {
            return (!activite[30].isPiece() && !activite[31].isPiece());
        }
        // Rangee #6
        static public bool F348(CaseActivite[] activite)
        {
            return (!activite[36].isPiece());
        }
        static public bool F349(CaseActivite[] activite)
        {
            return (!activite[37].isPiece());
        }
        static public bool F350(CaseActivite[] activite)
        {
            return (!activite[38].isPiece());
        }
        // Rangee #7
        static public bool F351(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[43].isPiece());
        }
        static public bool F352(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[45].isPiece());
        }
        static public bool F353(CaseActivite[] activite)
        {
            return (!activite[38].isPiece() && !activite[47].isPiece());
        }
        // Rangee #8
        static public bool F354(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[43].isPiece() && !activite[50].isPiece());
        }
        static public bool F355(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[45].isPiece() && !activite[53].isPiece());
        }

// Case #30 (F356->F361)

        // Rangee #4
        static public bool F356(CaseActivite[] activite)
        {
            return (!activite[31].isPiece());
        }
        // Rangee #6
        static public bool F357(CaseActivite[] activite)
        {
            return (!activite[37].isPiece());
        }
        static public bool F358(CaseActivite[] activite)
        {
            return (!activite[38].isPiece());
        }
        static public bool F359(CaseActivite[] activite)
        {
            return (!activite[39].isPiece());
        }
        // Rangee #7
        static public bool F360(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[44].isPiece());
        }
        static public bool F361(CaseActivite[] activite)
        {
            return (!activite[38].isPiece() && !activite[46].isPiece());
        }
        // Rangee #8
        static public bool F517(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[44].isPiece() && !activite[51].isPiece());
        }
        static public bool F518(CaseActivite[] activite)
        {
            return (!activite[38].isPiece() && !activite[46].isPiece() && !activite[54].isPiece());
        }

// Case #31 (F362->F367)

        // Rangee #6
        static public bool F362(CaseActivite[] activite)
        {
            return (!activite[38].isPiece());
        }
        static public bool F363(CaseActivite[] activite)
        {
            return (!activite[39].isPiece());
        }
        // Rangee #7
        static public bool F364(CaseActivite[] activite)
        {
            return (!activite[38].isPiece() && !activite[45].isPiece());
        }
        static public bool F365(CaseActivite[] activite)
        {
            return (!activite[39].isPiece() && !activite[47].isPiece());
        }
        // Rangee #8
        static public bool F366(CaseActivite[] activite)
        {
            return (!activite[38].isPiece() && !activite[45].isPiece() && !activite[52].isPiece());
        }
        static public bool F367(CaseActivite[] activite)
        {
            return (!activite[39].isPiece() && !activite[47].isPiece() && !activite[55].isPiece());
        }

// Case #32 (F368->F373)

        // Rangee #6
        static public bool F368(CaseActivite[] activite)
        {
            return (!activite[39].isPiece());
        }
        static public bool F369(CaseActivite[] activite)
        {
            return (!activite[40].isPiece());
        }
        // Rangee #7
        static public bool F370(CaseActivite[] activite)
        {
            return (!activite[39].isPiece() && !activite[46].isPiece());
        }
        static public bool F371(CaseActivite[] activite)
        {
            return (!activite[40].isPiece() && !activite[48].isPiece());
        }
        // Rangee #8
        static public bool F372(CaseActivite[] activite)
        {
            return (!activite[39].isPiece() && !activite[46].isPiece() && !activite[53].isPiece());
        }
        static public bool F373(CaseActivite[] activite)
        {
            return (!activite[40].isPiece() && !activite[48].isPiece() && !activite[56].isPiece());
        }

// Case #33 (F374->F383)

        // Rangee #5
        static public bool F374(CaseActivite[] activite)
        {
            return (!activite[34].isPiece());
        }
        static public bool F375(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[35].isPiece());
        }
        static public bool F376(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[35].isPiece() && !activite[36].isPiece());
        }
        static public bool F377(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[35].isPiece() && !activite[36].isPiece() && !activite[37].isPiece());
        }
        static public bool F378(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[35].isPiece() && !activite[36].isPiece() && !activite[37].isPiece() && !activite[38].isPiece());
        }
        static public bool F379(CaseActivite[] activite)
        {
            return (!activite[34].isPiece() && !activite[35].isPiece() && !activite[36].isPiece() && !activite[37].isPiece() && !activite[38].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F380(CaseActivite[] activite)
        {
            return (!activite[41].isPiece());
        }
        static public bool F381(CaseActivite[] activite)
        {
            return (!activite[42].isPiece());
        }
        // Rangee #8
        static public bool F382(CaseActivite[] activite)
        {
            return (!activite[41].isPiece() && !activite[49].isPiece());
        }
        static public bool F383(CaseActivite[] activite)
        {
            return (!activite[42].isPiece() && !activite[51].isPiece());
        }

// Case #34 (F384->F392)

        // Rangee #5
        static public bool F384(CaseActivite[] activite)
        {
            return (!activite[35].isPiece());
        }
        static public bool F385(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[36].isPiece());
        }
        static public bool F386(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[36].isPiece() && !activite[37].isPiece());
        }
        static public bool F387(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[36].isPiece() && !activite[37].isPiece() && !activite[38].isPiece());
        }
        static public bool F388(CaseActivite[] activite)
        {
            return (!activite[35].isPiece() && !activite[36].isPiece() && !activite[37].isPiece() && !activite[38].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F389(CaseActivite[] activite)
        {
            return (!activite[42].isPiece());
        }
        static public bool F390(CaseActivite[] activite)
        {
            return (!activite[43].isPiece());
        }
        // Rangee #8
        static public bool F391(CaseActivite[] activite)
        {
            return (!activite[42].isPiece() && !activite[50].isPiece());
        }
        static public bool F392(CaseActivite[] activite)
        {
            return (!activite[43].isPiece() && !activite[52].isPiece());
        }
 
// Case #35 (F393->F401)

        // Rangee #5
        static public bool F393(CaseActivite[] activite)
        {
            return (!activite[36].isPiece());
        }
        static public bool F394(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[37].isPiece());
        }
        static public bool F395(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[37].isPiece() && !activite[38].isPiece());
        }
        static public bool F396(CaseActivite[] activite)
        {
            return (!activite[36].isPiece() && !activite[37].isPiece() && !activite[38].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F397(CaseActivite[] activite)
        {
            return (!activite[42].isPiece());
        }
        static public bool F398(CaseActivite[] activite)
        {
            return (!activite[43].isPiece());
        }
        static public bool F399(CaseActivite[] activite)
        {
            return (!activite[44].isPiece());
        }
        // Rangee #8
        static public bool F400(CaseActivite[] activite)
        {
            return (!activite[43].isPiece() && !activite[51].isPiece());
        }
        static public bool F401(CaseActivite[] activite)
        {
            return (!activite[44].isPiece() && !activite[53].isPiece());
        }

// Case #36 (F402->F410)

        // Rangee #5
        static public bool F402(CaseActivite[] activite)
        {
            return (!activite[37].isPiece());
        }
        static public bool F403(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[38].isPiece());
        }
        static public bool F404(CaseActivite[] activite)
        {
            return (!activite[37].isPiece() && !activite[38].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F405(CaseActivite[] activite)
        {
            return (!activite[43].isPiece());
        }
        static public bool F406(CaseActivite[] activite)
        {
            return (!activite[44].isPiece());
        }
        static public bool F407(CaseActivite[] activite)
        {
            return (!activite[45].isPiece());
        }
        // Rangee #8
        static public bool F408(CaseActivite[] activite)
        {
            return (!activite[43].isPiece() && !activite[50].isPiece());
        }
        static public bool F409(CaseActivite[] activite)
        {
            return (!activite[44].isPiece() && !activite[52].isPiece());
        }
        static public bool F410(CaseActivite[] activite)
        {
            return (!activite[45].isPiece() && !activite[54].isPiece());
        }

// Case #37 (F411->F418)

        // Rangee #5
        static public bool F411(CaseActivite[] activite)
        {
            return (!activite[38].isPiece());
        }
        static public bool F412(CaseActivite[] activite)
        {
            return (!activite[38].isPiece() && !activite[39].isPiece());
        }
        // Rangee #7
        static public bool F413(CaseActivite[] activite)
        {
            return (!activite[44].isPiece());
        }
        static public bool F414(CaseActivite[] activite)
        {
            return (!activite[45].isPiece());
        }
        static public bool F415(CaseActivite[] activite)
        {
            return (!activite[46].isPiece());
        }
        // Rangee #8
        static public bool F416(CaseActivite[] activite)
        {
            return (!activite[44].isPiece() && !activite[51].isPiece());
        }
        static public bool F417(CaseActivite[] activite)
        {
            return (!activite[45].isPiece() && !activite[53].isPiece());
        }
        static public bool F418(CaseActivite[] activite)
        {
            return (!activite[46].isPiece() && !activite[55].isPiece());
        }

// Case #38 (F419->F424)

        // Rangee #5
        static public bool F419(CaseActivite[] activite)
        {
            return (!activite[39].isPiece());
        }
        // Rangee #7
        static public bool F420(CaseActivite[] activite)
        {
            return (!activite[45].isPiece());
        }
        static public bool F421(CaseActivite[] activite)
        {
            return (!activite[46].isPiece());
        }
        static public bool F422(CaseActivite[] activite)
        {
            return (!activite[47].isPiece());
        }
        // Rangee #8
        static public bool F423(CaseActivite[] activite)
        {
            return (!activite[45].isPiece() && !activite[52].isPiece());
        }
        static public bool F424(CaseActivite[] activite)
        {
            return (!activite[46].isPiece() && !activite[54].isPiece());
        }

// Case #39 (F425->F428)

        // Rangee #7
        static public bool F425(CaseActivite[] activite)
        {
            return (!activite[46].isPiece());
        }
        static public bool F426(CaseActivite[] activite)
        {
            return (!activite[47].isPiece());
        }
        // Rangee #8
        static public bool F427(CaseActivite[] activite)
        {
            return (!activite[46].isPiece() && !activite[53].isPiece());
        }
        static public bool F428(CaseActivite[] activite)
        {
            return (!activite[47].isPiece() && !activite[55].isPiece());
        }

// Case #40 (F429->F432)

        // Rangee #7
        static public bool F429(CaseActivite[] activite)
        {
            return (!activite[47].isPiece());
        }
        static public bool F430(CaseActivite[] activite)
        {
            return (!activite[48].isPiece());
        }
        // Rangee #8
        static public bool F431(CaseActivite[] activite)
        {
            return (!activite[47].isPiece() && !activite[54].isPiece());
        }
        static public bool F432(CaseActivite[] activite)
        {
            return (!activite[48].isPiece() && !activite[56].isPiece());
        }


// Start

// Case #41 (F433->F440)

        // Rangee #6
        static public bool F433(CaseActivite[] activite)
        {
            return (!activite[42].isPiece());
        }
        static public bool F434(CaseActivite[] activite)
        {
            return (!activite[42].isPiece() && !activite[43].isPiece());
        }
        static public bool F435(CaseActivite[] activite)
        {
            return (!activite[42].isPiece() && !activite[43].isPiece() && !activite[44].isPiece());
        }
        static public bool F436(CaseActivite[] activite)
        {
            return (!activite[42].isPiece() && !activite[43].isPiece() && !activite[44].isPiece() && !activite[45].isPiece());
        }
        static public bool F437(CaseActivite[] activite)
        {
            return (!activite[42].isPiece() && !activite[43].isPiece() && !activite[44].isPiece() && !activite[45].isPiece() && !activite[46].isPiece());
        }
        static public bool F438(CaseActivite[] activite)
        {
            return (!activite[42].isPiece() && !activite[43].isPiece() && !activite[44].isPiece() && !activite[45].isPiece() && !activite[46].isPiece() && !activite[47].isPiece());
        }

        // Rangee #8
        static public bool F439(CaseActivite[] activite)
        {
            return (!activite[49].isPiece());
        }
        static public bool F440(CaseActivite[] activite)
        {
            return (!activite[50].isPiece());
        }

// Case #42 (F441->F447)

        // Rangee #6
        static public bool F441(CaseActivite[] activite)
        {
            return (!activite[43].isPiece());
        }
        static public bool F442(CaseActivite[] activite)
        {
            return (!activite[43].isPiece() && !activite[44].isPiece());
        }
        static public bool F443(CaseActivite[] activite)
        {
            return (!activite[43].isPiece() && !activite[44].isPiece() && !activite[45].isPiece());
        }
        static public bool F444(CaseActivite[] activite)
        {
            return (!activite[43].isPiece() && !activite[44].isPiece() && !activite[45].isPiece() && !activite[46].isPiece());
        }
        static public bool F445(CaseActivite[] activite)
        {
            return (!activite[43].isPiece() && !activite[44].isPiece() && !activite[45].isPiece() && !activite[46].isPiece() && !activite[47].isPiece());
        }

        // Rangee #8
        static public bool F446(CaseActivite[] activite)
        {
            return (!activite[50].isPiece());
        }
        static public bool F447(CaseActivite[] activite)
        {
            return (!activite[51].isPiece());
        }

// Case #43 (F448->F454)

        // Rangee #6
        static public bool F448(CaseActivite[] activite)
        {
            return (!activite[44].isPiece());
        }
        static public bool F449(CaseActivite[] activite)
        {
            return (!activite[44].isPiece() && !activite[45].isPiece());
        }
        static public bool F450(CaseActivite[] activite)
        {
            return (!activite[44].isPiece() && !activite[45].isPiece() && !activite[46].isPiece());
        }
        static public bool F451(CaseActivite[] activite)
        {
            return (!activite[44].isPiece() && !activite[45].isPiece() && !activite[46].isPiece() && !activite[47].isPiece());
        }

        // Rangee #8
        static public bool F452(CaseActivite[] activite)
        {
            return (!activite[50].isPiece());
        }
        static public bool F453(CaseActivite[] activite)
        {
            return (!activite[51].isPiece());
        }
        static public bool F454(CaseActivite[] activite)
        {
            return (!activite[52].isPiece());
        }

// Case #44 (F455->F460)

        // Rangee #6
        static public bool F455(CaseActivite[] activite)
        {
            return (!activite[45].isPiece());
        }
        static public bool F456(CaseActivite[] activite)
        {
            return (!activite[45].isPiece() && !activite[46].isPiece());
        }
        static public bool F457(CaseActivite[] activite)
        {
            return (!activite[45].isPiece() && !activite[46].isPiece() && !activite[47].isPiece());
        }

        // Rangee #8
        static public bool F458(CaseActivite[] activite)
        {
            return (!activite[51].isPiece());
        }
        static public bool F459(CaseActivite[] activite)
        {
            return (!activite[52].isPiece());
        }
        static public bool F460(CaseActivite[] activite)
        {
            return (!activite[53].isPiece());
        }

// Case #45 (F461->F465)

        // Rangee #6
        static public bool F461(CaseActivite[] activite)
        {
            return (!activite[46].isPiece());
        }
        static public bool F462(CaseActivite[] activite)
        {
            return (!activite[46].isPiece() && !activite[47].isPiece());
        }

        // Rangee #8
        static public bool F463(CaseActivite[] activite)
        {
            return (!activite[52].isPiece());
        }
        static public bool F464(CaseActivite[] activite)
        {
            return (!activite[53].isPiece());
        }
        static public bool F465(CaseActivite[] activite)
        {
            return (!activite[54].isPiece());
        }

// Case #46 (F466->F469)

        // Rangee #6
        static public bool F466(CaseActivite[] activite)
        {
            return (!activite[47].isPiece());
        }

        // Rangee #8
        static public bool F467(CaseActivite[] activite)
        {
            return (!activite[53].isPiece());
        }
        static public bool F468(CaseActivite[] activite)
        {
            return (!activite[54].isPiece());
        }
        static public bool F469(CaseActivite[] activite)
        {
            return (!activite[55].isPiece());
        }

// Case #47 (F470->F471)

        // Rangee #8
        static public bool F470(CaseActivite[] activite)
        {
            return (!activite[54].isPiece());
        }
        static public bool F471(CaseActivite[] activite)
        {
            return (!activite[55].isPiece());
        }

// Case #48 (F472->F473)

        // Rangee #8
        static public bool F472(CaseActivite[] activite)
        {
            return (!activite[55].isPiece());
        }
        static public bool F473(CaseActivite[] activite)
        {
            return (!activite[56].isPiece());
        }

// Case #49 (F474->F383)

        // Rangee #7
        static public bool F474(CaseActivite[] activite)
        {
            return (!activite[50].isPiece());
        }
        static public bool F475(CaseActivite[] activite)
        {
            return (!activite[50].isPiece() && !activite[51].isPiece());
        }
        static public bool F476(CaseActivite[] activite)
        {
            return (!activite[50].isPiece() && !activite[51].isPiece() && !activite[52].isPiece());
        }
        static public bool F477(CaseActivite[] activite)
        {
            return (!activite[50].isPiece() && !activite[51].isPiece() && !activite[52].isPiece() && !activite[53].isPiece());
        }
        static public bool F478(CaseActivite[] activite)
        {
            return (!activite[50].isPiece() && !activite[51].isPiece() && !activite[52].isPiece() && !activite[53].isPiece() && !activite[54].isPiece());
        }
        static public bool F479(CaseActivite[] activite)
        {
            return (!activite[50].isPiece() && !activite[51].isPiece() && !activite[52].isPiece() && !activite[53].isPiece() && !activite[54].isPiece() && !activite[55].isPiece());
        }

// Case #50 (F480->F484)

        // Rangee #7
        static public bool F480(CaseActivite[] activite)
        {
            return (!activite[51].isPiece());
        }
        static public bool F481(CaseActivite[] activite)
        {
            return (!activite[51].isPiece() && !activite[52].isPiece());
        }
        static public bool F482(CaseActivite[] activite)
        {
            return (!activite[51].isPiece() && !activite[52].isPiece() && !activite[53].isPiece());
        }
        static public bool F483(CaseActivite[] activite)
        {
            return (!activite[51].isPiece() && !activite[52].isPiece() && !activite[53].isPiece() && !activite[54].isPiece());
        }
        static public bool F484(CaseActivite[] activite)
        {
            return (!activite[51].isPiece() && !activite[52].isPiece() && !activite[53].isPiece() && !activite[54].isPiece() && !activite[55].isPiece());
        }

// Case #51 (F485->F488)

        // Rangee #7
        static public bool F485(CaseActivite[] activite)
        {
            return (!activite[52].isPiece());
        }
        static public bool F486(CaseActivite[] activite)
        {
            return (!activite[52].isPiece() && !activite[53].isPiece());
        }
        static public bool F487(CaseActivite[] activite)
        {
            return (!activite[52].isPiece() && !activite[53].isPiece() && !activite[54].isPiece());
        }
        static public bool F488(CaseActivite[] activite)
        {
            return (!activite[52].isPiece() && !activite[53].isPiece() && !activite[54].isPiece() && !activite[55].isPiece());
        }

// Case #52 (F489->F491)

        // Rangee #7
        static public bool F489(CaseActivite[] activite)
        {
            return (!activite[53].isPiece());
        }
        static public bool F490(CaseActivite[] activite)
        {
            return (!activite[53].isPiece() && !activite[54].isPiece());
        }
        static public bool F491(CaseActivite[] activite)
        {
            return (!activite[53].isPiece() && !activite[54].isPiece() && !activite[55].isPiece());
        }

// Case #53 (F492->F493)

        // Rangee #7
        static public bool F492(CaseActivite[] activite)
        {
            return (!activite[54].isPiece());
        }
        static public bool F493(CaseActivite[] activite)
        {
            return (!activite[54].isPiece() && !activite[55].isPiece());
        }

// Case #54 (F494)

        // Rangee #7
        static public bool F494(CaseActivite[] activite)
        {
            return (!activite[55].isPiece());
        }

// Case #57 (F495->F500)

        // Rangee #8
        static public bool F495(CaseActivite[] activite)
        {
            return (!activite[58].isPiece());
        }
        static public bool F496(CaseActivite[] activite)
        {
            return (!activite[58].isPiece() && !activite[59].isPiece());
        }
        static public bool F497(CaseActivite[] activite)
        {
            return (!activite[58].isPiece() && !activite[59].isPiece() && !activite[60].isPiece());
        }
        static public bool F498(CaseActivite[] activite)
        {
            return (!activite[58].isPiece() && !activite[59].isPiece() && !activite[60].isPiece() && !activite[61].isPiece());
        }
        static public bool F499(CaseActivite[] activite)
        {
            return (!activite[58].isPiece() && !activite[59].isPiece() && !activite[60].isPiece() && !activite[61].isPiece() && !activite[62].isPiece());
        }
        static public bool F500(CaseActivite[] activite)
        {
            return (!activite[58].isPiece() && !activite[59].isPiece() && !activite[60].isPiece() && !activite[61].isPiece() && !activite[62].isPiece() && !activite[63].isPiece());
        }

// Case #58 (F501->F505)

        // Rangee #8
        static public bool F501(CaseActivite[] activite)
        {
            return (!activite[59].isPiece());
        }
        static public bool F502(CaseActivite[] activite)
        {
            return (!activite[59].isPiece() && !activite[60].isPiece());
        }
        static public bool F503(CaseActivite[] activite)
        {
            return (!activite[59].isPiece() && !activite[60].isPiece() && !activite[61].isPiece());
        }
        static public bool F504(CaseActivite[] activite)
        {
            return (!activite[59].isPiece() && !activite[60].isPiece() && !activite[61].isPiece() && !activite[62].isPiece());
        }
        static public bool F505(CaseActivite[] activite)
        {
            return (!activite[59].isPiece() && !activite[60].isPiece() && !activite[61].isPiece() && !activite[62].isPiece() && !activite[63].isPiece());
        }

// Case #59 (F506->F509)

        // Rangee #8
        static public bool F506(CaseActivite[] activite)
        {
            return (!activite[60].isPiece());
        }
        static public bool F507(CaseActivite[] activite)
        {
            return (!activite[60].isPiece() && !activite[61].isPiece());
        }
        static public bool F508(CaseActivite[] activite)
        {
            return (!activite[60].isPiece() && !activite[61].isPiece() && !activite[62].isPiece());
        }
        static public bool F509(CaseActivite[] activite)
        {
            return (!activite[60].isPiece() && !activite[61].isPiece() && !activite[62].isPiece() && !activite[63].isPiece());
        }

// Case #60 (F510->F512)

        // Rangee #8
        static public bool F510(CaseActivite[] activite)
        {
            return (!activite[61].isPiece());
        }
        static public bool F511(CaseActivite[] activite)
        {
            return (!activite[61].isPiece() && !activite[62].isPiece());
        }
        static public bool F512(CaseActivite[] activite)
        {
            return (!activite[61].isPiece() && !activite[62].isPiece() && !activite[63].isPiece());
        }

// Case #61 (F513->F514)

        // Rangee #8
        static public bool F513(CaseActivite[] activite)
        {
            return (!activite[62].isPiece());
        }
        static public bool F514(CaseActivite[] activite)
        {
            return (!activite[62].isPiece() && !activite[63].isPiece());
        }

// Case #62 (F515)

        // Rangee #8
        static public bool F515(CaseActivite[] activite)
        {
            return (!activite[63].isPiece());
        }

    }
}
