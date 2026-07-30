namespace GymFlow.WebUI.ViewModels
{
    public class ActionButtonsVM
    {
        public int Id { get; set; }
        public string Controller { get; set; } = "";

        // Optional: hide buttons when needed
        public bool ShowEdit { get; set; } = true;
        public bool ShowDetails { get; set; } = true;
        public bool ShowDelete { get; set; } = true;
    }
}
