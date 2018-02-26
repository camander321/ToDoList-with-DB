using System.Collections.Generic;
using System;
using ToDoList;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace ToDoList.Models
{
  public class Category
  {
    private int _id;
    private string _categoryName;

    public Category(string CategoryName, int Id = 0)
    {
      _id = Id;
      _categoryName = CategoryName;
    }

    public override bool Equals(System.Object otherCat)
    {
      if (!(otherCat is Category))
      {
        return false;
      }
      else
      {
        Category newCat = (Category) otherCat;
        bool idEquality = (this.GetId() == newCat.GetId());
        bool categoryNameEquality = (this.GetCategoryName() == newCat.GetCategoryName());
        return (idEquality && categoryNameEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public string GetCategoryName()
    {
      return _categoryName;
    }

    public void SetCategoryName(string categoryName)
    {
      _categoryName = categoryName;
    }

    public int GetId()
    {
      return _id;
    }

    public void SetId(int id)
    {
      _id = id;
    }

    public static List<Category> GetAll()
    {
      List<Category> allCategories = new List<Category> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int categoryId = rdr.GetInt32(0);
        string categoryName = rdr.GetString(1);
        Category newCategory = new Category(categoryName, categoryId);
        allCategories.Add(newCategory);
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
      return allCategories;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE categories;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static void ResetId()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = "UPDATE categories SET id = 0";
      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void Save()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"INSERT INTO categories (categoryName) VALUES (@categoryName);";

      MySqlParameter categoryName = new MySqlParameter();
      categoryName.ParameterName = "@categoryName";
      categoryName.Value = this._categoryName;
      cmd.Parameters.Add(categoryName);

      cmd.ExecuteNonQuery();
      _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static Category Find(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM categories WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = id;
      cmd.Parameters.Add(searchId);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      int categoryId = 0;
      string categoryName = "";

      while (rdr.Read())
      {
        categoryId = rdr.GetInt32(0);
        categoryName = rdr.GetString(1);
      }

      Category foundCategory = new Category(categoryName, categoryId);

       conn.Close();
       if (conn != null)
       {
         conn.Dispose();
       }

      return foundCategory;
    }

    public void Edit(string newCategoryName)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE categories SET categoryName = @newCategoryName WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter categoryName = new MySqlParameter();
      categoryName.ParameterName = "@newCategoryName";
      categoryName.Value = newCategoryName;
      cmd.Parameters.Add(categoryName);

      cmd.ExecuteNonQuery();
      _categoryName = newCategoryName;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public List<Item> GetItems()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT item_id FROM categories_items WHERE category_id = @category_id;";

      MySqlParameter categoryIdParameter = new MySqlParameter();
      categoryIdParameter.ParameterName = "@category_id";
      categoryIdParameter.Value = _id;
      cmd.Parameters.Add(categoryIdParameter);

      var rdr = cmd.ExecuteReader() as MySqlDataReader;

      List<int> itemIds = new List<int> {};
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        itemIds.Add(itemId);
      }
      rdr.Dispose();

      List<Item> items = new List<Item> {};
      foreach (int itemId in itemIds)
      {
        var itemQuery = conn.CreateCommand() as MySqlCommand;
        itemQuery.CommandText = @"SELECT * FROM categories WHERE id = @ItemId;";

        MySqlParameter itemIdParameter = new MySqlParameter();
        itemIdParameter.ParameterName = "@CategoryId";
        itemIdParameter.Value = itemId;
        itemQuery.Parameters.Add(itemIdParameter);

        var itemQueryRdr = itemQuery.ExecuteReader() as MySqlDataReader;
        while(itemQueryRdr.Read())
        {
          int thisItemId = itemQueryRdr.GetInt32(0);
          string itemDescription = itemQueryRdr.GetString(1);
          DateTime itemDueDate = itemQueryRdr.GetDateTime(2);
          Item foundItem = new Item(itemDescription, itemDueDate, thisItemId);
          items.Add(foundItem);
        }
      }
      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

      return items;
    }
  }
}
