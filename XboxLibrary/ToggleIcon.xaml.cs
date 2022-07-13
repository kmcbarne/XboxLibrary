using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace XboxLibrary
{
    public sealed partial class ToggleIcon : UserControl, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public event VisualStateChangedEventHandler VisualStateChanged;

        public bool IsChecked
        {
            get { return (bool)GetValue(IsCheckedProperty); }
            set
            {
                if (!IsChecked)
                {
                    VisualStateManager.GoToState(this, "Checked", false);
                    SetValue(IsCheckedProperty, true);
                }
                else
                {
                    VisualStateManager.GoToState(this, "Unchecked", false);
                    SetValue(IsCheckedProperty, false);
                }

                SetValue(IsCheckedProperty, value); 
            }
        }

        public static readonly DependencyProperty IsCheckedProperty =
            DependencyProperty.Register("IsChecked", typeof(bool), typeof(ToggleIcon), new PropertyMetadata(false, IsChecked_Changed));

        private static void IsChecked_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //if ((bool)e.NewValue == false)
            //{
            //    VisualStateManager.GoToState((Control)d, "Unchecked", false);
            //}
            //else
            //{
            //    VisualStateManager.GoToState((Control)d, "Checked", false);
            //}
        }

        public Uri UncheckedImage
        {
            get { return (Uri)GetValue(UncheckedImageProperty); }
            set { SetValue(UncheckedImageProperty, value); }
        }

        public static readonly DependencyProperty UncheckedImageProperty =
            DependencyProperty.Register("UncheckedImage", typeof(Uri), typeof(ToggleIcon), new PropertyMetadata(new Uri("ms-appx:///Assets/default-unchecked.png")));

        public Uri CheckedImage
        {
            get { return (Uri)GetValue(CheckedImageProperty); }
            set { SetValue(CheckedImageProperty, value); }
        }

        public static readonly DependencyProperty CheckedImageProperty =
            DependencyProperty.Register("CheckedImage", typeof(Uri), typeof(ToggleIcon), new PropertyMetadata(new Uri("ms-appx:///Assets/default-checked.png")));


        public ToggleIcon()
        {
            this.InitializeComponent();
            VisualStateChanged += StateChanged;
            DataContext = this;
        }

        private void StateChanged(object sender, VisualStateChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void Flip(ref ToggleIcon toggle)
        {
            if (!IsChecked)
            {
                VisualStateManager.GoToState(toggle, "Checked", false);
                SetValue(IsCheckedProperty, true);
            }
            else
            {
                VisualStateManager.GoToState(toggle, "Unchecked", false);
                SetValue(IsCheckedProperty, false);
            }
        }

        public static void FlipIsChecked(ref ToggleIcon toggle)
        {
            toggle.Flip(ref toggle);            
        }

        private void Border_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {            
            if (!IsChecked)
            {
                VisualStateManager.GoToState(this, "Checked", false);
                SetValue(IsCheckedProperty, true);
            }
            else
            {
                VisualStateManager.GoToState(this, "Unchecked", false);
                SetValue(IsCheckedProperty, false);
            }
        }
    }
}
