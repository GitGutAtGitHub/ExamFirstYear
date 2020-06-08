﻿using ExamProjectFirstYear.Components;
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
        #region Constructors

        /// <summary>
        /// Constructor for SQLiteHandler.
        /// </summary>
        public SQLiteHandler()
        {

        }

        #endregion


        #region Generic methods

        /// <summary>
        /// Returns the connectionsstring for the database. Check App.config for details.
        /// </summary>
        /// <returns></returns>
        public string LoadSQLiteConnectionString()
        {
            return ConfigurationManager.AppSettings["ExamProjectFirstYearDB"];
        }

        /// <summary>
        /// Method for simplifying and executing SQLiteCommands as NonQuery. The commandText is the command to be executed.
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteNonQuerySQLiteCommand(SQLiteCommand command, SQLiteConnection connection)
        {
            using (connection)
            {
                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Method for simplifying and executing SQLiteCommands as Scalar. The commandText is the command to be executed.
        ///Returns the result as an integer.
        /// </summary>
        /// <param name="commandText"></param>
        public int ExecuteScalarInt(SQLiteCommand command, SQLiteConnection connection)
        {
            int value;

            using (connection)
            {
                connection.Open();
                value = Convert.ToInt32(command.ExecuteScalar());
            }

            return value;
        }

        /// <summary>
        /// Method for simplifying SQLiteCommands as Scalar. The commandText is the command to be executed.
        /// Returns the result as a string.
        /// </summary>
        /// <param name="commandText"></param>
        public string ExecuteScalarString(SQLiteCommand command, SQLiteConnection connection)
        {
            string value;

            using (connection)
            {
                connection.Open();
                value = Convert.ToString(command.ExecuteScalar());
            }

            return value;
        }

        /// <summary>
        /// Execute a read on a selected column from a table.
        /// Returns the read as a list of integers.
        /// </summary>
        /// <param name="selectedColumn"></param>
        /// <param name="fromTable"></param>
        /// <param name="whereDefinition"></param>
        /// <returns></returns>
        public List<int> ExecuteIntReader(string selectedColumn, string fromTable, string whereDefinition)
        {
            List<int> values = new List<int>();
            SQLiteConnection connection = new SQLiteConnection(LoadSQLiteConnectionString());
            SQLiteCommand command;

            using (connection)
            {
                connection.Open();

                command = new SQLiteCommand($"SELECT {selectedColumn} FROM {fromTable} WHERE {whereDefinition}", connection);

                SQLiteDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    values.Add(reader.GetInt32(0));
                }
            }

            return values;
        }

        /// <summary>
        /// Creates a specified table.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="columns"></param>
        /// <param name="connection"></param>
        public void CreateTable(string tableName, string columns, SQLiteConnection connection)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"CREATE TABLE IF NOT EXISTS {tableName} ({columns})", connection), connection);
        }

        /// <summary>
        /// Method for clearing a table. Useful for clearing/overwriting old save fales, journals, inventories etc.
        /// </summary>
        /// <param name="tableName"></param>
        public void ClearTable(string tableName, SQLiteConnection connection)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"DELETE FROM {tableName};", connection), connection);
        }

        /// <summary>
        /// Fill out one or more table rows.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableValues"></param>
        public void InsertIntoTable(string tableName, string tableValues, SQLiteConnection connection)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"INSERT OR IGNORE INTO {tableName} VALUES ({tableValues});",
                                         connection), connection);
        }

        /// <summary>
        /// Fill out one or more table rows with a specified WHERE condition.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableValues"></param>
        /// <param name="definedValues"></param>
        /// <param name="compareFrom"></param>
        /// <param name="compareTo"></param>
        public void InsertIntoTableWhere(string tableName, string tableValues, string definedValues, string whereDefinition, SQLiteConnection connection)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"INSERT OR IGNORE INTO {tableName} ({tableValues}) {definedValues} " +
                                       $"WHERE {whereDefinition};", connection), connection);
        }

        /// <summary>
        /// Update a table row with a specified WHERE condition. 
        /// Useful for updating inventory amounts.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateDefinition"></param>
        /// <param name="whereDefinition"></param>
        public void UpdateTableWhere(string tableName, string updateDefinition, string whereDefinition, SQLiteConnection connection)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"UPDATE {tableName} SET {updateDefinition} WHERE " +
                                      $"{whereDefinition};", connection), connection);
        }

        /// <summary>
        /// Returns an int value from a specfied table row with a WHERE condition.
        /// </summary>
        /// <param name="selectDefinition"></param>
        /// <param name="tableName"></param>
        /// <param name="whereDefinition"></param>
        /// <returns></returns>
        public int SelectIntValuesWhere(string selectDefinition, string tableName, string whereDefinition, SQLiteConnection connection)
        {
            return ExecuteScalarInt(new SQLiteCommand($"SELECT {selectDefinition} FROM {tableName} WHERE " +
                                              $"{whereDefinition};", connection), connection);
        }

        /// <summary>
        /// Returns a string value from a specfied table row with a WHERE condition.
        /// </summary>
        /// <param name="selectDefinition"></param>
        /// <param name="tableName"></param>
        /// <param name="whereDefinition"></param>
        /// <returns></returns>
        public string SelectStringValuesWhere(string selectDefinition, string tableName, string whereDefinition, SQLiteConnection connection)
        {
            return ExecuteScalarString(new SQLiteCommand($"SELECT {selectDefinition} FROM {tableName} WHERE " +
                                                 $"{whereDefinition};", connection), connection);
        }

        /// <summary>
        /// Method used when building the database.
        /// This is run once initialization (GameWorld.Instance.Initialize).
        /// </summary>
        public void BuildDatabase()
        {
            //Creates all the necessary tables for the game, if they haven't already been created.
            CreateTable("Inventory", "ID INTEGER PRIMARY KEY", new SQLiteConnection(LoadSQLiteConnectionString()));
            CreateTable("Journal", "ID INTEGER PRIMARY KEY, InventoryID INTEGER, Health INTEGER, OpenDoor BOOLEAN, " +
                        "PositionX INTEGER, PositionY INTEGER, Mana INTEGER, FOREIGN KEY (InventoryID) REFERENCES Inventory(ID)",
                         new SQLiteConnection(LoadSQLiteConnectionString()));

            CreateTable("MaterialType", "ID INTEGER PRIMARY KEY, Name STRING", new SQLiteConnection(LoadSQLiteConnectionString()));
            CreateTable("RequiredMaterial", "MaterialTypeID INTEGER, BlueprintID INTEGER, Amount INTEGER, " +
                        "FOREIGN KEY (MaterialTypeID) REFERENCES MaterialType(ID), FOREIGN KEY (BlueprintID) REFERENCES Blueprint(ID)," +
                        "UNIQUE (MaterialTypeID)", new SQLiteConnection(LoadSQLiteConnectionString()));
            CreateTable("StoredMaterial", "MaterialTypeID INTEGER, InventoryID INTEGER, Amount INTEGER, Slot INTEGER UNIQUE, " +
                        "FOREIGN KEY (MaterialTypeID) REFERENCES MaterialType(ID), FOREIGN KEY (InventoryID) REFERENCES Inventory(ID), " +
                        "UNIQUE (MaterialTypeID, InventoryID)", new SQLiteConnection(LoadSQLiteConnectionString()));

            CreateTable("Creature", "ID INTEGER PRIMARY KEY, MaterialTypeID INTEGER, Name STRING, Type STRING, " +
                        "Description STRING, Location STRING, FOREIGN KEY(MaterialTypeID) REFERENCES MaterialType(ID)",
                         new SQLiteConnection(LoadSQLiteConnectionString()));
            CreateTable("RecordedCreature", "CreatureID INTEGER, JournalID INTEGER, " +
                        "FOREIGN KEY (CreatureID) REFERENCES Creature(ID), FOREIGN KEY (JournalID) REFERENCES Journal(ID)," +
                        "UNIQUE (CreatureID, JournalID)", new SQLiteConnection(LoadSQLiteConnectionString()));

            CreateTable("Blueprint", "ID INTEGER PRIMARY KEY, Name STRING, Description STRING",
                         new SQLiteConnection(LoadSQLiteConnectionString()));
            CreateTable("RecordedBP", "BlueprintID INTEGER, JournalID INTEGER, " +
                        "FOREIGN KEY (BlueprintID) REFERENCES Blueprint(ID), FOREIGN KEY (JournalID) REFERENCES Journal(ID)," +
                        "UNIQUE (BlueprintID, JournalID)", new SQLiteConnection(LoadSQLiteConnectionString()));


            //Fills out the specified tables with necessary values for the game. 
            //Ignore if an identical row with unique values already exists.
            //This ensures the table won't be filled with duplicates if the game is loaded more than once.
            InsertIntoTable("Inventory", "1", new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTableWhere("Journal", "ID, InventoryID, Health, OpenDoor, PositionX, PositionY, Mana",
                                 "SELECT 1, Inventory.ID, 5, 0, 50, 50, 5 FROM Inventory", "Inventory.ID=ID",
                                  new SQLiteConnection(LoadSQLiteConnectionString()));

            InsertIntoTable("Blueprint", "1, 'Door opening device', 'Somehow, this is supposed to open the door?'",
                             new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTable("RecordedBP", "1, 1", new SQLiteConnection(LoadSQLiteConnectionString()));

            InsertIntoTable("Creature", "1, 1, 'Spider Bulb', 'Melee', 'A glowing light bulb spider', 'Dark caves'",
                             new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTable("Creature", "2, 2, 'Match Stick Insect', 'Ranged', 'A stick insect with a sulphur head', 'Dark caves'",
                             new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTable("Creature", "3, 3, 'Mirror Moth', 'Flying', 'A moth with highly reflective wings', 'Dark caves'",
                             new SQLiteConnection(LoadSQLiteConnectionString()));

            InsertIntoTable("MaterialType", "1, 'Spider filament'", new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTable("MaterialType", "2, 'Match head'", new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTable("MaterialType", "3, 'Moth wing'", new SQLiteConnection(LoadSQLiteConnectionString()));


            InsertIntoTable("RequiredMaterial", "1, 1, 3", new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTable("RequiredMaterial", "2, 1, 5", new SQLiteConnection(LoadSQLiteConnectionString()));
            InsertIntoTable("RequiredMaterial", "3, 1, 2", new SQLiteConnection(LoadSQLiteConnectionString()));
        }

        #endregion


        #region Game specific methods

        /// <summary>
        /// Increase the amount of the given material by 1.
        /// </summary>
        /// <param name="materialName"></param>
        public void IncreaseAmountStoredMaterial(int materialTypeID, int inventoryID)
        {
            //Get a list of allt the inventoryIDs.
            List<int> inventoryIDs = ExecuteIntReader("InventoryID", "StoredMaterial",
                                   $"MaterialTypeID={materialTypeID}");

            //Insert the material into the table if the ID isn't present.
            //This ensures that materials are only shown in the onscreen Inventory if the player has at least 1 of the material.
            if (!inventoryIDs.Contains(inventoryID))
            {
                InsertIntoTable("StoredMaterial", $"{materialTypeID}, {inventoryID}, 1, {materialTypeID}",
                                 new SQLiteConnection(LoadSQLiteConnectionString()));
            }

            //If a player already has that material, add it by one.
            if (inventoryIDs.Contains(inventoryID))
            {
                UpdateTableWhere("StoredMaterial", "Amount=Amount+1", $"MaterialTypeID={materialTypeID} " +
                                $"AND InventoryID={inventoryID}", new SQLiteConnection(LoadSQLiteConnectionString()));
            }
        }

        /// <summary>
        /// Records a new Blueprint to the Journal in RecordedBP.
        /// This isn't currently used, but can be used to include multiple BP records in the game.
        /// </summary>
        /// <param name="blueprintName"></param>
        public void AddRecordedBP(int blueprintID, int journalID)
        {
            InsertIntoTable($"RecordedBP", $"{blueprintID}, {journalID}",
                              new SQLiteConnection(LoadSQLiteConnectionString()));
        }

        /// <summary>
        /// Records a new Creature to the Journal in RecordedCreature.
        /// </summary>
        /// <param name="blueprintName"></param>
        public void AddRecordedCreature(int creatureID, int journalID)
        {
            //if (!GameWorld.Instance.journal.RecordedCreatureIDs.Contains(creatureID))
            //{
            //    GameWorld.Instance.journal.RecordedCreatureIDs.Add(creatureID);
            //}

            InsertIntoTable($"RecordedCreature", $"{creatureID}, {journalID}",
                              new SQLiteConnection(LoadSQLiteConnectionString()));
        }

        /// <summary>
        /// Method for saving the game in a specific JournalID (save slot).
        /// Not currently in used since the game is very small.
        /// Define the parameters as the player current Health, OpenDoor and Mana.
        /// </summary>
        /// <param name="health"></param>
        /// <param name="openDoor"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="ID"></param>
        public void SaveGame(int health, int openDoor, int ID, int mana)
        {
            int positionX = (int)GameWorld.Instance.Player.GameObject.Transform.Position.X;
            int positionY = (int)GameWorld.Instance.Player.GameObject.Transform.Position.Y;

            UpdateTableWhere("Journal", $"Health={health}, OpenDoor={openDoor}, PositionX={positionX}, " +
                       $"PositionY={positionY}, Mana={mana}", $"ID={ID}",
                         new SQLiteConnection(LoadSQLiteConnectionString()));
        }

        #endregion
    }
}
