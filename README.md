#.net 常用工具类库
##IpHelper类
- IntranetIp4() 获得服务器的内网IP
- InternetIp4() 获得服务器的公网IP
- GetAvailablePort() 随机获得指定IP的一个没有被使用的端口

## SQLBuilder类，

该类用于生成动态 WHERE 查询条件，减少条件判断，减少字符串拼接错误

```
例子：

var builder = new SQLBuilder();
builder.AppendWhere();
builder.Append(query.CustomerId > 0, "and", "CustomerId=@CustomerId");
builder.Append(query.AccountId > 0, "and", "AccountId=@AccountId");
return builder.Sql();

如果 query.CustomerId > 0 query.AccountId == 0 则输入以下SQL

"WHERE CustomerId=@CustomerId and AccountId=@AccountId"

如果 query.CustomerId == 0 query.AccountId > 0 则输入以下SQL

"WHERE AccountId=@AccountId"

```

