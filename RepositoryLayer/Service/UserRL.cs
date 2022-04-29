﻿using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
   public class UserRL:IUserRL
    {
        private IConfiguration Configuration;
        private SqlConnection sqlConnection;
        public UserRL(IConfiguration Configuration)
        {
            this.Configuration = Configuration;
        }
        //To register A User 
        public UserModel RegisterUser(UserModel userModel)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStore App"));

            try
            {
                using (sqlConnection)
                {
                    SqlCommand cmd = new SqlCommand("spRegisterUser", sqlConnection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@FullName", userModel.FullName);
                    cmd.Parameters.AddWithValue("@EmailId", userModel.EmailId);
                    cmd.Parameters.AddWithValue("@Password", userModel.Password);
                    cmd.Parameters.AddWithValue("@MobileNumber", userModel.MobileNumber);
                    sqlConnection.Open();
                    int i = cmd.ExecuteNonQuery();
                    sqlConnection.Close();
                    if (i >= 1)
                    {
                        return userModel;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public string GenerateJWTToken(string email, int UserId)
        {
            // header
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // payload
            var claims = new[]
            {
                new Claim("Email", email),
                new Claim("Id", UserId.ToString()),
            };

            // signature
            var token = new JwtSecurityToken(
                this.Configuration["Jwt:Issuer"],
                this.Configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string Login(string email, string password)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            try
            {
                using (sqlConnection)
                {
                    UserModel model = new UserModel();

                    SqlCommand command = new SqlCommand("spUserLogin", sqlConnection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailId", email);
                    command.Parameters.AddWithValue("@Password", password);
                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        int UserId = 0;
                        while (reader.Read())
                        {
                            model.EmailId = Convert.ToString(reader["EmailId"] == DBNull.Value ? default : reader["EmailId"]);
                            UserId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
                            model.Password = Convert.ToString(reader["Password"] == DBNull.Value ? default : reader["Password"]);
                        }
                        sqlConnection.Close();
                        var token = GenerateJWTToken(email, UserId);
                        return token;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }

        }

        public string GenerateJWTTokenForForgotPassword(string email, int userId)
        {
            // header
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this.Configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            // payload
            var claims = new[]
            {
                new Claim("EmailId", email),
                new Claim("Id", userId.ToString()),
            };

            // signature
            var token = new JwtSecurityToken(
                this.Configuration["Jwt:Issuer"],
                this.Configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public string ForgotPassword(string email)
        {
            sqlConnection = new SqlConnection(this.Configuration.GetConnectionString("BookStoreDB"));
            try
            {
                using (sqlConnection)
                {
                    UserModel model = new UserModel();
                    SqlCommand command = new SqlCommand("spUserForgotPassword", sqlConnection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@EmailId", email);

                    sqlConnection.Open();
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        int userId = 0;
                        while (reader.Read())
                        {
                            email = Convert.ToString(reader["EmailId"] == DBNull.Value ? default : reader["Email"]);
                            userId = Convert.ToInt32(reader["UserId"] == DBNull.Value ? default : reader["UserId"]);
                        }
                        sqlConnection.Close();
                        var token = GenerateJWTTokenForForgotPassword(email, userId);
                        new MSMQ().Sender(token);
                        return token;
                    }
                    else
                    {
                        sqlConnection.Close();
                        return null;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

        public bool ResetPassword(string email, string newPassword, string confirmPassword)
        {
            try
            {
                if (newPassword == confirmPassword)
                {
                    sqlConnection = new SqlConnection(Configuration.GetConnectionString("BookStoreDB"));
                    using (sqlConnection)
                    {
                        SqlCommand command = new SqlCommand("spUserResetPassword", sqlConnection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@EmailId", email);
                        command.Parameters.AddWithValue("@Password", confirmPassword);
                        sqlConnection.Open();
                        int i = command.ExecuteNonQuery();
                        sqlConnection.Close();
                        if (i >= 1)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                sqlConnection.Close();
            }
        }

 

      
    }
}
