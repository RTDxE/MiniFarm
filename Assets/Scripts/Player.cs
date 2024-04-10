using MiniFarm.Items;
using MiniFarm.ScriptableObjects;
using UnityEngine;

namespace MiniFarm
{
    public class Player: MonoBehaviour
    {
        [SerializeField] private InventoryData _inventory;
        public InventoryData Inventory => _inventory;

        private int _activeItemIdx;
        public ItemInstance ActiveItem => _inventory.items[_activeItemIdx];

        public ItemInstance TakeActiveItem(int count = 1)
        {
            return Inventory.Take(ActiveItem.item, count);
        }

        public void GiveItem(ItemInstance item)
        {
            Inventory.Give(item);
        }
    }
}