using System.Collections;
using MiniFarm.Items;
using UnityEngine;
using UnityEngine.UI;

namespace MiniFarm
{
    public class Garden : MonoBehaviour
    {
        [SerializeField] private SeedItem _seedItem;
        [SerializeField] private float _currentTime = 0;

        [SerializeField] private Image itemSprite;

        public GardenState CurrentState => _state;
        private GardenState _state = GardenState.EMPTY;

        public void Interact(Player player)
        {
            switch (_state)
            {
                case GardenState.EMPTY when
                    player.ActiveItem.GetType() == typeof(SeedItem):
                    Plant(player);
                    break;
                case GardenState.READY:
                case GardenState.LOST:
                    Collect(player);
                    break;
                case GardenState.PLOW when
                    player.ActiveItem.item.GetType() == typeof(ToolItem) &&
                    ((ToolItem)player.ActiveItem.item).type == ToolType.HOE &&
                    ((ToolItemInstance)player.ActiveItem).currentDurability > 0:
                    Plow(player);
                    break;
            }
        }

        public void Plant(Player player)
        {
            var item = player.TakeActiveItem();
            if (item == null)
                return;

            _seedItem = item.item as SeedItem;

            NextState(GardenState.GROW);
        }

        public void Collect(Player player)
        {
            if (_state == GardenState.READY)
                player.GiveItem(_seedItem.growItem);
        }

        public void Plow(Player player)
        {
            ((ToolItemInstance)player.ActiveItem).currentDurability -= 1;

            _state = GardenState.EMPTY;
        }

        public void NextState(GardenState nextState)
        {
            switch (nextState)
            {
                case GardenState.EMPTY:
                    itemSprite.sprite = null;
                    break;
                case GardenState.GROW:
                    itemSprite.sprite = _seedItem.sprite;
                    break;
                case GardenState.READY:
                    _currentTime = 0;
                    itemSprite.sprite = _seedItem.growItem.item.sprite;
                    break;
                case GardenState.PLOW:
                    itemSprite.sprite = null;
                    break;
                case GardenState.LOST:
                    _currentTime = 0;
                    itemSprite.sprite = _seedItem.lostSprite;
                    break;
            }

            _state = nextState;
        }

        private void Update()
        {
            if (_seedItem != null)
            {
                switch (_state)
                {
                    case GardenState.GROW:
                    {
                        _currentTime += Time.deltaTime;
                        if (!(_currentTime >= _seedItem.growTime)) return;
                        NextState(GardenState.READY);

                        break;
                    }
                    case GardenState.READY:
                    {
                        _currentTime += Time.deltaTime;
                        if (!(_currentTime >= _seedItem.lostTime)) return;
                        NextState(GardenState.LOST);

                        break;
                    }
                }
            }
        }
    }
}