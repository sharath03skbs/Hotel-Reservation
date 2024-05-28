using System.Data.SqlClient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Reservation.Pages.Guests
{
    [Authorize(Policy = "RequireStaffRole")]
    public class IndexModel : PageModel
    {

        public List<GuestInfo> listguests = new List<GuestInfo>();
        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString)) 
                { 
                    connection.Open();
                    String sql = "SELECT * FROM guests";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                            {
                            while (reader.Read())
                            {
                                GuestInfo guestInfo = new GuestInfo();
                                guestInfo.guest_id= "" + reader.GetInt32(0);
                                guestInfo.first_name = reader.GetString(1);
                                guestInfo.last_name = reader.GetString(2);
                                guestInfo.phone = reader.GetString(3);
                                guestInfo.email = reader.GetString(4);
                                guestInfo.check_in= reader.GetDateTime(5).ToString();

                                listguests.Add(guestInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex) {

                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }

    public  class GuestInfo
    {
        public String guest_id ;

        public String first_name;

        public String last_name; 

        public String email; 

        public String phone; 

        public String check_in;

    }
}
