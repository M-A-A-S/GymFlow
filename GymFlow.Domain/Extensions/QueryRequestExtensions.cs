

using GymFlow.Domain.DTOs.Common;
using System.Globalization;

namespace GymFlow.Domain.Extensions
{
    public static class QueryRequestExtensions
    {
        public static string GetSortIcon(this BaseFilterDTO request, string column)
        {
            if (request.SortBy != column)
            {
                return "fa-sort";
            }

            return request.Descending ? "fa-sort-down" : "fa-sort-up";
        }

        public static bool ToggleDescending(this BaseFilterDTO request, string column)
        {
            return request.SortBy == column ? !request.Descending : false;
        }

        //public static Dictionary<string, string> ToRouteDictionary(this QueryRequest request)
        //{
        //    return request
        //        .GetType()
        //        .GetProperties()
        //        .ToDictionary(
        //            x => x.Name,
        //            x => x.GetValue(request)?.ToString() ?? "");
        //}

        //public static Dictionary<string, string> ToRouteDictionary<T>(
        //this T request)
        //where T : QueryRequest
        //{
        //    return request
        //        .GetType()
        //        .GetProperties()
        //        .ToDictionary(
        //            p => p.Name,
        //            p => p.GetValue(request)?.ToString() ?? ""
        //        );
        //}

        public static Dictionary<string, string> ToRouteDictionary<T>(
    this T request)
    where T : BaseFilterDTO
        {
            return request
                .GetType()
                .GetProperties()
                .Where(p => p.GetValue(request) != null)
                .ToDictionary(
                    p => p.Name,
                    p => Convert.ToString(
                            p.GetValue(request),
                            CultureInfo.InvariantCulture
                         )!
                );
        }

    }
}
