using System.Collections.Generic;
using MiniFarm.Items;
using UnityEngine;

namespace MiniFarm.ScriptableObjects
{
    [CreateAssetMenu(fileName = "InventoryData", menuName = "Farm/Data/Inventory", order = 0)]
    public class InventoryData : ScriptableObject
    {
        public string inventoryName;
        public List<ItemInstance> items = new();

        public void Save()
        {
            PlayerPrefs.SetString($"Inventory_{inventoryName}", JsonUtility.ToJson(this));
        }

        public void Load()
        {
            if (PlayerPrefs.HasKey($"Inventory_{inventoryName}"))
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString($"Inventory_{inventoryName}"), this);
        }

        public ItemInstance Take(Item item, int count)
        {
            int itemsCount = 0;
            foreach (ItemInstance itemInstance in items)
            {
                if (itemInstance.item.GetType() == item.GetType() && itemInstance.count > 0)
                {
                    itemsCount += itemInstance.count;
                }
            }

            if (itemsCount < count)
                return null;

            int targetCount = count;

            foreach (ItemInstance itemInstance in items)
            {
                if (itemInstance.item.GetType() != item.GetType() || itemInstance.count <= 0) continue;
                if (targetCount < itemInstance.count)
                {
                    itemInstance.count -= targetCount;
                    break;
                }

                targetCount -= itemInstance.count;
                itemInstance.count = 0;
                itemInstance.item = null;

                if (targetCount == 0) break;
            }

            return new ItemInstance
            {
                item = item,
                count = count
            };
        }

        public void Give(ItemInstance item)
        {
            foreach (ItemInstance inventoryItem in items)
            {
                if (inventoryItem.item.GetType() == item.GetType())
                {
                    //TODO Может быть переполнение стака. Так что надо добавить дополнительную проверку
                    inventoryItem.count += item.count;
                }
            }
        }
    }
}