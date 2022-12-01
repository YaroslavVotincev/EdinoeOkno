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

namespace EdinoeOkno_program
{
    /// <summary>
    /// Логика взаимодействия для Forms_UserControl.xaml
    /// </summary>
    public partial class Forms_UserControl : UserControl
    {

        private TextBlock answer = new TextBlock();
        private ComboBox select_type = new ComboBox()
        {
            Height = 30,
            HorizontalAlignment = HorizontalAlignment.Right,
            //VerticalAlignment = VerticalAlignment.Bottom
        };
        StackPanel stackElementQuestion = new StackPanel();
        TextBox radioText = new TextBox()
        {
            Width = 150,
            HorizontalAlignment = HorizontalAlignment.Left,
            AcceptsReturn = true,
            AcceptsTab = true,
            FontSize = 15,
            TextWrapping = TextWrapping.Wrap,
            Foreground = Brushes.Gray,
            Text = "Вариант",
            FontWeight = FontWeights.Bold,
            Margin = new Thickness(10)
        };

        Button AddAnswer = new Button()
        {
            Margin = new Thickness(20, 0, 0, 0),
            FontSize = 15,
            Content = "Добавить вариант"

        };

        public Forms_UserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddFormFunction();
            //MessageBox.Show("Кнопка нажата");
        }

        private void requestsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void AddFormFunction()
        {
            StackPanel MyStackPanel = new StackPanel();
            workingArea.Content = MyStackPanel;
            MyStackPanel.Margin = new Thickness(5);
            //MyStackPanel.Orientation = Orientation.Horizontal;

            //Блок с вопросом
            GroupBox question_block = new GroupBox();
            MyStackPanel.Children.Add(question_block);
            
            ////Для вертикального расположения элементов
            //StackPanel SP_vertical = new StackPanel();
            //question_block.Content = SP_vertical;
            //StackPanel SP_horizontal = new StackPanel() { Orientation = Orientation.Horizontal };

            //StackPanel stackElementQuestion = new StackPanel(); // Для вертикального добавления элементов внутки блока
            question_block.Content = stackElementQuestion;
            Grid question_grid = new Grid();
            stackElementQuestion.Children.Add(question_grid);

            ///////////////////////////////////////////////////////////////
            //Вопрос
            TextBox question = new TextBox()
            {
                Width = 200,
                HorizontalAlignment = HorizontalAlignment.Left,
                AcceptsReturn = true,
                AcceptsTab = true,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.Gray,
                Text = "Вопрос",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10)
            };
            question.GotKeyboardFocus += question_GotKeyboardFocus;
            question.LostKeyboardFocus += question_LostKeyboardFocus;
            question_grid.Children.Add(question);



            void question_GotKeyboardFocus (object sender, EventArgs e)
            {
                if (question.Text == "Вопрос")
                {
                    question.Clear();
                    question.Foreground = Brushes.Black;
                }
            }

            void question_LostKeyboardFocus(object sender, EventArgs e)
            {
                if (question.Text == "")
                {
                    question.Text = "Вопрос";
                    question.Foreground = Brushes.Gray;
                }
            }
            ///////////////////////////////////////////////////////////////

            //Комбобокс
            //ComboBox select_type = new ComboBox()
            //{
            //    Height = 30,
            //    HorizontalAlignment = HorizontalAlignment.Right,
            //    //VerticalAlignment = VerticalAlignment.Bottom
            //};
            select_type.Margin = new Thickness(10);
            question_grid.Children.Add (select_type);
            ComboBoxItem radio = new ComboBoxItem();
            radio.Content = "Один из списка";
            select_type.Items.Add(radio);
            ComboBoxItem checkbox = new ComboBoxItem();
            checkbox.Content = "Несколько из списка";
            select_type.Items.Add (checkbox);
            ComboBoxItem TextBox = new ComboBoxItem();
            TextBox.Content = "Текст";
            select_type.Items.Add(TextBox);
            select_type.SelectedIndex = 0;




            //TextBlock answer = new TextBlock();
            answer.Text = select_type.Text;
            answer.Margin = new Thickness(10);
            answer.FontSize = 15;
            answer.FontWeight = FontWeights.Bold;
            stackElementQuestion.Children.Add (answer);
            select_type.SelectionChanged += SelectedTypeFunc;

            ///////////////////////////////////////////////////////////////
            StackPanel radioHorizontal = new StackPanel() { Orientation = Orientation.Horizontal};
            stackElementQuestion.Children.Add(radioHorizontal);
            RadioButton radio1 = new RadioButton()
            {
                Margin = new Thickness(0, 3, 0, 0),
                IsEnabled = false
            };
            radioHorizontal.Children.Add(radio1);


            radioText.GotKeyboardFocus += radioText_GotKeyboardFocus;
            radioText.LostKeyboardFocus += radioText_LostKeyboardFocus;
            radioHorizontal.Children.Add(radioText);

            ///////////////////////////////////////////////////////////////

            stackElementQuestion.Children.Add(AddAnswer);
            AddAnswer.Click += AddAnswerClick;
        }

        private void AddAnswerClick(object sender, RoutedEventArgs e)
        {
            StackPanel radioHorizontal = new StackPanel() { Orientation = Orientation.Horizontal };


            stackElementQuestion.Children.RemoveAt(stackElementQuestion.Children.Count-1);
            stackElementQuestion.Children.Add(radioHorizontal);
            stackElementQuestion.Children.Add(AddAnswer);

            RadioButton radio1 = new RadioButton()
            {
                Margin = new Thickness(0, 3, 0, 0),
                IsEnabled = false
            };
            radioHorizontal.Children.Add(radio1);

            TextBox radioText = new TextBox()
            {
                Width = 150,
                HorizontalAlignment = HorizontalAlignment.Left,
                AcceptsReturn = true,
                AcceptsTab = true,
                FontSize = 15,
                TextWrapping = TextWrapping.Wrap,
                Foreground = Brushes.Gray,
                Text = "Вариант",
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(10)
            };
            radioText.GotKeyboardFocus += radioText_GotKeyboardFocus;
            radioText.LostKeyboardFocus += radioText_LostKeyboardFocus;
            radioHorizontal.Children.Add(radioText);
        }

        private void radioText_GotKeyboardFocus(object sender, EventArgs e)
        {
            TextBox r = (TextBox)sender;
            if (r.Text == "Вариант")
            {
                r.Clear();
                r.Foreground = Brushes.Black;
            }
        }

        private void radioText_LostKeyboardFocus(object sender, EventArgs e)
        {
            TextBox r = (TextBox)sender;
            if (r.Text == "")
            {
                r.Text = "Вариант";
                r.Foreground = Brushes.Gray;
            }
        }
        private void SelectedTypeFunc(object sender, SelectionChangedEventArgs e)
        {

            answer.Text = ((ComboBoxItem)(((ComboBox)sender).SelectedItem)).Content.ToString();
            if (select_type.SelectedIndex == 0)
            {
                
            }
            else if (select_type.SelectedIndex == 1)
            {

            }
            else if (select_type.SelectedIndex == 2)
            {

            }

        }


        //private void Question_MouseEnter(object sender, MouseEventArgs e)
        //{
        //    throw new NotImplementedException();
        //}
    }
}
