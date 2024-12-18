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
        int CountRecords;
        int CountPage;
        int CurrentPage = 0;

        List<Agent> CurrentPageList = new List<Agent>();
        List<Agent> TableList;

        public AgentPage()
        {
            InitializeComponent();

            var currentServices = ГерасимоваГлазкиSaveEntities.GetContext().Agent.ToList();

            ServiceListView.ItemsSource = currentServices;
            
            ComboSort.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;

            UpdateAgent();
          
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
                PageListBox.SelectedIndex = CurrentPage;

                min = CurrentPage * 10 + 10 < CountRecords ? CurrentPage * 10 + 10 : CountRecords;
                TBCount.Text = min.ToString();
                TBAllRecords.Text = " из " + CountRecords.ToString();

                ServiceListView.ItemsSource = CurrentPageList;

                ServiceListView.Items.Refresh();
            }
           
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

                currentAgent = currentAgent.OrderBy(p => p.Discount).ToList();
            }
            if (ComboSort.SelectedIndex == 4)
            {
                
                currentAgent = currentAgent.OrderByDescending(p => p.Discount).ToList();
            }
            if (ComboSort.SelectedIndex == 5)
            {
                currentAgent = currentAgent.OrderBy(p => p.Priority).ToList();
            }
            if (ComboSort.SelectedIndex == 6)
            {
                currentAgent = currentAgent.OrderByDescending(p => p.Priority).ToList();
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
            ServiceListView.ItemsSource = currentAgent;

            TableList = currentAgent;
            ChangePage(0, 0);
            
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

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
            
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible) {
                ГерасимоваГлазкиSaveEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                ServiceListView.ItemsSource = ГерасимоваГлазкиSaveEntities.GetContext().Agent.ToList();
                UpdateAgent();
               

            }
        }

        private void ServiceListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(ServiceListView.SelectedItems.Count >1)
                PriorityButton.Visibility = Visibility.Visible;
            else
                PriorityButton.Visibility = Visibility.Hidden;
                

        }

        private void PriorityButton_Click(object sender, RoutedEventArgs e)
        {
            int maxprior = 0;
            foreach (Agent agent in ServiceListView.SelectedItems) {
                if (agent.Priority > maxprior) {
                    maxprior = agent.Priority;
                }
            }
            SetWindow myWindow = new SetWindow(maxprior);
            myWindow.ShowDialog();
            if (string.IsNullOrEmpty(myWindow.TBPriority.Text) || !int.TryParse(myWindow.TBPriority.Text, out int priorityValue) || priorityValue < 0)
            {
                MessageBox.Show("Измнений не произошло");
            }
            else
            {
                int newPriority = Convert.ToInt32(myWindow.TBPriority.Text);

                foreach (Agent agent in ServiceListView.SelectedItems)
                {
                    agent.Priority = newPriority;
                }
                try
                {
                    ГерасимоваГлазкиSaveEntities.GetContext().SaveChanges();
                    MessageBox.Show("информация сохранена");
                    UpdateAgent();
                }
                catch (Exception ex) { 
                    MessageBox.Show(ex.Message.ToString());


                }

            }


        }

       
    }
}
