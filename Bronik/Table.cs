namespace Bronik
{
    public class Table
    {
        public int id;
        public bool booked;
        DateTime from;
        string name="";
        string phone="";
        public Table(int id)
        {
            this.booked= false;
            this.id = id;
        }
        public Table(int id, bool booked) : this(id)
        {
            this.booked = booked;
        }
        public Table(int id, bool booked, DateTime from, string name, string phone) : this(id)
        {
            this.booked = booked;
            this.from = from;
            this.name = name;
            this.phone = phone;
        }
    }
}
