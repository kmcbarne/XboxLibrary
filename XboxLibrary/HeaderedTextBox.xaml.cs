using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace XboxLibrary
{
    /// <summary>
    /// A Control containing a Header section attached to the left of a TextBox.
    /// </summary>    
    public sealed partial class HeaderedTextBox : UserControl
    {
        /// <summary>
        /// Defines the Header DependencyProperty which contains the text inside the label segment of the HeaderedTextBox Control.
        /// </summary>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(HeaderedTextBox), new PropertyMetadata("X"));

        /// <summary>
        /// Defines the HeaderWidth DependencyProperty which determines the width of the label segment of the HeaderedTextBox Control.
        /// </summary>
        public static readonly DependencyProperty HeaderWidthProperty =
            DependencyProperty.Register("HeaderWidth", typeof(GridLength), typeof(HeaderedTextBox), new PropertyMetadata(new GridLength(40.0)));

        /// <summary>
        /// Defines the Text DependencyProperty which references the text inside the TextBox segment of the HeaderedTextBox Control.
        /// </summary>
        public static readonly DependencyProperty TextProperty =
            DependencyProperty.Register("Text", typeof(string), typeof(HeaderedTextBox), new PropertyMetadata(""));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        public GridLength HeaderWidth
        {
            get { return (GridLength)GetValue(HeaderWidthProperty); }
            set { SetValue(HeaderWidthProperty, value); }
        }

        public string Text
        {
            get { return (string)GetValue(TextProperty); }
            set { SetValue(TextProperty, value); }
        }

        public HeaderedTextBox()
        {
            this.InitializeComponent();
        }
    }
}
