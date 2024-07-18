using KayitSistemiApi.Models;
using KayitSistemiApi.Models.Dtos;
using KayitSistemiApi.Models.ViewModel;

namespace KayitSistemiApi.Service.Abstraction
{
    public interface IUserService
    {
        public ServiceResult<ICollection<UserDTO>> GetAll();
        public ServiceResult<UserDTO> GetById(int id);
        public ServiceResult<bool> Delete(int id);
        public ServiceResult<bool> Update(UserViewModel userDTO);
        public ServiceResult<UserDTO> Add(UserViewModel userDTO);

    }
}
