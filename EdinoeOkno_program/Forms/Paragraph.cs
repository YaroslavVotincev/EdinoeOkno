using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace EdinoeOkno_program.Forms
{
    internal class Paragraph : IForms_Element
    {
        public static string css_class = "form-control";

        private StackPanel body = new StackPanel()
        {
            Background = Brushes.White,
            Margin = new Thickness(7),
        };

        private TextBlock description = new TextBlock()
        {
            Text = "Тип: Параграф",
            Foreground = Brushes.Gray,
        };

        private TextBox paragraph_text = new TextBox()
        {
            Text = "Параграф",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            TextWrapping = TextWrapping.Wrap,
            Background = new SolidColorBrush(Color.FromArgb(255, 232, 240, 254)),
        };

        public Paragraph()
        {
            body.Children.Add(description);
            body.Children.Add(paragraph_text);
        }

        public string GetPreviewHtml(int number)
        {
            return $"<div class=\"{css_class}\">\n" +
                $"\t<label>{paragraph_text.Text}\n</label>" +
                $"</div>\n";
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
