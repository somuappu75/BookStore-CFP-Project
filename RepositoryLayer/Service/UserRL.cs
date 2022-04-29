using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace RepositoryLayer.Service
{
   public class UserRL
    {
        private readonly IConfiguration Configuration;
        public UserRL(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        //To register A User 
        public void AddUser(UserModel userModel)
        {
            using (SqlConnection con = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB")))
            {
                SqlCommand cmd = new SqlCommand("spRegisterUser", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@FullName", userModel.FullName);
                cmd.Parameters.AddWithValue("@EmailId",userModel.EmailId);
                cmd.Parameters.AddWithValue("@Password",userModel.Password);
                cmd.Parameters.AddWithValue("@MobileNumber",userModel.MobileNumber);
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
        }
    }
}
