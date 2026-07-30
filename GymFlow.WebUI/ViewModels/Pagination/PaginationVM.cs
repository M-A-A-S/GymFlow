namespace GymFlow.WebUI.ViewModels.Pagination
{
    public class PaginationVM
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalPages { get; set; }
        public int TotalCount { get; set; }

        // Current filters and sorting values
        // will be used to keep the URL state.
        public Dictionary<string, string> RouteValues { get; set; } = new();
    }
}
