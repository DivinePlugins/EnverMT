using Divine.Entity;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity.Entities.Abilities.Items;
using Divine.Extensions;
using Divine.Modifier.Modifiers;
using Divine.Update;

namespace AmuletAutoUse
{
    internal class SpamAmulet : IDisposable
    {
        private Item item;
        public SpamAmulet()
        {
            UpdateManager.CreateIngameUpdate(200, UseItem);
        }

        private void UseItem()
        {
            if (!CanBeCasted(AbilityId.item_shadow_amulet)) return;

            Modifier modifier = EntityManager.LocalHero.ModifierStatus.GetBuffsByName("modifier_item_shadow_amulet_fade").FirstOrDefault();
            float remainingTime = modifier == null ? 0 : modifier.RemainingTime;

            if (remainingTime <= PluginMenu.Cooldown.Value)
            {
                item.Cast(EntityManager.LocalHero, false, false);
            }
        }

        private bool CanBeCasted(AbilityId abilityId)
        {
            item = UnitExtensions.GetItemById(EntityManager.LocalHero, abilityId);
            if (item == null) return false;
            if (item.AbilityState != AbilityState.Ready) return false;
            if (item.Cooldown > 0f) return false;
            if (item.Level == 0) return false;

            if (!EntityManager.LocalHero.IsAlive) return false;
            if (EntityManager.LocalHero.IsStunned()) return false;
            if (EntityManager.LocalHero.IsMuted()) return false;

            return true;
        }

        public void Dispose()
        {
            UpdateManager.DestroyIngameUpdate(UseItem);
        }
    }
}
