using System;
using System.Collections.Generic;
using System.Linq;
using Tutorial.Web.Model;

namespace Tutorial.Web.Services
{
    public class InMemoryRepository : IRepository<Student>
    {
        private readonly List<Student> _student;

        public InMemoryRepository()
        {
            _student = new List<Student>
            {
                new Student
                {
                    Id=1,
                    FirstName="Nick",
                    LastName="Carter",
                    BirthDate=new DateTime(1999,1,5)
                },
                new Student
                {
                    Id=2,
                    FirstName="Kevin",
                    LastName="Richardson",
                    BirthDate=new DateTime(1999,2,5)
                },
                new Student
                {
                    Id=3,
                    FirstName="Howie",
                    LastName="DCarter",
                    BirthDate=new DateTime(1999,3,5)
                },
            };
        }
        public IEnumerable<Student> GetAll()
        {
            return _student;

        }

        public Student GetById(int id)
        {
            return _student.FirstOrDefault(x => x.Id == id);
        }

        public Student Add(Student newStudent)
        {
            var maxId = _student.Max(x => x.Id);
            newStudent.Id = maxId + 1;
            _student.Add(newStudent);
            return newStudent;
        }
    }
}
