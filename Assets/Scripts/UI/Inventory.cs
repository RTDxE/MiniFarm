using MiniFarm.ScriptableObjects;
using UnityEngine;

namespace MiniFarm.UI
{
    public abstract class Inventory : MonoBehaviour
    {
        [SerializeField] protected InventoryData inventory;
        [SerializeField] private Transform cells;

        private void Start()
        {
            inventory.Load();
            UpdateInventory();

            ItemCell _itemCell;
            for (var idx = 0; idx < cells.childCount; idx++)
            {
                cells.GetChild(idx).TryGetComponent(out _itemCell);
                _itemCell.SetInventory(this);
            }
        }

        private void OnDestroy()
        {
            inventory.Save();
        }

        public abstract void CellPressed(ItemCell cell);

        private void UpdateInventory()
        {
            for (var idx = 0; idx < Mathf.Min(cells.childCount, inventory.items.Count); idx++)
            {
                //TODO очень некрасивая конструкция, так как постоянно вызывается GetComponent
                cells.GetChild(idx).GetComponent<ItemCell>().SetItem(inventory.items[idx]);
            }
        }
    }
}