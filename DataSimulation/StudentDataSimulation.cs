using StudentAPI.Model;

namespace StudentAPI.DataSimulation
{
    public class StudentDataSimulation
    {
        public static readonly List<Student> StudentsList = new List<Student>
        {
            new Student{Id = 1,Name = "Ali Ahmed",Age = 20,Grade = 88},
            new Student{Id = 2,Name = "Fadi Khaled",Age = 22,Grade = 77},
            new Student{Id = 3,Name = "Ola Gaber",Age = 21,Grade = 66},
            new Student{Id = 4,Name = "Abdullah Mohammed",Age = 22,Grade = 40}
        };
    }
}
    