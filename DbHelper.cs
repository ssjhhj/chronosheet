using System;
using System.Collections.Generic;
using Microsoft.Data.Sqlite;

namespace Chronosheet
{
    /// <summary>
    /// SQLite 数据库操作封装类
    /// </summary>
    public static class DbHelper
    {
        private const string ConnectionString = "Data Source=time_records.db";

        /// <summary>
        /// 首次启动时确保数据库和表存在
        /// </summary>
        public static void EnsureDatabase()
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                CREATE TABLE IF NOT EXISTS HourlyRecords (
                    Date      TEXT NOT NULL,
                    Hour      INTEGER NOT NULL,
                    Content   TEXT NOT NULL DEFAULT '',
                    UpdatedAt TEXT NOT NULL,
                    PRIMARY KEY (Date, Hour)
                )";
            cmd.ExecuteNonQuery();
        }

        /// <summary>
        /// 加载指定日期的 24 小时记录，没有记录的小时 Content 为空字符串
        /// </summary>
        public static Dictionary<int, string> GetDailyRecords(string date)
        {
            var result = new Dictionary<int, string>();
            for (int h = 0; h < 24; h++)
                result[h] = string.Empty;

            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = "SELECT Hour, Content FROM HourlyRecords WHERE Date = @Date";
            cmd.Parameters.AddWithValue("@Date", date);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int hour = reader.GetInt32(0);
                string content = reader.GetString(1);
                if (hour >= 0 && hour < 24)
                    result[hour] = content;
            }
            return result;
        }

        /// <summary>
        /// 批量 upsert 保存当天 24 小时记录
        /// </summary>
        public static void SaveDailyRecords(string date, Dictionary<int, string> records)
        {
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var tx = conn.BeginTransaction();
            using var cmd = conn.CreateCommand();
            cmd.Transaction = tx;
            cmd.CommandText = @"
                INSERT INTO HourlyRecords (Date, Hour, Content, UpdatedAt)
                VALUES (@Date, @Hour, @Content, @UpdatedAt)
                ON CONFLICT(Date, Hour) DO UPDATE SET
                    Content = excluded.Content,
                    UpdatedAt = excluded.UpdatedAt";

            var pDate = cmd.Parameters.Add("@Date", SqliteType.Text);
            var pHour = cmd.Parameters.Add("@Hour", SqliteType.Integer);
            var pContent = cmd.Parameters.Add("@Content", SqliteType.Text);
            var pUpdated = cmd.Parameters.Add("@UpdatedAt", SqliteType.Text);

            string now = DateTime.Now.ToString("o");
            pDate.Value = date;
            pUpdated.Value = now;

            foreach (var kv in records)
            {
                pHour.Value = kv.Key;
                pContent.Value = kv.Value ?? string.Empty;
                cmd.ExecuteNonQuery();
            }
            tx.Commit();
        }

        /// <summary>
        /// 获取指定日期中 Content 非空的小时列表（用于日历视图右侧展示）
        /// </summary>
        public static List<(int Hour, string Content)> GetNonEmptyRecords(string date)
        {
            var list = new List<(int, string)>();
            using var conn = new SqliteConnection(ConnectionString);
            conn.Open();
            using var cmd = conn.CreateCommand();
            cmd.CommandText = @"
                SELECT Hour, Content FROM HourlyRecords
                WHERE Date = @Date AND COALESCE(TRIM(Content), '') <> ''
                ORDER BY Hour";
            cmd.Parameters.AddWithValue("@Date", date);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                list.Add((reader.GetInt32(0), reader.GetString(1)));
            }
            return list;
        }
    }
}
