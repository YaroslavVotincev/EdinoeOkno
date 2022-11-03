using System;
using System.Collections.Generic;
using System.Data.Common;
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
using Npgsql;

namespace EdinoeOkno_program
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        const string CONNECTION_STRING = "Server=26.137.232.44;Port=5432;User id=Stas;Password=1;Database=EdinoeOkno";
        List<Request> requestsList = new List<Request>();
        NpgsqlConnection dBconnection;
        Request selectedRequest;
        public MainWindow()
        {
            InitializeComponent();
            ConnectDB();
            GetRequestList();
            FillRequestsListBox();
            
        }

        private void ConnectDB()
        {
            dBconnection = new NpgsqlConnection(CONNECTION_STRING);
            dBconnection.Open();
        }

        private void GetRequestList()
        {
            using (NpgsqlCommand cmd =
                new NpgsqlCommand($@"SELECT * FROM dev.req_new_back", dBconnection))
            {
                using (NpgsqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        foreach (DbDataRecord dB in reader)
                        {
                            //var temp = (int)dB["request_id"];
                            //var temp1 = (string)dB["request_name"];
                            //var temp2 = (string)dB["status_name"];
                            //var temp3 = (string)dB["first_name"];
                            //var temp4 = (string)dB["last_name"];
                            //var temp5 = (string)dB["patronymic"];
                            //var temp6 = (string)dB["email"];
                            //var temp7 = (string)dB["faculty_name"];
                            //var temp8 = (string)dB["short_name"];
                            //var temp9 = (string)dB["student_group"];
                            //var temp10 = (string)dB["time_when_added"];
                            //var temp11 = (string)dB["dir_path"];
                            //var temp12 = (int)dB["files_attached"];
                            requestsList.Add(
                                new Request(
                                (int)   dB["request_id"],
                                (string)dB["request_name"],
                                (string)dB["status_name"],
                                (string)dB["first_name"],
                                (string)dB["last_name"],
                                (string)dB["patronymic"],
                                (string)dB["email"],
                                (string)dB["faculty_name"],
                                (string)dB["short_name"],
                                (string)dB["student_group"],
                                (string)dB["time_when_added"],
                                (string)dB["dir_path"],
                                (int)   dB["files_attached"]));


                        }
                    }
                }
            }
        }

        private void FillRequestsListBox()
        {
            requestsListBox.Items.Clear();
            string preview;
            Request r;
            for (int i = 0; i < requestsList.Count; i++)
            {
                r = requestsList[i];
                r.button = new Button();
                preview = $"Заявка №{r.request_id}\n{r.request_name}\n";
                r.button.Tag = r;
                r.button.HorizontalContentAlignment = HorizontalAlignment.Left;
                r.button.FontSize = 10;
                r.button.Width = requestsListBox.Width - 35;
                r.button.Content = preview;
                r.button.Click += SelectRequest;
                requestsListBox.Items.Add(r.button);               
            }
        }

        private void SelectRequest(object sender, EventArgs eventArgs)
        {
            Button selectedButton = (Button)sender;
            selectedRequest = (Request)selectedButton.Tag;
            ConstructWorkingArea();

            //for (int i = 0; i < requestsList.Count; i++)
            //{
            //    if (selectedButton == requestsList[i].button)
            //    {
            //        selectedRequest = requestsList[i];
            //        ConstructWorkingArea();
            //        break;
            //    }
            //}
        }
        private void buttonSend_Click(object sender, EventArgs eventArgs)
        {
            Button button = (Button)sender;
            TextBox text = (TextBox)button.Tag;
            MailSend.Send(selectedRequest.email, selectedRequest.request_name, text.Text);
        }
        private void ConstructWorkingArea()
        {     
            StackPanel st = new StackPanel();
            workingArea.Content = st;
            Label selected = new Label();
            selected.Content = $"Заявка №{selectedRequest.request_id}\n{selectedRequest.request_name}\n\nИмя: {selectedRequest.first_name}\n" +
                $"Фамилия: {selectedRequest.last_name}\nОтчество: {selectedRequest.patronymic}\nemail: {selectedRequest.email}\n" +
                $"Факультет: {selectedRequest.faculty_name}\nГруппа: {selectedRequest.group}\n";
            st.Children.Add(selected);
            TextBox responseArea = new TextBox();
            responseArea.Width = workingArea.Width;
            responseArea.Height = 50;
            Button buttonSend = new Button();
            buttonSend.Height = 20;
            buttonSend.Content = "Отправить";
            buttonSend.Tag = responseArea;
            buttonSend.Click += buttonSend_Click;
            st.Children.Add(responseArea);
            st.Children.Add(buttonSend);
        }

    }
}
