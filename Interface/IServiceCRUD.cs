using System.Collections.Generic;
using System.Threading.Tasks;
using smarthome.Model;

namespace smarthome.Interface
{
    public interface IServiceCRUD<T>
    {
        ServiceResponse<List<T>> GetAll();
        Task<ServiceResponse<bool>> Add(T _entry);
        Task<ServiceResponse<bool>> Edit(T _entry);
        Task<ServiceResponse<T>> Remove(int id);
    }
}