using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExamProjectFirstYear;
using System.Data.SQLite;
using Rhino.Mocks;
using System.Configuration;

namespace DBTesting
{
    [TestClass]
    public class DBTest
    {
        //[TestMethod]
        //public void TestSQLiteHandlerMethod()
        //{
        //    //Arrange
        //    SQLiteHandler sQLiteHandler = new SQLiteHandler();
        //    ConfigurationManager.AppSettings.Set("ExamProjectFirstYearDB", "DataSource=:memory:");
        //    SQLiteConnection connection = new SQLiteConnection(sQLiteHandler.LoadSQLiteConnectionString());

        //    sQLiteHandler.CreateTable("TestTable", "TestColumn1 INTEGER, TestColumn2 INTEGER", connection);
        //    sQLiteHandler.InsertIntoTable("TestTable", "1, 2", connection);

        //    //Act
        //    //int value = sQLiteHandler.SelectIntValues("Health", "Journal", connection);

        //    //Assert
        //    //Assert.AreEqual(value, 5);
        //}


        //[TestMethod]
        //public void TestLoadSQLiteConnectionString()
        //{
        //    //Arrange
        //    SQLiteHandler sQLiteHandler = new SQLiteHandler();
        //    ConfigurationManager.AppSettings.Set("ExamProjectFirstYearDB", "hej");

        //    //Act
        //    string result = sQLiteHandler.LoadSQLiteConnectionString();

        //    //Assert
        //    Assert.AreEqual(result, "hej");
        //}

        //public void TestExecuteNonQuerySQLiteCommand()
        //{
        //    //Arrange
        //    SQLiteHandler sQLiteHandler = new SQLiteHandler();
        //    ConfigurationManager.AppSettings.Set("ExamProjectFirstYearDB", "DataSource=:memory:");
        //    SQLiteConnection connection;



        //    //Act

        //}

        
    }
}
