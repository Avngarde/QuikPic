using Microsoft.CodeAnalysis;

namespace QuikPic.Web.Errors
{
    public enum ErrorType
    {
        FileNotFound,
        FileIsNotAnImage,
        AddPresetError,
        EditPresetError,
        DownloadError
    }

    public class Error
    {
        public string? ErrorMessage { get; set; }
        public ErrorType ErrorType { get; set; }
    }
}
