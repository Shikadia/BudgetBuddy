using BudgetBuddy.Core.DTOs;

namespace BudgetBuddy.Core.Interface
{
    public interface ITransactionService
    {
        Task<ResponseDto<TransactionResponseDTO>> GetAllTransactions(int pageSize, int pageNumber, string id);
        Task<ResponseDto<TransactionResponseDTO>> AddTransaction(int pageSize, int pageNumber, string id, TransactionRequestDTO requestDTO);
        Task<ResponseDto<string>> ResetTransactionsBalance(string id);
        Task<ResponseDto<EditTransactionResponseDTO>> EditTransaction(EditTransactionRequestDTO request, string id);
    }
}
