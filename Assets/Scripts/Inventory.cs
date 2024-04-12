using System;
using System.Collections.Generic;
using MiniFarm.Items;

namespace MiniFarm
{
    [Serializable]
    public class Inventory
    {
        public List<ItemInstance> items = new();
    }
}