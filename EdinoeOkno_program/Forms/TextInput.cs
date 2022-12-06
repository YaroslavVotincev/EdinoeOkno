﻿using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace EdinoeOkno_program.Forms
{
    internal class TextInput : IForms_Element
    {
        public static string css_class = "form-control";
        //public static string label_class = "forms-question-title";
        //public static string input_class = "forms-textfield";

        private int maxInputLength = 20;

        private StackPanel body = new StackPanel()
        {
            Background = Brushes.White,
            Margin = new Thickness(7),
            
        };

        private StackPanel miscRow = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
        };

        private TextBlock description = new TextBlock()
        {
            Text = "Тип: Текстовый ввод   ",
            Foreground = Brushes.Gray,
        };

        private CheckBox requiredAnswer = new CheckBox()
        {
            Content = "Обязательный вопрос?"
        };

        private TextBox title = new TextBox()
        {
            Text = "Вопрос",
            FontWeight = FontWeights.Bold,
            HorizontalAlignment = HorizontalAlignment.Stretch,
            TextWrapping = TextWrapping.Wrap,
            Background = new SolidColorBrush(Color.FromArgb(255, 232, 240, 254)),
        };

        private TextBox userInput = new TextBox()
        {
            Text = "Ввод ответа",
            FontStyle = FontStyles.Italic,
            IsReadOnly = true,
            HorizontalAlignment = HorizontalAlignment.Left,
            Background = new SolidColorBrush(Color.FromArgb(255, 232, 240, 254)),
        };

        private StackPanel maxInputLengthRow = new StackPanel()
        { 
            Orientation= Orientation.Horizontal,
        };

        private TextBlock maxInputLengthTextBlock = new TextBlock()
        {
            Text = "Максимальная длина ввода:",
        };

        private TextBox maxInputLengthBox = new TextBox()
        {
            Text = "20",
            HorizontalAlignment = HorizontalAlignment.Stretch,
            Background = new SolidColorBrush(Color.FromArgb(255, 232, 240, 254)),
        };


        public TextInput()
        {
            miscRow.Children.Add(description);
            miscRow.Children.Add(requiredAnswer);
            maxInputLengthRow.Children.Add(maxInputLengthTextBlock);
            maxInputLengthRow.Children.Add(maxInputLengthBox);
            maxInputLengthBox.TextChanged += maxInputLengthBox_TextChanged;

            body.Children.Add(miscRow);
            body.Children.Add(title);
            body.Children.Add(userInput);
            body.Children.Add(maxInputLengthRow);
        }

        private void maxInputLengthBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (int.TryParse(maxInputLengthBox.Text, out _))
            {
                maxInputLength = int.Parse(maxInputLengthBox.Text);
            }
        }

        public string GetPreviewHtml(int number)
        {
            string isRequired = requiredAnswer.IsChecked == true ? "required" : "";
            return $"<div class=\"{css_class}\">\n" +
                $"\t<input class =\"none\" value=\"'id_question', 'id_answer', \" name=\"{number}_textinput[]\">\n" +
                $"\t<label>вопрос</label>\n" +
                $"\t<input maxlength=\"{maxInputLength}\" name=\"{number}_textinput[]\" type=\"text\" {isRequired}>\n" +
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
