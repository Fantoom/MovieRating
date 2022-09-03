using Microsoft.EntityFrameworkCore;
using X.PagedList;

namespace MovieRating.Dal.Data
{
    public class AsyncPagedList<T> : BasePagedList<T>
    {
        public static async Task<AsyncPagedList<T>> CreateAsync(IQueryable<T> superset, int pageNumber, int pageSize)
        {
            var pagedList = new AsyncPagedList<T>();
            if (pageNumber < 1)
                throw new ArgumentOutOfRangeException(nameof(pageNumber), pageNumber, "PageNumber cannot be below 1.");
            if (pageSize < 1)
                throw new ArgumentOutOfRangeException(nameof(pageSize), pageSize, "PageSize cannot be less than 1.");

            // set source to blank list if superset is null to prevent exceptions
            pagedList.TotalItemCount = superset == null ? 0 : await superset.CountAsync();
            pagedList.PageSize = pageSize;
            pagedList.PageNumber = pageNumber;
            pagedList.PageCount = pagedList.TotalItemCount > 0
                        ? (int)Math.Ceiling(pagedList.TotalItemCount / (double)pagedList.PageSize)
                        : 0;
            pagedList.HasPreviousPage = pagedList.PageNumber > 1;
            pagedList.HasNextPage = pagedList.PageNumber < pagedList.PageCount;
            pagedList.IsFirstPage = pagedList.PageNumber == 1;
            pagedList.IsLastPage = pagedList.PageNumber >= pagedList.PageCount;
            pagedList.FirstItemOnPage = (pagedList.PageNumber - 1) * pagedList.PageSize + 1;
            var numberOfLastItemOnPage = pagedList.FirstItemOnPage + pagedList.PageSize - 1;
            pagedList.LastItemOnPage = numberOfLastItemOnPage > pagedList.TotalItemCount
                            ? pagedList.TotalItemCount
                            : numberOfLastItemOnPage;

            // add items to internal list
            if (superset != null && pagedList.TotalItemCount > 0)
                pagedList.Subset.AddRange(pageNumber == 1
                    ? await superset.Skip(0).Take(pageSize).ToListAsync()
                    : await superset.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync()
                );
            return pagedList;
        }
    }
}
