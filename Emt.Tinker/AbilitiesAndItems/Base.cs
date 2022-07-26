using System;
using Divine.Entity.Entities.Abilities;
using Divine.Entity.Entities.Units;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Extensions;
using Divine.Entity;
using Divine.Numerics;

namespace Emt_Tinker.AbilitiesAndItems
{
    internal class Base
    {
		private Ability Ability;
		public Base()
		{
		}
		public Base(Ability ability)
		{
			this.Ability = ability;
		}
		public void Update(Ability ability)
		{			
			this.Ability = ability;
		}
		public void UpdateItemToNull()
		{			
			this.Ability = null;
		}
		public Ability GetAbility()
		{
			return this.Ability;
		}

		public virtual bool CanBeCasted()
		{	
			if (this.Ability == null) return false;						
			if (this.Ability.Cooldown > 0f) return false;
			if (this.Ability.Level == 0) return false;

			Hero hero = this.Ability.Owner as Hero;

			if (hero == null) return false;
			if (!hero.IsAlive) return false;
			
			if (UnitExtensions.IsMuted(hero)) return false;
			if (UnitExtensions.IsSilenced(hero)) return false;
			if (UnitExtensions.IsStunned(hero)) return false;
			if (UnitExtensions.IsChanneling(hero)) return false;

			if (((Hero)this.Ability.Owner).Mana < (float)this.Ability.ManaCost) return false;			
			return true;
		}

		public virtual bool CanBeCasted(Unit unit)
		{
			if (!this.CanBeCasted()) return false;

			if (unit == null) return false;			
			if (!unit.IsVisible) return false;
			if (!unit.IsAlive) return false;
			if (unit.IsMagicImmune()) return false;
			if (unit.IsInvulnerable()) return false;

			return true;
		}

		public virtual bool Cast(Vector3 position, bool queue = false, bool bypass = false)
		{			
			if (this.Ability == null) return false;			
			return this.Ability.Cast(position, queue, bypass);
		}
		public virtual bool Cast(Unit unit, bool queue = false, bool bypass = false)
		{			
			if (this.Ability == null) return false;			
			if (this.Ability.Cooldown != 0) return false;
			if (this.Ability.Level == 0) return false;

			return this.Ability.Cast(unit, queue, bypass);
		}

		public virtual bool Cast(bool queue = false, bool bypass = false)
		{		
			if (this.Ability == null) return false;			
			return this.Ability.Cast(queue, bypass);
		}
	}
}
