using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ExplolerClassificatorWPF.Display
{
    public partial class StatusBar : UserControl
    {
        List<StatusElem> statusElems = new List<StatusElem>();
        public StatusBar() => InitializeComponent();
        void Update()
        {
            byte[] image;
            if (Tasks.Items.Count == 0)
            {
                TextLeft.Text = "";
                image = Properties.Resources.chocolate;
            }
            else image = Properties.Resources.bar;
            Bar.Source = new ImageSourceConverter().ConvertFrom(image) as ImageSource;
        }

        class StatusElem
        {
            static public int key = 0;
            public int Key = ++key;
            public string Text { get; set; }
        }
        public void add(string text)
        {
            statusElems.Add(new StatusElem { Text = text });
            Tasks.ItemsSource = null;
            Tasks.ItemsSource = statusElems;
            TextLeft.Text = text;
        }
        public int Add(string text)
        {
            add(text);
            Update();
            return StatusElem.key;
        }
        void remove(int key)
        {
            foreach (var task in Tasks.Items)
                if ((task as StatusElem).Key == key)
                {
                    statusElems.Remove(task as StatusElem);
                    Tasks.ItemsSource = null;
                    Tasks.ItemsSource = statusElems;
                    break;
                }
        }
        public void Remove(int key)
        {
            remove(key);
            Update();
        }
        public int RemoveAdd(int keyRemove, string textAdd)
        {
            remove(keyRemove);
            add(textAdd);
            Update();
            return StatusElem.key;
        }
        void TasksVis_Click(object o, RoutedEventArgs e) => Tasks.Visibility = Tasks.IsVisible ? Visibility.Hidden : Visibility.Visible;
    }
}