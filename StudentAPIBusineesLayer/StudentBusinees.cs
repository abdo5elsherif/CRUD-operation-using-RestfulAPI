using StudentDataAccessLayer;

namespace StudentAPIBusineesLayer
{
    public class StudentBusinees
    {
        public enum enMode { AddNew = 0, Update = 1 }
        private enMode _Mode = enMode.AddNew;

        public int ID { get; set; }
        public string Name { get; set; }
        public int Grade { get; set; }
        public int Age { get; set; }
        public StudentDTO SDTO { get { return new StudentDTO(this.ID, this.Name, this.Age, this.Grade); }  }

        public StudentBusinees(StudentDTO SDTO,enMode mode = enMode.AddNew)
        {
            this.ID = SDTO.Id;
            this.Name = SDTO.Name;
            this.Grade = SDTO.Grade;
            this.Age = SDTO.Age;

            _Mode = mode;
        }

        private bool _AddNewStudent()
        {
            this.ID = StudentData.AddNewStudent(SDTO);
            return (this.ID != -1);
        }

        private bool _UpdateStudent()
        {
            return StudentData.UpdateStudent(SDTO);
        }
        //  public StudentDTO
        public static List<StudentDTO> GetAllStudent()
        {
            return StudentData.GetAllStudent();
        }
        public static List<StudentDTO> GetPassedStudents()
        {
            return StudentData.GetPassedStudent();
        }
        public static double GetAverageGrade()
        {
            return StudentData.GetAverageGrade();
        }

        public static StudentBusinees Find(int ID)
        {
            StudentDTO SDTO = StudentData.GetStudentByID(ID);
            if( SDTO != null )
            {
                return new StudentBusinees(SDTO, enMode.Update);
            }
            else
            {
                return null; 
            }
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.AddNew:
                    if (_AddNewStudent())
                    {
                        _Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                    case enMode.Update:
                    return _UpdateStudent();
                   
            }
            return false;
        }

        public static bool DeleteStudent(int ID)
        {
            return StudentData.DeleteStudent(ID);
        }
    }
}
