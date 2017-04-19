﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Common_Ground_Project.Models;

namespace Common_Ground_Project.DataAccess
{
    public class EmergencyContactSql
    {
        public EmergencyContact GetEmergencyContact(EmergencyContact contact)
        {
            List<EmergencyContact> returnList = new List<EmergencyContact>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@EmergencyContactID", contact.EmergencyContactID));

            SqlCommand cmd = new SqlCommand("Master.dbo.EmergencyContactGetByID");
            returnList = createConnection(cmd, parameters);

            if (returnList.Count > 0)
                return returnList[0];
            else
                return new EmergencyContact();
        }
        public List<EmergencyContact> GetEmergencyContactList(Individual individual)
        {
            List<EmergencyContact> returnList = new List<EmergencyContact>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IndividualID", individual.IndividualID));

            SqlCommand cmd = new SqlCommand("Master.dbo.EmergencyContactGetByIndividual");
            returnList = createConnection(cmd, parameters);

            return returnList;
        }

        public void SaveEmergencyContact(EmergencyContact contact)
        {
            using (SqlConnection connection = new SqlConnection(LoginCredentials.ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (contact.EmergencyContactID == 0)
                        {
                            cmd.CommandText = "Master.dbo.EmergencyContactInsert";
                            cmd.Parameters.Add(new SqlParameter("@NewID", DBNull.Value).Direction = System.Data.ParameterDirection.ReturnValue);
                        }
                        else
                        {
                            cmd.CommandText = "Master.dbo.EmergencyContactUpdate";
                            cmd.Parameters.AddWithValue("@EmergencyContactID", contact.EmergencyContactID);
                        }

                        cmd.Parameters.AddWithValue("@IndividualID", contact.IndividualID);
                        cmd.Parameters.AddWithValue("@Name", contact.Name);
                        cmd.Parameters.AddWithValue("@Phone", contact.Phone);
                        cmd.Parameters.AddWithValue("@Email", contact.Email);

                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection.Open();
                        cmd.ExecuteScalar();

                        if (contact.EmergencyContactID == 0)
                        {
                            int returnID = Convert.ToInt32(cmd.Parameters["@NewID"].Value);
                            if (returnID > 0)
                                contact.EmergencyContactID = returnID;
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void DeleteEmergencyContact(EmergencyContact contact)
        {
            using (SqlConnection connection = new SqlConnection(LoginCredentials.ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "Master.dbo.EmergencyContactDelete";

                        cmd.Parameters.AddWithValue("@EmergencyContactID", contact.EmergencyContactID);

                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection.Open();
                        cmd.ExecuteScalar();
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
        }



        private List<EmergencyContact> createConnection(SqlCommand cmd, List<SqlParameter> parameters = null)
        {
            List<EmergencyContact> returnList = new List<EmergencyContact>();
            using (SqlConnection connection = new SqlConnection(LoginCredentials.ConnectionString))
            {
                try
                {
                    using (cmd)
                    {
                        if (parameters != null)
                            cmd.Parameters.AddRange(parameters.ToArray());

                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection.Open();
                        using (SqlDataReader rdr = cmd.ExecuteReader())
                        {
                            while (rdr.Read())
                            {
                                returnList.Add(new EmergencyContact(rdr));
                            }
                        }
                    }
                }
                catch (Exception)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            return returnList;
        }
    }
}