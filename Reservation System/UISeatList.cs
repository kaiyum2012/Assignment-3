using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Reservation_System
{
    [XmlRoot("SeatLayout")]
    class UISeatList : IList<UISeat>
    {
        List<UISeat> uISeats = new List<UISeat>();

        public UISeat this[int index] { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public List<UISeat> UISeats { get => uISeats; set => uISeats = value; }

        public int Count => throw new NotImplementedException();

        public bool IsReadOnly => throw new NotImplementedException();

        public void Add(UISeat uISeat)
        {
            UISeats.Add(uISeat);
        }

        public void Clear()
        {
            UISeats.Clear();
        }

        public bool Contains(UISeat item)
        {
            return UISeats.Contains(item);
        }

        public void CopyTo(UISeat[] array, int arrayIndex)
        {
            UISeats.CopyTo(array, arrayIndex);
        }

        public IEnumerator<UISeat> GetEnumerator()
        {
            return UISeats.GetEnumerator();
        }

        public int IndexOf(UISeat item)
        {
            return UISeats.IndexOf(item);
        }

        public void Insert(int index, UISeat item)
        {
            UISeats.Insert(index, item);
        }
    
        public bool Remove(UISeat item)
        {
            return UISeats.Remove(item);
        }

        public void RemoveAt(int index)
        {
            UISeats.RemoveAt(index);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return UISeats.GetEnumerator();
                 
        }
    }
}
