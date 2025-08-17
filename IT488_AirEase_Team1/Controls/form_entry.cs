
using Android.Content;
using Android.Util;
using Android.Views;



namespace IT488_AirEase_Team1.Controls
{
    public class FormEntry : LinearLayout
    {
        private TextView _label = null!;
        private EditText _input = null!;
        
        public FormEntry(Context context) : base(context)
        {
            Init(context);
        
        }

        public FormEntry(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init(context);
        }

        private void Init(Context context)
        {
            
            var view = LayoutInflater.From(context)!.Inflate(Resource.Layout.form_entry, this, true);
            _label = view.FindViewById<TextView>(Resource.Id.label)!;
            _input = view.FindViewById<EditText>(Resource.Id.input)!;
        }

        public string Title
        {
            get => _label!.Text!;
            set => _label!.Text = value;
        }

        public string Text
        {
            get => _input!.Text!;
            set => _input!.Text = value;
        }

        public string Placeholder
        {
            get => _input!.Hint!;
            set => _input!.Hint = value;
        }
    }
}
