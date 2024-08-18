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
1. 如果有一张图像是包含多个游戏角色的动作，可以点击图像使用Sprite Editor按钮中的Slice对其进行分割
<br>
2. 如果图像模糊，可以将精灵表中的Compression的选项卡改为None，并且将Fliter Mode改为Point，这样会使预想变得锐利

### 使用子对象使游戏物体在正中心
将Sprite Renderer组件添加在子物体中,其他例如colider,RigidBody等都添加在父物体中

### Animition controller
将该组件添加到游戏物体上，之后通过Animator的状态机来控制动画的播放
### Animition
该选项卡用于制作一个动作组，可以从刚才切割的精灵添加到animition中，会生成.anim文件,可以设置采样率控制动画播放的速度

### Animator
该选项卡用于控制状态机的播放，可以通过Animator的Parameters的选项卡来控制状态机间的切换

### 动画之间的过渡时间问题解决
点击动画机间过渡的那条线，取消Has Exit Time的勾选，并将Settings中的Transition Duratior设置为0
