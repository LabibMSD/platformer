using UnityEngine;

public class MobilePlatformCheck : MonoBehaviour
{
    void Start()
    {
        // Default matiin dulu semua anak-anaknya (tombol-pembol)
        // Agar pas di Editor/PC tidak menghalangi pemandangan
        // Kecuali kalau kita lagi testing mode mobile di Editor Unity
#if UNITY_ANDROID || UNITY_IOS
        gameObject.SetActive(true); // Hidup di HP
#else
        // Opsional: Hidupkan ini jika ingin ngetest di Unity Editor seolah-olah HP
        // gameObject.SetActive(true); 
        
        gameObject.SetActive(false); // Mati di PC / EXE
#endif
    }
}
