using Microsoft.Win32;
using Reservation_System.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;
using System.Xml.Serialization;

namespace Reservation_System
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        const int NO_OF_SEATS = 16;
        const int NO_ROW = 4;
        const int NO_COLUMN = 4;
        const string AVAILABLE_SEAT_PATH = "empty.png";
        const string SEAT_SELECTED_PATH = "not-available.png";
        const string OCCUPIED_SEAT_PATH = "occupied.png";
        private const int minLength = 0;
        private const int maxLength = 20;
        int currentReservationId = 0;

        List<Seat> seats = new List<Seat>();

        List<string> customerNames = new List<string>();

        List<CheckBox> checkedSeats = new List<CheckBox>();

        //UISeatList uISeatList = new UISeatList();
        List<UISeat> uISeats = new List<UISeat>();

        List<Reservation> reservations = new List<Reservation>();

        public List<Seat> Seats { get => seats; set => seats = value; }

        private IEnumerable CustomerNames => reservations.Select(i => i.Customer);

        public MainWindow()
        {
            InitializeComponent();

            CreateSeats();

            SetupCustomerComboBox();

            ReservationSerializer util = new ReservationSerializer();
            
        }

        private List<ReservationGridDataRow> GetReservationDetails()
        {
            List<ReservationGridDataRow> data = new List<ReservationGridDataRow>();
            foreach (var item in reservations)
            {
                data.Add(item.Row);
                //data.Add(new ReservationGridDataRow(item.Customer.Name, item.GetReservedSeatsString()));
            }

            return data;
        }

        private void SetupCustomerComboBox()
        {
            customerCombo.ItemsSource = CustomerNames;
            this.customerCombo.SelectionChanged += CustomerCombo_SelectionChanged;
        }

        private void SetSeatCustomerLabel(int seatNo, string customerName)
        {
            uISeats[seatNo].customerLabel.Content = customerName;
            //uISeatList[seatNo].customerLabel.Content = customerName;
        }
 
        private void CreateSeats()
        {
            for (int i = 0; i < NO_OF_SEATS; i++)
            {
                Seats.Add(new Seat(i));
            }

            SetupSeatGrid();
        }

        private void SetupSeatGrid()
        {
            for (int row = 0; row < NO_ROW; row++)
            {
                seatGrid.RowDefinitions.Add(new RowDefinition());
            }

            for (int col = 0; col < NO_COLUMN; col++)
            {
                seatGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            int seat = 0;
            for (int i = 0; i < NO_OF_SEATS / NO_ROW; i++)
            {
                for (int j = 0; j < NO_OF_SEATS/NO_COLUMN; j++)
                {
                    StackPanel stack = new StackPanel
                    {
                        Name = "stack_" + seats[seat].SeatName,
                        Orientation = Orientation.Vertical,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };

                    //TODO:: set grid position 
                    Grid.SetRow(stack, i);
                    Grid.SetColumn(stack, j);
                    
                    //TODO:: Create checkbox
                    CheckBox checkBox = new CheckBox
                    {
                        Name = seats[seat].SeatName,
                        HorizontalAlignment = HorizontalAlignment.Left,
                    };

                    checkBox.Checked += Seat_Checked;
                    checkBox.Unchecked += Seat_Unchecked;

                    //seatCheckBoxes.Add(checkBox);

                    //TODO:: Create seat image
                    Image image = new Image
                    {
                        Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative)),
                        Height = 40,
                        //Width = 40,
                        Stretch = Stretch.Uniform,
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };

                    //seatIcons.Add(image);

                    checkBox.Content = image;

                    //TODO:: Create Label

                    Console.WriteLine(seats[seat].SeatName);
                    Label label = new Label
                    {
                        Content = seats[seat].SeatName,
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    //seatLabels.Add(label);

                    Label customerLabel = new Label
                    {
                        Content = "",
                        HorizontalAlignment = HorizontalAlignment.Center,
                    };
                    
                    //TODO:: Create Label
                    stack.Children.Add(checkBox);
                    stack.Children.Add(label);
                    stack.Children.Add(customerLabel);

                    var uiSeat = new UISeat(stack, checkBox, image, label, customerLabel);
                    uiSeat.SetSeat(Seats[seat]);

                    uISeats.Add(uiSeat);
                    //uISeatList.Add(uiSeat);


                    seatGrid.Children.Add(stack);

                    seat++;
                }
            }
        }
    
        //private void CheckboxSeatImageSetAt(int selectedSeat)
        //{
        //    uISeats[selectedSeat].checkBox.IsChecked = !uISeats[selectedSeat].checkBox.IsChecked;

        //    if (uISeats[selectedSeat].checkBox.IsChecked == true)
        //    {
        //        seats[selectedSeat].State = SeatState.SELECTED;
        //        uISeats[selectedSeat].seatIcon.Source = new BitmapImage(new Uri(@SEAT_SELECTED_PATH, UriKind.Relative));
        //    }
        //    else
        //    {
        //        seats[selectedSeat].State = SeatState.AVAILABLE;
        //        uISeats[selectedSeat].seatIcon.Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative));
        //    }
        //}

        private bool CheckSeatBookSelection()
        {
            if(/*seatCombo.SelectedIndex != -1 ||*/ checkedSeats.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please select seat(s)","Warning");
                Console.WriteLine("Please select seat(s)");
                return false;
            } 
        }

        private bool CheckCustomerName()
        {
            if(string.IsNullOrWhiteSpace(customerName.Text))
            {
                MessageBox.Show("Please provide customer name","Warning");
                Console.WriteLine("Please provide customer name");
                return false;
            }
            else if(customerName.Text.Length > maxLength)
            {
                MessageBox.Show($"Customer name is longer than {maxLength} character");
                Console.WriteLine($"Customer name is longer than {maxLength} character");
                return false;
            }
            return true;
        }

        private void MarkSeat(int seatId, SeatState state)
        {
            switch (state)
            {
                case SeatState.AVAILABLE:
                    seats[seatId].State = SeatState.AVAILABLE;
                    uISeats[seatId].seatIcon.Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative));
                    //uISeatList[seatId].seatIcon.Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative));
                    uISeats[seatId].checkBox.IsEnabled = true;
                    //uISeatList[seatId].checkBox.IsEnabled = true;
                    break;
                case SeatState.RESERVED:
                    seats[seatId].State = SeatState.RESERVED;
                    uISeats[seatId].seatIcon.Source = new BitmapImage(new Uri(@OCCUPIED_SEAT_PATH, UriKind.Relative));
                    //uISeatList[seatId].seatIcon.Source = new BitmapImage(new Uri(@OCCUPIED_SEAT_PATH, UriKind.Relative));
                    uISeats[seatId].checkBox.IsEnabled = false;
                    //uISeatList[seatId].checkBox.IsEnabled = false;
                    break;
                case SeatState.NOT_AVAILABLE:
                    break;
                case SeatState.SELECTED:
                    seats[seatId].State = SeatState.SELECTED;
                    uISeats[seatId].seatIcon.Source = new BitmapImage(new Uri(@SEAT_SELECTED_PATH, UriKind.Relative));
                    //uISeatList[seatId].seatIcon.Source = new BitmapImage(new Uri(@SEAT_SELECTED_PATH, UriKind.Relative));
                    break;
                default:
                    break;
            }

        }

        private void AddSeatToCheckedSeats(int seatId)
        {
            checkedSeats.Add(uISeats[seatId].checkBox);
            //checkedSeats.Add(uISeatList[seatId].checkBox);
        }

        private void RemoveFromCheckedSeats(int seatId)
        {
            uISeats[seatId].checkBox.IsChecked = false;
            //uISeatList[seatId].checkBox.IsChecked = false;
            checkedSeats.Remove(uISeats[seatId].checkBox);
            //checkedSeats.Remove(uISeatList[seatId].checkBox);
        }

        private bool IsSeatAvailable()
        {
            foreach (var seat in uISeats)
            //foreach (var seat in uISeatList)
            {
                if (seat.GetSeat().State == SeatState.AVAILABLE || seat.GetSeat().State == SeatState.SELECTED)
                {
                    return true;
                }
            }
            MessageBox.Show("No Seat(s) available","Warning");
            return false;
        }

        private void ReloadReservationDataGrid()
        {
            reservationsDataGrid.ItemsSource = GetReservationDetails();
        }

        private void ReloadCustomerComboBox()
        {
            customerCombo.ItemsSource = CustomerNames;
        }

        private void ClearCustomerName()
        {
            customerName.Text = "";
        }

        private void ExportCurrentReservationToXml()
        {
            var filename = "_tempSeatingLayout.xml";
            ExportToxml(filename, reservations);
        }

        #region Event handling 

        private void CustomerCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedCustomer = customerCombo.SelectedIndex;
            reservationsDataGrid.SelectedIndex = selectedCustomer;
        }

        private void Seat_Unchecked(object sender, RoutedEventArgs e)
        { 
            var seat = sender as CheckBox;
            try
            {
                int selectedSeat = int.Parse(seat.Name.Split('_')[1]) - 1; // Seat_1 => 1 
                MarkSeat(selectedSeat, SeatState.AVAILABLE);
                RemoveFromCheckedSeats(selectedSeat);
            }
            catch (Exception)
            {
                Console.WriteLine("Seat Checked Not working");
            }
        }

        private void Seat_Checked(object sender, RoutedEventArgs e)
        {
            var seat = sender as CheckBox;
            try
            {
                int selectedSeat = int.Parse(seat.Name.Split('_')[1]) - 1; // Seat_1 => 1 
                MarkSeat(selectedSeat, SeatState.SELECTED);
                AddSeatToCheckedSeats(selectedSeat);
            }
            catch (Exception)
            {
                Console.WriteLine("Seat UnChecked Not working");
            }
        }

        private void Rest_Click(object sender, RoutedEventArgs e)
        {

            foreach (var seat in uISeats)
            //foreach (var seat in uISeatList)
            {
                seat.checkBox.IsChecked = false;
                MarkSeat(seat.GetSeat().SeatNo, SeatState.AVAILABLE);
                SetSeatCustomerLabel(seat.GetSeat().SeatNo, "");
            }

            foreach (var reservation in reservations)
            {
                HighlightSeatsFor(reservation.ReservedSeats, false);
            }

            reservations.Clear();
            ReloadReservationDataGrid();
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (!IsSeatAvailable()) return;

            if (CheckSeatBookSelection() && CheckCustomerName())
            {
                reservations.Add(new Reservation(currentReservationId++, new Customer(customerName.Text), GetSelectedSeats()));

                checkedSeats.Clear();

                MarkSeatsReserved();
                
                ReloadReservationDataGrid();

                ReloadCustomerComboBox();

                ClearCustomerName();
            }

            List<Seat> GetSelectedSeats()
            {
                return seats.Where(i => i.State == SeatState.SELECTED).ToList();
            }

            void MarkSeatsReserved()
            {
                var processingSeats = GetSelectedSeats();
                foreach (var seat in  processingSeats)
                {
                    //seat.State = SeatState.RESERVED;
                    //uISeats[seat.SeatNo].seatIcon.Source = new BitmapImage(new Uri(@OCCUPIED_SEAT_PATH, UriKind.Relative));
                    MarkSeat(seat.SeatNo, SeatState.RESERVED);
                    
                    //uISeats[seat.SeatNo].checkBox.IsEnabled = false;
                    
                    //uISeats[seat.SeatNo].customerLabel.Content = customerName.Text;
                    SetSeatCustomerLabel(seat.SeatNo, customerName.Text);
                }
            }
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            var rows = reservationsDataGrid.SelectedItems;

            if(rows == null || rows.Count == 0)
            {
                MessageBox.Show("Please Select reservation","Warning");
                return;
            }
            
            foreach (ReservationGridDataRow row in rows)
            {
                var seats = reservations.Where(i => i.Id == row.ReservationIndex).Select(i => i.ReservedSeats).FirstOrDefault();

                foreach (var seat in seats)
                {

                    MarkSeat(seat.SeatNo, SeatState.AVAILABLE);
                    RemoveFromCheckedSeats(seat.SeatNo);
                    SetSeatCustomerLabel(seat.SeatNo, "");
                    //seat.State = SeatState.AVAILABLE;
                }

                var r = reservations.Where(i => i.Id == row.ReservationIndex).ToArray();
                if(r != null)
                {
                    reservations.Remove(r[0]);
                }
            }

            ReloadReservationDataGrid();

            ReloadCustomerComboBox();
        }

        private void ReservationsDataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if(e.Column.Header.ToString() == "ReservationIndex")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
        }

        private void HighlightSeatsFor(List<Seat> seats, bool isVisible)
        {
            var color = isVisible ? new SolidColorBrush(Colors.AliceBlue) : new SolidColorBrush(Colors.White);

            foreach (var seat in seats)
            {
                uISeats[seat.SeatNo].stack.Background = color;
                //uISeatList[seat.SeatNo].stack.Background = color;
            }
        }

        private void DataGridRow_GotFocus(object sender, RoutedEventArgs e)
        {
            //var rows = reservationsDataGrid.SelectedItems;

            //foreach (ReservationGridDataRow row in rows)
            //{
            //    var seats = reservations.Where(i => i.Id == row.ReservationIndex).Select(i => i.ReservedSeats).FirstOrDefault();
            //    HighlightSeatsFor(seats, true);
            //}
        }

        private void DataGridRow_LostFocus(object sender, RoutedEventArgs e)
        {
            //var rows = reservationsDataGrid.SelectedItems;

            //foreach (ReservationGridDataRow row in rows)
            //{
            //    var seats = reservations.Where(i => i.Id == row.ReservationIndex).Select(i => i.ReservedSeats).FirstOrDefault();

            //    HighlightSeatsFor(seats, false);
            //}
        }

        private void CustomerName_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!System.Text.RegularExpressions.Regex.IsMatch(e.Text, "^[a-zA-Z]"))
            {
                e.Handled = true;
            }
        }
       
        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            customerName.Text = "";
        }

        #endregion

        private void CustomerZToA_Click(object sender, RoutedEventArgs e)
        {

            if (reservations.Count == 0)
            {
                MessageBox.Show("No Reservations Available");
                return;
            }

            reservations =  reservations.OrderByDescending(i => i.Customer.Name).ToList();

            MessageBox.Show("Reservations record sorted by Customer Name (Z to A)","Customer (Z->A)");

            ReloadReservationDataGrid();
        }

        private void SeatShortToLongest(object sender, RoutedEventArgs e)
        {
           //reservations = reservations.OrderBy(i => i.ReservedSeats.Count).ToList();

            if(reservations.Count == 0)
            {
                MessageBox.Show("No Reservations Available");
                return;
            }

            reservations = reservations.OrderBy(i => i.Customer.Name.Length).ToList();
            
            MessageBox.Show("Reservations sorted by customer name shortest to longest)","Customer Nmae Short->Long");

            ReloadReservationDataGrid();
        }

        private void UnreserveSeatAToZ_Click(object sender, RoutedEventArgs e)
        {
            var unreservedSeats = uISeats.Where(i => i.GetSeat().State == SeatState.AVAILABLE).ToList();
            //var unreservedSeats = uISeatList.Where(i => i.GetSeat().State == SeatState.AVAILABLE).ToList();
            var result = "";
            var count = 0;
            foreach (UISeat unreserveSeat in unreservedSeats)
            {
                result += unreserveSeat.GetSeat().SeatName.ToString() + "\n";
                count++;
            }
            
            // remove trailing comma.
            //result = result.TrimEnd(',',' ');

            if(result.Length == 0)
            {
                result = "NO unreserve seats";
            }
            result +=  $"Total Count: {count}";
            MessageBox.Show($"{result}","Unreserved Seats");

            ReloadReservationDataGrid();
        }

        private void OpenMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (reservations.Count > 0)
                {
                    MessageBoxResult result = MessageBox.Show("Would you like to save current reservations before loading backup?", "", MessageBoxButton.YesNo);

                    if (result == MessageBoxResult.Yes)
                    {
                        ExportCurrentReservationToXml();
                    }
                }

                OpenFileDialog openFileDialog = new OpenFileDialog
                {
                    Multiselect = false,
                    Filter = "XML Files (*.xml)|*.xml"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    ImportFromXml(openFileDialog.FileName);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("OpenMenuItem_Click -> Error: {0}", ex.ToString());
            }
           
        }

        private void SaveMenuItem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if(reservations.Count == 0)
                {
                    MessageBox.Show("No reservations to Export, hence Exiting !");
                    return;
                }

                var filename = "SeatLayout_"+ DateTime.Today.Date.ToString("dd-MM-yyyy")  +".xml";
                ExportToxml(filename,reservations);
                MessageBox.Show("Succesfully Exported to With name: "+filename, "Export Seat Layout - XML");
            }
            catch (Exception ex)
            {
                Console.WriteLine("SaveMenuItem_Click -> Error: {0}", ex.ToString());
            }
        }

        private void ExportToxml(string filename, Object data)
        {
            try
            {
                var fileStream = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite, FileShare.None);
                XmlSerializer serializer = new XmlSerializer(typeof(List<Reservation>));

                serializer.Serialize(fileStream, data);
                fileStream.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(" Error occured while exporting: {0}", ex.ToString());
            }
         
        }
        private void ImportFromXml(string filename)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(List<Reservation>));

            XmlTextReader reader = new XmlTextReader(filename);
            var importedReservations = serializer.Deserialize(reader) as List<Reservation>;

            reservations.Clear();
            reader.Close();

            foreach (var item in importedReservations)
            {
                item.Row = new ReservationGridDataRow(item.Id, item.Customer.Name, item.GetReservedSeatsString());
                reservations.Add(item);

                foreach (var seat in item.ReservedSeats)
                {
                    MarkSeat(seat.SeatNo, seat.State);
                    AddSeatToCheckedSeats(seat.SeatNo);
                    SetSeatCustomerLabel(seat.SeatNo, item.Customer.Name);
                }
            }

            ReloadCustomerComboBox();

            reservationsDataGrid.ItemsSource = null;
            ReloadReservationDataGrid();
        }

    }
}

  