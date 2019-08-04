using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;

namespace BusinessLogic
{
    public interface IEmployee
    {
        int Id { get; set; }
        string City { get; set; }
        string Gender { get; set; }
        DateTime? DateOfBirth { get; set; }
    }
    public class Employee : IEmployee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime? DateOfBirth { get; set; }
    }

    public class EmployeeManagement
    {
        public EmployeeManagement()
        {

        }

        public IEnumerable<Employee> GetEmployeeDetails()
        {
            List<Employee> employeeList = new List<Employee>();

            try
            {
                string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

                if (!string.IsNullOrEmpty(connectionString))
                {
                    using (SqlConnection con = new SqlConnection(connectionString))
                    {
                        SqlCommand sqlCommand = new SqlCommand("SP_GetEmployeeDetails");
                        sqlCommand.CommandType = CommandType.StoredProcedure;
                        sqlCommand.Connection = con;
                        sqlCommand.CommandTimeout = 0;
                        con.Open();
                        SqlDataReader dataReader = sqlCommand.ExecuteReader();

                        if (dataReader.HasRows)
                        {
                            while (dataReader.Read())
                            {
                                Employee emp = new Employee();
                                emp.Id = Convert.ToInt32(dataReader["Id"]);
                                emp.Name = dataReader["Name"].ToString();
                                emp.City = dataReader["City"].ToString();
                                emp.Gender = dataReader["Gender"].ToString();

                                if (!(dataReader["DateOfBirth"] is DBNull))
                                    emp.DateOfBirth = Convert.ToDateTime(dataReader["DateOfBirth"]);

                                employeeList.Add(emp);
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                throw;
            }

            return employeeList;
        }

        public void AddNewEmployee(Employee employee)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand("SP_AddEmployee");
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Connection = con;
                    sqlCommand.CommandTimeout = 0;

                    sqlCommand.Parameters.Add(new SqlParameter("@Name", employee.Name));
                    sqlCommand.Parameters.Add(new SqlParameter("@Gender", employee.Gender));
                    sqlCommand.Parameters.Add(new SqlParameter("@City", employee.City));
                    sqlCommand.Parameters.Add(new SqlParameter("@DateOfBirth", employee.DateOfBirth));

                    con.Open();
                    sqlCommand.ExecuteNonQuery();

                }
            }
        }

        public void SaveEmployee(Employee employee)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand("spSaveEmployee");
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Connection = con;
                    sqlCommand.CommandTimeout = 0;

                    sqlCommand.Parameters.Add(new SqlParameter("@Id", employee.Id));
                    sqlCommand.Parameters.Add(new SqlParameter("@Name", employee.Name));
                    sqlCommand.Parameters.Add(new SqlParameter("@Gender", employee.Gender));
                    sqlCommand.Parameters.Add(new SqlParameter("@City", employee.City));
                    sqlCommand.Parameters.Add(new SqlParameter("@DateOfBirth", employee.DateOfBirth));

                    con.Open();
                    sqlCommand.ExecuteNonQuery();

                }
            }
        }

        public void DeleteEmployee(int id)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["DBCS"].ConnectionString;

            if (!string.IsNullOrEmpty(connectionString))
            {
                using (SqlConnection con = new SqlConnection(connectionString))
                {
                    SqlCommand sqlCommand = new SqlCommand("SP_DeleteEmployee");
                    sqlCommand.CommandType = CommandType.StoredProcedure;
                    sqlCommand.Connection = con;
                    sqlCommand.CommandTimeout = 0;

                    sqlCommand.Parameters.Add(new SqlParameter("@Id", id));

                    con.Open();
                    sqlCommand.ExecuteNonQuery();

                }
            }
        }
    }
}
