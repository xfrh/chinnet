using System;
using System.Runtime.Serialization;

namespace ManageSystem.Core
{
    /// <summary>
    /// 代表应用程序执行期间发生的错误
    /// </summary>
    [Serializable]
    public class ManageSystemException : Exception
    {
        /// <summary>
        /// 初始化异常类的一个新实例。
        /// </summary>
        public ManageSystemException()
        {
        }

        /// <summary>
        ///初始化异常类的一个新实例指定的错误消息。
        /// </summary>
        /// <param name="message">描述了错误的消息。</param>
        public ManageSystemException(string message)
            : base(message)
        {
        }

        /// <summary>
        ///初始化异常类的一个新实例指定的错误消息。
        /// </summary>
		/// <param name="messageFormat">异常消息格式。</param>
		/// <param name="args">异常消息参数.</param>
        public ManageSystemException(string messageFormat, params object[] args)
			: base(string.Format(messageFormat, args))
		{
		}

        /// <summary>
        /// 初始化一个新的实例序列化数据的异常类。
        /// </summary>
        /// <param name="info">的SerializationInfo序列化对象数据的异常被抛出。</param>
        /// <param name="context">包含上下文的StreamingContext源或目标的信息。</param>
        protected ManageSystemException(SerializationInfo
            info, StreamingContext context)
            : base(info, context)
        {
        }

        /// <summary>
        /// 异常类的初始化一个新的实例指定的错误消息和引用内部异常,这种异常的原因。
        /// </summary>
        /// <param name="message">错误消息解释异常的原因。</param>
        /// <param name="innerException">除了当前异常的原因,或一个空引用如果没有指定内部异常。</param>
        public ManageSystemException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
