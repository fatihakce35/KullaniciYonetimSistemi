using KayitSistemiApi.Models;
using KayitSistemiApi.Models.Dtos;
using KayitSistemiApi.Models.ViewModel;
using KayitSistemiApi.Repository.Abstraction;
using KayitSistemiApi.Service.Abstraction;

namespace KayitSistemiApi.Service
{
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;
        private readonly IGenericRepository<User> _repository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserService(IGenericRepository<User> repository, ILogger<UserService> logger, IHttpContextAccessor httpContextAccessor)
        {
            _repository = repository;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public ServiceResult<UserDTO> Add(UserViewModel userViewModel)
        {
            var serviceResult = new ServiceResult<UserDTO>();

            try
            {
                var user = new User
                {
                    Name = userViewModel.Name,
                    Email = userViewModel.Email,
                    Password = userViewModel.Password, 
                    ProfilePicturePath = SaveProfilePicture(userViewModel.ProfilePicture)
                };

                _repository.Save(user);

                serviceResult.IsSuccess = true;
                serviceResult.Data = new UserDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    ProfilePicture = userViewModel.ProfilePicture.FileName
                };
                serviceResult.StatusCode = 201;
                serviceResult.Message = "User successfully added.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding user.");
                serviceResult.IsSuccess = false;
                serviceResult.Message = "Error adding user.";
                serviceResult.StatusCode = 500;
            }

            return serviceResult;
        }

        public ServiceResult<bool> Delete(int id)
        {
            var serviceResult = new ServiceResult<bool>();

            try
            {
                var user = _repository.FindById(id);
                if (user != null)
                {
                    _repository.Delete(user);
                    serviceResult.IsSuccess = true;
                    serviceResult.Data = true;
                    serviceResult.Message = "User successfully deleted.";
                    serviceResult.StatusCode = 200;
                }
                else
                {
                    serviceResult.IsSuccess = false;
                    serviceResult.Data = false;
                    serviceResult.Message = "User not found.";
                    serviceResult.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting user.");
                serviceResult.IsSuccess = false;
                serviceResult.Data = false;
                serviceResult.Message = "Error deleting user.";
                serviceResult.StatusCode = 500;
            }

            return serviceResult;
        }

        public ServiceResult<ICollection<UserDTO>> GetAll()
        {
            var serviceResult = new ServiceResult<ICollection<UserDTO>>();

            try
            {
                var users = _repository.GetAll();
                var userDTOs = users.Select(u => new UserDTO
                {
                    Id = u.Id,
                    Name = u.Name,
                    Email = u.Email,
                    Password = u.Password,
                    ProfilePicture = ConvertToFullUrl(u.ProfilePicturePath) 
                }).ToList();

                serviceResult.IsSuccess = true;
                serviceResult.Data = userDTOs;
                serviceResult.Message = "Users successfully retrieved.";
                serviceResult.StatusCode = 200;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving users.");
                serviceResult.IsSuccess = false;
                serviceResult.Message = "Error retrieving users.";
                serviceResult.StatusCode = 500;
            }

            return serviceResult;
        }

        public ServiceResult<UserDTO> GetById(int id)
        {
            var serviceResult = new ServiceResult<UserDTO>();

            try
            {
                var user = _repository.FindById(id);
                if (user != null)
                {
                    var userDTO = new UserDTO
                    {
                        Id = user.Id,
                        Name = user.Name,
                        Email = user.Email,
                        Password = user.Password,
                        ProfilePicture = null // ProfilePicture dosyalarını istemciye dönmek genellikle tavsiye edilmez
                    };

                    serviceResult.IsSuccess = true;
                    serviceResult.Data = userDTO;
                    serviceResult.Message = "User successfully retrieved.";
                    serviceResult.StatusCode = 200;
                }
                else
                {
                    serviceResult.IsSuccess = false;
                    serviceResult.Message = "User not found.";
                    serviceResult.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving user.");
                serviceResult.IsSuccess = false;
                serviceResult.Message = "Error retrieving user.";
                serviceResult.StatusCode = 500;
            }

            return serviceResult;
        }

        public ServiceResult<bool> Update(UserViewModel userViewModel)
        {
            var serviceResult = new ServiceResult<bool>();

            try
            {
                var user = _repository.FindById(userViewModel.Id);
                if (user != null)
                {
                    user.Name = userViewModel.Name;
                    user.Email = userViewModel.Email;
                    user.Password = userViewModel.Password; // Parolayı hashlemeyi unutmayın!
                    user.ProfilePicturePath = SaveProfilePicture(userViewModel.ProfilePicture);

                    _repository.Update(user);

                    serviceResult.IsSuccess = true;
                    serviceResult.Data = true;
                    serviceResult.Message = "User successfully updated.";
                    serviceResult.StatusCode = 200;
                }
                else
                {
                    serviceResult.IsSuccess = false;
                    serviceResult.Data = false;
                    serviceResult.Message = "User not found.";
                    serviceResult.StatusCode = 404;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user.");
                serviceResult.IsSuccess = false;
                serviceResult.Data = false;
                serviceResult.Message = "Error updating user.";
                serviceResult.StatusCode = 500;
            }

            return serviceResult;
        }

        private string SaveProfilePicture(IFormFile profilePicture)
        {
            if (profilePicture == null || profilePicture.Length == 0)
                return null;

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(profilePicture.FileName);

            var relativePath = Path.Combine("images", fileName);

            var filePath = Path.Combine("wwwroot", relativePath);

           
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                profilePicture.CopyTo(stream);
            }

            return relativePath.Replace("\\", "/");
        }

        private string ConvertToFullUrl(string relativePath)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            var baseUrl = $"{request.Scheme}://{request.Host}{request.PathBase}";
            return $"{baseUrl}/{relativePath.Replace("\\", "/").Replace("wwwroot","")}";
        }
    }

}
