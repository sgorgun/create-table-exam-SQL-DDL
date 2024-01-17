﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using AutocodeDB.Models;
using AutocodeDB.Parsers;
using AutocodeDB.SQLTemplates;

namespace AutocodeDB.Helpers
{
    public static class CreateTableHelper
    {
        private static RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase;
        private static readonly Regex CreateRegExp = new Regex(CreateTableEntity.CreateTable,options);
        private static readonly Regex PrimaryKeyRegExp = new Regex(CreateTableEntity.PrimaryKey,options);
        private static readonly Regex ForeignKeyRegExp = new Regex(CreateTableEntity.ForeignKey,options);
        private static readonly Regex UniqueKeyRegExp = new Regex(CreateTableEntity.UniqueKey,options);
        private static readonly Regex OnDeleteRegExp = new Regex(CreateTableEntity.OnDelete,options);

        private static Dictionary<string, DbTable>? tableMap;


        public static IEnumerable<DbTable> DbTables
        {
            get
            {
                return tableMap.Values.ToArray();
            }
        }
        public static int TablesWithForeignKeys(int numberFK)
        {
            int result = 0;
            foreach(var table in DbTables)
            {
                if(table.ForeignKeys.Count == numberFK)
                    ++result;
            }
            return result;
        }
        public static void ValidateConstrainKeyIntegrity(string query, IEnumerable<string> queries)
        {
            LoadTables(queries);
            var table = QueryParser.ParseTable(query);
            if (tableMap == null)
            {
                throw new ArgumentException($"Incorrect table list given{query}");
            }
            else
            {
                foreach (var fk in table.ForeignKeys)
                {
                    var sequenceNumber = tableMap[table.TableName].SequenceNumber;
                    var refTable = tableMap[fk.RefTable];
                    if (sequenceNumber < refTable.SequenceNumber)
                        throw new ArgumentException($"Table '{fk.RefTable}' must be created before table '{table.TableName}'.");
                    if (!refTable.ColumnList.ContainsKey(fk.RefColumn))
                        throw new ArgumentException($"Foreign key '{fk.LocalColumn}' in table '{table.TableName}' REFERENCES not existing column '{fk.RefColumn}' in table '{fk.RefTable}'.");
                    if (refTable.ColumnList[fk.RefColumn] != table.ColumnList[fk.LocalColumn])
                        throw new ArgumentException($"Column '{table.TableName}.{fk.LocalColumn}' and '{refTable.TableName}.{fk.RefColumn}'\n have different types ({table.ColumnList[fk.LocalColumn]}!={refTable.ColumnList[fk.RefColumn]}).");
                }
            }
        }
        
        public static IEnumerable<string> GetOnlyQueriesWithForeignKeys(IEnumerable<string> queries) => queries.Where(ContainsForeignKey);
        public static bool ContainsCreateTableStatement(string query) => CreateRegExp.IsMatch(query);
        public static bool ContainsPrimaryKey(string query) => PrimaryKeyRegExp.IsMatch(query);
        public static bool ContainsForeignKey(string query) => ForeignKeyRegExp.IsMatch(query);
        public static bool ContainsUniqueKey(string query) => UniqueKeyRegExp.IsMatch(query);

        public static bool ContainsOnDeleteCascade(string query) => OnDeleteRegExp.IsMatch(query);

        public static void LoadTables(IEnumerable<string> queries)
        {
            if (tableMap is { }) 
                return;
            tableMap = new Dictionary<string, DbTable>(13);
            int i = 0;
            foreach(var query in queries)
            {
                var table = QueryParser.ParseTable(query);
                table.SequenceNumber = i;
                ++i;
                if(tableMap.ContainsKey(table.TableName))  
                    throw new ArgumentException($"Table {table.TableName} alredy exists in Table Map.");
                tableMap.Add(table.TableName, table);
            }
        }
        public static void ResetMap()
        {
            tableMap=null; 
        }

        public static string MapToString()
        {
            if(tableMap==null)
                return "string.Empty";
            return string.Join(",", tableMap.Keys);
        }
    }
}