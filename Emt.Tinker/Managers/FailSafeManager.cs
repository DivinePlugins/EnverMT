using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Divine;
using Divine.Entity;
using Divine.Order.Orders;
using Divine.Order;
using Divine.Order.EventArgs;
using Divine.Order.Orders.Components;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Abilities.Spells.Components;
using Divine.Extensions;

namespace Emt.Tinker.Managers
{
    static internal class FailSafeManager
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
            if (e.Order?.Type != OrderType.Cast && e.Order?.Type != OrderType.CastTarget) return;

            SafeRearm(e);
            SafeRocket(e);
        }

        private static float GetAbilitiesCooldownSum()
        {
            float sum = 0f;
            foreach (var s in EntityManager.LocalHero.Spellbook.GetSpells(SpellSlot.SpellbookSlot1, SpellSlot.SpellbookSlot6))
            {
                sum += s.Cooldown;
            }

            foreach (var i in EntityManager.LocalHero.Inventory.MainItems)
            {
                sum += i.Cooldown;
            }
            return sum;
        }

        private static void SafeRearm(OrderAddingEventArgs e)
        {
            if (!PluginMenu.FailSafeSwitcher_Rearm) return;
            if (e.Order?.Ability?.Id != AbilityId.tinker_rearm) return;
            if (GetAbilitiesCooldownSum() > 0) return;

            if (PluginMenu.FailSafeSwitcher_Rearm_ignoreWithShiva &&
                UnitExtensions.GetItemById(EntityManager.LocalHero, AbilityId.item_shivas_guard) != null) return;

            e.Process = false;
        }

        private static void SafeRocket(OrderAddingEventArgs e)
        {
            if (!PluginMenu.FailSafeSwitcher_Rocket) return;
            if (e.Order?.Ability?.Id != AbilityId.tinker_heat_seeking_missile) return;
            if (TargetManager.targetForRocket != null) return;
            
            e.Process = false;
        }
    }
}
