using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Assets.Scripts;
using TMPro;
using UnityEngine.UI;
public class CharacterManage : MonoBehaviour{
    [SerializeField] GameObject[] PepperOPrefabs;//orange
    [SerializeField] GameObject[] PepperRPrefabs;//red
    [SerializeField] GameObject[] PepperGPrefabs;//green
    [SerializeField] GameObject[] PepperYPrefabs;//yellow
    [SerializeField] GameObject[] PepperSPrefabs;//spicy
    [SerializeField] GameObject[] YangGuPrefabs;//yang gu
    [SerializeField] GameObject[] JinJenGuRPrefabs;//jin jen gu
    [SerializeField] GameObject[] XianGuPrefabs;//xian gu
    [SerializeField] GameObject[] XinBaouGuPrefabs;//xin baou gu
    [SerializeField] GameObject[] XioJenGuPrefabs;//xio jen gu
    [SerializeField] GameObject[] MyWatermelonPrefabs;
    [SerializeField] GameObject[] MyBananaPrefabs;
    [SerializeField] GameObject[] MyPepperSPrefabs;
    [SerializeField] GameObject[] MyXianGuPrefabs;
    [SerializeField] GameObject[] w1coolbar;
    [SerializeField] GameObject[] w2coolbar;
    [SerializeField] GameObject[] w3coolbar;
    [SerializeField] GameObject[] w4coolbar;
    [SerializeField] GameObject[] w5coolbar;
    [SerializeField] TextMeshProUGUI[] wEnergy_text;
    private GameObject[][] EnemyPrefabs;
    private GameObject[][] WarriorPrefabs;
    private GameObject[][] wcoolbar;
    public GameObject[] castles;
    float passtime;
    static public int record;
    static public int CharacterIdInCd=0;
    int GoOnShoot = 0;
    public TextMeshProUGUI Level_Title; //  Level Title
    private ServerMethod.Server ServerScript;
    void Start(){
        ServerScript = FindObjectOfType<ServerMethod.Server>();
        ShowMyCastle(ServerScript.faction[1]);
        passtime=0f;
        record=0;
        EnemyPrefabs = new GameObject[][] {
            PepperOPrefabs,PepperGPrefabs,PepperYPrefabs,PepperRPrefabs,PepperSPrefabs,new GameObject[0],
            YangGuPrefabs,JinJenGuRPrefabs,XianGuPrefabs,XinBaouGuPrefabs,XioJenGuPrefabs,new GameObject[0]
        };
        WarriorPrefabs = new GameObject[][] {MyWatermelonPrefabs,MyBananaPrefabs,MyPepperSPrefabs,MyXianGuPrefabs};
        wcoolbar = new GameObject[][] {w1coolbar,w2coolbar,w3coolbar,w4coolbar,w5coolbar};
        EnemySeq = new int[][] {
            Seq_1_1,Seq_1_2,Seq_1_3,Seq_1_4,Seq_1_5,Seq_1_6,
            Seq_2_1,Seq_2_2,Seq_2_3,Seq_2_4,Seq_2_5,Seq_2_6
        };
        EnemyTime = new int[][] {
            time_1_1,time_1_2,time_1_3,time_1_4,time_1_5,time_1_6,
            time_2_1,time_2_2,time_2_3,time_2_4,time_2_5,time_2_6,
        };
    }
    private bool[] wisUseable=new bool[5]{true,true,true,true,true};
    private float[] wCoolTime=new float[5]{0f,0f,0f,0f,0f};
    private float[] temp=new float[5]{0f,0f,0f,0f,0f};
    private int[] wi=new int[5]{0,0,0,0,0};
    private float[] wCoolTimeUnit_builtin=new float[7]{4.77f,3f,7f,4.77f,5.88f,4f,4.5f};
    private int[] wEnergy_builtin=new int[7]{150,70,250,150,200,150,200};
    private float[] wCoolTimeUnit=new float[5];
    private int[] wEnergy=new int[5];
    private int who;
    private int wholevel;
    void Update()
    {
        for(int i=0;i<5;i++){
            who=ServerScript.lineup[i]-1;
            wholevel=ServerScript.character[who];
            wCoolTimeUnit[i]=wCoolTimeUnit_builtin[who]*(float)Math.Pow(1.15,wholevel-1);
            wEnergy[i]=(int)((double)wEnergy_builtin[who]*Math.Pow(1.4,wholevel-1));
            wEnergy_text[i].text=wEnergy[i].ToString();
        }
        //判斷是否進入CD
        if(CharacterIdInCd!=0){
            warriorProduct(CharacterIdInCd-1);
            CharacterIdInCd=0;
        }
        if(ButtonFunction.GameIsStart){
            passtime+=Time.deltaTime;
            //Debug.Log("CM:"+GameManage.currentLevel);
            if(GameManage.currentLevel!=0)Level(GameManage.currentLevel);
        }
        for(int i=0;i<5;i++){
            if(!wisUseable[i]){
                wCoolTime[i]+=Time.deltaTime;
                temp[i]+=Time.deltaTime;
                if(temp[i]>=wCoolTimeUnit[i]*0.25f){
                    wcoolbar[i][wi[i]].SetActive(false);
                    temp[i]=0f;
                    wi[i]++;
                }
                if(wCoolTime[i]>=wCoolTimeUnit[i]){
                    wCoolTime[i]=0f;
                    wcoolbar[i][3].SetActive(false);
                    wisUseable[i]=true;
                }
            }else wi[i] = 0;
        }
    }
    public void warriorProduct(int index){
        if(wisUseable[index]&&GoOnShoot==1&&CharacterIdInCd==index+1){
            wisUseable[index]=false;
            for(int i=0;i<4;i++)wcoolbar[index][i].SetActive(true);
            CharacterIdInCd=0;
            GoOnShoot=0;
        }else if(ButtonFunction.currentEnergy>=wEnergy[index] && wisUseable[index]){
            Slingshot shot = castles[ServerScript.faction[1]-2].GetComponent<Slingshot>();
            if(shot.Rock!=null){
                Destroy(shot.Rock);
                shot.slingshotState = SlingshotState.do_nothing;
            }
            shot.SetEnergy(wEnergy[index]);
            //ButtonFunction.currentEnergy-=150;
            GameObject Warrior=Instantiate(WarriorPrefabs[ServerScript.faction[1]-2][ServerScript.lineup[index]-1], transform);
            //Watermelon1.transform.position=new Vector3(-7.08f, -1f, 0f);
            shot.CharacterIdInCd_shoot=index+1;
            shot.Rock=Warrior;
            if (shot != null)shot.slingshotState = SlingshotState.Idle;
            GoOnShoot=1;
        }
    }
    int[] Seq_1_1 = { 4,-1,1,2,2,1,5,2,2,2,2,5,1,4,5,1,5,2 };
    int[] time_1_1 = { 6,12,27,35,36,46,58,70,74,78,82,93,103,113,140,155,170,175 };
    int[] Seq_1_2 = { 1, -1, 4, 2, 1, 2, 2, 2, 5, 1, 2, 5, 2, 2, 1, 4, 5 };
    int[] time_1_2 = { 6, 12, 22, 34, 44, 54, 58, 80, 90, 100, 110, 120, 130, 135, 145, 155, 165 };
    int[] Seq_1_3 = { 2, -1, 1, 2, 2, 2, 1, 2, 1, 2, 4, 5, 5, 2, 1, 4, 1 };
    int[] time_1_3 = { 6, 12, 22, 30, 35, 45, 58, 80, 90, 100, 110, 120, 130, 135, 148, 161, 162 };
    int[] Seq_1_4 = { 5,-1,4,1,2,5,2,1,2,2,5,4,2,2,4,1,5,5,2 };
    int[] time_1_4 = { 7,15,25,35,45,47,61,71,75,79,91,101,111,115,125,135,157,172,177 };
    int[] Seq_1_5 = { 5, -1, 4, 5, 4, 5, 1, 2, 2, 5, 4, 3, 1, -1, 5, 2, 1, 4, 4 };
    int[] time_1_5 = { 6, 12, 22, 34, 40, 54, 68, 76, 90, 100, 110, 123, 130, 140, 144, 151, 161, 172 };
    int[] Seq_1_6 = { 5, -1, 4, 1, 2, 1, 4, 1, 2, 3, 2, 1, 5, 2, 4, -1, 3, 2, 4, 4, 2 };
    int[] time_1_6 = { 6, 12, 25, 35, 38, 46, 58, 70, 74, 78, 82, 92, 102, 108, 113, 130, 140, 145, 155, 170, 175 };
    int[] Seq_2_1 = {2, 2, 2, 1, 4, 5, 2, -1, 2, 4, 5, 2, 4, 2, 2, 5, 2};
    int[] time_2_1 = {1, 5, 15, 17, 37, 57, 62, 82, 87, 97, 117, 127, 137, 142, 149, 169, 176};
    int[] Seq_2_2 = {4, 2, 2, 2, -1, 2, 1, 5, 4, 2, 2, 2, 1, 5, 4, 2, 2};
    int[] time_2_2 = {6, 12, 22, 32, 52, 57, 67, 87, 97, 112, 118, 124, 134, 154, 164, 170, 175};
    int[] Seq_2_3 = {5, 2, 1, 4, 4, -1, 5, 2, 2, 2, 2, 5, 4, 1, 2};
    int[] time_2_3 = {6, 11, 21, 36, 46, 66, 86, 101, 106, 111, 116, 136, 152, 160, 170};
    int[] Seq_2_4 = {4, -1, 5, 5, 4, 4, 2, -1, 1, 5, 4, -1, 5, 3, 3, 4, 2};
    int[] time_2_4 = {6, 12, 22, 30, 35, 45, 58, 60, 70, 80, 90, 105, 110, 115, 120, 130, 140};
    int[] Seq_2_5 = {4, -1, 4, 5, 5, 4, 1, -1, 3, 5, 3, -1, 4, 4, 1, 3, 3};
    int[] time_2_5 = {6, 12, 22, 30, 35, 45, 58, 60, 80, 90, 110, 120, 130, 135, 145, 161, 171};
    int[] Seq_2_6 = {4, -1, 4, 3, 5, 4, 1, -1, 3, 5, 3, -1, 5, 4, 4, 3, 4, 4};
    int[] time_2_6 = {6, 12, 22, 30, 35, 45, 58, 60, 80, 90, 110, 120, 130, 135, 140, 150, 160, 170};
    private int[][] EnemySeq;
    private int[][] EnemyTime;
    void Level(int index){
        Level_Title.text = (index/10).ToString() + "-" + (index%10).ToString().ToString();
        index=(index/10-1)*6+index%10-1;
        if(passtime>=EnemyTime[index][record]){
            if(EnemySeq[index][record]!=(-1)){
                int PrefabIndex;
                if(index%6==5)PrefabIndex=index/6*6+record%5;else PrefabIndex=index;
                GameObject enemies=Instantiate(EnemyPrefabs[PrefabIndex][EnemySeq[index][record]-1], transform);
                enemies.transform.position=new Vector3(15.0f, 0.0f, 0f);
                enemies.transform.rotation = Quaternion.Euler(0, 180f, 0);
            }
            record++;
            if(record==EnemyTime[index].Length-1){
                passtime=0;
                record=0;
            }
        }
    }
    void ShowMyCastle(int tower)
    {
        castles[tower-2].SetActive(true);
    }
}