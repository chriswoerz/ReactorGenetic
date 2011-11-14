﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ReactorGeneric
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Main : Window
    {
        public Main()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, RoutedEventArgs e)
        {
            if (ValidInputs())
            {
                DisableTextInput();
            }
            else
            {
                return;
            }

            btnStart.IsEnabled = false;
            btnStop.IsEnabled = true;
            btnPause.IsEnabled = true;
            ThunderDome.Report += ReportToUserHandler;   
            ThunderDome.Start(Convert.ToInt32(txtPopulation.Text), Convert.ToInt32(txtChamber.Text), Convert.ToInt32(txtGenerations.Text), Convert.ToInt32(txtTicks.Text));
        }

        private void DisableTextInput()
        {
            txtChamber.IsEnabled = false;
            txtGenerations.IsEnabled = false;
            txtPopulation.IsEnabled = false;
            txtTicks.IsEnabled = false;
        }
        private void EnableTextInput()
        {
            txtChamber.IsEnabled = true;
            txtGenerations.IsEnabled = true;
            txtPopulation.IsEnabled = true;
            txtTicks.IsEnabled = true;
        }

        private void ReportToUserHandler(object sender, ReportEventArgs e)
        {
            txtOutput.AppendText(e.ReportText); 
        }

        private bool ValidInputs()
        {
            try
            {
                var chambers = int.Parse(txtChamber.Text);
                var generations = int.Parse(txtGenerations.Text);
                var population = int.Parse(txtPopulation.Text);
                var ticks = int.Parse(txtTicks.Text);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private void btnPause_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = true;
            btnStart.Content = "Resume";

            ThunderDome.Pause();
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            btnStart.IsEnabled = true;
            btnStart.Content = "Start";
            ThunderDome.Stop();
            EnableTextInput();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtOutput.Clear();
        }
    }

    public delegate void ReportEventHandler(object sender, ReportEventArgs args);
}