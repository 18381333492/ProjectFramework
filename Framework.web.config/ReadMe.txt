该程序集完成项目的配置读取和获取工作。

在WebConfig类中提供一个静态方法

1.String LoadElement(string key)

可通过key获取到对应的配置信息，包括连接字符串，日志文件路径等。

注：配置文件为frame.config.json，里面是json格式的字符串。