using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] pwanTowers;

    [SerializeField]
    private List<GameObject> allNodes;
    public int nodesCount = 16;

    [SerializeField]
    private int towerBuildeGold;
    [SerializeField]
    private PlayerGold playerGold;

    List<GameObject> candidatesNodes = new List<GameObject>();
    public List<GameObject> towers = new List<GameObject>();

    public void RandomNodeBuild()
    {
        if (nodesCount > 0)
        {
            int randomTowerIndex = Random.Range(0, 2);//������, ��� �����ϰ� ������

            for (int i = 0; i < allNodes.Count; i++)
            {//��ü ����� ������ŭ �ݺ�
                if (allNodes[i].CompareTag("Node"))
                {
                    candidatesNodes.Add(allNodes[i]);
                }
            }

            int randomNodeIndex = Random.Range(0, candidatesNodes.Count);

            if (candidatesNodes[randomNodeIndex] != null && towerBuildeGold <= playerGold.CurrentGold)
            {
                playerGold.CurrentGold -= towerBuildeGold;
                GameObject obj = Instantiate(pwanTowers[randomTowerIndex], candidatesNodes[randomNodeIndex].transform.position, Quaternion.identity);
                obj.GetComponent<Tower>().node = candidatesNodes[randomNodeIndex];
                //Ÿ���� �ؿ� �ִ� ����� ������ �˰ԵǾ���.
                obj.GetComponent<Tower>().node.tag = "BuildNode";
                //�±׸� �ٲ��
                towers.Add(obj);
                nodesCount--;
                candidatesNodes.Clear();
            }
        }
        else if (nodesCount <= 0)
        {
            Debug.Log("Ÿ���� �Ǽ��� ���� �����ϴ�.");
            return;
        }
        else
        {
            Debug.Log("��尡 �����մϴ�.");
            return;
        }
    }
    public void Retart()
    {
        nodesCount = 16;
        TowerClear();
        for (int i = 0; i < allNodes.Count; i++)
        {
            allNodes[i].tag = "node";
        }
        candidatesNodes.Clear();

    }

    private void TowerClear()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers != null)
                Destroy(towers[i]);
        }
        towers.Clear();
    }
}
