using System.Collections.Generic;
using Divine.Entity.Entities.Abilities.Components;

namespace Emt_Tinker
{
    internal class Data
    {
        internal static class Menu
        {
            public static readonly Dictionary<AbilityId, bool> ComboVectorCastItems = new Dictionary<AbilityId, bool> {                
                { AbilityId.item_arcane_blink, true },
                { AbilityId.item_overwhelming_blink, true },
                { AbilityId.item_swift_blink, true },
                { AbilityId.item_blink, true },
            };
            public static readonly Dictionary<AbilityId, bool> ComboSelfCastItems = new Dictionary<AbilityId, bool> {
                {AbilityId.item_lotus_orb,true},                
                {AbilityId.item_glimmer_cape,true},
            };
            public static readonly Dictionary<AbilityId, bool> ComboNoTargetItems = new Dictionary<AbilityId, bool>
            {
                {AbilityId.item_ghost,true},
                {AbilityId.item_soul_ring,true},
                {AbilityId.item_guardian_greaves,true},
                {AbilityId.item_shivas_guard,true},
                {AbilityId.item_bloodstone,true},
                {AbilityId.item_eternal_shroud,true},
            };
            public static readonly Dictionary<AbilityId, bool> ComboTargetItems = new Dictionary<AbilityId, bool>
            {
                {AbilityId.item_sheepstick,true},
                {AbilityId.item_ethereal_blade,true},
                {AbilityId.item_veil_of_discord,true},
                {AbilityId.item_orchid,true},
                {AbilityId.item_bloodthorn,true},
                {AbilityId.item_rod_of_atos,true},
                {AbilityId.item_nullifier,true}
            };

            public static readonly Dictionary<AbilityId, bool> ComboTargetItemsDagons = new Dictionary<AbilityId, bool> {
                {AbilityId.item_dagon_5,true},
                {AbilityId.item_dagon_4,true},
                {AbilityId.item_dagon_3,true},
                {AbilityId.item_dagon_2,true},
                {AbilityId.item_dagon,true},

            };

            public static readonly Dictionary<AbilityId, bool> ComboAbilities = new Dictionary<AbilityId, bool>
            {
                {AbilityId.tinker_laser,true},
                {AbilityId.tinker_heat_seeking_missile,true},
				//{AbilityId.tinker_march_of_the_machines,true},
				{AbilityId.tinker_defense_matrix,true},
                {AbilityId.tinker_warp_grenade,true},
                {AbilityId.tinker_rearm,true}
            };

            public static readonly string[] TargetSelectorModes = new string[]
            {
                "First In radius of Cursor, then nearest to Hero",
                "Nearest to Hero",
                "In radius of Cursor",
            };

            public static readonly string[] ComboBlinkModes = new string[]
            {
                "To cursor",
                "In radius"
            };

            public static readonly string[] LinkenBreakerModes = new string[]
            {
                "First what can be used (not Hex)",
                "Laser"
            };
        }
    }
}
