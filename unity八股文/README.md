### 一个游戏中多个图片点击,怎么优化内存?
1.通过图片压缩算法将其压缩<br>
2.当内存占用达到某个阈值,销毁之前存放在栈底部的图片内存,使用list模拟栈<br>

### 静态成员属于类,而不是实例
不论用子类去改还是用子类的实例去改,都只有一份,共享变量

### 但是对于泛型类,静态成员只与类型相同的泛型共用
```
public class Father<T>
{
  public static int static_I = 0;
}

public Son1 : Father<Son1>
{

}

public Son2 : Father<Son2>
{

}

static void Main(string[] args)
{
    Son1.static_I = 20;
    Son2.static_I = 30;
    Console.WriteLine(Father<int>.static_I);
    Console.WriteLine(Father<Son1>.static_I);
    Console.WriteLine(Son1.static_I);
    Console.WriteLine(Son2.static_I);
}
```
以上代码运行结果应该是0 20 20 30,泛型变化会使父类类型变化

### 使用c#制作游戏存档功能
1.xml
2.json
3.二进制

### c#是否可以通过反射获取到类内部的私有成员?
可以

### 游戏存档功能时反射的作用
1.序列化时,动态获取数据结构类信息,可以动态获取字段用于存储
2.反序列化时,可以通过反射实例化对象,写入数据
3.结构发生变化时,可以利用反射机制进行判断
