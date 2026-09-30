# Chronosheet · 个人时间记录工具

轻量级 C# WinForms (.NET 8, Windows Desktop) 桌面小工具：**单窗口、纯本地、启动快、无登录、无网络请求**。
集成「正/倒计时计时器」「24 小时分段时间记录」「日历视图回看」三大功能；SQLite 本地存储，ClosedXML 一键导出 Excel。

## ✨ 功能一览

| 模块 | 说明 |
|---|---|
| 🔘 计时器 | 正计时 / 倒计时切换；倒计时归零弹窗 + 系统提示音；一键进入「迷你置顶模式」（无边框小窗，可拖动，始终在最前） |
| 📝 小时记录 | 24 行小时分段表格；**编辑即自动保存**（无需点保存按钮）；支持**连续多时段合并/拆分/同步内容**（合并组视觉高亮，复制/导出/日历视图统一按 `08:00~11:00` 格式）；一键复制到剪贴板；一键导出为 `.xlsx` |
| 📅 日历视图 | 左侧 `MonthCalendar` 点选日期，右侧显示该天非空记录（合并组显示区间） |
| 💾 数据存储 | SQLite 单表 `HourlyRecords`，DB 文件 `time_records.db` 与 exe 同目录，首次启动自动建库建表 |
| 🌏 UI | 中文界面，Microsoft YaHei UI 9pt，浅色扁平主题 |

## 📁 项目结构

```
Chronosheet/
├── Chronosheet.csproj              # .NET 8 Windows Forms 项目，引用：
│                                   #   · Microsoft.Data.Sqlite 8.0.10
│                                   #   · ClosedXML 0.102.2
├── Program.cs                      # 入口：建库 → 启动主窗体
├── DbHelper.cs                     # SQLite 静态封装：建表 / 按日加载 / 日级 UPSERT / 非空记录查询
├── MainForm.Designer.cs            # 主窗体设计器：3 个 Tab（计时器 / 小时记录 / 日历视图）
├── MainForm.cs                     # 主窗体业务：计时器逻辑 + 自动保存 + 合并/拆分/同步 + 迷你模式切换
├── MiniTimerForm.Designer.cs       # 迷你计时器窗口设计器（无边框、深色主题）
├── MiniTimerForm.cs                # 迷你窗口逻辑：可拖动、还原主窗、关闭、每秒刷新时间
├── chronsheet.ico                  # 程序图标（.csproj 中 ApplicationIcon + 窗体 Icon 均使用）
├── chronsheet.jpg                  # 图标源图（可选保留）
├── .gitignore                      # 忽略 bin/obj/*.db/IDE 临时文件等
└── README.md                       # 本文件
```

## 🚀 运行与发布

### 环境要求
- Windows 10 / 11 x64
- 开发机需安装 [.NET 8 SDK](https://dotnet.microsoft.com/zh-cn/download/dotnet/8.0)
- 发布为 `--self-contained false` 时，目标机需安装 **.NET 8 Desktop Runtime**；`true` 时无需任何运行时（单包体积较大）

### 本地运行
```bash
cd Chronosheet
dotnet restore
dotnet build
dotnet run
```

### 发布 · 框架依赖（推荐，体积小）
```bash
dotnet publish -c Release -r win-x64 --self-contained false /p:PublishSingleFile=true
```
输出目录：`bin\Release\net8.0-windows\win-x64\publish\`  
把该目录下的 `Chronosheet.exe`、`chronsheet.ico` 拷到同一文件夹，双击即可运行；首次启动会在同目录生成 `time_records.db`。

### 发布 · 独立部署（无需运行时）
```bash
dotnet publish -c Release -r win-x64 --self-contained true /p:PublishSingleFile=true /p:IncludeNativeLibrariesForSelfExtract=true
```

## 🗃️ 数据库结构（单表）

表名：`HourlyRecords`

| 字段 | 类型 | 说明 |
|---|---|---|
| `Date` | TEXT NOT NULL | 日期，格式 `yyyy-MM-dd` |
| `Hour` | INTEGER NOT NULL | 小时，取值 `0~23` |
| `Content` | TEXT NOT NULL DEFAULT '' | 该小时的内容记录 |
| `UpdatedAt` | TEXT NOT NULL | 最后更新时间，ISO 8601 (`DateTime.Now.ToString("o")`) |
| **主键** | `(Date, Hour)` | 按日+小时唯一，保存时使用 SQLite `UPSERT` 语义 |
