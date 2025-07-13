using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Application.DTOs.Purchase;

namespace VendingMachine.Application.Interfaces
{
    public interface IPurchaseService
    {
        Task DepositAsync(string userId, int coin);
        Task<PurchaseResponseDto> BuyAsync(string userId, int productId, int quantity);
        Task ResetDepositAsync(string userId);
    }
}
