using UnityEngine;
using System.Collections.Generic;
public class ObjectPool : MonoBehaviour
{
    public ObjectPool(GameObject obje)
    {
        this.obje = obje;
        this.list = new List<GameObject>();
    }


    [SerializeField]
    List<GameObject> list;
    GameObject obje;
    private bool isNotEnough = true;

///////Obje havuzunun obje ürettiği ve tekrardan kullandığı fonksiyon
    public GameObject GetObje()
    {

        if (list.Count > 0)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (!list[i].activeInHierarchy)
                {
                    return list[i];
                }
            }
        }
        if (isNotEnough)
        {
            GameObject prefab = Instantiate(obje);
            prefab.SetActive(false);
            list.Add(prefab);
            return prefab;

        }

        return null;
    }


}