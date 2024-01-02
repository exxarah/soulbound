using Core.Unity.Flow;

namespace Core.Unity
{
    public static class ExecutionOrder
    {
        public const int MIN = int.MinValue;
        
        public const int FlowManager = DEFAULT - 1;
        
        public const int DEFAULT = 0;

        public const int MAX = int.MaxValue;
    }
}