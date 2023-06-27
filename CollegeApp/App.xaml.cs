﻿using System;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

namespace CollegeApp
{
    public partial class App : Application
    {
        static CollegeDatabase _database;

        private static CollegeDatabase CollegeDatabase
        {
            get
            {
                if (_database == null)
                {
                    _database = new CollegeDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "collegeDB"));
                }
                return _database;
            }
        }

        public App ()
        {
            InitializeComponent();

            MainPage = new MainPage();
           
        }

        protected override void OnStart ()
        {
        }

        protected override void OnSleep ()
        {
        }

        protected override void OnResume ()
        {
        }
    }
}
