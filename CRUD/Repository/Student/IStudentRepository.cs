using CRUD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRUD.Repository.Student
{
    public interface IStudentRepository
    {
        Task<ApiResponse> GetAllStates();
        Task<ApiResponse> GetCityByStateId(int stateId);

        Task<ApiResponse> AddUpdateStudents(StudentVM studentVM);

        Task<ApiResponse> GetStudentInfoById(int id);

        Task<ApiResponse> DeleteStudent(int id);

        Task<ApiResponse> GetAllStudents(string search = null, string draw = null, string order = null, string orderDir = null, int startRec = 0, int pageSize = 10);
    }
}
