using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace STEMotion.Application.Interfaces.ServiceInterfaces
{
    public interface IPaymentService
    {
        Task<bool> CheckUserPaymentAsync(Guid userId);
    }
}
