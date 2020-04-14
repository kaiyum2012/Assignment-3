namespace Reservation_System
{
    public class Customer
    {
        string name;

        public Customer() { }

        public Customer(string name)
        {
            this.Name = name;
        }

        public string Name { get => name; set => name = value; }
    }
}