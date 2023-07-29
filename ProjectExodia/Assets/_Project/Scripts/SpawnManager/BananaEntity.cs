namespace ProjectExodia
{
    public class BananaEntity : EntityBase
    {
        public override void PerformCollision()
        {
            Destroy(gameObject);
        }
    }
}