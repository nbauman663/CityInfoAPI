namespace CityInfo.API.Services
{
    /// <summary>
    /// Object containing pagination metadata
    /// </summary>
    public class PaginationMetadata
    {
        /// <summary>
        /// Total count of items returned
        /// </summary>
        public int TotalItemCount { get; set; }

        /// <summary>
        /// Total number of pages after item split up by page size
        /// </summary>
        public int TotalPageCount { get; set; }

        /// <summary>
        /// Size of page
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Number of current page
        /// </summary>
        public int CurrentPage { get; set; }

        /// <summary>
        /// Constructor for PaginationMetadata
        /// </summary>
        /// <param name="totalItemCount">Total number of items returned</param>
        /// <param name="pageSize">Size of page</param>
        /// <param name="currentPage">Number of current page</param>
        public PaginationMetadata(int totalItemCount, int pageSize, int currentPage)
        {
            TotalItemCount = totalItemCount;
            PageSize = pageSize;
            CurrentPage = currentPage;
            TotalPageCount = (int)Math.Ceiling(totalItemCount / (double)pageSize);
        }   
    }
}
