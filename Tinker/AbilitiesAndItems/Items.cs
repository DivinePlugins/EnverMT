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


            if (isInInventory(AbilityId.item_bloodthorn)) {this.bloodthorn.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_bloodthorn));}
            else {this.bloodthorn.UpdateItemToNull();}

            if (isInInventory(AbilityId.item_eternal_shroud)) { this.eternalShroud.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_eternal_shroud)); }
            else { this.eternalShroud.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_ethereal_blade)) { this.etherealBlade.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_ethereal_blade)); }
            else { this.etherealBlade.UpdateItemToNull(); }
            
            if (isInInventory(AbilityId.item_glimmer_cape)) { this.glimmerCape.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_glimmer_cape)); }
            else { this.glimmerCape.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_guardian_greaves)) { this.guardianGreaves.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_guardian_greaves)); }
            else { this.guardianGreaves.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_aether_lens)) { this.lens.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_aether_lens)); }
            else { this.lens.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_lotus_orb)) { this.lotusOrb.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_lotus_orb)); }
            else { this.lotusOrb.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_nullifier)) { this.nullifier.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_nullifier)); }
            else { this.nullifier.UpdateItemToNull(); }
            
            if (isInInventory(AbilityId.item_octarine_core)) { this.octarine.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_octarine_core)); }
            else { this.octarine.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_orchid)) { this.orchid.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_orchid)); }
            else { this.orchid.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_rod_of_atos)) { this.rodOfAtos.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_rod_of_atos)); }
            else { this.rodOfAtos.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_sheepstick)) { this.scytheOfVyse.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_sheepstick)); }
            else { this.scytheOfVyse.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_shivas_guard)) { this.shivasGuard.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_shivas_guard)); }
            else { this.shivasGuard.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_soul_ring)) { this.soulRing.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_soul_ring)); }
            else { this.soulRing.UpdateItemToNull(); }

            if (isInInventory(AbilityId.item_veil_of_discord)) { this.veilOfDiscord.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_veil_of_discord)); }
            else { this.veilOfDiscord.UpdateItemToNull(); }



            if (isInInventory(AbilityId.item_arcane_blink))
            {
                this.blink.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_arcane_blink));
            } else if (isInInventory(AbilityId.item_swift_blink))
            {
                this.blink.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_swift_blink));
            } else if (isInInventory(AbilityId.item_overwhelming_blink))
            {
                this.blink.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_overwhelming_blink));
            }
            else if (isInInventory(AbilityId.item_blink))
            {
                this.blink.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_blink));
            }
            else
            {
                this.blink.UpdateItemToNull();
            }


            if (isInInventory(AbilityId.item_dagon_5))
            {
                this.dagon.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_dagon_5));
            } else if (isInInventory(AbilityId.item_dagon_4))
            {
                this.dagon.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_dagon_4));
            }
            else if (isInInventory(AbilityId.item_dagon_3))
            {
                this.dagon.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_dagon_3));
            }
            else if (isInInventory(AbilityId.item_dagon_2))
            {
                this.dagon.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_dagon_2));
            }
            else if (isInInventory(AbilityId.item_dagon))
            {
                this.dagon.Update(UnitExtensions.GetItemById(localHero, AbilityId.item_dagon));
            } else
            {
                this.dagon.UpdateItemToNull();
            }
        }

        private bool isInInventory(AbilityId abilityId)
        {
            foreach (var i in this.localHero.Inventory.MainItems)
            {
                if (i.Id == abilityId) return true;                
            }
            return false;
        }
    }
}