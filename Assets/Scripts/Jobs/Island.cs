using UnityEngine;

public class Island
{
    public Vector3 Position { get; private set; }
    public int FoodRequired { get; private set; } // TODO: presently, this is unused
    public float DeliveryTime { get; private set; }
    public float Reward { get; private set; }

    public Island(Vector3 position, int foodRequired, float deliveryTime)
    {
        Position = position;
        FoodRequired = foodRequired;
        DeliveryTime = deliveryTime;
    }
}