using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOutline : MonoBehaviour
{
    public float edgeSize = 0.2f;

    public string parentLayer = "characters";
    public string borderLayer = "borders";

    //////Objenin aynısı siyah oluşturulur. Objenin çocuk objesi olarak ayarlanır. Nokta seçildiğinde grup belirginleştirilir.
    public void Outline()
    {
        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer rd in renderers)
        {
            GameObject child = new GameObject("Edge");
            SpriteRenderer crd = child.AddComponent<SpriteRenderer>();
            crd.sprite = rd.sprite;
            crd.color = Color.black;
            crd.sortingLayerName = borderLayer;
            crd.sortingOrder=1;
            rd.sortingLayerName = parentLayer;

            Vector2 edges = Vector2.one;

            float sx = crd.sprite.bounds.size.x;
            float sy = crd.sprite.bounds.size.y;

            float r = 0f;
            if (sy > sx)
            {
                r = sy / sx;
                edges.x += edgeSize * r;
                edges.y += edgeSize;
            }
            else
            {
                r = sx / sy;
                edges.y += edgeSize * r;
                edges.x += edgeSize;
            }

            child.transform.parent = rd.transform;
            child.transform.localScale = edges;
            child.transform.localEulerAngles = Vector2.zero;
            child.transform.localPosition = Vector2.zero;
        }
    }

///// Oluşturulan outlinenın silme fonksiyonu
    public void DeleteOutline()
    {
 //       Debug.Log("silindi");

        SpriteRenderer[] renderers = GetComponentsInChildren<SpriteRenderer>();

        foreach (SpriteRenderer rd in renderers)
        {
            if (rd.name == "Edge")
            {
                Destroy(rd.gameObject);
            }
        }
    }
}
