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

namespace ГерасимоваГлазкиSave
{
    /// <summary>
    /// Логика взаимодействия для AgentPage.xaml
    /// </summary>
    public partial class AgentPage : Page
    {
        public AgentPage()
        {
            InitializeComponent();

            var currentServices = ГерасимоваГлазкиSaveEntities.GetContext().Agent.ToList();

            ServiceListView.ItemsSource = currentServices;
            
            ComboSort.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;

            UpdateAgent();
        }

        private void UpdateAgent()
        {

            var currentAgent = ГерасимоваГлазкиSaveEntities.GetContext().Agent.ToList();

            if (ComboSort.SelectedIndex == 0)
            {
                currentAgent = currentAgent.ToList();
            }
            if (ComboSort.SelectedIndex == 1)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 2)
            {
                currentAgent = currentAgent.OrderBy(p => p.Title).ToList();
            }
            if (ComboSort.SelectedIndex == 3) { 
                //пока что нет
            }
            if (ComboSort.SelectedIndex == 4)
            {
                //пока что нет
            }
            if (ComboSort.SelectedIndex == 5)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
            }
            if (ComboSort.SelectedIndex == 6)
            {
                currentAgent = currentAgent.OrderBy(p=> p.Priority).ToList();
            }

           //фильтрация по типу
            if (ComboType.SelectedIndex == 0)
            {
                currentAgent = currentAgent.ToList();
            }
            
            if (ComboType.SelectedIndex == 1)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "МФО").ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ООО").ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ЗАО").ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "МКК").ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ОАО").ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentAgent = currentAgent.Where(p => p.AgentType.Title == "ПАО").ToList();
            }


            // Приводим текст поиска к нижнему регистру один раз
            string searchText = TBoxSearch.Text.ToLower().Replace("+", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "");
           // Фильтруем список агентов по всем трем полям одновременно
            currentAgent = currentAgent
                .Where(p => p.Title.ToLower().Contains(searchText) ||
                           p.Phone.Replace("+", "").Replace("(", "").Replace(")", "").Replace(" ", "").Replace("-", "").Contains(searchText)||
                           p.Email.ToLower().Contains(searchText))
                .ToList();

           

            //отображение списка
            ServiceListView.ItemsSource = currentAgent.ToList();
            
        }

        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage());
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgent();
        }
    }
}
