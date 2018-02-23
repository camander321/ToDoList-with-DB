using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Tests
{
  [TestClass]
  public class ItemTest : IDisposable
  {
      public void ItemTests()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todolist_test;";
      }

      public void Dispose()
      {
        Item.DeleteAll();
        Category.DeleteAll();
      }

      [TestMethod]
      public void GetAll_DatabaseEmptyAtFirst_0()
      {
        //Arrange, Act
        int result = Item.GetAll().Count;

        //Assert
        Assert.AreEqual(0, result);
      }

      [TestMethod]
      public void Equals_ReturnsTrueIfDescriptionsAreTheSame_Item()
      {
        // Arrange
        System.DateTime dueDate = System.DateTime.Parse("03/15/2018");
        Item firstItem = new Item("Mow the lawn", dueDate, 1);
        Item secondItem = new Item("Mow the lawn", dueDate, 1);

        //Act
        firstItem.Save();
        secondItem.Save();


        // Assert
        Assert.AreEqual(true, firstItem.GetDescription().Equals(secondItem.GetDescription()));
      }

      [TestMethod]
      public void Save_SavesToDatabase_ItemList()
      {
        //Arrange
        string description = "Mow the Lawn";
        System.DateTime dueDate = System.DateTime.Parse("03/15/2018");
        Item testItem = new Item(description, dueDate, 1);

        //Act
        testItem.Save();
        List<Item> result = Item.GetAll();
        List<Item> testList = new List<Item>{testItem};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }

      [TestMethod]
      public void Save_DatabaseAssignsIdToObject_Id()
      {
        //Arrange
        string description = "Mow the Lawn";
        System.DateTime dueDate = System.DateTime.Parse("03/15/2018");
        Item testItem = new Item(description, dueDate, 1);
        testItem.Save();

        //Act
        Item savedItem = Item.GetAll()[0];

        int result = savedItem.GetId();
        int testId = testItem.GetId();

        //Assert
        Assert.AreEqual(testId, result);
      }

      [TestMethod]
      public void GetDescription_ReturnsDescription_String()
      {
        //Arrange
        string description = "Walk the dog.";
        System.DateTime dueDate = System.DateTime.Today;
        Item newItem = new Item(description, dueDate, 1);

        //Act
        string result = newItem.GetDescription();

        //Assert
        Assert.AreEqual(description, result);
      }

      [TestMethod]
      public void GetDueDate_ReturnsDueDate_DateTime()
      {
        //Arrange
        System.DateTime dueDate = System.DateTime.Today;
        Item newItem = new Item("Do Task", dueDate, 1);

        //Act
        DateTime result = newItem.GetDueDate();

        //Assert
        Assert.AreEqual(dueDate, result);
      }


      [TestMethod]
      public void GetAll_ReturnsItems_ItemList()
      {
        //Arrange
        string description01 = "Walk the dog";
        string description02 = "Wash the dishes";
        System.DateTime dueDate = System.DateTime.Today;
        Item newItem1 = new Item(description01, dueDate, 1);
        Item newItem2 = new Item(description02, dueDate, 1);
        List<Item> newList = new List<Item> {newItem1, newItem2};

        //Act
        newItem1.Save();
        newItem2.Save();
        List<Item> result = Item.GetAll();

        //Assert
        CollectionAssert.AreEqual(newList, result);
      }

      [TestMethod]
      public void Save_AssignsIdToObject_Id()
      {
        //Arrange
        System.DateTime dueDate = System.DateTime.Parse("03/15/2018");
        Item testItem = new Item("Mow the lawn", dueDate, 1);

        //Act
        testItem.Save();
        Item savedItem = Item.GetAll()[0];

        int result = savedItem.GetId();
        int testId = testItem.GetId();

        //Assert
        Assert.AreEqual(testId, result);
      }


      [TestMethod]
      public void Find_FindsItemInDatabase_Item()
      {
        //Arrange
        System.DateTime dueDate = System.DateTime.Parse("03/15/2018");
        Item testItem = new Item("Mow the lawn", dueDate, 1);
        testItem.Save();

        //Act
        Item foundItem = Item.Find(testItem.GetId());

        //Assert
        Assert.AreEqual(testItem, foundItem);
      }

      [TestMethod]
      public void Edit_UpdateItemsInDatabase_String()
      {
        //Arrange
        string firstDescription = "Walk the Dog";
        System.DateTime dueDate = System.DateTime.Parse("03/15/2018");
        Item testItem = new Item(firstDescription, dueDate, 1);
        testItem.Save();
        string secondDescription = "Mow the lawn";

        //Act
        testItem.Edit(secondDescription);
        string result = Item.Find(testItem.GetId()).GetDescription();

        //Assert
        Assert.AreEqual(secondDescription, result);
      }

      [TestMethod]
      public void Delete_DeleteOneItemInDatabase_True()
      {
        //Arrange
        string firstDescription = "Walk the Dog";
        string secondDescription = "Mow the lawn";
        System.DateTime dueDate = System.DateTime.Parse("03/16/2018");
        Item firstItem = new Item(firstDescription, dueDate, 1);
        Item secondItem = new Item(secondDescription, dueDate, 2);
        List<Item> testList = new List<Item>{secondItem};
        firstItem.Save();
        secondItem.Save();

        //Act
        int firstId = firstItem.GetId();
        firstItem.DeleteItem(firstId);
        List<Item> compareList = Item.GetAll();
        Console.WriteLine(compareList.Count);
        //Assert
        Assert.AreEqual(testList.Count, compareList.Count);
      }


    }
}
