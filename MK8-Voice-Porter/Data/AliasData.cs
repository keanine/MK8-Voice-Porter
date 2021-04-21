using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MK8VoicePorter
{
    class AliasData
    {
        private static (string devName, string friendlyName)[] driverAlias = new (string, string)[]
        {
            ("MARIO", "Mario"),
            ("KOOPA", "Bowser")
        };

        public static (string devName, string friendlyName)[] voiceFileAlias = new (string, string)[]
        {
            ( "pSE_HORN_DRIVERNAME_OFF"         , "SFX_HORN_OFF"             ),
            ( "pSE_HORN_DRIVERNAME_ON"          , "SFX_HORN_ON"              ),
            ( "pSE_HORN_DRIVERNAME_ON_SP"       , "SFX_ITEM_HORN"            ),
            ( "pSE_DRIVERCODE_BK_JPACT_SA"      , "SFX_BIKE_TRICK_SA"        ),
            ( "pSE_DRIVERCODE_BK_JPACT_SB"      , "SFX_BIKE_TRICK_SB"        ),
            ( "pSE_DRIVERCODE_KT_JPACT"         , "SFX_KART_TRICK"           ),
            ( "pSE_DRIVERCODE_KT_JPACT_SA"      , "SFX_KART_TRICK_SA"        ),
            ( "pSE_DRIVERCODE_KT_JPACT_SB"      , "SFX_KART_TRICK_SB"        ),
            ( "pVO_DRIVERCODE_CMB_LAVA"         , "RETRIEVED_FROM_LAVA_1"    ),
            ( "pVO_DRIVERCODE_CMB_LAVA_END"     , "RETRIEVED_FROM_LAVA_2"    ),
            ( "pVO_DRIVERCODE_CMB_WATER"        , "RETRIEVED_FROM_WATER_1"   ),
            ( "pVO_DRIVERCODE_CMB_WATER_END"    , "RETRIEVED_FROM_WATER_2"   ),
            ( "pVO_DRIVERCODE_DMG_BOUND"        , "DAMAGE_BOUND_1"           ),
            ( "pVO_DRIVERCODE_DMG_BOUND_END"    , "DAMAGE_BOUND_2"           ),
            ( "pVO_DRIVERCODE_DMG_CRSH"         , "DAMAGE_CRASH_1"           ),
            ( "pVO_DRIVERCODE_DMG_CRSH_END"     , "DAMAGE_CRASH_2"           ),
            ( "pVO_DRIVERCODE_DMG_S"            , "DAMAGE_S_1"               ),
            ( "pVO_DRIVERCODE_DMG_S_END"        , "DAMAGE_S_2"               ),
            ( "pVO_DRIVERCODE_DMG_SPN"          , "DAMAGE_SPIN_1"            ),
            ( "pVO_DRIVERCODE_DMG_SPN_END"      , "DAMAGE_SPIN_2"            ),
            ( "pVO_DRIVERCODE_DSH"              , "DASH_1"                   ),
            ( "pVO_DRIVERCODE_DSH2"             , "DASH_2"                   ),
            ( "pVO_DRIVERCODE_DSH3"             , "DASH_3"                   ),
            ( "pVO_DRIVERCODE_DSH_END"          , "DASH_4"                   ),
            ( "pVO_DRIVERCODE_DSH_S"            , "DASH_S_1"                 ),
            ( "pVO_DRIVERCODE_DSH_S2"           , "DASH_S_2"                 ),
            ( "pVO_DRIVERCODE_DSH_S3"           , "DASH_S_3"                 ),
            ( "pVO_DRIVERCODE_DSH_S_END"        , "DASH_S_4"                 ),
            ( "pVO_DRIVERCODE_FALL_LAVA"        , "FALL_LAVA_1"              ),
            ( "pVO_DRIVERCODE_FALL_LAVA_END"    , "FALL_LAVA_2"              ),
            ( "pVO_DRIVERCODE_FALL_SHORT"       , "FALL_SHORT_1"             ),
            ( "pVO_DRIVERCODE_FALL_SHORT_END"   , "FALL_SHORT_2"             ),
            ( "pVO_DRIVERCODE_FALL_WATER"       , "FALL_WATER_1"             ),
            ( "pVO_DRIVERCODE_FALL_WATER_END"   , "FALL_WATER_2"             ),
            ( "pVO_DRIVERCODE_GOL_BAD"          , "FINISH_BAD_1"             ),
            ( "pVO_DRIVERCODE_GOL_BAD2"         , "FINISH_BAD_2"             ),
            ( "pVO_DRIVERCODE_GOL_BAD_END"      , "FINISH_BAD_3"             ),
            ( "pVO_DRIVERCODE_GOL_GOD"          , "FINISH_GOOD_1"            ),
            ( "pVO_DRIVERCODE_GOL_GOD2"         , "FINISH_GOOD_2"            ),
            ( "pVO_DRIVERCODE_GOL_GOD_END"      , "FINISH_GOOD_3"            ),
            ( "pVO_DRIVERCODE_GOL_TOP"          , "FINISH_FIRST_1"           ),
            ( "pVO_DRIVERCODE_GOL_TOP2"         , "FINISH_FIRST_2"           ),
            ( "pVO_DRIVERCODE_GOL_TOP_END"      , "FINISH_FIRST_3"           ),
            ( "pVO_DRIVERCODE_HOVER_CHARGE"     , "ANTI-GRAVITY_SPIN_1"      ),
            ( "pVO_DRIVERCODE_HOVER_CHARGE2"    , "ANTI-GRAVITY_SPIN_2"      ),
            ( "pVO_DRIVERCODE_HOVER_CHARGE_END" , "ANTI-GRAVITY_SPIN_3"      ),
            ( "pVO_DRIVERCODE_HOVER_DASH"       , "ANTI-GRAVITY_DASH_1"      ),
            ( "pVO_DRIVERCODE_HOVER_DASH2"      , "ANTI-GRAVITY_DASH_2"      ),
            ( "pVO_DRIVERCODE_HOVER_DASH_END"   , "ANTI-GRAVITY_DASH_3"      ),
            ( "pVO_DRIVERCODE_ITM_GESSO"        , "ITEM_BLOOPER_1"           ),
            ( "pVO_DRIVERCODE_ITM_GESSO_END"    , "ITEM_BLOOPER_2"           ),
            ( "pVO_DRIVERCODE_ITM_PUT"          , "ITEM_PLACE_BEHIND_1"      ),
            ( "pVO_DRIVERCODE_ITM_PUT_END"      , "ITEM_PLACE_BEHIND_2"      ),
            ( "pVO_DRIVERCODE_ITM_SCES"         , "ITEM_HIT_SUCCESS_1"       ),
            ( "pVO_DRIVERCODE_ITM_SCES2"        , "ITEM_HIT_SUCCESS_2"       ),
            ( "pVO_DRIVERCODE_ITM_SCES_END"     , "ITEM_HIT_SUCCESS_3"       ),
            ( "pVO_DRIVERCODE_ITM_STAR"         , "ITEM_STAR_1"              ),
            ( "pVO_DRIVERCODE_ITM_STAR_END"     , "ITEM_STAR_2"              ),
            ( "pVO_DRIVERCODE_ITM_TRW"          , "ITEM_THROW_FORWARD_1"     ),
            ( "pVO_DRIVERCODE_ITM_TRW_END"      , "ITEM_THROW_FORWARD_2"     ),
            ( "pVO_DRIVERCODE_JP_ACT"           , "TRICK_1"                  ),
            ( "pVO_DRIVERCODE_JP_ACT2"          , "TRICK_2"                  ),
            ( "pVO_DRIVERCODE_JP_ACT_END"       , "TRICK_3"                  ),
            ( "pVO_DRIVERCODE_JP_ACT_SA"        , "TRICK_SA_1"               ),
            ( "pVO_DRIVERCODE_JP_ACT_SA2"       , "TRICK_SA_2"               ),
            ( "pVO_DRIVERCODE_JP_ACT_SA_END"    , "TRICK_SA_3"               ),
            ( "pVO_DRIVERCODE_JP_ACT_SB"        , "TRICK_SB_1"               ),
            ( "pVO_DRIVERCODE_JP_ACT_SB2"       , "TRICK_SB_2"               ),
            ( "pVO_DRIVERCODE_JP_ACT_SB_END"    , "TRICK_SB_3"               ),
            ( "pVO_DRIVERCODE_OVTAK"            , "OVERTAKE_1"               ),
            ( "pVO_DRIVERCODE_OVTAK2"           , "OVERTAKE_2"               ),
            ( "pVO_DRIVERCODE_OVTAK3"           , "OVERTAKE_3"               ),
            ( "pVO_DRIVERCODE_OVTAK4"           , "OVERTAKE_4"               ),
            ( "pVO_DRIVERCODE_OVTAK_END"        , "OVERTAKE_5"               ),
            ( "pVO_DRIVERCODE_START_GLIDE"      , "START_GLIDE_1"            ),
            ( "pVO_DRIVERCODE_START_GLIDE2"     , "START_GLIDE_2"            ),
            ( "pVO_DRIVERCODE_START_GLIDE_END"  , "START_GLIDE_3"            ),
            ( "pVO_DRIVERCODE_START_HOVER"      , "START_ANTI-GRAVITY_1"     ),
            ( "pVO_DRIVERCODE_START_HOVER2"     , "START_ANTI-GRAVITY_2"     ),
            ( "pVO_DRIVERCODE_START_HOVER_END"  , "START_ANTI-GRAVITY_3"     ),
            ( "pVO_DRIVERCODE_STRM"             , "SLIPSTREAM_1"             ),
            ( "pVO_DRIVERCODE_STRM2"            , "SLIPSTREAM_2"             ),
            ( "pVO_DRIVERCODE_STRM_END"         , "SLIPSTREAM_3"             ),
            ( "pVO_DRIVERCODE_STR_FAIL"         , "STALL_1"                  ),
            ( "pVO_DRIVERCODE_STR_FAIL_END"     , "STALL_2"                  ),
            ( "pVO_M_DRIVERCODE_SELECT_CHAR"    , "SELECT_DRIVER"            )
        };

        public static string GetDevDriverAlias(string friendlyName)
        {
            foreach (var a in driverAlias)
            {
                if (a.friendlyName == friendlyName)
                {
                    return a.devName;
                }
            }

            return "ERROR";
        }

        public static string GetFriendlyDriverAlias(string devName)
        {
            foreach (var a in driverAlias)
            {
                if (a.devName == devName)
                {
                    return a.friendlyName;
                }
            }

            return "ERROR";
        }

        public static string GetFriendlyVoiceAlias(string devName, string driverName, string driverCode)
        {
            string key = string.Empty;

            if (devName.StartsWith("pSE_HORN"))
            {
                key = devName.Replace($"_{driverName}_", "_DRIVERNAME_");
            }
            else
            {
                key = devName.Replace($"_{driverCode}_", "_DRIVERCODE_");
            }

            foreach (var alias in voiceFileAlias)
            {
                if (alias.devName == key)
                {
                    return alias.friendlyName;
                }
            }
            return "NO FRIENDLY ALIAS FOUND FOR " + devName;
        }

        public static (string devName, string friendlyName)[] GetCopyOfVoiceFileAlias()
        {
            return voiceFileAlias;
        }
    }
}
