using System.Collections.Generic;
using MiniFarm.Items;
using UnityEngine;

namespace MiniFarm
{
    public class Garden : MonoBehaviour
    {
        [SerializeField] private SeedItem _seedItem;
        [SerializeField] private float _currentTime = 0;

        [SerializeField] private Sprite plowSprite;
        [SerializeField] private SpriteRenderer itemSprite;

        public GardenState CurrentState => _state;
        private GardenState _state = GardenState.EMPTY;

        public void Interact(Player.Player player)
        {
            switch (_state)
            {
                case GardenState.EMPTY when
                    player.PlayerInventory.ActiveItem.item is SeedItem :
                    Plant(player);
                    break;
                case GardenState.READY:
                case GardenState.LOST:
                    Collect(player);
                    break;
                case GardenState.PLOW when
                    player.PlayerInventory.ActiveItem.item is ToolItem item &&
                    item.type == ToolType.HOE &&
                    ((ToolItemInstance)player.PlayerInventory.ActiveItem).currentDurability > 0:
                    Plow(player);
                    break;
            }
        }

        public void Plant(Player.Player player)
        {
            var item = player.PlayerInventory.TakeActiveItem();
            if (item == null)
                return;

            _seedItem = item.item as SeedItem;

            NextState(GardenState.GROW);
        }

        public void Collect(Player.Player player)
        {
            if (_state == GardenState.READY)
                player.PlayerInventory.GiveItem(_seedItem.growItem);
            
            NextState(GardenState.PLOW);
        }

        public void Plow(Player.Player player)
        {
            ((ToolItemInstance)player.PlayerInventory.ActiveItem).currentDurability -= 1;

            NextState(GardenState.EMPTY);
        }

        public void NextState(GardenState nextState)
        {
            _currentTime = 0;
            switch (nextState)
            {
                case GardenState.EMPTY:
                    itemSprite.sprite = null;
                    break;
                case GardenState.GROW:
                    itemSprite.sprite = _seedItem.sprite;
                    break;
                case GardenState.READY:
                    itemSprite.sprite = _seedItem.growItem.item.sprite;
                    break;
                case GardenState.PLOW:
                    itemSprite.sprite = plowSprite;
                    break;
                case GardenState.LOST:
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

        public string Save()
        {
            return JsonUtility.ToJson(new Dictionary<string, object>
            {
                { "SeedItem", _seedItem },
                { "CurrentTime", _currentTime },
                { "CurrentState", CurrentState }
            });

        }

        public void Load(string data)
        {
            Dictionary<string, object> loadedData = JsonUtility.FromJson<Dictionary<string, object>>(data);
            
            _seedItem = loadedData["SeedItem"] as SeedItem;
            _currentTime = loadedData["CurrentTime"] as float? ?? 0;
            _state = loadedData["CurrentState"] is GardenState ? (GardenState)loadedData["CurrentState"] : GardenState.EMPTY;

            NextState(_state);
        }
    }
}