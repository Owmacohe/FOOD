using UnityEngine;

public class Island
{
    public GameObject Object { get; private set; }
    public int FoodRequired { get; private set; }
    public float DeliveryTime { get; private set; }
    public float Reward { get; private set; }

    public Island(GameObject obj, int foodRequired, float deliveryTime)
    {
        Object = obj;
        FoodRequired = foodRequired;
        DeliveryTime = deliveryTime;
    }
}