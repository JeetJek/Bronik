namespace Bronik
{
    public class Desk
    {
        public Int64 number;
        public List<Order> orders = new List<Order>();
        
        public Desk(Int64 number)
        {
            this.number = number;
        }
        public Desk(Int64 number, List<Order> orders) : this(number)
        {
            this.orders = orders;
        }
    }
}
