namespace GymFlow.WebUI.ViewModels
{
    public class ImageInputVM
    {

        /// <summary>
        /// Current image (Edit page)
        /// </summary>
        public string? ExistingUrl { get; set; }

        /// <summary>
        /// User entered URL
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// Uploaded file
        /// </summary>
        public IFormFile? File { get; set; }

        // UI only
        public string Prefix { get; set; } = "Image";

    }
}
