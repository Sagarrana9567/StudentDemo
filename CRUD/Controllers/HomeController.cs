using CRUD.Entity;
using CRUD.Repository.Student;
using CRUD.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace CRUD.Controllers
{
    public class HomeController : Controller
    {
        private IStudentRepository _studentRepository;
        private readonly StudDBEntities _db;
        public HomeController()
        {
            this._studentRepository = new StudentRepository(new StudDBEntities());

        }
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> GetStudentData()
        {
            JsonResult result = new JsonResult();
            try
            {
                string search = Request.Form.GetValues("search[value]")[0];
                string draw = Request.Form.GetValues("draw")[0];
                string order = Request.Form.GetValues("order[0][column]")[0];
                string orderDir = Request.Form.GetValues("order[0][dir]")[0];
                int startRec = Convert.ToInt32(Request.Form.GetValues("start")[0]);
                int pageSize = Convert.ToInt32(Request.Form.GetValues("length")[0]);

                var data = await _studentRepository.GetAllStudents(search, draw, order, orderDir, startRec, pageSize);
                List<StudentVM> studentData = data.ResponseData;
                var modifiedData = studentData.Select(d =>
                    new
                    {
                        d.Id,
                        d.Name,
                        d.Email,
                        d.PhoneNo,

                    }
                    );
                result = this.Json(new
                {
                    draw = Convert.ToInt32(draw),
                    recordsTotal = data.TotalRecord,
                    recordsFiltered = data.RecordFilter,
                    data = modifiedData
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Console.Write(ex);
            }
            return result;
        }



        [HttpGet]
        [Route("GetAllStates")]
        public async Task<JsonResult> GetAllStates()
        {
            return Json(await _studentRepository.GetAllStates(), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("GetCityByStateId")]
        public async Task<JsonResult> GetCityByStateId(int stateId)
        {
            return Json(await _studentRepository.GetCityByStateId(stateId), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Route("AddUpdateStudent")]
        public async Task<JsonResult> AddUpdateStudent(StudentVM studentVM)
        {
            return Json(await _studentRepository.AddUpdateStudents(studentVM), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("GetStudentInfoById")]
        public async Task<JsonResult> GetStudentInfoById(int id)
        {
            return Json(await _studentRepository.GetStudentInfoById(id), JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        [Route("DeleteStudent")]
        public async Task<JsonResult> DeleteStudent(int id)
        {
            return Json(await _studentRepository.DeleteStudent(id), JsonRequestBehavior.AllowGet);
        }

    }
}