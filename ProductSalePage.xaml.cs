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
    /// Логика взаимодействия для ProductSalePage.xaml
    /// </summary>
    /// 
   
    public partial class ProductSalePage : Page
    {
        private Agent currentAgent = new Agent();
        private List<Product> _allProducts; // Список всех продуктов
        private List<ProductSale> _currentProductSales; // Список текущих продаж
        public ProductSalePage(Agent SelectedAgent)
        {

            InitializeComponent();

            if (SelectedAgent != null)
            {
                currentAgent = SelectedAgent;
                DataContext = currentAgent;

                // Инициализируем список продаж
                LoadCurrentProductSales();
            }
            else
            {
                ProductSaleList.ItemsSource = new List<ProductSale>();
            }

            // Загружаем все продукты и устанавливаем источник данных для ComboBox
            _allProducts = ГерасимоваГлазкиSaveEntities.GetContext().Product.ToList();
            ComboProduct.ItemsSource = _allProducts;
            /* 
             InitializeComponent();

             if (SelectedAgent != null)
             {
                 currentAgent = SelectedAgent;
                 DataContext = currentAgent;

                 // Инициализируем список продаж
                 LoadCurrentProductSales();
             }
             else
             {
                 ProductSaleList.ItemsSource = new List<ProductSale>();
             }

             // Загружаем все продукты и устанавливаем источник данных для ComboBox
             _allProducts = ГерасимоваГлазкиSaveEntities.GetContext().Product.ToList();
             ComboProduct.ItemsSource = _allProducts;*/
        }



        private void LoadCurrentProductSales()
        {
            _currentProductSales = ГерасимоваГлазкиSaveEntities.GetContext().ProductSale
                .Where(ps => ps.AgentID == currentAgent.ID)
                .ToList();

            ProductSaleList.ItemsSource = _currentProductSales; // Устанавливаем источник данных для списка продаж
        }

        private void SaveProuct_Click(object sender, RoutedEventArgs e)
        {

            // Получаем выбранный продукт из ComboBox
    var selectedProduct = ComboProduct.SelectedItem as Product;

            // Проверяем, был ли выбран продукт
            if (selectedProduct == null)
            {
                MessageBox.Show("Пожалуйста, выберите продукт для добавления.");
                return;
            }

            // Получаем количество из TextBox (например, для количества продукта)
            int productCount;
            if (!int.TryParse(ProductCountTextBox.Text, out productCount) || productCount <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное количество.");
                return;
            }

            // Создаем новый объект ProductSale
            var newSale = new ProductSale
            {
                AgentID = currentAgent.ID,
                ProductID = selectedProduct.ID,
                SaleDate = StartDate.SelectedDate ?? DateTime.Now,
                ProductCount = productCount
            };

            // Добавляем новый объект в контекст и сохраняем изменения
            ГерасимоваГлазкиSaveEntities.GetContext().ProductSale.Add(newSale);
            ГерасимоваГлазкиSaveEntities.GetContext().SaveChanges();

            MessageBox.Show("Информация сохранена");

            // Обновляем список продаж
            LoadCurrentProductSales();

            // Очистка полей ввода (по желанию)
            ComboProduct.SelectedItem = null;
            ProductCountTextBox.Clear();
            StartDate.SelectedDate = null;
            /*// Получаем выбранный продукт из ComboBox
            var selectedProduct = ComboProduct.SelectedItem as Product;

            // Получаем количество из TextBox (например, для количества продукта)
            int productCount;
            if (!int.TryParse(ProductCountTextBox.Text, out productCount) || productCount <= 0)
            {
                MessageBox.Show("Пожалуйста, введите корректное количество.");
                return;
            }

            if (selectedProduct != null)
            {
                // Создаем новый объект ProductSale
                var newSale = new ProductSale
                {
                    AgentID = currentAgent.ID,
                    ProductID = selectedProduct.ID,
                    SaleDate = StartDate.SelectedDate ?? DateTime.Now,
                    ProductCount = productCount
                };

                // Добавляем новый объект в контекст и сохраняем изменения
                ГерасимоваГлазкиSaveEntities.GetContext().ProductSale.Add(newSale);
                ГерасимоваГлазкиSaveEntities.GetContext().SaveChanges();

                MessageBox.Show("Информация сохранена");

                // Обновляем список продаж
                LoadCurrentProductSales();

                // Очистка полей ввода (по желанию)
                ComboProduct.SelectedItem = null;
                ProductCountTextBox.Clear();
                StartDate.SelectedDate = null;
            }
            else
            {
                MessageBox.Show("Пожалуйста, выберите продукт для добавления.");
            }*/
        
        /*
        // Получаем выбранный продукт из ComboBox
        var selectedProduct = ComboProduct.SelectedItem as Product;

        // Получаем количество из TextBox (например, для количества продукта)
        int productCount;
        if (!int.TryParse(ProductCountTextBox.Text, out productCount) || productCount <= 0)
        {
            MessageBox.Show("Пожалуйста, введите корректное количество.");
            return;
        }

        if (selectedProduct != null)
        {
            // Создаем новый объект ProductSale
            var newSale = new ProductSale
            {
                AgentID = currentAgent.ID, // Указываем ID агента
                ProductID = selectedProduct.ID, // Указываем ID выбранного продукта
                SaleDate = StartDate.SelectedDate ?? DateTime.Now, // Указываем дату продажи (если выбрана)
                ProductCount = productCount // Указываем количество, введенное пользователем
            };

            // Добавляем новый объект в контекст и сохраняем изменения
            ГерасимоваГлазкиSaveEntities.GetContext().ProductSale.Add(newSale);
            ГерасимоваГлазкиSaveEntities.GetContext().SaveChanges();

            MessageBox.Show("информация сохранена");

            // Обновляем список продаж
            LoadCurrentProductSales();

            // Очистка полей ввода (по желанию)
            ComboProduct.SelectedItem = null;
            ProductCountTextBox.Clear();
            StartDate.SelectedDate = null;
        }
        else
        {
            MessageBox.Show("Пожалуйста, выберите продукт для добавления.");
        }*/
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {

            // Запрашиваем подтверждение у пользователя
            if (MessageBox.Show("Вы точно хотите выполнить удаление?", "Внимание!", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    // Получаем контекст базы данных
                    var context = ГерасимоваГлазкиSaveEntities.GetContext();

                    // Получаем выбранный элемент из списка
                    var selectedItem = (sender as Button).DataContext as ProductSale;

                    if (selectedItem != null)
                    {
                        // Удаляем элемент из контекста
                        context.ProductSale.Remove(selectedItem);
                        context.SaveChanges(); // Сохраняем изменения в базе данных

                        // Обновляем список продаж
                        LoadProductSalesForCurrentAgent();

                        // Информируем пользователя об успешном удалении
                        MessageBox.Show("Элемент удален.");
                    }
                    else
                    {
                        MessageBox.Show("Пожалуйста, выберите элемент для удаления.");
                    }
                }
                catch (Exception ex)
                {
                    // Обрабатываем исключения и выводим сообщение об ошибке
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}");
                }
            }
        }

        private void LoadProductSalesForCurrentAgent()
        {
            var currentProductSales = ГерасимоваГлазкиSaveEntities.GetContext().ProductSale
                .Where(ps => ps.AgentID == currentAgent.ID) 
                .ToList();

               
                ProductSaleList.ItemsSource = currentProductSales;
        }

        private void EditTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = (sender as TextBox).Text.ToLower();

            // Фильтруем список продуктов на основе введенного текста
            var filteredProducts = _allProducts.Where(p => p.Title.ToLower().Contains(searchText)).ToList();

            // Устанавливаем отфильтрованный список как источник данных для ComboBox
            ComboProduct.ItemsSource = filteredProducts;

            // Открываем выпадающий список, если есть результаты
            if (filteredProducts.Count > 0)
            {
                ComboProduct.IsDropDownOpen = true;
            }
        }

        private void ComboProduct_Loaded(object sender, RoutedEventArgs e)
        {
            // Получаем доступ к внутреннему TextBox
            TextBox editTextBox = ComboProduct.Template.FindName("PART_EditableTextBox", ComboProduct) as TextBox;

            if (editTextBox != null)
            {
                editTextBox.TextChanged += EditTextBox_TextChanged; // Подписываемся на событие TextChanged
            }
        }
    }
}
