using System.Collections.Generic;

namespace TravianAnalytics.Services.Contracts.FlashMessage {
    public interface IFlashMessage {
        public List<IFlashMessageModel> Peek();
        public List<IFlashMessageModel> Retrieve();
        public void Success(string message, string title = null, bool isHtml = false);
        public void Danger(string message, string title = null, bool isHtml = false);
        public void Info(string message, string title = null, bool isHtml = false);
        public void Warning(string message, string title = null, bool isHtml = false);
    }
}