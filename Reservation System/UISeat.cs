using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml.Serialization;

namespace Reservation_System
{
    [Serializable]
    public class UISeat
    {
        [XmlAttribute]
        public StackPanel stack;
        [XmlAttribute]
        public CheckBox checkBox;
        [XmlAttribute]
        public Image seatIcon;
        [XmlAttribute]
        public Label seatLabel;
        [XmlAttribute]
        public Label customerLabel;

        [XmlAttribute]
        private Seat seat;
        
        public UISeat() { }
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
