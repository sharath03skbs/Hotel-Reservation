using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Hotel_Reservation.Pages.Guests
{
    public class EditModel : PageModel
    {
        public GuestInfo guestinfo = new GuestInfo();
        public String errormsg = "";
        public String successmsg = "";
        public void OnGet(int id)
        {
           

            try
            {
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = $"SELECT * FROM guests WHERE guest_id = {id}";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{
						using (SqlDataReader reader = command.ExecuteReader())
						{
							if (reader.Read())
							{
								guestinfo.guest_id = "" + reader.GetInt32(0);
								guestinfo.first_name = reader.GetString(1);
								guestinfo.last_name = reader.GetString(2);
								guestinfo.email = reader.GetString(3);
								guestinfo.phone = reader.GetString(4);
							
							}
						}
					}
				}
			}
            catch (Exception ex)
            {
                errormsg = ex.Message;
            }
        }

		public void OnPost()
		{
			guestinfo.guest_id = Request.Form["id"];
			guestinfo.first_name = Request.Form["first_name"];
			guestinfo.last_name = Request.Form["last_name"];
			guestinfo.phone = Request.Form["phone"];
			guestinfo.email = Request.Form["email"];

			if (guestinfo.guest_id.Length == 0 || guestinfo.first_name.Length == 0 || guestinfo.last_name.Length == 0
				|| guestinfo.phone.Length == 0 || guestinfo.email.Length == 0)
			{
				errormsg = "All fields are required";
				return;
			}
			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE guests "+
									"SET first_name = @first_name,last_name = @last_name,"+
									 "email= @email ,phone = @phone "+
									 "WHERE guest_id= @guest_id";

					using (SqlCommand command = new SqlCommand(sql, connection))
					{

						command.Parameters.AddWithValue("@first_name", guestinfo.first_name);
						command.Parameters.AddWithValue("@last_name", guestinfo.last_name);
						command.Parameters.AddWithValue("@email", guestinfo.email);
						command.Parameters.AddWithValue("@phone", guestinfo.phone);
						command.Parameters.AddWithValue("@guest_id", guestinfo.guest_id);

						command.ExecuteNonQuery();
	
					}
				}

			}
			catch (Exception ex)
			{
				errormsg = ex.Message;
				return;
			}

			Response.Redirect("/Guests/Index");

		}
	}
}
