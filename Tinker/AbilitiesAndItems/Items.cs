using System;
using System.Linq;
using Divine.Entity;
using Divine.Entity.Entities.Abilities;
using Divine.Entity.Entities.Abilities.Items;
using Divine.Entity.Entities.Units.Components;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Extensions;
using Divine.Helpers;
using Divine.Entity.Entities.Abilities.Components;

namespace Tinker.AbilitiesAndItems
{
    internal class Items : Base
    {
        #region Variables
        private Hero localHero;        
        public readonly Items.Blink blink;
        public readonly Items.Bloodthorn bloodthorn;
        public readonly Items.BloodStone bloodStone;
        public readonly Items.Dagon dagon;
        public readonly Items.EternalShroud eternalShroud;
        public readonly Items.EtherealBlade etherealBlade;
        public readonly Items.GhostScepter ghostScepter;
        public readonly Items.GlimmerCape glimmerCape;
        public readonly Items.GuardianGreaves guardianGreaves;
        public readonly Items.Lens lens;
        public readonly Items.LotusOrb lotusOrb;
        public readonly Items.Nullifier nullifier;
        public readonly Items.Octarine octarine;
        public readonly Items.Orchid orchid;
        public readonly Items.RodOfAtos rodOfAtos;
        public readonly Items.ScytheOfVyse scytheOfVyse;
        public readonly Items.ShivasGuard shivasGuard;
        public readonly Items.SoulRing soulRing;
        public readonly Items.VeilOfDiscord veilOfDiscord;
        #endregion
        #region ItemClasses        
        internal class Blink : Base { }
        internal class Bloodthorn : Base { }
        internal class BloodStone : Base { }
        internal class Dagon : Base { }
        internal class EternalShroud : Base { }
        internal class EtherealBlade : Base { }
        internal class GhostScepter : Base { }
        internal class GlimmerCape : Base { }
        internal class GuardianGreaves : Base { }
        internal class Lens : Base { }
        internal class LotusOrb : Base { }
        internal class Nullifier : Base { }
        internal class Octarine : Base { }
        internal class Orchid : Base { }
        internal class RodOfAtos : Base { }
        internal class ScytheOfVyse : Base { }
        internal class ShivasGuard : Base { }
        internal class SoulRing : Base { }
        internal class VeilOfDiscord : Base { }
        #endregion
        public Items()
        {
            blink = new Items.Blink();
            bloodthorn = new Items.Bloodthorn();
            bloodStone = new Items.BloodStone();
            dagon = new Items.Dagon();
            eternalShroud = new Items.EternalShroud();
            etherealBlade = new Items.EtherealBlade();
            ghostScepter = new Items.GhostScepter();
            glimmerCape = new Items.GlimmerCape();
            guardianGreaves = new Items.GuardianGreaves();
            lens = new Items.Lens();
            lotusOrb = new Items.LotusOrb();
            nullifier = new Items.Nullifier();
            octarine = new Items.Octarine();
            orchid = new Items.Orchid();
            rodOfAtos = new Items.RodOfAtos();
            scytheOfVyse = new Items.ScytheOfVyse();
            shivasGuard = new Items.ShivasGuard();
            soulRing = new Items.SoulRing();
            veilOfDiscord = new Items.VeilOfDiscord();
        }
        public void Update()
        {            
            this.localHero = EntityManager.LocalHero;
            if (this.localHero == null) return;           

            this.bloodthorn.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_bloodthorn));
            this.bloodStone.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_bloodstone));
            this.eternalShroud.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_eternal_shroud));
            this.etherealBlade.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_ethereal_blade));
            this.ghostScepter.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_ghost));
            this.glimmerCape.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_glimmer_cape));
            this.guardianGreaves.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_guardian_greaves));
            this.lens.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_aether_lens));
            this.lotusOrb.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_lotus_orb));
            this.nullifier.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_nullifier));
            this.octarine.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_octarine_core));
            this.orchid.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_orchid));
            this.rodOfAtos.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_rod_of_atos));
            this.scytheOfVyse.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_sheepstick));
            this.shivasGuard.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_shivas_guard));
            this.soulRing.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_soul_ring));
            this.veilOfDiscord.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_veil_of_discord));
            #region BlinkUpdate

            Inventory inventory = localHero.Inventory;
            Ability ability;
            if (inventory == null)
            {
                ability = null;
            }
            else
            {
                ability = inventory.MainItems.FirstOrDefault((Item x) =>
                    x.Id == AbilityId.item_blink
                    || x.Id == AbilityId.item_overwhelming_blink
                    || x.Id == AbilityId.item_swift_blink
                    || x.Id == AbilityId.item_arcane_blink);
            }
            this.blink.Update(ability);
            #endregion
            #region DagonUpdate
            Base baseAbility2 = this.dagon;
            Inventory inventory2 = localHero.Inventory;
            Ability ability2;
            if (inventory2 == null)
            {
                ability2 = null;
            }
            else
            {
                ability2 = inventory2.MainItems.FirstOrDefault((Item x) => LocalizationHelper.LocalizeAbilityName(x.Name) == "Dagon");
            }
            baseAbility2.Update(ability2);
            #endregion            
        }
    }
}