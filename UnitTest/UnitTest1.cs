using MediKeeperDemo.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sql;
using System;
using System.Collections.Generic;
using System.Linq;

namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            List<Item> items = new List<Item>();
            try
            {
                items = Connection.LoadItems();
                if (items.Count > 0)
                {
                    Connection.SaveItem(new Item { name = "Unit Test Item", cost = 100 });
                    items = Connection.LoadItems();
                    Item itemToTest = items.FirstOrDefault(x => x.name == "Unit Test Item");
                    if (itemToTest != null)
                    {
                        itemToTest.name = "Changed Name";
                        itemToTest.cost -= 1;
                        Connection.UpdateItem(itemToTest);
                        Item found = items.FirstOrDefault(x => x.name == "Changed Name");
                        if (found != null)
                        {
                            items = Connection.GetMaxPrice(found);
                            if (items.Count > 1)
                            {
                                Assert.Fail("More than one item found for single item call.");
                            }
                            items = Connection.LoadItems();
                            int count = items.Count;
                            Connection.DeleteItem(found);
                            items = Connection.LoadItems();
                            int countPostDelete = items.Count;
                            if (count == countPostDelete)
                            {
                                Assert.Fail("Failed to delete items");
                            }
                        }
                        else
                        {
                            Assert.Fail("Failed to update an item.");
                        }
                    }
                    else
                    {
                        Assert.Fail("Failed to save an item.");
                    }
                }
                else
                {
                    Assert.Fail("There are no items in the list!");
                }
            }
            catch (Exception e) {
                if (e.Message.Contains("no such table: ITEMS"))
                {
                    Connection.CreateTable();
                    Connection.SaveItem(new Item { name = "Unit Test Item", cost = 99 });
                }
                else { Assert.Fail(e.Message); }
            }
        }
    }
}
