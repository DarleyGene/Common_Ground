﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Common_Ground_Project.Models;

namespace Common_Ground_Project.DataAccess
{
    public class ActivitySql
    {
        public Activity GetActivity(Activity activity)
        {
            List<Activity> returnList = new List<Activity>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@ActivityID", activity.ActivityID));

            SqlCommand cmd = new SqlCommand("Master.dbo.ActivityGetByID");
            returnList = createConnection(cmd, parameters);

            if (returnList.Count > 0)
                return returnList[0];
            else
                return new Activity();
        }
        public List<Activity> GetActivityList(Individual individual)
        {
            List<Activity> returnList = new List<Activity>();
            List<SqlParameter> parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@IndividualID", individual.IndividualID));

            SqlCommand cmd = new SqlCommand("Master.dbo.ActivityGetByIndividual");
            returnList = createConnection(cmd, parameters);

            return returnList;
        }

        public void SaveActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(LoginCredentials.ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        if (activity.ActivityID == 0)
                        {
                            cmd.CommandText = "Master.dbo.ActivityInsert";
                            cmd.Parameters.Add(new SqlParameter("@NewID", DBNull.Value).Direction = System.Data.ParameterDirection.ReturnValue);
                        }
                        else
                        {
                            cmd.CommandText = "Master.dbo.ActivityUpdate";
                            cmd.Parameters.AddWithValue("@ActivityID", activity.ActivityID);
                        }

                        cmd.Parameters.AddWithValue("@Title", activity.Title);
                        cmd.Parameters.AddWithValue("@TripLeaderID", activity.TripLeaderID);
                        cmd.Parameters.AddWithValue("@Description", activity.Description);
                        cmd.Parameters.AddWithValue("@Location", activity.Location);
                        cmd.Parameters.AddWithValue("@StartDate", activity.StartDate);
                        cmd.Parameters.AddWithValue("@EndDate", activity.EndDate);
                        cmd.Parameters.AddWithValue("@PickUpTime", activity.PickUpTime);
                        cmd.Parameters.AddWithValue("@DropOffTime", activity.DropOffTime);
                        cmd.Parameters.AddWithValue("@StaffArrivalTime", activity.StaffArrivalTime);
                        cmd.Parameters.AddWithValue("@Cost", activity.Cost);

                        cmd.Connection = connection;
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Connection.Open();
                        cmd.ExecuteScalar();

                        if (activity.ActivityID == 0)
                        {
                            int returnID = Convert.ToInt32(cmd.Parameters["@NewID"].Value);
                            if (returnID > 0)
                                activity.ActivityID = returnID;
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

        public void DeleteActivity(Activity activity)
        {
            using (SqlConnection connection = new SqlConnection(LoginCredentials.ConnectionString))
            {
                try
                {
                    using (SqlCommand cmd = new SqlCommand())
                    {
                        cmd.CommandText = "Master.dbo.ActivityDelete";

                        cmd.Parameters.AddWithValue("@ActivityID", activity.ActivityID);

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



        private List<Activity> createConnection(SqlCommand cmd, List<SqlParameter> parameters = null)
        {
            List<Activity> returnList = new List<Activity>();
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
                                returnList.Add(new Activity(rdr));
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
