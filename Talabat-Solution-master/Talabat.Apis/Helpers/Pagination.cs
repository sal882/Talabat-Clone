namespace Talabat.Apis.Helpers
{
    /// <summary>
    /// this class represnt the default Represintaion to all end point that getAll
    /// </summary>
    /// <typeparam name="T">generic to any controller</typeparam>
    public class Pagination<T>
    {
        public int PageSize { get; set; }
        public int PageIndex { get; set; }
        public int Count { get; set; }
        public IReadOnlyList<T> Data { get; set; }

        public Pagination(int pageSize, int pageIndex, int count, IReadOnlyList<T> data)
        {
            PageSize = pageSize;
            PageIndex = pageIndex;
            Count = count;
            Data = data;
        }
    }
}
