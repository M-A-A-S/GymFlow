using GymFlow.Domain.DTOs.Common;

namespace GymFlow.WebUI.Extensions
{
    public static class QueryRequestExtensions
    {
        public static string GetSortIcon(this QueryRequest request, string column)
        {
            if (request.SortBy != column)
            {
                return "fa-sort";
            }

            return request.Descending ? "fa-sort-down" : "fa-sort-up";
        }

        public static bool ToggleDescending(this QueryRequest request, string column)
        {
            return request.SortBy == column ? !request.Descending : false;
        }

        public static Dictionary<string, string> ToRouteDictionary(this QueryRequest request)
        {
            return request
                .GetType()
                .GetProperties()
                .ToDictionary(
                    x => x.Name,
                    x => x.GetValue(request)?.ToString() ?? "");
        }

    }
}
