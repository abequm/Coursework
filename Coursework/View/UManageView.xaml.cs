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

namespace Coursework.View
{
    /// <summary>
    /// Логика взаимодействия для UManageView.xaml
    /// </summary>
    public partial class UManageView : UserControl
    {
        public UManageView()
        {
            InitializeComponent();
        }

        public void CountPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !char.IsDigit(e.Text, 0);

        }
        public void DigitnDotPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (char.IsDigit(e.Text, 0) | e.Text == ".")
            {
                e.Handled = false;
                return;
            }
            e.Handled = true;
        }
        public void AvgScorePreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (e.Text[0] != '.' && !char.IsDigit(e.Text, 0) && e.Text[0]!=',')
            {
                e.Handled = true;
            }


        }

        private void AvgScoreTextChanged(object sender, TextChangedEventArgs e)
        {
            var self = (TextBox)sender;
            if (self.Text.Length > 1 && self.Text[1] != '.' && self.Text[1]!=',')
            {
                self.Text = self.Text.Replace(self.Text[1].ToString(), "");
            }
            if (self.Text.Count(p => p == '.') > 1)
            {
                self.Text = self.Text.Substring(0, self.Text.Length - 1);
            }

            if (self.Text.Length >= 5)
            {
                self.Text = self.Text.Substring(0, 4);
            }
        }



        private void YearTextChanged(object sender, TextChangedEventArgs e)
        {
            var self = (TextBox)sender;
            if (self.Text.Length > 4)
            {
                self.Text = self.Text.Substring(0, 4);
            }
        }

        private void Null_OnLostFocus(object sender, RoutedEventArgs e)
        {
            (sender as ListBox).SelectedItem = null;
        }
    }
}
