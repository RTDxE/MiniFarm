using System;
using MiniFarm.Items;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFarm.UI
{
    [RequireComponent(typeof(Button))]
    public class ItemCell : MonoBehaviour
    {
        [SerializeField] private Image image;
        [SerializeField] private TMP_Text count;

        private Button _cellButton;
        private Inventory _inventory;
        
        private void Start()
        {
            _cellButton = GetComponent<Button>();
            _cellButton.onClick.AddListener(Pressed);
        }

        public void SetInventory(Inventory inventory)
        {
            _cellButton.onClick.RemoveAllListeners();
            _inventory = inventory;
            if (_inventory != null)
                _cellButton.onClick.AddListener(Pressed);
        }

        private void Pressed()
        {
            _inventory.CellPressed(this);
        }

        public void SetItem(ItemInstance item)
        {
            if (item.item == null || item.count <= 0)
            {
                image.sprite = null;
                count.text = "";
            }
            else
            {
                image.sprite = item.item.sprite;
                if (item.count > 1)
                {
                    count.text = item.count.ToString();
                }
                else
                {
                    count.text = "";
                }
            }
        }
    }
}