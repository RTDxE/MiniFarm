using UnityEngine;

namespace MiniFarm.Items
{
    [CreateAssetMenu(fileName = "ToolItem", menuName = "Farm/Items/Tool", order = 0)]
    public class ToolItem : Item
    {
        [Header("Tool")] 
        public ToolType type;
        public int durability;
    }

    [System.Serializable]
    public enum ToolType
    {
        HOE,
        AXE,
        SHOVEL
    }
}