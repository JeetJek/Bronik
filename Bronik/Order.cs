namespace Bronik
{
    public class Order
    {
        public Int64 id;
        public string fullName="";
        public Int64 quantity;
        public DateTime from;
        public DateTime to;
        public string phone="";
        public bool state;
        public Order() { }
        public Order(Int64 id,string fullName, Int64 quantity,DateTime from,string phone,bool state)
        {
            this.id = id;  
            this.fullName = fullName;  
            this.quantity = quantity;
            this.from = from;
            this.phone = phone;
            this.state = state;
        }
        public Order(Int64 id, string fullName, Int64 quantity, DateTime from, DateTime to, string phone, bool state) : this(id, fullName, quantity, from, phone, state)
        {
            this.to = to;
        }

    }
}
