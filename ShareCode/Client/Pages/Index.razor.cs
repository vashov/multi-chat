using Microsoft.AspNetCore.Components;

namespace ShareCode.Client.Pages
{
    public partial class Index
    {
        [Inject]
        private NavigationManager Navigator { get; set; }

        public void GoToCreateRoom()
        {
            Navigator.NavigateTo("/room/create");
        }
    }
}
