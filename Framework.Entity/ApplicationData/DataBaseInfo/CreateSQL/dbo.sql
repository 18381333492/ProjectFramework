/*
Navicat SQL Server Data Transfer

Source Server         : 199
Source Server Version : 105000
Source Host           : 10.2.1.199:1433
Source Database       : EHECD_PermissionSystem
Source Schema         : dbo

Target Server Type    : SQL Server
Target Server Version : 105000
File Encoding         : 65001

Date: 2016-09-26 07:36:07
*/


-- ----------------------------
-- Table structure for EHECD_Categories
-- ----------------------------
DROP TABLE [dbo].[EHECD_Categories]
GO
CREATE TABLE [dbo].[EHECD_Categories] (
[PID] uniqueidentifier NULL ,
[sCategoryName] nvarchar(255) NOT NULL DEFAULT '' ,
[sCategoryCaption] nvarchar(255) NOT NULL DEFAULT '' ,
[iOrder] int NOT NULL DEFAULT ((0)) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[dInsertTime] datetime NOT NULL DEFAULT (getdate()) ,
[sImgUri] varchar(MAX) NOT NULL DEFAULT '' ,
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
NULL, NULL)) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'商品分类'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'商品分类'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'PID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'上级ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'PID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'上级ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'PID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'sCategoryName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'商品种类名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'sCategoryName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'商品种类名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'sCategoryName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'sCategoryCaption')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'商品种类简述'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'sCategoryCaption'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'商品种类简述'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'sCategoryCaption'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'iOrder')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'排序'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'iOrder'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'排序'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'iOrder'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'dInsertTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'dInsertTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'dInsertTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'sImgUri')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'分类图片'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'sImgUri'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'分类图片'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'sImgUri'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Categories', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Categories'
, @level2type = 'COLUMN', @level2name = N'ID'
GO

-- ----------------------------
-- Records of EHECD_Categories
-- ----------------------------
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'D2C230AC-4487-46AD-B7F7-654DFF91A7F8', N'fff', N'fff', N'1', N'1', N'2016-09-24 17:21:14.000', N'', N'E57C8ACD-0638-C75A-8B2B-08D3E45C21CC')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E57C8ACD-0638-C75A-8B2B-08D3E45C21CC', N'mm', N'mm', N'1', N'1', N'2016-09-24 17:21:35.000', N'', N'61B2B16D-8878-CAD8-0AA7-08D3E45C2EA0')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'D2C230AC-4487-46AD-B7F7-654DFF91A7F8', N'fsx', N'fsx', N'1', N'1', N'2016-09-24 17:22:16.000', N'', N'60EA3F72-9F36-C7F8-297B-08D3E45C46B3')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'sfsafasdfa', N'asfdasdfasdf', N'1', N'1', N'2016-09-24 17:24:32.000', N'', N'C2325120-1558-C7B3-1B60-08D3E45C981C')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'测试目录2', N'测试目录2', N'0', N'1', N'2016-09-21 17:22:30.123', N'', N'15D9F01B-A74E-4671-91E2-09CC5D77160D')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'88', N'88', N'88', N'1', N'2016-09-24 15:37:44.043', N'', N'2095EF85-4E6F-4A01-96B2-23F0449EF615')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'11', N'11', N'11', N'1', N'2016-09-24 15:36:20.087', N'', N'B17DC60E-03D2-402C-BA24-2C6872E18835')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'33', N'33', N'33', N'1', N'2016-09-24 15:38:28.830', N'', N'B38C913C-5A43-4F7F-AF73-2E71EDC2CF85')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'D3FF250A-CA9B-40E4-AFC8-F2735667F1BF', N'所有种类', N'所有种类', N'0', N'0', N'2016-09-21 17:21:55.523', N'', N'F68D3E4C-391A-4BF5-BC1C-60024F85C224')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'123', N'123', N'123', N'1', N'2016-09-24 15:27:14.090', N'', N'D2C230AC-4487-46AD-B7F7-654DFF91A7F8')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'F68D3E4C-391A-4BF5-BC1C-60024F85C224', N'测试目录1', N'测试目录1', N'0', N'1', N'2016-09-21 17:22:12.677', N'', N'E13F2D16-BC33-403B-9409-68C44FBB6231')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'2095EF85-4E6F-4A01-96B2-23F0449EF615', N'44', N'44', N'44', N'1', N'2016-09-24 15:39:08.280', N'', N'435F1D15-7766-4B1D-AD53-7171D16BF830')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'123', N'123', N'123', N'1', N'2016-09-24 15:26:56.210', N'', N'28C8D263-674B-4E5F-A8D2-83434673AC33')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'15D9F01B-A74E-4671-91E2-09CC5D77160D', N'测试目录3', N'测试目录3', N'0', N'1', N'2016-09-21 17:22:48.323', N'', N'C6BEC6E4-A1A6-4EF1-96A2-AFF20883B50C')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (null, N'根目录', N'根目录', N'0', N'0', N'2016-09-20 22:42:08.553', N'', N'D3FF250A-CA9B-40E4-AFC8-F2735667F1BF')
GO
GO
INSERT INTO [dbo].[EHECD_Categories] ([PID], [sCategoryName], [sCategoryCaption], [iOrder], [bIsDeleted], [dInsertTime], [sImgUri], [ID]) VALUES (N'E13F2D16-BC33-403B-9409-68C44FBB6231', N'123', N'123', N'123', N'1', N'2016-09-24 15:37:08.110', N'', N'5C324E3A-43B7-47E0-99AE-FE2AABC4F75F')
GO
GO

-- ----------------------------
-- Table structure for EHECD_Client
-- ----------------------------
DROP TABLE [dbo].[EHECD_Client]
GO
CREATE TABLE [dbo].[EHECD_Client] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sName] nvarchar(255) NOT NULL DEFAULT '' ,
[sPhone] nvarchar(11) NOT NULL DEFAULT '' ,
[sPassWord] nvarchar(50) NOT NULL DEFAULT '' ,
[sHeadPic] nvarchar(255) NOT NULL DEFAULT '' ,
[iState] int NOT NULL DEFAULT ((1)) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[dAddTime] datetime NOT NULL DEFAULT (getdate()) ,
[dAccountBalance] decimal(18,2) NOT NULL DEFAULT ((0)) ,
[iClientType] int NOT NULL DEFAULT ((0)) ,
[sPayPassWord] nvarchar(50) NOT NULL DEFAULT '' ,
[sPayPassWordGrade] int NOT NULL DEFAULT ((0)) ,
[sMail] nvarchar(50) NOT NULL DEFAULT '' ,
[sRemark] nvarchar(MAX) NOT NULL DEFAULT ('暂无备注') ,
[sRegCode] nvarchar(30) NOT NULL DEFAULT '' ,
[dLoginEndTime] datetime NOT NULL DEFAULT (getdate()) ,
[iSource] int NOT NULL DEFAULT ((0)) ,
[sNickName] nvarchar(30) NOT NULL DEFAULT '' ,
[iSex] int NOT NULL DEFAULT ((0)) ,
[sQQ] nvarchar(30) NOT NULL DEFAULT '' ,
[dBirthday] datetime NOT NULL DEFAULT (getdate()) ,
[iAge] int NOT NULL DEFAULT ((0)) ,
[sEduBackground] nvarchar(30) NOT NULL DEFAULT '' ,
[sPofessional] nvarchar(50) NOT NULL DEFAULT '' ,
[dIncome] decimal(12,2) NOT NULL DEFAULT ((0)) ,
[sIDType] nvarchar(50) NOT NULL DEFAULT '' ,
[sIDCard] nvarchar(50) NOT NULL DEFAULT '' ,
[iIntegral] int NOT NULL DEFAULT ((0)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'客户姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'客户姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sPhone')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'电话'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPhone'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'电话'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPhone'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sPassWord')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPassWord'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPassWord'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sHeadPic')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'头像地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sHeadPic'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'头像地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sHeadPic'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'iState')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'1-正常   0-冻结'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iState'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'1-正常   0-冻结'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iState'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'删除标志'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'删除标志'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'dAddTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'添加时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dAddTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'添加时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dAddTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'dAccountBalance')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'账户余额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dAccountBalance'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'账户余额'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dAccountBalance'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'iClientType')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'0-普通客户'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iClientType'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'0-普通客户'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iClientType'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sPayPassWord')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'支付密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPayPassWord'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'支付密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPayPassWord'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sPayPassWordGrade')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'支付密码安全等级 0.无  1.弱  2.中 3.强'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPayPassWordGrade'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'支付密码安全等级 0.无  1.弱  2.中 3.强'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPayPassWordGrade'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sMail')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'电子邮件'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sMail'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'电子邮件'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sMail'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sRemark')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'备注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sRemark'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'备注'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sRemark'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sRegCode')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'注册邀请码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sRegCode'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'注册邀请码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sRegCode'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'dLoginEndTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'最后登录时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dLoginEndTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'最后登录时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dLoginEndTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'iSource')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'数据来源（1-PC，2-微信，3-安卓，4-IOS）'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iSource'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'数据来源（1-PC，2-微信，3-安卓，4-IOS）'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iSource'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sNickName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'昵称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sNickName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'昵称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sNickName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'iSex')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'性别:1男,2女'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iSex'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'性别:1男,2女'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iSex'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sQQ')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'qq'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sQQ'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'qq'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sQQ'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'dBirthday')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'生日'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dBirthday'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'生日'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dBirthday'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'iAge')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'年龄'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iAge'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'年龄'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iAge'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sEduBackground')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'学历 （文盲 小学 中学 高中  学士  硕士 博士）'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sEduBackground'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'学历 （文盲 小学 中学 高中  学士  硕士 博士）'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sEduBackground'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sPofessional')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'职业'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPofessional'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'职业'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sPofessional'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'dIncome')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'收入'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dIncome'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'收入'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'dIncome'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sIDType')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'证件类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sIDType'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'证件类型'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sIDType'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'sIDCard')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'证件号码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sIDCard'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'证件号码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'sIDCard'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Client', 
'COLUMN', N'iIntegral')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'积分'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iIntegral'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'积分'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Client'
, @level2type = 'COLUMN', @level2name = N'iIntegral'
GO

-- ----------------------------
-- Records of EHECD_Client
-- ----------------------------
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'E8F0121A-5B11-4C10-A82C-06AB4187A101', N'周建1', N'13880422567', N'E10ADC3949BA59ABBE56E057F20F883E', N'/Files/2016-04/20160413173925763.jpg', N'1', N'0', N'2016-03-25 10:23:38.913', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'583E4CF6-E7E7-4ACC-BBA5-293694B55150', N'文大爷', N'13281843979', N'E10ADC3949BA59ABBE56E057F20F883E', N'/Files/2016-04/20160419091225655.jpg', N'1', N'0', N'2016-03-30 16:05:48.850', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'1BF7FBD5-4030-4AE6-96C5-86D5C3CF0249', N'jbgxhgfcv', N'13880422367', N'96E79218965EB72C92A549DD5A330112', N'/Files/2016-04/20160413174834037.jpg', N'1', N'0', N'2016-04-13 17:46:20.260', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'3F746EA3-2B28-4055-AC60-9C532F9739CE', N'', N'13551892614', N'E10ADC3949BA59ABBE56E057F20F883E', N'', N'1', N'0', N'2016-04-21 17:13:56.657', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'10D439D6-1EEA-4DD2-A3F4-B0A9FDE87C96', N'', N'13512345678', N'E10ADC3949BA59ABBE56E057F20F883E', N'', N'1', N'0', N'2016-04-18 14:07:53.500', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'8EB73E78-EB30-4223-BF3C-C1C5108964F9', N'', N'15555555555', N'E10ADC3949BA59ABBE56E057F20F883E', N'', N'1', N'0', N'2016-04-13 16:06:28.913', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'BD7315E9-D9D9-47E5-8D16-C47D27187DFB', N'', N'18380470194', N'E10ADC3949BA59ABBE56E057F20F883E', N'', N'1', N'0', N'2016-04-21 17:06:11.987', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'0201A77B-24F7-4A2F-BEA5-DD8CA12557BC', N'', N'13222222222', N'E10ADC3949BA59ABBE56E057F20F883E', N'', N'1', N'0', N'2016-04-13 16:03:00.493', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'41CDEE5F-CCA6-483D-A0D3-EE62D84E2534', N'', N'18096228185', N'E10ADC3949BA59ABBE56E057F20F883E', N'', N'1', N'0', N'2016-04-25 11:22:38.643', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'45556B18-FB9B-44BD-87B8-F6AFB415C97D', N'光华锦哈哈', N'18780106390', N'E10ADC3949BA59ABBE56E057F20F883E', N'/Files/2016-05/20160503175136222.jpg', N'1', N'0', N'2016-04-14 10:11:23.410', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Client] ([ID], [sName], [sPhone], [sPassWord], [sHeadPic], [iState], [bIsDeleted], [dAddTime], [dAccountBalance], [iClientType], [sPayPassWord], [sPayPassWordGrade], [sMail], [sRemark], [sRegCode], [dLoginEndTime], [iSource], [sNickName], [iSex], [sQQ], [dBirthday], [iAge], [sEduBackground], [sPofessional], [dIncome], [sIDType], [sIDCard], [iIntegral]) VALUES (N'37BF2575-2DC0-4515-88EA-FE64E499EC13', N'', N'13551384875', N'E10ADC3949BA59ABBE56E057F20F883E', N'', N'1', N'0', N'2016-04-21 17:11:38.637', N'.00', N'1', N'', N'0', N'', N'', N'', N'2016-03-25 10:23:38.913', N'0', N'', N'0', N' ', N'2016-03-25 10:23:38.913', N'0', N' ', N' ', N'.00', N'0', N' ', N'0')
GO
GO

-- ----------------------------
-- Table structure for EHECD_ClientAddress
-- ----------------------------
DROP TABLE [dbo].[EHECD_ClientAddress]
GO
CREATE TABLE [dbo].[EHECD_ClientAddress] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sClientID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sUserName] nvarchar(100) NOT NULL DEFAULT '' ,
[sPhone] nvarchar(30) NOT NULL DEFAULT '' ,
[sProvince] nvarchar(20) NOT NULL DEFAULT '' ,
[sCity] nvarchar(20) NOT NULL DEFAULT '' ,
[sCounty] nvarchar(20) NOT NULL DEFAULT '' ,
[sAddress] nvarchar(150) NOT NULL DEFAULT '' ,
[bIsInThreeRound] bit NOT NULL DEFAULT ((0)) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[bIsDefault] bit NOT NULL DEFAULT ((0)) ,
[bIsToVillage] bit NOT NULL DEFAULT ((0)) ,
[dLatitude] decimal(9,6) NOT NULL ,
[dLongtidude] decimal(9,6) NOT NULL 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
NULL, NULL)) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'客户地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'客户地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'sClientID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'客户ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sClientID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'客户ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sClientID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'sUserName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'联系人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sUserName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'联系人'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sUserName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'sPhone')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'联系电话'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sPhone'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'联系电话'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sPhone'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'sProvince')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'省'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sProvince'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'省'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sProvince'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'sCity')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'市'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sCity'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'市'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sCity'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'sCounty')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'区县'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sCounty'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'区县'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sCounty'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'sAddress')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'具体地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sAddress'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'具体地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'sAddress'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'bIsInThreeRound')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否三环内'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'bIsInThreeRound'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否三环内'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'bIsInThreeRound'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'bIsDefault')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否是默认地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'bIsDefault'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否是默认地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'bIsDefault'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'bIsToVillage')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否送达乡镇'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'bIsToVillage'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否送达乡镇'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'bIsToVillage'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'dLatitude')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'维度'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'dLatitude'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'维度'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'dLatitude'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_ClientAddress', 
'COLUMN', N'dLongtidude')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'经度'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'dLongtidude'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'经度'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_ClientAddress'
, @level2type = 'COLUMN', @level2name = N'dLongtidude'
GO

-- ----------------------------
-- Records of EHECD_ClientAddress
-- ----------------------------

-- ----------------------------
-- Table structure for EHECD_FunctionMenu
-- ----------------------------
DROP TABLE [dbo].[EHECD_FunctionMenu]
GO
CREATE TABLE [dbo].[EHECD_FunctionMenu] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sMenuName] nvarchar(20) NOT NULL DEFAULT '' ,
[sPID] uniqueidentifier NULL ,
[sUrl] nvarchar(50) NOT NULL DEFAULT '' ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[iOrder] int NOT NULL DEFAULT ((0)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_FunctionMenu', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_FunctionMenu', 
'COLUMN', N'sMenuName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'菜单名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'sMenuName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'菜单名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'sMenuName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_FunctionMenu', 
'COLUMN', N'sPID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'上级菜单标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'sPID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'上级菜单标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'sPID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_FunctionMenu', 
'COLUMN', N'sUrl')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'对应链接地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'sUrl'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'对应链接地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'sUrl'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_FunctionMenu', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_FunctionMenu', 
'COLUMN', N'iOrder')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'排序编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'iOrder'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'排序编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_FunctionMenu'
, @level2type = 'COLUMN', @level2name = N'iOrder'
GO

-- ----------------------------
-- Records of EHECD_FunctionMenu
-- ----------------------------
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'8D469A2F-070D-CD6E-2650-08D3DF08EEA0', N'商品管理', null, N'', N'0', N'1')
GO
GO
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'商品分类管理', N'8D469A2F-070D-CD6E-2650-08D3DF08EEA0', N'/Admin/GoodsCategory', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'EAA410FB-8019-CC11-B6C5-08D3E2FB0203', N'客户管理', null, N'', N'0', N'2')
GO
GO
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'客户管理', N'EAA410FB-8019-CC11-B6C5-08D3E2FB0203', N'/Admin/ClientManager', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'菜单管理', N'A04BADFD-4F07-46BD-9816-A71EC4776B84', N'/Admin/MenuManage', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'用户管理', N'A04BADFD-4F07-46BD-9816-A71EC4776B84', N'/Admin/SystemUser', N'0', N'2')
GO
GO
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'A04BADFD-4F07-46BD-9816-A71EC4776B84', N'系统管理', null, N'', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_FunctionMenu] ([ID], [sMenuName], [sPID], [sUrl], [bIsDeleted], [iOrder]) VALUES (N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'角色管理', N'A04BADFD-4F07-46BD-9816-A71EC4776B84', N'/Admin/RoleManage', N'0', N'1')
GO
GO

-- ----------------------------
-- Table structure for EHECD_MenuButton
-- ----------------------------
DROP TABLE [dbo].[EHECD_MenuButton]
GO
CREATE TABLE [dbo].[EHECD_MenuButton] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sButtonName] nvarchar(20) NOT NULL DEFAULT '' ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[iOrder] int NOT NULL DEFAULT ((0)) ,
[sIcon] nvarchar(15) NOT NULL DEFAULT ('icon-add') ,
[sDataID] nvarchar(50) NULL DEFAULT '' 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_MenuButton', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_MenuButton', 
'COLUMN', N'sButtonName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'按钮名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'sButtonName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'按钮名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'sButtonName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_MenuButton', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_MenuButton', 
'COLUMN', N'iOrder')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'排序编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'iOrder'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'排序编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'iOrder'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_MenuButton', 
'COLUMN', N'sIcon')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'菜单ICON'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'sIcon'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'菜单ICON'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'sIcon'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_MenuButton', 
'COLUMN', N'sDataID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'标识符'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'sDataID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'标识符'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_MenuButton'
, @level2type = 'COLUMN', @level2name = N'sDataID'
GO

-- ----------------------------
-- Records of EHECD_MenuButton
-- ----------------------------
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'BEFACFC7-E5A7-C33E-C038-08D3D4C348A4', N'删除菜单', N'0', N'2', N'icon-remove', N'del_menu_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'1D6C01C3-6346-C07C-5026-08D3D4C35598', N'编辑菜单', N'0', N'1', N'icon-edit', N'edit_menu_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'9C2E9C1A-4EE4-C683-A4D5-08D3D4C5A173', N'查询角色', N'0', N'0', N'icon-search', N'search_role_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'E9F0409C-E979-CC62-02DD-08D3D4C5B3B3', N'新建角色', N'0', N'1', N'icon-add', N'add_role_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'E35156B3-0D4C-C13B-29D3-08D3D4C6A755', N'删除角色', N'0', N'3', N'icon-remove', N'del_role_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'7D5D83C6-EC1C-CC0F-4C08-08D3D4C6D2E4', N'编辑角色', N'0', N'2', N'icon-edit', N'edit_role_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'6F85B3F8-9ACB-CA9B-F84F-08D3D4C74935', N'查询用户', N'0', N'0', N'icon-search', N'search_user_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'7C5CD6FD-9F29-C92D-853E-08D3D4C7605E', N'新建用户', N'0', N'1', N'icon-add', N'add_systemuser_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'B9FDF00F-FC25-CD11-692D-08D3D4C76C31', N'编辑用户', N'0', N'2', N'icon-edit', N'edit_systemuser_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'7335A1B5-368D-CFBC-6B84-08D3D4C777DE', N'删除用户', N'0', N'3', N'icon-remove', N'del_systemuser_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'B4D8AB85-4AB1-CF48-7C6D-08D3D65FC9E1', N'添加按钮', N'0', N'3', N'icon-add', N'add_menubutton_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'4B36B98F-A75A-C43E-752A-08D3D65FF139', N'编辑按钮', N'0', N'4', N'icon-edit', N'edit_menubutton_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'683BD737-93EB-C199-28A9-08D3D966521D', N'添加菜单', N'0', N'0', N'icon-add', N'add_menu_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'647BB4C1-CDC3-CC87-5262-08D3D96A9D73', N'删除按钮', N'0', N'5', N'icon-remove', N'del_menubutton_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'4256CB77-8426-C2E1-0A45-08D3D9F9EB9D', N'冻结/解冻用户', N'0', N'4', N'icon-filter', N'frozen_systemuser_button')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'0BFF2076-E2C5-C8D3-5663-08D3DC3C5D96', N'分配角色', N'0', N'5', N'icon-clipboard', N'distribution_role_to_systemuser')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'79EE823E-3643-C2CF-D1D1-08D3DC3EC3A3', N'分配菜单权限', N'0', N'4', N'icon-clipboard', N'distribution_menu_authority')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'6F88D006-583A-C51E-B325-08D3DC3EF683', N'分配按钮权限', N'0', N'5', N'icon-clipboard', N'distribution_button_authority')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'C6B14F22-892D-C0C1-77E3-08D3DE202D52', N'物理清除冗余数据', N'0', N'6', N'icon-clear', N'delete_database_deldata')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'051C4BA1-1B3E-C61B-846B-08D3E1FDBC8F', N'添加种类', N'0', N'0', N'icon-add', N'add_goods_category')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'6A2DF70E-E4F9-C63A-E072-08D3E1FDC691', N'编辑种类', N'0', N'1', N'icon-edit', N'edit_goods_category')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'48D39F58-9ACB-C717-A2ED-08D3E1FDD1AD', N'删除种类', N'0', N'2', N'icon-remove', N'del_goods_category')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'4AC8EA60-7EB6-C8CE-BE8C-08D3E2FB30C6', N'查询客户', N'0', N'0', N'icon-search', N'search_client_domain')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'6854A804-7B34-CB0A-A1FC-08D3E2FB3A19', N'编辑客户', N'0', N'1', N'icon-edit', N'edit')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'9783595C-8BC2-C2BF-B624-08D3E2FB4BFF', N'删除客户', N'0', N'2', N'icon-remove', N'delete_client_domain')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'1C282C3A-D839-C047-E493-08D3E2FB7274', N'冻结客户', N'0', N'3', N'icon-filter', N'forzen_client_domain')
GO
GO
INSERT INTO [dbo].[EHECD_MenuButton] ([ID], [sButtonName], [bIsDeleted], [iOrder], [sIcon], [sDataID]) VALUES (N'8B429131-CF53-CFFE-9A2B-08D3E3CB71E5', N'导入客户', N'0', N'4', N'icon-print', N'import_client_domain')
GO
GO

-- ----------------------------
-- Table structure for EHECD_Privilege
-- ----------------------------
DROP TABLE [dbo].[EHECD_Privilege]
GO
CREATE TABLE [dbo].[EHECD_Privilege] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sPrivilegeMaster] varchar(15) NOT NULL DEFAULT '' ,
[sPrivilegeMasterValue] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sPrivilegeAccess] varchar(15) NOT NULL DEFAULT '' ,
[sPrivilegeAccessValue] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sBelong] varchar(15) NOT NULL DEFAULT '' ,
[sBelongValue] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[bPrivilegeOperation] bit NOT NULL DEFAULT ((0)) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
NULL, NULL)) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'该表将记录下某种权限或者某个角色所拥有的特权
   比如 
   xx角色拥有xx菜单
   xx用户拥有xx按钮
   xx角色拥有xx按钮'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'该表将记录下某种权限或者某个角色所拥有的特权
   比如 
   xx角色拥有xx菜单
   xx用户拥有xx按钮
   xx角色拥有xx按钮'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'sPrivilegeMaster')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'配置该特权所属的对象
   
   如，
   
   这个特权是属于角色的，那么这个字段表示为role
   这个特权是属于用户的，那么这个字段表示为user'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeMaster'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'配置该特权所属的对象
   
   如，
   
   这个特权是属于角色的，那么这个字段表示为role
   这个特权是属于用户的，那么这个字段表示为user'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeMaster'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'sPrivilegeMasterValue')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'对应的特权所有者的唯一标识
   如特权所有者是role
   则该字段就是记录的对应的特权所有者的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeMasterValue'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'对应的特权所有者的唯一标识
   如特权所有者是role
   则该字段就是记录的对应的特权所有者的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeMasterValue'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'sPrivilegeAccess')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'特权类型标识
   该字段标识了这个特权的类型
   比如：
   这是一个菜单特权，则这里用menu来标识
   这是一个按钮特权，则这里用button来标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeAccess'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'特权类型标识
   该字段标识了这个特权的类型
   比如：
   这是一个菜单特权，则这里用menu来标识
   这是一个按钮特权，则这里用button来标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeAccess'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'sPrivilegeAccessValue')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'对应的特权
   如
   对应的菜单ID
   对应的按钮ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeAccessValue'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'对应的特权
   如
   对应的菜单ID
   对应的按钮ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sPrivilegeAccessValue'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'sBelong')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'标识这个特权所有者的类型
   与特权所属对象对应，以指定这个特权所属的类型
   如，该特权是赋予用户的，则用user来标识
   如，该特权是赋予角色的，则用role来标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sBelong'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'标识这个特权所有者的类型
   与特权所属对象对应，以指定这个特权所属的类型
   如，该特权是赋予用户的，则用user来标识
   如，该特权是赋予角色的，则用role来标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sBelong'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'sBelongValue')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'标识这个特权是属于哪个的'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sBelongValue'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'标识这个特权是属于哪个的'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'sBelongValue'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'bPrivilegeOperation')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否要禁用该特权 0为不禁用 1为禁用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'bPrivilegeOperation'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否要禁用该特权 0为不禁用 1为禁用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'bPrivilegeOperation'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Privilege', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除 0 未删除 1删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除 0 未删除 1删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Privilege'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO

-- ----------------------------
-- Records of EHECD_Privilege
-- ----------------------------
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'03FFD356-44D7-C9DE-C038-08D3D4C348A4', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'button', N'BEFACFC7-E5A7-C33E-C038-08D3D4C348A4', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'8FC39B5D-3F7E-CD57-5026-08D3D4C35598', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'button', N'1D6C01C3-6346-C07C-5026-08D3D4C35598', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'D3A5B2C3-DB5A-C265-A4D5-08D3D4C5A173', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'button', N'9C2E9C1A-4EE4-C683-A4D5-08D3D4C5A173', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'29E73873-6846-C93A-02DD-08D3D4C5B3B3', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'button', N'E9F0409C-E979-CC62-02DD-08D3D4C5B3B3', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'7012B54D-3AF7-CC55-29D3-08D3D4C6A755', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'button', N'E35156B3-0D4C-C13B-29D3-08D3D4C6A755', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'373F2FF4-078A-C0F3-4C08-08D3D4C6D2E4', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'button', N'7D5D83C6-EC1C-CC0F-4C08-08D3D4C6D2E4', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'48A024A2-4E4C-C145-F84F-08D3D4C74935', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'button', N'6F85B3F8-9ACB-CA9B-F84F-08D3D4C74935', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'633E45F9-1218-C4A1-853E-08D3D4C7605E', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'button', N'7C5CD6FD-9F29-C92D-853E-08D3D4C7605E', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'10907BF1-FCFA-C4FD-692D-08D3D4C76C31', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'button', N'B9FDF00F-FC25-CD11-692D-08D3D4C76C31', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'BF44696D-F290-C14E-6B84-08D3D4C777DE', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'button', N'7335A1B5-368D-CFBC-6B84-08D3D4C777DE', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'35C3F5F7-CB16-CD83-7C6D-08D3D65FC9E1', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'button', N'B4D8AB85-4AB1-CF48-7C6D-08D3D65FC9E1', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'755D0FA1-3780-CB42-752A-08D3D65FF139', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'button', N'4B36B98F-A75A-C43E-752A-08D3D65FF139', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'BBBE4050-D5DA-CA3E-28A9-08D3D966521D', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'button', N'683BD737-93EB-C199-28A9-08D3D966521D', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'7A5C92B3-8BDA-C5FF-5262-08D3D96A9D73', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'button', N'647BB4C1-CDC3-CC87-5262-08D3D96A9D73', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9D30C754-D042-CAA7-0A45-08D3D9F9EB9D', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'button', N'4256CB77-8426-C2E1-0A45-08D3D9F9EB9D', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'78E5FE03-7F55-C05E-5663-08D3DC3C5D96', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'button', N'0BFF2076-E2C5-C8D3-5663-08D3DC3C5D96', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'28F225BD-A483-C57E-D1D1-08D3DC3EC3A3', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'button', N'79EE823E-3643-C2CF-D1D1-08D3DC3EC3A3', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'577C44DF-50D9-CF2D-B325-08D3DC3EF683', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'button', N'6F88D006-583A-C51E-B325-08D3DC3EF683', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'4B3B1A5C-6507-CD6A-77E3-08D3DE202D52', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'button', N'C6B14F22-892D-C0C1-77E3-08D3DE202D52', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'344A2272-54CB-C66E-846B-08D3E1FDBC8F', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'button', N'051C4BA1-1B3E-C61B-846B-08D3E1FDBC8F', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'EF9BA91C-9317-C7C6-E072-08D3E1FDC691', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'button', N'6A2DF70E-E4F9-C63A-E072-08D3E1FDC691', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9DCA649A-6677-C977-A2ED-08D3E1FDD1AD', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'button', N'48D39F58-9ACB-C717-A2ED-08D3E1FDD1AD', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'EE52A52E-06E5-CC13-E5A7-08D3E2FB30C6', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'button', N'4AC8EA60-7EB6-C8CE-BE8C-08D3E2FB30C6', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'3F9CA92E-2959-C198-A1FC-08D3E2FB3A19', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'button', N'6854A804-7B34-CB0A-A1FC-08D3E2FB3A19', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'FAB479AA-ED7B-C46E-B624-08D3E2FB4BFF', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'button', N'9783595C-8BC2-C2BF-B624-08D3E2FB4BFF', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'8139760C-582E-C6D1-E493-08D3E2FB7274', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'button', N'1C282C3A-D839-C047-E493-08D3E2FB7274', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A5C20AE3-A63C-C19A-9A2B-08D3E3CB71E5', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'button', N'8B429131-CF53-CFFE-9A2B-08D3E3CB71E5', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'1B896684-DA7F-E611-8EF1-2C56DCDC514B', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'051C4BA1-1B3E-C61B-846B-08D3E1FDBC8F', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'1C896684-DA7F-E611-8EF1-2C56DCDC514B', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'6A2DF70E-E4F9-C63A-E072-08D3E1FDC691', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'1D896684-DA7F-E611-8EF1-2C56DCDC514B', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'48D39F58-9ACB-C717-A2ED-08D3E1FDD1AD', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'D047B65F-ED7B-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'A04BADFD-4F07-46BD-9816-A71EC4776B84', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'D147B65F-ED7B-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'D247B65F-ED7B-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'D347B65F-ED7B-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'339D689B-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'1D6C01C3-6346-C07C-5026-08D3D4C35598', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'349D689B-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'B4D8AB85-4AB1-CF48-7C6D-08D3D65FC9E1', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'359D689B-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'4B36B98F-A75A-C43E-752A-08D3D65FF139', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'360B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'BEFACFC7-E5A7-C33E-C038-08D3D4C348A4', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'3A0B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'683BD737-93EB-C199-28A9-08D3D966521D', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'3B0B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'647BB4C1-CDC3-CC87-5262-08D3D96A9D73', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'3C0B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'C6B14F22-892D-C0C1-77E3-08D3DE202D52', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'3D0B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'9C2E9C1A-4EE4-C683-A4D5-08D3D4C5A173', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'3E0B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'E9F0409C-E979-CC62-02DD-08D3D4C5B3B3', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'3F0B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'E35156B3-0D4C-C13B-29D3-08D3D4C6A755', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'400B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'7D5D83C6-EC1C-CC0F-4C08-08D3D4C6D2E4', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'410B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'79EE823E-3643-C2CF-D1D1-08D3DC3EC3A3', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'420B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'6F88D006-583A-C51E-B325-08D3DC3EF683', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'430B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'6F85B3F8-9ACB-CA9B-F84F-08D3D4C74935', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'440B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'7C5CD6FD-9F29-C92D-853E-08D3D4C7605E', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'450B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'B9FDF00F-FC25-CD11-692D-08D3D4C76C31', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'460B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'7335A1B5-368D-CFBC-6B84-08D3D4C777DE', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'470B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'4256CB77-8426-C2E1-0A45-08D3D9F9EB9D', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'480B17D2-DE7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'0BFF2076-E2C5-C8D3-5663-08D3DC3C5D96', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F5A0241A-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'A04BADFD-4F07-46BD-9816-A71EC4776B84', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F6A0241A-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F7A0241A-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F8A0241A-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'EEBC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'C6B14F22-892D-C0C1-77E3-08D3DE202D52', N'menu', N'B138CE7D-048E-4293-AEB5-210F55FCB674', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'EFBC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'9C2E9C1A-4EE4-C683-A4D5-08D3D4C5A173', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F0BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'E9F0409C-E979-CC62-02DD-08D3D4C5B3B3', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F1BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'E35156B3-0D4C-C13B-29D3-08D3D4C6A755', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F2BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'7D5D83C6-EC1C-CC0F-4C08-08D3D4C6D2E4', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F3BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'6F85B3F8-9ACB-CA9B-F84F-08D3D4C74935', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F4BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'7C5CD6FD-9F29-C92D-853E-08D3D4C7605E', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F5BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'B9FDF00F-FC25-CD11-692D-08D3D4C76C31', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F6BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'7335A1B5-368D-CFBC-6B84-08D3D4C777DE', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F7BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'4256CB77-8426-C2E1-0A45-08D3D9F9EB9D', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'F8BC1437-DF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'0BFF2076-E2C5-C8D3-5663-08D3DC3C5D96', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A873FB3A-ED7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'8D469A2F-070D-CD6E-2650-08D3DF08EEA0', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A973FB3A-ED7C-E611-8D93-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9140F0E0-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'8D469A2F-070D-CD6E-2650-08D3DF08EEA0', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9240F0E0-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9D3367EB-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'menu', N'A04BADFD-4F07-46BD-9816-A71EC4776B84', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9E3367EB-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9F3367EB-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A03367EB-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'menu', N'8D469A2F-070D-CD6E-2650-08D3DF08EEA0', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A13367EB-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'7BEEF3F3-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'button', N'9C2E9C1A-4EE4-C683-A4D5-08D3D4C5A173', N'menu', N'13663269-A6F3-4E97-8546-E6192E61C5AC', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'7CEEF3F3-EF7C-E611-8D93-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'button', N'6F85B3F8-9ACB-CA9B-F84F-08D3D4C74935', N'menu', N'7E7D35EB-A425-4288-8D0F-9ABEA6EDCEB9', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'6A11AF95-D780-E611-8D94-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'EAA410FB-8019-CC11-B6C5-08D3E2FB0203', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'6B11AF95-D780-E611-8D94-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'8211AF95-D780-E611-8D94-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'4AC8EA60-7EB6-C8CE-BE8C-08D3E2FB30C6', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'8311AF95-D780-E611-8D94-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'6854A804-7B34-CB0A-A1FC-08D3E2FB3A19', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'8411AF95-D780-E611-8D94-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'9783595C-8BC2-C2BF-B624-08D3E2FB4BFF', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'8511AF95-D780-E611-8D94-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'1C282C3A-D839-C047-E493-08D3E2FB7274', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'0DD96A7D-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'EAA410FB-8019-CC11-B6C5-08D3E2FB0203', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'0ED96A7D-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9EB77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'051C4BA1-1B3E-C61B-846B-08D3E1FDBC8F', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'9FB77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'6A2DF70E-E4F9-C63A-E072-08D3E1FDC691', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A0B77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'48D39F58-9ACB-C717-A2ED-08D3E1FDD1AD', N'menu', N'CDCCA5C8-EFF6-C039-E31B-08D3DF09330B', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A1B77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'4AC8EA60-7EB6-C8CE-BE8C-08D3E2FB30C6', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A2B77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'6854A804-7B34-CB0A-A1FC-08D3E2FB3A19', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A3B77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'9783595C-8BC2-C2BF-B624-08D3E2FB4BFF', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'A4B77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'button', N'1C282C3A-D839-C047-E493-08D3E2FB7274', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'AAB77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'menu', N'EAA410FB-8019-CC11-B6C5-08D3E2FB0203', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'ABB77786-9581-E611-8D94-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'78AA6B98-9581-E611-8D94-60A44C3DDDA6', N'role', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'button', N'4AC8EA60-7EB6-C8CE-BE8C-08D3E2FB30C6', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Privilege] ([ID], [sPrivilegeMaster], [sPrivilegeMasterValue], [sPrivilegeAccess], [sPrivilegeAccessValue], [sBelong], [sBelongValue], [bPrivilegeOperation], [bIsDeleted]) VALUES (N'1D868292-A781-E611-8D94-60A44C3DDDA6', N'role', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'button', N'8B429131-CF53-CFFE-9A2B-08D3E3CB71E5', N'menu', N'EB0D9A25-6A72-C217-F524-08D3E2FB14B8', N'0', N'0')
GO
GO

-- ----------------------------
-- Table structure for EHECD_Role
-- ----------------------------
DROP TABLE [dbo].[EHECD_Role]
GO
CREATE TABLE [dbo].[EHECD_Role] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sRoleName] nvarchar(20) NOT NULL DEFAULT '' ,
[bEnable] bit NOT NULL DEFAULT ((0)) ,
[dCreateTime] datetime NOT NULL DEFAULT (getdate()) ,
[dModifyTime] datetime NOT NULL DEFAULT (getdate()) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[iOrder] int NOT NULL DEFAULT ((0)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Role', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Role', 
'COLUMN', N'sRoleName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'角色名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'sRoleName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'角色名称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'sRoleName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Role', 
'COLUMN', N'bEnable')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否可用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'bEnable'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否可用'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'bEnable'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Role', 
'COLUMN', N'dCreateTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'dCreateTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'dCreateTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Role', 
'COLUMN', N'dModifyTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'修改时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'dModifyTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'修改时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'dModifyTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Role', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_Role', 
'COLUMN', N'iOrder')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'排序编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'iOrder'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'排序编号'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_Role'
, @level2type = 'COLUMN', @level2name = N'iOrder'
GO

-- ----------------------------
-- Records of EHECD_Role
-- ----------------------------
INSERT INTO [dbo].[EHECD_Role] ([ID], [sRoleName], [bEnable], [dCreateTime], [dModifyTime], [bIsDeleted], [iOrder]) VALUES (N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'超级管理员', N'1', N'2016-09-15 20:12:17.000', N'2016-09-15 20:12:17.000', N'0', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_Role] ([ID], [sRoleName], [bEnable], [dCreateTime], [dModifyTime], [bIsDeleted], [iOrder]) VALUES (N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'普通用户', N'1', N'2016-09-16 18:39:02.000', N'2016-09-16 18:41:49.000', N'0', N'2')
GO
GO
INSERT INTO [dbo].[EHECD_Role] ([ID], [sRoleName], [bEnable], [dCreateTime], [dModifyTime], [bIsDeleted], [iOrder]) VALUES (N'689A7510-70EF-42CD-9AD1-1611B29061D2', N'管理员', N'1', N'2016-08-28 08:41:35.003', N'2016-09-15 20:12:23.000', N'0', N'1')
GO
GO

-- ----------------------------
-- Table structure for EHECD_SystemLog
-- ----------------------------
DROP TABLE [dbo].[EHECD_SystemLog]
GO
CREATE TABLE [dbo].[EHECD_SystemLog] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sDomainDetail] nvarchar(100) NOT NULL DEFAULT '' ,
[sLoginName] nvarchar(20) NOT NULL DEFAULT '' ,
[sUserName] nvarchar(15) NOT NULL DEFAULT '' ,
[dInsertTime] datetime NOT NULL DEFAULT (getdate()) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[sIPAddress] varchar(25) NOT NULL DEFAULT '' ,
[sDoMainId] varchar(255) NOT NULL DEFAULT '' ,
[tDoType] smallint NOT NULL DEFAULT ((0)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'sDomainDetail')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'操作内容'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sDomainDetail'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'操作内容'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sDomainDetail'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'sLoginName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'登录名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sLoginName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'登录名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sLoginName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'sUserName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'用户姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sUserName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'用户姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sUserName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'dInsertTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'dInsertTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'dInsertTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'sIPAddress')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'登录IP地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sIPAddress'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'登录IP地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sIPAddress'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'sDoMainId')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'操作的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sDoMainId'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'操作的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'sDoMainId'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemLog', 
'COLUMN', N'tDoType')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'操作类型 short'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'tDoType'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'操作类型 short'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemLog'
, @level2type = 'COLUMN', @level2name = N'tDoType'
GO

-- ----------------------------
-- Records of EHECD_SystemLog
-- ----------------------------
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'CCEA921F-9028-C91D-CC76-08D3DEBB49D7', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 13:27:16.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D634A4AA-D56B-CFC1-6816-08D3DEBB7214', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 13:28:24.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'ACC56F96-41D1-CE77-35DB-08D3DEBC89A8', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 13:36:13.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'074782D8-8088-CBC6-A597-08D3DEBCA097', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 13:36:51.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D4B105DC-F151-CF69-D372-08D3DEBCA770', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 13:37:03.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'B3884A9D-2E80-C18B-DB6C-08D3DEBCF963', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 13:39:20.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'9CC62BDD-F592-CAAC-B228-08D3DEBD018D', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 13:39:34.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'87B03E96-EE73-C624-3AF7-08D3DEC9D0FF', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'管理员', N'2016-09-17 15:11:16.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'8ED58C98-D684-C51D-A144-08D3DEC9DBC4', N'系统用户分配角色菜单按钮d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'管理员', N'2016-09-17 15:11:34.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'9E95B12E-1A7D-CF84-06F0-08D3DEC9FED0', N'系统用户添加用户8b087a87-749b-c6d0-31eb-08d3dec9fece', N'admin', N'管理员', N'2016-09-17 15:12:33.000', N'0', N'127.0.0.1', N'8b087a87-749b-c6d0-31eb-08d3dec9fece', N'16392')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'230AF62A-9E95-CFF1-E1FD-08D3DECA096E', N'系统用户编辑用户8b087a87-749b-c6d0-31eb-08d3dec9fece', N'admin', N'管理员', N'2016-09-17 15:12:51.000', N'0', N'127.0.0.1', N'8b087a87-749b-c6d0-31eb-08d3dec9fece', N'4104')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'B267ACA6-7DFF-CDFE-E748-08D3DECA0DF1', N'分配系统用户角色8b087a87-749b-c6d0-31eb-08d3dec9fece', N'admin', N'管理员', N'2016-09-17 15:12:58.000', N'0', N'127.0.0.1', N'8b087a87-749b-c6d0-31eb-08d3dec9fece', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'19B2A021-71D4-C4E5-9799-08D3DECA1686', N'分配系统用户角色685d010c-d3bd-c5c6-db67-08d3dae88320', N'admin', N'管理员', N'2016-09-17 15:13:13.000', N'0', N'127.0.0.1', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'8AEF90C3-EC97-CAC6-FE7F-08D3DECA203F', N'系统用户分配角色菜单d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'管理员', N'2016-09-17 15:13:29.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'89D60012-DEA9-C859-2FF5-08D3DECA24CC', N'系统用户分配角色菜单按钮d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'管理员', N'2016-09-17 15:13:37.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7085F6EC-C67D-C8CC-34AA-08D3DECCA4AB', N'系统用户添加角色e01aacd2-29a2-c135-78eb-08d3decca4a4', N'admin', N'管理员', N'2016-09-17 15:31:30.000', N'0', N'127.0.0.1', N'e01aacd2-29a2-c135-78eb-08d3decca4a4', N'16400')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D20A03B8-5522-C95F-5A74-08D3DECCB0B5', N'系统用户删除角色e01aacd2-29a2-c135-78eb-08d3decca4a4', N'admin', N'管理员', N'2016-09-17 15:31:50.000', N'0', N'127.0.0.1', N'e01aacd2-29a2-c135-78eb-08d3decca4a4', N'8208')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'38CC6649-21D5-C416-313D-08D3DECCB5AB', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'管理员', N'2016-09-17 15:31:59.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'DC133FE3-B98C-C486-DB3F-08D3DEDD1BC9', N'系统用户登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 17:29:22.000', N'0', N'', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'658EBCBB-7FCC-C716-6B88-08D3DEDD2AD8', N'分配系统用户角色685d010c-d3bd-c5c6-db67-08d3dae88320', N'yangyukun', N'杨瑜堃', N'2016-09-17 17:29:47.000', N'0', N'127.0.0.1', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F4DCA2D0-C25A-CF52-03E6-08D3DEDD2C26', N'系统用户退出登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 17:29:49.000', N'0', N'127.0.0.1', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'DDED9DFF-1D51-C3A0-2644-08D3DEDD319D', N'系统用户登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 17:29:59.000', N'0', N'', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'52206E38-7824-C1ED-4611-08D3DEDDA187', N'系统用户登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 17:33:06.000', N'0', N'', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'2BE3CC87-68DE-C02B-0A5F-08D3DEDE25ED', N'系统用户退出登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 17:36:48.000', N'0', N'127.0.0.1', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'CFDB50F2-C3A6-C84E-5A43-08D3DEDE2B1A', N'系统用户登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 17:36:57.000', N'0', N'', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'98313EB9-8E9B-C56D-9ABE-08D3DEEC8A5D', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 19:19:50.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'78176B11-16B4-C1B8-58E0-08D3DEEC9FBA', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 19:20:26.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'07E55B4B-2E91-CEE1-F7A1-08D3DEECA61A', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 19:20:36.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C797CFF5-699C-CB9B-22CA-08D3DEECA9FC', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'管理员', N'2016-09-17 19:20:43.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'4FBBBA45-08B3-C49F-38DC-08D3DEECAD7D', N'系统用户分配角色菜单按钮d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'管理员', N'2016-09-17 19:20:49.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0F1ACD79-2E98-CE2B-5287-08D3DEECB4DB', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'管理员', N'2016-09-17 19:21:01.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'AD57E5A6-F3D3-C207-F1EA-08D3DEECECAE', N'系统用户删除用户8b087a87-749b-c6d0-31eb-08d3dec9fece', N'admin', N'管理员', N'2016-09-17 19:22:35.000', N'0', N'127.0.0.1', N'8b087a87-749b-c6d0-31eb-08d3dec9fece', N'8200')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'2AB96B18-A075-CAD7-5814-08D3DEECEF9F', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'管理员', N'2016-09-17 19:22:40.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C9D0316E-41B1-CD9F-B8CC-08D3DEF1C018', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'管理员', N'2016-09-17 19:57:08.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'B1F1F957-64D0-C113-5A73-08D3DEF3AB15', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'管理员', N'2016-09-17 20:10:51.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F5644159-4F30-CDFB-38AA-08D3DEF58098', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2失败', N'admin', N'管理员', N'2016-09-17 20:23:59.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'BA1B92C6-13E2-CCD9-BD39-08D3DEF734D0', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2失败', N'admin', N'管理员', N'2016-09-17 20:36:11.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'DB66EFD4-B3CC-C9E1-6F30-08D3DEF8DBC6', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'管理员', N'2016-09-17 20:48:00.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'CB79DDF8-0F99-C042-32CF-08D3DEF9FB03', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 20:56:02.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0CE437DC-5965-C85B-4AC0-08D3DF00E386', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 21:45:29.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'E7AB70D2-249C-C482-26B7-08D3DF00EC22', N'系统用户分配角色菜单689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'管理员', N'2016-09-17 21:45:44.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'061B9D21-2603-C6FB-CB9D-08D3DF00EEEA', N'系统用户分配角色菜单d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'管理员', N'2016-09-17 21:45:49.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'111C7905-0ACF-CE8A-5306-08D3DF00F663', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'管理员', N'2016-09-17 21:46:01.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D2B6B02F-B447-C4DF-413A-08D3DF01D4DC', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 21:52:14.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C96F1624-BF7B-CAA2-3EF4-08D3DF029C1F', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 21:57:49.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'99FC1785-D49D-C64C-D04C-08D3DF02B6E8', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'管理员', N'2016-09-17 21:58:34.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'2E57C5F9-6913-C6D8-8E45-08D3DF02FEF3', N'系统用户分配角色菜单689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'管理员', N'2016-09-17 22:00:34.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C4661081-CA36-C089-8500-08D3DF031E20', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'管理员', N'2016-09-17 22:01:27.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F38B40FD-F10D-CC59-EF80-08D3DF035581', N'系统用户退出登录', N'admin', N'管理员', N'2016-09-17 22:03:00.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5B882EE3-274E-C245-DB96-08D3DF035A69', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 22:03:08.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C9DF9C4C-7521-C3B7-8ACF-08D3DF03653F', N'分配系统用户角色685d010c-d3bd-c5c6-db67-08d3dae88320', N'admin', N'管理员', N'2016-09-17 22:03:26.000', N'0', N'127.0.0.1', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'748AF1C9-DA07-C822-683C-08D3DF0368AB', N'系统用户退出登录', N'admin', N'管理员', N'2016-09-17 22:03:32.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0BEFB8BE-B98E-C96F-EAD9-08D3DF036DE4', N'系统用户登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 22:03:41.000', N'0', N'', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F2D08BFA-65F3-C00C-89CB-08D3DF038634', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 22:04:21.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5210ADAB-55DC-CA94-4CE6-08D3DF03898C', N'系统用户退出登录', N'admin', N'管理员', N'2016-09-17 22:04:27.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'42728B87-C982-CD7E-0B8D-08D3DF038E07', N'系统用户登录', N'yangyukun', N'杨瑜堃', N'2016-09-17 22:04:35.000', N'0', N'', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'CCFCAEF5-1AF5-C33D-0A21-08D3DF07A5AE', N'系统用户登录', N'admin', N'管理员', N'2016-09-17 22:33:52.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'38212AFB-C5C0-C50C-C319-08D3DF07B5A2', N'系统用户退出登录', N'admin', N'管理员', N'2016-09-17 22:34:19.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7EF793F9-191B-CE2F-5D6F-08D3DF07B847', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-17 22:34:23.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'BA9CD9BD-5699-CEFC-D45D-08D3DF08EEA1', N'系统用户创建菜单商品管理', N'admin', N'系统管理员', N'2016-09-17 22:43:04.000', N'0', N'127.0.0.1', N'8d469a2f-070d-cd6e-2650-08d3df08eea0', N'16385')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'919934CB-196C-C796-3110-08D3DF09330C', N'系统用户创建菜单商品种类管理', N'admin', N'系统管理员', N'2016-09-17 22:44:59.000', N'0', N'127.0.0.1', N'cdcca5c8-eff6-c039-e31b-08d3df09330b', N'16385')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'48790F82-7798-C772-998C-08D3DF0E5419', N'系统用户在菜单cdcca5c8-eff6-c039-e31b-08d3df09330b下添加按钮1', N'admin', N'系统管理员', N'2016-09-17 23:21:42.000', N'0', N'127.0.0.1', N'691d0618-47a6-c4bf-ebaa-08d3df0e5415,cdcca5c8-eff6-c039-e31b-08d3df09330b', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'B0A60F92-9431-C6B2-A14C-08D3DF0E5B91', N'系统用户删除菜单按钮691d0618-47a6-c4bf-ebaa-08d3df0e5415', N'admin', N'系统管理员', N'2016-09-17 23:21:54.000', N'0', N'127.0.0.1', N'691d0618-47a6-c4bf-ebaa-08d3df0e5415', N'8194')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'9BAD5FBE-B5E2-CF4F-7914-08D3DF0E5D3D', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'系统管理员', N'2016-09-17 23:21:57.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'4E048C8C-5A1A-C62B-D3EE-08D3DF111705', N'分配系统用户角色893b8fa1-f002-4206-936b-1b357a478b34', N'admin', N'系统管理员', N'2016-09-17 23:41:28.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D145D716-EEA7-CA99-94DF-08D3DF111FCA', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-17 23:41:43.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D88F418E-E45A-C506-50BB-08D3DF1146FA', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-17 23:42:48.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'E1495720-2ED7-C523-4752-08D3DF11DE5D', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-17 23:47:02.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'4526345A-DB5D-CFDE-6073-08D3DF11F0B5', N'系统用户更新菜单按钮0bff2076-e2c5-c8d3-5663-08d3dc3c5d96', N'admin', N'系统管理员', N'2016-09-17 23:47:33.000', N'0', N'127.0.0.1', N'0bff2076-e2c5-c8d3-5663-08d3dc3c5d96', N'4098')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5779934A-2D8E-C482-B372-08D3DF11F7B4', N'系统用户更新菜单按钮79ee823e-3643-c2cf-d1d1-08d3dc3ec3a3', N'admin', N'系统管理员', N'2016-09-17 23:47:45.000', N'0', N'127.0.0.1', N'79ee823e-3643-c2cf-d1d1-08d3dc3ec3a3', N'4098')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'61F6F831-6632-C5A7-3F19-08D3DF11FA9E', N'系统用户更新菜单按钮6f88d006-583a-c51e-b325-08d3dc3ef683', N'admin', N'系统管理员', N'2016-09-17 23:47:50.000', N'0', N'127.0.0.1', N'6f88d006-583a-c51e-b325-08d3dc3ef683', N'4098')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'B49653EB-AB76-C569-AF73-08D3DF13C5BF', N'系统用户分配角色菜单689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'系统管理员', N'2016-09-18 00:00:40.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'2C6F7EB9-A1D8-C512-0637-08D3DF13D036', N'系统用户分配角色菜单d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'系统管理员', N'2016-09-18 00:00:57.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'AA55CD1A-C150-C322-FDA9-08D3DF13D8C1', N'系统用户分配角色菜单按钮d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'系统管理员', N'2016-09-18 00:01:12.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'02444662-B9E6-CDFD-1907-08D3DF13EEDF', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'系统管理员', N'2016-09-18 00:01:49.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'781AF4CC-BDAC-CF7B-9C1E-08D3DF13F899', N'分配系统用户角色893b8fa1-f002-4206-936b-1b357a478b34', N'admin', N'系统管理员', N'2016-09-18 00:02:05.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D0AA6533-A3F7-C74D-50A5-08D3DF13FC34', N'分配系统用户角色685d010c-d3bd-c5c6-db67-08d3dae88320', N'admin', N'系统管理员', N'2016-09-18 00:02:11.000', N'0', N'127.0.0.1', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5FDEF015-D0B4-C162-C4C7-08D3DF13FE78', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-18 00:02:15.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'FEA28804-436C-C1D2-AF4F-08D3DF140876', N'系统用户登录', N'yangyukun', N'杨瑜堃', N'2016-09-18 00:02:32.000', N'0', N'', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'DBCECCFE-4D19-C753-6416-08D3DF141726', N'系统用户退出登录', N'yangyukun', N'杨瑜堃', N'2016-09-18 00:02:57.000', N'0', N'127.0.0.1', N'685d010c-d3bd-c5c6-db67-08d3dae88320', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'4260DF7B-60E0-C0AF-452A-08D3DF141990', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-18 00:03:01.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'44AEB2CF-D78B-C0C2-598E-08D3DFBE8AF6', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-18 20:23:05.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'699520F0-0243-C37A-72C6-08D3DFD3023F', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-18 22:49:35.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D92029E3-4C76-CDDD-DFAA-08D3E08A2E86', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-19 20:40:48.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5B00AB3F-9DF0-C5D1-43DA-08D3E08B829B', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-19 20:50:18.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F6333424-2326-C1C7-CECC-08D3E08CEA82', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-19 21:00:22.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'A9A0BF8A-838B-C8A4-820E-08D3E08D0335', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-19 21:01:03.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'490ACA9E-6A6E-C8A7-394E-08D3E090DCBC', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-19 21:28:37.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'58641E2D-3F8A-C138-513C-08D3E092090F', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-19 21:37:01.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0D117A36-6F95-C8F4-189E-08D3E092CDE8', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-19 21:42:31.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'8992ECA6-47DB-C331-9271-08D3E092D24D', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-19 21:42:38.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'250C3F61-E5D5-C93C-CD61-08D3E094E962', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'系统管理员', N'2016-09-19 21:57:36.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C393178A-52AF-C066-FC47-08D3E094FA7C', N'系统用户清除了权限系统的逻辑删除数据', N'admin', N'系统管理员', N'2016-09-19 21:58:05.000', N'0', N'127.0.0.1', N'', N'8219')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'15B44742-4031-CE7B-85E8-08D3E09933CA', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-19 22:28:19.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D3C8E3A5-2C88-C14A-0529-08D3E1F8CBEB', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-21 16:25:07.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F4CE9216-1BC8-CC6B-4A1B-08D3E1F8E5D5', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-21 16:25:51.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'781E67A5-D6E7-CFED-6900-08D3E1FC2463', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-21 16:49:04.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'3402D754-26BC-CE9F-7A56-08D3E1FC280B', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-21 16:49:10.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C6F4BB8A-E40F-CC09-5514-08D3E1FDBC93', N'系统用户在菜单cdcca5c8-eff6-c039-e31b-08d3df09330b下添加按钮添加种类', N'admin', N'系统管理员', N'2016-09-21 17:00:29.000', N'0', N'127.0.0.1', N'051c4ba1-1b3e-c61b-846b-08d3e1fdbc8f,cdcca5c8-eff6-c039-e31b-08d3df09330b', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0CAA3CF3-FA66-C849-62F9-08D3E1FDC695', N'系统用户在菜单cdcca5c8-eff6-c039-e31b-08d3df09330b下添加按钮编辑种类', N'admin', N'系统管理员', N'2016-09-21 17:00:46.000', N'0', N'127.0.0.1', N'6a2df70e-e4f9-c63a-e072-08d3e1fdc691,cdcca5c8-eff6-c039-e31b-08d3df09330b', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'4C604778-596B-CD15-4C85-08D3E1FDD1B1', N'系统用户在菜单cdcca5c8-eff6-c039-e31b-08d3df09330b下添加按钮删除种类', N'admin', N'系统管理员', N'2016-09-21 17:01:05.000', N'0', N'127.0.0.1', N'48d39f58-9acb-c717-a2ed-08d3e1fdd1ad,cdcca5c8-eff6-c039-e31b-08d3df09330b', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5801609C-2FF6-C513-9BAE-08D3E1FDE8AC', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-21 17:01:43.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'490723FF-8AB7-C674-87D2-08D3E1FDEB67', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-21 17:01:48.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'E6B019F4-65D3-C151-3207-08D3E1FE6949', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-21 17:05:19.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'3A35B8F2-08CC-C746-00A1-08D3E1FE6F8A', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-21 17:05:29.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'ED6598CF-BB0E-CD86-34F1-08D3E1FE71E0', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-21 17:05:33.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'9E833946-7063-C338-994A-08D3E1FF8C01', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-21 17:13:27.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'025A65F8-FED0-C09B-B35D-08D3E1FF8F7F', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-21 17:13:32.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D37F4769-0A70-C05B-0C9D-08D3E1FF9A11', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-21 17:13:50.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F2322C82-7EEE-C56E-D34B-08D3E1FFBD70', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-21 17:14:50.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'489741D5-1A5E-C64C-5988-08D3E2183F26', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-21 20:10:15.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'59BDE3A0-7A11-C0AC-AA2B-08D3E218419F', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-21 20:10:19.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'73B3663F-D719-CB01-C3EB-08D3E22F71A1', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-21 22:56:18.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'252E5B0C-3C81-C1DC-6D8A-08D3E2FAEC52', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-22 23:12:52.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D39E48BF-06A9-CE8B-3DA4-08D3E2FB0205', N'系统用户创建菜单客户管理', N'admin', N'系统管理员', N'2016-09-22 23:13:28.000', N'0', N'127.0.0.1', N'eaa410fb-8019-cc11-b6c5-08d3e2fb0203', N'16385')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'063E866A-CE7A-CB04-434C-08D3E2FB14B9', N'系统用户创建菜单客户管理', N'admin', N'系统管理员', N'2016-09-22 23:14:00.000', N'0', N'127.0.0.1', N'eb0d9a25-6a72-c217-f524-08d3e2fb14b8', N'16385')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'2A69D513-1EF9-CAD3-6C81-08D3E2FB30C8', N'系统用户在菜单eb0d9a25-6a72-c217-f524-08d3e2fb14b8下添加按钮查询客户', N'admin', N'系统管理员', N'2016-09-22 23:14:47.000', N'0', N'127.0.0.1', N'4ac8ea60-7eb6-c8ce-be8c-08d3e2fb30c6,eb0d9a25-6a72-c217-f524-08d3e2fb14b8', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'E82B7F72-7277-C809-6584-08D3E2FB3A1A', N'系统用户在菜单eb0d9a25-6a72-c217-f524-08d3e2fb14b8下添加按钮编辑客户', N'admin', N'系统管理员', N'2016-09-22 23:15:02.000', N'0', N'127.0.0.1', N'6854a804-7b34-cb0a-a1fc-08d3e2fb3a19,eb0d9a25-6a72-c217-f524-08d3e2fb14b8', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'A1B8170C-C8F6-C251-2B8D-08D3E2FB4C00', N'系统用户在菜单eb0d9a25-6a72-c217-f524-08d3e2fb14b8下添加按钮删除客户', N'admin', N'系统管理员', N'2016-09-22 23:15:32.000', N'0', N'127.0.0.1', N'9783595c-8bc2-c2bf-b624-08d3e2fb4bff,eb0d9a25-6a72-c217-f524-08d3e2fb14b8', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'839328F3-BE7B-CC3B-80E7-08D3E2FB7275', N'系统用户在菜单eb0d9a25-6a72-c217-f524-08d3e2fb14b8下添加按钮冻结客户', N'admin', N'系统管理员', N'2016-09-22 23:16:37.000', N'0', N'127.0.0.1', N'1c282c3a-d839-c047-e493-08d3e2fb7274,eb0d9a25-6a72-c217-f524-08d3e2fb14b8', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'14820146-68FE-C7DD-3037-08D3E2FB7A7F', N'系统用户分配角色菜单a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-22 23:16:50.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7EC6AFAE-8393-CA22-BAE5-08D3E2FB7DA4', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-22 23:16:56.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C809FCB5-0D4D-CE47-4D24-08D3E2FB801C', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-22 23:17:00.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'07F42F0B-0A94-C035-6CBF-08D3E2FB8288', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-22 23:17:04.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7F8AAEEF-87DE-C0E8-4722-08D3E2FC6BBB', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-22 23:23:35.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F330FD3A-9C91-CA31-4296-08D3E2FC6E7B', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-22 23:23:40.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'3DBBD496-3997-C9CA-34FA-08D3E2FCEAFD', N'分配系统用户角色893b8fa1-f002-4206-936b-1b357a478b34', N'admin', N'系统管理员', N'2016-09-22 23:27:09.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'20504')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'6D48BAA2-AB7C-C0A1-EC15-08D3E2FCF50B', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-22 23:27:26.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C70B764A-38FE-CBBF-21B3-08D3E2FD0516', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-22 23:27:52.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7F70CCAE-976D-C179-A215-08D3E3B5E618', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-23 21:31:17.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'D580728A-C7C9-CD3B-31C4-08D3E3B9623B', N'系统用户分配角色菜单689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'系统管理员', N'2016-09-23 21:56:14.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0ADCE50E-EBC2-C033-3259-08D3E3B96B4B', N'系统用户分配角色菜单按钮689a7510-70ef-42cd-9ad1-1611b29061d2', N'admin', N'系统管理员', N'2016-09-23 21:56:29.000', N'0', N'127.0.0.1', N'689a7510-70ef-42cd-9ad1-1611b29061d2', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'ECEE7271-B5BF-C9F4-D4F0-08D3E3B96E4E', N'系统用户分配角色菜单d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'系统管理员', N'2016-09-23 21:56:34.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28689')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7D5A679F-414C-C626-9291-08D3E3B97D39', N'系统用户分配角色菜单按钮d89d5e7e-931a-cae2-0101-08d3de1dacb6', N'admin', N'系统管理员', N'2016-09-23 21:56:59.000', N'0', N'127.0.0.1', N'd89d5e7e-931a-cae2-0101-08d3de1dacb6', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'53F18AEB-88B4-CA32-DF25-08D3E3C74904', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-23 23:35:45.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F9060E83-9C87-CBB7-BD5B-08D3E3CB71E7', N'系统用户在菜单eb0d9a25-6a72-c217-f524-08d3e2fb14b8下添加按钮导入客户', N'admin', N'系统管理员', N'2016-09-24 00:05:31.000', N'0', N'127.0.0.1', N'8b429131-cf53-cffe-9a2b-08d3e3cb71e5,eb0d9a25-6a72-c217-f524-08d3e2fb14b8', N'16386')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'4E6406DA-B372-C1B4-B57D-08D3E3CB7757', N'系统用户分配角色菜单按钮a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'admin', N'系统管理员', N'2016-09-24 00:05:40.000', N'0', N'127.0.0.1', N'a0eb4d90-3276-cc7a-19e9-08d3dd61893e', N'28690')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'B203710D-010D-C6D3-5B55-08D3E3CB7995', N'系统用户退出登录', N'admin', N'系统管理员', N'2016-09-24 00:05:44.000', N'0', N'127.0.0.1', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F06C7BA4-1DB5-CC43-8A5A-08D3E3CB7CB0', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-24 00:05:49.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'E2021E80-1F6E-C1B9-E50C-08D3E40E1729', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-24 08:02:35.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'6134F8DC-239C-C87E-B65D-08D3E42A05E8', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-24 11:22:32.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'DCB82116-2FBD-C125-BBDE-08D3E44C30D0', N'系统用户创建商品分类123', N'admin', N'系统管理员', N'2016-09-24 15:27:07.000', N'0', N'127.0.0.1', N'', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5D64866F-86D1-C747-7E16-08D3E44C3B77', N'系统用户创建商品分类123', N'admin', N'系统管理员', N'2016-09-24 15:27:25.000', N'0', N'127.0.0.1', N'', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'E9CE9EC0-80E5-C087-2EE4-08D3E44D80F0', N'系统用户创建商品分类11', N'admin', N'系统管理员', N'2016-09-24 15:36:31.000', N'0', N'127.0.0.1', N'', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0654F25A-78C8-C52E-2B38-08D3E44D87C8', N'系统用户登录', N'admin', N'系统管理员', N'2016-09-24 15:36:43.000', N'0', N'', N'893b8fa1-f002-4206-936b-1b357a478b34', N'12')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'E9AA1432-0772-C6D2-B278-08D3E44D9D89', N'系统用户创建商品分类123', N'admin', N'系统管理员', N'2016-09-24 15:37:19.000', N'0', N'127.0.0.1', N'', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'79EC51B6-F9E4-C1A6-78CC-08D3E44DB2F4', N'系统用户创建商品分类88', N'admin', N'系统管理员', N'2016-09-24 15:37:55.000', N'0', N'127.0.0.1', N'', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'5C1417C1-884B-CC17-BF4E-08D3E44DCDA7', N'系统用户创建商品分类33', N'admin', N'系统管理员', N'2016-09-24 15:38:40.000', N'0', N'127.0.0.1', N'', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'1A11B86C-79C9-C030-6CE4-08D3E44DE52A', N'系统用户创建商品分类44', N'admin', N'系统管理员', N'2016-09-24 15:39:19.000', N'0', N'127.0.0.1', N'', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'9A5676A2-A789-C02B-8CB1-08D3E45C21D8', N'系统用户创建商品分类fff', N'admin', N'系统管理员', N'2016-09-24 17:21:14.000', N'0', N'127.0.0.1', N'e57c8acd-0638-c75a-8b2b-08d3e45c21cc', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7DEDEC2D-193D-C672-7FE8-08D3E45C2EA0', N'系统用户创建商品分类mm', N'admin', N'系统管理员', N'2016-09-24 17:21:35.000', N'0', N'127.0.0.1', N'61b2b16d-8878-cad8-0aa7-08d3e45c2ea0', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'0AA6DAB2-656A-CA9A-77A4-08D3E45C46B3', N'系统用户创建商品分类fsx', N'admin', N'系统管理员', N'2016-09-24 17:22:16.000', N'0', N'127.0.0.1', N'60ea3f72-9f36-c7f8-297b-08d3e45c46b3', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'7C9BB511-6AEC-C9A9-698B-08D3E45C981C', N'系统用户创建商品分类sfsafasdfa', N'admin', N'系统管理员', N'2016-09-24 17:24:32.000', N'0', N'127.0.0.1', N'c2325120-1558-c7b3-1b60-08d3e45c981c', N'16384')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'F7DEAE78-980A-CCCA-F9E1-08D3E4669086', N'系统用户更新菜单cdcca5c8-eff6-c039-e31b-08d3df09330b', N'admin', N'系统管理员', N'2016-09-24 18:35:55.000', N'0', N'127.0.0.1', N'cdcca5c8-eff6-c039-e31b-08d3df09330b', N'4097')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'C421EF5A-7194-CC60-77A1-08D3E46BF88D', N'系统用户删除商品分类e13f2d16-bc33-403b-9409-68c44fbb6231', N'admin', N'系统管理员', N'2016-09-24 19:14:37.000', N'0', N'127.0.0.1', N'e13f2d16-bc33-403b-9409-68c44fbb6231', N'8192')
GO
GO
INSERT INTO [dbo].[EHECD_SystemLog] ([ID], [sDomainDetail], [sLoginName], [sUserName], [dInsertTime], [bIsDeleted], [sIPAddress], [sDoMainId], [tDoType]) VALUES (N'062C236F-063E-C60E-459F-08D3E46CCAAE', N'系统用户删除商品分类f68d3e4c-391a-4bf5-bc1c-60024f85c224', N'admin', N'系统管理员', N'2016-09-24 19:20:29.000', N'0', N'127.0.0.1', N'f68d3e4c-391a-4bf5-bc1c-60024f85c224', N'8192')
GO
GO

-- ----------------------------
-- Table structure for EHECD_SystemUser
-- ----------------------------
DROP TABLE [dbo].[EHECD_SystemUser]
GO
CREATE TABLE [dbo].[EHECD_SystemUser] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sLoginName] nvarchar(20) NOT NULL DEFAULT '' ,
[sPassWord] nvarchar(50) NOT NULL DEFAULT '' ,
[sUserName] nvarchar(15) NOT NULL DEFAULT '' ,
[tUserState] tinyint NOT NULL DEFAULT ((0)) ,
[tUserType] tinyint NOT NULL DEFAULT ((0)) ,
[sUserNickName] nvarchar(20) NOT NULL DEFAULT '' ,
[dCreateTime] datetime NOT NULL DEFAULT (getdate()) ,
[dLastLoginTime] datetime NOT NULL DEFAULT (getdate()) ,
[sProvice] nvarchar(20) NOT NULL DEFAULT '' ,
[sCity] nvarchar(20) NOT NULL DEFAULT '' ,
[sCounty] nvarchar(20) NOT NULL DEFAULT '' ,
[sAddress] nvarchar(30) NOT NULL DEFAULT '' ,
[tSex] tinyint NOT NULL DEFAULT ((0)) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) ,
[sMobileNum] nvarchar(25) NOT NULL DEFAULT '' 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sLoginName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'登录名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sLoginName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'登录名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sLoginName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sPassWord')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'登录密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sPassWord'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'登录密码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sPassWord'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sUserName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'用户姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sUserName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'用户姓名'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sUserName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'tUserState')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'用户状态 0：正常 1：冻结'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'tUserState'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'用户状态 0：正常 1：冻结'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'tUserState'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'tUserType')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'用户类型 0：平台用户'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'tUserType'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'用户类型 0：平台用户'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'tUserType'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sUserNickName')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'用户昵称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sUserNickName'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'用户昵称'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sUserNickName'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'dCreateTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'dCreateTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'创建时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'dCreateTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'dLastLoginTime')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'最后登录时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'dLastLoginTime'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'最后登录时间'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'dLastLoginTime'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sProvice')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'所在省'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sProvice'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'所在省'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sProvice'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sCity')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'所在市'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sCity'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'所在市'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sCity'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sCounty')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'所在地区'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sCounty'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'所在地区'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sCounty'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sAddress')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'详细地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sAddress'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'详细地址'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sAddress'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'tSex')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'性别 0:女 1:男'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'tSex'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'性别 0:女 1:男'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'tSex'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser', 
'COLUMN', N'sMobileNum')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'手机号码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sMobileNum'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'手机号码'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser'
, @level2type = 'COLUMN', @level2name = N'sMobileNum'
GO

-- ----------------------------
-- Records of EHECD_SystemUser
-- ----------------------------
INSERT INTO [dbo].[EHECD_SystemUser] ([ID], [sLoginName], [sPassWord], [sUserName], [tUserState], [tUserType], [sUserNickName], [dCreateTime], [dLastLoginTime], [sProvice], [sCity], [sCounty], [sAddress], [tSex], [bIsDeleted], [sMobileNum]) VALUES (N'685D010C-D3BD-C5C6-DB67-08D3DAE88320', N'yangyukun', N'202CB962AC59075B964B07152D234B70', N'杨瑜堃', N'0', N'0', N'平台用户杨瑜堃', N'2016-09-12 16:40:53.000', N'2016-09-18 00:02:32.000', N'四川省', N'成都市', N'金牛区', N'光荣北路', N'1', N'0', N'13540685528')
GO
GO
INSERT INTO [dbo].[EHECD_SystemUser] ([ID], [sLoginName], [sPassWord], [sUserName], [tUserState], [tUserType], [sUserNickName], [dCreateTime], [dLastLoginTime], [sProvice], [sCity], [sCounty], [sAddress], [tSex], [bIsDeleted], [sMobileNum]) VALUES (N'893B8FA1-F002-4206-936B-1B357A478B34', N'admin', N'202cb962ac59075b964b07152d234b70', N'系统管理员', N'0', N'0', N'超级管理员', N'2016-08-28 08:39:33.260', N'2016-09-24 15:36:43.000', N'四川省', N'成都市', N'青羊区', N'光华中心', N'0', N'0', N'13888888888')
GO
GO

-- ----------------------------
-- Table structure for EHECD_SystemUser_R_Role
-- ----------------------------
DROP TABLE [dbo].[EHECD_SystemUser_R_Role]
GO
CREATE TABLE [dbo].[EHECD_SystemUser_R_Role] (
[ID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sUserID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[sRoleID] uniqueidentifier NOT NULL DEFAULT (newid()) ,
[bIsDeleted] bit NOT NULL DEFAULT ((0)) 
)


GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser_R_Role', 
'COLUMN', N'ID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'ID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'唯一标识'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'ID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser_R_Role', 
'COLUMN', N'sUserID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'用户的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'sUserID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'用户的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'sUserID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser_R_Role', 
'COLUMN', N'sRoleID')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'角色的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'sRoleID'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'角色的ID'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'sRoleID'
GO
IF ((SELECT COUNT(*) from fn_listextendedproperty('MS_Description', 
'SCHEMA', N'dbo', 
'TABLE', N'EHECD_SystemUser_R_Role', 
'COLUMN', N'bIsDeleted')) > 0) 
EXEC sp_updateextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
ELSE
EXEC sp_addextendedproperty @name = N'MS_Description', @value = N'是否删除'
, @level0type = 'SCHEMA', @level0name = N'dbo'
, @level1type = 'TABLE', @level1name = N'EHECD_SystemUser_R_Role'
, @level2type = 'COLUMN', @level2name = N'bIsDeleted'
GO

-- ----------------------------
-- Records of EHECD_SystemUser_R_Role
-- ----------------------------
INSERT INTO [dbo].[EHECD_SystemUser_R_Role] ([ID], [sUserID], [sRoleID], [bIsDeleted]) VALUES (N'3E758992-3F97-C819-2554-08D3DD61B159', N'893B8FA1-F002-4206-936B-1B357A478B34', N'A0EB4D90-3276-CC7A-19E9-08D3DD61893E', N'0')
GO
GO
INSERT INTO [dbo].[EHECD_SystemUser_R_Role] ([ID], [sUserID], [sRoleID], [bIsDeleted]) VALUES (N'51A53E13-DD35-CD81-17F4-08D3DF13FC33', N'685D010C-D3BD-C5C6-DB67-08D3DAE88320', N'D89D5E7E-931A-CAE2-0101-08D3DE1DACB6', N'0')
GO
GO

-- ----------------------------
-- Indexes structure for table EHECD_Categories
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_Categories
-- ----------------------------
ALTER TABLE [dbo].[EHECD_Categories] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table EHECD_Client
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_Client
-- ----------------------------
ALTER TABLE [dbo].[EHECD_Client] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table EHECD_ClientAddress
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_ClientAddress
-- ----------------------------
ALTER TABLE [dbo].[EHECD_ClientAddress] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table EHECD_FunctionMenu
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_FunctionMenu
-- ----------------------------
ALTER TABLE [dbo].[EHECD_FunctionMenu] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Checks structure for table EHECD_FunctionMenu
-- ----------------------------
ALTER TABLE [dbo].[EHECD_FunctionMenu] ADD CHECK (([bIsDeleted]>=(0) AND [bIsDeleted]<=(1)))
GO

-- ----------------------------
-- Indexes structure for table EHECD_MenuButton
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_MenuButton
-- ----------------------------
ALTER TABLE [dbo].[EHECD_MenuButton] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Checks structure for table EHECD_MenuButton
-- ----------------------------
ALTER TABLE [dbo].[EHECD_MenuButton] ADD CHECK (([bIsDeleted]>=(0) AND [bIsDeleted]<=(1)))
GO

-- ----------------------------
-- Indexes structure for table EHECD_Privilege
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_Privilege
-- ----------------------------
ALTER TABLE [dbo].[EHECD_Privilege] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Checks structure for table EHECD_Privilege
-- ----------------------------
ALTER TABLE [dbo].[EHECD_Privilege] ADD CHECK (([bIsDeleted]>=(0) AND [bIsDeleted]<=(1)))
GO

-- ----------------------------
-- Indexes structure for table EHECD_Role
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_Role
-- ----------------------------
ALTER TABLE [dbo].[EHECD_Role] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Checks structure for table EHECD_Role
-- ----------------------------
ALTER TABLE [dbo].[EHECD_Role] ADD CHECK (([bIsDeleted]>=(0) AND [bIsDeleted]<=(1)))
GO

-- ----------------------------
-- Indexes structure for table EHECD_SystemLog
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_SystemLog
-- ----------------------------
ALTER TABLE [dbo].[EHECD_SystemLog] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Indexes structure for table EHECD_SystemUser
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_SystemUser
-- ----------------------------
ALTER TABLE [dbo].[EHECD_SystemUser] ADD PRIMARY KEY ([ID])
GO

-- ----------------------------
-- Checks structure for table EHECD_SystemUser
-- ----------------------------
ALTER TABLE [dbo].[EHECD_SystemUser] ADD CHECK (([tUserState]>=(0) AND [tUserState]<=(10)))
GO
ALTER TABLE [dbo].[EHECD_SystemUser] ADD CHECK (([tUserType]>=(0) AND [tUserType]<=(10)))
GO
ALTER TABLE [dbo].[EHECD_SystemUser] ADD CHECK (([tSex]>=(0) AND [tSex]<=(60)))
GO

-- ----------------------------
-- Indexes structure for table EHECD_SystemUser_R_Role
-- ----------------------------

-- ----------------------------
-- Primary Key structure for table EHECD_SystemUser_R_Role
-- ----------------------------
ALTER TABLE [dbo].[EHECD_SystemUser_R_Role] ADD PRIMARY KEY ([ID])
GO
