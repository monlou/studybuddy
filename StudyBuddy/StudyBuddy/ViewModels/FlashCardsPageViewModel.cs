using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StudyBuddy.ViewModels
{
	public class FlashCardsPageViewModel : BindableBase
	{
        public System.Windows.Input.ICommand EditorFABCommand { get; protected set; }

        public FlashCardsPageViewModel()
        {

        }
	}
}
