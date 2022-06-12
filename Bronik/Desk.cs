namespace Bronik
{
    public class Desk
    {
        public int id;
        public bool booked;
        
        public Desk(int id)
        {
            this.booked= false;
            this.id = id;
        }
        public Desk(int id, bool booked) : this(id)
        {
            this.booked = booked;
        }
        public Desk(int id, bool booked, DateTime from, string name, string phone) : this(id)
        {
            this.booked = booked;
            this.from = from;
            this.name = name;
            this.phone = phone;
        }
    }
}
