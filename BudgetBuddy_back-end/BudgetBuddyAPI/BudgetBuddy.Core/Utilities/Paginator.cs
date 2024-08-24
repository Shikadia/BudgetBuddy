using AutoMapper;
using BudgetBuddy.Core.DTOs;

namespace BudgetBuddy.Core.Utilities
{
    public static class Paginator
    {
        public static async Task<PaginationResult<IEnumerable<TDestination>>> PaginationAsync<TSource, TDestination>(this IQueryable<TSource> queryable,
            int pageSize, int pageNumber, IMapper mapper)
            where TSource : class
            where TDestination : class
        {
            var count = queryable.Count();

            if (count == 0)
            {
                return new PaginationResult<IEnumerable<TDestination>>
                {
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    PreviousPage = 0,
                    NumberOfPages = 0,
                    PageItems = Enumerable.Empty<TDestination>(),
                    TotalTransactions = 0,
                };
            }

            var pageResult = new PaginationResult<IEnumerable<TDestination>>
            {
                PageSize = (pageSize > 10 || pageNumber < 1) ? count : pageSize,
                CurrentPage = pageNumber > 1 ? pageNumber : 1,
                PreviousPage = pageNumber > 0 ? pageNumber - 1 : 0,
                TotalTransactions =  count,
            };

            pageResult.NumberOfPages = count % pageResult.PageSize != 0
                ? count / pageResult.PageSize + 1
                : count / pageResult.PageSize;
           var sourceList = queryable.Skip((pageResult.CurrentPage - 1) * pageResult.PageSize).Take(pageResult.PageSize).ToList();
            var destinationList = mapper.Map<IEnumerable<TDestination>>(sourceList);
            pageResult.PageItems = destinationList;
            return pageResult;
        }
    }
}
