using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace ExplolerClassificatorWPF.Display.Customize
{
    public partial class Splitter : UserControl
    {
        public double MinWidthExtend { get; set; } = 20;
        public double MaxWidthExtend { get; set; } = 500;
        public string Text { get; set; } = "";
        public Brush BrushSplit { get; set; } = Brushes.Black;
        public Thickness Thickness { get; set; }
        public Splitter() => InitializeComponent();
        void Button_PreviewMouseMove(object o, MouseEventArgs e)
        {
            (o as Button).Cursor = Cursors.SizeWE;
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                var mpos = Mouse.GetPosition(this).X - (o as Button).RenderTransformOrigin.X;
                var width = mpos > MinWidthExtend ? mpos : MinWidthExtend; //check min
                Width = width < MaxWidthExtend ? width : MaxWidthExtend; //check max, also equal final width
            }
        }
    }
}