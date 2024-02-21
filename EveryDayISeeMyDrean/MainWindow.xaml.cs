using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace EveryDayISeeMyDrean
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<Zametka> zametki = json.Deser<List<Zametka>>("zametki.json") ?? new List<Zametka>();
        string template = "";
        public MainWindow()
        {
            InitializeComponent();
            Date.SelectedDate = DateTime.Now;
            update_list();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (Title.Text.Length != 0 && Description.Text.Length != 0)
            {
                if (!zamentka_contain())
                    zametki.Add(new Zametka(Title.Text, Description.Text, Date.SelectedDate.Value.ToShortDateString()));
                update_list();
            }
            else MessageBox.Show("Не все поля заполнены или заполнены неправильно");
        }

        private void Date_CalendarClosed(object sender, RoutedEventArgs e) => update_list();
        void update_list()
        {
            ListBox.Items.Clear();
            List<Zametka> rezult = new List<Zametka>();
            foreach (var a in zametki)
                if (a.Date == Date.SelectedDate.Value.ToShortDateString()) ListBox.Items.Add(a.Title);
            Title.Text = "";
            Description.Text = "";
            write_file();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ListBox.SelectedItem != null)
            {
                foreach (var a in zametki)
                {
                    if (ListBox.SelectedItem == a.Title)
                    {
                        Title.Text = a.Title;
                        Description.Text = a.Description;
                        template = a.Title;
                        break;
                    }
                }
            }
        }
        bool zamentka_contain()
        {
            foreach (var a in zametki)
            {
                if (a.Title == Title.Text)
                {
                    MessageBox.Show("Такая заметка уже существует");
                    return true;
                }
            }
            return false;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Title.Text.Length != 0 && Description.Text.Length != 0)
            {
                if (!zamentka_contain())
                {
                    foreach (var a in zametki)
                    {
                        if (a.Title == template)
                        {
                            a.Title = template;
                            a.Description = Description.Text;
                            update_list();
                            break;
                        }
                    }
                }
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (ListBox.SelectedItem != null)
            {
                foreach (var a in zametki)
                {
                    if (a.Title == template)
                    {
                        zametki.Remove(a);
                        break;
                    }
                }
                update_list();
            }
            else MessageBox.Show("Элемент не был выбран");
        }
        void write_file() => json.Ser<List<Zametka>>(zametki, "zametki.json");
    }
    public class Zametka
    {
        public string Title { get; set; }
        public string Description;
        public string Date;

        public Zametka(string title, string description, string date)
        {
            Title = title;
            Description = description;
            Date = date;
        }
    }
    public class json
    {
        public static T Deser<T>(string path)
        {
            if (!File.Exists(path))
                File.WriteAllText(path, "");
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(path));
        }
        public static void Ser<T>(T obj, string path) => File.WriteAllText(path, JsonConvert.SerializeObject(obj));
    }
}
