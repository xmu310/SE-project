using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour{
    //ButtonFunction buttonFunction = new ButtonFunction(); 
    //public ButtonFunction buttonFunction;   
    public int maxHealth = 100;
    public int currentHealth=100;
    [SerializeField] GameObject HpBar;

    private void Start(){
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damage){
        int currentFrame = Time.frameCount;
        Debug.Log("Current Frame: " + currentFrame);
        Debug.Log(gameObject+" -50");
        if(gameObject.layer==6||gameObject.layer==8){
            currentHealth =(currentHealth-damage<=maxHealth) ? currentHealth-damage : maxHealth ;
            HpBar.GetComponent<Image>().fillAmount = (float)currentHealth/maxHealth;
        }
        else{
            currentHealth =(currentHealth-damage<=maxHealth) ? currentHealth-damage : maxHealth ;
        }
        if (currentHealth <= 0){
            Debug.Log(gameObject);
            StartCoroutine(Die());
        }
    }
private IEnumerator Die() {
        // Implement death logic here, such as playing death animation or removing the object

        float disappearDuration = 0.5f;     //消失的時間
        float timer = 0f;
        if(gameObject.layer==6)
        {
            //主塔死亡並觸發動畫
            GetComponent<Animator>().SetTrigger("crash");
            yield return new WaitForSeconds(1.5f);
            ButtonFunction.judge_defeat=1;
            //Time.timeScale=0f;
        }
        else if(gameObject.layer==8)
        {
            //主塔死亡並觸發動畫
            Debug.Log("敵方主堡爆掉");
            GetComponent<Animator>().SetTrigger("crash");
            yield return new WaitForSeconds(1.5f);
            ButtonFunction.judge_victory=1;
        }
        else
        {   
            yield return new WaitForEndOfFrame();// WaitForEndOfFrame();
            gameObject.tag = "Untagged";
            while (timer < disappearDuration) 
            {
                Color currentColor = GetComponent<SpriteRenderer>().color;              // 變透明
                currentColor.a = Mathf.Lerp(1f, 0f, timer / disappearDuration);
                GetComponent<SpriteRenderer>().color = currentColor;
                timer += Time.deltaTime;
                yield return null;
            }
            Destroy(gameObject);
        }
    }
    
}
