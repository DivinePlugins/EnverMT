using Divine.Entity;
using Divine.Entity.Entities.Units.Heroes;
using Divine.Extensions;
using Divine.Helpers;

namespace Tinker.AbilitiesAndItems
{
    internal class Abilities
    {
		public Abilities()
		{
			this.defenseMatrix = new Abilities.DefenseMatrix();
			this.heatSeekingMissile = new Abilities.HeatSeekingMissile();
			this.keenConveyance = new Abilities.KeenConveyance();
			this.laser = new Abilities.Laser();
			this.marchOfTheMachines = new Abilities.MarchOfTheMachines();
			this.warpGrenade = new Abilities.WarpGrenade();
			this.rearm = new Abilities.Rearm();
		}

		public void Update()
		{
			if (!this.UpdateSleeper.Sleeping)
			{
				Hero localHero = EntityManager.LocalHero;
				if (localHero == null)
				{
					return;
				}
				this.defenseMatrix.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_defense_matrix));
				this.heatSeekingMissile.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_heat_seeking_missile));
				this.keenConveyance.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_keen_teleport));
				this.laser.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_laser));
				this.marchOfTheMachines.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_march_of_the_machines));
				this.warpGrenade.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_warp_grenade));
				this.rearm.Update(UnitExtensions.GetAbilityById(localHero, Divine.Entity.Entities.Abilities.Components.AbilityId.tinker_rearm));
				this.UpdateSleeper.Sleep(500f);
			}
		}

		private readonly Sleeper UpdateSleeper = new Sleeper();
		public readonly Abilities.DefenseMatrix defenseMatrix;
		public readonly Abilities.HeatSeekingMissile heatSeekingMissile;
		public readonly Abilities.KeenConveyance keenConveyance;
		public readonly Abilities.Laser laser;
		public readonly Abilities.MarchOfTheMachines marchOfTheMachines;
		public readonly Abilities.WarpGrenade warpGrenade;
		public readonly Abilities.Rearm rearm;
		internal class DefenseMatrix : Base { }
		internal class HeatSeekingMissile : Base { }
		internal class KeenConveyance : Base { }
		internal class Laser : Base { }
		internal class MarchOfTheMachines : Base { }
		internal class WarpGrenade : Base { }
		internal class Rearm : Base { }
	}
}
