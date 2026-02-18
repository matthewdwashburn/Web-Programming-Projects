using Microsoft.AspNetCore.Mvc;
using Students.Entities;
using Students.Models;
using System.Net;
using static Students.Entities.StudentEntity;

namespace Students.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentsController : Controller
    {
        //initialize one student in the array 
        private static List<StudentModel> students = new List<StudentModel>()
        {
            new StudentModel {
            FirstName = "Matthew",
            LastName = "Washburn",
            FavoriteCharacter = "R2D2"
            }
        };

        [HttpGet]
        public StudentsEntity Get()
        {
            return new StudentsEntity()
            {
                FavoriteCharacters = students.Select(student => new StudentEntity(student)).ToList()
            };
        }
        //Get a student index
        [HttpGet("{index:int}")]
        public IActionResult GetOne(int index)
        {
            if (index < 0 || index >= students.Count)
            {
                return Content($"Invalid index: {index}. Must be between 0 and {students.Count - 1}.", "text/plain", System.Text.Encoding.UTF8);
            }

            var studentEntity = new StudentEntity(students[index]);
            return Json(studentEntity);
        }
        //Get a student index's views
        [HttpGet("{index:int}/views")]
        public IActionResult GetViews(int index)
        {
            Console.WriteLine(index);
            if (index < 0 || index >= students.Count)
            {
                return Content($"Invalid index: {index}. Must be between 0 and {students.Count - 1}.", "text/plain", System.Text.Encoding.UTF8);
            }

            var studentEntity = students[index].ViewDates;
            return Json(studentEntity);
        }
        //Post to student index
        [HttpPost]
        public StudentEntity Post(StudentEntity studentEntity)
        {
            if (students.Count >= 30) {
                students.Clear();
            }
            students.Add(studentEntity.ToModel());
            
            return studentEntity;
        }
        //Post to student index's views
        [HttpPost("{index:int}/views")]
        public IActionResult PostViews(int index, [FromBody] ViewEntity? newView)
        {
            if (index < 0 || index >= students.Count)
            {
                return Content($"Invalid index: {index}. Must be between 0 and {students.Count - 1}.", "text/plain", System.Text.Encoding.UTF8);
            }

            if (newView == null || string.IsNullOrWhiteSpace(newView.ViewDate))
            {
                return Content("ViewDate is required and cannot be empty.", "text/plain", System.Text.Encoding.UTF8);
            }


            // Add the new view date to the student's list
            students[index].ViewDates.Add(newView.ViewDate);

            // Return the updated list of views for this student
            return Json(students[index].ViewDates);
        }

        //Put student index
        [HttpPut("{index:int}")]
        public IActionResult Put(int index, StudentEntity studentEntity)
        {
            if(index < 0 || index >= students.Count)
            {
                return Content($"Invalid index: {index}. Must be between 0 and {students.Count - 1}.", "text/plain", System.Text.Encoding.UTF8);
            }
            students[index] = studentEntity.ToModel();

            return Json(studentEntity);
        }
        //Delete student index
        [HttpDelete("{index:int}")]
        public IActionResult Delete(int index)
        {
            if (index < 0 || index >= students.Count)
            {
                return Content($"Invalid index: {index}. Must be between 0 and {students.Count - 1}.", "text/plain", System.Text.Encoding.UTF8);
            }
            students.RemoveAt(index);

            return Content("Student successfully deleted.", "text/plain", System.Text.Encoding.UTF8);
        }
        //Return View index
        [HttpGet("{studentIndex:int}/views/{viewIndex:int}")]
        public IActionResult GetSingleView(int studentIndex, int viewIndex)
        {
            // Validate student index
            if (studentIndex < 0 || studentIndex >= students.Count)
            {
                return Content(
                    $"Invalid student index: {studentIndex}. Must be between 0 and {students.Count - 1}.",
                    "text/plain", System.Text.Encoding.UTF8
                );
            }

            var student = students[studentIndex];

            // Validate view index
            if (viewIndex < 0 || viewIndex >= student.ViewDates.Count)
            {
                return Content(
                    $"Invalid view index: {viewIndex}. Must be between 0 and {student.ViewDates.Count - 1} for student '{student.FirstName} {student.LastName}'.",
                    "text/plain", System.Text.Encoding.UTF8
                );
            }

            // Return a well-formatted JSON object for this view
            var result = new
            {
                StudentIndex = studentIndex,
                StudentName = $"{student.FirstName} {student.LastName}",
                ViewIndex = viewIndex,
                ViewDate = student.ViewDates[viewIndex]
            };

            return Json(result);
        }
        //Patch student index
        [HttpPatch("{index:int}")]
        public IActionResult Patch(int index, [FromBody] StudentPatchEntity studentPatchEntity)
        {
            // Validate index
            if (index < 0 || index >= students.Count)
            {
                return Content($"Invalid student index: {index}. Must be between 0 and {students.Count - 1}.", "text/plain", System.Text.Encoding.UTF8);
            }

            // Apply the updates only for non-null and non-empty fields
            if (!string.IsNullOrWhiteSpace(studentPatchEntity.FirstName))
            {
                students[index].FirstName = studentPatchEntity.FirstName;
            }

            if (!string.IsNullOrWhiteSpace(studentPatchEntity.LastName))
            {
                students[index].LastName = studentPatchEntity.LastName;
            }

            if (!string.IsNullOrWhiteSpace(studentPatchEntity.FavoriteCharacter))
            {
                students[index].FavoriteCharacter = studentPatchEntity.FavoriteCharacter;
            }

            // Return the updated student entity
            var updatedStudent = new StudentEntity(students[index]);
            return Json(updatedStudent);
        }


    }
}
