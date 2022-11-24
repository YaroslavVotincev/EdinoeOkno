using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Npgsql;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows;
using System.Data.Common;

namespace EdinoeOkno_program
{
    class Request
    {
        public int request_id;
        public string request_code;
        public string request_name;
        public string status_code;
        public string status_name;
        public string status_short_name;
        public string first_name;
        public string last_name;
        public string patronymic;
        public string email;
        public string faculty_code;
        public string faculty_name;
        public string faculty_short_name;
        public string student_group;
        public int doc_storage_id;
        public int doc_amount;
        public string public_url;
        public string time_when_added;
        public string time_when_updated = "-";
        public string staff_member_login = "-";
        public string response_content;

        public Button button;    
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
        public async Task UpdateRequest(string new_status_code, string title, string response_content, NpgsqlConnection dBconnection, string dBSchema)
        {
            try
            {
                string query = $@"WITH tmp AS
	                            (INSERT INTO {dBSchema}.responses (email, title, response_content, type) 
	                                VALUES ('{this.email}', '{title}', '{response_content}', 'request')
	                                RETURNING response_id
	                            )
                                UPDATE {dBSchema}.requests SET
	                                status_code = '{new_status_code}',
	                                response_id = (SELECT * FROM tmp),
	                                time_when_updated = now()
                                WHERE request_id = '{this.request_id}';";
                NpgsqlCommand cmd = new NpgsqlCommand(query, dBconnection);
                await cmd.ExecuteNonQueryAsync();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
