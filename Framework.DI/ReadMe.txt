该程序集用来配置依赖注入，使用的是微软的Unity

其中单例模式对象DIEntity用作获取注入对象，

可通过DIEntity.GetInstance().GetImpl<T 是注入的接口信息（接口，抽象类）>()方法来获取注入的实现对象

请注意，现在我把注入的配置移到了web.config，不再使用代码方式配置注入了