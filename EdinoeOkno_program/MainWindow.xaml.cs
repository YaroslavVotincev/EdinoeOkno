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
            updateListBoxButton.Click += updateListBoxButton_Click;
            DefaultWorkingArea();
            ConnectDB();
            GetRequestList(new_requestsList, 0);
            //GetRequestList(done_requestsList, 1);
            //GetRequestList(decl_requestsList, 2);
            FillRequestsListBox(new_requestsList);
        }

        /// <summary>
        /// Обновляет requestListBox с помощью повторного запроса к БД
        /// </summary>
        private void updateListBoxButton_Click(object sender, EventArgs e)
        {
            DefaultListBox();
            DefaultWorkingArea();
            if(statusComboBox.SelectedIndex == 0)
            {
                GetRequestList(new_requestsList, 0);
                FillRequestsListBox(new_requestsList);
            }
            else if (statusComboBox.SelectedIndex == 1)
            {
                GetRequestList(done_requestsList, 1);
                FillRequestsListBox(done_requestsList);
            }
            else
            {
                GetRequestList(decl_requestsList, 2);
                FillRequestsListBox(decl_requestsList);
            }

        }
        /// <summary>
        /// Меняет текущий requestListBox на лист с заявками с другим статусом
        /// </summary>
        private void statusComboBox_SelectionChanged(object sender, EventArgs e)
        {
            DefaultListBox();
            DefaultWorkingArea();
            if (statusComboBox.SelectedIndex == 0)
            {
                GetRequestList(new_requestsList, 0);
                FillRequestsListBox(new_requestsList);
            }
            else if (statusComboBox.SelectedIndex == 1)
            {
                GetRequestList(done_requestsList, 1);
                FillRequestsListBox(done_requestsList);
            }
            else
            {
                GetRequestList(decl_requestsList, 2);
                FillRequestsListBox(decl_requestsList);
            }
        }
        /// <summary>
        /// Подключается к БД
        /// </summary>
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
        /// <summary>
        /// Очищает requestsListBox
        /// </summary>
        private void DefaultListBox()
        {
            requestsListBox.Items.Clear();
            TextBlock loadingMessage = new TextBlock
            {
                Text = "Происходит загрузка, пожалуйста подождите...",
                TextWrapping = TextWrapping.Wrap,
                Width = requestsListBox.Width,
                FontWeight = FontWeights.Bold,
                FontSize = 16
            };
            requestsListBox.Items.Add(loadingMessage);
        }
        /// <summary>
        /// Очищает workingArea
        /// </summary>
        private void DefaultWorkingArea()
        {
            workingArea.Content = null;
            StackPanel temp  = new StackPanel();
            workingArea.Content = temp;
            TextBlock defaultWorkingAreaMessage = new TextBlock
            {
                Text = "Выберите элемент из списка...",
                TextWrapping = TextWrapping.Wrap,
                Width = requestsListBox.Width,
                FontSize = 12
            };
            temp.Children.Add(defaultWorkingAreaMessage);
        }
        /// <summary>
        /// Заполняет заданный requestsList со статусом status_code с помощью запроса к БД
        /// </summary>
        private void GetRequestList(List<Request> requestsList, int status_code)
        {
            if(dBconnection.State == System.Data.ConnectionState.Closed)
            {
                return;
            }
            try
            {
               requestsList.Clear();
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
                                if (status_code == 0)
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
                                else
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
                                        (int)dB["files_attached"],
                                        (string)dB["time_when_update"],
                                        (string)dB["email_response"]));
                                requestsList.Last<Request>().status_code = status_code;
                            }
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                requestsListBox.Items.Clear();
                TextBlock queryErrorLoadingMessage = new TextBlock
                {
                    TextWrapping = TextWrapping.Wrap,
                    Width = requestsListBox.Width,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16
                };
                requestsListBox.Items.Add(queryErrorLoadingMessage);
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// Заполняет requestListBox исходя из переданного requestsList
        /// </summary>
        private void FillRequestsListBox(List<Request> requestsList)
        {
            requestsListBox.Items.Clear();
            if (requestsList.Count == 0)
            {
                TextBlock noRequestsMessage = new TextBlock
                {
                    Text = "Заявления не найдены...",
                    TextWrapping = TextWrapping.Wrap,
                    Width = requestsListBox.Width,
                    FontWeight = FontWeights.Bold,
                    FontSize = 16
                };
                requestsListBox.Items.Add(noRequestsMessage);
                return;
            }
            Request r;
            for (int i = 0; i < requestsList.Count; i++)
            {
                r = requestsList[i];
                r.button = new Button();
                TextBlock preview = new TextBlock
                {
                    Text = $"Заявка №{r.request_id}\n" +
                    $"Состояние: {r.status_name}\n" +
                    $"Время поступления: {r.time_when_requested}\n" +
                    $"Факультет: {r.faculty_name_short}\n" +
                    $"Тип: {r.request_name}",
                    TextWrapping = TextWrapping.Wrap
                };
                r.button.Tag = r;
                r.button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                r.button.FontSize = 11;
                r.button.Width = requestsListBox.Width - 35;
                r.button.Content = preview;
                r.button.Click += SelectRequest;
                requestsListBox.Items.Add(r.button);               
            }
        }
        /// <summary>
        /// По нажатию кнопки из requestListBox выбирается соответствующий selectedRequest
        /// </summary>
        private void SelectRequest(object sender, EventArgs eventArgs)
        {
            Button selectedButton = sender as Button;
            selectedRequest = selectedButton.Tag as Request;
            ConstructWorkingArea();
        }
        private void buttonSend_Click(object sender, EventArgs eventArgs)
        {
            Button button = sender as Button;
            TextBox text = button.Tag as TextBox;
            //MailSend.Send(selectedRequest.email, selectedRequest.request_name, text.Text);
        }
        /// <summary>
        /// По нажатию кнопки в буфер обмена копируется Tag этой кнопки
        /// </summary>
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
            StackPanel st = new StackPanel();
            workingArea.Content = st;

            //Заголовок
            TextBox header = SetReadTextBox();
            header.Text = $" Заявка №{selectedRequest.request_id}" +
                $" ({selectedRequest.time_when_requested})";
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
            StackPanel first_nameRow = new StackPanel() { Orientation = Orientation.Horizontal };
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
            StackPanel last_nameRow = new StackPanel() { Orientation = Orientation.Horizontal };
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
            StackPanel patronymicRow = new StackPanel() { Orientation = Orientation.Horizontal };
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
            StackPanel facultyRow = new StackPanel() { Orientation = Orientation.Horizontal };
            Label facultyLabel = new Label() { Content = "Факультет: " };
            TextBox facultyBox = SetReadTextBox();
            facultyBox.Text = $"({selectedRequest.faculty_name_short}) {selectedRequest.faculty_name}";
            facultyBox.VerticalContentAlignment = VerticalAlignment.Center;
            facultyBox.Width = 300;
            facultyBox.TextWrapping = TextWrapping.Wrap;
            Button facultyCopyButton = SetCopyButton(facultyBox.Text);
            facultyRow.Children.Add(facultyLabel);
            facultyRow.Children.Add(facultyBox);
            facultyRow.Children.Add(facultyCopyButton);
            st.Children.Add(facultyRow);
            //группа
            StackPanel groupRow = new StackPanel() { Orientation = Orientation.Horizontal };
            Label groupLabel = new Label() { Content = "Группа: " };
            TextBox groupBox = SetReadTextBox();
            groupBox.Text = selectedRequest.group.ToUpper();
            groupBox.VerticalContentAlignment = VerticalAlignment.Center;
            Button groupCopyButton = SetCopyButton(groupBox.Text);
            groupRow.Children.Add(groupLabel);
            groupRow.Children.Add(groupBox);
            groupRow.Children.Add(groupCopyButton);
            st.Children.Add(groupRow);
            //email
            StackPanel emailRow = new StackPanel() { Orientation = Orientation.Horizontal };
            Label emailLabel = new Label() { Content = "Эл. почта: " };
            TextBox emailBox = SetReadTextBox();
            emailBox.Text = selectedRequest.email;
            emailBox.VerticalContentAlignment = VerticalAlignment.Center;
            Button emailCopyButton = SetCopyButton(emailBox.Text);
            emailRow.Children.Add(emailLabel);
            emailRow.Children.Add(emailBox);
            emailRow.Children.Add(emailCopyButton);
            st.Children.Add(emailRow);
            //кол-во файлов
            TextBlock filesInfo = new TextBlock();
            filesInfo.Text = $"\nПрикреплённые документы:" +
                $"\nКоличество файлов: {selectedRequest.files_attached}";
            st.Children.Add(filesInfo);
            //файлы
            StackPanel linkRow = new StackPanel() { Orientation = Orientation.Horizontal };
            Label linkLabel = new Label() { Content = "Ссылка на файлы: " };
            TextBox linkBox = SetReadTextBox();
            linkBox.Text = selectedRequest.dir_path;
            linkBox.VerticalContentAlignment = VerticalAlignment.Center;
            Button linkCopyButton = SetCopyButton(linkBox.Text);
            linkCopyButton.Tag = linkBox.Text;
            linkCopyButton.Click += linkBox_Click;
            linkCopyButton.Content = "open";
            linkRow.Children.Add(linkLabel);
            linkRow.Children.Add(linkBox);
            linkRow.Children.Add(linkCopyButton);
            st.Children.Add(linkRow);
            //ответ на почту
            TextBlock responseHeader = new TextBlock();
            responseHeader.Text = $"Ответ на почту {selectedRequest.email}:";
            st.Children.Add(responseHeader);
            //ввод ответа
            TextBox responseBox = new TextBox()
            {
                Text = selectedRequest.response,
                Width = 350,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 100,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                AcceptsTab = true,
                IsReadOnly = !(selectedRequest.status_code == 0)
            };
            st.Children.Add(responseBox);
            //подтверждение отправки ответа
            if (selectedRequest.status_code == 0)
            {
                CheckBox confirm_responseCheckBox = new CheckBox()
                {
                    Content = "Без ответа на почту",
                    Tag = responseBox
                };
                confirm_responseCheckBox.Click += confirm_responseCheckBox_Click;
                st.Children.Add(confirm_responseCheckBox);

                StackPanel changeStatusButtonsRow = new StackPanel() { Orientation = Orientation.Horizontal };
                Button doneButton = new Button()
                {
                    Content = "Подтвердить выполнение",
                    Background = Brushes.Green,
                    Tag = responseBox
                };
                Button declineButton = new Button()
                {
                    Content = "Отклонить заявку",
                    Background = Brushes.Red,
                    Tag = responseBox
                };
                doneButton.Click += doneButton_Click;
                declineButton.Click += declineButton_Click;
                changeStatusButtonsRow.Children.Add(doneButton);
                changeStatusButtonsRow.Children.Add(declineButton);
                st.Children.Add(changeStatusButtonsRow);
            } 
        }

        private void declineButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            TextBox textBox = button.Tag as TextBox;
            DefaultWorkingArea();
            new_requestsList.Remove(selectedRequest);
            FillRequestsListBox(new_requestsList);
            selectedRequest.UpdateRequest(2, textBox.Text, dBconnection, dBSchema);            
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            TextBox textBox = button.Tag as TextBox;
            DefaultWorkingArea();
            new_requestsList.Remove(selectedRequest);
            FillRequestsListBox(new_requestsList);
            selectedRequest.UpdateRequest(1, textBox.Text, dBconnection, dBSchema);

        }

        private void FreezeScreen()
        {

        }

        private void confirm_responseCheckBox_Click(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            TextBox textBox = checkBox.Tag as TextBox;
            textBox.IsEnabled = !textBox.IsEnabled;
        }

        private void linkBox_Click(object sender, EventArgs e)
        {
            Button textBox = sender as Button;
            var sInfo = new System.Diagnostics.ProcessStartInfo((string)(textBox.Tag)) { UseShellExecute = true };
            System.Diagnostics.Process.Start(sInfo);
        }
    }
}
