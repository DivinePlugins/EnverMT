using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Abilities.Items.Components;

using System.Collections.Generic;

namespace Emt.Tinker
{
    static internal class Data
    {
        internal static class Menu
        {
            public static readonly Dictionary<ItemId, bool> ComboItems = new Dictionary<ItemId, bool> {
                {ItemId.item_soul_ring,true},
                {ItemId.item_guardian_greaves,true},
                {ItemId.item_lotus_orb,true},
                {ItemId.item_glimmer_cape,true},
                {ItemId.item_ghost,true},
                {ItemId.item_shivas_guard,true},
                {ItemId.item_bloodstone,true},
                {ItemId.item_eternal_shroud,true},
                {ItemId.item_ethereal_blade,true},
                {ItemId.item_orchid,true},
                {ItemId.item_bloodthorn,true},
                {ItemId.item_rod_of_atos,true},
                {ItemId.item_nullifier,true},
                {ItemId.item_dagon,true},
                {ItemId.item_sheepstick,true},
                {ItemId.item_blink, true},
            };

            public static readonly Dictionary<ItemId, bool> PreCastItems = new Dictionary<ItemId, bool> {
                {ItemId.item_soul_ring,true},
                {ItemId.item_guardian_greaves,true},
                {ItemId.item_bottle,true},
            };

            public static readonly Dictionary<ItemId, bool> ComboNeutralItems = new Dictionary<ItemId, bool> {
                {ItemId.item_arcane_ring,true},
                {ItemId.item_bullwhip,true},
                {ItemId.item_psychic_headband,true},
                {ItemId.item_ninja_gear,true},
                {ItemId.item_trickster_cloak,true},
                {ItemId.item_seer_stone,true},
                {ItemId.item_ex_machina,true},
            };

            public static readonly Dictionary<AbilityId, bool> PreCastAbilities = new Dictionary<AbilityId, bool>
            {
                {AbilityId.tinker_laser,true},
                {AbilityId.tinker_heat_seeking_missile,true},
                {AbilityId.tinker_defense_matrix,false},
                {AbilityId.tinker_warp_grenade,false},
                {AbilityId.tinker_keen_teleport,false},
				//{AbilityId.tinker_march_of_the_machines,true},
                {AbilityId.tinker_rearm,false}
            };

            public static readonly Dictionary<AbilityId, bool> ComboAbilities = new Dictionary<AbilityId, bool>
            {
                {AbilityId.tinker_laser,true},
                {AbilityId.tinker_heat_seeking_missile,true},
                {AbilityId.tinker_defense_matrix,true},
                {AbilityId.tinker_warp_grenade,true},
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
