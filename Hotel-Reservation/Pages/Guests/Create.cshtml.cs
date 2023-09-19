using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Reservation.Pages.Guests
{
    public class CreateModel : PageModel
    {
        public GuestInfo guestinfo = new GuestInfo();
        public String errormsg = "";
        public String successmsg = "";
        public void OnGet()
        {

        }

        public void OnPost()
        {
            guestinfo.first_name = Request.Form["first_name"];
            guestinfo.last_name = Request.Form["last_name"];
			guestinfo.email = Request.Form["email"];
			guestinfo.phone = Request.Form["phone"];

            if(guestinfo.first_name.Length == 0 || guestinfo.last_name.Length == 0
                || guestinfo.phone.Length == 0 || guestinfo.email.Length == 0)
            {
                errormsg = "All fields are required";
                return;
            }

            try
            {
                String connectionString = " Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String sql = "INSERT INTO guests " +
                                  "(first_name , last_name , email, phone) VALUES" +
                                  "(@first_name, @last_name, @email, @phone);";

                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@first_name", guestinfo.first_name);
						command.Parameters.AddWithValue("@last_name", guestinfo.last_name);
						command.Parameters.AddWithValue("@email", guestinfo.email);
						command.Parameters.AddWithValue("@phone", guestinfo.phone);

                        command.ExecuteNonQuery();
					}
				}
            }
            catch(Exception ex)
            {
                errormsg=ex.Message;
                return;
            }


            guestinfo.first_name = " ";
			guestinfo.last_name = " ";
			guestinfo.email = " ";
			guestinfo.phone = " ";

            successmsg = " New Guest added successfully";

            Response.Redirect("/Guests/Index");

		}
	}
}
