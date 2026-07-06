# 全局强制语言规则
1. 你所有思考过程、中间日志、工具执行说明、提示文字、输出结果全部使用简体中文，禁止任何英文自然描述；
2. 仅代码、命令、报错码（CSxxxx、dotnet、shell指令）保留英文，解释文字必须中文；
3. 执行工具、编译、类型检查、排查问题时，每一步的Thought、操作说明全部翻译成中文展示；
4. 收到指令fix typecheck errors后，全程中文输出排查、编译、修复流程。

# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Build Commands

```bash
# Build the main solution
dotnet build SMOM.KAIZHONG.sln

# Build a specific project (when VS locks DLLs, use -o to redirect output)
dotnet build Modules/SMES/MES/SIE.MES/SIE.MES.csproj
dotnet build Modules/SMES/MES/SIE.Web.MES/SIE.Web.MES.csproj

# Build bypassing locked DLLs (Visual Studio often holds obj/ files)
dotnet build SMOM.KAIZHONG.sln -o "$env:TEMP\smom_build"

# Restore NuGet packages (uses local nupkgs/ folder as source)
dotnet restore SMOM.KAIZHONG.sln
```

## Test Commands

```bash
# Run xUnit tests
dotnet test Modules/SMES/MES/SIE.xUnit.MES/SIE.xUnit.MES.csproj

# Run a specific test
dotnet test --filter "FullyQualifiedName~ClassName.MethodName"
```

## Architecture

This is a .NET 6 MES (Manufacturing Execution System) solution called SMOM (Smart Manufacturing Operations Management), customized for the KaiZhong factory. It uses a proprietary SIE framework (v10.0.5) distributed via local NuGet packages in `nupkgs/`.

### Solution Structure

- **SMOM.KAIZHONG.sln** — main solution combining all modules
- **SMES.Core.sln / SEDO.Core.sln** — subset solutions for specific subsystems

### Layer Separation

Each business module follows a two-project pattern:
- **SIE.{Module}** — domain layer: entities, controllers, business logic
- **SIE.Web.{Module}** — web/UI layer: view configurations, commands (C# + JS)

### Key Directories

- **Projects/SMOM/** — deployable hosts (WebClient, WebApiHost, WpfClient, ScheduleServer)
- **Modules/Common/** — shared modules (Core, Items, Resources, Warehouses, Equipments, etc.)
- **Modules/SMES/** — MES-specific modules (MES, Barcodes, DashBoards, Reports, etc.)
- **Modules/SEDO/** — Equipment management modules (EMS)
- **Modules/KaiZhong/** — Customer-specific customizations
- **Common/** — shared host bootstrapping code (Startup, Program, Configuration)
- **platform/** — platform-level libraries
- **nupkgs/** — local NuGet package source for SIE framework packages

### SIE Framework Patterns

**Entity definition** (`SIE.{Module}/EntityName/EntityName.cs`):
- `[RootEntity]` for top-level entities, `[ChildEntity]` for children
- `[ConditionQueryType(typeof(...))]` links a query criteria class — only use on root entities, never on `[ChildEntity]`
- Properties use static `Property<T>` registration: `P<Entity>.Register(e => e.Prop)`
- `RegisterReadOnly` for computed properties with dependency tracking
- `RegisterView` for database-computed view properties
- `RegisterRef` / `RegisterRefId` for entity references
- `RegisterList` for parent-child collections
- `EntityConfig<T>` maps entity to database table via `Meta.MapTable("TABLE_NAME").MapAllProperties()`

**View configuration** (`SIE.Web.{Module}/EntityName/EntityNameViewConfig.cs`):
- Inherits `WebViewConfig<TEntity>`
- `ConfigListView()` for list grid configuration
- `ConfigView()` with `DeclareExtendViewGroup` for alternate view groups
- `View.UseCommands(...)` to add toolbar buttons
- `View.Property(p => p.X).ShowInList(width:N)` for column display
- `View.ChildrenProperty(p => p.List).Show(ChildShowInWhere.All).UseViewGroup("ViewName")` for child grids
- `View.UseDetail(columns)` switches to form/detail layout

**Commands** (`SIE.Web.{Module}/EntityName/Commands/`):
- C# class with `[JsCommand("namespace.CommandName")]` attribute for registration
- Corresponding `.js` file with `SIE.defineCommand('namespace.CommandName', {...})` for client behavior
- `extend: 'SIE.cmd.Save'` / `SIE.cmd.Add` etc. for standard operations
- `onSaved(view, res)` callback for post-save logic
- `view._parent.reloadData()` to refresh parent view after child save

**Query criteria** (`SIE.{Module}/EntityName/EntityNameCriteria.cs`):
- `[QueryEntity]` attribute, inherits `Criteria`
- Properties mirror the filterable fields on the root entity
- `Fetch()` method calls the controller to execute the query

**Controller** (`SIE.{Module}/EntityName/EntityNameController.cs`):
- Inherits `DomainController`
- `Query<T>()` builder with `.Where()` chains for conditional filtering
- `.ToList(pagingInfo, eagerLoadOptions)` for execution

### Frontend

The web client uses ExtJS (via the SIE.Web framework). Client-side JS files are embedded resources in `SIE.Web.*` projects and served automatically. Commands, behaviors, and editors are defined as ExtJS classes via `SIE.defineCommand()`.

### NuGet Configuration

All SIE framework packages (version 10.0.5) are sourced from the local `nupkgs/` folder. No external NuGet feeds are configured.
