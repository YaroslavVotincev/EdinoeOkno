using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace EdinoeOkno_program
{
    class Request
    {
        public int request_id;
        public string request_name;
        public int status_code;
        public string status_name;
        public string first_name;
        public string last_name;
        public string patronymic;
        public string email;
        public string faculty_name;
        public string faculty_name_short;
        public string group;
        public string dir_path;
        public int files_attached;
        public string time_when_requested;
        public string time_when_updated = "-";
        public string response = "";

        public bool valid_email;

        public Button button;
        
        public Request( int request_id,
                        string request_name,
                        string status_name,
                        string first_name,
                        string last_name,
                        string patronymic,
                        string email,
                        string faculty_name,
                        string faculty_name_short,
                        string group,
                        string time_when_requested,
                        string dir_path,
                        int files_attached)
        {
            this.request_id = request_id;
            this.request_name = request_name;
            this.status_name = status_name;
            this.first_name = first_name;
            this.last_name = last_name;
            this.patronymic = patronymic;
            this.email = email;
            this.faculty_name = faculty_name;
            this.faculty_name_short = faculty_name_short;
            this.group = group;
            this.time_when_requested = time_when_requested;
            this.dir_path = dir_path;
            this.files_attached = files_attached;
            this.valid_email = IsValidEmail(email);
        }
        public Request(int request_id,
                        string request_name,
                        string status_name,
                        string first_name,
                        string last_name,
                        string patronymic,
                        string email,
                        string faculty_name,
                        string faculty_name_short,
                        string group,
                        string time_when_requested,
                        string dir_path,
                        int files_attached,
                        string time_when_updated,
                        string response)
        {
            this.request_id = request_id;
            this.request_name = request_name;
            this.status_name = status_name;
            this.first_name = first_name;
            this.last_name = last_name;
            this.patronymic = patronymic;
            this.email = email;
            this.faculty_name = faculty_name;
            this.faculty_name_short = faculty_name_short;
            this.group = group;
            this.time_when_requested = time_when_requested;
            this.dir_path = dir_path;
            this.files_attached = files_attached;
            this.valid_email = IsValidEmail(email);
            this.time_when_updated = time_when_updated;
            this.response = response; 
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
