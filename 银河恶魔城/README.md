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

### visual studio的操作小tips
按住alt + enter 可以提取方法，按住alt + up/down可以将某一行向下移

### LayerMask
将某值赋值给layermask后可以进行地面检测，将platform的层设置为Ground，其余设置为default

### Header语法
[Header("Collision info")] 可以在unity中给出提示信息

### 粘墙的问题解决
使用create -> 2D -> physics materials 将材料赋值给地面和人物，并且将摩擦力改为0
<br>
其中，Friction为摩擦力，Bounciness为弹力

### 使用混合树创建JumpFall
先分别制作两个的动画，之后将阈值设置为-1 - 1，当这个值变化的时候，就是混合树变化

### 创建地面检查点
1.Transform groundCheck
2.float groundCheckDistance
3.OnDrawGizmos()

### 攻击动作事件触发器
找到事件的最后一个精灵，点击添加事件，新建函数触发器，将状态机函数中改变状态

### 取消玩家间碰撞的方法
edit -> project settings -> physics 2D -> layer collision matrix -> 取消物体之间collider的勾选框

