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
        public AgentPage()
        {
            InitializeComponent();
            var currentGlazki = Vetoshkin_GlazkiEntities.GetContext().Agent.ToList();
            AgentListView.ItemsSource = currentGlazki;

            SortBy.SelectedIndex = 0;
            ComboType.SelectedIndex = 0;

            UpdateAgents();
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

            }
            if (SortBy.SelectedIndex == 4)
            {

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
                currentGlazki = currentGlazki.Where(p => p.AgentTypeID == 3).ToList();
            }
            if (ComboType.SelectedIndex == 2)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeID == 5).ToList();
            }
            if (ComboType.SelectedIndex == 3)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeID == 1).ToList();
            }
            if (ComboType.SelectedIndex == 4)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeID == 2).ToList();
            }
            if (ComboType.SelectedIndex == 5)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeID == 4).ToList();
            }
            if (ComboType.SelectedIndex == 6)
            {
                currentGlazki = currentGlazki.Where(p => p.AgentTypeID == 6).ToList();
            }
            AgentListView.ItemsSource = currentGlazki;
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
    }
}
