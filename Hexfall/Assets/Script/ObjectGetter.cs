using UnityEngine;

public class ObjectGetter : MonoBehaviour
{
    public static ObjectPool hexagon, explosion;

    ////////// Dinamik obje havuzunda üretilecek objeler seçilir.
    private void Awake()
    {
        hexagon = new ObjectPool(Resources.Load("Hexagon") as GameObject);
        explosion = new ObjectPool(Resources.Load("Explosion") as GameObject);
    }
}