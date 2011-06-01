
namespace Mindplex.Core.Event.Threads
{
    /// <summary>
    /// 
    /// </summary>
    /// 
    public interface ILifeCycle
    {
        /// <summary>
        /// 
        /// </summary>
        /// 
        void Start();

        /// <summary>
        /// 
        /// </summary>
        /// 
        void Stop();

        /// <summary>
        /// 
        /// </summary>
        /// 
        bool Active { get; }
    }
}
