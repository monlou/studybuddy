using System;
using Xamarin.Forms;

namespace StudyBuddy.Views
{
    public partial class ChatPage : ContentPage
    {

        public string CategoryInput = "Question";

        public ChatPage()
        {
            InitializeComponent();
        }

        void OnPickerSelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;

            if (selectedIndex != -1)
            {
                CategoryInput = picker.Items[selectedIndex];
            }
        }

    }
}
