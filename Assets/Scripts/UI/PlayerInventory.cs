namespace MiniFarm.UI
{
    public class PlayerInventory : Inventory
    {
        private ItemCell _pressedItemCell;

        public override void CellPressed(ItemCell cell)
        {
            int cellIdx = cell.transform.GetSiblingIndex();
            if (_pressedItemCell == null)
            {
                if (inventory.items[cellIdx].item != null && inventory.items[cellIdx].count > 0)
                {
                    _pressedItemCell = cell;
                    cell.MarkPressed();
                }
            }
            else
            {
                inventory.Swap(cellIdx, _pressedItemCell.transform.GetSiblingIndex());
                ResetInventoryState();
            }
        }

        public void ResetInventoryState()
        {
            if (_pressedItemCell == null) return;
            _pressedItemCell.UnmarkPressed();
            _pressedItemCell = null;
        }
    }
}