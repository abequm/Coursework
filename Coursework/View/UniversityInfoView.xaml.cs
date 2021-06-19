using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// Логика взаимодействия для UniversityInfoView.xaml
    /// </summary>
    public partial class UniversityInfoView : UserControl
    {
        public UniversityInfoView()
        {
            InitializeComponent();
        }

        private void AvgScoreTextChanged(object sender, TextChangedEventArgs e)
        {
            var regexes = new Regex[] { new Regex("[12345]"), new Regex("[12345][.,]"), new Regex("[12345][.,][1234567890]") };
            var text = (sender as TextBox).Text;
            if (text.Length == 0) return;
            if (text.Length > 3) goto outindex;
            if (regexes[text.Length - 1].IsMatch(text))
            {
                return;
            }
            outindex: ((TextBox)sender).Text = text.Substring(0, text.Length - 1);
        }
        private void PreviewDigits(object sender, TextCompositionEventArgs e)
        {
            var regex = new Regex("[0-9]");
            if (!regex.IsMatch(e.Text[0].ToString()))
            {
                e.Handled = true;
            }
        }
    }
}
