using System;
using System.Reflection;
using System.Windows.Input;
using Xamarin.Forms;

namespace StudyBuddy
{
    public class ChatPageEditorBehaviour : Behavior<Editor>
    {

        public static readonly BindableProperty CommandProperty = BindableProperty.Create("Command", typeof(ICommand), typeof(ChatPageEditorBehaviour), null);

        public Editor AssociatedObject { get; private set; }

        protected override void OnAttachedTo(Editor bindable)
        {
            base.OnAttachedTo(bindable);

            AssociatedObject = bindable;

            bindable.BindingContextChanged += Bindable_BindingContextChanged;
            bindable.Completed += Bindable_Completed;
        }
        protected override void OnDetachingFrom(Editor bindable)
        {
            base.OnDetachingFrom(bindable);

            bindable.BindingContextChanged -= Bindable_BindingContextChanged;
            bindable.Completed -= Bindable_Completed;

            AssociatedObject = null;
        }

        private void Bindable_BindingContextChanged(object sender, System.EventArgs e)
        {
            OnBindingContextChanged();
        }

        private void Bindable_Completed(object sender, System.EventArgs e)
        {
            if (Command == null) return;

            //object parameter = Converter.Convert(e, typeof(object), null, null);
            if (Command.CanExecute(e))
            {
                Command.Execute(e);
            }

        }

        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            BindingContext = AssociatedObject.BindingContext;
        }

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
    }
}