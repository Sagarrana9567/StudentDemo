using CRUD.Entity;
using CRUD.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace CRUD.Repository.Student
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudDBEntities _db;
        public StudentRepository(StudDBEntities db)
        {
            this._db = db;
        }
        public async Task<ApiResponse> GetAllStates()
        {

            try
            {
                var getAllStates = await _db.Master_State.Where(s => s.IsActive == true).Select(s => new
                {
                    s.Id,
                    s.State
                }).ToListAsync();
                return new ApiResponse
                {
                    IsSuccess = true,
                    ResponseData = getAllStates,
                    Message = "Get state successfully."
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse> GetCityByStateId(int stateId)
        {

            try
            {
                var getAllCity = await _db.Master_City.Where(s => s.IsActive == true && s.StateId == stateId).ToListAsync();
                return new ApiResponse
                {
                    IsSuccess = true,
                    ResponseData = getAllCity,
                    Message = "Get city successfully."
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ApiResponse> AddUpdateStudents(StudentVM studentVM)
        {

            try
            {
                if (studentVM.Id == 0)
                {
                    _db.AddUpdateStudent(studentVM.Id, studentVM.Name, studentVM.Email, studentVM.PhoneNo, studentVM.Address, studentVM.StateId, studentVM.CityId);
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        Message = "Add student successfully."
                    };
                }
                else
                {
                    var getStudentInfo = await _db.Master_Student.FirstOrDefaultAsync(s => s.Id == studentVM.Id);
                    if (getStudentInfo != null)
                    {
                        _db.AddUpdateStudent(studentVM.Id, studentVM.Name, studentVM.Email, studentVM.PhoneNo, studentVM.Address, studentVM.StateId, studentVM.CityId);
                        return new ApiResponse
                        {
                            IsSuccess = true,
                            Message = "Update student successfully."
                        };
                    }
                    else
                    {
                        return new ApiResponse
                        {
                            IsSuccess = false,
                            Message = "Student not found."
                        };
                    }
                }
            }
            catch (Exception ex)
            {

                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse> GetStudentInfoById(int id)
        {

            try
            {
                var getStudentInfo = await _db.Master_Student.FirstOrDefaultAsync(s => s.IsActive == true && s.Id == id);
                return new ApiResponse
                {
                    IsSuccess = true,
                    ResponseData = getStudentInfo,
                    Message = "Get student successfully."
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        public async Task<ApiResponse> DeleteStudent(int id)
        {

            try
            {
                var getStudentInfo = await _db.Master_Student.FirstOrDefaultAsync(s => s.IsActive == true && s.Id == id);
                if (getStudentInfo == null)
                {
                    return new ApiResponse
                    {
                        IsSuccess = false,
                        Message = "Student not found."
                    };
                }
                else
                {
                    _db.Master_Student.Remove(getStudentInfo);
                    await _db.SaveChangesAsync();
                    return new ApiResponse
                    {
                        IsSuccess = true,
                        Message = "Delete student successfully."
                    };
                }

            }
            catch (Exception ex)
            {

                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse> GetAllStudents(string search = null, string draw = null, string order = null, string orderDir = null, int startRec = 0, int pageSize = 10)
        {

            try
            {
                List<StudentVM> data = await _db.Master_Student.Where(s => s.IsActive == true).Select(s => new StudentVM
                {
                    Id = s.Id,
                    Name = s.Name,
                    Email = s.Email,
                    PhoneNo = s.PhoneNo,

                }).ToListAsync();
                int totalRecords = data.Count;
                if (!string.IsNullOrEmpty(search) &&
                    !string.IsNullOrWhiteSpace(search))
                {
                    data = data.Where(p => p.Id.ToString().ToLower().Contains(search.ToLower()) ||
                        p.Name.ToString().Contains(search.ToLower()) ||
                        p.Email.ToString().Contains(search.ToLower()) ||
                        p.PhoneNo.ToString().Contains(search.ToLower())).ToList();
                }
                data = SortTableData(order, orderDir, data);
                int recFilter = data.Count;
                data = data.Skip(startRec).Take(pageSize).ToList();
                return new ApiResponse
                {
                    IsSuccess = true,
                    ResponseData = data,
                    TotalRecord = totalRecords,
                    RecordFilter = recFilter
                };
            }
            catch (Exception ex)
            {

                return new ApiResponse
                {
                    IsSuccess = false,
                    Message = ex.Message
                };
            }
        }
        private List<StudentVM> SortTableData(string order, string orderDir, List<StudentVM> data)
        {
            List<StudentVM> lst = new List<StudentVM>();
            try
            {
                switch (order)
                {
                    case "0":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Name).ToList()
                                                                                                 : data.OrderBy(p => p.Name).ToList();
                        break;
                    case "1":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Email).ToList()
                                                                                                 : data.OrderBy(p => p.Email).ToList();
                        break;
                    case "2":
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.PhoneNo).ToList()
                                                                                                 : data.OrderBy(p => p.PhoneNo).ToList();
                        break;

                    default:
                        lst = orderDir.Equals("DESC", StringComparison.CurrentCultureIgnoreCase) ? data.OrderByDescending(p => p.Id).ToList()
                                                                                                 : data.OrderBy(p => p.Id).ToList();
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return lst;
        }
    }
}