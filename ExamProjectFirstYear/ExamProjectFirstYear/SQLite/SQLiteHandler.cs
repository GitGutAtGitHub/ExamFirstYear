using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExamProjectFirstYear
{
    /// <summary>
    /// For handling all SQLite-commands and connections.
    /// </summary>
    public class SQLiteHandler
    {
        #region Fields

        private static SQLiteHandler instance;

        private SQLiteConnection connection;

        private SQLiteCommand command;

        private MouseState previousMouseState;
        private MouseState currentMouseState;

        #endregion


        #region Properties

        /// <summary>
        /// SQLiteHandler as a Singleton.
        /// </summary>
        public static SQLiteHandler Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SQLiteHandler();
                }

                return instance;
            }
        }

        #endregion


        #region Generic methods

        /// <summary>
        /// Returns connectionsstring for the database.
        /// </summary>
        /// <returns></returns>
        public static string LoadSQLiteConnectionString()
        {
            return ConfigurationManager.ConnectionStrings["ExamProjectFirstYearDB"].ConnectionString;
        }

        /// <summary>
        /// Method for simplifying SQLiteCommands. The commandText is the command to be executed.
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteNonQuerySQLiteCommand(string commandText)
        {
            connection = new SQLiteConnection(LoadSQLiteConnectionString());

            using (connection)
            {
                connection.Open();
                command = new SQLiteCommand(commandText, connection);
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method for clearing a table. Useful for clearing/overwriting old save fales, journals, inventories etc.
        /// </summary>
        /// <param name="tableName"></param>
        public void ClearTable(string tableName)
        {
            ExecuteNonQuerySQLiteCommand($"DELETE FROM {tableName};");
        }

        /// <summary>
        /// Delete specific row from table. Identified by row name and ID.
        /// </summary>
        /// <param name="tableName"></param>
        public void DeleteFromTable(string tableName, string rowIDName, int ID)
        {
            ExecuteNonQuerySQLiteCommand($"DELETE FROM {tableName} WHERE {rowIDName}={ID};");
        }

        /// <summary>
        /// Fill out a table row.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableValues"></param>
        public void InsertIntoTable(string tableName, string tableValues)
        {
            ExecuteNonQuerySQLiteCommand($"INSERT INTO {tableName} VALUES ({tableValues});");
        }

        /// <summary>
        /// Fill out a table row with specified WHERE.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableValues"></param>
        /// <param name="definedValues"></param>
        /// <param name="compareFrom"></param>
        /// <param name="compareTo"></param>
        public void InsertIntoTableWhere(string tableName, string tableValues, string definedValues, string whereDefinition)
        {
            ExecuteNonQuerySQLiteCommand($"INSERT INTO {tableName} VALUES ({tableValues}) {definedValues} WHERE {whereDefinition};");
        }

        /// <summary>
        /// Update a table row. Useful for updating inventory amounts.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateDefinition"></param>
        /// <param name="whereDefinition"></param>
        public void UpdateTable(string tableName, string updateDefinition, string whereDefinition)
        {
            ExecuteNonQuerySQLiteCommand($"UPDATE {tableName} SET {updateDefinition} WHERE {whereDefinition};");
        }

        /// <summary>
        /// Select values from a table. Can be used for displaying tables.
        /// </summary>
        /// <param name="selectDefinition"></param>
        /// <param name="tableName"></param>
        public void SelectFromTable(string selectDefinition, string tableName)
        {
            ExecuteNonQuerySQLiteCommand($"SELECT {selectDefinition} FROM {tableName};");
        }

        /// <summary>
        /// Select values from a table with a specified WHERE. Can be used for displaying tables.
        /// </summary>
        /// <param name="selectDefinition"></param>
        /// <param name="tableName"></param>
        /// <param name="whereDefinition"></param>
        public void SelectFromTableWhere(string selectDefinition, string tableName, string whereDefinition)
        {
            ExecuteNonQuerySQLiteCommand($"SELECT {selectDefinition} FROM {tableName} WHERE {whereDefinition};");
        }

        #endregion


        #region Game specific methods

        /// <summary>
        /// Increase the amount of the given material by 1.
        /// </summary>
        /// <param name="materialName"></param>
        public void IncreaseAmountStoredMaterial(int materialTypeID)
        {
            UpdateTable("StoredMaterial", "Amount=Amount+1", $"MaterialTypeID={materialTypeID}");

            //int materialTypeID;

            ////Defines the ID bases on the material name.
            //switch (materialName)
            //{
            //    case "Light bulb":
            //        materialTypeID = 1;
            //        break;
            //}
        }

        /// <summary>
        /// Decrease the amount of the given material by 1.
        /// </summary>
        /// <param name="materialName"></param>
        public void DecreaseAmountStoredMaterial(int materialTypeID)
        {
            UpdateTable("StoredMaterial", "Amount=Amount-1", $"MaterialTypeID={materialTypeID}");
        }

        /// <summary>
        /// Records a new Blueprint to the Journal in RecordedBP.
        /// </summary>
        /// <param name="blueprintName"></param>
        public void AddRecordedBP(int blueprintID, int journalID)
        {
            InsertIntoTable($"RecordedBP", $"{blueprintID}, {journalID}");

            //switch (blueprintName)
            //{
            //    case "Door opening device":
            //        //ExecuteNonQuerySQLiteCommand($"INSERT INTO RecordedBP VALUES(1, {journalID});");
            //        InsertIntoTable($"{tableName}", $"1, {journalID}");
            //        break;
            //}
        }

        /// <summary>
        /// Records a new Creature to the Journal in RecordedCreature.
        /// </summary>
        /// <param name="blueprintName"></param>
        public void AddRecordedCreature(int creatureID, int journalID)
        {
            InsertIntoTable($"RecordedCreature", $"{creatureID}, {journalID}");

            //switch (blueprintName)
            //{
            //    case "Door opening device":
            //        //ExecuteNonQuerySQLiteCommand($"INSERT INTO RecordedBP VALUES(1, {journalID});");
            //        InsertIntoTable($"{tableName}", $"1, {journalID}");
            //        break;
            //}
        }

        /// <summary>
        /// Method for saving the game in a specific Journal ID (save slot).
        /// </summary>
        /// <param name="health"></param>
        /// <param name="openDoor"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="ID"></param>
        public void SaveGame(int health, int openDoor, int positionX, int positionY, int ID)
        {
            //Her skal man også definere at parametrene er tilsvarende properties som er defineret for spiller.
            //Altså at f.eks.  positionX = Player.Position.X
            //Eller at  openDoor = Door.Open
            UpdateTable("Journal", $"Health={health}, OpenDoor={openDoor}, PositionX={positionX}, PositionY={positionY}", $"ID={ID}");
        }

        #endregion


        /// <summary>
		/// Method for testing database logic.
		/// </summary>
		public void TestMethod()
        {
            previousMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (currentMouseState.LeftButton == ButtonState.Released && previousMouseState.LeftButton == ButtonState.Pressed)
            {
                Console.WriteLine("Button pressed");

                //Insert the method you want to test here.
                SaveGame(100, 0, 50, 50, 1);
            }
        }
    }
}
