using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Reservation_System
{
    [Serializable]
    public class Reservation
    {
        int id;
        Customer customer;
        List<Seat> reservedSeats = new List<Seat>();

        ReservationGridDataRow row = null;

        public Reservation() { 
        }

        public Reservation(int id,Customer customer, List<Seat> seats)
        {
            this.Id = id;
            this.Customer = customer;
            this.ReservedSeats = seats;

            Row = new ReservationGridDataRow(id,customer.Name, GetReservedSeatsString());
        }

        public Customer Customer { get => customer; set => customer = value; }
        public List<Seat> ReservedSeats { get => reservedSeats; set => reservedSeats = value; }
        public int Id { get => id; set => id = value; }
        internal ReservationGridDataRow Row { get => row; set => row = value; }


        public string GetReservedSeatsString()
        {
            string seats = "";
            foreach (var item in reservedSeats)
            {
                seats += item.SeatName + ",";
            }

            seats = seats.TrimEnd(',', ' ');

            return seats;
        }
    }

    class ReservationGridDataRow
    {
        int reservationIndex;
        string customerName;
        string seats;

        public ReservationGridDataRow() { }

        public ReservationGridDataRow(int index,string customerName, string seats)
        {
            this.reservationIndex = index;
            this.CustomerName = customerName;
            this.Seats = seats;
        }

        public string CustomerName { get => customerName; set => customerName = value; }
        public string Seats { get => seats; set => seats = value; }
        public int ReservationIndex { get => reservationIndex; set => reservationIndex = value; }
    }

}
