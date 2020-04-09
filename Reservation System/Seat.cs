using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Reservation_System
{
    public enum SeatState
    {
        AVAILABLE = 0,
        RESERVED,
        NOT_AVAILABLE,
        SELECTED
    }

    public class Seat
    {
        int seatNo;
        string seatName;
        //string customerName;
        SeatState state;
        string bg;

        public Seat(int seatNo,SeatState state = SeatState.AVAILABLE)
        {
            SeatNo = seatNo;
            //CustomerName = customerName;
            State = state;
        }

        public int SeatNo
        {
            get
            {
                return seatNo;
            }

            set
            {
                seatNo = value;
                SeatName = "Seat_" + (seatNo + 1);
            }
        }
        //public string CustomerName { get => customerName; set => customerName = value; }
        public string SeatName { get => seatName; set => seatName = value; }
        public SeatState State {
        
            get { return state;}
            
            set { 
                state = value;

                //switch (state)
                //{
                //    case SeatState.AVAILABLE:
                //        bg = "#ffffff";
                //        break;
                //    case SeatState.RESERVED:
                //        bg = "red";
                //        break;
                //    case SeatState.NOT_AVAILABLE:
                //        bg = "black";
                //        break;
                //    case SeatState.SELECTED:
                //        bg = "blue";
                //        break;
                //}
                //Console.WriteLine(bg);

            }
        }
    }
}
