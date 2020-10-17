using TravianAnalytics.Services.Contracts.FlashMessage;

namespace TravianAnalytics.Services.Components.FlashMessage {
    public class FlashMessageModel : IFlashMessageModel {
        public bool IsHtml { get; set; }

        public string Title { get; set; }

        public string Message { get; set; }

        public FlashMessageType Type { get; set; }

        public FlashMessageModel() {
            IsHtml = false;
            Type = FlashMessageType.Success;
        }
    }
}