using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Reservation_System
{
    public class UISeat
    {
        public StackPanel stack;
        public CheckBox checkBox;
        public Image seatIcon;
        public Label seatLabel;
        public Label customerLabel;
        
        private Seat seat;
        
        public UISeat(StackPanel stack, CheckBox checkBox,Image image, Label seatLabel, Label customerLabel)
        {
            this.stack = stack;
            this.checkBox = checkBox;
            this.seatIcon = image;
            this.seatLabel = seatLabel;
            this.customerLabel = customerLabel;

           // SetHierarchy();
        }


        public Seat GetSeat()
        {
            return this.seat;
        }

        public void SetSeat(Seat seat)
        {
            this.seat = seat;
        }

        private void SetHierarchy()
        {
            stack.Children.Add(checkBox);
            stack.Children.Add(seatLabel);
            stack.Children.Add(customerLabel);
        }
    }
}
