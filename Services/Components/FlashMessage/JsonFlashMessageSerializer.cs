using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using TravianAnalytics.Services.Contracts.FlashMessage;

namespace TravianAnalytics.Services.Components.FlashMessage {
    public class JsonFlashMessageSerializer : IFlashMessageSerializer {

        public List<IFlashMessageModel> Deserialize(string data) {
            return JsonSerializer.Deserialize<List<FlashMessageModel>>(data).Cast<IFlashMessageModel>().ToList();
        }

        public string Serialize(IList<IFlashMessageModel> messages) {
            return JsonSerializer.Serialize(messages);
        }
    }
}