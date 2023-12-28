namespace PracticeJam.Game.Combat
{
    public interface IDamageable
    {
        public void DoDamage(DamageParams @params);
        public bool CanBeDamaged();
    }

    public struct DamageParams
    {
        public int DamageAmount;
    }
}