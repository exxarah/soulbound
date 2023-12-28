using Cysharp.Threading.Tasks;

namespace Core.Unity.Online
{
    public class RequestManager : Singleton<RequestManager>
    {
        private ServerConfig m_config = null;

        private RequestManager(){}

        public async UniTask<Response> SendRequest(Request request)
        {
            await Internal_SendRequest(request);

            Response response = new Response();
            request.OnCompleted?.Invoke(request, response);

            return response;
        }

        public async UniTask<Response<T>> SendRequest<T>(Request request)
        {
            await Internal_SendRequest(request);

            Response<T> response = new Response<T>();
            request.OnCompleted?.Invoke(request, response);

            return response;
        }

        private async UniTask Internal_SendRequest(Request request)
        {
            // TODO: Actually do this
            await UniTask.Yield();

            request.Successful = true;
        }

        public void Initialise(ServerConfig config)
        {
            m_config = config;
        }
    }
}