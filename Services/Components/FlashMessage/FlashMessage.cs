using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using TravianAnalytics.Services.Contracts.FlashMessage;

namespace TravianAnalytics.Services.Components.FlashMessage {
    public class FlashMessage : IFlashMessage {
        private ITempDataDictionary _tempData;
        private ITempDataDictionaryFactory _tempDataFactory;
        private IHttpContextAccessor _httpContextAccessor;
        private IFlashMessageSerializer _messageSerializer;
        public static string KeyName { get; set; } = "_FlashMessage";

        public ITempDataDictionary TempData {
            protected set => _tempData = value;
            get { return _tempData ??= _tempDataFactory.GetTempData(_httpContextAccessor.HttpContext); }
        }

        public FlashMessage(ITempDataDictionaryFactory tempDataFactory, IHttpContextAccessor httpContextAccessor,
            IFlashMessageSerializer messageSerializer) {
            _tempDataFactory = tempDataFactory;
            _httpContextAccessor = httpContextAccessor;
            _messageSerializer = messageSerializer;
        }

        public void Queue(IFlashMessageModel message) {
            var flashMessageModelList = Peek();
            flashMessageModelList.Add(message);
            Store(flashMessageModelList);
        }

        private void Store(IList<IFlashMessageModel> messages) {
            TempData[KeyName] = _messageSerializer.Serialize(messages);
        }

        public List<IFlashMessageModel> Retrieve() {
            var obj = TempData[KeyName];
            if (obj == null) return new List<IFlashMessageModel>();
            TempData.Remove(KeyName);
            return _messageSerializer.Deserialize((string)obj);
        }

        public List<IFlashMessageModel> Peek() {
            var obj = TempData.Peek(KeyName);
            return obj == null ? new List<IFlashMessageModel>() : _messageSerializer.Deserialize((string)obj);
        }

        public void Success(string message, string title = null, bool isHtml = false) {
            Queue(new FlashMessageModel() {
                Message = message,
                Title = title,
                IsHtml = isHtml,
                Type = FlashMessageType.Success
            });
        }

        public void Danger(string message, string title = null, bool isHtml = false) {
            Queue(new FlashMessageModel() {
                Message = message,
                Title = title,
                IsHtml = isHtml,
                Type = FlashMessageType.Danger
            });
        }

        public void Info(string message, string title = null, bool isHtml = false) {
            Queue(new FlashMessageModel() {
                Message = message,
                Title = title,
                IsHtml = isHtml,
                Type = FlashMessageType.Info
            });
        }

        public void Warning(string message, string title = null, bool isHtml = false) {
            Queue(new FlashMessageModel() {
                Message = message,
                Title = title,
                IsHtml = isHtml,
                Type = FlashMessageType.Warning
            });
        }
    }
}