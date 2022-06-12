using System.Data.SQLite;
using System.Data;
namespace Bronik;

public class Worker
{
    private SQLiteConnection connection;
    public Worker()
    {
        connection = new SQLiteConnection("Data Source=booking.db");
    }
    public List<Desk> GetTables()
    {
        List < Desk > tables= new List<Desk>();
        DataTable desks= new DataTable();
        desks = Execute("select * from desk");
        foreach (DataRow row in desks.Rows)
        {
            
        }
        return tables;
    }
    public void CloseDesk(int table)
    {
        Execute("update tables set state="+0+" where num="+table+";");

        Execute("");
    }
    
    public void OpenDesk(int table_num,string name,int clients,string tel,DateTime date)
    {
        Execute("update tables set state="+1+" where num="+table_num+";");
        SQLiteCommand cmd = new SQLiteCommand();
        cmd.CommandText = "insert into booking('table','fio','num','date','tel','action') values (@table,@fio,@num,@date,@tel,@action);";
        cmd.Parameters.AddWithValue("@table", table_num);
        cmd.Parameters.AddWithValue("@fio", name);
        cmd.Parameters.AddWithValue("@num", clients);
        cmd.Parameters.AddWithValue("@date", date);
        cmd.Parameters.AddWithValue("@tel", tel);
        cmd.Parameters.AddWithValue("@action", 1);
        Console.WriteLine(cmd.CommandText);
        Execute(cmd);
        Console.WriteLine($"Стол {table_num} забронирован {name}");
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
    public DataTable Execute(SQLiteCommand command)
    {
        DataTable res = new DataTable();
        command.Connection = connection;
        connection.Open();
        try
        {
            res.Load(command.ExecuteReader());
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
