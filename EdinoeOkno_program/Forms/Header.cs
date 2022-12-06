using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EdinoeOkno_program.Forms
{
    internal class Header : IForms_Element
    {
        public static string css_class = "form-control";

        private StackPanel body = new StackPanel()
        {
            Background = Brushes.White,
            Margin = new Thickness(7),
        };

        private TextBlock description = new TextBlock()
        {
            Text = "Тип: Заголовок",
            Foreground = Brushes.Gray,
        };

        private TextBox title = new TextBox()
        {
            Text = "Заголовок",
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            TextWrapping = TextWrapping.Wrap,
            Background = new SolidColorBrush(Color.FromArgb(255, 232, 240, 254)),
        };

        public Header()
        {
            body.Children.Add(description);
            body.Children.Add(title);
        }

        public string GetPreviewHtml(int number)
        {
            return $"<div class=\"{css_class}\">\n\t<h1>{title.Text}</h1>\n\t</div>\n";
        }
        public StackPanel GetUIElement()
        {
            return body;
        }
        public void CreateDBElement(int id_form, NpgsqlConnection dBconnection)
        {

        }
    }
}
