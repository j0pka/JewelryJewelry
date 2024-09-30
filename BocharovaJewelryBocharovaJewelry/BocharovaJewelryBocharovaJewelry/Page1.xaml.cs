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
        public Page1()
        {
            InitializeComponent();
            
            var currenDecor = Bocharova_JewelryEntities.GetContext().Product.ToList();
            ProductListView.ItemsSource = currenDecor;

            ComboType.SelectedIndex = 0;

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
    }
}
