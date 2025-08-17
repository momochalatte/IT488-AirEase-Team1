using Android.Content;
using Android.Graphics;
using Android.Util;
using Android.Widget;

namespace IT488_AirEase_Team1.Controls
{
    public class PrimaryButton : Button
    {
        public PrimaryButton(Context context) : base(context)
        {
            Init();
        }

        public PrimaryButton(Context context, IAttributeSet attrs) : base(context, attrs)
        {
            Init();
        }

        private void Init()
        {
            SetBackgroundColor(Color.ParseColor("#007BFF"));
            SetTextColor(Color.White);
            SetPadding(40, 20, 40, 20);
            Typeface = Typeface.DefaultBold;
            TextSize = 18;
        }
    }
}
