using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager instance; // 单例模式

	// "Current scene enemies of amount"
	[Header("How to generate enemies")]
    [SerializeField] private List<Transform> enemyPosition;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] public List<int> currentCheckEnemiesOfAmount; // 当前检查点的敌人个数

	private int checkpointIndex; // 当前位于的检查点
    private int startIndex;
    private Transform[] checkpoints;

    private Player player;
    private bool isBoss;
    private bool isGenerated;
	private bool isFinished;

	// 生成的敌人都在这里,用于后续检查敌人是否杀完
	public Transform enemiesParent;
	public Queue<int> currentEnemies = new Queue<int>();

	private void Awake()
	{
		if (instance != null)
		{
			Destroy(instance.gameObject);
		}
		else
		{
			instance = this;
		}

	}
	// Start is called before the first frame update
	void Start()
    {
		checkpointIndex = PlayerManager.instance.rightBound;
		for (int i = 0; i < checkpointIndex; i++)
		{ 
			startIndex += currentCheckEnemiesOfAmount[i]; 
		}
        player = PlayerManager.instance.player;
		checkpoints = PlayerManager.instance.checkpoints;
		isBoss = (PlayerManager.instance.rightBound == currentCheckEnemiesOfAmount.Count - 1) ? true : false;
	}

    // Update is called once per frame
    void Update()
    {
		if (isFinished) return;
		// 检查是否应该换下一个检查点
		if (currentEnemies.Count != 0) return;

		// 如果当前没有敌人先生成敌人,如果不能生成敌人,则再看是否应该解除移动限制

		// 当玩家走到前面一点点时触发生成
		if(!isGenerated)
		{
			if (isBoss && player.transform.position.x > checkpoints[checkpointIndex - 1].position.x + 5f)
			{
				GenerateEnemies();
				isGenerated = true;
			}

			//  && !checkpoints[checkpointIndex].GetComponent<Checkpoint>().activated
			// 走到这里时检查点不可能被激活所以不需要判断
			if (!isBoss && player.transform.position.x > checkpoints[checkpointIndex].position.x - 3f)
			{
				GenerateEnemies();
				isGenerated = true;
			}
			return;
		}

		// 如果前面灯没亮并且已经确定不能生成敌人
		if (isBoss)
		{
			// 能到这里说明所有的怪物都死亡
			// 触发胜利动画
			GameObject.Find("Canvas").GetComponent<UI>().SwitchOnEndScreen(true);
			MapManager.instance.currentPassMap++;
			MapManager.instance.UnlockMap();
			PlayerManager.instance.rightBound = 0;
			isFinished = true;
			return;
		}

		if ((!checkpoints[checkpointIndex].GetComponent<Checkpoint>().activated))
		{
			player.isNotRestrict = true;
		}

		else
		{
			player.isNotRestrict = false;
			checkpointIndex++;
			PlayerManager.instance.rightBound++;
			if (checkpointIndex == currentCheckEnemiesOfAmount.Count - 1)
			{
				isBoss = true;
			}
			isGenerated = false;
		}
	}

    private void GenerateEnemies()
    {

		for (int i = 0;i < currentCheckEnemiesOfAmount[checkpointIndex];i++)
        {
            StartCoroutine(CreateEnemy(enemies[i + startIndex], enemyPosition[i + startIndex]));
			currentEnemies.Enqueue(0);
		}
        startIndex += currentCheckEnemiesOfAmount[checkpointIndex];
	}

    private IEnumerator CreateEnemy(GameObject _enemy,Transform _transform)
    {
        yield return new WaitForSeconds(.3f);
		GameObject newEnemy = Instantiate(_enemy, _transform.position,Quaternion.identity);
        newEnemy.transform.parent = enemiesParent;

	}
    
}
