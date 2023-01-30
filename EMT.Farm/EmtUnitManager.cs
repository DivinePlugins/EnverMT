﻿using Divine.Entity;
using Divine.Entity.Entities.Units;
using Divine.Extensions;
using Divine.Update;


namespace EMT.Farm
{
    internal class EmtUnitManager : IDisposable
    {
        private readonly Context context;
        private SortedDictionary<uint, EmtUnit> unitsTracker;
        private float trackRange;

        public EmtUnitManager(Context context)
        {
            this.context = context;
            this.unitsTracker = new();
            this.context.pluginMenu.pluginStatus.ValueChanged += PluginStatus_ValueChanged;
        }
        public void Dispose()
        {
            this.context.pluginMenu.pluginStatus.ValueChanged -= PluginStatus_ValueChanged;
            UpdateManager.DestroyIngameUpdate(Update);
            this.unitsTracker.ForEach(u=>u.Value.Dispose());
        }
        private void PluginStatus_ValueChanged(Divine.Menu.Items.MenuSwitcher switcher, Divine.Menu.EventArgs.SwitcherEventArgs e)
        {
            if (e.Value)
            {
                UpdateManager.CreateIngameUpdate(Update);
            }
            else
            {
                UpdateManager.DestroyIngameUpdate(Update);
                this.unitsTracker.ForEach(u => u.Value.Dispose());
            }
        }
        private void Update()
        {
            Divine.Entity.Entities.Units.Heroes.Hero localHero = EntityManager.LocalHero!;
            if (localHero == null) return;
            this.trackRange = (float)localHero.AttackRange + (float)context.pluginMenu.additionalRange.Value;

            this.AddUnitsToTracker();
            this.RemoveNotAliveUnitsFromTracker();
        }

        private void AddUnitsToTracker()
        {
            List<Unit> units = UnitExtensions.GetUnitsInRange<Unit>(EntityManager.LocalHero!, this.trackRange)
            .Where(u =>
                u.Type == Divine.Entity.Entities.Components.EntityType.Creep ||
                u.Type == Divine.Entity.Entities.Components.EntityType.Courier
                )
            .Where(u => !unitsTracker.ContainsKey(u.Handle) && u.IsValid).ToList();

            foreach (Unit u in units)
            {
                unitsTracker.Add(u.Handle, new EmtUnit(u));
            }
        }

        private void RemoveNotAliveUnitsFromTracker()
        {
            float additionalRange = 500f; // Additional range to remove Unit from Tracklist            
            var dict = this.unitsTracker.Where(u =>
                !u.Value.unit.IsAlive ||
                u.Value.unit.Distance2D(EntityManager.LocalHero!) > (this.trackRange + additionalRange) ||
                !u.Value.unit.IsVisible ||
                !u.Value.unit.IsValid).ToDictionary(t => t.Key, t => t.Value);

            foreach (var u in dict)
            {
                u.Value.Dispose();
                this.unitsTracker.Remove(u.Value.unit.Handle);
            }
        }
    }
}