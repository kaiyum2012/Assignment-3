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
        SeatState state;

        public Seat(int seatNo,SeatState state = SeatState.AVAILABLE)
        {
            SeatNo = seatNo;
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
        public string SeatName { get => seatName; set => seatName = value; }
        public SeatState State {
        
            get { return state;}
            
            set { 
                state = value;
            }
        }
    }
}
