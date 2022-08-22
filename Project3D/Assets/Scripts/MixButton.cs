using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixButton : MonoBehaviour
{
    //������
    //�����Ҷ� �ߺ� ������ �ȵǰ� �ؾߵȴ�

    public GameObject selectTowerA;
    public GameObject selectTowerB;

    public GameObject[] bishopTower;
    public GameObject[] rookTower;
    public GameObject[] knightTower;
    public GameObject[] queenTower;
    public GameObject[] kingTower;

    public GameObject selectParticle;

    Ray ray;
    RaycastHit hitInfo; //������ Ÿ��

    [SerializeField]
    private int MixGold;

    //������Ʈ
    public BuildManager buildManager;
    private List<GameObject> buildList;
    public List<GameObject> towers;

    [SerializeField]
    private PlayerGold playerGold;

    private void Awake()
    {
        buildList = new List<GameObject>();
        towers = new List<GameObject>();
    }
    private void Update()
    {
        SelectTower();

    }

    public void Restart()
    {
        buildList.Clear();
        TowerClear();

        if (selectTowerA != null)
        {
            CancleShader(selectTowerA);
            selectTowerA = null;
            Destroy(selectTowerA);
        }
        if (selectTowerB != null)
        {
            CancleShader(selectTowerB);
            selectTowerB = null;
            Destroy(selectTowerB);
        }
    }

    private void TowerClear()
    {
        for (int i = 0; i < towers.Count; i++)
        {
            if (towers[i] != null)
                Destroy(towers[i]);
        }
        towers.Clear();
    }

    private void SelectTower()
    {//Ŭ���� ���� ����
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {//Ŭ��������
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.CompareTag("Tower") && selectTowerA == null)
                {//A�� �±װ� Tower �̰� selectTowerA�� ��������� 
                    selectTowerA = hitInfo.transform.gameObject;
                    SelectShader(selectTowerA);
                    buildList.Add(selectTowerA);
                }
                else if (hitInfo.transform.CompareTag("Tower") && selectTowerA != null && selectTowerB == null)
                {//A�� �±װ� Tower �̰� selectTowerA�� ���� �ְ� B�� ���������
                    selectTowerB = hitInfo.transform.gameObject;
                    SelectShader(selectTowerB);
                    buildList.Add(selectTowerB);
                }
                else if (selectTowerA != null && selectTowerB != null && hitInfo.transform.CompareTag("Tower"))
                {//�Ѵ� ���� �ְ� Ŭ���Ѱ� Ÿ���϶� 
                    buildList.Clear(); //����Ʈ�� �����
                    CancleShader(selectTowerA); //���̴� ����
                    selectTowerA = selectTowerB; //A�� B�� �ְ�
                    selectTowerB = null;
                    buildList.Add(selectTowerA);//�ٽ� ����Ʈ�� �ְ�

                    selectTowerB = hitInfo.transform.gameObject; //B�� �浹������ �ְ�
                    buildList.Add(selectTowerB);//����Ʈ�� �ְ�
                    SelectShader(selectTowerB); //���̴� �Ѱ�
                }
                else if (!hitInfo.transform.CompareTag("Tower"))
                {//�ٸ��� Ŭ�� ������
                    buildList.Clear();
                    if (selectTowerA != null)
                    {
                        CancleShader(selectTowerA);
                        selectTowerA = null;
                    }
                    if (selectTowerB != null)
                    {
                        CancleShader(selectTowerB);
                        selectTowerB = null;
                    }

                }
            }
        }
    }
    private void SelectShader(GameObject obj)
    {
        obj.transform.GetChild(0).GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0.2f);
        //Outline�� �ƴ϶� ���̴� �ȿ� �ִ� _Outline�� ��ߵ�!!
    }
    private void CancleShader(GameObject obj)
    {
        obj.transform.GetChild(0).GetComponent<Renderer>().materials[1].SetFloat("_Outline", 0f);
    }

    public void MixTower()
    {
        if (buildManager.nodesCount >= 0)
        {
            if (selectTowerA != null && selectTowerB != null)
            {
                for (int i = 0; i < buildList.Count; i++)
                {
                    if (buildList[i].GetComponent<Tower>().type == buildList[i + 1].GetComponent<Tower>().type)
                    {
                        switch (buildList[i].GetComponent<Tower>().type)
                        {
                            case Tower.Type.Pwan:
                                BuildBishop();
                                break;
                            case Tower.Type.Bishop:
                                BuildRook();
                                break;
                            case Tower.Type.Rook:
                                BuildKnight();
                                break;
                            case Tower.Type.Knight:
                                BuildQueen();
                                break;
                            case Tower.Type.Queen:
                                BuildKing();
                                break;
                            case Tower.Type.King:
                                CancleShader(selectTowerA);
                                selectTowerA = null;
                                CancleShader(selectTowerB);
                                selectTowerB = null;
                                break;
                        }
                    }
                    else if (buildList[i].GetComponent<Tower>().type != buildList[i + 1].GetComponent<Tower>().type)
                    {
                        CancleShader(selectTowerA);
                        selectTowerA = null;
                        CancleShader(selectTowerB);
                        selectTowerB = null;
                        return;
                    }
                }
            }
        }
    }

    private void BuildBishop()
    {
        int randomTowerIndex = Random.Range(0, 2);

        buildList.Clear();
        MixGold = 50;
        if (playerGold.CurrentGold >= MixGold)
        {
            playerGold.CurrentGold -= MixGold;
            GameObject obj = Instantiate(bishopTower[randomTowerIndex], selectTowerB.transform.position, Quaternion.identity);
            obj.GetComponent<Tower>().node = selectTowerB.GetComponent<Tower>().node;
            //������ Ÿ���� ��� ������ ������ �־�� ��.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            towers.Add(obj);
            //selectTowerA�� ������ �ִ� ����� �±׸� �ٲ۴�!
        }
        else
            return;

        Destroy(selectTowerA);
        Destroy(selectTowerB);
        selectTowerA = null;
        selectTowerB = null;

        buildManager.nodesCount++;
    }

    private void BuildRook()
    {
        int randomTowerIndex = Random.Range(0, 2);

        buildList.Clear();
        MixGold = 150;
        if (playerGold.CurrentGold >= MixGold)
        {
            playerGold.CurrentGold -= MixGold;
            GameObject obj = Instantiate(rookTower[randomTowerIndex], selectTowerB.transform.position, Quaternion.identity);
            obj.GetComponent<Tower>().node = selectTowerB.GetComponent<Tower>().node;
            //������ Ÿ���� ��� ������ ������ �־�� ��.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA�� ������ �ִ� ����� �±׸� �ٲ۴�!
            towers.Add(obj);

        }
        else
            return;
        Destroy(selectTowerA);
        Destroy(selectTowerB);
        selectTowerA = null;
        selectTowerB = null;

        buildManager.nodesCount++;
    }

    private void BuildKnight()
    {
        int randomTowerIndex = Random.Range(0, 2);

        buildList.Clear();
        MixGold = 500;
        if (playerGold.CurrentGold >= MixGold)
        {
            playerGold.CurrentGold -= MixGold;
            GameObject obj = Instantiate(knightTower[randomTowerIndex], selectTowerB.transform.position, Quaternion.identity);
            obj.GetComponent<Tower>().node = selectTowerB.GetComponent<Tower>().node;
            //������ Ÿ���� ��� ������ ������ �־�� ��.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA�� ������ �ִ� ����� �±׸� �ٲ۴�!
            towers.Add(obj);

        }
        else
            return;
        Destroy(selectTowerA);
        Destroy(selectTowerB);
        selectTowerA = null;
        selectTowerB = null;

        buildManager.nodesCount++;
    }

    private void BuildQueen()
    {
        int randomTowerIndex = Random.Range(0, 2);

        buildList.Clear();
        MixGold = 1000;
        if (playerGold.CurrentGold >= MixGold)
        {
            playerGold.CurrentGold -= MixGold;
            GameObject obj = Instantiate(queenTower[randomTowerIndex], selectTowerB.transform.position, Quaternion.identity);
            obj.GetComponent<Tower>().node = selectTowerB.GetComponent<Tower>().node;
            //������ Ÿ���� ��� ������ ������ �־�� ��.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA�� ������ �ִ� ����� �±׸� �ٲ۴�!
            towers.Add(obj);

        }
        else
            return;
        Destroy(selectTowerA);
        Destroy(selectTowerB);
        selectTowerA = null;
        selectTowerB = null;

        buildManager.nodesCount++;
    }

    private void BuildKing()
    {
        int randomTowerIndex = Random.Range(0, 2);

        buildList.Clear();
        MixGold = 2000;
        playerGold.CurrentGold -= MixGold;
        if (playerGold.CurrentGold >= MixGold)
        {
            GameObject obj = Instantiate(kingTower[randomTowerIndex], selectTowerB.transform.position, Quaternion.identity);
            obj.GetComponent<Tower>().node = selectTowerB.GetComponent<Tower>().node;
            //������ Ÿ���� ��� ������ ������ �־�� ��.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA�� ������ �ִ� ����� �±׸� �ٲ۴�!
            towers.Add(obj);

        }
        else
            return;
        Destroy(selectTowerA);
        Destroy(selectTowerB);
        selectTowerA = null;
        selectTowerB = null;

        buildManager.nodesCount++;
    }
}
