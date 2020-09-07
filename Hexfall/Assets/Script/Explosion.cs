using UnityEngine;

public class Explosion : MonoBehaviour {
    float destroyTime;
    private void OnEnable() {
        destroyTime=0f;
    }
    //// patlama efekti havuzda kullanmak için 2 saniye sonra kapatılır.
    private void Update() {
        destroyTime+=Time.deltaTime;
        if(destroyTime>2f){
            gameObject.SetActive(false);
        }
    }
}