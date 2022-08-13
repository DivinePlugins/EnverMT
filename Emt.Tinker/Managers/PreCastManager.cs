using Divine.Order;
using Divine.Order.EventArgs;
using Divine.Order.Orders.Components;

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
            if (e.Order?.Type != OrderType.Cast && e.Order?.Type != OrderType.CastTarget) return;

            foreach (var ability in PluginMenu.PreCastAbilities.Values)
            {
                if (e.Order?.Ability?.Id == ability.Key && ability.Value == true)
                {   
                    foreach (var item in PluginMenu.PreCastItems.Values)
                    {
                        if (item.Value == true)
                        {                            
                            CastManager.castItem(item.Key,null,false,false);
                        }
                    }
                }
            }

        }
    }
}
