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
   'Ψһ��ʶ',
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
   '�˵�����',
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
   '�ϼ��˵���ʶ',
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
   '��Ӧ���ӵ�ַ',
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
   '�Ƿ�ɾ��',
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
   '������',
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
   'Ψһ��ʶ',
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
   '��ť����',
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
   '�Ƿ�ɾ��',
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
   '������',
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
   '�ñ���¼��ĳ��Ȩ�޻���ĳ����ɫ��ӵ�е���Ȩ
   ���� 
   xx��ɫӵ��xx�˵�
   xx�û�ӵ��xx��ť
   xx��ɫӵ��xx��ť', 
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
   'Ψһ��ʶ',
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
   '���ø���Ȩ�����Ķ���
   
   �磬
   
   �����Ȩ�����ڽ�ɫ�ģ���ô����ֶα�ʾΪrole
   �����Ȩ�������û��ģ���ô����ֶα�ʾΪuser',
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
   '��Ӧ����Ȩ�����ߵ�Ψһ��ʶ
   ����Ȩ��������role
   ����ֶξ��Ǽ�¼�Ķ�Ӧ����Ȩ�����ߵ�ID',
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
   '��Ȩ���ͱ�ʶ
   ���ֶα�ʶ�������Ȩ������
   ���磺
   ����һ���˵���Ȩ����������menu����ʶ
   ����һ����ť��Ȩ����������button����ʶ',
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
   '��Ӧ����Ȩ
   ��
   ��Ӧ�Ĳ˵�ID
   ��Ӧ�İ�ťID',
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
   '��ʶ�����Ȩ�����ߵ�����
   ����Ȩ���������Ӧ����ָ�������Ȩ����������
   �磬����Ȩ�Ǹ����û��ģ�����user����ʶ
   �磬����Ȩ�Ǹ����ɫ�ģ�����role����ʶ',
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
   '��ʶ�����Ȩ�������ĸ���',
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
   '�Ƿ�Ҫ���ø���Ȩ 0Ϊ������ 1Ϊ����',
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
   '�Ƿ�ɾ�� 0 δɾ�� 1ɾ��',
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
   'Ψһ��ʶ',
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
   '��ɫ����',
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
   '�Ƿ����',
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
   '����ʱ��',
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
   '�޸�ʱ��',
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
   '�Ƿ�ɾ��',
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
   '������',
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
   'Ψһ��ʶ',
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
   '��¼��',
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
   '��¼����',
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
   '�û�����',
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
   '�û�״̬',
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
   '�û�����',
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
   '�û��ǳ�',
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
   '����ʱ��',
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
   '����¼ʱ��',
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
   '����ʡ',
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
   '������',
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
   '���ڵ���',
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
   '��ϸ��ַ',
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
   '�Ա�',
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
   '�Ƿ�ɾ��',
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
   'Ψһ��ʶ',
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
   '�û���ID',
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
   '��ɫ��ID',
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
   '�Ƿ�ɾ��',
   'user', @CurrentUser, 'table', 'EHECD_SystemUser_R_Role', 'column', 'bIsDeleted'
go

