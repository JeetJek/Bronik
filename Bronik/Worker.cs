using System.Data.SQLite;
using System.Data;
namespace Bronik;

public class Worker
{
    private SQLiteConnection connection;
    //sqlite отдаёт integer в виде int64
    private int ToInt(Int64 a)
    {
        return Convert.ToInt32(a);
    }
    public Worker()
    {
        connection = new SQLiteConnection("Data Source=booking.db");
    }
    public List<Desk> GetTables()
    {
        List <Desk> tables= new List<Desk>();
        DataTable desks= new DataTable();
        desks = Execute("select * from desk");
        foreach (DataRow row in desks.Rows)
        {
            tables.Add(
                new Desk((Int64)row["number"],
                GetOrders((Int64)row["id"]))
                );
        }
        return tables;
    }
    public List<Order> GetOrders(Int64 desk)
    {
        List<Order> orders = new List<Order>();
        SQLiteCommand cmd = new SQLiteCommand("select id from 'order' where desk_id=@desk");
        cmd.CommandType = CommandType.Text;
        cmd.Parameters.AddWithValue("@desk", desk);
        DataTable dt = Execute(cmd);
        foreach (DataRow row in dt.Rows)
        {
            orders.Add(GetOrder(ToInt((Int64)row["id"])));
        }
        return orders;
    }
    public Order GetOrder(Int64 id)
    {
        SQLiteCommand command = new SQLiteCommand("select   max(oh.id),  oh.full_name,  oh.quantity,  oh.'from',  oh.phone,  oh.state  from 'order' o join order_history oh on o.id=oh.order_id where o.id=@id");
        command.CommandType = CommandType.Text;
        command.Parameters.AddWithValue("@id", id);
        DataTable dt = Execute(command);
        Order order=new Order();
        if (dt.Rows.Count > 0)
        {
            order = new Order(id, (string)dt.Rows[0]["full_name"], (Int64)dt.Rows[0]["quantity"],DateTime.Parse((string)dt.Rows[0]["from"]), (string)dt.Rows[0]["phone"], bool.Parse((string)dt.Rows[0]["state"]));
        }
        return order;
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
