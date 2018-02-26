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
    private bool _isDone = false;

    public Item(string Description, DateTime DueDate, int Id = 0)
    {
      _id = Id;
      _description = Description;
      _dueDate = DueDate;
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
        return (idEquality && descriptionEquality && dueDateEquality);
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

    public bool GetIsDone()
    {
      return _isDone;
    }

    public void SetIsDone(bool isDone)
    {
      _isDone = isDone;
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
        Item newItem = new Item(itemDescription, itemDueDate, itemId);
        bool isDone = rdr.GetBoolean(3);
        newItem.SetIsDone(isDone);
        allItems.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return allItems;
    }

    public static List<Item> GetCompleted(bool isDone)
    {
      List<Item> allItems = new List<Item> {};
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"SELECT * FROM items WHERE isDone = @IsDone;";
      cmd.Parameters.Add(new MySqlParameter("@IsDone", isDone));
      var rdr = cmd.ExecuteReader() as MySqlDataReader;
      while(rdr.Read())
      {
        int itemId = rdr.GetInt32(0);
        string itemDescription = rdr.GetString(1);
        DateTime itemDueDate = rdr.GetDateTime(2);
        Item newItem = new Item(itemDescription, itemDueDate, itemId);
        newItem.SetIsDone(isDone);
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
      cmd.CommandText = "UPDATE items SET id = 0";
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
    cmd.CommandText = @"INSERT INTO items (description, dueDate, isDone) VALUES (@ItemDescription, @ItemDueDate, @IsDone);";

     MySqlParameter description = new MySqlParameter();
     description.ParameterName = "@ItemDescription";
     description.Value = this._description;
     cmd.Parameters.Add(description);

     MySqlParameter dueDate = new MySqlParameter();
     dueDate.ParameterName = "@ItemDueDate";
     dueDate.Value = this._dueDate;
     cmd.Parameters.Add(dueDate);

     cmd.Parameters.Add(new MySqlParameter("@IsDone", _isDone));

     cmd.ExecuteNonQuery();
     _id = (int) cmd.LastInsertedId;

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }

    public void MarkAsDone(bool isDone)
    {
      _isDone = isDone;

      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"UPDATE items SET isDone = @IsDone WHERE id = @searchId;";

      cmd.Parameters.Add(new MySqlParameter("@searchId", _id));
      cmd.Parameters.Add(new MySqlParameter("@IsDone", isDone));

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
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
     bool isDone = false;

     while (rdr.Read())
     {
       itemId = rdr.GetInt32(0);
       itemDescription = rdr.GetString(1);
       itemDueDate = rdr.GetDateTime(2);
       isDone = rdr.GetBoolean(3);
     }

     Item foundItem= new Item(itemDescription, itemDueDate, itemId);
     foundItem.SetIsDone(isDone);

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
        Item newItem = new Item(itemDescription, itemDueDate, itemId);
        bool isDone = rdr.GetBoolean(3);
        newItem.SetIsDone(isDone);
        filterByDueDate.Add(newItem);
      }
      conn.Close();
      if (conn != null)
      {
          conn.Dispose();
      }
      return filterByDueDate;
    }

    public void AddCategory(Category newCategory)
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"INSERT INTO categories_items (category_id, item_id) VALUES (@CategoryId, @ItemId);";

        MySqlParameter category_id = new MySqlParameter();
        category_id.ParameterName = "@CategoryId";
        category_id.Value = newCategory.GetId();
        cmd.Parameters.Add(category_id);

        MySqlParameter item_id = new MySqlParameter();
        item_id.ParameterName = "@ItemId";
        item_id.Value = _id;
        cmd.Parameters.Add(item_id);

        cmd.ExecuteNonQuery();
        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
    }

    public List<Category> GetCategories()
    {
        MySqlConnection conn = DB.Connection();
        conn.Open();
        var cmd = conn.CreateCommand() as MySqlCommand;
        cmd.CommandText = @"SELECT categories.* FROM items
                        JOIN categories_items ON (items.id = categories_items.item_id)
                        JOIN categories ON (categories_items.category_id = categories.id)
                        WHERE items.id = @ItemId;";

        MySqlParameter itemIdParameter = new MySqlParameter();
        itemIdParameter.ParameterName = "@ItemId";
        itemIdParameter.Value = _id;
        cmd.Parameters.Add(itemIdParameter);

        MySqlDataReader rdr = cmd.ExecuteReader() as MySqlDataReader;

        List<Category> categories = new List<Category> {};
        while(rdr.Read())
        {
            int categoryId = rdr.GetInt32(0);
            string categoryName = rdr.GetString(1);
            Category foundCategory = new Category(categoryName, categoryId);
            categories.Add(foundCategory);
        }
        rdr.Dispose();

        conn.Close();
        if (conn != null)
        {
            conn.Dispose();
        }
        return categories;
    }

    public void Delete()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();
      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"DELETE FROM items WHERE id = @ItemId; DELETE FROM categories_items WHERE item_id = @ItemId;";

      MySqlParameter itemIdParameter = new MySqlParameter();
      itemIdParameter.ParameterName = "@ItemId";
      itemIdParameter.Value = this.GetId();
      cmd.Parameters.Add(itemIdParameter);

      cmd.ExecuteNonQuery();
      if (conn != null)
      {
        conn.Close();
      }
    }
  }
}
