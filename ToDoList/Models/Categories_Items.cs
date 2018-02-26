using System.Collections.Generic;
using System;
using ToDoList;
using MySql.Data.MySqlClient;
using System.Globalization;

namespace ToDoList.Models
{
  public class categories_items
  {
    private int _id;
    private int _category_id;
    private int _item_id;

    public categories_items(int id, int cat, int item)
    {
      _id = id;
      _category_id = cat;
      _item_id = item;
    }

    public static void DeleteAll()
    {
      MySqlConnection conn = DB.Connection();
      conn.Open();

      var cmd = conn.CreateCommand() as MySqlCommand;
      cmd.CommandText = @"TRUNCATE TABLE categories_items;";

      cmd.ExecuteNonQuery();

      conn.Close();
      if (conn != null)
      {
        conn.Dispose();
      }
    }
  }
}
