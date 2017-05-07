/*==============================================================*/
/* DBMS name:      Microsoft SQL Server 2008                    */
/* Created on:     2016/8/28 8:29:19                            */
/*==============================================================*/


if exists (select 1
            from  sysobjects
           where  id = object_id('EHECD_FunctionMenu')
            and   type = 'U')
   drop table EHECD_FunctionMenu
go

if exists (select 1
            from  sysobjects
           where  id = object_id('EHECD_MenuButton')
            and   type = 'U')
   drop table EHECD_MenuButton
go

if exists (select 1
            from  sysobjects
           where  id = object_id('EHECD_Privilege')
            and   type = 'U')
   drop table EHECD_Privilege
go

if exists (select 1
            from  sysobjects
           where  id = object_id('EHECD_Role')
            and   type = 'U')
   drop table EHECD_Role
go

if exists (select 1
            from  sysobjects
           where  id = object_id('EHECD_SystemUser')
            and   type = 'U')
   drop table EHECD_SystemUser
go

if exists (select 1
            from  sysobjects
           where  id = object_id('EHECD_SystemUser_R_Role')
            and   type = 'U')
   drop table EHECD_SystemUser_R_Role
go

/*==============================================================*/
/* Table: EHECD_FunctionMenu                                    */
/*==============================================================*/
create table EHECD_FunctionMenu (
   ID                   uniqueidentifier     not null default newid(),
   sMenuName            nvarchar(20)         not null default '',
   sPID                 uniqueidentifier     null,
   sUrl                 nvarchar(50)         not null default '',
   bIsDeleted           bit                  not null default 0
      constraint CKC_BISDELETED_EHECD_FU check (bIsDeleted between 0 and 1),
   iOrder               int                  not null default 0,
   constraint PK_EHECD_FUNCTIONMENU primary key (ID)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_FunctionMenu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'ID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '唯一标识',
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'ID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_FunctionMenu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sMenuName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'sMenuName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '菜单名称',
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'sMenuName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_FunctionMenu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sPID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'sPID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '上级菜单标识',
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'sPID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_FunctionMenu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sUrl')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'sUrl'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '对应链接地址',
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'sUrl'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_FunctionMenu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bIsDeleted')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'bIsDeleted'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否删除',
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'bIsDeleted'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_FunctionMenu')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'iOrder')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'iOrder'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '排序编号',
   'user', @CurrentUser, 'table', 'EHECD_FunctionMenu', 'column', 'iOrder'
go

/*==============================================================*/
/* Table: EHECD_MenuButton                                      */
/*==============================================================*/
create table EHECD_MenuButton (
   ID                   uniqueidentifier     not null default newid(),
   sButtonName          nvarchar(20)         not null default '',
   bIsDeleted           bit                  not null default 0
      constraint CKC_BISDELETED_EHECD_ME check (bIsDeleted between 0 and 1),
   iOrder               int                  not null default 0,
   constraint PK_EHECD_MENUBUTTON primary key (ID)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_MenuButton')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'ID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '唯一标识',
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'ID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_MenuButton')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sButtonName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'sButtonName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '按钮名称',
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'sButtonName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_MenuButton')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bIsDeleted')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'bIsDeleted'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否删除',
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'bIsDeleted'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_MenuButton')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'iOrder')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'iOrder'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '排序编号',
   'user', @CurrentUser, 'table', 'EHECD_MenuButton', 'column', 'iOrder'
go

/*==============================================================*/
/* Table: EHECD_Privilege                                       */
/*==============================================================*/
create table EHECD_Privilege (
   ID                   uniqueidentifier     not null default newid(),
   sPrivilegeMaster     varchar(15)          not null default '',
   sPrivilegeMasterValue uniqueidentifier     not null default newid(),
   sPrivilegeAccess     varchar(15)          not null default '',
   sPrivilegeAccessValue uniqueidentifier     not null default newid(),
   sBelong              varchar(15)          not null default '',
   sBelongValue         uniqueidentifier     not null default newid(),
   bPrivilegeOperation  bit                  not null default 0,
   bIsDeleted           bit                  not null default 0
      constraint CKC_BISDELETED_EHECD_PR check (bIsDeleted between 0 and 1),
   constraint PK_EHECD_PRIVILEGE primary key (ID)
)
go

if exists (select 1 from  sys.extended_properties
           where major_id = object_id('EHECD_Privilege') and minor_id = 0)
begin 
   declare @CurrentUser sysname 
select @CurrentUser = user_name() 
execute sp_dropextendedproperty 'MS_Description',  
   'user', @CurrentUser, 'table', 'EHECD_Privilege' 
 
end 


select @CurrentUser = user_name() 
execute sp_addextendedproperty 'MS_Description',  
   '该表将记录下某种权限或者某个角色所拥有的特权
   比如 
   xx角色拥有xx菜单
   xx用户拥有xx按钮
   xx角色拥有xx按钮', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'ID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '唯一标识',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'ID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sPrivilegeMaster')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeMaster'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '配置该特权所属的对象
   
   如，
   
   这个特权是属于角色的，那么这个字段表示为role
   这个特权是属于用户的，那么这个字段表示为user',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeMaster'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sPrivilegeMasterValue')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeMasterValue'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '对应的特权所有者的唯一标识
   如特权所有者是role
   则该字段就是记录的对应的特权所有者的ID',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeMasterValue'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sPrivilegeAccess')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeAccess'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '特权类型标识
   该字段标识了这个特权的类型
   比如：
   这是一个菜单特权，则这里用menu来标识
   这是一个按钮特权，则这里用button来标识',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeAccess'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sPrivilegeAccessValue')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeAccessValue'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '对应的特权
   如
   对应的菜单ID
   对应的按钮ID',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sPrivilegeAccessValue'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sBelong')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sBelong'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '标识这个特权所有者的类型
   与特权所属对象对应，以指定这个特权所属的类型
   如，该特权是赋予用户的，则用user来标识
   如，该特权是赋予角色的，则用role来标识',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sBelong'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sBelongValue')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sBelongValue'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '标识这个特权是属于哪个的',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'sBelongValue'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bPrivilegeOperation')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'bPrivilegeOperation'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否要禁用该特权 0为不禁用 1为禁用',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'bPrivilegeOperation'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Privilege')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bIsDeleted')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'bIsDeleted'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否删除 0 未删除 1删除',
   'user', @CurrentUser, 'table', 'EHECD_Privilege', 'column', 'bIsDeleted'
go

/*==============================================================*/
/* Table: EHECD_Role                                            */
/*==============================================================*/
create table EHECD_Role (
   ID                   uniqueidentifier     not null default newid(),
   sRoleName            nvarchar(20)         not null default '',
   bEnable              bit                  not null default 0,
   dCreateTime          datetime             not null default getdate(),
   dModifyTime          datetime             not null default getdate(),
   bIsDeleted           bit                  not null default 0
      constraint CKC_BISDELETED_EHECD_RO check (bIsDeleted between 0 and 1),
   iOrder               int                  not null default 0,
   constraint PK_EHECD_ROLE primary key (ID)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'ID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '唯一标识',
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'ID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sRoleName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'sRoleName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色名称',
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'sRoleName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bEnable')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'bEnable'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否可用',
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'bEnable'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'dCreateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'dCreateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'dCreateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'dModifyTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'dModifyTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '修改时间',
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'dModifyTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bIsDeleted')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'bIsDeleted'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否删除',
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'bIsDeleted'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'iOrder')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'iOrder'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '排序编号',
   'user', @CurrentUser, 'table', 'EHECD_Role', 'column', 'iOrder'
go

/*==============================================================*/
/* Table: EHECD_SystemUser                                      */
/*==============================================================*/
create table EHECD_SystemUser (
   ID                   uniqueidentifier     not null default newid(),
   sLoginName           nvarchar(20)         not null default '',
   sPassWord            nvarchar(50)         not null default '',
   sUserName            nvarchar(15)         not null default '',
   tUserState           tinyint              not null default 0
      constraint CKC_TUSERSTATE_EHECD_SY check (tUserState between 0 and 10),
   tUserType            tinyint              not null default 0
      constraint CKC_TUSERTYPE_EHECD_SY check (tUserType between 0 and 10),
   sUserNickName        nvarchar(20)         not null default '',
   dCreateTime          datetime             not null default getdate(),
   dLastLoginTime       datetime             not null default getdate(),
   sProvice             nvarchar(20)         not null default '',
   sCity                nvarchar(20)         not null default '',
   sCounty              nvarchar(20)         not null default '',
   sAddress             nvarchar(30)         not null default '',
   tSex                 tinyint              not null default 0
      constraint CKC_TSEX_EHECD_SY check (tSex between 0 and 60),
   bIsDeleted           bit                  not null default 0,
   constraint PK_EHECD_SYSTEMUSER primary key (ID)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'ID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '唯一标识',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'ID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sLoginName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sLoginName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '登录名',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sLoginName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sPassWord')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sPassWord'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '登录密码',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sPassWord'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sUserName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sUserName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户姓名',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sUserName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'tUserState')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'tUserState'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户状态',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'tUserState'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'tUserType')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'tUserType'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户类型',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'tUserType'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sUserNickName')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sUserNickName'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户昵称',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sUserNickName'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'dCreateTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'dCreateTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '创建时间',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'dCreateTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'dLastLoginTime')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'dLastLoginTime'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '最后登录时间',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'dLastLoginTime'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sProvice')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sProvice'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '所在省',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sProvice'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sCity')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sCity'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '所在市',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sCity'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sCounty')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sCounty'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '所在地区',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sCounty'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sAddress')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sAddress'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '详细地址',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'sAddress'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'tSex')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'tSex'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '性别',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'tSex'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bIsDeleted')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'bIsDeleted'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否删除',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser', 'column', 'bIsDeleted'
go

/*==============================================================*/
/* Table: EHECD_SystemUser_R_Role                               */
/*==============================================================*/
create table EHECD_SystemUser_R_Role (
   ID                   uniqueidentifier     not null default newid(),
   sUserID              uniqueidentifier     not null default newid(),
   sRoleID              uniqueidentifier     not null default newid(),
   bIsDeleted           bit                  not null default 0,
   constraint PK_EHECD_SYSTEMUSER_R_ROLE primary key (ID)
)
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser_R_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'ID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'ID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '唯一标识',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'ID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser_R_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sUserID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'sUserID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '用户的ID',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'sUserID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser_R_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'sRoleID')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'sRoleID'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '角色的ID',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'sRoleID'
go

if exists(select 1 from sys.extended_properties p where
      p.major_id = object_id('EHECD_SystemUser_R_Role')
  and p.minor_id = (select c.column_id from sys.columns c where c.object_id = p.major_id and c.name = 'bIsDeleted')
)
begin
   declare @CurrentUser sysname
select @CurrentUser = user_name()
execute sp_dropextendedproperty 'MS_Description', 
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'bIsDeleted'

end


select @CurrentUser = user_name()
execute sp_addextendedproperty 'MS_Description', 
   '是否删除',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'bIsDeleted'
go

