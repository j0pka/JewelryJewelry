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

namespace BocharovaJewelryBocharovaJewelry
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class Page1 : Page
    {
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;
        List<Product> CurrentPageList = new List<Product>();
        List<Product> TableList;
        public Page1(User user)
        {
            InitializeComponent();          
            var currenDecor = Bocharova_JewelryEntities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currenDecor;
            ComboType.SelectedIndex = 0;
            UpdateProduct();

            if (user != null)
            {
                FIOTB.Text = user.UserSurname + "" + user.UserName + "" + user.UserPatronymic;

                switch (user.UserRole)
                {
                    case 1:
                        RoleTB.Text = "Администратор";break;
                    case 2:
                        RoleTB.Text = "Менеджер"; break;
                    case 3:
                        RoleTB.Text = "Клиент"; break;
                    default:
                        RoleTB.Text = "Гость";break;
                }
            }
            else
            {
                FIOTB.Text = "Гость";
                RoleTB.Text = "Гость";
            }
            UpdateProduct();

        }

        private void UpdateProduct()
        {
            var currenDecor = Bocharova_JewelryEntities.GetContext().Product.ToList();

            if (ComboType.SelectedIndex == 0)
            {
                currenDecor = currenDecor.Where( p=> (p.ProductCurrentDiscount >= 0 && p.ProductCurrentDiscount <= 100) ).ToList(); 
            }
            if (ComboType.SelectedIndex == 1)
            {
                currenDecor=currenDecor.Where( p=> ( p.ProductCurrentDiscount >= 0 && p.ProductCurrentDiscount < 10 ) ).ToList();

            }
            if (ComboType.SelectedIndex == 2)
            {
                currenDecor = currenDecor.Where(p => (p.ProductCurrentDiscount >= 10 && p.ProductCurrentDiscount < 15)).ToList();

            }
            if (ComboType.SelectedIndex == 3)
            {
                currenDecor = currenDecor.Where(p => (p.ProductCurrentDiscount >= 15 && p.ProductCurrentDiscount < 100)).ToList();
            }

            currenDecor=currenDecor.Where(p=> p.ProductName.ToLower().Contains(TBoxSearch.Text.ToLower())).ToList();

            if (RButtonDown.IsChecked.Value)
            {
                currenDecor=currenDecor.OrderByDescending(p => p.ProductCost).ToList();
            }
            if(RButtonUp.IsChecked.Value)
            {
                currenDecor=currenDecor.OrderBy(p => p.ProductCost).ToList();
            }

            ProductListView.ItemsSource= currenDecor;
            TableList = currenDecor;
            ChangePage(0, 0);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {

            UpdateProduct();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            UpdateProduct();
        }

        private void RButtonUp_Checked(object sender, RoutedEventArgs e)
        {

            UpdateProduct();
        }

        
        private void RButtonDown_Checked(object sender, RoutedEventArgs e)
        {

            UpdateProduct();
        }

        
        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }
        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString())-1);
        }

        private void ChangePage(int direction, int? selectedPage)
        {
            CurrentPageList.Clear();
            CountRecords = TableList.Count;

            if (CountRecords % 10 > 0)
            {
                CountPage = CountRecords / 10 + 1;
            }
            else
            {
                CountPage = CountRecords / 10;
            }
            Boolean Ifupdate = true;
            int min;
            if (selectedPage.HasValue)
            {
                if (selectedPage >= 0 && selectedPage <= CountPage)
                {
                    CurrentPage = (int)selectedPage;
                    min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                    for (int i = CurrentPage * 10; i < min; i++)
                    {
                        CurrentPageList.Add(TableList[i]);
                    }
                }
            }
            else
            {
                switch (direction)
                {
                    case 1:
                        if (CurrentPage > 0)
                        {
                            CurrentPage--;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;

                    case 2:
                        if (CurrentPage < CountPage - 1)
                        {
                            CurrentPage++;
                            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                            for (int i = CurrentPage * 10; i < min; i++)
                            {
                                CurrentPageList.Add(TableList[i]);
                            }
                        }
                        else
                        {
                            Ifupdate = false;
                        }
                        break;
                }
            }
            if (Ifupdate)
            {
                PageListBox.Items.Clear();
                for (int i = 1; i <= CountPage; i++)
                {
                    PageListBox.Items.Add(i);
                }

            }
            PageListBox.SelectedIndex = CurrentPage;
            min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
            TBCount.Text = min.ToString();
            TBAllRecords.Text = " из " + CountRecords.ToString();
            ProductListView.ItemsSource = CurrentPageList;
            ProductListView.Items.Refresh();
        }

       
    }
}
