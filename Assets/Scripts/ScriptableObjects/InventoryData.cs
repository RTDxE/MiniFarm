using System;
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

        public Action onInventoryUpdated;

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
                if (itemInstance.item == null) continue;
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
            
            onInventoryUpdated?.Invoke();

            return new ItemInstance
            {
                item = item,
                count = count
            };
        }

        public void Give(ItemInstance item)
        {
            int itemCount = item.count;
            if (!item.item.stackable)
            {
                foreach (ItemInstance inventoryItem in items)
                {
                    if (inventoryItem.item != null) continue;
                    inventoryItem.item = item.item;
                    inventoryItem.count = itemCount;
                    break;
                }
            }
            else
            {
                foreach (ItemInstance inventoryItem in items)
                {
                    if (inventoryItem.item == null)
                        continue;

                    if (inventoryItem.item.GetType() != item.item.GetType()) continue;
                    
                    // Если можно все засунуть в один слот
                    if (item.item.stackCount - inventoryItem.count > itemCount)
                    {
                        inventoryItem.count += itemCount;
                        itemCount = 0;

                        break;
                    }
                    // а если нет, то засовываем сколько можем и идем дальше
                    else
                    {
                        inventoryItem.count = item.item.stackCount;
                        itemCount = item.item.stackCount - inventoryItem.count;
                    }
                }

                if (itemCount > 0)
                {
                    foreach (ItemInstance inventoryItem in items)
                    {
                        if (inventoryItem.item != null) continue;
                        inventoryItem.item = item.item;
                        inventoryItem.count = itemCount;
                        break;
                    }
                }
            }


            onInventoryUpdated?.Invoke();
        }

        public void Swap(int fromIdx, int toIdx)
        {
            (items[fromIdx], items[toIdx]) = (items[toIdx], items[fromIdx]);
            onInventoryUpdated?.Invoke();
        }
    }
}