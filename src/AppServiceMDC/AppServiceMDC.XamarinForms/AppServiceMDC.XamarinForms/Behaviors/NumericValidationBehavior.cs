using Xamarin.Forms;

namespace AppServiceMDC.XamarinForms.Behaviors
{
    public class NumericValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += EntryOnTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= EntryOnTextChanged;
            base.OnDetachingFrom(entry);
        }

        private void EntryOnTextChanged(object sender, TextChangedEventArgs args)
        {
            decimal result;
            var isValid = decimal.TryParse(args.NewTextValue, out result);
            ((Entry) sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}