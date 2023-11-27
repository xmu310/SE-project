using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using System;

namespace ServerMethod{
    public class data{
        public bool success;
        public string token;
        public int money;
        public int[] exp;
        public int[] character;
        public int[] lineup;
        public int tear;
        public int castleLevel;
        public int slingshotLevel;
        public int[] clearance;
        public int energy;
        public string updateTime;
        public int volume;
        public int backVolume;
        public bool shock;
        public bool remind;
        public string chestTime;
        public int faction;
        public int[] props;
    }
    public class chestReturn{
        public bool success;
        public int result;
        public int situation;
        public bool get;
        public int character;
    }
    public class Rank{
        public List<string> data;
    }
    public class Server : MonoBehaviour
    {
        public bool success;
        public string token;
        public int money;
        public int[] exp;
        public int[] character;
        public int[] lineup;
        public int tear;
        public int castleLevel;
        public int slingshotLevel;
        public int[] clearance;
        public int energy;
        public string updateTime;
        public int volume;
        public int backVolume;
        public bool shock;
        public bool remind;
        public string chestTime;
        public int faction;
        public int[] props;

        public List<string> rankName = new List<string>();
        public List<string> rankClear = new List<string>();

        void Awake(){
            token = TokenManager.Instance.Token;
            CallUpdate();
        }
        private void Start() {
            StartCoroutine(autoUpdate());
        }
        public void CallUpdate() {
            StartCoroutine(updateData());
        }
        public IEnumerator updateData(){

            WWWForm form = new WWWForm();
            form.AddField("token", token);

            UnityWebRequest www = UnityWebRequest.Post("https://pc167.csie.ntnu.edu.tw/updateData", form);
            
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success){
                string response = www.downloadHandler.text;
                
                data playerData = JsonUtility.FromJson<data>(response);

                success = playerData.success;
                money = playerData.money;
                exp = playerData.exp;
                character = playerData.character;
                lineup = playerData.lineup;
                tear = playerData.tear;
                castleLevel = playerData.castleLevel;
                slingshotLevel = playerData.slingshotLevel;
                clearance = playerData.clearance;
                energy = playerData.energy;
                updateTime = playerData.updateTime;
                volume = playerData.volume;
                backVolume = playerData.backVolume;
                shock = playerData.shock;
                remind = playerData.remind;
                chestTime = playerData.chestTime;
                faction = playerData.faction;
                props = playerData.props;

            }
            else{ //非法token需跳回登入頁面
                SceneManager.LoadScene("MainMenu");
                Debug.Log("未連接至伺服器");
            }

            if(success == false){
                SceneManager.LoadScene("MainMenu");
                Debug.Log("Error2");
            }
            StartCoroutine(updateRank(1));
            StartCoroutine(updateRank(2));

        }     
        public IEnumerator autoUpdate(){
            while (true){
                yield return new WaitForSecondsRealtime(300f);
                yield return StartCoroutine(updateData());
            }
        }
        public IEnumerator openChest(bool openType) {
            WWWForm form = new WWWForm();

            form.AddField("token", token);
            if(openType)
                form.AddField("openType", "True");
            else
                form.AddField("openType", "False");

            UnityWebRequest www = UnityWebRequest.Post("https://pc167.csie.ntnu.edu.tw/openChest", form);
            
            yield return www.SendWebRequest();

            chestReturn result = new chestReturn();


            if(www.result == UnityWebRequest.Result.Success){
                string response = www.downloadHandler.text;
                result = JsonUtility.FromJson<chestReturn>(response);
                CallUpdate();
            }
            else{
                result.success = false;
            }

            yield return result;

        }
        public IEnumerator beforeGame(){
            WWWForm form = new WWWForm();
            form.AddField("token", token);

            UnityWebRequest www = UnityWebRequest.Post("https://pc167.csie.ntnu.edu.tw/beforeGame", form);
            
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success){
                string response = www.downloadHandler.text;
                Debug.Log(response);
                CallUpdate();
            }
            

        }
        public IEnumerator afterGame(bool clear, string target){
            WWWForm form = new WWWForm();
            form.AddField("token", token);
            form.AddField("target", target);

            if(clear)
                form.AddField("clear", "True");
            else
                form.AddField("clear", "False");
            

            UnityWebRequest www = UnityWebRequest.Post("https://pc167.csie.ntnu.edu.tw/afterGame", form);
            
            yield return www.SendWebRequest();

            if(www.result == UnityWebRequest.Result.Success){
                string response = www.downloadHandler.text;
                Debug.Log(response);
                CallUpdate();
            }

        }
        public IEnumerator updateCard(int target, int mode){
            WWWForm form = new WWWForm();
            form.AddField("token", token);
            form.AddField("target", target);
            form.AddField("mode", mode);

            UnityWebRequest www = UnityWebRequest.Post("https://pc167.csie.ntnu.edu.tw/updateCard", form);
            
            yield return www.SendWebRequest();

            bool success = new bool();

            if(www.result == UnityWebRequest.Result.Success){
                CallUpdate();
                success = true;
            }
            else
                success = false;

            yield return success;
            
        }
        public IEnumerator updateLineup(int[] lineup){
            WWWForm form = new WWWForm();
            string lineupString = string.Join(",", lineup);
            form.AddField("lineup", lineupString);
            form.AddField("token", token);

            UnityWebRequest www = UnityWebRequest.Post("https://pc167.csie.ntnu.edu.tw/updateLineup", form);
            
            yield return www.SendWebRequest();

            bool success = new bool();

            if(www.result == UnityWebRequest.Result.Success){
                CallUpdate();
                success = true;
            }
            else
                success = false;

            yield return success;
            
        }
        
        public IEnumerator updateRank(int mode){
            WWWForm form = new WWWForm();
            form.AddField("mode", mode);

            UnityWebRequest www = UnityWebRequest.Post("https://pc167.csie.ntnu.edu.tw/updateRank", form);
            
            yield return www.SendWebRequest();


            if(www.result == UnityWebRequest.Result.Success){
                string response = www.downloadHandler.text;

                Rank rankReturn = JsonUtility.FromJson<Rank>(response);

                foreach(string data in rankReturn.data){
                    if(mode == 1 ){
                        if(!rankName.Contains(data))
                            rankName.Add(data);
                    }
                    else{
                        if(!rankClear.Contains(data))
                            rankClear.Add(data);
                    }
                }
            }
            
            
        }
    }
}