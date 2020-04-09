using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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

        List<Seat> seats = new List<Seat>();

        //List<CheckBox> seatCheckBoxes = new List<CheckBox>();
        //List<Image> seatIcons = new List<Image>();
        //List<Label> seatLabels = new List<Label>();

        List<CheckBox> checkedSeats = new List<CheckBox>();

        List<UISeat> uISeats = new List<UISeat>();

        List<Reservation> reservations = new List<Reservation>();

        public List<Seat> Seats { get => seats; set => seats = value; }

        public MainWindow()
        {
            InitializeComponent();

            CreateSeats();

            SetUpSeatComboBox();
        }

        private void SetUpSeatComboBox()
        {
            seatCombo.ItemsSource = seats;
            this.seatCombo.SelectionChanged += SeatCombo_SelectionChanged;
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
                        HorizontalAlignment = HorizontalAlignment.Center
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
                        HorizontalAlignment = HorizontalAlignment.Center
                    };

                    //TODO:: Create Label
                    stack.Children.Add(checkBox);
                    stack.Children.Add(label);
                    stack.Children.Add(customerLabel);

                    var uiSeat = new UISeat(stack, checkBox, image, label, customerLabel);
                    uiSeat.SetSeat(Seats[seat]);

                    uISeats.Add(uiSeat);
                    
                    seatGrid.Children.Add(stack);

                    seat++;
                }
            }
        }
    
        private void CheckboxSeatImageSetAt(int selectedSeat)
        {
            uISeats[selectedSeat].checkBox.IsChecked = !uISeats[selectedSeat].checkBox.IsChecked;

            if (uISeats[selectedSeat].checkBox.IsChecked == true)
            {
                seats[selectedSeat].State = SeatState.SELECTED;
                uISeats[selectedSeat].seatIcon.Source = new BitmapImage(new Uri(@SEAT_SELECTED_PATH, UriKind.Relative));
            }
            else
            {
                seats[selectedSeat].State = SeatState.AVAILABLE;
                uISeats[selectedSeat].seatIcon.Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative));
            }
        }

        private bool CheckSeatBookSelection()
        {
            if(seatCombo.SelectedIndex != -1 || checkedSeats.Count > 0)
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please select seat(s)");
                Console.WriteLine("Please select seat(s)");
                return false;
            } 
        }

        private bool CheckCustomerName()
        {
            if(customerName.Text != "")
            {
                return true;
            }
            else
            {
                MessageBox.Show("Please provide customer name");
                Console.WriteLine("Please provide customer name");
                return false;

            }
        }

        #region Event handling 

        private void SeatCombo_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int selectedSeat = seatCombo.SelectedIndex;

            CheckboxSeatImageSetAt(selectedSeat);
            //seatCheckBoxes[selectedSeat].IsChecked = !seatCheckBoxes[selectedSeat].IsChecked;

            //if (seatCheckBoxes[selectedSeat].IsChecked == true)
            //{
            //    seats[selectedSeat].State = SeatState.SELECTED;
            //    seatIcons[selectedSeat].Source = new BitmapImage(new Uri(@SEAT_SELECTED_PATH, UriKind.Relative));
            //}
            //else
            //{
            //    seats[selectedSeat].State = SeatState.AVAILABLE;
            //    seatIcons[selectedSeat].Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative));
            //}

        }

        private void Seat_Unchecked(object sender, RoutedEventArgs e)
        {
            var seat = sender as CheckBox;
            checkedSeats.Remove(seat);

            try
            {
                int selectedSeat = int.Parse(seat.Name.Split('_')[1]) - 1; // Seat_1 => 1 
                seats[selectedSeat].State = SeatState.AVAILABLE;
                
                uISeats[selectedSeat].seatIcon.Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative));

                //seatIcons[selectedSeat].Source = new BitmapImage(new Uri(@AVAILABLE_SEAT_PATH, UriKind.Relative));
            }
            catch (Exception)
            {
                Console.WriteLine("Seat Checked Not working");
            }
        }

        private void Seat_Checked(object sender, RoutedEventArgs e)
        {
            var seat = sender as CheckBox;

            checkedSeats.Add(seat);

            try
            {
                int selectedSeat = int.Parse(seat.Name.Split('_')[1]) - 1; // Seat_1 => 1 

                seats[selectedSeat].State = SeatState.SELECTED;
                uISeats[selectedSeat].seatIcon.Source = new BitmapImage(new Uri(@SEAT_SELECTED_PATH, UriKind.Relative));
                //seatIcons[selectedSeat].Source = new BitmapImage(new Uri(@SEAT_SELECTED_PATH, UriKind.Relative));
            }
            catch (Exception)
            {
                Console.WriteLine("Seat UnChecked Not working");
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Seat 1 selected");
        }

        private void Rest_Click(object sender, RoutedEventArgs e)
        {
            foreach (var seat in uISeats)
            {
                seat.checkBox.IsChecked = false;
            }
        }

        private void Book_Click(object sender, RoutedEventArgs e)
        {
            if (CheckSeatBookSelection() && CheckCustomerName())
            {
                reservations.Add(new Reservation(new Customer(customerName.Text), GetSelectedSeats()));

                checkedSeats.Clear();

                MarkSeatsReserved();

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
                    seat.State = SeatState.RESERVED;
                    uISeats[seat.SeatNo].seatIcon.Source = new BitmapImage(new Uri(@OCCUPIED_SEAT_PATH, UriKind.Relative));
                    uISeats[seat.SeatNo].checkBox.IsEnabled = false;
                    uISeats[seat.SeatNo].customerLabel.Content = customerName.Text;
                }
            }
        }
        #endregion
    }
}

