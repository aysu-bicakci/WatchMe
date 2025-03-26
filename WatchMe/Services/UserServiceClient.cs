using System;
using System.ServiceModel;
using System.Threading.Tasks;
using System.ServiceModel.Description;
using System.ServiceModel.Channels;

namespace WatchMe.Services
{
    [ServiceContract]
    public interface IUserService
    {
        [OperationContract]
        Task<string> GetUserNameAsync(int userId);
    }

    public class UserServiceClient : ClientBase<IUserService>, IUserService
    {
        public UserServiceClient(Binding binding, EndpointAddress endpointAddress) 
            : base(binding, endpointAddress)
        {
        }

        public Task<string> GetUserNameAsync(int userId)
        {
            return Channel.GetUserNameAsync(userId=1); // SOAP üzerinden çağırma
        }
    }
}