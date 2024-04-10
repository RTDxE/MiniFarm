using UnityEngine;

namespace MiniFarm.Items
{
    [CreateAssetMenu(fileName = "SeedItem", menuName = "Farm/Items/Seed", order = 0)]
    public class SeedItem : Item
    {
        [Header("Seed")]
        public float growTime;
        public ItemInstance growItem;
        
        public float lostTime;
        public Sprite lostSprite;
    }
}