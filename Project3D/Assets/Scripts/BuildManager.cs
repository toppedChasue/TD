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
            int randomTowerIndex = Random.Range(0, 2);//검은색, 흰색 랜덤하게 나오게

            for (int i = 0; i < allNodes.Count; i++)
            {//전체 노드의 갯수만큼 반복
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
                //타워가 밑에 있는 노드의 정보를 알게되었다.
                obj.GetComponent<Tower>().node.tag = "BuildNode";
                //태그를 바꿨다
                towers.Add(obj);
                nodesCount--;
                candidatesNodes.Clear();
            }
        }
        else if (nodesCount <= 0)
        {
            Debug.Log("타워를 건설할 곳이 없습니다.");
            return;
        }
        else
        {
            Debug.Log("골드가 부족합니다.");
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
