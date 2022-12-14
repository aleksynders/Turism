using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
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
    /// Логика взаимодействия для AddOrUpdate.xaml
    /// </summary>
    public partial class AddOrUpdate : Page
    {
        Hotel hotel;
        bool flagUpdate = false;
        public AddOrUpdate()
        {
            InitializeComponent();
            UploadFields();
        }

        public AddOrUpdate(Hotel hotel)
        {
            InitializeComponent();
            UploadFields();
            this.hotel = hotel;
            flagUpdate = true;
            Header.Text = "Изменение существующего отеля";
            btAdd.Content = "Измененить отель";
            tbName.Text = hotel.Name;
            tbStars.Text = Convert.ToString(hotel.CountOfStars);
            cbCountry.SelectedValue = hotel.CountryCode;
            tbDescription.Text = hotel.Description;
        }

        private void UploadFields()
        {
            cbCountry.ItemsSource = BaseClass.BD.Country.ToList();
            cbCountry.SelectedValuePath = "Code";
            cbCountry.DisplayMemberPath = "Name";
            cbCountry.SelectedIndex = 0;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            FrameClass.MainFrame.Navigate(new HotelsPage());
        }

        private void tbStars_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!((e.Text == "0") || (e.Text == "1") || (e.Text == "2") || (e.Text == "3") || (e.Text == "4") || (e.Text == "5")))
            {
                e.Handled = true;
            }
        }

        private void btAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(tbName.Text))
                {
                    MessageBox.Show("Наименование отеля должно быть заполнено!");
                    return;
                }
                if (string.IsNullOrWhiteSpace(tbStars.Text))
                {
                    MessageBox.Show("Кол-во звёзд у отеля должно быть заполнено!");
                    return;
                }
                if (string.IsNullOrWhiteSpace(cbCountry.Text))
                {
                    MessageBox.Show("Страна отеля должна быть указана!");
                    return;
                }
                if (flagUpdate == false)
                {
                    hotel = new Hotel();
                }
                hotel.Name = Convert.ToString(tbName.Text);
                hotel.CountOfStars = Convert.ToInt32(tbStars.Text);
                hotel.CountryCode = Convert.ToString(cbCountry.SelectedValue);
                hotel.Description = Convert.ToString(tbDescription.Text);
                if (flagUpdate == false)
                {
                    BaseClass.BD.Hotel.Add(hotel);
                }
                BaseClass.BD.SaveChanges();
                if (flagUpdate == true)
                {
                    MessageBox.Show("Запись успешно изменена");
                }
                else
                {
                    MessageBox.Show("Запись добавлена в базу");
                }
                FrameClass.MainFrame.Navigate(new HotelsPage());
            }
            catch
            {
                    MessageBox.Show("Ошибка :(");
            }
        }
    }
}
