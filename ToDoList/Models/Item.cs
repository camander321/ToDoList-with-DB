using System.Collections.Generic;
using System;
using ToDoList;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace ToDoList.Models
{
  public class Item
  {
    private int _id;
    private string _description;
    private System.DateTime _dueDate;
    private int _categoryId;

    public Item(string Description, DateTime DueDate, int categoryId, int Id = 0)
    {
      _id = Id;
      _description = Description;
      _dueDate = DueDate;
      _categoryId = categoryId;
    }

    public override bool Equals(System.Object otherItem)
    {
      if (!(otherItem is Item))
      {
        return false;
      }
      else
      {
        Item newItem = (Item) otherItem;
        bool idEquality = (this.GetId() == newItem.GetId());
        bool descriptionEquality = (this.GetDescription() == newItem.GetDescription());
        bool dueDateEquality = (this.GetDueDate() == newItem.GetDueDate());
        bool categoryEquality = (this.GetCategoryId() == newItem.GetCategoryId());
        return (idEquality && descriptionEquality && dueDateEquality && categoryEquality);
      }
    }

    public override int GetHashCode()
    {
      return this.GetId().GetHashCode();
    }

    public int GetId()
    {
      return _id;
    }
    public void SetId(int newId)
    {
      _id = newId;
    }
    public string GetDescription()
    {
      return _description;
    }
    public void SetDescription(string newDescription)
    {
      _description = newDescription;
    }
    public DateTime GetDueDate()
    {
      return _dueDate;
    }
    public void SetDueDate(DateTime newDueDate)
    {
      _dueDate = newDueDate;
    }
    public int GetCategoryId()
    {
      return _categoryId;
    }

    public static List<Item> GetAll()
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items;";
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        DateTime itemDueDate = rdr.GetDateTime(2);
        int itemCategoryId = rdr.GetInt32(3);
        Item newItem = new Item(itemDescription, itemDueDate, itemCategoryId, itemId);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allItems;
    }

    public static void DeleteAll()
    {
     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"TRUNCATE TABLE items;";

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
      cmd.CommandText = "UPDATE items SET id = 0, category_id = 0";
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
    cmd.CommandText = @"INSERT INTO items (description, dueDate, category_id) VALUES (@ItemDescription, @ItemDueDate, @category_id);";

     MySqlParameter description = new MySqlParameter();
     description.ParameterName = "@ItemDescription";
     description.Value = this._description;
     cmd.Parameters.Add(description);

     MySqlParameter dueDate = new MySqlParameter();
     dueDate.ParameterName = "@ItemDueDate";
     dueDate.Value = this._dueDate;
     cmd.Parameters.Add(dueDate);

     MySqlParameter category_id = new MySqlParameter();
     category_id.ParameterName = "@category_id";
     category_id.Value = this._categoryId;
     cmd.Parameters.Add(category_id);

     cmd.ExecuteNonQuery();
     _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void CategoryLookup()
    {
      //Is category_id equal to categories Database descriptions?
    }

    public static Item Find(int id)
    {
     MySqlConnection conn = DB.Connection();
     conn.Open();

     var cmd = conn.CreateCommand() as MySqlCommand;
     cmd.CommandText = @"SELECT * FROM items WHERE id = @searchId;";

     MySqlParameter searchId = new MySqlParameter();
     searchId.ParameterName = "@searchId";
     searchId.Value = id;
     cmd.Parameters.Add(searchId);

     var rdr = cmd.ExecuteReader() as MySqlDataReader;

     int itemId = 0;
     string itemDescription = "";
     DateTime itemDueDate = DateTime.Now;
     int itemCategoryId = 0;

     while (rdr.Read())
     {
       itemId = rdr.GetInt32(0);
       itemDescription = rdr.GetString(1);
       itemDueDate = rdr.GetDateTime(2);
       itemCategoryId = rdr.GetInt32(3);
     }

     Item foundItem= new Item(itemDescription, itemDueDate, itemCategoryId, itemId);

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }

     return foundItem;
    }

    public void Edit(string newDescription)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET description = @newDescription WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter description = new MySqlParameter();
      description.ParameterName = "@newdescription";
      description.Value = newDescription;
      cmd.Parameters.Add(description);

      cmd.ExecuteNonQuery();
      _description = newDescription;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void EditDueDate(System.DateTime newDueDate)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET dueDate = @newDueDate WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      MySqlParameter dueDate = new MySqlParameter();
      dueDate.ParameterName = "@newDueDate";
      dueDate.Value = newDueDate;
      cmd.Parameters.Add(dueDate);

      cmd.ExecuteNonQuery();
      _dueDate = newDueDate;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void DeleteItem(int id)
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items WHERE id = @searchId;";

      MySqlParameter searchId = new MySqlParameter();
      searchId.ParameterName = "@searchId";
      searchId.Value = _id;
      cmd.Parameters.Add(searchId);

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public static List<Item> FilterDueDate()
    {
      List<Item> filterByDueDate = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      MySqlCommand cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items ORDER BY dueDate;";
      MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        DateTime itemDueDate = rdr.GetDateTime(2);
        int itemCategoryId = rdr.GetInt32(3);
        Item newItem = new Item(itemDescription, itemDueDate, itemCategoryId, itemId);
        filterByDueDate.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return filterByDueDate;
    }
  }
}
