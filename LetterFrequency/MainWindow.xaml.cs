﻿using System;
using System.Collections.Generic;
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
using Microsoft.Win32;
using System.Security.Cryptography;
using System.IO;
using System.Drawing;
using System.Text.RegularExpressions;

namespace LetterFrequency
{
    public partial class MainWindow : Window
    {
        private static MainWindow _instance = null;
        private string filePath = null;
        public class Character
        {
            public string Char { get; set; }
            public int Amount { get; set; }
            public string Percentage { get; set; }
        }
        public MainWindow()
        {
            InitializeComponent();
            _instance = this;
        }

        private void Input_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                DefaultExt = ".txt",
                Filter = "Plain Text|*txt"
            };
            Nullable<bool> result = openFileDialog.ShowDialog();
            if (result == true)
            {
                filePath = openFileDialog.FileName;
                _instance.InputFile.Content = Path.GetFileName(filePath);
            }
        }

        private void Analyze_Click(object sender, RoutedEventArgs e)
        {
            if(filePath == null)
            {
                MessageBox.Show("Please select a text file to analyze", "No file selected", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            List<Character> found = new List<Character>();
            Regex filter = new Regex(@"[^\s]", RegexOptions.Compiled);

            if((bool)_instance.Btn1.IsChecked)
            {
                filter = new Regex(@"[a-zA-Z]", RegexOptions.Compiled);
            }
            else if ((bool)_instance.Btn2.IsChecked)
            {
                filter = new Regex(@"[0-9]", RegexOptions.Compiled);
            } else if ((bool)_instance.Btn3.IsChecked)
            {
                filter = new Regex(@"[a-zA-Z0-9]", RegexOptions.Compiled);
            } else if ((bool)_instance.Btn4.IsChecked)
            {
                filter = new Regex(@"[^\s]", RegexOptions.Compiled);
            } else
            {
                MessageBox.Show("Please select a mode to use", "No mode selected", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string text = File.ReadAllText(filePath);
            if((bool)_instance.Case.IsChecked)
            {
                text = text.ToLower();
            }
            char[] chars = text.ToCharArray();
            char[] filtered = chars.Where(c => filter.Match(c.ToString()).Success).ToArray();
            char[] filteredDist = filtered.Distinct().ToArray();
            foreach(char ch in filteredDist)
            {
                int count = text.Length - text.Replace(ch.ToString(), "").Length;
                found.Add(new Character() { Char = ch.ToString(), Amount = count, Percentage = ((Convert.ToDouble(count)/Convert.ToDouble(filtered.Length))*Convert.ToDouble(100)).ToString("F")+"%" });
            }

            _instance.list.ItemsSource = found;
            MessageBox.Show("Analasys Finished", "Sucess", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }
}
