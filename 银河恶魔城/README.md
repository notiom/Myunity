### 银河恶魔城
  该文主要记录制作银河恶魔城的细节，步骤，学习笔记

### 编辑器配置
下载unity之后，首先需要将layout布局设置为Tall,之后导入visual studio或者vscode
<br>
Edit -> Preference -> External Tools

### Input按键属性配置
Edit -> Project Settings -> Input Manager -> Axes

### [SerializeField]
通常情况下，可以使用该字段使得成员变量私有化的同时将变量暴露给unity

### GetComponent<>
可以获得该游戏物体对象上的某一个组件

### 精灵选择器
如果有一张图像是包含多个游戏角色的动作，可以点击图像使用Sprite Editor按钮中的Slice对其进行分割
<br>
如果图像模糊，可以将精灵表中的Compression的选项卡改为None，并且将Fliter Mode改为Point，这样会使预想变得锐利
