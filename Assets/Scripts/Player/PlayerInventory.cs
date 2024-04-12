using System.Collections.Generic;
using MiniFarm.Items;
using MiniFarm.UI;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFarm
{
    public class PlayerInventory : MonoBehaviour
    {
        [SerializeField] private ItemCell _activeItemCell;
        [SerializeField] private List<ItemCell> _inventoryCells = new();

        private Inventory _inventory;

        public ItemInstance ActiveItem => _inventory.items[_inventory.items.Count - 1];

        private ItemCell _pressedItemCell;
        
        private void Start()
        {
            // Заполнение инвентаря
            if (!PlayerPrefs.HasKey("PlayerInventory"))
            {
                _inventory = new Inventory();
                // Ячейки инвентаря
                foreach (ItemCell itemCell in _inventoryCells)
                {
                    _inventory.items.Add(new ItemInstance());
                }
                
                // Активный айтем
                _inventory.items.Add(new ItemInstance());
            }
            else
            {
                LoadInventory();
            }
            
            UpdateInventory();
            
            // Привязка кнопок в инвентаре

            foreach (ItemCell itemCell in _inventoryCells)
            {
                itemCell.GetComponent<Button>().onClick.AddListener(() => CellPressed(itemCell));
            }
        }
        
        private void LoadInventory()
        {
            if (PlayerPrefs.HasKey("PlayerInventory"))
                JsonUtility.FromJsonOverwrite(PlayerPrefs.GetString("PlayerInventory"), _inventory);
        }
        
        private void SaveInventory()
        {
            PlayerPrefs.SetString("PlayerInventory", JsonUtility.ToJson(_inventory));
        }

        public ItemInstance TakeActiveItem(int count = 1)
        {
            ItemInstance item = _inventory.items[0];
            if (item == null)
                return null;

            if (item.count < count)
                return null;

            item.count -= count;

            UpdateInventory();

            return new ItemInstance()
            {
                item = item.item,
                count = count
            };
        }

        private void UpdateInventory()
        {
            SaveInventory();

            for (var index = 0; index < _inventory.items.Count; index++)
            {
                var itemInstance = _inventory.items[index];

                if (index == _inventory.items.Count - 1)
                {
                    _activeItemCell.SetItem(itemInstance);
                }
                else
                {
                    _inventoryCells[index].SetItem(itemInstance);
                }
            }
        }

        public void GiveItem(ItemInstance item)
        {
            int itemCount = item.count;
            if (!item.item.stackable)
            {
                foreach (ItemInstance inventoryItem in _inventory.items)
                {
                    if (inventoryItem.item != null) continue;
                    inventoryItem.item = item.item;
                    inventoryItem.count = itemCount;
                    break;
                }
            }
            else
            {
                foreach (ItemInstance inventoryItem in _inventory.items)
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
                    foreach (ItemInstance inventoryItem in _inventory.items)
                    {
                        if (inventoryItem.item != null) continue;
                        inventoryItem.item = item.item;
                        inventoryItem.count = itemCount;
                        break;
                    }
                }
            }
            UpdateInventory();
        }
        
        public void CellPressed(ItemCell cell)
        {
            int cellIdx = GetCellIdx(cell);
            
            if (cellIdx < 0) return;
            
            if (_pressedItemCell == null)
            {
                if (_inventory.items[cellIdx].item != null && _inventory.items[cellIdx].count > 0)
                {
                    _pressedItemCell = cell;
                    cell.MarkPressed();
                }
            }
            else
            {
                int pressedCellIdx = GetCellIdx(_pressedItemCell);
                if (cellIdx != pressedCellIdx)
                    Swap(cellIdx, pressedCellIdx);
                ResetInventoryState();
            }
        }

        private int GetCellIdx(ItemCell cell)
        {
            if (cell == _activeItemCell)
            {
                return _inventory.items.Count - 1;
            }
            else
            {
                for (var index = 0; index < _inventoryCells.Count; index++)
                {
                    var itemCell = _inventoryCells[index];
                    if (itemCell == cell)
                    {
                        return index;
                    }
                }
            }

            return -1;
        }
        
        private void Swap(int fromIdx, int toIdx)
        {
            (_inventory.items[fromIdx], _inventory.items[toIdx]) = (_inventory.items[toIdx], _inventory.items[fromIdx]);
            UpdateInventory();
        }

        public void ResetInventoryState()
        {
            if (_pressedItemCell == null) return;
            _pressedItemCell.UnmarkPressed();
            _pressedItemCell = null;
        }
    }
}