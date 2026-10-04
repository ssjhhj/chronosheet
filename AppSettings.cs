using System;
using System.Drawing;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using Microsoft.Win32;

namespace Chronosheet
{
    /// <summary>
    /// 应用设置：持久化到 exe 同目录 settings.json
    /// 负责开机自启动（HKCU 注册表 Run 键）的读写 + 迷你模式外观
    /// </summary>
    public sealed class AppSettings
    {
        private const string RegRunKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
        private const string AppRegName = "Chronosheet";
        private static readonly string SettingsPath =
            Path.Combine(AppContext.BaseDirectory, "settings.json");

        private static readonly JsonSerializerOptions JsonOpts = new JsonSerializerOptions
        {
            WriteIndented = true,
            ReadCommentHandling = JsonCommentHandling.Skip,
            AllowTrailingCommas = true,
        };

        // 默认迷你模式外观
        private const double DefaultOpacity = 1.0;
        private static readonly Color DefaultBackColor = Color.FromArgb(45, 55, 72);

        [JsonPropertyName("autoStart")]
        public bool AutoStartOnBoot { get; set; } = false;

        /// <summary>不透明度 0.3 ~ 1.0（1=完全不透明）</summary>
        [JsonPropertyName("miniOpacity")]
        public double MiniOpacity { get; set; } = DefaultOpacity;

        /// <summary>迷你模式背景色的 ARGB int（ToArgb）</summary>
        [JsonPropertyName("miniBackColorArgb")]
        public int MiniBackColorArgb { get; set; } = DefaultBackColor.ToArgb();

        [JsonIgnore]
        public Color MiniBackColor
        {
            get => Color.FromArgb(MiniBackColorArgb);
            set => MiniBackColorArgb = value.ToArgb();
        }

        /// <summary>
        /// 从 settings.json 加载；若文件不存在则返回默认值
        /// </summary>
        public static AppSettings Load()
        {
            AppSettings? s = null;
            try
            {
                if (File.Exists(SettingsPath))
                {
                    string json = File.ReadAllText(SettingsPath);
                    if (!string.IsNullOrWhiteSpace(json))
                    {
                        s = JsonSerializer.Deserialize<AppSettings>(json, JsonOpts);
                    }
                }
            }
            catch
            {
                // 读取失败回退到默认
            }

            s ??= new AppSettings();

            // 注册表实际值是最终真相（覆盖 JSON 里记录的 autoStart）
            s.AutoStartOnBoot = s.IsAutoStartInRegistry();

            // 合法范围夹紧
            if (s.MiniOpacity < 0.3) s.MiniOpacity = 0.3;
            if (s.MiniOpacity > 1.0) s.MiniOpacity = 1.0;
            if (s.MiniBackColorArgb == 0) s.MiniBackColorArgb = DefaultBackColor.ToArgb();

            return s;
        }

        /// <summary>
        /// 保存到 settings.json
        /// </summary>
        public void Save()
        {
            try
            {
                string json = JsonSerializer.Serialize(this, JsonOpts);
                File.WriteAllText(SettingsPath, json);
            }
            catch
            {
                // 忽略写入失败
            }
        }

        /// <summary>
        /// 检查注册表中当前是否已开启自启动
        /// </summary>
        public bool IsAutoStartInRegistry()
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(RegRunKey, false);
                if (key == null) return false;
                object? val = key.GetValue(AppRegName);
                if (val == null) return false;
                string exePath = GetExePathForRegistry();
                return string.Equals(val.ToString()?.Trim('"'), exePath, StringComparison.OrdinalIgnoreCase);
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 设置开机自启动开关
        /// </summary>
        /// <returns>是否操作成功</returns>
        public bool SetAutoStart(bool enable)
        {
            try
            {
                using var key = Registry.CurrentUser.CreateSubKey(RegRunKey, true);
                if (key == null) return false;

                string exePath = GetExePathForRegistry();

                if (enable)
                {
                    key.SetValue(AppRegName, exePath, RegistryValueKind.String);
                }
                else
                {
                    if (key.GetValue(AppRegName) != null)
                        key.DeleteValue(AppRegName, false);
                }

                AutoStartOnBoot = enable;
                Save();
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static string GetExePathForRegistry()
        {
            string? path = Environment.ProcessPath;
            if (string.IsNullOrWhiteSpace(path))
            {
                path = Path.Combine(AppContext.BaseDirectory,
                    System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + ".exe");
            }
            return path;
        }
    }
}
