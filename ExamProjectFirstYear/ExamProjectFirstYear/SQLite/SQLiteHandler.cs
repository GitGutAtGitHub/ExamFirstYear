using ExamProjectFirstYear.Components;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SQLite;
using System.Linq;
using System.Text;

namespace ExamProjectFirstYear
{
    /// <summary>
    /// For handling all SQLite-commands and connections.
    /// </summary>
    public class SQLiteHandler
    {
        #region Fields

        //private static SQLiteHandler instance;

        private SQLiteConnection connection;

        private SQLiteCommand command;

        #endregion


        #region Properties

        /// <summary>
        /// SQLiteHandler as a Singleton.
        /// </summary>
        //public static SQLiteHandler Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            instance = new SQLiteHandler();
        //        }

        //        return instance;
        //    }
        //}

        #endregion


        #region Constructors

        public SQLiteHandler()
        {

        }

        #endregion


        #region Generic methods

        /// <summary>
        /// Returns connectionsstring for the database.
        /// </summary>
        /// <returns></returns>
        public string LoadSQLiteConnectionString()
        {
            return ConfigurationManager.AppSettings["ExamProjectFirstYearDB"];
        }

        /// <summary>
        /// Method for simplifying SQLiteCommands. The commandText is the command to be executed.
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

        public void CreateTable(string tableName, string columns, SQLiteConnection connection)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"CREATE TABLE IF NOT EXISTS {tableName} ({columns})", connection), connection);
        }

        /// <summary>
        /// Method for clearing a table. Useful for clearing/overwriting old save fales, journals, inventories etc.
        /// </summary>
        /// <param name="tableName"></param>
        public void ClearTable(string tableName)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"DELETE FROM {tableName};", connection = new SQLiteConnection(LoadSQLiteConnectionString())), connection);
        }

        /// <summary>
        /// Delete specific row from table. Identified by row name and ID.
        /// </summary>
        /// <param name="tableName"></param>
        public void DeleteFromTable(string tableName, string rowIDName, int ID)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"DELETE FROM {tableName} WHERE {rowIDName}={ID};", connection = new SQLiteConnection(LoadSQLiteConnectionString())), connection);
        }

        /// <summary>
        /// Fill out a table row.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="tableValues"></param>
        public void InsertIntoTable(string tableName, string tableValues, SQLiteConnection connection)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"INSERT INTO {tableName} VALUES ({tableValues});", connection), connection);
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
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"INSERT INTO {tableName} VALUES ({tableValues}) {definedValues} WHERE {whereDefinition};", connection = new SQLiteConnection(LoadSQLiteConnectionString())), connection);
        }

        /// <summary>
        /// Update a table row. Useful for updating inventory amounts.
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="updateDefinition"></param>
        /// <param name="whereDefinition"></param>
        public void UpdateTable(string tableName, string updateDefinition, string whereDefinition)
        {
            ExecuteNonQuerySQLiteCommand(new SQLiteCommand($"UPDATE {tableName} SET {updateDefinition} WHERE {whereDefinition};", connection = new SQLiteConnection(LoadSQLiteConnectionString())), connection);
        }




        ///// <summary>
        ///// Select values from a table. Can be used for displaying tables.
        ///// </summary>
        ///// <param name="selectDefinition"></param>
        ///// <param name="tableName"></param>
        //public void SelectFromTable(string selectDefinition, string tableName)
        //{
        //    ExecuteNonQuerySQLiteCommand($"SELECT {selectDefinition} FROM {tableName};");
        //}

        ///// <summary>
        ///// Select values from a table with a specified WHERE. Can be used for displaying tables.
        ///// </summary>
        ///// <param name="selectDefinition"></param>
        ///// <param name="tableName"></param>
        ///// <param name="whereDefinition"></param>
        //public void SelectFromTableWhere(string selectDefinition, string tableName, string whereDefinition)
        //{
        //    ExecuteNonQuerySQLiteCommand($"SELECT {selectDefinition} FROM {tableName} WHERE {whereDefinition};");
        //}




        /// <summary>
        /// Returns an int value from a specfied table row with a WHERE condition.
        /// </summary>
        /// <param name="selectDefinition"></param>
        /// <param name="tableName"></param>
        /// <param name="whereDefinition"></param>
        /// <returns></returns>
        public int SelectIntValuesWhere(string selectDefinition, string tableName, string whereDefinition)
        {
            //return ExecuteNonQuerySQLiteCommand($"SELECT {selectDefinition} FROM {tableName} WHERE {whereDefinition};");
            int value;

            connection = new SQLiteConnection(LoadSQLiteConnectionString());

            using (connection)
            {
                connection.Open();
                command = new SQLiteCommand($"SELECT {selectDefinition} FROM {tableName} WHERE {whereDefinition};", connection);
                value = Convert.ToInt32(command.ExecuteScalar());
            }

            return value;
        }

        /// <summary>
        /// Returns a string value from a specfied table row with a WHERE condition.
        /// </summary>
        /// <param name="selectDefinition"></param>
        /// <param name="tableName"></param>
        /// <param name="whereDefinition"></param>
        /// <returns></returns>
        public string SelectStringValuesWhere(string selectDefinition, string tableName, string whereDefinition)
        {
            string value;

            connection = new SQLiteConnection(LoadSQLiteConnectionString());
            
            using (connection)
            {
                connection.Open();
                command = new SQLiteCommand($"SELECT {selectDefinition} FROM {tableName} WHERE {whereDefinition};", connection);
                value = Convert.ToString(command.ExecuteScalar());
            }

            return value;
        }

        /// <summary>
        /// Returns an int value from a specfied table row.
        /// </summary>
        /// <param name="selectDefinition"></param>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public int SelectIntValues(string selectDefinition, string tableName, SQLiteConnection connection)
        {
            int value;
            
            using (connection)
            {
                connection.Open();

                command = new SQLiteCommand($"SELECT {selectDefinition} FROM {tableName}", connection);
                value = Convert.ToInt32(command.ExecuteScalar());
            }

            return value;
        }

        /// <summary>
        /// Code used when building the database.
        /// </summary>
        public void BuildDatabase()
        {
            //CreateTable("Inventory", "ID INTEGER PRIMARY KEY");
            //CreateTable("Journal", "ID INTEGER PRIMARY KEY, InventoryID INTEGER, Health INTEGER, OpenDoor BOOLEAN, " +
            //            "PositionX INTEGER, PositionY INTEGER, Mana INTEGER, FOREIGN KEY (InventoryID) REFERENCES Inventory(ID)");

            //CreateTable("MaterialType", "ID INTEGER PRIMARY KEY, Name STRING");
            //CreateTable("RequiredMaterial", "MaterialTypeID INTEGER, BlueprintID INTEGER, Amount INTEGER, " +
            //            "FOREIGN KEY (MaterialTypeID) REFERENCES MaterialType(ID), FOREIGN KEY (BlueprintID) REFERENCES Blueprint(ID)");
            //CreateTable("StoredMaterial", "MaterialTypeID INTEGER, InventoryID INTEGER, Amount INTEGER, Slot INTEGER UNIQUE, " +
            //            "FOREIGN KEY (MaterialTypeID) REFERENCES MaterialType(ID), FOREIGN KEY (InventoryID) REFERENCES Inventory(ID)");

            //CreateTable("Creature", "ID INTEGER PRIMARY KEY, MaterialTypeID INTEGER, Name STRING, Type STRING, " +
            //            "Description STRING, Location STRING, FOREIGN KEY(MaterialTypeID) REFERENCES MaterialType(ID)");
            //CreateTable("RecordedCreature", "CreatureID INTEGER, JournalID INTEGER, " +
            //            "FOREIGN KEY (CreatureID) REFERENCES Creature(ID), FOREIGN KEY (JournalID) REFERENCES Journal(ID)");

            //CreateTable("Blueprint", "ID INTEGER PRIMARY KEY, Name STRING, Description STRING");
            //CreateTable("RecordedBP", "BlueprintID INTEGER, JournalID INTEGER, " +
            //            "FOREIGN KEY (BlueprintID) REFERENCES Blueprint(ID), FOREIGN KEY (JournalID) REFERENCES Journal(ID)");


            //InsertIntoTable("Inventory", "(NULL)");
            //ExecuteNonQuerySQLiteCommand("INSERT INTO Journal (ID, InventoryID, Health, OpenDoor, PositionX, PositionY, Mana) SELECT NULL, Inventory.ID, 5, 0, 50, 50, 5 FROM Inventory WHERE Inventory.ID=ID");

            //ExecuteNonQuerySQLiteCommand("INSERT INTO Blueprint VALUES (NULL, 'Door opening device', 'Somehow, this is supposed to open the door?')");
            //ExecuteNonQuerySQLiteCommand("INSERT INTO RecordedBP VALUES(1, 1)");

            //ExecuteNonQuerySQLiteCommand("INSERT INTO Creature VALUES(NULL, 1, 'Spider Bulb', 'Melee', 'A glowing light bulb spider', 'Dark caves')");
            //ExecuteNonQuerySQLiteCommand("INSERT INTO RecordedCreature VALUES(1, 1)");

            //ExecuteNonQuerySQLiteCommand("INSERT INTO MaterialType VALUES(NULL, 'Light bulb')");
            //ExecuteNonQuerySQLiteCommand("INSERT INTO RequiredMaterial VALUES(1, 1, 3)");
            //ExecuteNonQuerySQLiteCommand("INSERT INTO StoredMaterial VALUES(1, 1, 0, 1)");


            //ExecuteNonQuerySQLiteCommand("DELETE FROM RecordedBP");

            //ExecuteNonQuerySQLiteCommand("DELETE FROM RecordedCreature");
            //ExecuteNonQuerySQLiteCommand("DELETE FROM Creature");

            //ExecuteNonQuerySQLiteCommand("DELETE FROM MaterialType");
            //ExecuteNonQuerySQLiteCommand("DELETE FROM RequiredMaterial");
            //ExecuteNonQuerySQLiteCommand("DELETE FROM StoredMaterial");

            //ExecuteNonQuerySQLiteCommand("DELETE FROM Inventory");
            //ExecuteNonQuerySQLiteCommand("DELETE FROM Journal");
        }

        #endregion


        #region Game specific methods

        /// <summary>
        /// Increase the amount of the given material by 1.
        /// </summary>
        /// <param name="materialName"></param>
        public void IncreaseAmountStoredMaterial(int materialTypeID)
        {
            GameWorld.Instance.inventory.MaterialTypeIDs.Add(materialTypeID);
            UpdateTable("StoredMaterial", "Amount=Amount+1", $"MaterialTypeID={materialTypeID}");
        }

        /// <summary>
        /// Decrease the amount of the given material by 1.
        /// </summary>
        /// <param name="materialName"></param>
        public void DecreaseAmountStoredMaterial(int materialTypeID)
        {
            GameWorld.Instance.inventory.MaterialTypeIDs.Remove(materialTypeID);
            UpdateTable("StoredMaterial", "Amount=Amount-1", $"MaterialTypeID={materialTypeID}");
        }

        /// <summary>
        /// Records a new Blueprint to the Journal in RecordedBP.
        /// </summary>
        /// <param name="blueprintName"></param>
        public void AddRecordedBP(int blueprintID, int journalID)
        {
            GameWorld.Instance.journal.RecordedBlueprintIDs.Add(blueprintID);
            InsertIntoTable($"RecordedBP", $"{blueprintID}, {journalID}", connection = new SQLiteConnection(LoadSQLiteConnectionString()));
        }

        /// <summary>
        /// Records a new Creature to the Journal in RecordedCreature.
        /// </summary>
        /// <param name="blueprintName"></param>
        public void AddRecordedCreature(int creatureID, int journalID)
        {
            GameWorld.Instance.journal.RecordedCreatureIDs.Add(creatureID);
            InsertIntoTable($"RecordedCreature", $"{creatureID}, {journalID}", connection = new SQLiteConnection(LoadSQLiteConnectionString()));
        }

        /// <summary>
        /// Method for saving the game in a specific JournalID (save slot).
        /// </summary>
        /// <param name="health"></param>
        /// <param name="openDoor"></param>
        /// <param name="positionX"></param>
        /// <param name="positionY"></param>
        /// <param name="ID"></param>
        public void SaveGame(int health, int openDoor, int ID, int mana)
        {
            //Her skal man også definere at parametrene er tilsvarende properties som er defineret for spiller.
            //Altså at f.eks.  positionX = Player.Position.X
            //Eller at  openDoor = Door.Open

            int positionX = (int)GameWorld.Instance.player.GameObject.Transform.Position.X;
            int positionY = (int)GameWorld.Instance.player.GameObject.Transform.Position.Y;

            UpdateTable("Journal", $"Health={health}, OpenDoor={openDoor}, PositionX={positionX}, PositionY={positionY}, Mana={mana}", $"ID={ID}");
        }

        #endregion


        #region GetTable Methods

        /// <summary>
        /// Returns a temporary Journal that can be interacted with outside SQLite.
        /// </summary>
        /// <param name="journalID"></param>
        /// <returns></returns>
        public TmpJournal GetJournal(int journalID)
        {
            int inventoryID = SelectIntValuesWhere("InventoryID", "Journal", $"ID = {journalID}");
            int health = SelectIntValuesWhere("Health", "Journal", $"ID = {journalID}");
            int openDoor = SelectIntValuesWhere("OpenDoor", "Journal", $"ID = {journalID}");
            int positionX = SelectIntValuesWhere("PositionX", "Journal", $"ID = {journalID}");
            int positiony = SelectIntValuesWhere("PositionY", "Journal", $"ID = {journalID}");
            int mana = SelectIntValuesWhere("Mana", "Journal", $"ID = {journalID}");

            TmpJournal tmpJournal = new TmpJournal(journalID, inventoryID, health, positionX, positiony, openDoor, mana);

            return tmpJournal;
        }

        /// <summary>
        /// Returns a temporary Blueprint to get values from the table.
        /// </summary>
        /// <param name="blueprintID"></param>
        /// <returns></returns>
        public TmpBlueprint GetBlueprint(int blueprintID)
        {
            string name = SelectStringValuesWhere("Name", "Blueprint", $"ID = {blueprintID}");
            string description = SelectStringValuesWhere("Description", "Blueprint", $"ID = {blueprintID}");

            TmpBlueprint tmpBlueprint = new TmpBlueprint(blueprintID, name, description);

            return tmpBlueprint;
        }

        /// <summary>
        /// Returns a temporary RequiredMaterial to get values from the table.
        /// </summary>
        /// <param name="blueprintID"></param>
        /// <returns></returns>
        public TmpRequiredMaterial GetRequiredMaterial(int blueprintID)
        {
            int materialTypeID = SelectIntValuesWhere("MaterialTypeID", "RequiredMaterial", $"BlueprintID = {blueprintID}");
            int amount = SelectIntValuesWhere("Amount", "RequiredMaterial", $"BlueprintID = {blueprintID}");

            TmpRequiredMaterial tmpRequiredMaterial = new TmpRequiredMaterial(materialTypeID, amount);

            return tmpRequiredMaterial;
        }

        /// <summary>
        /// Returns a temporary MaterialType to get values from the table.
        /// </summary>
        /// <param name="materialTypeID"></param>
        /// <returns></returns>
        public TmpMaterialType GetMaterialType(int materialTypeID)
        {
            string name = SelectStringValuesWhere("Name", "MaterialType", $"ID = {materialTypeID}");

            TmpMaterialType tmpMaterialType = new TmpMaterialType(name);

            return tmpMaterialType;
        }

        /// <summary>
        /// Returns a temporary Creature to get values from the table.
        /// </summary>
        /// <param name="creatureID"></param>
        /// <returns></returns>
        public TmpCreature GetCreature(int creatureID)
        {
            int materialTypeID = SelectIntValuesWhere("MaterialTypeID", "Creature", $"ID = {creatureID}");
            string name = SelectStringValuesWhere("Name", "Creature", $"ID = {creatureID}");
            string type = SelectStringValuesWhere("Type", "Creature", $"ID = {creatureID}");
            string description = SelectStringValuesWhere("Description", "Creature", $"ID = {creatureID}");
            string location = SelectStringValuesWhere("Location", "Creature", $"ID = {creatureID}");

            TmpCreature tmpCreature = new TmpCreature(materialTypeID, name, type, description, location);

            return tmpCreature;
        }

        /// <summary>
        /// Returns a temporary StoredMaterial to get values from the table.
        /// </summary>
        /// <param name="creatureID"></param>
        /// <returns></returns>
        public TmpStoredMaterial GetStoredMaterial(int materialTypeID, int inventoryID)
        {
            int amount = SelectIntValuesWhere("Amount", "StoredMaterial", $"MaterialTypeID = {materialTypeID} AND InventoryID = {inventoryID}");
            int slot = SelectIntValuesWhere("Slot", "StoredMaterial", $"MaterialTypeID = {materialTypeID} AND InventoryID = {inventoryID}");

            TmpStoredMaterial tmpStoredMaterial = new TmpStoredMaterial(amount, slot);

            return tmpStoredMaterial;
        }

        /// <summary>
        /// Returns a temporary Inventory to get values from the table.
        /// </summary>
        /// <param name="creatureID"></param>
        /// <returns></returns>
        public TmpInventory GetInventory()
        {
            int iD = SelectIntValuesWhere("ID", "Inventory", $"ID = {GameWorld.Instance.player.PlayerID}");

            TmpInventory tmpInventory = new TmpInventory(iD);

            return tmpInventory;
        }

        #endregion
    }
}
