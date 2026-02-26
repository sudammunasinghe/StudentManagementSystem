using SendGrid.Helpers.Errors.Model;
using StudentManagementSystem.Application.DTOs.Course;
using StudentManagementSystem.Application.DTOs.CourseContent;
using StudentManagementSystem.Application.DTOs.Instructor;
using StudentManagementSystem.Application.Interfaces.IRepositories;
using StudentManagementSystem.Application.Interfaces.IServices;

namespace StudentManagementSystem.Application.Services
{
    public class InstructorService : IInstructorService
    {
        private readonly IInstructorRepository _instructorRepository;
        public InstructorService(IInstructorRepository instructorRepository)
        {
            _instructorRepository = instructorRepository;
        }

        public async Task<InstructorResponseDto> GetInstructorDetailsByInstructorIdAsync(int instructorId)
        {
            var instructor =
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(instructorId);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

            var ownCourses =
                await _instructorRepository.GetOwnCoursesByInstructorIdAsync(instructorId);

            var ownCourseContents =
                await _instructorRepository.GetCourseContentByInstructorIdAsync(instructorId);

            return new InstructorResponseDto
            {
                InstructorId = instructor.Id,
                OwnCourses = ownCourses.Select(oc => new CourseDto
                {
                    CourseId = oc.Id,
                    Title = oc.Title,
                    Credits = oc.Credits,
                    CourseContents = ownCourseContents
                        .Where(occ => occ.CourseId == oc.Id)
                        .Select(occ => new CourseContentDto
                        {
                            ContentId = occ.Id,
                            Title = occ.Title,
                            Description = occ.Description,
                            ContentType = occ.ContentType,
                            FileSize = occ.FileSize
                        }).ToList()

                }).ToList()
            };
        }

        public async Task<IEnumerable<InstructorResponseDto>> GetAllInstructorsAsync()
        {
            var instructors =
                await _instructorRepository.GetAllInstructorsAsync();

            return instructors.Select(i => new InstructorResponseDto
            {
                InstructorId = i.Id,
            }).ToList();
        }

        public async Task<int> CreateInstructorAsync(CreateInstructorDto dto)
        {
            //var newInstructor = Instructor.Create(
            //    dto.FirstName,
            //    dto.LastName,
            //    dto.NIC,
            //    dto.Address);

            //return await _instructorRepository.CreateInstructorAsync(newInstructor);
            return 1;
        }

        public async Task<InstructorResponseDto> UpdateInstructorDetailsAsync(UpdateInstructorDto dto)
        {
            var instructor =
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(dto.Id);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

            instructor.Update(
                1, 2
               );

            var affectedRows =
                await _instructorRepository.UpdateInstructorDetailsAsync(instructor);

            if (affectedRows == 0)
                throw new Exception("Instructor update failed ...");

            return new InstructorResponseDto
            {
                InstructorId = instructor.Id
            };
        }

        public async Task InactivateInstructorByInstructorIdAsync(int instructorId)
        {
            var instructor =
                await _instructorRepository.GetInstructorDetailsByInstructorIdAsync(instructorId);

            if (instructor == null)
                throw new NotFoundException("Instructor not found ...");

            var affectedRows =
                await _instructorRepository.InactivateInstructorByInstructorIdAsync(instructorId);

            if (affectedRows == 0)
                throw new Exception("Instructor inactivation failed...");
        }
    }
}
