using System.Data.SqlClient;
using Hotel_Reservation.Pages.Guests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Reservation.Pages.Rooms
{
    public class IndexModel : PageModel
    {
        public List<Roominfo> listroom= new List<Roominfo>();

        public void OnGet()
        {
            try
            {
                String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True";

                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "SELECT * FROM rooms";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Roominfo roominfo = new Roominfo();
                                roominfo.room_number = "" + reader.GetInt32(0);
                                roominfo.room_type = reader.GetString(1);
                                roominfo.price = "" +reader.GetDecimal(2);
                                roominfo.is_occupied="" +  reader.GetBoolean(4);
                                try
                                {
                                    roominfo.guest_id = "" + reader.GetInt32(3);
                                }
                                catch {
                                    roominfo.guest_id = "" + 0;
                                }

                                listroom.Add(roominfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Exception: " + ex.ToString());
            }
        }
    }
    public class Roominfo
    {
        public String room_number;

        public String room_type;

        public String price;

        public String guest_id;

        public String is_occupied;


    }
}
