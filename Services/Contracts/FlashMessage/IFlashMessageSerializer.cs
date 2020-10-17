using System.Collections.Generic;

namespace TravianAnalytics.Services.Contracts.FlashMessage {
    public interface IFlashMessageSerializer {
        List<IFlashMessageModel> Deserialize(string data);
        string Serialize(IList<IFlashMessageModel> messages);
    }
}