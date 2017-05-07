该程序集引用Dapper，定义了查询接口和增删改接口，通过模板模式对接口进行了实现。由于时间有限，对Dapper的封装较简单，但足够灵活，下一步我会加入对表达式的解析，以实现以linq方式查询数据。

为了达到项目数据层的灵活性，复杂操作的SQL将自己传入。

1.查询接口的定义

QueryHelper是查询的接口，外部引用的查询统一入口就是该抽象类，该类定义了3个虚方法

//查询一条数据
1.T SingleQuery<T>(string sqlCommand,Object parameter) where T : new()

// 查询多条数据
2.IList<T> QueryList<T>(string sqlCommand, Object parameter) where T : new()

// 分页查询
3. PagingRet<T> PaginationQuery<T>(string sqlCommand, PageInfo pageInfo, Object parameter) where T : new()

实现该接口的模板类是QueryHelperBase抽象类，该类重写了以上3个方法，
并将3个方法密封以保证其实现类无法再次重写，同时定义了3个抽象方法
以指定其实现类必须实现的查询数据的行为，在实现的QueryHelper的3个
方法中，对异常进行了处理，对数据连接资源进行了初始化和释放，使真
正使用的数据操作类不再关心这些操作。

DapperDBHelper就是继承了QueryHelperBase抽象类的具体实现类，其实现的3个方法将对查询进行封装，外部调用只需要传入sql语句和条件。

注：现在实现了自定义字段的查询，如果你要查的东西没有对应的DTO，在查询时请指定返回实体对象为字典即可。



