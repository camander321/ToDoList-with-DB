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
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todo;";
      }

      public void Dispose()
      {
        Item.DeleteAll();
        Category.DeleteAll();
        categories_items.DeleteAll();
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
        Item firstItem = new Item("Mow the lawn", dueDate);
        Item secondItem = new Item("Mow the lawn", dueDate);

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
        Item testItem = new Item(description, dueDate);

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
        Item testItem = new Item(description, dueDate);
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
        Item newItem = new Item(description, dueDate);

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
        Item newItem = new Item("Do Task", dueDate);

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
        Item newItem1 = new Item(description01, dueDate);
        Item newItem2 = new Item(description02, dueDate);
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
        Item testItem = new Item("Mow the lawn", dueDate);

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
        Item testItem = new Item("Mow the lawn", dueDate);
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
        Item testItem = new Item(firstDescription, dueDate);
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
        Item firstItem = new Item(firstDescription, dueDate);
        Item secondItem = new Item(secondDescription, dueDate);
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

      [TestMethod]
      public void AddCategory_AddsCategoryToItem_CategoryList()
      {
        //Arrange
        System.DateTime dueDate = System.DateTime.Parse("03/16/2018");
        Item testItem1 = new Item("Mow the lawn", dueDate);
        testItem1.Save();

        Category testCategory = new Category("Home Stuff");
        testCategory.Save();

        //Act
        testItem1.AddCategory(testCategory);
        List<Category> result = testItem1.GetCategories();
        List<Category> testList = new List<Category>{testCategory};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }

      [TestMethod]
      public void GetCategories_ReturnsAllItemCategories_CategoryList()
      {
        //Arrange
        System.DateTime dueDate = System.DateTime.Parse("03/16/2018");
        Item testItem1 = new Item("Mow the lawn", dueDate);
        testItem1.Save();

        Category testCategory1 = new Category("Home stuff");
        testCategory1.Save();

        Category testCategory2 = new Category("Work stuff");
        testCategory2.Save();

        //Act
        testItem1.AddCategory(testCategory1);
        List<Category> result = testItem1.GetCategories();
        List<Category> testList = new List<Category> {testCategory1};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }

      [TestMethod]
      public void Delete_DeletesItemAssociationFromDatabase_ItemList()
      {
        //Arrange
        Category testCategory = new Category("Home stuff");
        testCategory.Save();

        string testDescription = "Mow the lawn";
        DateTime dueDate = DateTime.Parse("03/16/2018");
        Item testItem = new Item(testDescription, dueDate);
        testItem.Save();

        //Act
        testItem.AddCategory(testCategory);
        testItem.Delete();

        List<Item> resultCategoryItems = testCategory.GetItems();
        List<Item> testCategoryItems = new List<Item> {};

        //Assert
        CollectionAssert.AreEqual(testCategoryItems, resultCategoryItems);
      }

      [TestMethod]
      public void MarkAsDone_FlagsItemAsBeingCompleted_Void()
      {
        Item testItem1 = new Item("test 1", DateTime.Parse("03/16/2018"));
        testItem1.Save();
        testItem1.MarkAsDone(true);


        Item testItem2 = new Item("test 2", DateTime.Parse("03/17/2018"));
        testItem2.Save();
        testItem2.MarkAsDone(false);

        Item testItem3 = new Item("test 3", DateTime.Parse("03/18/2018"));
        testItem3.Save();
        testItem3.MarkAsDone(true);

        List<Item> testItems = new List<Item> {testItem1, testItem3};
        List<Item> resultItems = Item.GetCompleted(true);

        Assert.AreEqual(testItems.Count, resultItems.Count);
        CollectionAssert.AreEqual(testItems, resultItems);
      }
    }
}
