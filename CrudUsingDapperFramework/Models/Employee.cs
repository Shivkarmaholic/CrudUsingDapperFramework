using Dapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq;

namespace CrudUsingDapperFramework.Models
{
    [Table("EmpTable")]
    public class Employee
    {

        [Key]
        [ScaffoldColumn(false)]
        public int Id { get; set; }
        [Required(ErrorMessage = "Name is required")]
        [Display(Name = "Employee Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Salary is required")]
        [Display(Name = "Employee Salary")]
        public double Salary { get; set; }

        public static string ConnectionString = "Server=DESKTOP-SQKJ3UE\\SQLEXPRESS;Database=Sample;Integrated Security=True;";

    }

    public class EmployeeDAL
    {
        public static IEnumerable<Employee> GetAllEmployee()
        {
            List<Employee> empList = new List<Employee>();
            using (IDbConnection con = new SqlConnection(Employee.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                empList = con.Query<Employee>("SP_SelectAll_EmpTable").ToList();

            }
            return empList;
        }
        public static Employee GetEmployeeById(int id)
        {
            Employee emp = new Employee();
            using (IDbConnection con = new SqlConnection(Employee.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@id", id);
                emp = con.Query<Employee>("SP_Select_ById_EmpTable", dynamicParameters, commandType: CommandType.StoredProcedure).FirstOrDefault();

            }
            return emp;
        }

        public static int AddEmployee(Employee emp)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(Employee.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@name", emp.Name);
                dynamicParameters.Add("@salary", emp.Salary);
                result = con.Execute("SP_Insert_EmpTable", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public static int UpdateEmployee(Employee emp)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(Employee.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@id", emp.Id);
                dynamicParameters.Add("@name", emp.Name);
                dynamicParameters.Add("@salary", emp.Salary);
                result = con.Execute("SP_Update_EmpTable", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }

        public static int DeleteEmployee(int id)
        {
            int result = 0;
            using (IDbConnection con = new SqlConnection(Employee.ConnectionString))
            {
                if (con.State == ConnectionState.Closed)
                    con.Open();
                DynamicParameters dynamicParameters = new DynamicParameters();
                dynamicParameters.Add("@id", id);
                result = con.Execute("SP_Delete_EmpTable", dynamicParameters, commandType: CommandType.StoredProcedure);
            }
            return result;
        }
    }


}
