using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using ToDoList.Models;

namespace ToDoList.Tests
{
  [TestClass]
  public class CategoryTest : IDisposable
  {
      public void CategoryTests()
      {
        DBConfiguration.ConnectionString = "server=localhost;user id=root;password=root;port=8889;database=todolist_test;";
      }

      public void Dispose()
      {
        Item.DeleteAll();
        Category.DeleteAll();
      }

      [TestMethod]
      public void GetAll_CategoriesDatabaseEmptyAtFirst_0()
      {
        //Arrange, Act
        int result = Category.GetAll().Count;

        //Assert
        Assert.AreEqual(0, result);
      }

      [TestMethod]
      public void Equals_ReturnsTrueForSameName_Category()
      {
        //Arrange, Act
        Category firstCategory = new Category("Household chores");
        Category secondCategory = new Category("Household chores");

        //Assert
        Assert.AreEqual(firstCategory, secondCategory);
      }

      [TestMethod]
      public void Save_SavesCategoryToDatabase_CategoryList()
      {
        //Arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        //Act
        List<Category> result = Category.GetAll();
        List<Category> testList = new List<Category>{testCategory};

        //Assert
        CollectionAssert.AreEqual(testList, result);
      }

      [TestMethod]
      public void Save_DatabaseAssignsIdToCategory_Id()
      {
        //Arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        //Act
        Category savedCategory = Category.GetAll()[0];

        int result = savedCategory.GetId();
        int testId = testCategory.GetId();

        //Assert
        Assert.AreEqual(result, testId);
      }

      [TestMethod]
      public void Find_FindsCategoryInDatabase_Category()
      {
        //Arrange
        Category testCategory = new Category("Household chores");
        testCategory.Save();

        //Act
        Category foundCategory = Category.Find(testCategory.GetId());

        //Assert
        Assert.AreEqual(testCategory, foundCategory);
      }

      // [TestMethod]
      // public void GetItems_RetrievesAllItemsWithCategory_ItemList()
      // {
      //   Category testCategory = new Category("Household chores");
      //   testCategory.Save();
      //
      //   string description1 = "Mow the Lawn";
      //   System.DateTime dueDate = System.DateTime.Parse("03/15/2018");
      //   Item firstItem = new Item(description1, dueDate, testCategory.GetId());
      //   firstItem.Save();
      //
      //   string description2 = "Wash the dishes";
      //   Item secondItem = new Item(description2, dueDate, testCategory.GetId());
      //   secondItem.Save();
      //
      //   List<Item> testItemList = new List<Item> {firstItem, secondItem};
      //   List<Item> resultItemList = testCategory.GetItems();
      //
      //   CollectionAssert.AreEqual(testItemList, resultItemList);
      // }
  }
}
