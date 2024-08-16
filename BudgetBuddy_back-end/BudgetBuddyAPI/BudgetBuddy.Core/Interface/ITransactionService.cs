using BudgetBuddy.Core.DTOs;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BudgetBuddy.Core.Interface
{
    public interface ITransactionService
    {
        Task<ResponseDto<TransactionResponseDTO>> GetAllTransactions(int pageSize, int pageNumber, string id);
        Task<ResponseDto<TransactionResponseDTO>> AddTransaction(int pageSize, int pageNumber, string id, TransactionRequestDTO requestDTO);

    }
}
