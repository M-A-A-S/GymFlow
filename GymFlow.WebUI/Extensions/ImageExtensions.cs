namespace GymFlow.WebUI.Extensions
{
    public static class ImageExtensions
    {

        public static string GetImageUrl(
            this string? image,
            string folder)
        {
            if (string.IsNullOrWhiteSpace(image))
            {
                return "/uploads/no-image.svg";
            }

            // External URL
            if (Uri.TryCreate(
                    image,
                    UriKind.Absolute,
                    out var uri)
                && (uri.Scheme == Uri.UriSchemeHttp ||
                    uri.Scheme == Uri.UriSchemeHttps))
            {
                return image;
            }


            // Uploaded file
            return $"/uploads/{folder}/{image}";
        }

    }
}
