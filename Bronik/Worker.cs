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
    public void CloseOrder(Int64 id)
    {
        var order=GetOrder(id);
        SQLiteCommand command = new SQLiteCommand();
        command.CommandText = "insert into order_history('order_id','full_name','quantity','from','to','phone','state') values (@order_id,@full_name,@quantity,@from,@to,@phone,@state)";
        command.Parameters.AddWithValue("@order_id", id);
        command.Parameters.AddWithValue("@full_name", order.fullName);
        command.Parameters.AddWithValue("@quantity", order.quantity);
        command.Parameters.AddWithValue("@from", order.from);
        command.Parameters.AddWithValue("@to", DateTime.Now);
        command.Parameters.AddWithValue("@phone", order.phone);
        command.Parameters.AddWithValue("@state", "false");
        Execute(command);
    }

    public void OpenDesk(int desk_id,string fullName,int quantity,string phone,DateTime from)
    {
        SQLiteCommand command = new SQLiteCommand("insert into 'order'('desk_id') values(@desk_id)");
        command.Parameters.AddWithValue("@desk_id", desk_id);
        Execute(command);
        command.CommandText = "select max(id),id from 'order' where desk_id=@desk_id";
        DataTable dt = Execute(command);
        command.CommandText = "insert into order_history('order_id','full_name','quantity','from','phone','state') values (@order_id,@full_name,@quantity,@from,@phone,@state)";
        command.Parameters.AddWithValue("@order_id", (Int64)dt.Rows[0][0]);
        command.Parameters.AddWithValue("@full_name", fullName);
        command.Parameters.AddWithValue("@quantity", quantity);
        command.Parameters.AddWithValue("@from", from);
        command.Parameters.AddWithValue("@phone", phone);
        command.Parameters.AddWithValue("@state", "true");
        Execute(command);
        Console.WriteLine($"Стол {desk_id} забронирован {fullName}");
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
            Console.WriteLine(command.CommandText);
            foreach (SQLiteParameter param in command.Parameters)
            {
                Console.WriteLine(param.Value);
            }
        }
        finally
        {
            connection.Close();
        }
        return res;
    }
}
