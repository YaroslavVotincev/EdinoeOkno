using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
        const string dBServer = "localhost";
        const string dBPort = "5432";
        const string dBUser = "postgres";
        const string dBPassword = "";
        const string dBDatabase = "EdinoeOkno";
        const string dBSchema = "dev";
        string CONNECTION_STRING = $"Server={dBServer};Port={dBPort};User id={dBUser};Password={dBPassword};Database={dBDatabase}";
        List<Request> new_requestsList = new List<Request>();
        List<Request> done_requestsList = new List<Request>();
        List<Request> decl_requestsList = new List<Request>();
        NpgsqlConnection dBconnection;
        Request selectedRequest;
        MailSend responseMail = new MailSend();
        string[] views = { "req_new_back", "req_done_back", "req_decl_back" };

        public MainWindow()
        {
            InitializeComponent();
            DefaultListBox();
            statusComboBox.SelectionChanged += statusComboBox_SelectionChanged;
            DefaultWorkingArea();
            ConnectDB();
            GetRequestList(new_requestsList, 0);
            GetRequestList(done_requestsList, 1);
            GetRequestList(decl_requestsList, 2);
            FillRequestsListBox(new_requestsList);
        }

        private void statusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (statusComboBox.SelectedIndex == 0)
            {
                FillRequestsListBox(new_requestsList);
            }
            else if (statusComboBox.SelectedIndex == 1)
            {
                FillRequestsListBox(done_requestsList);
            }
            else FillRequestsListBox(decl_requestsList);
            DefaultWorkingArea();
        }
        private void ConnectDB()
        {
            dBconnection = new NpgsqlConnection(CONNECTION_STRING);
            try
            {
                dBconnection.Open();
                statusComboBox.IsEnabled = true;
            }
            catch(Exception ex)
            {
                requestsListBox.Items.Clear();
                TextBlock dBErrorLoadingMessage = new TextBlock();
                dBErrorLoadingMessage.Text = $"Произошла ошибка при подключении к базе данных:\n{ex.Message}";
                dBErrorLoadingMessage.TextWrapping = TextWrapping.Wrap;
                dBErrorLoadingMessage.Width = requestsListBox.Width;
                dBErrorLoadingMessage.FontWeight = FontWeights.Bold;
                dBErrorLoadingMessage.FontSize = 16;
                requestsListBox.Items.Add(dBErrorLoadingMessage);
                statusComboBox.IsEnabled = false;
                //MessageBox.Show(ex.Message);
            }
        }
        private void DefaultListBox()
        {
            requestsListBox.Items.Clear();
            TextBlock loadingMessage = new TextBlock();
            loadingMessage.Text = "Происходит загрузка, пожалуйста подождите...";
            loadingMessage.TextWrapping = TextWrapping.Wrap;
            loadingMessage.Width = requestsListBox.Width;
            loadingMessage.FontWeight = FontWeights.Bold;
            loadingMessage.FontSize = 16;
            requestsListBox.Items.Add(loadingMessage);
        }
        private void DefaultWorkingArea()
        {
            workingArea.Content = null;
            StackPanel temp  = new StackPanel();
            workingArea.Content = temp;
            TextBlock defaultWorkingAreaMessage = new TextBlock();
            defaultWorkingAreaMessage.Text = "Выберите элемент из списка...";
            defaultWorkingAreaMessage.TextWrapping = TextWrapping.Wrap;
            defaultWorkingAreaMessage.Width = requestsListBox.Width;
            defaultWorkingAreaMessage.FontSize = 12;
            temp.Children.Add(defaultWorkingAreaMessage);
        }
        private void GetRequestList(List<Request> requestsList, int status_code)
        {
            if(dBconnection.State == System.Data.ConnectionState.Closed)
            {
                return;
            }
            try
            {
               using (NpgsqlCommand cmd =
               new NpgsqlCommand($@"SELECT * FROM {dBSchema}.{views[status_code]}", dBconnection))
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
                                    (int)dB["request_id"],
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
                                    (int)dB["files_attached"]));
                                new_requestsList.Last<Request>().status_code = status_code;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                requestsListBox.Items.Clear();
                TextBlock queryErrorLoadingMessage = new TextBlock();
                queryErrorLoadingMessage.TextWrapping = TextWrapping.Wrap;
                queryErrorLoadingMessage.Width = requestsListBox.Width;
                queryErrorLoadingMessage.FontWeight = FontWeights.Bold;
                queryErrorLoadingMessage.FontSize = 16;
                requestsListBox.Items.Add(queryErrorLoadingMessage);
                MessageBox.Show(ex.Message);
            }
        }

        private void FillRequestsListBox(List<Request> requestsList)
        {
            requestsListBox.Items.Clear();
            if (requestsList.Count == 0)
            {               
                TextBlock noRequestsMessage = new TextBlock();
                noRequestsMessage.Text = "Заявления не найдены...";
                noRequestsMessage.TextWrapping = TextWrapping.Wrap;
                noRequestsMessage.Width = requestsListBox.Width;
                noRequestsMessage.FontWeight = FontWeights.Bold;
                noRequestsMessage.FontSize = 16;
                requestsListBox.Items.Add(noRequestsMessage);
                return;
            }
            Request r;
            for (int i = 0; i < requestsList.Count; i++)
            {
                r = requestsList[i];
                r.button = new Button();
                TextBlock preview = new TextBlock();
                preview.Text = $"Заявка №{r.request_id}\n" +
                    $"Состояние: {r.status_name}\n" +
                    $"Время поступления: {r.time_when_requested}\n" +
                    $"Факультет: {r.faculty_name_short}\n" +
                    $"Тип: {r.request_name}";
                preview.TextWrapping = TextWrapping.Wrap;
                r.button.Tag = r;
                r.button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                r.button.FontSize = 11;
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
        }
        private void buttonSend_Click(object sender, EventArgs eventArgs)
        {
            Button button = (Button)sender;
            TextBox text = (TextBox)button.Tag;
            //MailSend.Send(selectedRequest.email, selectedRequest.request_name, text.Text);
        }

        private void copyButton_Click(object sender, EventArgs eventArgs)
        {
            Clipboard.SetText(((Button)sender).Tag.ToString());
        }
        /// <summary>
        /// Возвращает новый TextBox только для чтения
        /// </summary>
        public TextBox SetReadTextBox()
        {
            return new TextBox()
            {
                Background = workingArea.Background,
                BorderBrush = null,
                IsReadOnly = true
            };
        }
        /// <summary>
        /// Возвращает новый Button для копирования текста
        /// </summary>
        public Button SetCopyButton(string copystr)
        {
            Button button = new Button()
            {
                Width = 24,
                Height = 18,
                Content = "copy",
                FontSize = 8,
                Tag = copystr,
            };
            button.Click += copyButton_Click;
            return button;
        }
        /// <summary>
        /// Строит workingArea исходя из текущего selectedRequest
        /// </summary>
        private void ConstructWorkingArea()
        {
            workingArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            StackPanel st = new StackPanel();
            workingArea.Content = st;
            StackPanel dataRow = new StackPanel();
            dataRow.Orientation = Orientation.Horizontal;
            //Заголовок
            TextBox header = SetReadTextBox();
            header.Text = $" Заявка №{selectedRequest.request_id} ({selectedRequest.time_when_requested})";
            header.FontSize = 15;
            header.FontWeight = FontWeights.Bold;
            st.Children.Add(header);
            //общая информация
            TextBox genInfo = SetReadTextBox();
            genInfo.Text = $"Состояние: {selectedRequest.status_name}\n" +
                        $"Время поступления: {selectedRequest.time_when_requested}\n" +
                        $"Время обновления:  {selectedRequest.time_when_updated}\n" +
                        $"Обработано сотрудником: -\n" +
                        $"Тип: {selectedRequest.request_name}";
            st.Children.Add(genInfo);
            //информация о заявившем
            TextBlock personInfo = new TextBlock();
            personInfo.Text = "\nИнформация о заявшившем:";
            st.Children.Add(personInfo);
            //имя
            StackPanel first_nameRow = new StackPanel();
            first_nameRow.Orientation = Orientation.Horizontal;
            Label first_nameLabel = new Label() { Content = "Имя:" };
            TextBox first_nameBox = SetReadTextBox();
            first_nameBox.Text = selectedRequest.first_name;
            first_nameBox.VerticalContentAlignment = VerticalAlignment.Center;
            Button first_nameCopyButton = SetCopyButton(first_nameBox.Text);
            first_nameRow.Children.Add(first_nameLabel);
            first_nameRow.Children.Add(first_nameBox);
            first_nameRow.Children.Add(first_nameCopyButton);
            st.Children.Add(first_nameRow);
            //фамилия
            StackPanel last_nameRow = new StackPanel();
            last_nameRow.Orientation = Orientation.Horizontal;
            Label last_nameLabel = new Label() { Content = "Фам:" };
            TextBox last_nameBox = SetReadTextBox();
            last_nameBox.Text = selectedRequest.last_name;
            last_nameBox.VerticalContentAlignment = VerticalAlignment.Center;
            Button last_nameCopyButton = SetCopyButton(last_nameBox.Text);
            last_nameRow.Children.Add(last_nameLabel);
            last_nameRow.Children.Add(last_nameBox);
            last_nameRow.Children.Add(last_nameCopyButton);
            st.Children.Add(last_nameRow);
            //отчество
            StackPanel patronymicRow = new StackPanel();
            patronymicRow.Orientation = Orientation.Horizontal;
            Label patronymicLabel = new Label() { Content = "Отч:" };
            TextBox patronymicBox = SetReadTextBox();
            patronymicBox.Text = selectedRequest.patronymic;
            patronymicBox.VerticalContentAlignment = VerticalAlignment.Center;
            Button patronymicCopyButton = SetCopyButton(patronymicBox.Text);
            patronymicRow.Children.Add(patronymicLabel);
            patronymicRow.Children.Add(patronymicBox);
            patronymicRow.Children.Add(patronymicCopyButton);
            st.Children.Add(patronymicRow);
            //Факультет

            //группа
            //email
            //кол-во файлов
            //файлы
            //перевод состояния
            //ввод ответа
            //подтверждение отправки ответа


            //Label selected = new Label();
            //selected.Content = $"Заявка №{selectedRequest.request_id}\n{selectedRequest.request_name}\n\nИмя: {selectedRequest.first_name}\n" +
            //    $"Фамилия: {selectedRequest.last_name}\nОтчество: {selectedRequest.patronymic}\nemail: {selectedRequest.email}\n" +
            //    $"Факультет: {selectedRequest.faculty_name}\nГруппа: {selectedRequest.group}\n";
            //st.Children.Add(selected);
            //TextBox responseArea = new TextBox();
            //responseArea.Width = workingArea.Width;
            //responseArea.AcceptsReturn = true;
            //responseArea.HorizontalScrollBarVisibility = ScrollBarVisibility.Auto;
            //responseArea.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //responseArea.Height = 50;
            //Button buttonSend = new Button();
            //buttonSend.Height = 20;
            //buttonSend.Content = "Отправить";
            //buttonSend.Tag = responseArea;
            //buttonSend.Click += buttonSend_Click;
            //st.Children.Add(responseArea);
            //st.Children.Add(buttonSend);
        }
    }
}
