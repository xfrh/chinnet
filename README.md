# CHINET 医疗管理系统

## 项目简介

CHINET（中国细菌耐药性监测网）医疗管理系统是一个基于.NET Framework的企业级医疗数据管理平台，主要用于医院细菌耐药性数据的收集、分析和报告。

## 技术栈

- **.NET Framework 4.8**
- **ASP.NET MVC 5**
- **ASP.NET Web API**
- **Entity Framework 6.1.3**
- **Dapper 1.50.4**
- **Autofac 3.5.2** (依赖注入)
- **Redis** (缓存)
- **SQL Server** (数据库)
- **log4net** (日志)
- **Bootstrap** (UI框架)
- **jQuery** (JavaScript库)

## 项目结构

```
CHINET/
├── Api/                          # API层
│   ├── WebApi/                   # 对外Web API
│   └── InnerWebApi/             # 内部Web API
├── Libraries/                    # 核心库层
│   ├── ManageSystem.Core/       # 核心业务逻辑
│   ├── ManageSystem.Data/       # 数据访问层
│   └── ManageSystem.Services/   # 业务服务层
├── Presentation/                # 表现层
│   ├── ManageSystem.Web/        # 主Web应用
│   ├── ManageSystem.Admin/      # 管理后台
│   ├── ManageSystem.Mobile/     # 移动端应用
│   ├── ManageSystem.InnerApi/   # 内部API
│   └── ManageSystem.Framework/  # 框架层
├── WindowsService/              # Windows服务
│   └── QuartzService/          # 定时任务服务
├── ConsoleApplication1/         # 控制台应用
├── database/                    # 数据库脚本
│   └── chinets.sql             # 数据库结构脚本
└── packages/                    # NuGet包
```

## 功能特性

### 核心功能
- **用户管理**: 用户注册、登录、权限管理
- **医院管理**: 医院信息维护、数据关联
- **数据上传**: 支持DBF格式文件上传，特定命名规范
- **数据分析**: 细菌耐药性数据统计分析
- **报告生成**: 自动生成分析报告
- **系统监控**: 操作日志、系统状态监控

### 技术特性
- **分层架构**: 清晰的分层设计，便于维护
- **依赖注入**: 使用Autofac实现IoC容器
- **缓存机制**: Redis缓存提升性能
- **定时任务**: Quartz.NET实现自动化任务
- **日志系统**: log4net记录系统日志
- **API接口**: RESTful API设计

## 环境要求

### 开发环境
- Visual Studio 2017 或更高版本
- .NET Framework 4.8
- SQL Server 2012 或更高版本
- Redis 3.0 或更高版本

### 运行环境
- Windows Server 2012 或更高版本
- IIS 7.5 或更高版本
- .NET Framework 4.8
- SQL Server 2012 或更高版本
- Redis 3.0 或更高版本

## 安装部署

### 1. 数据库配置
```sql
-- 创建数据库
CREATE DATABASE ChinetsDB;

-- 执行数据库脚本
-- 运行 database/chinets.sql
```

### 2. 配置文件
修改各项目的Web.config文件中的连接字符串：
```xml
<connectionStrings>
    <add name="ManageSystemContext" 
         connectionString="Data Source=.;Initial Catalog=ChinetsDB;User ID=sa;Password=123456" 
         providerName="System.Data.SqlClient" />
</connectionStrings>
```

### 3. Redis配置
确保Redis服务运行在127.0.0.1:6379，密码为888888

### 4. 编译部署
1. 使用Visual Studio打开HuiruiMedical.sln
2. 还原NuGet包
3. 编译解决方案
4. 发布到IIS

## 启动项目

### 从Visual Studio启动
1. 打开HuiruiMedical.sln
2. 设置ManageSystem.Web为启动项目
3. 按F5启动调试

### 多项目启动
1. 右键解决方案 → 属性 → 启动项目
2. 选择"多个启动项目"
3. 设置：
   - ManageSystem.Web: 启动
   - WebApi: 启动
   - InnerWebApi: 启动

## 数据上传规范

### 文件命名规范
- **原始文件**: 上传的原始文件名_医院名称.后缀
- **数据处理文件**: 
  - 上半年: w+年份2位+16+医院名称.DBF
  - 下半年: w+年份2位+712+医院名称.DBF
  - 全年: w+年份2位+T+医院名称.DBF

### 示例
- 2018年全年，医院代码hst: w18thst.dbf
- 2019年上半年，医院名称"北京医院": w1916北京医院.dbf

## API接口

### 主要API端点
- `/api/User` - 用户管理
- `/api/Hospital` - 医院管理
- `/api/Data` - 数据管理
- `/api/Report` - 报告生成

## 开发指南

### 项目依赖关系
```
ManageSystem.Core (基础层)
    ↓
ManageSystem.Data (数据层)
    ↓
ManageSystem.Services (服务层)
    ↓
Presentation Layer (表现层)
```

### 添加新功能
1. 在Core层定义实体和接口
2. 在Data层实现数据访问
3. 在Services层实现业务逻辑
4. 在Presentation层实现用户界面

## 许可证

本项目为内部项目，版权归相关医疗机构所有。

## 联系方式

如有问题，请联系项目维护团队。

## 更新日志

### v1.0.0 (2025-07-03)
- 初始版本发布
- 基础功能实现
- 数据库结构建立
