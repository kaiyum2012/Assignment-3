using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation_System
{
    class Reservation
    {
        Customer customer;
        List<Seat> reservedSeats = new List<Seat>();

        public Reservation(Customer customer, List<Seat> reservedSeats)
        {
            this.Customer = customer;
            this.ReservedSeats = reservedSeats;
        }

        public Customer Customer { get => customer; set => customer = value; }
        internal List<Seat> ReservedSeats { get => reservedSeats; set => reservedSeats = value; }
    }
}
