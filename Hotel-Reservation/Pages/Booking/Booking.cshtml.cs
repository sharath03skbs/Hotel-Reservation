using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using Hotel_Reservation.Pages.Guests;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.VisualBasic;

namespace Hotel_Reservation.Pages.Booking
{
    [Authorize(Policy = "RequireStaffRole")]
    public class BookingModel : PageModel
    {
		String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True";
		public BookingInfo bookinginfo = new BookingInfo();

		public int[] Availablerooms { get; set; }
		public int[] guestlist { get; set; }

		public String[] fnamelist { get; set; }
		public String[] lnamelist { get; set; }

		public void OnGet()
		{
			SqlConnection con = new SqlConnection(connectionString);
			DataTable dt = new DataTable();
			DataTable dt1 = new DataTable();
			SqlDataAdapter da = new SqlDataAdapter("select room_number from rooms where is_occupied=0", con);
			SqlDataAdapter da1 = new SqlDataAdapter("select guest_id,first_name,last_name from guests ", con);	

			da.Fill(dt);
			da1.Fill(dt1);

			Availablerooms = new int[dt.Rows.Count];

			guestlist = new int[dt1.Rows.Count];



			for (int i = 0; i < Availablerooms.Length; i++)
			{
				Availablerooms[i] = int.Parse(dt.Rows[i][0].ToString());
			}

			for (int i = 0; i < guestlist.Length; i++)
			{
				guestlist[i] = int.Parse(dt1.Rows[i]["guest_id"].ToString());

			}
		}
		public void OnPost() 
		{
			bookinginfo.guest_id = Request.Form["guestid"];
			bookinginfo.room_type= Request.Form["room"];
			bookinginfo.room_number = Request.Form["room_num"];

			try
			{
				String connectionString = "Data Source=.\\sqlexpress;Initial Catalog=hotel-reservation;Integrated Security=True";

				using (SqlConnection connection = new SqlConnection(connectionString))
				{
					connection.Open();
					String sql = "UPDATE rooms SET guest_id=@guest_id, is_occupied=1 WHERE room_number=@room_number;";

					using (SqlCommand command = new SqlCommand(sql, connection))
						{


							command.Parameters.AddWithValue("@guest_id", bookinginfo.guest_id);
							command.Parameters.AddWithValue("@room_number", bookinginfo.room_number);

							command.ExecuteNonQuery();

						}

					}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
				return;
			}

			Response.Redirect("/Rooms/Index");
		}
    }

    public class BookingInfo
    {
		public String room_type;

		public String guest_id;

		public String room_number;

		public String is_occupied;

	}

}
