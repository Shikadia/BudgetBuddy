using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using Microsoft.EntityFrameworkCore;
using ILogger = Serilog.ILogger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using BudgetBuddy.Domain.Models;
using BudgetBuddy.Core.Utilities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using BudgetBuddy.Domain.Enums;

namespace BudgetBuddy.Core.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        public TransactionService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager, ILogger logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<ResponseDto<TransactionResponseDTO>> AddTransaction(int pageSize, int pageNumber, string id, TransactionRequestDTO requestDTO)
        {
            try
            {
                DateTime validDate;
                decimal amount;
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.Error("Invalid user id was sent to Add Transaction");
                    return ResponseDto<TransactionResponseDTO>.Fail("An Error occured, we are on it", (int)HttpStatusCode.BadRequest);
                }

                if (requestDTO == null)
                {
                    _logger.Error("Invalid user id was sent to Add Transaction, Dto was null");
                    return ResponseDto<TransactionResponseDTO>.Fail("An Error occured, we are on it", (int)HttpStatusCode.BadRequest);
                }

                if (!decimal.TryParse(requestDTO.Amount.ToString(), out amount))
                {

                    _logger.Error("Invalid Amount, amount should be of type decimal");
                    return ResponseDto<TransactionResponseDTO>.Fail("Input a valid amount of type: decimal", (int)HttpStatusCode.BadRequest);
                }

                if (!DateTime.TryParse(requestDTO.DateTime.ToString(), out validDate))
                {
                    _logger.Error("Invalid DateTime format at AddTransaction");
                    return ResponseDto<TransactionResponseDTO>.Fail("Input a valid Date", (int)HttpStatusCode.BadRequest);
                }
                if (amount < 0)
                {
                    _logger.Error("Invalid Amount, amount should not be less then Zero");
                    return ResponseDto<TransactionResponseDTO>.Fail("Ma guy how nah, who give you negative amount", (int)HttpStatusCode.BadRequest);
                }
                if (requestDTO.Type.ToUpperInvariant() == TransactionType.INCOME.ToString())
                {
                    var income = new Income
                    {
                        Description = requestDTO.Description,
                        Tag = requestDTO.Tag,
                        Amount = amount,
                        AppUserID = user.Id,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = validDate
                    };

                    await _unitOfWork.IncomeRepository.InsertAsync(income);
                    user.Balance += amount;
                    await _userManager.UpdateAsync(user);
                    await _unitOfWork.Save();

                    return await GetAllTransactions(pageSize, pageNumber, id);
                }

                if (amount > user.Balance || user.Balance - amount < 0)
                {
                    _logger.Error("Niggar you broke");
                    return ResponseDto<TransactionResponseDTO>.Fail("Make I tell you the truth, you no get money", (int)HttpStatusCode.BadRequest);
                }

                if (requestDTO.Type.ToUpperInvariant() == TransactionType.EXPENSE.ToString())
                {
                    var expense = new Expense
                    {
                        Description = requestDTO.Description ?? "",
                        Tag = requestDTO.Tag,
                        Amount = amount,
                        AppUSerID = user.Id,
                        UpdatedAt = DateTime.UtcNow,
                        CreatedAt = validDate
                    };

                    await _unitOfWork.ExpenseRepository.InsertAsync(expense);
                    user.Balance -= amount;
                    await _userManager.UpdateAsync(user);
                    await _unitOfWork.Save();

                    return await GetAllTransactions(pageSize, pageNumber, id);
                }
                _logger.Error("Something Went Wrong");
                return ResponseDto<TransactionResponseDTO>.Fail("An Error occured, we are on it", (int)HttpStatusCode.InternalServerError);
            }
            catch (Exception ex)
            {
                _logger.Error("Something went wrong");
                return ResponseDto<TransactionResponseDTO>.Fail(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }

        public async Task<ResponseDto<TransactionResponseDTO>> GetAllTransactions(int pageSize, int pageNumber, string id)
        {
            try
            {
                var user = await _userManager.Users.Include(i => i.Incomes)
                    .Include(e => e.Expenses)
                    .SingleOrDefaultAsync(x => x.Id == id);

                if (user == null)
                {
                    _logger.Error("Invalid user id was sent to get all transactions");
                    return ResponseDto<TransactionResponseDTO>.Fail("An Error occured, we are on it", (int)HttpStatusCode.BadRequest);
                }
                var incomes = user.Incomes ?? Enumerable.Empty<Income>();
                var expenses = user.Expenses ?? Enumerable.Empty<Expense>();

                var incomeDtoQuery = incomes.Select(income => new ListOfTransactions
                {
                    Id = income.Id,
                    Type = "Income",
                    Amount = income.Amount,
                    Date = income.CreatedAt,
                    CategoryOrTag =  "No Tag",
                    Description = income.Description,
                });

                var expenseDtoQuery = expenses.Select(expense => new ListOfTransactions
                {
                    Id = expense.Id,
                    Type = "Expense",
                    Amount = expense.Amount,
                    Date = expense.CreatedAt,
                    CategoryOrTag = expense.Tag ?? "unCategorized",
                    Description = expense.Description,
                });

                var allTransactionsQuery = incomeDtoQuery.Concat(expenseDtoQuery).OrderBy(x => x.Date).AsQueryable();

                var currentBalance = user.Balance ?? 0;

                var paginatedResult = await Paginator.PaginationAsync<ListOfTransactions, ListOfTransactions>
                           (allTransactionsQuery, pageSize, pageNumber, _mapper);

                var responseDto = new TransactionResponseDTO
                {
                    ListOfTransactions = paginatedResult,
                    TotalAmount = currentBalance,
                    TotalIncome = incomes.Sum(i => i.Amount),
                    TotalExpense = expenses.Sum(e => e.Amount),
                };

                _logger.Information($"Transactions successfully retrieved for {user.Email}");
                return ResponseDto<TransactionResponseDTO>.Success("Transactions retrieved", responseDto, (int)HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.Error("Omething went wrong");
                return ResponseDto<TransactionResponseDTO>.Fail(ex.Message, (int)HttpStatusCode.NotFound);
            }

        }
    }
}
