using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
	public static EnemyManager instance; // ����ģʽ

	// "Current scene enemies of amount"
	[Header("How to generate enemies")]
    [SerializeField] private List<Transform> enemyPosition;
    [SerializeField] private List<GameObject> enemies;
    [SerializeField] public List<int> currentCheckEnemiesOfAmount; // ��ǰ����ĵ��˸���

	private int checkpointIndex; // ��ǰλ�ڵļ���
    private int startIndex;
    private Transform[] checkpoints;

    private Player player;
    private bool isBoss;
    private bool isGenerated;
	private bool isFinished;

	// ���ɵĵ��˶�������,���ں����������Ƿ�ɱ��
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
		// ����Ƿ�Ӧ�û���һ������
		if (currentEnemies.Count != 0) return;

		// �����ǰû�е��������ɵ���,����������ɵ���,���ٿ��Ƿ�Ӧ�ý���ƶ�����

		// ������ߵ�ǰ��һ���ʱ��������
		if(!isGenerated)
		{
			if (isBoss && player.transform.position.x > checkpoints[checkpointIndex - 1].position.x + 5f)
			{
				GenerateEnemies();
				isGenerated = true;
			}

			//  && !checkpoints[checkpointIndex].GetComponent<Checkpoint>().activated
			// �ߵ�����ʱ���㲻���ܱ��������Բ���Ҫ�ж�
			if (!isBoss && player.transform.position.x > checkpoints[checkpointIndex].position.x - 3f)
			{
				GenerateEnemies();
				isGenerated = true;
			}
			return;
		}

		// ���ǰ���û�������Ѿ�ȷ���������ɵ���
		if (isBoss)
		{
			// �ܵ�����˵�����еĹ��ﶼ����
			// ����ʤ������
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
