using System;
using System.Collections.Generic;
using Divine.Update;
using Divine.Entity.Entities.Abilities.Components;
using Divine.Entity;
using Divine.Extensions;
using Divine.Entity.Entities.Units;
using Divine.Numerics;
using Divine.Entity.Entities.Abilities.Items.Components;

namespace Emt_Tinker.Managers
{
    internal class AbilityManager : IDisposable
    {
        public Divine.Entity.Entities.Abilities.Ability ability { get; private set; }
        public AbilityId abilityId { get; private set; }        
        private Context Context;        
        public Dictionary<AbilityId, EnumAbilityTypes> abilityDictionary = new Dictionary<AbilityId, EnumAbilityTypes>();
        public enum EnumAbilityTypes
        {
            VectorCast = 0,
            SelftCast = 1,
            NoTarget = 2,
            Target = 3,
        }
        public AbilityManager(Context context)
        {
            this.Context = context;
            Context.PluginMenu.PluginStatus.ValueChanged += PluginStatus_ValueChanged;

        }
        private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.CreateIngameUpdate(1000, Update);
            }
            else
            {
                UpdateManager.DestroyIngameUpdate(Update);
            }
        }
        public void Dispose()
        {
            UpdateManager.DestroyIngameUpdate(Update);
        }
        private void Update()
        {
            this.updateAbilitiesHashSet();            
        }
        private void updateAbilitiesHashSet()
        {
            
            //ComboAbilities
            
            foreach (var item in Emt_Tinker.Data.Menu.ComboAbilities)
            {   
                if (item.Value == true && !this.abilityDictionary.ContainsKey(item.Key))
                {
                    switch (item.Key)
                    {
                        case AbilityId.tinker_laser:
                            {
                                this.abilityDictionary.Add(item.Key, EnumAbilityTypes.Target);
                                break;
                            }
                        case AbilityId.tinker_heat_seeking_missile:
                            {
                                this.abilityDictionary.Add(item.Key, EnumAbilityTypes.NoTarget);
                                break;
                            }
                        case AbilityId.tinker_defense_matrix:
                            {
                                this.abilityDictionary.Add(item.Key, EnumAbilityTypes.SelftCast);
                                break;
                            }
                        case AbilityId.tinker_warp_grenade:
                            {
                                this.abilityDictionary.Add(item.Key, EnumAbilityTypes.Target);
                                break;
                            }
                        case AbilityId.tinker_rearm:
                            {
                                this.abilityDictionary.Add(item.Key, EnumAbilityTypes.NoTarget);
                                break;
                            }
                        default: break;
                    }                    
                }                    
                if (item.Value == false && this.abilityDictionary.ContainsKey(item.Key)) this.abilityDictionary.Remove(item.Key);                
                //Console.WriteLine($"Ability ID = {item} and Priority = {Context.PluginMenu.ComboAbilitiesToggler.GetPriority(item.Key)}");
            }
            
            foreach (var item in Emt_Tinker.Data.Menu.ComboVectorCastItems)
            {   
                if (item.Value == true && !this.abilityDictionary.ContainsKey(item.Key) && this.IsItemInInventory(item.Key)) this.abilityDictionary.Add(item.Key, EnumAbilityTypes.VectorCast);
                if (item.Value == false && this.abilityDictionary.ContainsKey(item.Key)) this.abilityDictionary.Remove(item.Key);
            }
            foreach (var item in Emt_Tinker.Data.Menu.ComboSelfCastItems)
            {
                if (item.Value == true && !this.abilityDictionary.ContainsKey(item.Key) && this.IsItemInInventory(item.Key)) this.abilityDictionary.Add(item.Key, EnumAbilityTypes.SelftCast);
                if (item.Value == false && this.abilityDictionary.ContainsKey(item.Key)) this.abilityDictionary.Remove(item.Key);
            }
            foreach (var item in Emt_Tinker.Data.Menu.ComboNoTargetItems)
            {
                if (item.Value == true && !this.abilityDictionary.ContainsKey(item.Key) && this.IsItemInInventory(item.Key)) this.abilityDictionary.Add(item.Key, EnumAbilityTypes.NoTarget);
                if (item.Value == false && this.abilityDictionary.ContainsKey(item.Key)) this.abilityDictionary.Remove(item.Key);
            }
            foreach (var item in Emt_Tinker.Data.Menu.ComboTargetItems)
            {
                if (item.Value == true && !this.abilityDictionary.ContainsKey(item.Key) && this.IsItemInInventory(item.Key)) this.abilityDictionary.Add(item.Key, EnumAbilityTypes.Target);
                if (item.Value == false && this.abilityDictionary.ContainsKey(item.Key)) this.abilityDictionary.Remove(item.Key);
            }
            foreach (var item in Emt_Tinker.Data.Menu.ComboTargetItemsDagons)
            {
                if (item.Value == true && !this.abilityDictionary.ContainsKey(item.Key) && this.IsItemInInventory(item.Key)) this.abilityDictionary.Add(item.Key, EnumAbilityTypes.Target);
                if (item.Value == false && this.abilityDictionary.ContainsKey(item.Key)) this.abilityDictionary.Remove(item.Key);
            }            
        }     
        public bool IsItemInInventory(AbilityId abilityId)
        {            
            var itemsInInventory = EntityManager.LocalHero.Inventory.GetItems(ItemSlot.MainSlot1, ItemSlot.MainSlot6);              
            foreach (var i in itemsInInventory)
            {
                if (abilityId == i.Id) return true;                
            }
            return false;
        }
              
        public void SetAbility(AbilityId abilityId)
        {            
            this.ability = UnitExtensions.GetAbilityById(EntityManager.LocalHero, abilityId);                        
            this.abilityId = abilityId;
        }

        public void SetItem(AbilityId abilityId)
        {
            this.ability = UnitExtensions.GetItemById(EntityManager.LocalHero, abilityId);            
            this.abilityId = abilityId;
        }
        public virtual bool CanBeCasted()
        {
            if (!this.abilityDictionary.ContainsKey(this.abilityId)) return false;
            
            if (this.ability == null) return false;            
            if (this.ability.Cooldown > 0f) return false;            
            if (this.ability.Level == 0) return false;
            
            if (EntityManager.LocalHero == null) return false;
            if (!EntityManager.LocalHero.IsAlive) return false;

            if (UnitExtensions.IsMuted(EntityManager.LocalHero)) return false;
            if (UnitExtensions.IsSilenced(EntityManager.LocalHero)) return false;
            if (UnitExtensions.IsStunned(EntityManager.LocalHero)) return false;
            if (UnitExtensions.IsChanneling(EntityManager.LocalHero)) return false;

            if (EntityManager.LocalHero.Mana < this.ability.ManaCost) return false;            
            return true;
        }

        public virtual bool CanBeCasted(Unit unit)
        {
            if (unit == null) return false;
            if (!unit.IsVisible) return false;
            if (!unit.IsAlive) return false;
            if (unit.IsMagicImmune()) return false;
            if (unit.IsInvulnerable()) return false;

            return true;
        }

        public bool Cast(Vector3 position, bool queue = false, bool bypass = false)
        {   
            return this.ability.Cast(position, queue, bypass);
        }
        public bool Cast(Unit unit, bool queue = false, bool bypass = false)
        {            
            return this.ability.Cast(unit, queue, bypass);
        }

        public bool Cast(bool queue = false, bool bypass = false)
        {         
            return this.ability.Cast(queue, bypass);
        }
    }
}
