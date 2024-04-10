using UnityEngine;

namespace MiniFarm.Items
{
    public class Item : ScriptableObject
    {
        public string name;
        public Sprite sprite;

        public bool stackable;
        public int stackCount;
        
        [Header("Buy")]
        public float buyPrice;
        public int buyCount;
        
        [Header("Sell")]
        public bool sellable;
        public float sellPrice;
    }
}