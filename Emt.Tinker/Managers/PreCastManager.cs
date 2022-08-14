using System;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Order;
using Divine.Order.EventArgs;
using Divine.Order.Orders.Components;
using Divine.Entity;
using Divine.Extensions;

namespace Emt.Tinker.Managers
{
    static internal class PreCastManager
    {
        public static void Activate()
        {

            OrderManager.OrderAdding += OrderManager_OrderAdding;

        }
        public static void Dispose()
        {
            OrderManager.OrderAdding -= OrderManager_OrderAdding;
        }

        private static void OrderManager_OrderAdding(OrderAddingEventArgs e)
        {
            foreach (var ability in PluginMenu.PreCastAbilities.Values)
            {
                if (e.Order?.Ability?.Id == ability.Key && ability.Value == true)
                {
                    foreach (var item in PluginMenu.PreCastItems.Values)
                    {
                        if (item.Value == true)
                        {
                            CastManager.castItem(item.Key, null, false, false);
                        }
                    }
                }
            }

            SafeKeenTeleport(e);

        }
        private static void SafeKeenTeleport(OrderAddingEventArgs e)
        {
            if (!PluginMenu.PreCastDefenceBeforeKeen) return;
            if (e.Order?.Ability?.Id != AbilityId.tinker_keen_teleport) return;
            if (!EntityManager.LocalHero.HasModifier("modifier_fountain_aura_buff")) return;
            CastManager.castDefenseMatrix(false);
        }
    }
}
