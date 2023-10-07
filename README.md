# 开发一个简单的AR插件

## 前期准备
首先默认当前阅读文档的你，有一定的计算机编程能力，最好具备一定的C#经验 (Java也行，两者很相似会了其中一个另一个基本也能学会)

## 开始

1. 创建项目
![Alt text](/_resources/image.png)
![Alt text](/_resources/image-1.png)

2. 类库支持必须得 `.NET 6.0 +`
![Alt text](/_resources/image-2.png)

3. 创建项目完毕
![Alt text](/_resources/image-3.png)

## 添加依赖

4. 将 AimRobot-api 添加进项目依赖中
    - DLL引入方式
    - 项目代码引入的方式

在本文中，将使用第一种方式 **DLL引入方式** 作为示范

5. 在解决方案中右击此项 “Dependencies”
![中文版VS可能是其它的文字](/_resources/image-4.png)
![Alt text](/_resources/image-5.png)

6. 在弹出来的依赖管理设置中，将接口的DLL文件引入。  
 你可以从 **[AimRobot-api](https://github.com/H4rry217/AimRobot-api)** 中下载源码自行编译，也可以从AimRobotLite压缩包中获取
![Alt text](/_resources/image-6.png)
![Alt text](/_resources/image-7.png)
![Alt text](/_resources/image-8.png)

7. 添加依赖成功  
![Alt text](/_resources/image-9.png)

## 开始编写

本教程中使用默认的Class1作为类名，建议开发时根据需求另行取名

8. 继承 `PluginBase` 类并实现其抽象方法，每个抽象方法基本有注释说明其用途
![Alt text](/_resources/image-10.png)

9. **必须！必须！必须！** 在资源文件中，要注明继承了插件的主类（继承了 `PluginBase` 的类）的路径  
如当前插件主类的路径是 *ar_example_plugin.Class1*，即命名空间+类名

    - 打开项目设置
    ![Alt text](/_resources/image-11.png)
    - 打开资源设置
    ![Alt text](/_resources/image-12.png)
    - 写入 `MainClass` 为主类路径，然后保存设置并退出
    ![Alt text](/_resources/image-13.png)

10. 回到主页，此时插件已经完成了，可以构建dll了
![Alt text](/_resources/image-14.png)

11. 构建好的DLL一般在差不多这种位置，将DLL文件复制到ARL的plugins目录（不要复制其它多余的）
![Alt text](/_resources/image-15.png)
![Alt text](/_resources/image-16.png)

12. 启动ARL，就可以看到插件成功被加载
![Alt text](/_resources/image-17.png)
![Alt text](/_resources/image-18.png)

## 事件功能编写
上面的步骤中仅仅只是编写了一个能够被加载的插件，但其中并没写任何的功能，接下来将演示编写一个屏蔽骂人的玩家的功能

1. 在项目中创建一个新的事件监听类 `MyPluginEventListener`，并实现 `IEventListener`接口
![Alt text](/_resources/image-19.png)
![Alt text](/_resources/image-20.png)

2. 编写方法逻辑：当有人聊天发言中包含 *“shabi”* ，则屏蔽该玩家
![Alt text](/_resources/image-21.png)
解释：在继承了 `IEventListener` 的接口中，你可以编写一个的方法（方法名随意），方法的参数为你希望接收到的**事件**（如图中的`PlayerChatEvent`，表示当有人说话时，就会触发这个说话事件，然后会将对应的事件信息传递给这个方法），然后使用 `EventHandler` 属性标记这个方法（标记后ARL会在对应的事件触发时调用该被标记的方法）

3. 回到插件主类 `Class1`，将 `MyPluginEventListener` 这个事件监听器 **注册**  
建议在 **OnLoad** 方法中注册 `MyPluginEventListener` 事件监听器。你也可以在 **OnDisable**、**OnEnable** 等其它生命周期方法内注册，***不会限制此行为但不提倡***
![Alt text](/_resources/image-22.png)

4. 此时已经完成了编写，按照之前的步骤重新构建DLL，然后将DLL放入plugins文件夹再重启ARL
![Alt text](/_resources/image-23.png)
![Alt text](/_resources/image-24.png)  
功能正常 ~~***（出于演示目的，使用的是服主帐号发送脏话，所以没被屏蔽出去，毕竟没法屏蔽服主）***~~

ARL2.0版本最初提供 `PlayerChatEvent`、`PlayerDeathEvent` 两大事件(*玩家聊天*和*玩家被击杀*)，且会在后续更新中拓展更多的事件，可以根据这两个事件自由的编写类似 *“超杀屏蔽”*、*“脏话屏蔽”*、*“与Q群聊天”* 等功能插件。你也可以实现 `RobotEvent` 抽象类来 **拓展属于你自己的事件**

## 指令功能编写
ARL还提供了指令的拓展功能，免得插件开发者另外开发指令模块。你可以编写一个特定的指令，在ARL的软件中输入或者在游戏聊天中输入指令来完成对应的操作。下面将演示编写一个服主能在游戏中踢人的指令功能 *（当指令发送者为服主的时候，就屏蔽玩家，否则显示无权执行）*。

1. 新建一个类  
![Alt text](/_resources/image-25.png)

2. 实现 `ICommandListener` 接口
![Alt text](/_resources/image-26.png)
**GetCommandKeyword()** 的返回值为 *kick* ，表示当有人在游戏中输入 *“!kick”* 开头或者在ARL控制端输入 *“kick”* 开头的指令时，会触发这个指令 *OnCommand* 的执行，并且ARL会解析指令中的参数为 `CommandData` 对象传递给 *OnCommand* 方法。

     如当输入 ***!kick --name=RobotPlayer*** 时，ARL将会解析 name 中的 *RobotPlayer* 至 `CommandData`，可以使用GetValue获取到对应的参数值
    
3. 回到插件主类 `Class1`，将刚刚写好的指令注册监听
![Alt text](/_resources/image-27.png)

4. 重新构建DLL，然后将DLL放入plugins文件夹再重启ARL  
![Alt text](/_resources/image-28.png)  
![Alt text](/_resources/image-29.png)  
***指令同时也能在ARL的控制端使用***
![Alt text](/_resources/image-30.png)

## 结束
开发接口不仅提供了以上拓展，还支持拓展反作弊策略、其它语言插件的加载策略等等，这里仅展示其中的一小部分功能。ARL的基础功能（禁用二式、防刷屏）都是可关闭的，认为不满意你可以自己编写满意的程序逻辑来拓展ARL，如果遇到不了解的你也可以随时联系沟通。