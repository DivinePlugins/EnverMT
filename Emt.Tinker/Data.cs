using System.Collections.Generic;
using Divine.Entity.Entities.Abilities.Components;

namespace Emt.Tinker
{
    static internal class Data
    {
        internal static class Menu
        {
            public static readonly Dictionary<AbilityId, bool> ComboItems = new Dictionary<AbilityId, bool> {
                {AbilityId.item_soul_ring,true},
                {AbilityId.item_guardian_greaves,true},
                {AbilityId.item_lotus_orb,true},
                {AbilityId.item_glimmer_cape,true},
                {AbilityId.item_ghost,true},                                
                {AbilityId.item_shivas_guard,true},
                {AbilityId.item_bloodstone,true},
                {AbilityId.item_eternal_shroud,true},
                {AbilityId.item_ethereal_blade,true},
                {AbilityId.item_veil_of_discord,true},
                {AbilityId.item_orchid,true},
                {AbilityId.item_bloodthorn,true},
                {AbilityId.item_rod_of_atos,true},
                {AbilityId.item_nullifier,true},                
                {AbilityId.item_dagon,true},
                {AbilityId.item_sheepstick,true},
                {AbilityId.item_blink, true},
            };

            public static readonly Dictionary<AbilityId, bool> ComboAbilities = new Dictionary<AbilityId, bool>
            {
                {AbilityId.tinker_warp_grenade,true},
                {AbilityId.tinker_defense_matrix,true},
                {AbilityId.tinker_heat_seeking_missile,true},
                {AbilityId.tinker_laser,true},
				//{AbilityId.tinker_march_of_the_machines,true},
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
