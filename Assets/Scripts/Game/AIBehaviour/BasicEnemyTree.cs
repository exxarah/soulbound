using Game.AIBehaviour.Tasks;

namespace Game.AIBehaviour
{
    public class BasicEnemyTree : ABehaviourTree
    {
        protected override Node SetupTree()
        {
            Node root = new TaskIdle(this);
            return root;
        }
    }
}