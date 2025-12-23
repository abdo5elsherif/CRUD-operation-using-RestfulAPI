using Microsoft.Data.SqlClient;
using System.Data;

namespace StudentDataAccessLayer
{
    public class StudentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public int Grade { get; set; }

        public StudentDTO(int id, string name, int age, int grade)
        {
            Id = id;
            Name = name;
            Age = age;  
            Grade = grade;
        }
    }
    public static class StudentData
    {
        public static List<StudentDTO> GetAllStudent()
        {
            try
            {
                var studentsList = new List<StudentDTO>();
                using (SqlConnection conn = new SqlConnection(DataAccessSetting._connectionString))
                {
                    using (SqlCommand cmd = new SqlCommand("SP_GetAllStudents", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                studentsList.Add(new StudentDTO(
                                reader.GetInt32(reader.GetOrdinal("Id")),
                                reader.GetString(reader.GetOrdinal("Name")),
                                reader.GetInt32(reader.GetOrdinal("Age")),
                                reader.GetInt32(reader.GetOrdinal("Grade"))

                                    ));
                            }
                        }

                    }

                }
                return studentsList;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public static List<StudentDTO> GetPassedStudent()
        {
            var ListStudent = new List<StudentDTO>();

            using (SqlConnection conn = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetPassedStudents", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {

                            ListStudent.Add(new StudentDTO(
                            reader.GetInt32(reader.GetOrdinal("Id")),
                            reader.GetString(reader.GetOrdinal("Name")),
                            reader.GetInt32(reader.GetOrdinal("Age")),
                            reader.GetInt32(reader.GetOrdinal("Grade"))

                                ));
                        }
                    }


                }
            }
            return ListStudent;
        }

        public static double GetAverageGrade()
        {
            double AverageGrade = 0.0;

            using (SqlConnection conn = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetAverageGrade", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    conn.Open();

                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        AverageGrade = Convert.ToDouble(result);
                    }
                    else
                    { AverageGrade = 0.0; }




                }
            }
            return AverageGrade;
        }

        public static StudentDTO GetStudentByID(int ID)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_GetStudentById", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@StudentId", ID);
                    conn.Open();
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {

                            return new StudentDTO(
                             reader.GetInt32(reader.GetOrdinal("Id")),
                             reader.GetString(reader.GetOrdinal("Name")),
                             reader.GetInt32(reader.GetOrdinal("Age")),
                             reader.GetInt32(reader.GetOrdinal("Grade"))

                                 );
                        }
                        else
                            return null;
                    }


                }
            }
        }

        public static int AddNewStudent(StudentDTO StudentDTO)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_AddStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", StudentDTO.Name);
                    cmd.Parameters.AddWithValue("@Age", StudentDTO.Age);
                    cmd.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                    var OutputIdParam = new SqlParameter("@NewStudentId", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output,
                    };
                    cmd.Parameters.Add(OutputIdParam);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return (int)OutputIdParam.Value;

                }
            }
        }

        public static bool UpdateStudent(StudentDTO StudentDTO)
        {
            using (SqlConnection conn = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_UpdateStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Name", StudentDTO.Name);
                    cmd.Parameters.AddWithValue("@Age", StudentDTO.Age);
                    cmd.Parameters.AddWithValue("@Grade", StudentDTO.Grade);
                    cmd.Parameters.AddWithValue("@StudentId", StudentDTO.Id);


                    conn.Open();
                    cmd.ExecuteNonQuery();
                    return true;

                }
            }

        }

        public static bool DeleteStudent(int StudentID)
        {
            int Rowaffecte = -1;
            using (SqlConnection conn = new SqlConnection(DataAccessSetting._connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("SP_DeleteStudent", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                   
                    cmd.Parameters.AddWithValue("@StudentId", StudentID);


                    conn.Open();
                    object result = cmd.ExecuteScalar();
                    if (result != DBNull.Value)
                    {
                        Rowaffecte = Convert.ToInt32(result);
                    }
                    else
                    { Rowaffecte = -1; }

                   

                }
            }
            return Rowaffecte!=-1;
        }

    }
}
