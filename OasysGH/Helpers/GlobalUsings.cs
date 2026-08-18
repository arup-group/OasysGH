extern alias OasysGhSQLite_1_2_5;

#if ISOLATED_SQLITE
global using SQLiteConnection = OasysGhSQLite_1_2_5::System.Data.SQLite.SQLiteConnection;
global using SQLiteCommand = OasysGhSQLite_1_2_5::System.Data.SQLite.SQLiteCommand;
global using SQLiteDataReader = OasysGhSQLite_1_2_5::System.Data.SQLite.SQLiteDataReader;
#else
global using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
global using SQLiteCommand = System.Data.SQLite.SQLiteCommand;
global using SQLiteDataReader = System.Data.SQLite.SQLiteDataReader;
#endif
