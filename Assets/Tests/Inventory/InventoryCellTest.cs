using System.Collections;
using System.Collections.Generic;
using MiniFarm.Items;
using NUnit.Framework;
using MiniFarm.ScriptableObjects;
using UnityEngine;

public class InventoryCellTest
{
    [Test]
    public void InventoryCellTestSimplePasses()
    {
        InventoryData inventoryData = ScriptableObject.CreateInstance<InventoryData>();
        var item = ScriptableObject.CreateInstance<SeedItem>();

        inventoryData.items = new List<ItemInstance>();
        inventoryData.items.Add(new ItemInstance() {item = item, count = 1});
        Assert.AreEqual(inventoryData.items[0].item.name, item.name);
        inventoryData.Save();
        inventoryData.Load();
        Assert.AreEqual(inventoryData.items[0].item.name, item.name);
    }

    public IEnumerator InventoryCellTestWithEnumeratorPasses()
    {
        yield return null;
    }
}