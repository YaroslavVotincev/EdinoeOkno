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
    /// Логика взаимодействия для Requests_UserConrol.xaml
    /// </summary>
    public partial class Requests_UserConrol : UserControl
    {
        /*
        const string dBServer = "localhost";
        const string dBPort = "5432";
        const string dBUser = "postgres";
        const string dBPassword = "";
        const string dBDatabase = "EdinoeOkno";
        const string dBSchema = "dev1";
        string CONNECTION_STRING = $"Server={dBServer};Port={dBPort};User id={dBUser};Password={dBPassword};Database={dBDatabase}";
        */
        List<Request> currentRequestList = new List<Request>();
        NpgsqlConnection dBconnection = OurDatabase.GetConnection();
        Request selectedRequest;
        MailSend responseMail = new MailSend();
        string view = "req_back";

        List<string> type_filter = new List<string>();
        List<string> status_filter = new List<string>();
        List<string> faculty_filter = new List<string>();
        string last_name_search;

        public Requests_UserConrol()
        {
            InitializeComponent();

            GetNamesOfRequests();
            GetNamesOfStatus();
            GetNamesOfFaculty();
            last_nameSearchTextBox.TextChanged += last_nameSearchTextBox_TextChanged;
            updateListBoxButton.Click += updateListBoxButton_Click;

            DefaultListBox();
            DefaultWorkingArea();
            GetRequestList(currentRequestList);
            FillRequestsListBox(currentRequestList);
        }

        private void updateListBoxButton_Click(object sender, RoutedEventArgs e)
        {
            DefaultListBox();
            DefaultWorkingArea();
            GetRequestList(currentRequestList);
            FillRequestsListBox(currentRequestList);
        }

        private void last_nameSearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            last_name_search = last_nameSearchTextBox.Text;
        }

        private void GetNamesOfRequests()
        {
            filter_requestComboBox.SelectionChanged += filter_ComboBox_SelectionChanged;
            foreach(string[] req_name in OurDatabase.requestNamesList)
            {
                if (Authorization.account.request_privileges.Contains(req_name[0]))
                {
                    ComboBoxItem item = new ComboBoxItem()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Stretch
                    };
                    CheckBox checkBox = new CheckBox()
                    {
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Stretch,
                        HorizontalContentAlignment = HorizontalAlignment.Stretch,
                        VerticalContentAlignment = VerticalAlignment.Stretch,
                        IsChecked = true,
                        Tag = req_name[0],
                        Content = req_name[1]
                    };
                    checkBox.Checked += filter_requestCheckBox_Checked;
                    checkBox.Unchecked += filter_requestCheckBox_Unchecked;
                    item.Content = checkBox;
                    filter_requestComboBox.Items.Add(item);

                    type_filter.Add(req_name[0]);
                }   
            }
        }

        private void filter_requestCheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            type_filter.Add(checkBox.Tag as string);
        }

        private void filter_requestCheckBox_Unchecked(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            type_filter.Remove(checkBox.Tag as string);
        }

        private void filter_ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            comboBox.SelectedIndex = 0;
        }


        private void GetNamesOfStatus()
        {
            filter_statusComboBox.SelectionChanged += filter_ComboBox_SelectionChanged;
            foreach (string[] status_name in OurDatabase.statusNamesList)
            {
                ComboBoxItem item = new ComboBoxItem()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };
                CheckBox checkBox = new CheckBox()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                    IsChecked = true,
                    Tag = status_name[0],
                    Content = status_name[2]
                };
                checkBox.Checked += filter_statusCheckBox_Checked;
                checkBox.Unchecked += filter_statusCheckBox_Unchecked;
                item.Content = checkBox;
                filter_statusComboBox.Items.Add(item);

                status_filter.Add(status_name[0]);
            }
        }

        private void filter_statusCheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            status_filter.Add(checkBox.Tag as string);
        }

        private void filter_statusCheckBox_Unchecked(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            status_filter.Remove(checkBox.Tag as string);
        }

        private void GetNamesOfFaculty()
        {
            filter_facultyComboBox.SelectionChanged += filter_ComboBox_SelectionChanged;
            foreach (string[] fac_name in OurDatabase.facultyNamesList)
            {
                ComboBoxItem item = new ComboBoxItem()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch
                };
                CheckBox checkBox = new CheckBox()
                {
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
                    HorizontalContentAlignment = HorizontalAlignment.Stretch,
                    VerticalContentAlignment = VerticalAlignment.Stretch,
                    IsChecked = true,
                    Tag = fac_name[0],
                    Content = fac_name[2]
                };
                checkBox.Checked += filter_facultyCheckBox_Checked;
                checkBox.Unchecked += filter_facultyCheckBox_Unchecked;
                item.Content = checkBox;
                filter_facultyComboBox.Items.Add(item);

                faculty_filter.Add(fac_name[0]);
            }
        }

        private void filter_facultyCheckBox_Checked(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            faculty_filter.Add(checkBox.Tag as string);
        }

        private void filter_facultyCheckBox_Unchecked(object sender, EventArgs e)
        {
            CheckBox checkBox = sender as CheckBox;
            faculty_filter.Remove(checkBox.Tag as string);
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
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                FontWeight = FontWeights.Bold,
            };
            requestsListBox.Items.Add(loadingMessage);
        }
        /// <summary>
        /// Очищает workingArea
        /// </summary>
        private void DefaultWorkingArea()
        {
            workingArea.Content = null;
            TextBlock defaultWorkingAreaMessage = new TextBlock
            {
                Text = "Выберите элемент из списка...",
                TextWrapping = TextWrapping.Wrap,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
            };
            workingArea.Content = defaultWorkingAreaMessage;
        }
        /// <summary>
        /// Заполняет заданный requestsList со статусом status_code с помощью запроса к БД
        /// </summary>
        private void GetRequestList(List<Request> requestsList)
        {
            string status_condition = "('" + String.Join("','", status_filter) + "')";
            if (dBconnection.State == System.Data.ConnectionState.Open)
            try
            {
                requestsList.Clear();
                using (NpgsqlCommand cmd =
                new NpgsqlCommand($@"SELECT * FROM {OurDatabase.dBSchema}.{view} WHERE status_code IN {status_condition}", dBconnection))
                {
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            foreach (DbDataRecord dB in reader)
                            {
                                Request r_temp = new Request()
                                {
                                    request_id = (int)dB["request_id"],
                                    request_code = (string)dB["request_code"],
                                    request_name = (string)dB["request_name"],
                                    status_code = (string)dB["status_code"],
                                    status_name = (string)dB["status_name"],
                                    status_short_name = (string)dB["status_short_name"],
                                    first_name = (string)dB["first_name"],
                                    last_name = (string)dB["last_name"],
                                    patronymic = (string)dB["patronymic"],
                                    email = (string)dB["email"],
                                    faculty_code = (string)dB["faculty_code"],
                                    faculty_name = (string)dB["faculty_name"],
                                    faculty_short_name = (string)dB["faculty_short_name"],
                                    student_group = (string)dB["student_group"],
                                    doc_storage_id = (int)dB["doc_storage_id"],
                                    doc_amount = (int)dB["doc_amount"],
                                    public_url = (string)dB["public_url"],
                                    time_when_added = (string)dB["time_when_added"],
                                    time_when_updated = (string)dB["time_when_updated"],
                                    staff_member_login = (string)dB["staff_member_login"],
                                    response_content = (string)dB["response_content"]
                                };
                                requestsList.Add(r_temp);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
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
                    FontWeight = FontWeights.Bold,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    VerticalAlignment = VerticalAlignment.Stretch,
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
                    $"Время поступления: {r.time_when_added}\n" +
                    $"Факультет: {r.faculty_short_name}\n" +
                    $"Тип: {r.request_name}",
                    TextWrapping = TextWrapping.Wrap
                };
                r.button.Tag = r;
                r.button.HorizontalContentAlignment = HorizontalAlignment.Stretch;
                //r.button.FontSize = 11;
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
                $" ({selectedRequest.time_when_added})";
            header.FontSize = 15;
            header.FontWeight = FontWeights.Bold;
            st.Children.Add(header);
            //общая информация
            TextBox genInfo = SetReadTextBox();
            genInfo.Text = $"Состояние: {selectedRequest.status_name}\n" +
                        $"Время поступления: {selectedRequest.time_when_added}\n" +
                        $"Время обновления:  {selectedRequest.time_when_updated}\n" +
                        $"Обработано сотрудником: -\n" +
                        $"Тип: {selectedRequest.request_name}";
            st.Children.Add(genInfo);
            //информация о заявившем
            TextBlock personInfo = new TextBlock();
            personInfo.Text = "\nИнформация о заявившем:";
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
            facultyBox.Text = $"({selectedRequest.faculty_short_name}) {selectedRequest.faculty_name}";
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
            groupBox.Text = selectedRequest.student_group.ToUpper();
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
                $"\nКоличество файлов: {selectedRequest.doc_amount}";
            st.Children.Add(filesInfo);
            //файлы
            StackPanel linkRow = new StackPanel() { Orientation = Orientation.Horizontal };
            Label linkLabel = new Label() { Content = "Ссылка на файлы: " };
            TextBox linkBox = SetReadTextBox();
            linkBox.Text = selectedRequest.public_url;
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
                Text = selectedRequest.response_content,
                Width = 350,
                HorizontalAlignment = HorizontalAlignment.Left,
                Height = 100,
                TextWrapping = TextWrapping.Wrap,
                AcceptsReturn = true,
                AcceptsTab = true,
                IsReadOnly = !(selectedRequest.status_short_name == "new")
            };
            st.Children.Add(responseBox);
            //подтверждение отправки ответа
            if (selectedRequest.status_short_name == "new")
            {
                CheckBox confirm_responseCheckBox = new CheckBox()
                {
                    Content = "Без ответа на почту",
                    Tag = responseBox
                };
                confirm_responseCheckBox.Click += confirm_responseCheckBox_Click;
                st.Children.Add(confirm_responseCheckBox);

                responseBox.Tag = confirm_responseCheckBox;

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
            CheckBox checkBox = textBox.Tag as CheckBox;
            string title = $"Ваша заявка \"{selectedRequest.request_name}\" отклонена";
            DefaultWorkingArea();


            selectedRequest.UpdateRequest("102", title, textBox.Text, dBconnection, OurDatabase.dBSchema);

            if (checkBox.IsChecked == false)
                responseMail.Send(
                            addressee: selectedRequest.email,
                            name: $"{selectedRequest.first_name} {selectedRequest.last_name} {selectedRequest.patronymic}",
                            subject: $"Ваша заявка \"{selectedRequest.request_name}\" отклонена",
                            messageText: textBox.Text);
        }

        private void doneButton_Click(object sender, EventArgs e)
        {
            Button button = sender as Button;
            TextBox textBox = button.Tag as TextBox;
            CheckBox checkBox = textBox.Tag as CheckBox;
            string title = $"Ваша заявка \"{selectedRequest.request_name}\" выполнена";
            DefaultWorkingArea();

            selectedRequest.UpdateRequest("101", title, textBox.Text, dBconnection, OurDatabase.dBSchema);
            if (checkBox.IsChecked == false)
                responseMail.Send(selectedRequest.email,
                            $"{selectedRequest.first_name} {selectedRequest.last_name} {selectedRequest.patronymic}",
                            $"Ваша заявка \"{selectedRequest.request_name}\" выполнена",
                            messageText: textBox.Text);
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
