using Divine.Entity;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Extensions;

namespace Tinker.AbilitiesAndItems
{
    internal class Abilities : Base
    {
		public readonly Base defenseMatrix;
		public readonly Base heatSeekingMissile;
		public readonly Base keenConveyance;
		public readonly Base laser;		
		public readonly Base warpGrenade;
		public readonly Base rearm;		
		public Abilities()
		{
			this.defenseMatrix = new Base();
			this.heatSeekingMissile = new Base();
			this.keenConveyance = new Base();
			this.laser = new Base();			
			this.warpGrenade = new Base();
			this.rearm = new Base();
		}
		public void Update()
		{			
			Hero localHero = EntityManager.LocalHero;
			if (localHero == null) return;
			this.defenseMatrix.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_defense_matrix));
			this.heatSeekingMissile.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_heat_seeking_missile));
			this.keenConveyance.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_keen_teleport));
			this.laser.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_laser));			
			this.warpGrenade.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_warp_grenade));
			this.rearm.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_rearm));			
		}				
	}
}
