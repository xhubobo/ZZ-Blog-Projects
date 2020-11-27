-- 添加数据库和用户
set sql_mode=(select replace(@@sql_mode,'NO_AUTO_CREATE_USER','')); -- 设置自动创建用户
drop database if exists NewsPublish; -- 删除数据库
create database if not exists NewsPublish; -- 创建数据库
grant usage on *.* to 'NewsPublishUser'@'localhost'; -- 如果用户不存在则创建，然后赋权
drop user 'NewsPublishUser'@'localhost'; -- 删除用户
create user 'NewsPublishUser'@'localhost' identified by '123456'; -- 创建用户
grant all privileges on database.NewsPublish to 'NewsPublishUser'@'localhost'; -- 用户授权
flush privileges; -- 刷新系统权限

-- 切换数据库
use NewsPublish;

/*==============================================================*/
/* Table: Banner                                                */
/*==============================================================*/
create table Banner (
	Id int primary key AUTO_INCREMENT comment '编号',
	Image varchar(200) comment 'Banner图片' not null,
	Url varchar(100) comment '跳转地址',
	AddTime datetime comment '添加时间',
	Remark varchar(200) comment '备注'
	) comment 'Banner表' ENGINE = INNODB default CHARSET = utf8;

/*==============================================================*/
/* Table: News                                                  */
/*==============================================================*/
create table News (
	Id int AUTO_INCREMENT comment '编号',
	NewsClassifyId int comment '新闻类别编号' not null,
	Title varchar(1000) comment '新闻标题' not null,
	Image varchar(200) comment '新闻图片' null,
	Contents varchar(20) comment '新闻内容' null,
	PublishDate datetime comment '发布日期' null,
	Remark varchar(200) comment '备注' null,
	constraint PK_NEWS primary key(Id)
	) comment '新闻表' ENGINE = INNODB default CHARSET = utf8;
	
/*==============================================================*/
/* Table: NewsClassify                                          */
/*==============================================================*/
create table NewsClassify (
	Id int AUTO_INCREMENT comment '编号',
	Name varchar(100) comment '类别名称' null,
	Sort int comment '排序编号(从大到小排列)' null,
	Remark varchar(200) comment '备注' null,
	constraint PK_NEWSCLASSIFY primary key(Id)
	) comment '新闻类别表' ENGINE = INNODB default CHARSET = utf8;
	
/*==============================================================*/
/* Table: NewsComment                                           */
/*==============================================================*/
create table NewsComment (
	Id int AUTO_INCREMENT comment '编号',	
	NewsId int comment '新闻编号' not null,
	Contents varchar(2000) comment '评论内容' null,
	AddTime datetime comment '评论时间' null,
	Remark varchar(200) comment '备注' null,
	constraint PK_NEWSCOMMENT primary key(Id)
	) comment '新闻评论表' ENGINE = INNODB default CHARSET = utf8;

alter table News
   add constraint FK_NewsClassify_News foreign key (NewsClassifyId)
      references NewsClassify (Id);

alter table NewsComment
   add constraint FK_News_NewsComment foreign key (NewsId)
      references News (Id);