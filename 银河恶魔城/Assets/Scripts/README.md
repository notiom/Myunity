# 代码整理心得

构建该项目的代码设计思路为:<br>
1.构建游戏地图<br>
2.构建玩家的攻击逻辑以及各种技能逻辑<br>
3.构建每个敌人的动作效果<br>
4.构建背包系统inventory<br>
5.构建技能数<br>
6.构建主菜单与地图菜单<br>

# 代码分析

### 1.未在文件夹下的代码
1.1  CameraController -> 使用偏移量控制背景天空的移动<br>
1.2  Checkpoint -> 检查点函数,用于限制玩家移动的检查点,即只有击杀本个检查点前的怪物才可以进入下一个检查点<br>
1.3  DeadZone -> 只要落入该实体内,Entity就会立即死亡<br>
1.4  Entity一定有anim(动画师), rb(刚体组件), fx(特效), stats(数值状态), cd(碰撞器)<br>
所有的敌人玩家都继承于Entity<br>
<br>
1.4.1 RestrictPosition() -> 限制player或者enemy的位置不能超出检查点<br>
1.4.2 DamageEffect() -> 实体收到攻击后先闪烁再被击退<br>
1.4.3 SetupKnockbackDir() -> 设置一个击退方向,击退方向来自于攻击的反方向<br>
1.4.4 HitKnockback() -> 击退效果<br>
1.4.5 Flip() -> 翻转实体<br>
1.4.6 OnDrawGizmos() -> 画线,里面 DrawLine() -> 输入为(Vector3,Vector3) 两个位置 ,DrawWireSphere(Vector3,float) 一个位置,一个半径<br>

### 2.Effect
