using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using dbChange.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;

namespace dbChange.Repository{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IHubContext<SignalServer> _context;
        string connectionString = "";
        public EmployeeRepository(IConfiguration configuration,
                                    IHubContext<SignalServer> context)
        {
            connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }
        public List<Employee> GetAllEmployees()
        {
            var employees = new List<Employee>();

            using(SqlConnection conn = new SqlConnection(connectionString)){
                conn.Open();

                SqlDependency.Start(connectionString);

                string commandText = "select Id, Name, Age from dbo.Employees";

                SqlCommand cmd = new SqlCommand(commandText,conn);

                SqlDependency dependency = new SqlDependency(cmd);

                dependency.OnChange+=new OnChangeEventHandler(dbChangeNotification);

                var reader = cmd.ExecuteReader();

                while(reader.Read()){
                    var employee = new Employee{
                        Id = Convert.ToInt32(reader["Id"]),
                        Name = reader["Name"].ToString(),
                        Age = Convert.ToInt32(reader["Age"])
                    };

                    employees.Add(employee);
                }
            }

            return employees;
        }

        private void dbChangeNotification(object sender, SqlNotificationEventArgs e)
        {
            _context.Clients.All.SendAsync("refreshEmployees");
        }
    }
}