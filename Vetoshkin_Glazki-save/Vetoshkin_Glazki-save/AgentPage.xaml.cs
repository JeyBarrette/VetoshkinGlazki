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

namespace Vetoshkin_Glazki_save
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
        List<int> SelectPriority = new List<int>();

        public AgentPage()
        {
            InitializeComponent();
            var currentGlazki = Vetoshkin_GlazkiEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentGlazki;

            SortBy.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;

            UpdateAgents();
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

            Boolean IFUpdate = true;

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
                            IFUpdate = false;
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
                            IFUpdate = false;
                        }
                        break;
                }
            }

            if (IFUpdate)
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

                AgentListView.ItemsSource = CurrentPageList;
                AgentListView.Items.Refresh();
            }
        }

        private void UpdateAgents()
        {
            var currentGlazki = Vetoshkin_GlazkiEntities.GetContext().Agent.ToList();

            currentGlazki = currentGlazki.Where(p => (p.Title.ToLower().Contains(TBoxSearch.Text.ToLower())
            || p.Phone.Replace("+", "").Replace(" ", "").Replace("(", "").Replace(")", "").Replace("-", "").Contains(TBoxSearch.Text)
            || p.Email.ToLower().Contains(TBoxSearch.Text.ToLower()))).ToList();

            if (SortBy.SelectedIndex == 1)
            {
                currentGlazki = currentGlazki.OrderBy(p => p.Title).ToList();
            }
            if (SortBy.SelectedIndex == 2)
            {
                currentGlazki = currentGlazki.OrderByDescending(p => p.Title).ToList();
            }
            if (SortBy.SelectedIndex == 3)
            {
                currentGlazki = currentGlazki.OrderBy(p => p.SalePercent).ToList();
            }
            if (SortBy.SelectedIndex == 4)
            {
                currentGlazki = currentGlazki.OrderByDescending(p => p.SalePercent).ToList();
            }
            if (SortBy.SelectedIndex == 5)
            {
                currentGlazki = currentGlazki.OrderBy(p => p.Priority).ToList();
            }
            if (SortBy.SelectedIndex == 6)
            {
                currentGlazki = currentGlazki.OrderByDescending(p => p.Priority).ToList();
            }

            if (ComboType.SelectedIndex == 0)
            {
                currentGlazki = currentGlazki.ToList();
            }
            if (ComboType.SelectedIndex == 1)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "МФО").ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ООО").ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ЗАО").ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "МКК").ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ОАО").ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeString == "ПАО").ToList();
            }
            AgentListView.ItemsSource = currentGlazki;
            TableList = currentGlazki;
            ChangePage(0, 0);
        }

        private void TBoxSearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void SortBy_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void ComboType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateAgents();
        }

        private void PageListBox_MouseUp(object sender, MouseButtonEventArgs e)
        {
            ChangePage(0, Convert.ToInt32(PageListBox.SelectedItem.ToString()) - 1);
        }

        private void RightDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(2, null);
        }

        private void LeftDirButton_Click(object sender, RoutedEventArgs e)
        {
            ChangePage(1, null);
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage(null));
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            Manager.MainFrame.Navigate(new AddEditPage((sender as Button).DataContext as Agent));
        }

        private void Page_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (Visibility == Visibility.Visible)
            {
                Vetoshkin_GlazkiEntities.GetContext().ChangeTracker.Entries().ToList().ForEach(p => p.Reload());
                AgentListView.ItemsSource = Vetoshkin_GlazkiEntities.GetContext().Agent.ToList();
                UpdateAgents();
            }
        }

        private void PriorityButton_Click(object sender, RoutedEventArgs e)
        {
            int max_priority = 0;
            foreach (Agent agentPriority in AgentListView.SelectedItems)
            {
                if (max_priority < agentPriority.Priority)
                    max_priority = agentPriority.Priority;
            }

            PriorityEdit priorityEdit = new PriorityEdit(max_priority);
            priorityEdit.ShowDialog();

            if (string.IsNullOrEmpty(priorityEdit.PriorityBox.Text))
            {
                return;
            }
            if (!string.IsNullOrWhiteSpace(priorityEdit.PriorityBox.Text) && Convert.ToInt32(priorityEdit.PriorityBox.Text) > 0 && priorityEdit.CheckClick == true)
            {

                foreach (Agent agentPriority in AgentListView.SelectedItems)
                {
                    agentPriority.Priority = Convert.ToInt32(priorityEdit.PriorityBox.Text);
                }
                try
                {
                    Vetoshkin_GlazkiEntities.GetContext().SaveChanges();
                    MessageBox.Show("Информация сохранена");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                }
            }
            UpdateAgents();
        }

        private void AgentListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (AgentListView.SelectedItems.Count > 1)
            {
                PriorityButton.Visibility = Visibility.Visible;
            }
            else
            {
                PriorityButton.Visibility = Visibility.Hidden;
            }
        }
    }
}
