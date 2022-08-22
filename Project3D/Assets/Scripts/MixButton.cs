using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MixButton : MonoBehaviour
{
    //문제점
    //선택할때 중복 선택이 안되게 해야된다

    public GameObject selectTowerA;
    public GameObject selectTowerB;

    public GameObject[] bishopTower;
    public GameObject[] rookTower;
    public GameObject[] knightTower;
    public GameObject[] queenTower;
    public GameObject[] kingTower;

    public GameObject selectParticle;

    Ray ray;
    RaycastHit hitInfo; //선택한 타워

    [SerializeField]
    private int MixGold;

    //컴포넌트
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
    {//클릭시 정보 저장
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Input.GetMouseButtonDown(0))
        {//클릭했을때
            if (Physics.Raycast(ray, out hitInfo))
            {
                if (hitInfo.transform.CompareTag("Tower") && selectTowerA == null)
                {//A의 태그가 Tower 이고 selectTowerA가 비어있으면 
                    selectTowerA = hitInfo.transform.gameObject;
                    SelectShader(selectTowerA);
                    buildList.Add(selectTowerA);
                }
                else if (hitInfo.transform.CompareTag("Tower") && selectTowerA != null && selectTowerB == null)
                {//A의 태그가 Tower 이고 selectTowerA에 값이 있고 B가 비어있으면
                    selectTowerB = hitInfo.transform.gameObject;
                    SelectShader(selectTowerB);
                    buildList.Add(selectTowerB);
                }
                else if (selectTowerA != null && selectTowerB != null && hitInfo.transform.CompareTag("Tower"))
                {//둘다 값이 있고 클릭한게 타워일때 
                    buildList.Clear(); //리스트를 지우고
                    CancleShader(selectTowerA); //셰이더 끄고
                    selectTowerA = selectTowerB; //A에 B를 넣고
                    selectTowerB = null;
                    buildList.Add(selectTowerA);//다시 리스트에 넣고

                    selectTowerB = hitInfo.transform.gameObject; //B에 충돌정보를 넣고
                    buildList.Add(selectTowerB);//리스트에 넣고
                    SelectShader(selectTowerB); //셰이더 켜고
                }
                else if (!hitInfo.transform.CompareTag("Tower"))
                {//다른곳 클릭 햇을때
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
        //Outline이 아니라 셰이더 안에 있는 _Outline을 써야됨!!
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
            //생성된 타워가 노드 정보를 가지고 있어야 함.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            towers.Add(obj);
            //selectTowerA가 가지고 있는 노드의 태그를 바꾼다!
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
            //생성된 타워가 노드 정보를 가지고 있어야 함.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA가 가지고 있는 노드의 태그를 바꾼다!
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
            //생성된 타워가 노드 정보를 가지고 있어야 함.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA가 가지고 있는 노드의 태그를 바꾼다!
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
            //생성된 타워가 노드 정보를 가지고 있어야 함.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA가 가지고 있는 노드의 태그를 바꾼다!
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
            //생성된 타워가 노드 정보를 가지고 있어야 함.
            selectTowerA.GetComponent<Tower>().node.tag = "Node";
            //selectTowerA가 가지고 있는 노드의 태그를 바꾼다!
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
