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
    /// Логика взаимодействия для ToursPage.xaml
    /// </summary>
    public partial class ToursPage : Page
    {
        public ToursPage()
        {
            InitializeComponent();
            lvTour.ItemsSource = BaseClass.BD.Tour.ToList();
            List<Type> type = BaseClass.BD.Type.ToList();
            cbType.Items.Add("Все типы");
            for (int i = 0; i < type.Count; i++)
            {
                cbType.Items.Add(type[i].Name);
            }
            cbType.SelectedIndex = 0;
            cbSort.SelectedIndex = 0;
            tbCost.Text = GetCost(BaseClass.BD.Tour.ToList()).ToString("F3") + " РУБ";
        }

        private double GetCost(List<Tour> tours)
        {
            double summa = 0;
            foreach (Tour tour in tours)
            {
                summa = summa + (double)tour.Price * (double)tour.TicketCount;
            }
            return summa;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.MainFrame.Navigate(new HotelsPage());
        }

        void Filter()
        {
            List<Tour> tour = new List<Tour>();
            string SelectType = cbType.SelectedItem.ToString();
            int index = cbType.SelectedIndex;
            if (index != 0)
            {
                index = BaseClass.BD.Type.FirstOrDefault(x => x.Name == SelectType).Id;
                tour = TourType(index);
            }
            else
            {
                tour = BaseClass.BD.Tour.ToList();
            }
            try
            {
                if (!string.IsNullOrWhiteSpace(tbSearch.Text))
                {
                    tour = tour.Where(x => x.Name.ToLower().Contains(tbSearch.Text.ToLower())).ToList();
                }
            }
            catch { }
            if (cbActual.IsChecked == true)
            {
                tour = tour.Where(x => x.IsActual == true).ToList();
            }
            switch (cbSort.SelectedIndex)
            {
                case 1:
                        tour.Sort((x, y) => x.Price.CompareTo(y.Price));
                    break;
                case 2:
                        tour.Sort((x, y) => x.Price.CompareTo(y.Price));
                        tour.Reverse();
                    break;
            }
            lvTour.ItemsSource = tour;
            if (tour.Count == 0)
            {
                MessageBox.Show("В базе данных отсутствуют данные удовлетворяющие заданным условиям");
            }

            tbCost.Text = GetCost(tour).ToString("F3") + " РУБ";
        }

        private List<Tour> TourType(int index)
        {
            List<Tour> tour = new List<Tour>();
            List<TypeOfTour> typeOfTours = BaseClass.BD.TypeOfTour.ToList();
            foreach (TypeOfTour typeOfTour in typeOfTours)
            {
                if (typeOfTour.TypeId == index)
                {
                    tour.Add(typeOfTour.Tour);
                }
            }
            tour.Distinct();
            return tour;
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            Filter();
        }

        private void cbType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void tbSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void cbSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }
    }
}
