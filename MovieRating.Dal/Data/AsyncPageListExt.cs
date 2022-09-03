namespace MovieRating.Dal.Data
{
    public static class AsyncPageListExt
    {
        public static Task<AsyncPagedList<T>> ToAsyncPagedList<T>(this IQueryable<T> superset, int pageNumber, int pageSize)
        {
            return AsyncPagedList<T>.CreateAsync(superset, pageNumber, pageSize);
        }
    }
}
