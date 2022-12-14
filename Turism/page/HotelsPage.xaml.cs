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

namespace Turism
{
    /// <summary>
    /// Логика взаимодействия для HotelsPage.xaml
    /// </summary>
    public partial class HotelsPage : Page
    {
        PageChange pc = new PageChange();
        List<Hotel> HotelsFilter = new List<Hotel>();

        public HotelsPage()
        {
            InitializeComponent();
            dgHotel.ItemsSource = BaseClass.BD.Hotel.ToList();
            tbCountZ.Text = Convert.ToString(BaseClass.BD.Hotel.ToList().Count);
            HotelsFilter = BaseClass.BD.Hotel.ToList();
            pc.CountPage = BaseClass.BD.Hotel.ToList().Count;
            tbCountS.Text = pc.CountPages.ToString();
            tbCurrentS.Text = pc.CurrentPage.ToString();
            DataContext = pc;
            tbSCount.Text = "10";
            pc.VisibleButton[0] = "Hidden";
            if (pc.CountPages > 5)
            {
                pc.VisibleButton[1] = "Visible";
            }
            else
            {
                pc.VisibleButton[1] = "Hidden";
            }
        }


        public HotelsPage(string count)
        {
            InitializeComponent();
            dgHotel.ItemsSource = BaseClass.BD.Hotel.ToList();
            tbCountZ.Text = Convert.ToString(BaseClass.BD.Hotel.ToList().Count);
            HotelsFilter = BaseClass.BD.Hotel.ToList();
            pc.CountPage = BaseClass.BD.Hotel.ToList().Count;
            tbCountS.Text = pc.CountPages.ToString();
            tbCurrentS.Text = pc.CurrentPage.ToString();
            DataContext = pc;
            tbSCount.Text = count;
            pc.VisibleButton[0] = "Hidden";
            if (pc.CountPages > 5)
            {
                pc.VisibleButton[1] = "Visible";
            }
            else
            {
                pc.VisibleButton[1] = "Hidden";
            }
        }

        private void GoPage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            TextBlock tb = (TextBlock)sender;
            switch (tb.Uid)
            {
                case "prev":
                    pc.CurrentPage--;
                    break;
                case "next":
                    pc.CurrentPage++;
                    break;
                default:
                    pc.CurrentPage = Convert.ToInt32(tb.Text);
                    break;
            }
            dgHotel.ItemsSource = HotelsFilter.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
            tbCurrentS.Text = pc.CurrentPage.ToString();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.MainFrame.Navigate(new ToursPage());
        }

        private void tbSCount_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!(Char.IsDigit(e.Text, 0)))
            {
                e.Handled = true;
            }
        }

        private void txtNextFirst_MouseDown(object sender, MouseButtonEventArgs e)
        {
            pc.CurrentPage = 1;
            dgHotel.ItemsSource = HotelsFilter.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
            tbCurrentS.Text = pc.CurrentPage.ToString();
        }

        private void txtNextLast_MouseDown(object sender, MouseButtonEventArgs e) 
        {
            pc.CurrentPage = pc.CountPages;
            dgHotel.ItemsSource = HotelsFilter.Skip(pc.CurrentPage * pc.CountPage - pc.CountPage).Take(pc.CountPage).ToList();
            tbCurrentS.Text = pc.CurrentPage.ToString();
        }
        bool First = false;

        private void tbSCount_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                pc.CountPage = Convert.ToInt32(tbSCount.Text);
            }
            catch
            {
                pc.CountPage = HotelsFilter.Count;
            }
            pc.Countlist = HotelsFilter.Count;
            dgHotel.ItemsSource = HotelsFilter.Skip(0).Take(pc.CountPage).ToList();
            if (First == true)
            {
                pc.CurrentPage = 1;
            }
            else
            {
                First = true;
            }
            tbCountS.Text = pc.CountPages.ToString();
            tbCurrentS.Text = pc.CurrentPage.ToString();
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.MainFrame.Navigate(new AddOrUpdate());
        }

        private void btnUpd_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            int index = Convert.ToInt32(btn.Uid);
            Hotel hotel = BaseClass.BD.Hotel.FirstOrDefault(x => x.Id == index);
            FrameClass.MainFrame.Navigate(new AddOrUpdate(hotel));
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            if (dgHotel.SelectedItems.Count == 0)
            {
                MessageBox.Show("Для удаления не выбран элемент(ы)");
            }
            else
            {
                int k = 0;
                foreach (Hotel hotel in dgHotel.SelectedItems)
                {
                    List<HotelOfTour> hotelOfTour = BaseClass.BD.HotelOfTour.Where(x => x.HotelId == hotel.Id).ToList();
                    foreach (HotelOfTour hotelOfTour1 in hotelOfTour)
                    {
                        if (hotelOfTour1.Tour.IsActual == true)
                        {
                            MessageBox.Show("Отель: \"" + hotel.Name + "\" не  может быть удалён так как он находится в числе подходящих отелей для актуальных туров", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                            return;
                        }
                    }
                    if (MessageBox.Show("Вы уверены что хотите удалить отель: \"" + hotel.Name + "\"?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                    {
                        BaseClass.BD.Hotel.Remove(hotel);
                        MessageBox.Show("Отель: \"" + hotel.Name + "\" был удалён");
                        k++;
                    }
                }
                if (k != 0)
                {
                    BaseClass.BD.SaveChanges();
                    FrameClass.MainFrame.Navigate(new HotelsPage(tbSCount.Text));
                }
            }
        }
    }
}
