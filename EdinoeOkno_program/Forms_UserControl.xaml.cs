using EdinoeOkno_program.Forms;
using Npgsql;
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
        NpgsqlConnection dBconnection = OurDatabase.GetConnection();

        List<IForms_Element> _elements = new List<IForms_Element>();

        public Forms_UserControl()
        {
            InitializeComponent();
            previewButton.Click += previewButton_Click;

            StackPanel st = new StackPanel();
            formsWorkingArea.Content = st;

            Forms.Header h = new Forms.Header();
            Forms.Paragraph p = new Forms.Paragraph();
            Forms.TextInput ti = new Forms.TextInput();
            Forms.CheckboxGroup ch = new Forms.CheckboxGroup();
            Forms.CheckboxGroupWithTextInput ch2 = new CheckboxGroupWithTextInput();
            Forms.RadioGroup rg = new RadioGroup();
            Forms.RadioGroupWithTextInput rg2 = new RadioGroupWithTextInput();

            //st.Children.Add(h.GetUIElement());
            //st.Children.Add(p.GetUIElement());
            st.Children.Add(ti.GetUIElement());
            st.Children.Add(ch.GetUIElement());
            st.Children.Add(ch2.GetUIElement());
            st.Children.Add(rg.GetUIElement());
            st.Children.Add(rg2.GetUIElement());

            _elements.Add(h);
            _elements.Add(p);
            _elements.Add(ti);
            _elements.Add(ch);
            _elements.Add(ch2);
            _elements.Add(rg);
            _elements.Add(rg2);
        }

        private void previewButton_Click(object sender, RoutedEventArgs e)
        {
            int i = 1;
            tempOutput.Text = "";
            foreach(var element in _elements)
            {
                tempOutput.Text += element.GetPreviewHtml(i);
                //tempOutput.Text += "\n";
                i++;
            }
        }
    }
}
