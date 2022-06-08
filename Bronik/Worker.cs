﻿using System.Data.SQLite;
using System.Data;
namespace Bronik;

public class Worker
{
    private SQLiteConnection connection;
    public Worker()
    {
        connection = new SQLiteConnection("Data Source=booking.db");
    }
    public List<Table> GetTables()
    {
        List < Table > tables= new List<Table>();
        DataTable tmp=Execute("select * from tables");
        foreach (DataRow row in tmp.Rows)
        {
            int id = Convert.ToInt32(row["num"].ToString());
            tables.Add(new Table(id,Convert.ToBoolean((Int64)row["state"])));
        }
        return tables;
    }
    
    public void SetupTables(int tables)
    {
        var command=connection.CreateCommand();
        connection.Open();
        try
        {
            command.CommandText = "delete from tables;";
            command.ExecuteNonQuery();
            for (int i = 0; i < tables; i++)
            {
                command.CommandText = $"insert into tables values({i},{(bool)false})";
                command.ExecuteNonQuery();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при работе с БД: " + ex.ToString());
        }
        finally
        {
            connection.Close(); 
        }
    }
    public void SetTable(int id,string name)
    {
        Console.WriteLine($"Стол {id} забронирован {name}");
    }
    public void SetTable(int id, string name,DateTime date)
    {
        Console.WriteLine($"Стол {id} забронирован {name} на {date.ToString("MM/dd/yyyy HH:mm")}");
        Execute("update tables set state="+1+" where num="+id+";");
    }
    public DataTable Execute(string command)
    {
        DataTable res = new DataTable();
        var cmd = connection.CreateCommand();
        connection.Open();
        try
        {
            cmd.CommandText = command;
            res.Load(cmd.ExecuteReader());
        }
        catch (Exception ex)
        {
            Console.WriteLine("Ошибка при работе с БД: " + ex.ToString());
        }
        finally
        {
            connection.Close();
        }
        return res;
    }
}