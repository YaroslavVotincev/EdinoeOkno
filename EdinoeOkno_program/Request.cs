using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Text.RegularExpressions;

namespace EdinoeOkno_program
{
    class Request
    {
		private int request_id
        private string request_name;
		private string status_code;
        private string status_name;
        private string first_name;
        private string last_name;
        private string patronymic;
        private string email;
        private string faculty_name;
        private string faculty_name_short;
        private string group;
        private string dir_path;
        private int files_attached;
        private string time_when_requested;
        private string response;

        private bool valid_email; 
        
        public Request( string request_name,
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
