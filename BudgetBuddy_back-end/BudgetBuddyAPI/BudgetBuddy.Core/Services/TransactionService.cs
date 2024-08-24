using AutoMapper;
using BudgetBuddy.Core.DTOs;
using BudgetBuddy.Core.Interface;
using BudgetBuddy.Core.Utilities;
using BudgetBuddy.Domain.Enums;
using BudgetBuddy.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;
using ILogger = Serilog.ILogger;

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

                if (!DateTime.TryParse(requestDTO.Date.ToString(), out validDate))
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
                        Description = requestDTO.Description.ToLowerInvariant(),
                        Tag = requestDTO.Tag.ToLowerInvariant(),
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
                        Description = requestDTO.Description.ToLowerInvariant() ?? "",
                        Tag = requestDTO.Tag.ToLowerInvariant() ?? "",
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

        public async Task<ResponseDto<EditTransactionResponseDTO>> EditTransaction(EditTransactionRequestDTO request, string userId)
        {
            try
            {
                DateTime validDate;
                decimal amount;
                var user = await _userManager.Users.Include(i => i.Incomes)
                    .Include(e => e.Expenses)
                    .SingleOrDefaultAsync(x => x.Id == userId);
                if (user == null)
                {
                    _logger.Error("Invalid user id was sent to Add Transaction");
                    return ResponseDto<EditTransactionResponseDTO>.Fail("An Error occured, we are on it", (int)HttpStatusCode.BadRequest);
                }
                var income = user.Incomes.FirstOrDefault(x => x.Id == request.id);
                var expense = user.Expenses.FirstOrDefault(x => x.Id == request.id);
                if (income == null && expense == null)
                {
                    _logger.Error($"transaction with id : {request.id} not found");
                    return ResponseDto<EditTransactionResponseDTO>.Fail("Transaction not found or not for loggedin user", (int)HttpStatusCode.BadRequest);
                }

                if (request == null)
                {
                    _logger.Error("Invalid user id was sent to Add Transaction, Dto was null");
                    return ResponseDto<EditTransactionResponseDTO>.Fail("An Error occured, we are on it", (int)HttpStatusCode.BadRequest);
                }

                if (!decimal.TryParse(request.Amount.ToString(), out amount))
                {

                    _logger.Error("Invalid Amount, amount should be of type decimal");
                    return ResponseDto<EditTransactionResponseDTO>.Fail("Input a valid amount of type: decimal", (int)HttpStatusCode.BadRequest);
                }

                if (!DateTime.TryParse(request.Date.ToString(), out validDate))
                {
                    _logger.Error("Invalid DateTime format at AddTransaction");
                    return ResponseDto<EditTransactionResponseDTO>.Fail("Input a valid Date", (int)HttpStatusCode.BadRequest);
                }
                if (amount < 0)
                {
                    _logger.Error("Invalid Amount, amount should not be less then Zero");
                    return ResponseDto<EditTransactionResponseDTO>.Fail("Ma guy how nah, who give you negative amount", (int)HttpStatusCode.BadRequest);
                }
                if (income != null)
                {
                    user.Balance -= income.Amount;
                }
                else
                {
                    user.Balance += expense.Amount;
                }
                if (request.Type.ToUpperInvariant() == TransactionType.INCOME.ToString())
                {
                    if (income != null)
                    {
                        user.Balance += request.Amount;

                        income.Amount = request.Amount;
                        income.Description = request.Description;
                        income.UpdatedAt = DateTimeOffset.Now;
                        income.CreatedAt = request.Date;
                        income.Tag = request.Tag;

                        await _userManager.UpdateAsync(user);
                        _unitOfWork.IncomeRepository.Update(income);
                        await _unitOfWork.Save();

                        var updateIncome = await _unitOfWork.IncomeRepository.GetIncomeByIdAsync(request.id);
                        if (updateIncome != null)
                        {
                            var response = new EditTransactionResponseDTO
                            {
                                Description = updateIncome.Description,
                                Amount = updateIncome.Amount,
                                CategoryOrTag = updateIncome.Tag,
                                Type = "Income",
                                Date = updateIncome.UpdatedAt.ToString("dd-MM-yyyy HH:mm"),
                            };
                            return ResponseDto<EditTransactionResponseDTO>.Success("Successfully updated transaction", response, (int)HttpStatusCode.OK);
                        }
                        _logger.Error("Error: updating income failed");
                        return ResponseDto<EditTransactionResponseDTO>.Fail("Error in updating", (int)HttpStatusCode.BadRequest);
                    }
                    else
                    {
                        user.Balance += request.Amount;

                        await _unitOfWork.ExpenseRepository.DeleteAsync(request.id);
                        var newIncome = new Income
                        {
                            Amount = request.Amount,
                            Description = request.Description,
                            UpdatedAt = DateTimeOffset.Now,
                            CreatedAt = request.Date,
                            Tag = request.Tag,
                            AppUserID = user.Id,
                        };
                        await _unitOfWork.IncomeRepository.InsertAsync(newIncome);
                        await _userManager.UpdateAsync(user);
                        await _unitOfWork.Save();

                        var updateIncome = await _unitOfWork.IncomeRepository.GetIncomeByIdAsync(newIncome.Id);
                        if (updateIncome != null)
                        {
                            var response = new EditTransactionResponseDTO
                            {
                                Description = updateIncome.Description,
                                Amount = updateIncome.Amount,
                                CategoryOrTag = updateIncome.Tag,
                                Type = "Income",
                                Date = updateIncome.UpdatedAt.ToString("dd-MM-yyyy HH:mm"),
                            };
                            return ResponseDto<EditTransactionResponseDTO>.Success("Successfully updated transaction", response, (int)HttpStatusCode.OK);
                        }
                        _logger.Error("Error: updating income failed");
                        return ResponseDto<EditTransactionResponseDTO>.Fail("Error in updating", (int)HttpStatusCode.BadRequest);
                    }

                }


                if (request.Type.ToUpperInvariant() == TransactionType.EXPENSE.ToString())
                {
                    if (amount > user.Balance || user.Balance - amount < 0)
                    {
                        _logger.Error("Niggar you were broke  at that point");
                        return ResponseDto<EditTransactionResponseDTO>.Fail("Broke people can not update what dey never had", (int)HttpStatusCode.BadRequest);
                    }
                    if (expense != null)
                    {
                        user.Balance -= request.Amount;

                        expense.Amount = request.Amount;
                        expense.Description = request.Description;
                        expense.UpdatedAt = DateTimeOffset.Now;
                        expense.CreatedAt = request.Date;
                        expense.Tag = request.Tag;

                        _unitOfWork.ExpenseRepository.Update(expense);
                        await _userManager.UpdateAsync(user);
                        await _unitOfWork.Save();

                        var updateExpense = await _unitOfWork.ExpenseRepository.GetExpenseByIdAsync(request.id);
                        if (updateExpense != null)
                        {
                            var response = new EditTransactionResponseDTO
                            {
                                Description = updateExpense.Description,
                                Amount = updateExpense.Amount,
                                CategoryOrTag = updateExpense.Tag,
                                Type = "Expense",
                                Date = updateExpense.UpdatedAt.ToString("dd-MM-yyyy HH:mm"),
                            };
                            return ResponseDto<EditTransactionResponseDTO>.Success("Successfully updated transaction", response, (int)HttpStatusCode.OK);
                        }
                        _logger.Error("Error: updating Expense failed");
                        return ResponseDto<EditTransactionResponseDTO>.Fail("Error in updating", (int)HttpStatusCode.BadRequest);


                    }
                    else
                    {

                        user.Balance -= request.Amount;

                        await _unitOfWork.IncomeRepository.DeleteAsync(request.id);

                        var newExpense = new Expense
                        {
                            Amount = request.Amount,
                            Description = request.Description,
                            UpdatedAt = DateTimeOffset.Now,
                            CreatedAt = request.Date,
                            Tag = request.Tag,
                            AppUSerID = user.Id,
                        };
                        await _unitOfWork.ExpenseRepository.InsertAsync(newExpense);
                        await _userManager.UpdateAsync(user);
                        await _unitOfWork.Save();

                        var updateExpense = await _unitOfWork.ExpenseRepository.GetExpenseByIdAsync(newExpense.Id);
                        if (updateExpense != null)
                        {
                            var response = new EditTransactionResponseDTO
                            {
                                Description = updateExpense.Description,
                                Amount = updateExpense.Amount,
                                CategoryOrTag = updateExpense.Tag,
                                Type = "Expense",
                                Date = updateExpense.UpdatedAt.ToString("dd-MM-yyyy HH:mm"),
                            };
                            return ResponseDto<EditTransactionResponseDTO>.Success("Successfully updated transaction", response, (int)HttpStatusCode.OK);
                        }
                        _logger.Error("Error: updating Expense failed");
                        return ResponseDto<EditTransactionResponseDTO>.Fail("Error in updating", (int)HttpStatusCode.BadRequest);

                    }
                }
                _logger.Error("Something Went Wrong");
                return ResponseDto<EditTransactionResponseDTO>.Fail("An Error occured, we are on it", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error("Something went wrong");
                return ResponseDto<EditTransactionResponseDTO>.Fail(ex.Message, (int)HttpStatusCode.InternalServerError);
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
                    Date = income.CreatedAt.ToString("dd-MM-yyyy HH:mm"),
                    CategoryOrTag = income.Tag ?? "No Tag",
                    Description = income.Description,
                });

                var expenseDtoQuery = expenses.Select(expense => new ListOfTransactions
                {
                    Id = expense.Id,
                    Type = "Expense",
                    Amount = expense.Amount,
                    Date = expense.CreatedAt.ToString("dd-MM-yyyy HH:mm"),
                    CategoryOrTag = expense.Tag ?? "unCategorized",
                    Description = expense.Description,
                });

                var allTransactionsQuery = incomeDtoQuery.Concat(expenseDtoQuery).OrderBy(x => x.Date).AsQueryable();

                var currentBalance = user.Balance ?? 0;

                //var paginatedResult = await Paginator.PaginationAsync<ListOfTransactions, ListOfTransactions>
                //           (allTransactionsQuery, pageSize, pageNumber, _mapper);

                var responseDto = new TransactionResponseDTO
                {
                    ListOfTransactions = allTransactionsQuery.ToList(),
                    TotalAmount = currentBalance,
                    TotalIncome = incomes.Sum(i => i.Amount),
                    TotalExpense = expenses.Sum(e => e.Amount),
                };

                _logger.Information($"Transactions successfully retrieved for {user.Email}");
                return ResponseDto<TransactionResponseDTO>.Success("Transactions retrieved", responseDto, (int)HttpStatusCode.OK);

            }
            catch (Exception ex)
            {
                _logger.Error("Something went wrong");
                return ResponseDto<TransactionResponseDTO>.Fail(ex.Message, (int)HttpStatusCode.NotFound);
            }

        }

        public async Task<ResponseDto<string>> ResetTransactionsBalance(string userId)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(userId);

                if (user == null)
                {
                    _logger.Error("User not found");
                    return ResponseDto<string>.Fail("user not found", (int)HttpStatusCode.NotFound);
                }
                var incomes = _unitOfWork.IncomeRepository.GetAllIncomeAsync(userId);
                var expenses = _unitOfWork.ExpenseRepository.GetAllExpensesAsync(userId);
                _unitOfWork.IncomeRepository.DeleteRangeAsync(incomes);
                _unitOfWork.ExpenseRepository.DeleteRangeAsync(expenses);

                user.Balance = 0;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    await _unitOfWork.Save();

                    return ResponseDto<String>.Success("Reset completed", "Reset", (int)HttpStatusCode.OK);
                }
                _logger.Error("Error while reseting");
                return ResponseDto<string>.Fail("Error while reseting", (int)HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                _logger.Error("something went wrong");
                return ResponseDto<string>.Fail(ex.Message, (int)HttpStatusCode.InternalServerError);
            }
        }
    }
}
