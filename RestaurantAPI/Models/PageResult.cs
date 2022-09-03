namespace RestaurantAPI.Models
{
    public class PageResult<T> 
    {
        public PageResult(List<T> items, int pageNumer, int pageSize, int totalItemsCount)
        {
            Items = items;
            TotalPages = (int)Math.Ceiling(totalItemsCount / (double)pageSize);
            ItemFrom = pageSize * (pageNumer - 1) + 1;
            ItemTo = ItemFrom + pageSize - 1;
            TotalItemsCount = totalItemsCount;
        }

        List<T> Items { get; set; }
        public int TotalPages { get; set; }
        public int ItemFrom { get; set; }
        public int ItemTo { get; set; }
        public int TotalItemsCount { get; set; }
    }
}
