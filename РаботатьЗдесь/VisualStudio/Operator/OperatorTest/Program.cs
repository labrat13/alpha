using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Engine.DbSubsystem;

namespace OperatorTest
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //create database
            OperatorDbAdapter db = new OperatorDbAdapter(null);
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            filepath = Path.Combine(filepath, OperatorDbAdapter.AppDbFileName);
            string connString = OperatorDbAdapter.CreateConnectionString(filepath, false);
            if (!File.Exists(filepath))
            {
                OperatorDbAdapter.DatabaseCreate(filepath);
                db.Open(connString);
                //
                db.CreateDatabaseTables();
                //
                db.Close();
            }
            //create 3 ot more table items and add, update and delete them.


        }
    }
}
