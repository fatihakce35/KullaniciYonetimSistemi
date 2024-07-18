using KayitSistemiApi.Models.Dtos;
using KayitSistemiApi.Models.ViewModel;
using KayitSistemiApi.Models;
using KayitSistemiApi.Service.Abstraction;
using Microsoft.AspNetCore.Mvc;

namespace KayitSistemiApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IUserService userService, ILogger<UserController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpPost]
        [Route("Add")]
        public ActionResult<ServiceResult<UserDTO>> Add([FromForm] UserViewModel userViewModel)
        {
            try
            {
                var result = _userService.Add(userViewModel);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user.");
                return StatusCode(500, new ServiceResult<UserDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while adding the user."
                });
            }
        }

        [HttpDelete]
        [Route("Delete/{id}")]
        public ActionResult<ServiceResult<bool>> Delete(int id)
        {
            try
            {
                var result = _userService.Delete(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user.");
                return StatusCode(500, new ServiceResult<bool>
                {
                    IsSuccess = false,
                    Message = "An error occurred while deleting the user."
                });
            }
        }

        [HttpGet]
        [Route("GetAll")]
        public ActionResult<ServiceResult<ICollection<UserDTO>>> GetAll()
        {
            try
            {
                var result = _userService.GetAll();
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users.");
                return StatusCode(500, new ServiceResult<ICollection<UserDTO>>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the users."
                });
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public ActionResult<ServiceResult<UserDTO>> GetById(int id)
        {
            try
            {
                var result = _userService.GetById(id);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user.");
                return StatusCode(500, new ServiceResult<UserDTO>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving the user."
                });
            }
        }

        [HttpPut]
        [Route("Update")]
        public ActionResult<ServiceResult<bool>> Update([FromForm] UserViewModel userViewModel)
        {
            try
            {
                var result = _userService.Update(userViewModel);
                return StatusCode(result.StatusCode, result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user.");
                return StatusCode(500, new ServiceResult<bool>
                {
                    IsSuccess = false,
                    Message = "An error occurred while updating the user."
                });
            }
        }
    }
}
