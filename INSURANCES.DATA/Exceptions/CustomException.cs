using INSURANCES.DATA.Enum;

namespace INSURANCES.DATA.Exceptions
{
    public class CustomException : Exception
    {
        public  CustomException()
        {
            ExceptionMessage = string.Empty;
            ExceptionReason = string.Empty;
            ExceptionFix = string.Empty;
        }
        public CustomException(string message) : base(message)
        {
            ExceptionMessage = message;
            ExceptionReason = string.Empty;
            ExceptionFix = string.Empty;
        }
        public CustomException(string message, ExceptionTypeEnum type) : base(message)
        {
            Type = type;
            ExceptionMessage = message;
            ExceptionReason = string.Empty;
            ExceptionFix = string.Empty;
        }
        public CustomException(string message, Exception ex) : base(message, ex)
        {
            ExceptionMessage = message;
            ExceptionReason = string.Empty;
            ExceptionFix = string.Empty;
        }
        public CustomException(string message, Exception ex, ExceptionTypeEnum type) : base(message, ex)
        {
            Type = type;
            ExceptionMessage = message;
            ExceptionReason = string.Empty;
            ExceptionFix = string.Empty;
        }
        public virtual ExceptionTypeEnum Type { get; set; }
        public virtual string ExceptionMessage { get; set; }
        public virtual string ExceptionReason { get; set; }
        public virtual string ExceptionFix { get; set; }
    }
}
