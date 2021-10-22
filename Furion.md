# Furion Overview

[toc]	

## 1. Furion是什么？

[Furion官方文档](https://dotnetchina.gitee.io/furion/docs/)

按作者阐述，Furion是一个底层框架，不做任何业务上的实现;

### 框架依赖

`Furion` 为了追求极速入门，极致性能，尽可能的不使用或减少第三方依赖。目前 `Furion` 仅集成了以下两个依赖：

- [MiniProfiler](https://github.com/MiniProfiler/dotnet)：性能分析和监听必备
- [Swashbuckle](https://github.com/domaindrivendev/Swashbuckle.AspNetCore)：`Swagger` 接口文档

麻雀虽小五脏俱全。`Furion` 即使只集成了这两个依赖，但是主流的 `依赖注入/控制反转`，`AOP` 面向切面编程，`事件总线`，`数据验证`，`数据库操作` 等等一个都不少

### 框架特点：

* 全新面貌：基于 `.NET5/6` 平台，没有历史包袱
* 极少依赖：框架只依赖两个第三方包
* 极易入门：只需要一个 `Inject()` 即可完成配置
* 极速开发：内置丰富的企业应用开发功能
* 极其灵活：轻松面对多变复杂的需求
* 极易维护：采用独特的架构思想，只为长久维护设计
* 完整文档：提供完善的开发文档
* **跨全平台：支持所有主流操作系统及 .NET 全部项目类型**



### 功能模块

![功能欧快](https://dotnetchina.gitee.io/furion/img/functions.png)



### 环境要求

- Visual Studio 2019 16.8 +
- .NET 5 SDK +
- .Net Standard 2.1 +



## 2.  `Fruion` 功能支持模块介绍

### 项目结构介绍

- `Furion`：框架核心层
- `Furion.Application`：业务应用层（业务代码主要编写层）
- `Furion.Core`：核心层（实体，仓储，其他核心代码）
- `Furion.Database.Migrations`：EFCore 架构迁移文件层
- `Furion.EntityFramework.Core`：EF Core 配置层
- `Furion.Web.Core`：Web 核心层（存放 Web 公共代码，如 过滤器、中间件、Web Helpers 等）
- `Furion.Web.Entry`：Web 入口层/启动层

![image-20210811134402773](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210811134402773.png)





### [实体和仓储](https://dotnetchina.gitee.io/furion/docs/entity)

* **[实体](https://dotnetchina.gitee.io/furion/docs/entity)**：支持通过数据库自动生成模型（DB first）

  提供powershell脚本来自动生成实体（可通过界面或者命令行方式）

```powershell
 &"D:\yizit\Furion\YizitApi\YizitApi\tools\cli.ps1" -CoreProject "YizitApi.Core" -EntryProject "YizitApi.Web.Entry" -DbContextLocators "SSODBContextLocator" -UseDatabaseNames -NameSpace "YizitApi.Core" -OutputDir "./YizitApi.Core/Entities/" -ConnectionName "SSO" -Context "SSODbContext"
```

![image-20210810134255130](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210810134255130.png)



![image-20210810134319545](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210810134319545.png)

![image-20210810134431562](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210810134431562.png)

![image-20210810134517321](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210810134517321.png)



* [仓储](https://dotnetchina.gitee.io/furion/docs/dbcontext-repository)

  仓储是数据存取操作的载体，是领域层和数据映射层的中介；

  furion在通过根据db自动生成实体后，这些实体就可以被仓储层直接使用，常用的内置仓储有

  1. **泛型仓储**:  `IRepository<TEntity>`：默认数据库实体仓储接口

     ```c#
     //构造函数注入
     private readonly IRepository<Person> _personRepository;
     public FurionService(IRepository<Person> personRepository)
     {
         _personRepository = personRepository;
     }
     
     //方法参数注入
     public async Task<List<PersonDto>> GetAll([FromServices] IRepository<Person> repository, string keyword)
     {
         var persons = await repository.AsQueryable().ToListAsync();
         return persons.Adapt<List<PersonDto>>();
     }
     
     //DB.GetReponsitory获取泛型仓储
     var repository = Db.GetRepository<Person>();
     ```

     

  2. **泛型多数据库实体仓储**: 通过数据库上下文定位器实现

     - `IRepository<TEntity, TDbContextLocator>`：任意数据库的实体仓储接口

  3. 其他还支持更多的仓储：sql操作仓储，只读实体仓储，只写实体仓储...



### [动态API](https://dotnetchina.gitee.io/furion/docs/dynamic-api-controller)

* **动态API**： 本质上是将普通的类变成Controller，支持控制器的一切功能，框架额外封装了更多的功能

  **原因**：为什么要改造，从furion作者的角度来说，原先的WebAPI控制器需要遵循以下约定：

  - 控制器类**必须继承 `ControllerBase` 或间接继承**
  - 动作方法**必须贴有 `[HttpMethod]` 特性，如：`[HttpGet]`**
  - 控制器或动作方法**至少有一个配置 `[Route]` 特性**
  - 生成 `WebAPI` 路由地址时会自动去掉控制器名称 `Controller` 后缀，同时也会去掉动作方法匹配的 `HttpVerb` 谓词，如 `GET，POST，DELETE，PUT` 等
  - **不支持返回非 `IEnumerable<T>` 泛型对象**
  - **不支持类类型参数在 `GET，HEAD` 请求下生成 `Query` 参数**

  除了上述约定外，`WebAPI` 路由地址**基本靠手工完成**，不利于书写，不利于维护，再者，在移动应用对接中难以进行多版本控制

  除了这些约定，`.NET Core WebAPI` 有以下缺点：

  - 路由地址基本靠手工完成

  - 在现在移动为王的时代，不利于进行多版本控制

  - 对接 `Swagger` 文档分组比较复杂

  - 实现 `Policy` 策略授权也比较复杂

  - 不支持控制器热插拔插件化

  - 难以实现复杂自定义的 `RESTful API` 风格

    

  **动态WebAPI优点**：这个方式在继承了 `ASP.NET Core WebAPI` 所有优点，同时进行了大量拓展和优化。优化后的 `WebAPI` 具有以下优点：

  - 具备原有的 `ControllerBase` 所有功能
  - 支持**任意公开 非静态 非抽象 非泛型类**转控制器
  - 提供更加灵活方便的 `IDynamicApiController` 空接口或 `[DynamicApiController]` 特性替代 `ControllerBase` 抽象类
  - **无需手动配置 `[HttpMethod]` 特性**，同时支持一个动作方法多个 `HttpVerb`
  - **无需手动配置 `[Route]` 特性**，支持更加灵活的配置及自动路由生成
  - 支持返回泛型接口，泛型类
  - **和 `Swagger` 深度结合，提供极其方便的创建 `Swagger` 分组配置**
  - 支持 `Basic Auth，Jwt，ApiKey` 等多种权限灵活配置
  - 支持控制器、动作方法**版本控制**功能
  - 支持 `GET、HEAD` 请求自动转换 `类类型参数`
  - 支持生成 `OAS3` 接口规范

* 范例

  - **`IDynamicApiController` 方式**

  ```c#
  using Furion.DynamicApiController;
  
  namespace Furion.Application
  {
      public class FurionAppService : IDynamicApiController
      {
          public string Get()
          {
              return $"Hello {nameof(Furion)}";
          }
      }
  }
  ```

  

  - **`[DynamicApiController]` 方式**

  ```c#
  using Furion.DynamicApiController;
  
  namespace Furion.Application
  {
      [DynamicApiController]
      public class FurionAppService
      {
          public string Get()
          {
              return $"Hello {nameof(Furion)}";
          }
      }
  }
  ```

  

  如下图所示，一个 `WebAPI` 接口就这么生成了。

  ![img](https://dotnetchina.gitee.io/furion/img/dyglz.gif)

![image-20210811135828615](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210811135828615.png)

### DI/IOC

* furion推荐尽量采用构造函数注入（如果这个类支持）

* furion中不支持属性注入

* furion提供了内置依赖接口

  * `ITransient`：对应暂时/瞬时作用域服务生存期
  * `IScoped`：对应请求作用域服务生存期
  * `ISingleton`：对应单例作用域服务生存期

  > 以上三个接口只能实例类实现，其他静态类、抽象类、及接口不能实现。

 根据以上介绍，我们有了实体，仓储以及controller(动态API)后，中间如果涉及到一些业务逻辑处理的程序，一般采用编写Service来实现，那么Service的实现中就可以继承这三个接口来供Controller层依赖注入

*  **范例**

  创建 `IBusinessService` 接口和 `BusinessService` 实现类，代码如下

  ```c#
  using Furion.Core;
  using Furion.DatabaseAccessor;
  using Furion.DependencyInjection;
  
  namespace Furion.Application
  {
      public interface IBusinessService
      {
          Person Get(int id);
      }
  
      public class BusinessService : IBusinessService, ITransient
      {
          private readonly IRepository<Person> _personRepository;
  
          public BusinessService(IRepository<Person> personRepository)
          {
              _personRepository = personRepository;
          }
  
          public Person Get(int id)
          {
              return _personRepository.Find(id);
          }
      }
  }
  ```

  创建 `PersonController` 控制器，代码如下

  ```c#
  using Furion.Application;
  using Microsoft.AspNetCore.Mvc;
  
  namespace Furion.Web.Entry.Controllers
  {
      [Route("api/[controller]")]
      [ApiController]
      public class PersonController : ControllerBase
      {
          private readonly IBusinessService _businessService;
          public PersonController(IBusinessService businessService)
          {
              _businessService = businessService;
          }
  
          [HttpGet]
          public IActionResult Get(int id)
          {
              var person = _businessService.Get(id);
              return new JsonResult(person);
          }
      }
  }
  ```

  ![img](https://dotnetchina.gitee.io/furion/img/di1.gif)



### AOP

* **AOP**： `面向切面`编程； 在Furion框架中，实现`AOP`非常简单， 创建代理类，继承`AspectDispatchProxy` `IDispatchProxy`

比如实现记录日志

* **代理类代码**

```c#
using Furion.DependencyInjection;
using System;
using System.Reflection;

namespace Furion.Application
{
    public class LogDispatchProxy : AspectDispatchProxy, IDispatchProxy
    {
        /// <summary>
        /// 当前服务实例
        /// </summary>
        public object Target { get; set; }

        /// <summary>
        /// 服务提供器，可以用来解析服务，如：Services.GetService()
        /// </summary>
        public IServiceProvider Services { get; set; }

        /// <summary>
        /// 拦截方法
        /// </summary>
        /// <param name="method"></param>
        /// <param name="args"></param>
        /// <returns></returns>
        public override object Invoke(MethodInfo method, object[] args)
        {
            Console.WriteLine("SayHello 方法被调用了");

            var result = method.Invoke(Target, args);

            Console.WriteLine("SayHello 方法返回值：" + result);

            return result;
        }

        // 异步无返回值
        public override async Task InvokeAsync(MethodInfo method, object[] args)
        {
            Console.WriteLine("SayHello 方法被调用了");

            var task = method.Invoke(Target, args) as Task;
            await task;

             Console.WriteLine("SayHello 方法调用完成");
        }

        // 异步带返回值
        public override async Task<T> InvokeAsyncT<T>(MethodInfo method, object[] args)
        {
            Console.WriteLine("SayHello 方法被调用了");

            var taskT = method.Invoke(Target, args) as Task<T>;
            var result = await taskT;

            Console.WriteLine("SayHello 方法返回值：" + result);

            return result;
        }
    }
}
```

* **service代码**

```c#
using Furion.DatabaseAccessor;
using Furion.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YizitApi.Application.AOP;
using YizitApi.Core;

namespace YizitApi.Application.BuinessLayer
{

    [Injection(Proxy = typeof(LogDispatchProxy))]
    public class PrintService: ITransient, IPrintService
    {
        private readonly IRepository<SLogPrintT> _responsitory4Log;

        public PrintService(IRepository<SLogPrintT> responsitory4Log)
        {
            _responsitory4Log = responsitory4Log;
        }

        public PagedList<SLogPrintT> GetOperationList()
        {

            var data = _responsitory4Log.AsQueryable().ToPagedList(1, 20);
            return data;
        }
    }
   
    public interface IPrintService
    {
        PagedList<SLogPrintT> GetOperationList();
    }
}
```

* **Controller代码**

```c#
using Furion.DatabaseAccessor;
using Furion.DatabaseAccessor.Extensions;
using Furion.DynamicApiController;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YizitApi.Application.BuinessLayer;
using YizitApi.Core;

namespace YizitApi.Web.Entry.Controller
{
    [ApiDescriptionSettings( "Shipment")]
    [AllowAnonymous]
    public class PrintLogService: IDynamicApiController
    {
        private readonly IPrintService _service;

        public PrintLogService(IPrintService service)
        {
            _service = service;
        }
        public List<dynamic> GetList()
        {
            //数据库的时候，走的是ado.net，实体的映射，除非是实体和数据库字段名称一样，要不然这里如果用实体列表直接返回可能会取不到值
            var data = "Select * from S_Log_Print_T".SqlQuery<dynamic>();
            return data;
        }

        public PagedList<SLogPrintT> GetOperationList()
        {

            var data = _service.GetOperationList(); ;
            return data;
        }
    }
}

```

### [数据校验(DTO校验)](https://dotnetchina.gitee.io/furion/docs/data-validation)

* `DataValidation` 是 `Furion` 框架提供了全新的验证方式，完全兼容 `Mvc` 内置验证，并且赋予了超能。

* **`DataValidation`**优点
  * **完全兼容 `Mvc` 内置验证引擎**
  * **内置常见验证类型及可自定义验证类型功能**
  * 提供全局对象拓展验证方式
  * 支持验证消息后期配置，支持实时更新
  * 支持在任何类，任何方法、任何位置实现手动验证、特性方式验证等
  * 支持设置验证结果模型



* 兼容MVC验证特性

  ```c#
  using System.ComponentModel.DataAnnotations;
  
  namespace Furion.Application
  {
      public class TestDto
      {
          [Range(10, 20, ErrorMessage = "Id 只能在 10-20 区间取值")]
          public int Id { get; set; }
  
          [Required(ErrorMessage = "必填"), MinLength(3, ErrorMessage = "字符串长度不能少于3位")]
          public string Name { get; set; }
      }
  }
  ```

  ```c#
  using System.Collections.Generic;
  using System.ComponentModel.DataAnnotations;
  
  namespace Furion.Application
  {
      //兼容MVC复杂验证
      public class TestDto : IValidatableObject
      {
          [Range(10, 20, ErrorMessage = "Id 只能在 10-20 区间取值")]
          public int Id { get; set; }
  
          [Required(ErrorMessage = "必填"), MinLength(3, ErrorMessage = "字符串长度不能少于3位")]
          public string Name { get; set; }
  
          public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
          {
              // 还可以解析服务
              var service = validationContext.GetService(typeof(类型));
  
              if (Name.StartsWith("Furion"))
              {
                  yield return new ValidationResult(
                      "不能以 Furion 开头"
                      , new[] { nameof(Name) }
                  );
              }
          }
      }
  }
  ```

* Furion框架提供了很多常见的验证`ValidationTypes`

  `Furion` 内置了很多常用类型的数据验证，包括：

  - `Numeric`：数值类型
  - `PositiveNumber`：正数类型
  - `NegativeNumber`：负数类型
  - `Integer`：整数类型
  - `Money`：金钱类型
  - `Date`：日期类型
  - `Time`：时间类型
  - `IDCard`：身份证类型
  - `PostCode`：邮编类型
  - `PhoneNumber`：手机号类型
  - `Telephone`：固话类型
  - `PhoneOrTelNumber`：手机或固话类型
  - `EmailAddress`：邮件地址类型
  - `Url`：网址类型
  - `Color`：颜色值类型
  - `Chinese`：中文类型
  - `IPv4`：IPv4 地址类型
  - `IPv6`：IPv6 地址类型
  - `Age`：年龄类型
  - `ChineseName`：中文名类型
  - `EnglishName`：英文名类型
  - `Capital`：纯大写英文类型
  - `Lowercase`：纯小写英文类型
  - `Ascii`：Ascii 类型
  - `Md5`：Md5 字符串类型
  - `Zip`：压缩包格式类型
  - `Image`：图片格式类型
  - `Document`：文档格式类型
  - `MP3`：Mp3 格式类型
  - `Flash`：Flash 格式类型
  - `Video`：视频文件格式类型

  ```c#
  using Furion.DataValidation;
  
  namespace Furion.Application
  {
      public class TestDto
      {
          [DataValidation(ValidationTypes.Integer)]
          public int Id { get; set; }
  
          [DataValidation(ValidationTypes.Numeric, ValidationTypes.Integer)]
          public int Cost { get; set; }
  
          [DataValidation(ValidationPattern.AtLeastOne, ValidationTypes.Chinese, ValidationTypes.Date)]
          public string Name { get; set; }
  
          // 可以和Mvc特性共存
          [Required, DataValidation(ValidationTypes.Age)]
          public int Age { get; set; }
  
          [DataValidation(ValidationTypes.IDCard, ErrorMessage = "自定义身份证提示消息")]
          public string IDCard { get; set; }
      }
  }
  ```

* 自定义`ValidationTypes` 类型

  ```c#
  using Furion.DataValidation;
  using System.Text.RegularExpressions;
  
  namespace Furion.Application
  {
      [ValidationType]
      public enum MyValidationTypes
      {
          /// <summary>
          /// 强密码类型
          /// </summary>
          [ValidationItemMetadata(@"^(?=.*\d)(?=.*[a-z])(?=.*[A-Z]).{8,10}$", "必须须包含大小写字母和数字的组合，不能使用特殊字符，长度在8-10之间")]
          StrongPassword,
  
          /// <summary>
          /// 以 Furion 字符串开头，忽略大小写
          /// </summary>
          [ValidationItemMetadata(@"^(furion).*", "默认提示：必须以Fur字符串开头，忽略大小写", RegexOptions.IgnoreCase)]
          StartWithFurString
      }
  }
  
  ```

  ```c#
  //手动使用
  "q1w2e3".TryValidate(MyValidationTypes.StrongPassword); // => false
  
  "furos".TryValidate(MyValidationTypes.StartWithFurString); // => true
  //DataValidation 中使用
  [DataValidation(MyValidationTypes.StrongPassword)]
  public string Password { get; set; }
  ```

* [集成第三方校验 `FluentValidation`](https://fluentvalidation.net/)

  安装拓展包`FluentValidation.AspNetCore`

  ```powershell
  dotnet add package FluentValidation.AspNetCore
  ```

  注册

  ```c#
  services.AddControllers()
          .AddFluentValidation(fv => {
              fv.RegisterValidatorsFromAssemblies(App.Assemblies);
          });
  ```

  使用例子

  ```c#
  public class Person {
      public int Id { get; set; }
      public string Name { get; set; }
      public string Email { get; set; }
      public int Age { get; set; }
  }
  
  public class PersonValidator : AbstractValidator<Person> {
      public PersonValidator() {
          RuleFor(x => x.Id).NotNull();
          RuleFor(x => x.Name).Length(0, 10);
          RuleFor(x => x.Email).EmailAddress();
          RuleFor(x => x.Age).InclusiveBetween(18, 60);
      }
  }
  ```

  

### [友好异常 Oops.Oh](https://dotnetchina.gitee.io/furion/docs/friendly-exception)

* furion处理异常的方式采用了全局统一处理，并支持异常注解的方式处理

* furion友好异常处理支持

  * 对终端用户提示友好
  * 对后端开发人员提供详细的异常堆栈
  * 不干扰正常业务逻辑代码，如 没有 `try catch` 代码
  * 支持异常状态码多方设置
  * 支持异常消息本地化
  * 异常信息统一配置管理
  * 支持异常策略，如重试
  * 支持异常日志收集记录
  * 支持 CAP 分布式事务关联
  * 支持内部异常外部传播
  * 支持返回统一的异常格式数据

* 异常最佳实践

  实现自定义异常信息类型必须遵循以下配置：

  - 类型必须是公开且是 `Enum` 枚举类型
  - 枚举类型必须贴有 `[ErrorCodeType]` 特性
  - 枚举中每一项必须贴有 `[ErrorCodeItemMetadata]` 特性

  ```c#
  using Furion.FriendlyException;
  
  namespace Furion.Application
  {
      [ErrorCodeType]
      public enum ErrorCodes
      {
          [ErrorCodeItemMetadata("{0} 不能小于 {1}")]
          z1000,
  
          [ErrorCodeItemMetadata("数据不存在")]
          x1000,
  
          [ErrorCodeItemMetadata("{0} 发现 {1} 个异常", "百小僧", 2)]
          x1001,
  
          [ErrorCodeItemMetadata("服务器运行异常", ErrorCode = "Error")]
          SERVER_ERROR
      }
  
      [ErrorCodeType]
      public enum UserErrorCodes
      {
          [ErrorCodeItemMetadata("用户数据不存在")]
          u1000,
  
          [ErrorCodeItemMetadata("其他异常")]
          u1001
      }
  }
  ```

  `Furion` 框架提供了`[ErrorCodeItemMetadata]` 特性用来标识**枚举字段**异常元数据，该特性支持传入 `消息内容` 和 `格式化参数`。最终会使用 `String.Format(消息内容，格式化参数)` 进行格式化

  ```c#
  using Furion.DynamicApiController;
  using Furion.FriendlyException;
  
  namespace Furion.Application
  {
      public class FurionAppService : IDynamicApiController
      {
          public int Get(int id)
          {
              if (id < 3)
              {
                  throw Oops.Oh(ErrorCodes.z1000, id, 3);
              }
  
              return id;
          }
      }
  }
  ```

  异常方法重试

  ```c#
  Oops.Retry(() => {
      // Do.....
  }, 3, 1000);
  
  // 带返回值
  var value = Oops.Retry<int>(() => {
      // Do.....
  }, 3, 1000);
  
  // 只有特定异常才监听
  Oops.Retry(() => {
  
  }, 3, 1000, typeof(ArgumentNullException));
  ```

  ```c#
  throw Oops.Oh(1000);
  throw Oops.Oh(ErrorCodes.x1000);
  throw Oops.Oh("哈哈哈哈");
  throw Oops.Oh(errorCode: "x1001");
  throw Oops.Oh(1000, typeof(Exception));
  throw Oops.Oh(1000).StatusCode(400);    // 设置错误码
  throw Oops.Bah("用户名或密码错误"); // 抛出业务异常，状态码为 400
  throw Oops.Bah(1000);
  ```

###  安全鉴权（身份认证）

1. 基本介绍

   这里鉴权我们采用Token方式（JWT）。 Furion本身还支持另外的其他鉴权方式，比如Cookie身份验证，支持混合身份验证（cookie+token...）

   JSON WEB TOKEN(JWT)是目前最流行的跨域身份验证解决方案

   [JWT的官网地址](https://jwt.io/)

   通俗地来讲，JWT是能代表用户身份的令牌，可以使用JWT令牌在api接口中校验用户的身份以确认用户是否有访问api的权限。

   JWT中包含了身份认证必须的参数以及用户自定义的参数

   

2. 使用方式

   * 调用登录接口，获取返回token

   * 客户端请求头（XXX.YYY.ZZZ 是token，通过调用登录接口获取）

```JSON
authorization: Bearer XXX.YYY.ZZZ
```

![image-20210810111159485](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210810111159485.png)

   * 匿名访问

​	     如果需要对特定的 `Action` 或 `Controller` 允许匿名访问，则贴 `[AllowAnonymous]` 即可

![image-20210810111116323](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210810111116323.png)



​	[OAuth2.0](http://www.zyiz.net/tech/detail-146615.html)

​	[OAuth2.0实现微信登录](https://blog.csdn.net/qq_42005257/article/details/94559633)

### [获取用户等信息（App静态类）](https://dotnetchina.gitee.io/furion/docs/global/app)

* 获取登录`User`对象

  ```c#
  var contextUser = App.User;
  
  // 获取 `Jwt` 存储的信息
  var userId = App.User?.FindFirst("UserId").Value;
  ```

  

* 获取`HttpContext`

  ```c#
  var httpContext = App.HttpContext;
  ```

* 获取配置对象

  ```c#
  // 获取 IConfiguration 对象
  var configuration = App.Configuration;
  var value = configuration["xxx:xxx"];
  
  // 获取指定节点值并转成 T 类型
  var data = App.GetConfig<TConfig>("key:key2");
  
  // 重载/刷新配置
  App.Configuration.Reload();
  ```

### 国际化

​	furion完整支持多语言处理服务，步骤基本分：

 1. 注册服务

 2. 配置`LocalizationSettings` 定义语言列表以及默认语言

 3. 增加Resource文件

    

    **L静态类**：如何使用

    - `L.Text[文本]`：转换文本多语言

    

> 所有验证特性已自动支持多语言配置
>
> 所以异常消息已自动支持多语言配置

### [数据一致性（事务）](https://dotnetchina.gitee.io/furion/docs/tran)

 * 框架中提供了在控制器的Action中黏贴`[UnitOfWork]`特性开启工作单元模式，保证每一次请求都是一个整体，要么同时成功，要么同时失败；

   ![image-20210811163247603](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210811163247603.png)



### [多租户](https://dotnetchina.gitee.io/furion/docs/saas)

* 多租户技术或称多重租赁技术，简称 `SaaS`，是一种软件架构技术，是实现如何在多用户环境下（此处的多用户一般是面向企业用户）共用相同的系统或程序组件，并且可确保各用户间数据的隔离性

* furion实现多租户的方案，支持

  1. 独立数据库（基于Database的方式）
     * **优点**：隔离性最高，为不同的租户提供独立的数据库；如果出现故障，恢复数据比较简单
     * **缺点**：增多了数据库的安装数量，随之带来维护成本和购置成本的增加。 这种方案与传统的一个客户、一套数据、一套部署类似。如果定价较低，产品走低价路线，这种方案一般对运营商来说是无法承受的

  2. 共享数据库，独立Schema（基于Schema的方式）
     * **优点**：为安全性要求较高的租户提供了一定程度的逻辑数据隔离，并不是完全隔离；每个数据库可支持更多的租户数量
     * **缺点**：如果出现故障，数据恢复比较困难，因为恢复数据库将牵涉到其他租户的数据； 如果需要跨租户统计数据，存在一定困难

  3. **共享数据库，共享Schema（基于TenantId的方式）**: 
     * **优点**：共享程度最高，维护和购置成本最低，允许每个数据库支持的租户数量最多；
     * **缺点**：隔离级别最低，安全性最低；

* 三种方案各有优缺点， 采用什么方案，基本从以下角度来考虑，常用的是3和1；

  * **成本角度考虑**： 隔离性越好，设计和实现的难度和成本越高，初始成本越高。共享性越好，同一运营成本下支持的用户越多，运营成本越低
  * **安全因素**：要考虑业务和客户的安全方面的要求。安全性要求越高，越要倾向于隔离
  * **租户数量**：
    * 系统要支持多少租户？上百？上千还是上万？可能的租户越多，越倾向于共享。
    * 平均每个租户要存储数据需要的空间大小。存贮的数据越多，越倾向于隔离。
    * 每个租户的同时访问系统的最终用户数量。需要支持的越多，越倾向于隔离。
    * 是否想针对每一租户提供附加的服务，例如数据的备份和恢复等。这方面的需求越多， 越倾向于隔离

### [数据映射Mapper](https://dotnetchina.gitee.io/furion/docs/object-mapper)

​	减少代码编写

 * Furion默认采用了Mapster映射框架

   >在 `Furion.Core` 层安装 `Furion.Extras.ObjectMapper.Mapster` 拓展包，无需手动调用，`Furion` 会自动加载并调用。

   通过 `Mapster` 提供的对象映射方法：`Adapt` 方法改造上面的例子

* 最佳实践

  ```c#
  //没有映射框架前写法
  var entity = repository.Find(1);
  
  var dto = new Dto();
  dto.Id = entity.Id;
  dto.Name = entity.Name;
  dto.Age = entity.Age;
  dto.Address = entity.Address;
  dto.FullName = entity.FirstName + entity.LastName;
  dto.IdCard = entity.IdCard.Replace("1234", "****");
  
  //用映射框架改进写法
  var entity = repository.Find(1);
  var dto = entity.Adapt<Dto>();
  ```

  如果涉及到`entity->dto` 的转换，涉及到赋值的赋值操作，自定义映射规则

  ```c#
  using Mapster;
  using System;
  
  namespace Furion.Application
  {
      public class Mapper : IRegister
      {
          public void Register(TypeAdapterConfig config)
          {
              config.ForType<Entity, Dto>()
                  .Map(dest => dest.FullName, src => src.FirstName + src.LastName)
                  .Map(dest => dest.IdCard, src => src.IdCard.Replace("1234", "****"));
          }
      }
  }
  ```

  >该映射文件 `Mapper.cs` 可以放在任何项目或文件夹中，`Furion` 会在程序启动的时候自动扫描并注入配置。

* Master除了Adapt拓展方法以外，还提供依赖注入的方式

  ```c#
  public  Person(IMapper mapper)
  {
    var dto =  _mapper.Map<Dto>(entity);
  }
  ```

  

### [日志使用](https://dotnetchina.gitee.io/furion/docs/logging)

`.NET 5` 框架中并未提供写入`文件、数据库` 或其他介质的提供器，默认只提供了 `Debug、Console` 两种方式。这个时候我们就需要引用第三方日志组件，方便我们写入到多个介质中。

在这里，`Furion` 官方推荐使用 `Serilog` 日志组件，为此，`Furion` 提供了 `Furion.Extras.Logging.Serilog` 拓展包，方便快速和 `Furion` 框架结合

![image-20210813103747961](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210813103747961.png)

`Serilog` 拓展包使用[#](https://dotnetchina.gitee.io/furion/docs/logging#1851-serilog-拓展包使用)

- 安装 `Furion.Extras.Logging.Serilog` 拓展包

在 `Furion.Core` 层安装 `Furion.Extras.Logging.Serilog` 拓展包

- 在 `Program.cs` 中调用 `UseSerilogDefault()`

  ```c#
  using Microsoft.AspNetCore.Hosting;
  using Microsoft.Extensions.Hosting;
  
  namespace Furion.Web.Entry
  {
      public class Program
      {
          public static void Main(string[] args)
          {
              CreateHostBuilder(args).Build().Run();
          }
  
          public static IHostBuilder CreateHostBuilder(string[] args)
          {
              return Host.CreateDefaultBuilder(args)
                  .ConfigureWebHostDefaults(webBuilder =>
                  {
                      webBuilder.Inject()
                          .UseStartup<Startup>();
                  })
                  .UseSerilogDefault();
          }
      }
  }
  ```

  

### [远程请求](https://dotnetchina.gitee.io/furion/docs/http)

- 跨系统、跨设备通信

- 实现多个系统数据传输交互

- 跨编程语言协同开发

  使用之前需在 `Startup.cs` 注册 `远程请求服务`

  ```c#
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddRemoteRequest();
  }
  ```

  

### 规范化接口文档

* furion默认用swagger，用于生成，描述，调用和可视化的restful风格的web服务
* 默认启用Bearer Token授权配置，无需手动配置
* 支持性能见识MiniProfiler
* 支持分组，通过 `ApiDescriptionSettings`
* 支持组中组，通过`ApiDescriptionSettings`

![image-20210811164833641](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210811164833641.png)

![image-20210813085028767](C:\Users\dongm\AppData\Roaming\Typora\typora-user-images\image-20210813085028767.png)

```c#
[ApiDescriptionSettings( "Shipment")]
    [AllowAnonymous]
    public class PrintLogService: IDynamicApiController
```



### [跨域](https://dotnetchina.gitee.io/furion/docs/cors)

​	furion默认允许所有域名来源访问，无需配置； 前端请设置请求参数 withCredentials:false

​	如果需要配置，请增加如下相应配置

   ```json
{
  "CorsAccessorSettings": {
    "PolicyName": "自定义跨域策略名",
    "WithOrigins": ["http://localhost:4200", "https://furion.pro"]
  }
}
   ```

 `CorsAccessorSettings`配置

- ```
  CorsAccessorSettings
  ```

  - `PolicyName`：跨域策略名，字符串类型，必填，默认 `FurCorsAccessor`
  - `WithOrigins`：允许跨域的域名列表，字符串数组类型，默认 `[ "http://localhost:4200" ]`
  - `WithHeaders`：请求表头，没有配置则允许所有表头，字符串数组类型
  - `WithExposedHeaders`：响应标头，字符串数组类型
  - `WithMethods`：设置跨域允许请求谓词，没有配置则允许所有，字符串数组类型
  - `AllowCredentials`：跨域请求中的凭据，`bool` 类型
  - `SetPreflightMaxAge`：设置预检过期时间，`int` 类型，单位秒，**此配置可以控制客户端发送非 `GET`，`HEAD`，`POST` 请求前发送 `OPTION` 请求检查，状态码（204）

### [数据加解密](https://dotnetchina.gitee.io/furion/docs/encryption)

由于现在的互联网越具发达，数据成为了我们生活的一部分，当然也带来了很多数据安全性的问题，比如用户密码明文存储，用户信息明文存在在浏览器 `cookies` 中等等不安全操作

furino内置了一些加密算法

- `MD5` 加密
- `DESC` 加解密
- `AES` 加解密
- `JWT` 加解密
- `RSA` 加解密

另外提供字符串的拓展方式来实现加解密

```c#
// 测试 DESC 加解密
var descHash = DESCEncryption.Encrypt("百小僧", "Furion"); // 加密
var str = DESCEncryption.Decrypt(descHash, "Furion");  // 解密
return (descHash, str);
```



### [JSON序列化](https://dotnetchina.gitee.io/furion/docs/json-serialization)

JSON，是一种轻量级的数据数据格式，在与后端的数据交互中有较为广泛的应用

目前在 C# 语言中有两个主流的 `JSON` 序列化操作库：

- `System.Text.Json`：`.NET Core` 内置 `JSON` 序列化库，也是 `Furion` 框架默认实现
- `Newtonsoft.Json`：目前使用人数最多的 `JSON` 序列化库，需要安装 `Microsoft.AspNetCore.Mvc.NewtonsoftJson` 拓展包

`Furion` 框架为了解决多种序列化工具配置和用法上的差异问题，抽象出了 `IJsonSerializerProvider` 接口



### [缓存](https://dotnetchina.gitee.io/furion/docs/cache)

缓存可以减少生成内容所需的工作，从而显著提高应用程序的性能和可伸缩性。 **缓存最适用于不经常更改的数据，因为生成成本很高**。 通过缓存，可以比从数据源返回的数据的副本速度快得多。 应该对应用进行编写和测试，使其不要永远依赖于缓存的数据

> ##### 备注
>
> 在 `Furion` 框架中，内存缓存服务已经默认注册，无需手动注册

常用的缓存包括内存缓存和分布式缓存

#### **内存缓存**

内存缓存是最常用的缓存方式，具有存取快，效率高特点。

内存缓存通过注入 `IMemoryCache` 方式注入即可。

> furion已经内置内存缓存，无需手动注册

基本使用: 如，缓存当前时间：

```c#
using Furion.DynamicApiController;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Furion.Application
{
    public class CacheServices : IDynamicApiController
    {
        private const string _timeCacheKey = "cache_time";

        private readonly IMemoryCache _memoryCache;

        public CacheServices(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [ApiDescriptionSettings(KeepName = true)]
        public DateTimeOffset GetOrCreate()
        {
            return _memoryCache.GetOrCreate(_timeCacheKey, entry =>
            {
                return DateTimeOffset.UtcNow;
            });
        }
    }
}
```

#### **分布式缓存**

分布式缓存是由多个应用服务器共享的缓存，通常作为外部服务在访问它的应用服务器上维护。

与其他缓存方案相比，分布式缓存具有多项优势，其中缓存的数据存储在单个应用服务器上。

当分布式缓存数据时，数据将：

- (一致性) 跨多个 服务器的请求
- 存活在服务器重启和应用部署之间
- 不使用本地内存

常用的分布式缓存有`SQL Server`,`Redis`,`NCache`,`分布式内存缓存`等.



### [即时通讯](https://dotnetchina.gitee.io/furion/docs/signalr)

即时通讯（Instant messaging，简称 IM）通常是指互联网上用以进行实时通讯的系统；

即时通讯技术实现是复杂且很底层化，微软目前为了简化即时通讯，提供了一个强大并且容易使用的通辛苦：`SignalR`;

通过该库我们可以轻松实现类似 QQ、微信这类 IM 聊天工具，也能快速实现消息推送、订单推送这样的系统。

**类似我们订单发货系统中，各个解析服务，圆整服务中前后端的通信就通过`SignalR`通辛苦实现的**

在 `Furion` 框架中，要在 `Startup.cs` 中先添加注册`SignalR`

`SignalR` 包含两种用于在客户端和服务器之间进行通信的模型：`持久性连接`和 `集线器` 中心。

一般我们常用集线器；

集线器是一种基于连接 API 构建的更高级别管道，**它允许客户端和服务器直接调用方法**。 `SignalR` 就像魔术一样处理跨机器边界的调度，使客户端能够像本地方法一样轻松地调用服务器上的方法，反之亦然。 如果开发人员已使用远程调用 （如 .NET 远程处理），则将对使用中心通信模型非常熟悉。 使用集线器还可以将强类型参数传递给方法，从而启用模型绑定。

> ##### 小知识
>
> 想了解更多关于 `持久性连接` 和 `集线器中心` 可查阅 [SignalR 官方文档](https://docs.microsoft.com/zh-cn/aspnet/signalr/overview/getting-started/introduction-to-signalr#connections-and-hubs)

### [事件总线](https://dotnetchina.gitee.io/furion/docs/event-bus)

事件总线是对发布-订阅模式的一种实现。它是一种集中式事件处理机制，允许不同的组件之间进行彼此通信而又不需要相互依赖，达到一种解耦的目的。

Furion框架中提供了一种轻量级的事件总线实现机制： `MessageCenter`消息中心，`MessageCenter`采用字符串消息机制进行广播，可以再绝大多数据中小型项目中发挥作用，缺点是消息处理是在主线程中完成并且消息不支持分布式存储；

另外，`MessageCenter`支持单播，多播发布及多订阅，如图

![img](https://dotnetchina.gitee.io/furion/img/event2.png)



### [定时任务](https://dotnetchina.gitee.io/furion/docs/job)

定时任务就是在特定的时间或符合某种时间规律执行的任务。通常定时任务有四种时间调度方式：

- `缓隔时间` 方式：延迟多少时间后调配任务，这种方式任务只会被调用一次。
- `间隔时间` 方式：每隔一段固定时间调配任务，无间断调用任务。
- `Cron 表达式` 方法：通过 `Cron` 表达式计算下一次执行时间进行调配任务，可以配置特定时间范围内执行，也可以无间断执行。
- `自定义下次执行时间`：可以通过各种逻辑运算返回下一次执行时间

Furion框架提供了两种方式实现定时任务：

- `SpareTime` 静态类：`SpareTime` 静态类提供 `SpareTime.Do([options])` 方式调用。
- `ISpareTimeWorker` 依赖方式：通过自定义类实现 `ISpareTimeWorker` 接口并编写一定规则的方法即可。**需要在 `Startup.cs` 中注册 `services.AddTaskScheduler()`**

常用样例

1. **三秒后执行**

```c#
Console.WriteLine("当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

// timer 是定时器的对象，包含定时器相关信息
// count 表示执行次数，这里只有一次
SpareTime.DoOnce(3000, (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
});
```



2. **模拟后台执行**

```c#
// 此方法无需主线程等待即可返回，可大大提高性能
SpareTime.DoIt(() => {
    // 这里发送短信，发送邮件或记录访问记录
});
```



3. ### `ISpareTimeWorker` 方式

```c#
public class JobWorker : ISpareTimeWorker
{
    /// <summary>
    /// 3s 后执行
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime(3000, "jobName", DoOnce = true, StartNow = true)]
    public void DoSomething(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    /// <summary>
    /// 3s 后执行（支持异步）
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime(3000, "jobName", DoOnce = true, StartNow = true)]
    public async Task DoSomethingAsync(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        await Task.Completed;
    }
}
```



## 3. Furion功能模块具体使用介绍

### 安全鉴权

> 在添加授权服务之前，请先确保 `Startup.cs` 中 `Configure` 是否添加了以下两个中间件：
>
> ```c#
>   //授权中间件
>             app.UseAuthentication();
>             app.UseAuthorization();
> ```
>
> ##### 特别注意
>
> `JWT` 鉴权并未包含在 `Furion` 框架中，需要安装 `Furion` 框架提供的 `Furion.Extras.Authentication.JwtBearer` 拓展包。
>
> 另外 `.AddJwt()` 必须在 `.AddControllers()` 之前注册

* 添加jwt身份验证

```c#
//jwt身份验证
            services.AddJwt<JwtHandler>(enableGlobalAuthorize: true);
```

*  高级自定义授权: Furion` 框架提供了 `AppAuthorizeHandler` 策略授权处理程序提供基类，只需要创建自己的 `Handler` 继承它即可。如：`JwtHandler

  ```c#
  using Furion.Authorization;
  using Furion.Core;
  using Microsoft.AspNetCore.Authorization;
  using Microsoft.AspNetCore.Http;
  using Microsoft.IdentityModel.JsonWebTokens;
  
  namespace Furion.Web.Core
  {
      /// <summary>
      /// JWT 授权自定义处理程序
      /// </summary>
      public class JwtHandler : AppAuthorizeHandler
      {
          /// <summary>
          /// 请求管道
          /// </summary>
          /// <param name="context"></param>
          /// <param name="httpContext"></param>
          /// <returns></returns>
          public override Task<bool> PipelineAsync(AuthorizationHandlerContext context, DefaultHttpContext httpContext)
          {
              // 此处已经自动验证 Jwt token的有效性了，无需手动验证
  
              // 检查权限，如果方法是异步的就不用 Task.FromResult 包裹，直接使用 async/await 即可
              return Task.FromResult(CheckAuthorzie(httpContext));
          }
  
          /// <summary>
          /// 检查权限
          /// </summary>
          /// <param name="httpContext"></param>
          /// <returns></returns>
          private static bool CheckAuthorzie(DefaultHttpContext httpContext)
          {
              // 获取权限特性
              var securityDefineAttribute = httpContext.GetMetadata<SecurityDefineAttribute>();
              if (securityDefineAttribute == null) return true;
  
              return "查询数据库返回是否有权限";
          }
      }
  }
  ```

  

- 自定义 `Jwt` 配置（默认无需配置）

```json
{
  "JWTSettings": {
    "ValidateIssuerSigningKey": true, // 是否验证密钥，bool 类型，默认true
    "IssuerSigningKey": "4da-a888-08616091be40-Yizit-TE-DM", // 密钥，string 类型，必须是复杂密钥，长度大于16
    "ValidateIssuer": true, // 是否验证签发方，bool 类型，默认true
    "ValidIssuer": "Yizit", // 签发方，string 类型
    "ValidateAudience": true, // 是否验证签收方，bool 类型，默认true
    "ValidAudience": "YizitCustomer", // 签收方，string 类型
    "ValidateLifetime": true, // 是否验证过期时间，bool 类型，默认true，建议true
    "ExpiredTime": 20, // 过期时间，long 类型，单位分钟，默认20分钟
    "ClockSkew": 5, // 过期时间容错值，long 类型，单位秒，默认 5秒
    "Algorithm": "HS256" // 加密算法，string 类型，默认 SecurityAlgorithms.HmacSha256
  }
}
```



### PowerShell自动生成实体（DB First）

[数据库生成模型](https://dotnetchina.gitee.io/furion/docs/dbcontext-db-first)

```powershell
 &"D:\yizit\Furion\YizitApi\YizitApi\tools\cli.ps1" -CoreProject "YizitApi.Core" -EntryProject "YizitApi.Web.Entry" -DbContextLocators "SSODBContextLocator" -UseDatabaseNames -NameSpace "YizitApi.Core" -OutputDir "./YizitApi.Core/Entities/" -ConnectionName "SSO" -Context "SSODbContext"
```



支持参数如下：

- `-Tables`：配置要生成的数据库表，数组类型，如果为空，则生成数据库所有表和视图。如：`-Tables Person,PersonDetails`

- `-Context`：配置数据库上下文，默认 `FurionDbContext`，如果有多个数据库上下文，则此参数必须配置

- `-ConnectionName`：配置数据库连接字符串，对应 `appsetting.json` 中的 `ConnectionStrings` 定义的 `Key`

- `-OutputDir`：生成实体代码输出目录，默认为：`./Furion.Core/Entities/`

- ```
  -DbProvider
  ```

  ：数据库提供器，默认是

   

  ```
  Microsoft.EntityFrameworkCore.SqlServer
  ```

  ，其他数据库请指定对应程序集

  - `SqlServer`：`Microsoft.EntityFrameworkCore.SqlServer`
  - `Sqlite`：`Microsoft.EntityFrameworkCore.Sqlite`
  - `Cosmos`：`Microsoft.EntityFrameworkCore.Cosmos`
  - `InMemoryDatabase`：`Microsoft.EntityFrameworkCore.InMemory`
  - `MySql`：`Pomelo.EntityFrameworkCore.MySql` 或 `MySql.EntityFrameworkCore`
  - `PostgreSQL`：`Npgsql.EntityFrameworkCore.PostgreSQL`
  - `Oracle`：`Oracle.EntityFrameworkCore`
  - `Dm`：`Microsoft.EntityFrameworkCore.Dm`

- `-EntryProject`：Web 启用项目层名，默认 `Furion.Web.Entry`

- `-CoreProject`：实体项目层名，默认 `Furion.Core`

- `-DbContextLocators`：多数据库上下文定位器，默认 `MasterDbContextLocator`，支持多个，如：`MasterDbContextLocator,MySqlDbContextLocator`

- `-Product`：解决方案默认前缀，如 `Furion`

- `-UseDatabaseNames`：是否保持生成和数据库、表一致的名称

- `-Namespace`：指定实体命名空间



### [数据加解密](https://dotnetchina.gitee.io/furion/docs/encryption)

furion内置加密算法

- `MD5` 加密
- `DESC` 加解密
- `AES` 加解密
- `JWT` 加解密
- `RSA` 加解密

样例：

* `MD5`加密

```c#
// 测试 MD5 加密，比较
var md5Hash = MD5Encryption.Encrypt("百小僧");  // 加密
var isEqual = MD5Encryption.Compare("百小僧", md5Hash); // 比较
return (md5Hash, isEqual);

// 输出大写 MD5 加密
var md5Hash = MD5Encryption.Encrypt("百小僧", true);
```



* `DESC加解密

```c#
// 测试 DESC 加解密
var descHash = DESCEncryption.Encrypt("百小僧", "Furion"); // 加密
var str = DESCEncryption.Decrypt(descHash, "Furion");  // 解密
return (descHash, str);
```

* `AES`加解密

```c#
// 测试 AES 加解密
var key = Guid.NewGuid().ToString("N"); // 密钥，长度必须为24位或32位

var aesHash = AESEncryption.Encrypt("百小僧", key); // 加密
var str2 = AESEncryption.Decrypt(aesHash, key); // 解密
return (aesHash, str2);
```

* `JWT`加解密

```c#
var token = JWTEncryption.Encrypt(new Dictionary<string, object>()  // 加密
            {
                { "UserId", user.Id },
                { "Account",user.Account }
            });

var tokenData = JWTEncryption.ReadJwtToken("你的token");  // 解密

var (isValid, tokenData) = JWTEncryption.Validate("你的token"); // 验证token有效期
```

> ##### 特别注意
>
> `JWTEncryption` 加解密并未包含在 `Furion` 框架中，需要安装 `Furion` 框架提供的 `Furion.Extras.Authentication.JwtBearer` 拓展包

* `RSA` 加密

````c#
// 测试 RSA 加密
var (publicKey, privateKey) = RSAEncryption.GenerateSecretKey(2048);  //生成 RSA 秘钥 秘钥大小必须为 2048 到 16384，并且是 8 的倍数
var basestring = RSAEncryption.Encrypt("百小僧", publicKey);  // 加密
var str2 = RSAEncryption.Decrypt(basestring, privateKey); // 解密
return (basestring, str2);
````

> ##### 关于 `RSA` 签名和校验
>
> `Furion` 框架底层不内置 `RSA` 前面和检验功能，如需添加该功能可查阅开发者提交的代码：[查看 RSA 签名和校验](https://gitee.com/dotnetchina/Furion/pulls/349)



**另外提供字符串的拓展方式来实现加解密**

```
Furion` 框架也提供了字符串拓展方式进行 `MD5加密、AES/DESC加解密、RSA加解密
```

```c#
using Furion.DataEncryption.Extensions;

// MD5 加密
var s = "Furion".ToMD5Encrypt();
var b = "Furion".ToMD5Compare(s);   // 比较

// AES加解密
var s = "Furion".ToAESEncrypt("sfdsfdsfdsfdsfdsfdsfdsfdsfdfdsfdsfdfdfdfd");
var str = s.ToAESDecrypt("sfdsfdsfdsfdsfdsfdsfdsfdsfdfdsfdsfdfdfdfd");

// DESC 加解密
var s = "Furion".ToDESCEncrypt("sfdsfdsfdsfdsfdsfdsfdsfdsfdfdsfdsfdfdfdfd");
var str = s.ToDESCDecrypt("sfdsfdsfdsfdsfdsfdsfdsfdsfdfdsfdsfdfdfdfd");

// PBKDF2 加密（`Furion v2.12 +` 版本已移除！！！！！！！！）
var s = "Furion".ToPBKDF2Encrypt();
var b = "Furion".ToPBKDF2Compare(s);   // 比较

// RSA 加解密
var (publicKey, privateKey) = RSAEncryption.GenerateSecretKey(2048);  //生成 RSA 秘钥 秘钥大小必须为 2048 到 16384，并且是 8 的倍数
var s= "Furion".ToRSAEncrpyt(publicKey);  // 加密
var str=s.ToRSADecrypt(privateKey);  // 解密
```



### [JSON序列化](https://dotnetchina.gitee.io/furion/docs/json-serialization)

* **什么是JSON**

> JSON (JavaScript Object Notation, JS 对象标记) 是一种轻量级的数据交换格式。它基于 ECMAScript (w3c 制定的 js 规范)的一个子集，采用完全独立于编程语言的文本格式来存储和表示数据。简洁和清晰的层次结构使得 JSON 成为理想的数据交换语言。 易于人阅读和编写，同时也易于机器解析和生成，并有效地提升网络传输效率。

​	简单来说，JSON，是一种数据格式，在与后端的数据交互中有较为广泛的应用

* **关于序列化库**

目前在 C# 语言中有两个主流的 `JSON` 序列化操作库：

- `System.Text.Json`：`.NET Core` 内置 `JSON` 序列化库，也是 `Furion` 框架默认实现
- `Newtonsoft.Json`：目前使用人数最多的 `JSON` 序列化库，需要安装 `Microsoft.AspNetCore.Mvc.NewtonsoftJson` 拓展包

由于目前 `System.Text.Json` 相比 `Newtonsoft.Json` 功能和稳定性有许多不足之处，比如循环引用问题在 `System.Text.Json` 无解。但在 `.NET 6` 之后得到解决。

`Furion` 框架为了解决多种序列化工具配置和用法上的差异问题，抽象出了 `IJsonSerializerProvider` 接口

* **`IJsonSerializerProvider`接口**

  `Furion` 框架提供了 `IJsonSerializerProvider` 接口规范，同时**要求实现该接口的实体都必须采用单例模式**，该接口定义代码如下

  ```c#
  namespace Furion.JsonSerialization
  {
      /// <summary>
      /// Json 序列化提供器
      /// </summary>
      public interface IJsonSerializerProvider
      {
          /// <summary>
          /// 序列化对象
          /// </summary>
          /// <param name="value"></param>
          /// <param name="jsonSerializerOptions"></param>
          /// <returns></returns>
          string Serialize(object value, object jsonSerializerOptions = default);
  
          /// <summary>
          /// 反序列化字符串
          /// </summary>
          /// <typeparam name="T"></typeparam>
          /// <param name="json"></param>
          /// <param name="jsonSerializerOptions"></param>
          /// <returns></returns>
          T Deserialize<T>(string json, object jsonSerializerOptions = default);
  
          /// <summary>
          /// 返回读取全局配置的 JSON 选项
          /// </summary>
          /// <returns></returns>
          object GetSerializerOptions();
      }
  }
  ```

  > ##### 默认实现
  >
  > `SystemTextJsonSerializerProvider` 类是 `IJsonSerializerProvider` 接口的默认实现，在应用启动时已默认注册

  

* **如何使用**

   1. 获取序列化对象

      `Furion` 框架提供了两种方式获取 `IJsonSerializerProvider` 实例：

      - 构造函数注入 `IJsonSerializerProvider`
      - 静态类 `JSON.GetJsonSerializer()` 方式，**查看 [JSON 静态类](https://dotnetchina.gitee.io/furion/docs/global/json)**

      如：

      ```c#
      using Furion.DynamicApiController;
      using Furion.JsonSerialization;
      
      namespace Furion.Application
      {
          public class JsonDemo : IDynamicApiController
          {
              private readonly IJsonSerializerProvider _jsonSerializer;
              private readonly IJsonSerializerProvider _jsonSerializer2;
              public JsonDemo(IJsonSerializerProvider jsonSerializer)
              {
                  _jsonSerializer = jsonSerializer;
                  _jsonSerializer2 = JSON.GetJsonSerializer();
              }
          }
      }
      ```

      

   2. 序列化对象

      ```c#
      public string GetText()
      {
          return _jsonSerializer.Serialize(new
          {
              Id = 1,
              Name = "Furion"
          });
      }
      ```

      

   3. 反序列化字符串

      ```c#
      public object GetObject()
      {
          var json = "{\"Id\":1,\"Name\":\"Furion\"}";
          var obj = _jsonSerializer.Deserialize<object>(json);
          return obj;
      }
      ```

      > ​	`System.Text.Json` 默认反序列化大小写敏感，也就是不完全匹配的属性名称不会自动赋值。这时候我们可以全局配置或单独配置。
      >
      > - 全局配置
      >
      >   ```c#
      >   services.AddControllersWithViews()
      >           .AddJsonOptions(options => {
      >               options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
      >           });
      >   ```
      >
      >   * 单独配置
      >
      >   ```c#
      >   var obj = _jsonSerializer.Deserialize<object>(json, new JsonSerializerOptions
      >      {
      >          PropertyNameCaseInsensitive = true
      >      });
      >   ```
      >
      >   

   4. 序列化更多配置

      `Furion` 框架不推荐一个框架中有多种序列化实现类，也就是说使用 `System.Text.Json` 就不要使用 `Newtonsoft.Json`，反之亦然。

      如需配置更多选项，只需创建 `JsonSerializerOptions` 配置对象即可，如

      ```c#
      var json =  _jsonSerializer.Serialize(new
                  {
                      Id = 1,
                      Name = "Furion"
                  }, new JsonSerializerOptions {
                      WriteIndented = true
                  });
      ```

      

* **高级用法**

  1. **自定义序列化提供器**

     * 安装  `Microsoft.AspNetCore.Mvc.NewtonsoftJson` 拓展，并在 `Startup.cs` 中注册

     ```c#
     services.AddControllersWithViews()
             .AddNewtonsoftJson();
     ```

     * 实现`IJsonSerializerProvider` 提供器

     ```c#
     using Furion.DependencyInjection;
     using Furion.JsonSerialization;
     using Newtonsoft.Json;
     
     namespace Furion.Core
     {
         /// <summary>
         /// Newtonsoft.Json 实现
         /// </summary>
         public class NewtonsoftJsonSerializerProvider : IJsonSerializerProvider, ISingleton
         {
             /// <summary>
             /// 序列化对象
             /// </summary>
             /// <param name="value"></param>
             /// <param name="jsonSerializerOptions"></param>
             /// <returns></returns>
             public string Serialize(object value, object jsonSerializerOptions = null)
             {
                 return JsonConvert.SerializeObject(value, (jsonSerializerOptions ?? GetSerializerOptions()) as JsonSerializerSettings);
             }
     
             /// <summary>
             /// 反序列化字符串
             /// </summary>
             /// <typeparam name="T"></typeparam>
             /// <param name="json"></param>
             /// <param name="jsonSerializerOptions"></param>
             /// <returns></returns>
             public T Deserialize<T>(string json, object jsonSerializerOptions = null)
             {
                 return JsonConvert.DeserializeObject<T>(json, (jsonSerializerOptions ?? GetSerializerOptions()) as JsonSerializerSettings);
             }
     
             /// <summary>
             /// 返回读取全局配置的 JSON 选项
             /// </summary>
             /// <returns></returns>
             public object GetSerializerOptions()
             {
                 return App.GetOptions<MvcNewtonsoftJsonOptions>()?.SerializerSettings;
             }
         }
     }
     ```

     

  2. **序列化属性名大写（属性原样输出）**

     * `System.Text.Json` 方式

     ```c#
     services.AddControllersWithViews()
             .AddJsonOptions(options => {
                 options.JsonSerializerOptions.PropertyNamingPolicy = null;
                 // options.JsonSerializerOptions.DictionaryKeyPolicy = null;    // 配置 Dictionary 类型序列化输出
             });
     ```

     * `Newtonsoft.Json` 方式

     ```c#
     services.AddControllersWithViews()
             .AddNewtonsoftJson(options =>
             {
                 options.SerializerSettings.ContractResolver = new DefaultContractResolver();
             });
     ```

     > ##### 特别注意
     >
     > 采用 `Newtonsoft.Json` 方式接口返回值能够正常输出，但是 `Swagger` 界面中的 `Example Values` 依然显示小写字母开头的属性，这时只需要再添加 `System.Text.Json` 配置即可，如：
     >
     > ```c#
     > .AddJsonOptions(options => {
     >             options.JsonSerializerOptions.PropertyNamingPolicy = null;
     >         });
     > ```
     >
     > Copy
     >
     > 主要原因是 `Swagger` 拓展包底层依赖了 `System.Text.Json`

3. **时间格式化**

   * `System.Text.Json` 方式

   ```c#
   services.AddControllersWithViews()
           .AddJsonOptions(options =>
           {
               options.JsonSerializerOptions.Converters.AddDateFormatString("yyyy-MM-dd HH:mm:ss");
           });
   ```

   > ##### 小提示
   >
   > 如果使用使用了 `DateTimeOffset` 类型，那么可以设置 `.AddDateFormatString("yyyy-MM-dd HH:mm:ss", true)` 第二个参数为 `true`，自动转换成本地时间。
   >
   > 如果使用了 `Mysql` 数据库，且使用了 `Pomelo.EntityFrameworkCore.MySql` 包，那么会出现时区问题，比如少 8 小时，可以尝试配置第二个参数为 `true`。

需引用 `System.Text.Json` 命名空间。

* `Newtonsoft.Json` 方式

```c#
services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.DateFormatString = "yyyy-MM-dd HH:mm:ss";
        });
```

4. **忽略循环引用**

- `System.Text.Json` 方式

```c#
services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        });
```

> ##### 特别说明
>
> 在 `.NET 5` 中，`System.Text.Json` 并不支持处理循环引用问题，以上的解决方案仅限用于 `.NET 6 Preview 2+`。😂

需引用 `System.Text.Json` 命名空间。

- `Newtonsoft.Json` 方式

```c#
services.AddControllersWithViews()
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
        });
```

5. **包含成员字段序列化**

- `System.Text.Json` 方式

```c#
services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.IncludeFields = true;
        });
```



需引用 `System.Text.Json` 命名空间。

- `Newtonsoft.Json` 方式

无需配置。

6. **允许尾随逗号**

- `System.Text.Json` 方式

```c#
services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.AllowTrailingCommas = true;
        });
```

需引用 `System.Text.Json` 命名空间。

- `Newtonsoft.Json` 方式

无需配置。

7. **允许注释**

- `System.Text.Json` 方式

```c#
services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.ReadCommentHandling = JsonCommentHandling.Skip;
        });
```

需引用 `System.Text.Json` 命名空间。

- `Newtonsoft.Json` 方式

无需配置。

8. **处理乱码问题**

- `System.Text.Json` 方式

```c#
services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
        });
```

需引用 `System.Text.Json` 命名空间。

- `Newtonsoft.Json` 方式

无需配置。

9. **不区分大小写**

- `System.Text.Json` 方式

```c#
services.AddControllersWithViews()
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        });
```

需引用 `System.Text.Json` 命名空间。

- `Newtonsoft.Json` 方式

### [缓存](https://dotnetchina.gitee.io/furion/docs/cache)

* **内存缓存**

内存缓存是最常用的缓存方式，具有存取快，效率高特点。

内存缓存通过注入 `IMemoryCache` 方式注入即可。

> ##### 备注
>
> 在 `Furion` 框架中，内存缓存服务已经默认注册，无需手动注册。

 	1. **基本使用**

​	    缓存当前时间

​	

```c#
using Furion.DynamicApiController;
using Microsoft.Extensions.Caching.Memory;
using System;

namespace Furion.Application
{
    public class CacheServices : IDynamicApiController
    {
        private const string _timeCacheKey = "cache_time";

        private readonly IMemoryCache _memoryCache;

        public CacheServices(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }

        [ApiDescriptionSettings(KeepName = true)]
        public DateTimeOffset GetOrCreate()
        {
            return _memoryCache.GetOrCreate(_timeCacheKey, entry =>
            {
                return DateTimeOffset.UtcNow;
            });
        }
    }
}
```



2. **设置缓存选项**

内存缓存支持设置缓存时间、缓存大小、及绝对缓存过期时间等

```c#
_memoryCache.GetOrCreate(_timeCacheKey, entry =>
{
    entry.SlidingExpiration = TimeSpan.FromSeconds(3);  // 滑动缓存时间
    return DateTimeOffset.UtcNow;
});

await _memoryCache.GetOrCreateAsync(_timeCacheKey, async entry =>
{
    // 这里可以使用异步~~
});
```

>
>
>##### 关于缓存时间
>
>只有具有可调过期的缓存项集存在过时的风险。 如果访问的时间比滑动过期时间间隔更频繁，则该项将永不过期。
>
>将弹性过期与绝对过期组合在一起，以保证项目在其绝对过期时间通过后过期。 绝对过期会将项的上限设置为可缓存项的时间，同时仍允许项在可调整过期时间间隔内未请求时提前过期。
>
>如果同时指定了绝对过期和可调过期时间，则过期时间以逻辑方式运算。 如果滑动过期时间间隔 或 绝对过期时间通过，则从缓存中逐出该项。
>
>如：
>
>```c#
>_memoryCache.GetOrCreate(_timeCacheKey, entry =>
>{
>    entry.SetSlidingExpiration(TimeSpan.FromSeconds(3));
>    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(20);
>    return DateTime.Now;
>});
>```
>
>前面的代码保证数据的缓存时间不超过绝对时间。

3. **手动设置缓存选项**

除了上面的 `Func<MemoryCacheEntryOptions, object>` 方式设置缓存选项，我们可以手动创建并设置，如：

```c#
var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromSeconds(3));

_memoryCache.Set(_timeCacheKey, DateTimeOffset.UtcNow, cacheEntryOptions);
```

4. **缓存依赖关系**

下面的示例演示如何在依赖条目过期时使缓存条目过期。 `CancellationChangeToken` 添加到缓存的项。 当 `Cancel` 在上调用时 `CancellationTokenSource` ，将逐出两个缓存项。

```c#
public IActionResult CreateDependentEntries()
{
    var cts = new CancellationTokenSource();
    _cache.Set(CacheKeys.DependentCTS, cts);

    using (var entry = _cache.CreateEntry(CacheKeys.Parent))
    {
        // expire this entry if the dependant entry expires.
        entry.Value = DateTime.Now;
        entry.RegisterPostEvictionCallback(DependentEvictionCallback, this);

        _cache.Set(CacheKeys.Child,
            DateTime.Now,
            new CancellationChangeToken(cts.Token));
    }

    return RedirectToAction("GetDependentEntries");
}

public IActionResult GetDependentEntries()
{
    return View("Dependent", new DependentViewModel
    {
        ParentCachedTime = _cache.Get<DateTime?>(CacheKeys.Parent),
        ChildCachedTime = _cache.Get<DateTime?>(CacheKeys.Child),
        Message = _cache.Get<string>(CacheKeys.DependentMessage)
    });
}

public IActionResult RemoveChildEntry()
{
    _cache.Get<CancellationTokenSource>(CacheKeys.DependentCTS).Cancel();
    return RedirectToAction("GetDependentEntries");
}

private static void DependentEvictionCallback(object key, object value,
    EvictionReason reason, object state)
{
    var message = $"Parent entry was evicted. Reason: {reason}.";
    ((HomeController)state)._cache.Set(CacheKeys.DependentMessage, message);
}
```

使用 `CancellationTokenSource` 允许将多个缓存条目作为一个组逐出。 `using` 在上面的代码中，在块中创建的缓存条目 `using` 将继承触发器和过期设置。

>
>
>##### 了解更多
>
>想了解更多 `内存中的缓存` 知识可查阅 [ASP.NET Core - 内存缓存](https://docs.microsoft.com/zh-cn/aspnet/core/performance/caching/memory?view=aspnetcore-5.0) 章节。



* **分布式缓存**

分布式缓存是由多个应用服务器共享的缓存，通常作为外部服务在访问它的应用服务器上维护。 分布式缓存可以提高 `ASP.NET Core` 应用程序的性能和可伸缩性，尤其是在应用程序由云服务或服务器场托管时。

与其他缓存方案相比，分布式缓存具有多项优势，其中缓存的数据存储在单个应用服务器上。

当分布式缓存数据时，数据将：

- (一致性) 跨多个 服务器的请求
- 存活在服务器重启和应用部署之间
- 不使用本地内存

分布式缓存配置是特定于实现的。 本文介绍如何配置 `SQL Server` 和 `Redis` 分布式缓存。 第三方实现也可用，例如 GitHub 上的 [NCache](https://github.com/Alachisoft/NCache) (NCache) 。

**无论选择哪种实现，应用都会使用接口与缓存交互 `IDistributedCache` 。**

1. 使用条件

- 若要使用 `SQL Server` 分布式缓存，则添加 `Microsoft.Extensions.Caching.SqlServer` 包
- 若要使用 `Redis` 分布式缓存，则添加 `Microsoft.Extensions.Caching.StackExchangeRedis` 包
- 若要使用 `NCache` 分布式缓存，则添加 `NCache.Microsoft.Extensions.Caching.OpenSource` 包

2. ### `IDistributedCache`

`IDistributedCache` 接口提供以下方法来处理分布式缓存实现中的项：

- `Get/GetAsync`：接受字符串键，并检索缓存项作为 `byte[]` 数组（如果在缓存中找到）
- `Set/SetAsync`：使用字符串键将项 (作为 `byte[]` 数组) 添加到缓存中
- `Refresh/RefreshAsync` ：根据项的键刷新缓存中的项，重置其滑动过期超时（如果有）
- `Remove/RemoveAsync`：根据缓存项的字符串键删除缓存项

3. 分布式内存缓存

分布式内存缓存（`AddDistributedMemoryCache`）是一个框架提供的实现 `IDistributedCache` ，它将项存储在内存中。 **分布式内存缓存不是实际的分布式缓存，缓存项由应用程序实例存储在运行应用程序的服务器上。**

分布式内存缓存优点：

- 用于开发和测试方案。
- 在生产环境中使用单一服务器并且内存消耗不是问题。 实现分布式内存缓存会抽象化缓存的数据存储。 如果需要多个节点或容错，可以在将来实现真正的分布式缓存解决方案。

>##### 备注
>
>在 `Furion` 框架中，分布式内存缓存服务已经默认注册，无需手动调用 `services.AddDistributedMemoryCache();` 注册。

4. **分布式`SQL Server`缓存**

分布式 `SQL Server` 缓存实现 (`AddDistributedSqlServerCache`) 允许分布式缓存使用 `SQL Server` 数据库作为其后备存储。

若要在 `SQL Server` 实例中创建 `SQL Server` 缓存的项表，可以使用 `sql-cache` 工具。 该工具将创建一个表，其中包含指定的名称和架构。

通过运行命令 `sql-cache create` 创建一个表，提供 `SQL Server` 实例 (Data Source) 、数据库 (Initial Catalog) 、架构 (例如) dbo 和表名称。例如 `TestCache`：

```c#
dotnet sql-cache create "Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=DistCache;Integrated Security=True;" dbo TestCache
```

创建成功后，在 `Startup.cs` 中注册即可：

```c#
services.AddDistributedSqlServerCache(options =>
{
    options.ConnectionString =
        _config["DistCache_ConnectionString"];
    options.SchemaName = "dbo";
    options.TableName = "TestCache";
});
```

5. **分布式`Redis`缓存**

`Redis` 是内存中数据存储的开源数据存储，通常用作分布式缓存。在使用时通过 `services.AddStackExchangeRedisCache()` 中注册即可。

这里不细讲 `Redis` 相关内容，后续章节会使用基本例子演示。

`Redis` 基本配置：

```
services.AddStackExchangeRedisCache(options =>
{
    // 连接字符串，这里也可以读取配置文件
    options.Configuration = "192.168.111.134,password=aW1HAyupRKmiZn3Q";
    // 键名前缀
    options.InstanceName = "furion_";
});
```

6. 分布式`NCache`缓存

`NCache` 是在 `.NET` 和 `.Net Core` 中以本机方式开发的开源内存中分布式缓存。 `NCache` 在本地工作并配置为分布式缓存群集，适用于在 `Azure` 或其他托管平台上运行的 `ASP.NET Core` 应用。 若要在本地计算机上安装和配置 `NCache`，请参阅 [适用于 Windows 的 NCache 入门指南](https://www.alachisoft.com/resources/docs/ncache-oss/getting-started-guide-windows/)。

`NCache` 基本配置：

- 安装 `Alachisoft.NCache.OpenSource.SDK` 包
- 在 [ncconf](https://www.alachisoft.com/resources/docs/ncache-oss/admin-guide/client-config.html) 中配置缓存群集
- 注册 `NCache` 服务

```c#
services.AddNCacheDistributedCache(configuration =>
{
    configuration.CacheName = "demoClusteredCache";
    configuration.EnableLogs = true;
    configuration.ExceptionsEnabled = true;
});
```



#### 分布式缓存使用

若要使用 `IDistributedCache` 接口，请 `IDistributedCache` 通过构造函数依赖关系注入。

```c#
public class IndexModel : PageModel
{
    private readonly IDistributedCache _cache;

    public IndexModel(IDistributedCache cache)
    {
        _cache = cache;
    }

    public string CachedTimeUTC { get; set; }

    public async Task OnGetAsync()
    {
        CachedTimeUTC = "Cached Time Expired";
        // 获取分布式缓存
        var encodedCachedTimeUTC = await _cache.GetAsync("cachedTimeUTC");

        if (encodedCachedTimeUTC != null)
        {
            CachedTimeUTC = Encoding.UTF8.GetString(encodedCachedTimeUTC);
        }
    }

    public async Task<IActionResult> OnPostResetCachedTime()
    {
        var currentTimeUTC = DateTime.UtcNow.ToString();
        byte[] encodedCurrentTimeUTC = Encoding.UTF8.GetBytes(currentTimeUTC);

        // 设置分布式缓存
        var options = new DistributedCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(20));

        await _cache.SetAsync("cachedTimeUTC", encodedCurrentTimeUTC, options);

        return RedirectToPage();
    }
}
```

* **分布式缓存建议**

确定 `IDistributedCache` 最适合你的应用的实现时，请考虑以下事项：

- 现有基础结构
- 性能要求
- 成本
- 团队经验

缓存解决方案通常依赖于内存中的存储以快速检索缓存的数据，但是，内存是有限的资源，并且很昂贵。 仅将常用数据存储在缓存中。

通常，**`Redis` 缓存提供比 `SQL Server` 缓存更高的吞吐量和更低的延迟。** 但是，通常需要进行基准测试来确定缓存策略的性能特征。

当 `SQL Server` 用作分布式缓存后备存储时，对缓存使用同一数据库，并且应用的普通数据存储和检索会对这两种情况的性能产生负面影响。 建议使用分布式缓存后备存储的专用 `SQL Server` 实例。



### [即时通讯](https://dotnetchina.gitee.io/furion/docs/signalr)

#### **1. 什么是即时通讯**

即时通讯（Instant messaging，简称 IM）通常是指互联网上用以进行实时通讯的系统，允许两人或多人使用网络即时的传递文字信息、文档、语音与视频交流。

即时通讯不同于 e-mail 在于它的交谈是实时的。大部分的即时通讯服务提供了状态信息的特性 ── 显示联络人名单，联络人是否在线上与能否与联络人交谈。

在互联网上目前使用较广的即时通讯服务包括 Windows Live Messenger、AOL Instant Messenger、skype、Yahoo! Messenger、NET Messenger Service、Jabber、ICQ 与 QQ 等。

#### **2. 即时通讯引用场景**

即时通讯应用场景非常广泛，需要实时交互消息的都需要。如：

- 聊天工具：QQ、WeChat、在线客服等
- 手游网游：王者荣耀、魔兽等
- 网络直播：腾讯课堂、抖音直播等
- 订单推送：美团、餐饮下单系统等
- 协同办公：公司内部文件分享、工作安排、在线会议等。

以上只是列举了比较常用的应用场景，但即时通讯的作用远不止于此。

文档紧急编写中，可以先看官方文档：https://docs.microsoft.com/zh-cn/aspnet/core/signalr/introduction?view=aspnetcore-5.0

#### 3. 关于`SignalR`

即时通讯技术实现是复杂且过于底层化，所以微软为了简化即时通讯应用程序，开发出了一个强大且简易使用的通信库：`SignalR`，通过该库我们可以轻松实现类似 QQ、微信这类 IM 聊天工具，也能快速实现消息推送、订单推送这样的系统。

ASP.NET Core SignalR 是一种开放源代码库，可简化将实时 web 功能添加到应用程序的功能。 实时 web 功能使服务器端代码可以立即将内容推送到客户端。

适用于 SignalR ：

- 需要从服务器进行高频率更新的应用。 示例包括游戏、社交网络、投票、拍卖、地图和 GPS 应用。
- 仪表板和监视应用。 示例包括公司仪表板、即时销售更新或旅行警报。
- 协作应用。 协作应用的示例包括白板应用和团队会议软件。
- 需要通知的应用。 社交网络、电子邮件、聊天、游戏、旅行警报和很多其他应用都需使用通知。

目前 `SignalR` 已经内置在 `.NET 5 SDK` 中。同时 `SignalR` 支持 `Web、App、Console、Desktop` 等多个应用平台。

* 注册`SignalR`服务

```c#
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Furion.Web.Core
{
    public sealed class Startup : AppStartup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            // 其他代码...

            // 添加即时通讯
            services.AddSignalR();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            // 其他代码...

            app.UseEndpoints(endpoints =>
            {
                // 注册集线器
                endpoints.MapHubs();

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
```

####  4. `SignalR`长连接和集线器

`SignalR` 包含两种用于在客户端和服务器之间进行通信的模型：`持久性连接`和 `集线器` 中心

* ### 持久性连接

  连接表示用于发送单接收方、分组或广播消息的简单终结点。 `持久性连接` (在 .NET 代码中由 PersistentConnection 类表示) 使开发人员能够直接访问 `SignalR` 公开的低级别通信协议。 使用基于连接的 Api （如 Windows Communication Foundation）的开发人员将对使用连接通信模型非常熟悉

* ### 集线器

集线器是一种基于连接 API 构建的更高级别管道，**它允许客户端和服务器直接调用方法**。 `SignalR` 就像魔术一样处理跨机器边界的调度，使客户端能够像本地方法一样轻松地调用服务器上的方法，反之亦然。 如果开发人员已使用远程调用 （如 .NET 远程处理），则将对使用中心通信模型非常熟悉。 使用集线器还可以将强类型参数传递给方法，从而启用模型绑定

> ##### 小知识
>
> 想了解更多关于 `持久性连接` 和 `集线器中心` 可查阅 [SignalR 官方文档](https://docs.microsoft.com/zh-cn/aspnet/signalr/overview/getting-started/introduction-to-signalr#connections-and-hubs)

####  5. 使用`hub`集线器

 * **定义：支持两种定义**

    * `Hub`方式

      ```c#
      using Furion.InstantMessaging;
      using Microsoft.AspNetCore.SignalR;
      
      namespace Furion.Core
      {
          /// <summary>
          /// 聊天集线器
          /// </summary>
          public class ChatHub : Hub
          {
              // 定义一个方法供客户端调用
              public Task SendMessage(string user, string message)
              {
                  // 触发客户端定义监听的方法
                  return Clients.All.SendAsync("ReceiveMessage", user, message);
              }
          }
      }
      ```

      

    * `Hub<TStrongType>`类型方式

      ```c#
      public interface IChatClient
      {
          Task ReceiveMessage(string user, string message);
      }
      ```

      ```c#
      public class StronglyTypedChatHub : Hub<IChatClient>
      {
          // 定义一个方法供客户端调用
          public async Task SendMessage(string user, string message)
          {
              // 触发客户端定义监听的方法
              await Clients.All.ReceiveMessage(user, message);
          }
      }
      ```

      通过使用 `Hub<IChatClient>` 可以对客户端方法进行编译时检查。 这可以防止由于使用神奇字符串而导致的问题，因为 `Hub<T>` 只能提供对在接口中定义的方法的访问

* **`[MapHub]`配置连接地址**

  在 `SignalR` 库中要求每一个公开的集线器都需要配置客户端连接地址，所以，`Furion` 框架提供了更加 `[MapHub]` 配置，如：

  ```c#
  using Furion.InstantMessaging;
  using Microsoft.AspNetCore.SignalR;
  using System;
  using System.Threading.Tasks;
  
  namespace Furion.Core
  {
      /// <summary>
      /// 聊天集线器
      /// </summary>
      [MapHub("/hubs/chathub")]
      public class ChatHub : Hub
      {
          // ...
      }
  }
  ```

  > ##### `SIGNALR` 原生配置方式
  >
  > 在 `Furion` 中推荐使用 `[MapHub]` 方式配置集线器客户端连接地址，当然也可以使用 `SignalR` 提供的方式，如在 `Startup.cs` 配置：
  >
  > ```c#
  > public sealed class Startup : AppStartup
  > {
  >    // 其他代码
  >     public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
  >     {
  >         // 其他代码...
  >         app.UseEndpoints(endpoints =>
  >         {
  >             // 注册集线器
  >             endpoints.MapHub<ChatHub>("/hubs/chathub");
  >         });
  >     }
  > }
  > ```
  >
  > 

  

* `Hub`注册更多配置

  有些时候，我们需要注册 `Hub` 时配置更多参数，比如权限、跨域等，这时只需要在 `Hub` 派生类中编写以下静态方法即可：

  ```c#
  using Furion.InstantMessaging;
  using Microsoft.AspNetCore.SignalR;
  using System;
  using System.Threading.Tasks;
  
  namespace Furion.Core
  {
      [MapHub("/hubs/chathub")]
      public class ChatHub : Hub
      {
          // 其他代码
  
          public static void HttpConnectionDispatcherOptionsSettings(HttpConnectionDispatcherOptions options)
          {
              // 配置
          }
  
          public static void HubEndpointConventionBuilderSettings(HubEndpointConventionBuilder Builder)
          {
              // 配置
          }
      }
  }
  ```

  

以上配置等价于 `SignalR` 在 `Startup.cs` 中的配置：

```c#
app.UseEndpoints(endpoints =>
{
    var builder = endpoints.MapHub<ChatHub>("/hubs/chathub", options =>
       {
           // 配置
       });
});
```

#### 6. 服务端和客户端双工通信

触发所有客户端代码[#](https://dotnetchina.gitee.io/furion/docs/signalr#2471-触发所有客户端代码)

```cs
Clients.All.客户端方法(参数);
```

触发调用者客户端[#](https://dotnetchina.gitee.io/furion/docs/signalr#2472-触发调用者客户端)

```cs
Clients.Caller.客户端方法(参数);
```

 触发除了调用者以外的客户端[#](https://dotnetchina.gitee.io/furion/docs/signalr#2473-触发除了调用者以外的客户端)

```cs
Clients.Others.客户端方法(参数);
```

 触发特定用户客户端[#](https://dotnetchina.gitee.io/furion/docs/signalr#2474-触发特定用户客户端)

```cs
Clients.User("用户").客户端方法(参数);
```

 触发多个用户客户端[#](https://dotnetchina.gitee.io/furion/docs/signalr#2475-触发多个用户客户端)

```cs
Clients.Users("用户","用户2",...).客户端方法(参数);
```

 触发分组内客户端[#](https://dotnetchina.gitee.io/furion/docs/signalr#2476-触发分组内客户端)

```cs
Clients.Group("分组").客户端方法(参数);
```

触发多个分组客户端[#](https://dotnetchina.gitee.io/furion/docs/signalr#2477-触发多个分组客户端)

```cs
Clients.Groups("分组","分组2",...).客户端方法(参数);
```

 触发分组外的客户端[#](https://dotnetchina.gitee.io/furion/docs/signalr#2478-触发分组外的客户端)

```cs
Clients.GroupExcept("分组").客户端方法(参数);
```



### 事件总线

#### 1. 什么是事件总线

事件总线是对发布-订阅模式的一种实现。它是一种集中式事件处理机制，允许不同的组件之间进行彼此通信而又不需要相互依赖，达到一种解耦的目的。

我们来看看事件总线的处理流程：

![img](https://dotnetchina.gitee.io/furion/img/event1.png)



#### 2. `MessageCenter`消息中心

在 `Furion` 框架中，实现了一种轻量级的事件总线实现机制：`MessageCenter`（消息中心），`MessageCenter` 采用字符串消息机制进行广播， 可以在绝大多数中小型项目中发挥作用，缺点是消息处理是在主线程中完成并且消息不支持分布式存储。

另外，`MessageCenter` 支持单播、多播发布及多订阅。如图所示：

![img](https://dotnetchina.gitee.io/furion/img/event2.png)

#### 3. 使用

* 注册`轻量级事件总线服务`

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSimpleEventBus();
}
```

* 定义订阅处理程序

`MessageCenter` 是根据 `MesseageId` 消息 Id 来触发对应的处理程序的，所以我们需要定义 `订阅处理程序类`，该类需实现 `ISubscribeHandler` 接口，如：

```c#
public class UserChangeSubscribeHandler : ISubscribeHandler
{
    // 定义一条消息
    [SubscribeMessage("create:user")]
    public void CreateUser(string eventId, object payload)
    {
        Console.WriteLine("我是"+eventId);
    }

    // 多条消息共用同一个处理程序
    [SubscribeMessage("delete:user")]
    [SubscribeMessage("remove:user")]
    public void RemoveUser(string eventId, object payload)
    {
        Console.WriteLine("我是"+eventId);
    }

    // 支持异步
    [SubscribeMessage("delete:user")]
    public async Task SupportAsync(string eventId, object payload)
    {
        await MethodAsync();
    }
}
```

* 发布消息

  定义好订阅处理程序后，我们就可以在程序任何地方进行广播消息，事件总线会自动根据 `消息 Id` 触发对应的处理程序方法：

```c#
MessageCenter.Send("create:user", new User {}); // => 打印：我是create:user

MessageCenter.Send("delete:user", new User {}); // => 打印：我是delete:user
MessageCenter.Send("remove:user", new User {}); // => 打印：我是remove:user
```

* 直接订阅消息

在上面的例子中，我们需要创建 `ISubscribeHandler` 的派生类进行相关配置才能实现订阅处理。

在某些特殊情况下，可能只需要订阅一次即可。所以，在 `Furion` 框架中，为了更简便的订阅消息，也提供了 `MessageCenter.Subscribe<T>` 静态方法，如：

```c#
MessageCenter.Subscribe<User>("create:user", (i,p) => {
    // do something。。。
});
```

* 同步方式执行

  默认情况下，事件总线总是采用新线程方式执行，但是我们可以配置为同步方式，如：

```c#
MessageCenter.Send("create:user", isSync: true);
```

* 关于依赖注入

在 `Furion` 框架中，事件总线是不支持构造函数注入的，而且构造函数也只会执行一次。所以需要用到服务，应该通过静态类解析，`App.GetService<xx>()` 或 `Db.GetRepository<XX>()`。

```c#
public class UserChangeSubscribeHandler : ISubscribeHandler
{
    public UserChangeSubscribeHandler()
    {
        // 不支持这里解析服务！！！！！！！！！！！
    }

    // 定义一条消息
    [SubscribeMessage("create:user")]
    public void CreateUser(string eventId, object payload)
    {
        // 创建一个作用域，对象使用完成自动释放
        Scoped.Create((_, scope) =>
        {
            var services = scope.ServiceProvider;

            var repository = Db.GetRepository<Person>(services);    // services 传递进去
            var someService = App.GetService<ISomeService>(services);   // services 传递进去
            var otherService = services.GetService<IOtherService>();    // 直接用 services 解析
        });
    }
}
```

> ##### 关于 `APP.GETSERVICE<TSERVICE>()` 解析服务
>
> 在高频定时任务中调用`App.GetService(TService)`，可能会出现内存无法回收的情况，建议始终使用`scope.ServiceProvider.GetService(TService)`来获取`TService`

> ##### 数据库操作注意
>
> 如果作用域中对**数据库有任何变更操作**，需手动调用 `SaveChanges` 或带 `Now` 结尾的方法。也可以使用 `Scoped.CreateUow(handler)` 代替。

> ##### 关于依赖注入
>
> `ISubscribeHandler` 接口主要是用来查找定义事件对象的，也就是它的实现类并未提供依赖注入功能，所以在实现类并不支持构造函数注入依赖项。



### [定时任务/后台任务](https://dotnetchina.gitee.io/furion/docs/job)

#### 1. 关于定时任务

顾名思义，定时任务就是在特定的时间或符合某种时间规律执行的任务。通常定时任务有四种时间调度方式：

- `缓隔时间` 方式：延迟多少时间后调配任务，这种方式任务只会被调用一次。
- `间隔时间` 方式：每隔一段固定时间调配任务，无间断调用任务。
- `Cron 表达式` 方法：通过 `Cron` 表达式计算下一次执行时间进行调配任务，可以配置特定时间范围内执行，也可以无间断执行。
- `自定义下次执行时间`：可以通过各种逻辑运算返回下一次执行时间

> ##### IIS 部署说明
>
> 由于 IIS 有回收的机制，所以定时任务应该采用独立部署，不然经常出现不能触发的情况。查看【[Worker Service 章节](https://dotnetchina.gitee.io/furion/docs/process-service)】

#### 2. 如何实现

`Furion` 框架提供了两种方式实现定时任务：

- `SpareTime` 静态类：`SpareTime` 静态类提供 `SpareTime.Do([options])` 方式调用。
- `ISpareTimeWorker` 依赖方式：通过自定义类实现 `ISpareTimeWorker` 接口并编写一定规则的方法即可。**需要在 `Startup.cs` 中注册 `services.AddTaskScheduler()`**

#### 3. 缓隔方式使用

##### 	1. 特定时间后执行

三秒后执行

```c#
Console.WriteLine("当前时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

// timer 是定时器的对象，包含定时器相关信息
// count 表示执行次数，这里只有一次
SpareTime.DoOnce(3000, (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
});
```

##### 2. 配置任务信息

```c#
SpareTime.DoOnce(3000, (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
}, "jobName", "描述一下这个任务是干什么的");
```

`jobName` 标识任务的唯一标识，通过这个标识可以启动、暂停、销毁任务。

##### 3. 手动启动执行

默认情况下，任务初始化后就立即启动，等待符合的时间就执行，有些时候我们仅仅想初始化时间，不希望立即执行，只需要配置 `startNow` 即可

```c#
SpareTime.DoOnce(3000, (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
},"jobName", startNow: false);

// 手动启动执行
SpareTime.Start("jobName");
```

##### 4. 模拟后台执行

有些时候，我们只需要开启新线程去执行一个任务，比如发短信，发邮件，无需配置。

```
// 此方法无需主线程等待即可返回，可大大提高性能
SpareTime.DoIt(() => {
    // 这里发送短信，发送邮件或记录访问记录
});
```

还可以指定多长时间后触发，建议 `10-1000` 毫秒之间：

```c#
SpareTime.DoIt(() => {
    // 发送短信
}, 100);
```

##### 5. `ISpareTimeWorker `方式

```c#
public class JobWorker : ISpareTimeWorker
{
    /// <summary>
    /// 3s 后执行
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime(3000, "jobName", DoOnce = true, StartNow = true)]
    public void DoSomething(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    }

    /// <summary>
    /// 3s 后执行（支持异步）
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime(3000, "jobName", DoOnce = true, StartNow = true)]
    public async Task DoSomethingAsync(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        await Task.Completed;
    }
}
```

**需要在 `Startup.cs` 中注册 `services.AddTaskScheduler()`**



#### 间隔方式使用

##### 1. 每隔一段时间中行

```c#
// 每隔 1s 执行
SpareTime.Do(1000, (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
});
```

##### 2. 配置任务信息

```c#
SpareTime.Do(1000, (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
}, "jobName", "这是一个计时器任务");
```

##### 3. 手动启动执行

```c#
SpareTime.Do(1000, (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
}, "jobName", startNow:false);

SpareTime.Start("jobName");
```

##### 4. `ISpareTimeWorker`方式

```c#
public class JobWorker : ISpareTimeWorker
{
    /// <summary>
    /// 每隔 3s 执行
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime(3000, "jobName", StartNow = true)]
    public void DoSomething(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }
}
```

**需要在 `Startup.cs` 中注册 `services.AddTaskScheduler()`**

#### `Cron`表达式使用

Cron 表达式是一个字符串，字符串以 `5` 或 `6` 个空格隔开，分为 6 或 7 个域，每一个域代表一个含义，Cron 有如下两种语法格式：

（1） Seconds Minutes Hours DayofMonth Month DayofWeek Year

（2）Seconds Minutes Hours DayofMonth Month DayofWeek

Cron 从左到右（用空格隔开）：`秒` `分` `小时` `月份中的日期` `月份` `星期中的日期` `年份`

| 字段                   | 允许值                                      | 允许的特殊字符              |
| ---------------------- | ------------------------------------------- | --------------------------- |
| 秒（Seconds）          | `0~59` 的整数                               | `, - \* /` 四个字符         |
| 分（Minutes）          | `0~59` 的整数                               | `, - \* /` 四个字符         |
| 小时（Hours）          | `0~23` 的整数                               | `, - \* /` 四个字符         |
| 日期（DayofMonth）     | `1~31` 的整数（但是你需要考虑平闰月的天数） | `,- \* ? / L W C` 八个字符  |
| 月份（Month）          | `1~12` 的整数或者 `JAN-DEC`                 | `, - \* /` 四个字符         |
| 星期（DayofWeek）      | `1~7` 的整数或者 `SUN-SAT （1=SUN）`        | `, - \* ? / L C #` 八个字符 |
| 年(可选，留空)（Year） | `1970~2099`                                 | `, - \* /` 四个字符         |

每一个域都使用数字，但还可以出现如下特殊字符，它们的含义是：

（1）`_`：表示匹配该域的任意值。假如在 `Minutes` 域使用 `\_`, 即表示每分钟都会触发事件。

（2）`?`：只能用在 `DayofMonth` 和 `DayofWeek` 两个域。它也匹配域的任意值，但实际不会。因为 `DayofMonth` 和 `DayofWeek` 会相互影响。例如想在每月的 `20` 日触发调度，不管 `20` 日到底是星期几，则只能使用如下写法： `13 13 15 20 _ ?`, 其中最后一位只能用`？`，而不能使用_，如果使用*表示不管星期几都会触发，实际上并不是这样。

（3）`-`：表示范围。例如在 `Minutes` 域使用 `5-20`，表示从 `5` 分到 `20` 分钟每分钟触发一次

（4）`/`：表示起始时间开始触发，然后每隔固定时间触发一次。例如在 `Minutes` 域使用 `5/20`，则意味着 `5` 分钟触发一次，而 `25，45` 等分别触发一次.

（5）`,`：表示列出枚举值。例如：在 `Minutes` 域使用 `5,20`，则意味着在 `5` 和 `20` 分每分钟触发一次。

（6）`L`：表示最后，只能出现在 `DayofWeek` 和 `DayofMonth` 域。如果在 `DayofWeek` 域使用 `5L`,意味着在最后的一个星期四触发。

（7）`W`：表示有效工作日(周一到周五) 只能出现在 `DayofMonth` 域，系统将在离指定日期的最近的有效工作日触发事件。例如：在 `DayofMonth` 使用 `5W`，如果 `5` 日是星期六，则将在最近的工作日：星期五，即 `4` 日触发。如果 `5` 日是星期天，则在 `6` 日(周一)触发；如果 `5` 日在星期一到星期五中的一天，则就在 `5` 日触发。另外一点，`W` 的最近寻找不会跨过月份 。

（8）`LW`：这两个字符可以连用，表示在某个月最后一个工作日，即最后一个星期五。

（9）`#`：用于确定每个月第几个星期几，只能出现在 `DayofMonth` 域。例如在 `4#2`，表示某月的第二个星期三。

#####  [常见的`Cron`表达式](https://dotnetchina.gitee.io/furion/docs/job#2652-常见-cron-表达式)

| 表达式                   | 表达式代表含义                                             | 格式化                      |
| ------------------------ | ---------------------------------------------------------- | --------------------------- |
| `* * * * *`              | 每分钟                                                     | `CronFormat.Standard`       |
| `*/1 * * * *`            | 每分钟                                                     | `CronFormat.Standard`       |
| `0 0/1 * * * ?`          | 每分钟                                                     | `CronFormat.IncludeSeconds` |
| `0 0 * * * ?`            | 每小时                                                     | `CronFormat.IncludeSeconds` |
| `0 0 0/1 * * ?`          | 每小时                                                     | `CronFormat.IncludeSeconds` |
| `0 23 ? * MON-FRI`       | 晚上 11:00，周一至周五                                     | `CronFormat.Standard`       |
| `* * * * * *`            | 每秒                                                       | `CronFormat.IncludeSeconds` |
| `*/45 * * * * *`         | 每 45 秒                                                   | `CronFormat.IncludeSeconds` |
| `*/5 * * * *`            | 每 5 分钟                                                  | `CronFormat.Standard`       |
| `0 0/10 * * * ?`         | 每 10 分钟                                                 | `CronFormat.IncludeSeconds` |
| `0 */5 * * * *`          | 每 5 分钟                                                  | `CronFormat.IncludeSeconds` |
| `30 11 * * 1-5`          | 周一至周五上午 11:30                                       | `CronFormat.Standard`       |
| `30 11 * * *`            | 11:30                                                      | `CronFormat.Standard`       |
| `0-10 11 * * *`          | 上午 11:00 至 11:10 之间的每一分钟                         | `CronFormat.Standard`       |
| `* * * 3 *`              | 每分钟，只在 3 月份                                        | `CronFormat.Standard`       |
| `* * * 3,6 *`            | 每分钟，只在 3 月和 6 月                                   | `CronFormat.Standard`       |
| `30 14,16 * * *`         | 下午 02:30 分和 04:30 分                                   | `CronFormat.Standard`       |
| `30 6,14,16 * * *`       | 早上 06:30，下午 02:30 和 04:30                            | `CronFormat.Standard`       |
| `46 9 * * 1`             | 早上 09:46，只在星期一                                     | `CronFormat.Standard`       |
| `23 12 15 * *`           | 下午 12:23，在本月的第 15 天                               | `CronFormat.Standard`       |
| `23 12 * JAN *`          | 下午 12:23，只在 1 月份                                    | `CronFormat.Standard`       |
| `23 12 ? JAN *`          | 下午 12:23，只在 1 月份                                    | `CronFormat.Standard`       |
| `23 12 * JAN-FEB *`      | 下午 12:23，1 月至 2 月                                    | `CronFormat.Standard`       |
| `23 12 * JAN-MAR *`      | 下午 12:23，1 月至 3 月                                    | `CronFormat.Standard`       |
| `23 12 * * SUN`          | 下午 12:23，仅在星期天                                     | `CronFormat.Standard`       |
| `*/5 15 * * MON-FRI`     | 每 5 分钟，下午 0:00 至 03:59，周一至周五                  | `CronFormat.Standard`       |
| `* * * * MON#3`          | 每分钟，在月的第三个星期一                                 | `CronFormat.Standard`       |
| `* * * * 4L`             | 每一分钟，在本月的最后一天                                 | `CronFormat.Standard`       |
| `*/5 * L JAN *`          | 每月一次每月 5 分钟，只在 1 月份                           | `CronFormat.Standard`       |
| `30 02 14 * * *`         | 下午在 02:02:30                                            | `CronFormat.IncludeSeconds` |
| `5-10 * * * * *`         | 每分钟的 5-10 秒                                           | `CronFormat.IncludeSeconds` |
| `5-10 30-35 10-12 * * *` | 10:00 至 12:00 之间的每分钟 5-10 秒，每小时 30-35 分钟     | `CronFormat.IncludeSeconds` |
| `30 */5 * * * *`         | 每分钟的 30 秒，每五分钟                                   | `CronFormat.IncludeSeconds` |
| `0 30 10-13 ? * WED,FRI` | 每小时的 30 分钟，下午 10:00 至 01:00 之间，仅在周三和周五 | `CronFormat.IncludeSeconds` |
| `10 0/5 * * * ?`         | 每分钟的 10 秒，每 05 分钟                                 | `CronFormat.IncludeSeconds` |
| `0 0 6 1/1 * ?`          | 下午 06:00                                                 | `CronFormat.IncludeSeconds` |
| `0 5 0/1 * * ?`          | 一个小时的 05 分                                           | `CronFormat.IncludeSeconds` |
| `0 0 L * *`              | 每月最后一天上午 00：00                                    | `CronFormat.Standard`       |
| `0 0 L-1 * *`            | 每月最后一天的凌晨 00：00                                  | `CronFormat.Standard`       |
| `0 0 3W * *`             | 每月第 3 个工作日上午 00：00                               | `CronFormat.Standard`       |
| `0 0 LW * *`             | 在每月的最后一个工作日，上午 00：00                        | `CronFormat.Standard`       |
| `0 0 * * 2L`             | 本月最后一个星期二上午 00：00                              | `CronFormat.Standard`       |
| `0 0 * * 6#3`            | 每月第三个星期六上午 00：00                                | `CronFormat.Standard`       |
| `0 0 ? 1 MON#1`          | 1 月第一个星期一上午 00：00                                | `CronFormat.Standard`       |

##### 在线生成`Cron`表达式

https://cron.qqe2.com/

##### `Macro`标识符

为了方便定义 `Cron` 表达式，`Furion` 框架也提供了非常方便的占位符实现常用的时间格式：

| 占位符          | 对应表达式    | 占位符代表含义                 |
| --------------- | ------------- | ------------------------------ |
| `@every_second` | `* * * * * *` | 一秒钟跑一次                   |
| `@every_minute` | `* * * * *`   | 在分钟开始时每分钟运行一次     |
| `@hourly`       | `0 * * * *`   | 在小时开始时每小时运行一次     |
| `@daily`        | `0 0 * * *`   | 每天午夜运行一次               |
| `@midnight`     | `0 0 * * *`   | 每天午夜运行一次               |
| `@weekly`       | `0 0 * * 0`   | 周日上午午夜每周运行一次       |
| `@monthly`      | `0 0 1 * *`   | 每月在每月第一天的午夜运行一次 |
| `@yearly`       | `0 0 1 1 *`   | 每年 1 月 1 日午夜运行一次     |
| `@annually`     | `0 0 1 1 *`   | 每年 1 月 1 日午夜运行一次     |

```c#
// 每隔 1s 执行
SpareTime.Do("@every_second", (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
});
```



##### 使用`Cron`表达式

```c#
// 每隔 1s 执行
SpareTime.Do("* * * * * *", (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
}, cronFormat: CronFormat.IncludeSeconds);
```

>
>
>##### 关于 CRONFORMAT
>
>默认情况下，`Furion` 框架未启用对 `秒` 的支持，如需开启，则设置 `cronFormat: CronFormat.IncludeSeconds` 即可。默认值是 `cronFormat: CronFormat.Standard`

##### 配置任务信息

```c#
SpareTime.Do("* * * * *", (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
}, "cronName", "每分钟执行一次");
```

##### 手动启动执行

```c#
SpareTime.Do("* * * * *", (timer, count) => {
    Console.WriteLine("现在时间：" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
}, "cronName", "每分钟执行一次", startNow: false);

SpareTime.Start("cronName");
```

##### `ISpareTimeWorker`方式

```c#
public class JobWorker : ISpareTimeWorker
{
    /// <summary>
    /// 每分钟执行
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime("* * * * *", "jobName", StartNow = true)]
    public void DoSomething(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }

    /// <summary>
    /// 每分钟执行（支持异步）
    /// </summary>
    /// <param name="timer"></param>
    /// <param name="count"></param>
    [SpareTime("* * * * *", "jobName", StartNow = true)]
    public async Task DoSomethingAsync(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
        await Task.Completed;
    }
}
```

#### 自定义下次执行时间

有些时候我们需要进行一些业务逻辑，比如数据库查询等操作返回下一次执行时间，这个时候我们可以通过高级自定义方式。

##### 高级自定义间隔方式

```c#
SpareTime.Do(()=>{
    // 这里可以查询数据库或进行或进行任何业务逻辑

    if(符合逻辑){
        return 1000; // 每秒执行
    }
    else return -1; // 不符合逻辑取消任务
},
(timer,count)=>{
    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
});
```

>
>
>##### 配置是否持续检查
>
>默认情况下，该自定义会在返回 `小于或等于0` 时终止任务的执行。但是我们希望该任务不要终止，只要符合条件都一直执行，只需要配置 `cancelInNoneNextTime: false` 即可

##### 高级自定义`Cron`表达式

```c#
SpareTime.Do(()=>{
    // 这里可以查询数据库或进行或进行任何业务逻辑

    if(符合逻辑){
        return DateTimeOffset.Now.AddMinutes(10);  // 十分钟后再执行
    }
    else return null; // 不符合逻辑取消任务
},
(timer,count) => {
    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
});
```

> ##### 配置是否持续检查
>
> 默认情况下，该自定义会在返回 `null` 时终止任务的执行。但是我们希望该任务不要终止，只要符合条件都一直执行，只需要配置 `cancelInNoneNextTime: false` 即可，如：
>
> ```c#
> SpareTime.Do(()=>{
>     // 这里可以查询数据库或进行或进行任何业务逻辑
> 
>     if(符合逻辑){
>         return SpareTime.GetCronNextOccurrence("cron 表达式");
>     }
>     else return null; // 不符合逻辑继续检查
> },
> (timer,count) => {
>     Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
>     Console.WriteLine($"一共执行了：{count} 次");
> }, cancelInNoneNextTime: false);
> ```
>
> 

##### `ISpareTimeWorker`说明

除了上面的 `SpareTime` 静态类方式，`Furion` 框架还提供了 `ISpareTimeWorker` 方式，使用该方式非常简单，只需要自定义一个**公开且非抽象非静态**类并实现 `ISpareTimeWorker` 即可。

在该类中定义的任务方法需满足以下规则：

- 必须是**公开且实例方法**
- 该方法必须返回 `void` 且提供 `SpareTimer` 和 `long` 两个参数
- 必须贴有 `[SpareTime]` 特性

如：

```c#
public class JobWorker : ISpareTimeWorker
{
    // 每隔一秒执行，且立即启动
    [SpareTime(1000, "jobName1", StartNow = true)]
    public void DoSomething1(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }

    // 每分钟执行，且立即启动
    [SpareTime("* * * * *", "jobName2", StartNow = true)]
    public void DoSomething2(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }

    // 每秒执行，且等待启动
    [SpareTime("* * * * * *", "jobName3",CronFormat = CronFormat.IncludeSeconds, StartNow = false)]
    public void DoSomething3(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }

    // 每秒执行一次，每分钟也执行一次
    [SpareTime(1000, "jobName4", StartNow = true)]
    [SpareTime("* * * * *", "jobName5", StartNow = true)]
    public void DoSomething4(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }

    // 只执行一次
    [SpareTime(1000, "jobName5", StartNow = true, DoOnce = true)]
    public void DoSomething5(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }

    // 读取配置文件，通过 #(配置路径)
    [SpareTime("#(MyJob:Time)", "jobName6", StartNow = true, DoOnce = true)]
    public void DoSomething5(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
    }

    // 支持异步
    [SpareTime(1000, "jobName1", StartNow = true)]
    public async Task DoSomethingAsync(SpareTimer timer, long count)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
        Console.WriteLine($"一共执行了：{count} 次");
        await Task.Completed;
    }
}
```

>
>
>##### 关于依赖注入
>
>`ISpareTimeWorker` 接口主要是用来查找定时器对象的，也就是它的实现类并未提供依赖注入功能，所以在实现类并不支持构造函数注入依赖项。

##### [SpareTime]特性

`[SpareTime]` 支持以下配置属性

- `Interval`：间隔时间, `double` 类型
- `CronExpression`：`Cron` 表达式，`string` 类型
- `WorkerName`：任务唯一标识，`string` 类型，`必填`
- `Description`：任务描述，`string` 类型
- `DoOnce`：是否只执行一次，`bool` 类型，默认 `false`
- `StartNow`：是否立即启动，默认 `false`
- `CronFormat`：`Cron` 表达式格式化方式，`CronFormat` 枚举类型，默认 `CronFormat.Standard`
- `ExecuteType`：配置任务执行方式，`SpareTimeExecuteTypes` 枚举类型，默认 `SpareTimeExecuteTypes.Parallel`

#### `SpareTime`静态类

`SpareTime` 静态类提供了一些方法方便初始化和管理任务

初始化任务[#](https://dotnetchina.gitee.io/furion/docs/job#2681-初始化任务)

```cs
// 开启间隔任务SpareTime.Do(interval, [options]);
// 开启 Cron 表达式任务SpareTime.Do(expression, [options]);
// 只执行一次任务SpareTime.DoOnce(interval, [options]);
// 实现自定义任务SpareTime.Do(()=>{    return DateTime.Now.AddMinutes(10);},[options]);
```

实现后台执行[#](https://dotnetchina.gitee.io/furion/docs/job#2682-实现后台执行)

```cs
// 实现后台执行SpareTime.DoIt(()=>{});
```

开始一个任务[#](https://dotnetchina.gitee.io/furion/docs/job#2683-开始一个任务)

```cs
SpareTime.Start("任务标识");
```

 暂停一个任务[#](https://dotnetchina.gitee.io/furion/docs/job#2684-暂停一个任务)

```cs
SpareTime.Stop("任务标识");// 还可以标记一个任务执行失败SpareTime.Stop("任务标识", true);
```

 取消一个任务[#](https://dotnetchina.gitee.io/furion/docs/job#2685-取消一个任务)

```cs
SpareTime.Cancel("任务名称");
```

销毁所有任务[#](https://dotnetchina.gitee.io/furion/docs/job#2686-销毁所有任务)

```cs
SpareTime.Dispose();
```

 获取所有任务[#](https://dotnetchina.gitee.io/furion/docs/job#2687-获取所有任务)

```cs
var workers = SpareTime.GetWorkers();
```

获取单个任务[#](https://dotnetchina.gitee.io/furion/docs/job#2688-获取单个任务)

```cs
var worker = SpareTime.GetWorker("workerName");
```

 解析 `Cron` 表达式[#](https://dotnetchina.gitee.io/furion/docs/job#2689-解析-cron-表达式)

```cs
var nextTime = SpareTime.GetCronNextOccurrence("* * * * *");
```

#### `并行`和`串行`执行方式

`Furion` 框架提供了任务两种执行方式 `并行` 和 `串行`：

- `并行`：无需等待上一次任务完成，**默认值**
- `串行`：需等待上一次任务完成

##### `SpareTime`静态方式制定

```c#
SpareTime.Do(1000, (t, i) =>
{
    Thread.Sleep(5000); // 模拟执行耗时任务
    Console.WriteLine($"{t.WorkerName} -{t.Description} - {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {i}");
}, "serialName", "模拟串行任务", executeType: SpareTimeExecuteTypes.Serial);
```

##### `ISpareTimeWorker`方式

```c#
[SpareTime(1000, "jobName1", StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
public void DoSomething1(SpareTimer timer, long count)
{
    Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
    Console.WriteLine($"一共执行了：{count} 次");
}
```

#### 任务的异常处理

有些时候我们可能在执行任务过程中出现异常，`Furion` 也提供了属性判断是否有异常和异常信息，方便记录到日志中，如：

```c#
SpareTime.Do(1000, (t, c) =>
{
    // 判断是否有异常
    if (t.Exception.Any())
    {
        Console.WriteLine(t.Exception.Values.LastOrDefault()?.Message);
    }
    // 执行第三次抛异常
    if (c > 2)
    {
        throw Oops.Oh("抛异常" + c);
    }
    else
    {
        Console.WriteLine($"{t.WorkerName} -{t.Description} - {DateTime.Now:yyyy-MM-dd HH:mm:ss} - {c}");
    }
}, "exceptionJob");
```

>
>
>##### 特别说明
>
>如果一个任务错误次数达 `10次` 则任务将自动停止，并标记任务状态为 `Failed`。

#### 如何在任务中解析对象

有些时候我们需要在任务中进行数据库操作或解析服务，这时候我们只需要创建一个新的作用域即可

##### `Spareime`静态类中

```c#
SpareTime.Do(1000, (timer,count) => {
    Scoped.Create((_, scope) =>
    {
        var services = scope.ServiceProvider;

        // 获取数据库上下文
        var dbContext = Db.GetDbContext(services);

        // 获取仓储
        var respository = Db.GetRepository<Person>(services);

        // 解析其他服务
        var otherService = services.GetService<XXX>();
        var otherService2 = App.GetService<XXX>(services);
    });
}, "任务标识");
```

##### `ISpareTimeWorker`方式

```c#
[SpareTime(1000, "jobName1", StartNow = true, ExecuteType = SpareTimeExecuteTypes.Serial)]
public void DoSomething1(SpareTimer timer, long count)
{
    Scoped.Create((_, scope) =>
    {
        var services = scope.ServiceProvider;

        // 获取数据库上下文
        var dbContext = Db.GetDbContext(services);

        // 获取仓储
        var respository = Db.GetRepository<Person>(services);

        // 解析其他服务
        var otherService = services.GetService<XXX>();
        var otherService2 = App.GetService<XXX>(services);
    });
}
```

> ##### 数据库操作注意
>
> 如果作用域中对**数据库有任何变更操作**，需手动调用 `SaveChanges` 或带 `Now` 结尾的方法。也可以使用 `Scoped.CreateUow(handler)` 代替。

#### 在`BackgroundService`中使用

`BackgroundService` 是 `.NET Core 3` 之后提供的轻量级后台任务，同时可以发布到 `Windows` 服务和 `Linux` 守护进程中。

##### 间隔执行方式

```c#
using Furion.TaskScheduler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // 间隔执行任务
                await SpareTime.DoAsync(1000, () =>
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }, stoppingToken);
            }
        }
    }
}
```

##### `Cron`表达式执行方式

```c#
using Furion.TaskScheduler;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // 执行 Cron 表达式任务
                await SpareTime.DoAsync("*/5 * * * * *", () =>
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }, stoppingToken, CronFormat.IncludeSeconds);
            }
        }
    }
}
```



#### IIS部署回收设置

如果在项目中使用了定时任务且部署到 `IIS` 中，那么需要设置 `IIS` 禁止回收，如：

![img](https://dotnetchina.gitee.io/furion/img/iishuishou.jpg)

> ##### 部署建议
>
> 建议定时任务采用 `Worker Service` 独立部署方式，不应依托 `Web` 项目进程中。[查看【 Worker Service】章节](https://dotnetchina.gitee.io/furion/docs/process-service)



### [脱敏处理](https://dotnetchina.gitee.io/furion/docs/sensitive-detection)

#### 1. 关于脱敏

> 数据脱敏是指对某些敏感信息通过脱敏规则进行数据的变形，实现敏感隐私数据的可靠保护。在涉及客户安全数据或者一些商业性敏感数据的情况下，在不违反系统规则条件下，对真实数据进行改造并提供测试使用，如身份证号、手机号、卡号、客户号等个人信息都需要进行数据脱敏。数据安全技术之一，数据库安全技术主要包括：数据库漏扫、数据库加密、数据库防火墙、数据脱敏、数据库安全审计系统。

在 `Furion` 系统中，`脱敏处理` 指的是对不符合系统合法词汇检测验证

#### 2. 如何使用

`Furion` 框架内置了一套默认的脱敏词汇脱敏处理机制，并且提供自定义操作。

* 注册`脱敏词汇检测`服务

  ```c#
  public void ConfigureServices(IServiceCollection services)
  {
      services.AddSensitiveDetection();
  }
  ```

  

* 创建`sensitive-words.txt`文件

在 `Web` 启动层项目中创建 `sensitive-words.txt` 文件，**确保采用 `UTF-8` 编码格式且设置为嵌入式资源！**🎃

`sensitive-words.txt` 内容格式为每一行标识一个脱敏词汇：

```txt
坏人
无语
滚开
八嘎
```

接下来设置为嵌入式资源：

![img](https://dotnetchina.gitee.io/furion/img/tm.png)

* 使用脱敏检测

  * **实现数据验证脱敏检测 `[SensitiveDetection]`**

    `Furion` 框架提供了 `[SensitiveDetection]` 验证特性，可以对参数、属性进行脱敏验证，用法和 `[DataValidation]` 一致，如：

    ```c#
    // 在属性中使用
    public class Content
    {
        [SensitiveDetection]
        public string Text { get; set; }
    }
    
    // 在 动态API/Controller 中使用
    public void Test([SensitiveDetection] string text)
    {
    
    }
    ```

  - **通过 `ISensitiveDetectionProvider` 服务使用**

  `Furion` 框架也提供了 `ISensitiveDetectionProvider` 服务进行手动脱敏验证处理，如

  ```c#
  public class FurionService
  {
      private readonly ISensitiveDetectionProvider _sensitiveDetectionProvider;
      public FurionService(ISensitiveDetectionProvider sensitiveDetectionProvider)
      {
          _sensitiveDetectionProvider = sensitiveDetectionProvider;
      }
  
      /// <summary>
      /// 获取所有脱敏词汇
      /// </summary>
      /// <returns></returns>
      public async Task<IEnumerable<string>> GetWordsAsync()
      {
          return await _sensitiveDetectionProvider.GetWordsAsync();
      }
  
      /// <summary>
      /// 判断是否是正常的词汇
      /// </summary>
      /// <param name="text"></param>
      /// <returns></returns>
      public async Task<bool> VaildedAsync(string text)
      {
          return await _sensitiveDetectionProvider.VaildedAsync(text);
      }
  
      /// <summary>
      /// 替换非正常词汇
      /// </summary>
      /// <param name="text"></param>
      /// <returns></returns>
      public async Task<string> ReplaceAsync(string text)
      {
          return await _sensitiveDetectionProvider.ReplaceAsync(text, '*');
      }
  }
  ```

* 脱敏词汇替换

`Furion` 框架也提供了替换脱敏词汇的特性支持，如：

```c#
// 在属性中使用
public class Content
{
    [SensitiveDetection('*')]
    public string Text { get; set; }
}
```

> `Furion` 框架目前只提供类中使用替换特性支持，但未提供方法单个值替换支持，如**以下代码不受支持**：
>
> ```c#
> public void Test([SensitiveDetection('*')] string text){}
> ```

#### 3. 自定义脱敏词汇处理

`Furion` 框架除了内置了一套默认的 `脱敏处理` 程序，也支持自定义脱敏处理程序。

* 自定义`ISensitiveDectectionProvider`程序，如

```c#
/// <summary>
/// 自定义脱敏词汇检测器
/// </summary>
public class YourSensitiveDetectionProvider : ISensitiveDetectionProvider
{
    // 支持构造函数注入
    public YourSensitiveDetectionProvider()
    {
    }

    /// <summary>
    /// 返回所有脱敏词汇
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<string>> GetWordsAsync()
    {
        // 这里写你脱敏词汇数据的来源（如从数据库读取），建议做好缓存操作
    }

    /// <summary>
    /// 判断脱敏词汇是否有效
    /// </summary>
    /// <param name="text"></param>
    /// <returns></returns>
    public async Task<bool> VaildedAsync(string text)
    {
        // 这里写你如何判断是正常的字符，返回 true 正常，返回 false 表示是个脱敏词汇
    }

    /// <summary>
    /// 替换脱敏词汇
    /// </summary>
    /// <param name="text"></param>
    /// <param name="transfer"></param>
    /// <returns></returns>
    public async Task<string> ReplaceAsync(string text, char transfer = '*')
    {
        // 这里写你替换非正常字符为指定字符
    }
}
```

注册自定义脱敏提供器

```c#
public void ConfigureServices(IServiceCollection services)
{
    services.AddSensitiveDetection<YourSensitiveDetectionProvider>();
}
```

之后系统将自动采用自定义的方式进行脱敏处理



### [虚拟文件系统](https://dotnetchina.gitee.io/furion/docs/file-provider)

#### 1. 关于文件系统

本章所谓的 `文件系统` 有点名不副实，其实根本算不上一个系统，它仅仅是利用一个抽象化的 `IFileProvider` 以统一的方式提供所需的文件而已。通过该 `文件系统` 可以读取物理文件和嵌入资源文件，包括目录结果读取，文件内容读取，文件内容监听等等。

`Furion` 提供了两种文件系统类型：

- `Physical`：物理文件系统类型，也就是物理机中实际存在的文件
- `Embedded`：嵌入资源文件系统类型，也就是资源文件嵌入到了程序集中，常用于模块化开发

#### 2.使用

* 注册虚拟文件系统服务

```c#
services.AddVirtualFileServer();
```

* 获取文件系统`IFileProvider`实例

  * `Func<FileProviderTypes, object, IFileProvider>`方式

    `Furion` 框架提供了 `Func<FileProviderTypes, object, IFileProvider>` 委托供构造函数注入或解析服务，如

    ```c#
    public class PersonServices
    {
        private readonly IFileProvider _physicalFileProvider;
        private readonly IFileProvider _embeddedFileProvider;
    
        public PersonServices(Func<FileProviderTypes, object, IFileProvider> fileProviderResolve)
        {
            // 解析物理文件系统
            _physicalFileProvider = fileProviderResolve(FileProviderTypes.Physical, @"c:/test");
    
            // 解析嵌入资源文件系统
            _embeddedFileProvider = fileProviderResolve(FileProviderTypes.Embedded, Assembly.GetEntryAssembly());
        }
    }
    ```

  * `FS`静态类方式

    `Furion` 框架也提供了 `FS` 静态类方式创建，如：

    ```c#
    // 解析物理文件系统
    var physicalFileProvider = FS.GetPhysicalFileProvider(@"c:/test");
    
    // 解析嵌入资源文件系统
    var embeddedFileProvider = FS.GetEmbeddedFileProvider(Assembly.GetEntryAssembly());
    ```

* `IFileProvider` 常见操作

  * 读取文件内容

    ```c#
    byte[] buffer;
    using (Stream readStream = _fileProvider.GetFileInfo("你的文件路径").CreateReadStream())
    {
        buffer = new byte[readStream.Length];
        await readStream.ReadAsync(buffer.AsMemory(0, buffer.Length));
    }
    
    // 读取文件内容
    var content = Encoding.UTF8.GetString(buffer);
    ```

  * 获取文件目录内容（需递归查找）

    ```c#
    var rootPath = "当前目录路径";
    var fileinfos = _fileProvider.GetDirectoryContents(rootPath);
    foreach (var fileinfo in fileinfos)
    {
        if(fileinfo.IsDirectory)
        {
            // 这里递归。。。
        }
    }
    ```

  * 监听文件变化

    ```c#
    ChangeToken.OnChange(() => _fileProvider.Watch("监听的文件"), () =>
    {
        // 这里写你的逻辑
    });
    ```

* 模块化静态资源配置

  通常我们采用模块化开发，静态资源都是嵌入进程序集中，这时候我们需要通过配置 `UseFileServer` 指定模块静态资源路径，如：

  ```c#
  // 默认静态资源调用，wwwroot
  app.UseStaticFiles();
  
  // 配置模块化静态资源
  app.UseFileServer(new FileServerOptions
  {
      FileProvider = new EmbeddedFileProvider(模块程序集),
      RequestPath = "/模块名称",  // 后续所有资源都是通过 /模块名称/xxx.css 调用
      EnableDirectoryBrowsing = true
  });
  ```

  

### [会话和状态管理](https://dotnetchina.gitee.io/furion/docs/sesssion-state)

#### 1. 关于回话和状态管理

`HTTP` 是无状态的协议。 默认情况下，`HTTP` 请求是不保留用户值的独立消息。但是我们可以通过以下几种方式保留请求用户数据：

- `Cookie`：通常存储在客户端的数据，请求时带回服务端
- `Session`：存储在服务端的数据（可以在存储在内存、进程等介质中）
- `Query Strings`：通过 `Http` 请求地址参数共享
- `HttpContext.Items`：存储在服务端端，只在请求声明周期内使用，请求结束自动销毁
- `Cache`：服务端缓存，包括内存缓存、分布式内存缓存、IO 缓存、序列化缓存以及数据库缓存

#### 2. 如何使用

* `Cookie`使用

  使用 `Cookie` 非常简单，如：

  ```c#
  // 读取 Cookies
  var value = httpContext.Request.Cookies["key"];
  
  // 设置 Cookies
  var option = new CookieOptions();
  option.Expires = DateTime.Now.AddMilliseconds(10);
  httpContext.Response.Cookies.Append(key, value, option);
  
  // 删除 Cookies
  httpContext.Response.Cookies.Delete(key);
  ```

  > ##### 特别说明
  >
  > `httpContext` 可以通过 `IHttpContextAccessor` 获取，也可以通过 `App.HttpContext` 获取。

  我们还可以通过 `Cookie` 实现授权功能及单点登录（SSO）：[网站共享 Cookie](https://docs.microsoft.com/zh-cn/aspnet/core/security/cookie-sharing?view=aspnetcore-5.0)

* `Session`使用

  在使用 `Session` 之前，必须注册 `Session` 服务：（如果

  ```c#
  public class Startup
  {
      public void ConfigureServices(IServiceCollection services)
      {
          // services.AddDistributedMemoryCache(); 框架内部已经默认注册
  
          services.AddSession(options =>
          {
              options.IdleTimeout = TimeSpan.FromSeconds(10);
              options.Cookie.HttpOnly = true;
              options.Cookie.IsEssential = true;
          }); // 注意在控制器之前注册！！！！
  
          services.AddControllersWithViews();
      }
  
      public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
      {
          if (env.IsDevelopment())
          {
              app.UseDeveloperExceptionPage();
          }
          else
          {
              app.UseExceptionHandler("/Home/Error");
              app.UseHsts();
          }
  
          app.UseHttpsRedirection();
          app.UseStaticFiles();
  
          app.UseRouting();
  
          app.UseAuthentication();
          app.UseAuthorization();
  
          app.UseSession();
  
          app.UseEndpoints(endpoints =>
          {
              endpoints.MapDefaultControllerRoute();
              endpoints.MapRazorPages();
          });
      }
  }
  ```

  > ##### 中间件注册顺序
  >
  > `app.UseSession()` 必须在 `app.UseRouting()` 和 `app.UseEndpoints()` **之间**注册！

  *  常见例子

    ```c#
    // 读取 Session
    var byteArr = httpContext.Session.Get("key"); // 返回 byte[]
    var str = httpContext.Session.GetString("key");   // 返回 string[]
    var num = httpContext.Session.GetInt32("key");    // 返回 int
    
    // 设置 Session
    httpContext.Session.SetString("key", "value");  // 设置字符串
    httpContext.Session.SetInt32("key", 1); // 设置 int 类型
    ```

  * 自定义设置任一类型拓展:

    ```c#
    public static class SessionExtensions
    {
        public static void Set<T>(this ISession session, string key, T value)
        {
            session.SetString(key, JsonSerializer.Serialize(value));
        }
    
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            return value == null ? default : JsonSerializer.Deserialize<T>(value);
        }
    }
    ```

    - 防止 `Session ID` 改变或 `Session` 失效
    
    在 `Startup.cs` 的 `ConfigureServices` 配置即可：
    
    ```c#
    services.Configure<CookiePolicyOptions>(options =>
    {
     　　options.CheckConsentNeeded = context => false; // 默认为true，改为false
    　　 options.MinimumSameSitePolicy = SameSiteMode.None;
    });
    ```
  
* `Query Strings`使用

  该方式使用非常简单，只需 `httpContext.Request.Query["key"]` 即可

* `HttpContext.Items`使用

  `HttpContext` 对象提供了 `Items` 集合属性，可以让我们在单次请求间共享数据，请求结束立即销毁，可以存储任何数据。使用也非常简单，如：

  ```c#
  // 读取
  var value = httpContext.Items["key"];
  
  // 添加
  httpContext.Items["key"] = "任何值包括对象";
  
  // 删除
  httpContext.Items.Remove("key");
  ```

  

### 

### [`IPC`进程通信](https://dotnetchina.gitee.io/furion/docs/ipc)

#### 1. 关于IPC通信

> 引用百度百科
>
> IPC（Inter-Process Communication，进程间通信）。进程间通信是指两个进程的数据之间产生交互。

通俗点说，`IPC` 可以实现不同应用程序间通信（交互数据）。

#### 2. 实现IPC通信的方式

- 半双工 Unix 管道
- FIFOs(命名管道)
- **消息队列**（常用模式）
- 信号量
- **共享内存**（常用模式，`Furion` 框架默认实现方式）
- **网络 Socket**（常用模式）

#### 3. IPC通信模式

`IPC` 本身指的是 `进程间` 通信，但 `Furion` 框架将内置 `进程间/内` 两种进程通信模式。

- `进程内通信`：`Furion` 采用 `Channel` 管道提供进程内通信
- `进程外通信（未实现）`：`Furion` 采用 `MemoryMapperFile` 共享内存方式实现进程外通信（后续版本完善）

#### 4. 进程内通信（线程间）

进程内通信俗称线程间通信，`Furion` 框架采用 `C#` 提供的 `Channel（管道）` + `Lazy` + `Task.Factory` 实现长时间高性能的线程间通信机制。`Channel` 管道也是目前 `.NET/C#` 实现 `生产者-订阅者` 模式最简易且最为强大的实现。

* 了解Channel

  `Channel` 是在 `.NET Core 2.1+` 版本之后加入。`Channel` 底层实现是一个高效的、线程安全的队列，可以在线程之间传递数据。

  `Channel` 的主要应用场景是 `发布/订阅、观察者模式` 中使用，如：`事件总线` 就是最好的实现方式。通过 `Channel` 实现 `生产-订阅` 机制可以减少项目间的耦合，提高应用吞吐量。

  `Furion` 框架提供了 `ChannelBus<TMessage, THandler>` 密封类，提供 `UnBoundedChannel` 和 `BoundedChannel` 两种管道通信模式。

  - `UnBoundedChannel`：具有无限容量的 `Channel`, 生产者可以全速进行生产数据，但如果消费者的消费速度低于生产者，`Channel` 的资源使用会无限增加，会有服务器资源耗尽的可能。
  - `BoundedChannel`：具有有限容量的 `Channel`，`Furion` 框架默认为 `1000`，到达上限后，生产者进入等待写入直到有空闲，好处是可以控制生产的速度，控制系统资源的使用。**（推荐）**

* **常规使用**

  #### 创建 `ChannelHandler<TMessage>` 管道处理程序

  ```c#
  using Furion.ProcessChannel;
  using System;
  using System.Threading.Tasks;
  
  namespace Furion.Core
  {
      /// <summary>
      /// 创建管道处理程序（处理 String 类型消息）
      /// </summary>
      public class MyChannelHandler : ChannelHandler<string>
      {
          /// <summary>
          /// 接受到管道消息后处理程序
          /// </summary>
          /// <param name="message"></param>
          /// <returns></returns>
          public override Task InvokeAsync(string message)
          {
              Console.WriteLine(message);
  
              return Task.CompletedTask;
          }
      }
  }
  ```

  > ##### NOTE
  >
  > `ChannelHandler<TMessage>` 泛型类型决定了你要接受那种类型的消息，不同类型消息将会自动过滤筛选。

  #### 使用 `ChannelBus<TMessage, THandler>` 发送消息

  ```c#
  public async Task SendAsync()
  {
      for (int i = 0; i < 100; i++)
      {
          // 使用有限容量生产数据
          await ChannelBus<string, MyChannelHandler>.BoundedChannel.Writer.WriteAsync($"Loop {i} times.");
      }
  }
  ```

  以上代码也可以通过 `ChannelBus<string, MyChannelHandler>.BoundedChannel.Writer.TryWrite()` 同步写入。



* 实现多订阅

  默认情况下，`Furion` 初始化了一个长时间的 `Task` 任务进行数据检查及订阅，如需实现多订阅模式，可创建新的订阅任务即可：

  ```c#
  var reader = ChannelBus<string, MyChannelHandler>.BoundedChannel.Reader;
  ChannelHandler<string> handler = Activator.CreateInstance<MyChannelHandler>();
  
  // 创建长时间线程管道读取器
  _ = Task.Factory.StartNew(async () =>
    {
        while (await reader.WaitToReadAsync())
        {
            if (!reader.TryRead(out var message)) continue;
            // 默认重试 3 次（每次间隔 1s）
            await Retry.Invoke(async () => await handler.InvokeAsync(message), 3, 1000, finalThrow: false);
        }
    }, TaskCreationOptions.LongRunning);
  ```

  > 更多Channel知识
  >
  > 可查阅 [Dotnet Core 下的 Channel, 你用了吗？](https://www.cnblogs.com/tiger-wang/p/14068973.html) 博客教程（😃 写的不错）

`Furion` 目前暂未提供的进程外通信功能