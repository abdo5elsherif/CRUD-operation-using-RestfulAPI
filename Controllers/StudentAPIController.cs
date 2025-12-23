using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StudentAPI.DataSimulation;
using StudentAPI.Model;
using StudentAPIBusineesLayer;
using StudentDataAccessLayer;

namespace StudentAPI.Controllers
{
    [Route("api/StudentAPI")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        [HttpGet("All", Name = "GetAllStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<StudentDTO>> GetAllStudents()
        {
            List<StudentDTO> StudentsList = StudentBusinees.GetAllStudent();
            if (StudentsList.Count == 0)
            {
                return NotFound("No studen found.");
            }
            return Ok(StudentsList);
        }

        [HttpGet("Passed", Name = "GetPassedStudents")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<IEnumerable<Student>> GetPassedStudents()
        {

            var Passedstudents = StudentBusinees.GetPassedStudents();
            if (Passedstudents.Count == 0)
            {
                return NotFound("No students passed!");
            }

            return Ok(Passedstudents);
        }

     

        [HttpGet("AvgGrade", Name = "GetAvgGrade")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<double> GetAvgGrade()
        {
            var AvgGrade = StudentBusinees.GetAverageGrade();
            if (AvgGrade == 0)
            {
                return NotFound("No students found.");
            }
           

            return Ok(AvgGrade);
        }

        [HttpGet("{ID}", Name = "GetStudentbyID")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<StudentDTO> GetStudentByID(int ID)
        {
            if (ID < 1)
            {
                return BadRequest($"Not accepted ID {ID}");
            }
            var Student = StudentBusinees.Find(ID);
            //var Student = StudentDataSimulation.StudentsList.Where(s => s.Id == ID);
            if (Student == null)
            {
                return NotFound($"Student with ID {ID} Not exist");

               
            }
            return Ok(Student.SDTO);
        }

        [HttpPost("AddNew", Name = "AddStudent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public ActionResult<StudentDTO> AddNewStudent(StudentDTO newstudentDTO)
        {
            if (newstudentDTO == null || string.IsNullOrEmpty(newstudentDTO.Name) || newstudentDTO.Age < 0 || newstudentDTO.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }

            StudentBusinees Student = new StudentBusinees(new StudentDTO( newstudentDTO.Id,newstudentDTO.Name, newstudentDTO.Age, newstudentDTO.Grade));
            Student.Save();

            newstudentDTO.Id = Student.ID;
            return CreatedAtRoute("GetStudentbyID", new { id = newstudentDTO.Id }, newstudentDTO);

        }

        [HttpDelete("{id}", Name = "DeleteStudent")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public ActionResult DeleteStudent(int id) 
        {
            if(id < 1)
            {
                return BadRequest($"Not accepted ID: {id}");
            }

            // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);

            if (StudentBusinees.DeleteStudent(id))
            {
                return Ok($"Student with ID {id} has been deleted.");

            }
            else
                return NotFound($"Student with ID {id} not found.");
           
          
        }

        [HttpPut("{id}", Name = "UpdateStudent")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<StudentDTO> UpdateStudent(int id, StudentDTO updatedStudent)
        {
            if (id < 1 || updatedStudent == null || string.IsNullOrEmpty(updatedStudent.Name) || updatedStudent.Age < 0 || updatedStudent.Grade < 0)
            {
                return BadRequest("Invalid student data.");
            }
            var student = StudentBusinees.Find(id);

           // var student = StudentDataSimulation.StudentsList.FirstOrDefault(s => s.Id == id);
            if (student == null)
            {
                return NotFound($"Student with ID {id} not found.");
            }

            student.Name = updatedStudent.Name;
            student.Age = updatedStudent.Age;
            student.Grade = updatedStudent.Grade;
            if (student.Save())
            {
                return Ok(student.SDTO);
            }
            return StatusCode(500, new { message = $"Error Updating student with ID {id}." });
        }

        [HttpPost("Upload")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UploadImage(IFormFile imagefile)
        {
            if (imagefile == null || imagefile.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }

            var UploadDirectory = @"D:\MyUploads";

            var filename  = Guid.NewGuid().ToString() + Path.GetExtension(imagefile.FileName);
            var filepath = Path.Combine(UploadDirectory, filename);

            if (!Directory.Exists(UploadDirectory))
            {
                Directory.CreateDirectory(UploadDirectory);
            }

            using(var stream = new FileStream(filepath,FileMode.CreateNew))
            {
                imagefile.CopyTo(stream);
            }
            return Ok(filepath);
        }

        [HttpGet(("GetImage/{FileName}"))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult GetImage(string FileName)
        {
            var UploadDirectory = @"D:\MyUploads";
            var filepath = Path.Combine(UploadDirectory, FileName);

            if (!System.IO.File.Exists(filepath))
            {
                return BadRequest("Image not found.");
            }

            var image = System.IO.File.OpenRead(filepath);
            var mime = GetMimeType(filepath);

            return File(image, mime);
        }

        private string GetMimeType(string filepath)
        {
            var Extension = Path.GetExtension(filepath).ToLowerInvariant();
            return Extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png"  => "image/png",
                ".gif"  => "image/gif",
                _ =>"application/octet-stream"
            };
        }
    }

}
