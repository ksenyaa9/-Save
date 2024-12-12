using Microsoft.Win32;
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
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        private int edit = 0;
        private Agent currentAgent = new Agent();
        List<Agent> TableList;

        public AddEditPage(Agent SelectedAgent)
        {
            InitializeComponent();
            //currentAgent = ГерасимоваГлазкиSaveEntities.GetContext().Agent.FirstOrDefault();

            if (SelectedAgent != null) {
                currentAgent = SelectedAgent;
                edit = 1;
            }

            DataContext = currentAgent;

            LoadAgentTypes();

            

        }

    private void LoadAgentTypes()
    {
        var agentTypes = ГерасимоваГлазкиSaveEntities.GetContext().AgentType.ToList();
        ComboType.ItemsSource = agentTypes;
    }




        private void ChangePictureBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog myOpenFileDialog = new OpenFileDialog();

            if(myOpenFileDialog.ShowDialog() == true)
            {
                
                currentAgent.Logo = myOpenFileDialog.FileName;
                LogoImage.Source = new BitmapImage(new Uri(myOpenFileDialog.FileName));
            }

        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
           
            StringBuilder errors = new StringBuilder();

            if (string.IsNullOrWhiteSpace(currentAgent.Title))
                errors.AppendLine("Укажите наименование агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Address))
                errors.AppendLine("Укажите адрес агента");
            if (string.IsNullOrWhiteSpace(currentAgent.DirectorName))
                errors.AppendLine("Укажите ФИО директора");
            if (ComboType.SelectedItem == null)
                errors.AppendLine("Укажите тип агента");

            if (string.IsNullOrWhiteSpace(currentAgent.Priority.ToString()))
                errors.AppendLine("Укажите приоритет агента");
            if (currentAgent.Priority <= 0)
                errors.AppendLine("Укажите положительные приоритет агента");
            if (string.IsNullOrWhiteSpace(currentAgent.INN))
                errors.AppendLine("Укажите ИНН агента");
            if (string.IsNullOrWhiteSpace(currentAgent.KPP))
                errors.AppendLine("Укажите КПП агента");
            if (string.IsNullOrWhiteSpace(currentAgent.Phone))
                errors.AppendLine("Укажите телефон агента");

            else
            {
                if (edit == 0)
                {
                    string ph = currentAgent.Phone.Replace("(", "").Replace("-", "").Replace("+", "").Replace(")", "");
                    if (((ph[1] == '9' || ph[1] == '4' || ph[1] == '8') && ph.Length != 11) || (ph[1] == '3' && ph.Length != 12))
                        errors.AppendLine("Укажите правильно телефон агента");
                }
            }

            if (string.IsNullOrWhiteSpace(currentAgent.Email))
                errors.AppendLine("Укажите почту агента");
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString());
                return;
            }

            if(currentAgent.ID == 0)
                ГерасимоваГлазкиSaveEntities.GetContext().Agent.Add(currentAgent);

            try
            {
                ГерасимоваГлазкиSaveEntities.GetContext().SaveChanges();
                MessageBox.Show("информация сохранения");
                Manager.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при сохранении: " + ex.Message);
                
            }

            /*catch (Exception ex) {
                MessageBox.Show(ex.Message.ToString());
            }*/




        }

        
        private void DeleteBtn_Click(object sender, RoutedEventArgs e)
        {
            var currentAgentD = (sender as Button).DataContext as Agent;

            //проверка
            var currentSake = ГерасимоваГлазкиSaveEntities.GetContext().ProductSale.ToList();
            currentSake = currentSake.Where(p => p.AgentID == currentAgentD.ID).ToList();

            if (currentSake.Count != 0)
            {
                MessageBox.Show("Невозможно выполнить удаление");
            }
            else
            {
                if (MessageBox.Show("Вы точно хотите удалить агента?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    try
                    {
                        ГерасимоваГлазкиSaveEntities.GetContext().Agent.Remove(currentAgentD);
                        ГерасимоваГлазкиSaveEntities.GetContext().SaveChanges();
                        Manager.MainFrame.Navigate(new AgentPage());


                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString());
                    }


                }
            }


        }
    }
}
