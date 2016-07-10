#.net 常用工具类库
##IpHelper类
- IntranetIp4() 获得服务器的内网IP
- InternetIp4() 获得服务器的公网IP
- GetAvailablePort() 随机获得指定IP的一个没有被使用的端口

## SQLBuilder类

该类用于生成动态 WHERE 查询条件，减少条件判断，减少字符串拼接错误

```
例子：

var builder = new SQLBuilder();
builder.AppendWhere();
builder.Append(query.CustomerId > 0, "and", "CustomerId=@CustomerId");
builder.Append(query.AccountId > 0, "and", "AccountId=@AccountId");
return builder.Sql();

如果 query.CustomerId>0 并且 query.AccountId==0 则输出以下SQL

"WHERE CustomerId=@CustomerId and AccountId=@AccountId"

如果 query.CustomerId==0 并且 query.AccountId>0 则输出以下SQL

"WHERE AccountId=@AccountId"

```

## StringHelper类

该类用于将字符串类型转换成 数字 bool,等类型，如果转换失败，则可以指定默认值

```
StringHelper.ToInt32("12",0); // 转换成 int类型 12 
StringHelper.ToInt32("1顶起2",0); // 转换成 int类型 12 失败，则返回默认值 0
```

## NullHelper类
该类用于解决访问类实例属性性，导致出 未将对象引用至对象实例的错误,同时可以减少代代码null的条件判断
```
Customer customer = null;
String name = NullHelper.IfNull(customer, string.Empty, m => m.Name);

IfNull 方法
第一个参数 要访问的类实例
第三个参数 要访问类实例的属性
第二个参数 如果类实例为 null,则返回 string.Empty

```
## DESHelper类

DES加密和解密算法

- Encrypt() 将指定字符串加密，并返回16进制编码的字符串
- Decrypt() 将16进制加密字符串解密，并返回明文

## MD5Helper类
MD5加密

- Encrypt() 将指定字符串进行md5加密，并返回16进制编码的字符串
- Encrypt2() 将指定字符串进行md5加密，并返回字节数组

## SHA256Helper类
SHA256加密
- Signature() 将指定字符串进行sha256加密，并返回16进制编码的字符串
 
