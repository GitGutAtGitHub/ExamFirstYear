using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ExamProjectFirstYear;
using System.Data.SQLite;

namespace Testing
{
    /// <summary>
    /// For testing SQLiteHandler methods.
    /// </summary>
    [TestClass]
    public class Database
    {
        SQLiteConnection connection;
        SQLiteCommand command;

        [TestMethod]
        public void LoadConnectionStringTest()
        {
            ////Arrange
            //SQLiteConnection connection;
            //string connectionStringActual;
            //var connectionStringExpected = SQLiteConnection;

            ////Act
            //connection = new SQLiteConnection(SQLiteHandler.LoadConnectionString());

            //connectionStringActual = connection.ToString();

            ////Assert
            //Assert.AreEqual(connectionStringActual, connectionStringExpected);
        }

        public void ExecuteNonQuerySQLiteCommandTest()
        {
            //Arrange
            //connection = new SQLiteConnection(SQLiteHandler.LoadSQLiteConnectionString());

            ////Act
            //using (connection)
            //{
            //    connection.Open();
            //    command = new SQLiteCommand(commandText, connection);
            //    command.ExecuteNonQuery();
            //}

            ////Assert
        }
    }
}
